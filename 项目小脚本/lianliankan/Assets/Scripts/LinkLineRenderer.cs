using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI 是 RectTransform LineRenderer 只认世界坐标 所以需要转换
/// </summary>
[RequireComponent(typeof(LineRenderer))] // 防止忘了手动添加 LineRenderer 组件
public class LinkLineRenderer : MonoBehaviour
{
    [Header("Line Settings")]
    public LineRenderer line;           // LineRenderer 组件，留空则自动取自身
    public Camera targetCamera;         // 用于屏幕坐标转世界坐标，留空自动取 main camera
    public float zOffset = -0.1f;       // 提前/压后一点，避免与 UI Z 冲突
    public bool debugLog = true;       // 是否打印调试日志（默认关，需时可开）

    private float uiPlaneDistance = 10f; // 相机到 UI 平面的深度估计，用于 ScreenToWorldPoint

    void Awake()
    {
        if (line == null)
        {
            line = GetComponent<LineRenderer>();
        }
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
        if (targetCamera != null)
        {
            // 对于 2D/Overlay，UI 一般在 z=0，摄像机在 z<0，看向 +z，深度取摄像机到 z=0 的距离
            uiPlaneDistance = Mathf.Abs(targetCamera.transform.position.z);
        }
        if (line != null)
        {
            line.useWorldSpace = true;  // 世界坐标绘制，便于直接使用 RectTransform 的世界位置
            line.positionCount = 0; // 保证初始时没有线
        }
    }

    /// <summary>
    /// 绘制一条路径；path 为“外扩一圈”坐标，tiles 为棋盘。
    /// </summary>
    public void ShowPath(List<Vector2Int> path, Tile[,] tiles)
    {
        Clear(); // 先清空旧线条 保证视觉干净
        if (line == null)
        {
            if (debugLog) Debug.LogWarning("[LineRenderer] line 组件为空");
            return;
        }
        else if (!line.gameObject.scene.IsValid())
        {
            if (debugLog) Debug.LogWarning("[LineRenderer] line 存在但不在加载的场景中 (可能指向的是Prefab资产，请拖拽场景实例)");
            return;
        }
        if (targetCamera == null)
        {
            if (debugLog) Debug.LogWarning("[LineRenderer] targetCamera 为空");
            return;
        }
        if (tiles == null)
        {
            if (debugLog) Debug.LogWarning("[LineRenderer] tiles 二维数组为空");
            return;
        }
        if (path == null || path.Count < 2)
        {
            if (debugLog) Debug.LogWarning($"[LineRenderer] 路径无效 (数量={(path == null ? -1 : path.Count)})");
            return;
        }

        if (!TryGetCellSizePixels(tiles, out Vector2 cellSizePixels)) // 取格子像素尺寸
        {
            if (debugLog) Debug.LogWarning("[LineRenderer] 获取格子像素尺寸失败");
            return;
        }

        // 先计算屏幕坐标，后转世界坐标
        var screenPts = new List<Vector2>(path.Count); // 储存要绘制路径的所有屏幕坐标点
        for (int i = 0; i < path.Count; i++)
        {
            screenPts.Add(GridToScreen(path[i], tiles, cellSizePixels)); // 把逻辑路径点转换为屏幕坐标点
        }

        var positions = new List<Vector3>(screenPts.Count); // 储存最终的世界坐标点
        for (int i = 0; i < screenPts.Count; i++) 
        {
            var world = ScreenToWorld(screenPts[i]); // 屏幕坐标转世界坐标
            world.z += zOffset; // 应用 Z 偏移，避免与 UI 冲突
            positions.Add(world); // 加入世界坐标列表
        }

        line.positionCount = positions.Count;// 设置绘制点数量
        line.SetPositions(positions.ToArray()); // 设置绘制点位置  要用ToArray， Unity 的 LineRenderer 组件的 SetPositions 方法只接受数组 (Vector3[]) 作为参数，而不接受列表 (List<Vector3>)

        if (debugLog)
        {
            Debug.Log($"[LineRenderer] 显示路径 节点数={path.Count} 位置点数={positions.Count} z偏移={zOffset} 起点={positions[0]} 终点={positions[positions.Count - 1]}");
        }
    }

    /// <summary>
    /// 清空折线。
    /// </summary>
    public void Clear()
    {
        if (line == null) return;
        line.positionCount = 0;
    }

    /// <summary>
    /// 给一个路径点 p（可能在棋盘内，也可能在外面），算出一个合理的屏幕坐标。
    /// </summary>
    /// <param name="p">路径点坐标，可能在棋盘内或外扩一圈</param>
    /// <param name="tiles">棋盘的二维数组</param>
    /// <param name="cellSizePixels">每个格子的像素尺寸，考虑了缩放</param>
    /// <returns></returns>
    private Vector2 GridToScreen(Vector2Int p, Tile[,] tiles, Vector2 cellSizePixels)
    {
        // 获取棋盘的总行数和总列数
        int row = tiles.GetLength(0);
        int col = tiles.GetLength(1);

        // ------------------------------------------
        // 情况 1：如果点 p 在真实的棋盘范围内
        // path 里存的是 外扩坐标（比如 1,1），但你想在屏幕上找到那个格子的位置，得去 Tile[,] 数组里查。去数组查的时候，必须把坐标减 1（变回 0,0）
        // ------------------------------------------
        if (p.x >= 1 && p.x <= row && p.y >= 1 && p.y <= col)
        {
            // 拿到对应的真实格子 Tile 对象（索引要 -1，因为 p 是 1-based，数组是 0-based）
            Tile t = tiles[p.x - 1, p.y - 1];
            
            // 如果格子存在（不是空的）
            if (t != null)
            {
                // 拿到它的 RectTransform（UI 组件的位置信息）
                // 这是一个兼容性写法：如果有 image 就拿 image 的 rect，没有就拿 transform 的 rect
                var rt = t.image != null ? t.image.rectTransform : t.transform as RectTransform;
                
                // 核心 API：把 UI 物体的世界坐标 (World Position) 转换成屏幕坐标 (Screen Point)
                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, rt.position);
                
                // 直接返回这个屏幕坐标
                return screenPos;
            }
        }

