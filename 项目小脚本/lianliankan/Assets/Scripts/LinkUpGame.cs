using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

// 连连看核心控制器：
// - 棋盘位置固定，消除后只做“空”标记，不移动/压缩，便于路径判定和还原。
// - 采用经典规则：最多两次转弯（3段线），因此通过 MaxBends 约束搜索剪枝。
// - 路径搜索在原网格外包一圈虚拟空白，避免边界特判；BFS 记录“到达某格+方向的最少转弯数”以防重复无效扩展。
// - 保留详细日志便于在策划调整时快速定位路径异常或关卡问题。
public class LinkUpGame : MonoBehaviour
{
    public GameObject tilePrefab; // 我用什么东西来生成格子？ 格子预制体，要拖
    public Transform gridParent; // 生成出来的格子，放到谁下面？ Canvas下创建Grid作为格子父节点，要拖
    public int row = 8, col = 8; // 棋盘尺寸，要偶数以保证配对
    public Sprite[] tileSprites; // 不同类型的格子用到的图片，要拖 之后改成通过当前工程的AB包加载
    public LinkLineRenderer lineRenderer; // 连线路径渲染（可选，不拖则不显示线）

    private const int MaxBends = 2; // 连连看经典两次转弯限制；

    private Tile[,] tiles; // 核心：二维数组保存格子引用，通过tiles[x,y]访问格子
    private Tile firstTile, secondTile;// 记录 第一次点的格子，第二次点的格子，检查两次点击的格子是否能连通

    void Start()
    {
        // 获取 gridParent 物体上的 GridLayoutGroup 组件。
        // gridParent 是我们在 Inspector 面板里拖进去的那个父节点（通常是一个 Panel 或空物体），用来挂载所有生成的格子。
        // GridLayoutGroup 是 Unity 自带的 UI 布局组件，能自动把子物体排列成网格。
        var glg = gridParent.GetComponent<GridLayoutGroup>();

        // 检查是否成功获取到了组件。
        // 如果 gridParent 上没有挂 GridLayoutGroup，这一步会返回 null，为了防止报错，加个判断。
        if (glg != null)
        {
            // 设置布局约束模式为“固定列数”（FixedColumnCount）。
            // GridLayoutGroup 有三种约束模式：
            // 1. Flexible（灵活）：自动根据宽高换行。
            // 2. FixedColumnCount（固定列数）：强制每行只有 N 个，多了自动换行。
            // 3. FixedRowCount（固定行数）：强制每列只有 N 个，多了自动换列。
            // 这里我们选择固定列数，是为了让棋盘严格按照我们代码里定义的 col 变量来排版。
            glg.constraint = GridLayoutGroup.Constraint.FixedColumnCount;

            // 设置具体的列数限制。
            // 把我们在代码里定义的 col 变量（比如 8）赋值给 constraintCount。
            // 这样 GridLayoutGroup 就会强制每行只显示 8 个格子，第 9 个格子会自动换到下一行。
            glg.constraintCount = col;
        }

        GenerateTiles(); // 游戏开始时生成格子
        CheckDeadlockAndShuffleIfNeeded(); // 生成完先检测一次，初始就死局则立刻洗牌
    }

    // 构建网格：
    // 1) 生成成对的类型编号，保证数量可整除且必有配对。
    // 2) Fisher-Yates 洗牌保证均匀随机分布，避免局部重复导致局面过易/过难。
    // 3) 实例化 prefab，挂到同一父节点，绑定点击回调。
    void GenerateTiles()
    {
        tiles = new Tile[row, col]; // new一个二维数组 用于存放格子引用
        int total = row * col; // 计算总格子数
        if (total % 2 != 0) // 必须是偶数才能配对
        {
            Debug.LogError($"[Init] 棋盘格子总数必须为偶数才能配对，当前 {row}x{col}={total} 是奇数，生成中止，请调整 row/col。");
            return;
        }
        var types = new List<int>(); // 临时容器 把“每个格子是什么类型”算出来，再统一铺到棋盘上
        for (int i = 0; i < total / 2; i++) // total / 2 保证每种类型成对出现
        {
            int t = i % tileSprites.Length; // 保证 type 永远不会超过 sprite 数量
            types.Add(t); // 添加一对
            types.Add(t);
        }

        for (int i = 0; i < types.Count; i++)
        {
            int r = Random.Range(i, types.Count); // 左闭右开 范围 i 到 types.Count - 1, 包含 i 不包含 types.Count
            (types[i], types[r]) = (types[r], types[i]); // Fisher-Yates
        }

        for (int x = 0; x < row; x++)
            for (int y = 0; y < col; y++)
            {
                GameObject go = Instantiate(tilePrefab, gridParent); // 用tilePrefab生成一个格子实例，放到gridParent下面
                Tile tile = go.GetComponent<Tile>(); // 获取格子实例上的 Tile 脚本组件
                int type = types[x * col + y]; // 把一维List映射到二维坐标系上  公式：行号 * 列数 + 列号
                tile.Init(x, y, type, this); // 初始化格子坐标、类型、回调对象
                tile.image.sprite = tileSprites[type]; // 设置不同Type的格子图片
                tile.button.onClick.AddListener(tile.OnClick); // 绑定点击回调,如果有事件中心管理器的话，这里可以改成发事件
                tiles[x, y] = tile; // 把格子引用存到二维数组里，方便后续访问
            }
    }

