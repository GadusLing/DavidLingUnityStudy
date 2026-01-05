using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 基于 LineRenderer 的连线渲染：给定路径点（扩展坐标系），在世界空间绘制无缝折线。
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class LinkLineRenderer : MonoBehaviour
{
    [Header("Line Settings")]
    public LineRenderer line;           // LineRenderer 组件，留空则自动取自身
    public Camera targetCamera;         // 用于屏幕坐标转世界坐标，留空自动取 main camera
    public float zOffset = -0.1f;       // 提前/压后一点，避免与 UI Z 冲突
    public bool debugLog = false;       // 是否打印调试日志（默认关，需时可开）
    [Range(0.1f, 1.0f)]
    public float endExtendFactor = 0.5f; // 首尾延长系数（按格子尺寸比例）

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
            line.positionCount = 0;
        }
    }

    /// <summary>
    /// 绘制一条路径；path 为“外扩一圈”坐标，tiles 为棋盘。
    /// </summary>
    public void ShowPath(List<Vector2Int> path, Tile[,] tiles)
    {
        Clear();
        if (line == null)
        {
            if (debugLog) Debug.LogWarning("[LineRenderer] line is null");
            return;
        }
        else if (!line.gameObject.scene.IsValid())
        {
            if (debugLog) Debug.LogWarning("[LineRenderer] line exists but is not in a loaded scene (可能指向的是Prefab资产，请拖场景实例)");
            return;
        }
        if (targetCamera == null)
        {
            if (debugLog) Debug.LogWarning("[LineRenderer] targetCamera is null");
            return;
        }
        if (tiles == null)
        {
            if (debugLog) Debug.LogWarning("[LineRenderer] tiles null");
            return;
        }
        if (path == null || path.Count < 2)
        {
            if (debugLog) Debug.LogWarning($"[LineRenderer] invalid path (count={(path == null ? -1 : path.Count)})");
            return;
        }

        if (!TryGetCellSizePixels(tiles, out Vector2 cellSizePixels))
        {
            if (debugLog) Debug.LogWarning("[LineRenderer] failed to get cell size (pixels)");
            return;
        }

        // 先计算屏幕坐标，再把首尾延长，最后转世界坐标
        var screenPts = new List<Vector2>(path.Count);
        for (int i = 0; i < path.Count; i++)
        {
            screenPts.Add(GridToScreen(path[i], tiles, cellSizePixels));
        }

        float ext = Mathf.Min(cellSizePixels.x, cellSizePixels.y) * endExtendFactor; // 延长量，可在 Inspector 调整
        if (screenPts.Count >= 2)
        {
            Vector2 dirHead = (screenPts[1] - screenPts[0]).normalized;
            Vector2 dirTail = (screenPts[screenPts.Count - 1] - screenPts[screenPts.Count - 2]).normalized;
            if (dirHead.sqrMagnitude > 0.0001f) screenPts[0] -= dirHead * ext;
            if (dirTail.sqrMagnitude > 0.0001f) screenPts[screenPts.Count - 1] += dirTail * ext;
        }

        var positions = new List<Vector3>(screenPts.Count);
        for (int i = 0; i < screenPts.Count; i++)
        {
            var world = ScreenToWorld(screenPts[i]);
            world.z += zOffset;
            positions.Add(world);
        }

        line.positionCount = positions.Count;
        line.SetPositions(positions.ToArray());

        if (debugLog)
        {
            Debug.Log($"[LineRenderer] ShowPath count={path.Count} posCount={positions.Count} zOffset={zOffset} first={positions[0]} last={positions[positions.Count - 1]}");
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

    // 将扩展网格坐标转换为屏幕坐标；棋盘外圈用最近格子中心并按方向偏移半格
    private Vector2 GridToScreen(Vector2Int p, Tile[,] tiles, Vector2 cellSizePixels)
    {
        int row = tiles.GetLength(0);
        int col = tiles.GetLength(1);

        if (p.x >= 1 && p.x <= row && p.y >= 1 && p.y <= col)
        {
            Tile t = tiles[p.x - 1, p.y - 1];
            if (t != null)
            {
                var rt = t.image != null ? t.image.rectTransform : t.transform as RectTransform;
                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, rt.position);
                return screenPos;
            }
        }

        int cx = Mathf.Clamp(p.x - 1, 0, row - 1);
        int cy = Mathf.Clamp(p.y - 1, 0, col - 1);
        Tile refTile = tiles[cx, cy];
        if (refTile == null)
        {
            return Vector3.zero;
        }

        var refRT = refTile.image != null ? refTile.image.rectTransform : refTile.transform as RectTransform;
        Vector2 baseScreen = RectTransformUtility.WorldToScreenPoint(null, refRT.position);

        Vector2 offset = Vector2.zero;
        if (p.x == 0) offset.y += cellSizePixels.y;
        else if (p.x == row + 1) offset.y -= cellSizePixels.y;
        if (p.y == 0) offset.x -= cellSizePixels.x;
        else if (p.y == col + 1) offset.x += cellSizePixels.x;

        Vector2 targetScreen = baseScreen + offset * 0.5f;
        return targetScreen;
    }

    // 取一个格子的屏幕像素尺寸（考虑缩放）
    private bool TryGetCellSizePixels(Tile[,] tiles, out Vector2 cellSize)
    {
        int row = tiles.GetLength(0);
        int col = tiles.GetLength(1);
        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < col; y++)
            {
                var t = tiles[x, y];
                if (t != null && t.image != null)
                {
                    var rt = t.image.rectTransform;
                    var size = rt.rect.size;
                    var scale = rt.lossyScale;
                    cellSize = new Vector2(size.x * scale.x, size.y * scale.y);
                    if (debugLog)
                    {
                        Debug.Log($"[LineRenderer] cellSizePixels={cellSize} rawSize={size} scale={scale}");
                    }
                    return true;
                }
            }
        }
        cellSize = Vector2.one;
        return false;
    }

    // 将屏幕坐标转换为世界坐标（基于目标相机）
    private Vector3 ScreenToWorld(Vector2 screenPos)
    {
        return targetCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, uiPlaneDistance));
    }
}