        // ------------------------------------------
        // 情况 2：如果点 p 在棋盘外面（外扩的那一圈虚拟格子）
        // 这时候没有真实的 Tile 对象，我们需要“推算”它的位置
        // ------------------------------------------

        // 找一个“最近的参考格子”。
        // 比如当前点在棋盘左边外面 (0, 5)，我们就找它右边紧挨着的真实的格子 (0, 5) -> 对应数组索引 (0, 4) 来做参照物。
        // Clamp 的作用就是把越界的坐标“硬拉”回边界上。
        int cx = Mathf.Clamp(p.x - 1, 0, row - 1);
        int cy = Mathf.Clamp(p.y - 1, 0, col - 1);
        
        // 拿到这个参考格子
        Tile refTile = tiles[cx, cy];
        
        // 如果参考格子都没了（极端异常），就返回 (0,0)，没法算了
        if (refTile == null)
        {
            return Vector3.zero;
        }

        // 拿到参考格子的 RectTransform
        var refRT = refTile.image != null ? refTile.image.rectTransform : refTile.transform as RectTransform;
        
        // 算出参考格子在屏幕上的位置作为“基准点”
        Vector2 baseScreen = RectTransformUtility.WorldToScreenPoint(null, refRT.position);

        // 算偏移量。我们要根据当前点 p 相对于参考格子的方位，往外挪半个或一个格子的距离。
        Vector2 offset = Vector2.zero;

        // 如果 p 在棋盘上方外面 (行号为0)，我们就往上挪（y坐标变大）
        if (p.x == 0) offset.y += cellSizePixels.y;
        // 如果 p 在棋盘下方外面 (行号为row+1)，我们就往下挪（y坐标变小，这里用减法）
        else if (p.x == row + 1) offset.y -= cellSizePixels.y;
        
        // 如果 p 在棋盘左边外面 (列号为0)，我们就往左挪
        if (p.y == 0) offset.x -= cellSizePixels.x;
        // 如果 p 在棋盘右边外面 (列号为col+1)，我们就往右挪
        else if (p.y == col + 1) offset.x += cellSizePixels.x;

        // 最终位置 = 基准点位置 + 偏移量
        Vector2 targetScreen = baseScreen + offset;
        
        return targetScreen;
    }

    // 取一个格子的屏幕像素尺寸（考虑缩放）
    // 函数定义：输入是棋盘数组，输出是算出来的格子尺寸 cellSize（通过 out 参数返回）
    // 返回值是 bool，表示“是否成功量到了尺寸”
    private bool TryGetCellSizePixels(Tile[,] tiles, out Vector2 cellSize)
    {
        // 获取棋盘总行数和总列数
        int row = tiles.GetLength(0);
        int col = tiles.GetLength(1);

        // 双重循环遍历整个棋盘，目的就是为了“随便抓一个活着的格子来测量”
        // 因为这只是个 UI 布局，理论上每个格子大小都是一样的，所以我测谁都行，找到第一个我就收工。
        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < col; y++)
            {
                // 拿到当前遍历到的格子对象
                var t = tiles[x, y];
                
                // 只有当格子存在（不是空的），并且它身上有 image 组件（没有图片我就没法量尺寸），我才测它
                if (t != null && t.image != null)
                {
                    // 拿到它的 RectTransform（控制 UI 大小位置的组件）
                    var rt = t.image.rectTransform;
                    
                    // rt.rect.size 是它在 **自身局部坐标系** 下的尺寸（比如你预制体里设的宽高是 100x100）
                    var size = rt.rect.size;
                    
                    // rt.lossyScale 是它在 **全局世界坐标系** 下的缩放比例（考虑了父物体的缩放）
                    // 比如你在 Canvas 上把整个面板缩放了 0.5 倍，那这个值就是 (0.5, 0.5, 0.5)
                    // 真正的屏幕像素尺寸 = 原始尺寸 * 全局缩放比例
                    // 这一步非常关键！如果不乘 scale，当 UI 有缩放时，画出来的线就会错位。
                    var scale = rt.lossyScale;
                    cellSize = new Vector2(size.x * scale.x, size.y * scale.y);
                    
                    // 打印个日志，告诉我你量到了多少
                    if (debugLog)
                    {
                        Debug.Log($"[LineRenderer] 格子像素尺寸={cellSize} 原始尺寸={size} 缩放={scale}");
                    }
                    
                    // 测量成功，返回 true，并退出整个函数（不需要再测后面的格子了）
                    return true;
                }
            }
        }
        
        // 如果循环跑完了都没找到一个合法的格子（比如棋盘被消光了，或者虽然有格子但都没 Image 组件）
        // 那就给一个默认值 (1,1)，并返回 false 告诉调用者“我尽力了但没测到”
        cellSize = Vector2.one;
        return false;
    }

    // 将屏幕坐标转换为世界坐标（基于目标相机）
    private Vector3 ScreenToWorld(Vector2 screenPos)
    {
        return targetCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, uiPlaneDistance));
    }
}