    // 选择-判定-清除主流程：
    // - 只允许两个 不同 且 未清除 的格子进入判定。
    // - 判定成功后仅标记为空，不移位，保持网格索引稳定供后续搜索使用。
    public void OnTileClick(Tile tile)
    {
        if (tile.IsCleared) // 已清除的格子不能再点，理论上不会发生，因为设置了禁用点击 但是加个保险没坏处
        {
            Debug.Log($"[Click] ({tile.x},{tile.y}) already cleared");
            return;
        }

        if (firstTile == null) // 如果还没选第一个格子 
        {
            firstTile = tile; // 就把当前点击的格子设为第一个格子
            Debug.Log($"[Select A] ({tile.x},{tile.y}) type={tile.type}");
            return;
        }

        if (tile == firstTile) return; // 如果点击的是已经选中的第一个格子，忽略，不能自己跟自己连

        secondTile = tile; // 如果前面都走完了能到这里，就证明这是第二个格子
        Debug.Log($"[Select B] ({tile.x},{tile.y}) type={tile.type}");

        bool can = LinkPathFinder.TryFindPath(tiles, row, col, firstTile, secondTile, MaxBends, out var path); // 判定两个格子能否连通，并尝试拿到路径
        Debug.Log($"[LinkTest] A=({firstTile.x},{firstTile.y}) B=({secondTile.x},{secondTile.y}) result={can} pathCount={(path == null ? -1 : path.Count)}");
        if (can) // 如果能连通
        {
            // 如果挂了渲染器，就把路径交给它画；没挂则跳过
            if (lineRenderer != null && path != null)
            {
                Debug.Log($"[LineDraw] calling renderer with pathCount={path.Count}");
                lineRenderer.ShowPath(path, tiles);
            }

            firstTile.SetCleared(); // 标记第一个格子为已清除
            secondTile.SetCleared(); // 标记第二个格子为已清除
            Debug.Log($"[Cleared] ({firstTile.x},{firstTile.y}) & ({secondTile.x},{secondTile.y})");

            if (lineRenderer != null)
            {
                StartCoroutine(ClearLineWaitSecond()); // 过X秒清掉线，给玩家看到瞬间连线效果
            }
        }
        firstTile = null;// 置空选择状态 准备下一轮
        secondTile = null;

        // 无论成功或失败，都检测死局；初始或失败连线也能触发洗牌
        CheckDeadlockAndShuffleIfNeeded();
    }

    bool CanLink(Tile a, Tile b)
    {
        if (a.type != b.type) return false; // 不同类型不能连
        return LinkPathFinder.TryFindPath(tiles, row, col, a, b, MaxBends, out _); // 只要能找到路径即可
    }

    // 在棋盘中找到任意一对可连通的 Tile，找不到返回 null
    (Tile, Tile)? FindAnyLinkablePair()
    {
        for (int x1 = 0; x1 < row; x1++)
        {
            for (int y1 = 0; y1 < col; y1++)
            {
                Tile a = tiles[x1, y1];
                if (a == null || a.IsCleared) continue;

                for (int x2 = x1; x2 < row; x2++)
                {
                    int yStart = (x2 == x1) ? y1 + 1 : 0; // 同一行避免重复比较自己和之前的格子
                    for (int y2 = yStart; y2 < col; y2++)
                    {
                        Tile b = tiles[x2, y2];
                        if (b == null || b.IsCleared) continue;
                        if (a.type != b.type) continue;
                        if (CanLink(a, b)) return (a, b);
                    }
                }
            }
        }
        return null;
    }

    void CheckDeadlockAndShuffleIfNeeded()
    {
        if (FindAnyLinkablePair() == null)
        {
            Debug.Log("[Deadlock] no linkable pair, shuffling...");
            int guard = 0; // 防止极端情况下无限洗牌
            do
            {
                ShuffleBoard(); // 洗一次
                guard++;
            } while (FindAnyLinkablePair() == null && guard < 10);

            if (guard >= 10)
            {
                Debug.LogWarning("[Deadlock] still no pair after 10 shuffles, keeping current layout");
            }
        }
    }

    IEnumerator ClearLineWaitSecond()
    {
        yield return new WaitForSeconds(0.1f); // 等0.1秒，让线渲染出来 配合图片消除的节奏
        lineRenderer.Clear();
    }

    // 只洗未清除格子的 type，不动坐标和对象
    void ShuffleBoard()
    {
        var aliveTiles = new List<Tile>();
        var types = new List<int>();

        for (int x = 0; x < row; x++)
            for (int y = 0; y < col; y++)
            {
                Tile t = tiles[x, y];
                if (t != null && !t.IsCleared)
                {
                    aliveTiles.Add(t);
                    types.Add(t.type);
                }
            }

        if (types.Count % 2 != 0)
        {
            Debug.LogError("[ShuffleBoard] odd number of alive tiles, aborting shuffle");
            return;
        }

        for (int i = 0; i < types.Count; i++)
        {
            int r = Random.Range(i, types.Count);
            (types[i], types[r]) = (types[r], types[i]);
        }

        for (int i = 0; i < aliveTiles.Count; i++)
        {
            Tile t = aliveTiles[i];
            int newType = types[i];
            t.type = newType;
            t.image.sprite = tileSprites[newType];
            t.image.color = Color.white;
            t.button.interactable = true;
        }

        firstTile = null;
        secondTile = null;

        Debug.Log("[ShuffleBoard] board shuffled");
    }

}
