using UnityEngine;
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
    public int row = 8, col = 8; // 棋盘尺寸，建议偶数以保证配对
    public Sprite[] tileSprites;

    private const int MaxBends = 2; // 连连看经典两次转弯限制；

    private Tile[,] tiles; // 核心：二维数组保存格子引用，通过tiles[x,y]访问格子
    private Tile firstTile, secondTile;// 记录 第一次点的格子，第二次点的格子，检查两次点击的格子是否能连通

    void Start()
    {
        GenerateTiles();
    }

    // 构建网格：
    // 1) 生成成对的类型编号，保证数量可整除且必有配对。
    // 2) Fisher-Yates 洗牌保证均匀随机分布，避免局部重复导致局面过易/过难。
    // 3) 实例化 prefab，挂到同一父节点，绑定点击回调。
    void GenerateTiles()
    {
        tiles = new Tile[row, col];
        var types = new List<int>();
        for (int i = 0; i < row * col / 2; i++)
        {
            int t = i % tileSprites.Length;
            types.Add(t);
            types.Add(t);
        }
        for (int i = 0; i < types.Count; i++)
        {
            int r = Random.Range(i, types.Count);
            (types[i], types[r]) = (types[r], types[i]); // Fisher-Yates
        }

        for (int x = 0; x < row; x++)
        for (int y = 0; y < col; y++)
        {
            GameObject go = Instantiate(tilePrefab, gridParent);
            Tile tile = go.GetComponent<Tile>();
            int type = types[x * col + y];
            tile.Init(x, y, type, this);
            tile.image.sprite = tileSprites[type];
            tile.button.onClick.AddListener(tile.OnClick);
            tiles[x, y] = tile;
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

        secondTile = tile; // 如果前面都走完了，就证明这是第二个格子
        Debug.Log($"[Select B] ({tile.x},{tile.y}) type={tile.type}");

        bool can = CanLink(firstTile, secondTile); // 判定两个格子能否连通
        Debug.Log($"[LinkTest] A=({firstTile.x},{firstTile.y}) B=({secondTile.x},{secondTile.y}) result={can}");
        if (can) // 如果能连通
        {
            firstTile.SetCleared(); // 标记第一个格子为已清除
            secondTile.SetCleared(); // 标记第二个格子为已清除
            Debug.Log($"[Cleared] ({firstTile.x},{firstTile.y}) & ({secondTile.x},{secondTile.y})");
        }
        firstTile = null;// 置空选择状态 准备下一轮
        secondTile = null;
    }

    bool CanLink(Tile a, Tile b)
    {
        if (a.type != b.type) return false; // 不同类型不能连
        return PathSearch(a, b, MaxBends); // 如果类型相同 就搜索路径，判断能否在允许的转弯数内连通
    }

    // BFS 搜索路径：
    // - 核心:在搜什么？ 是否存在一条路径， 从 A 到 B, 只走空格, 转弯次数 ≤ MaxBends
    // - 坐标系向外扩一圈，使“走出边界再折返”合法，无需对边界做额外特判。
    // - 最小判断单位:格子 + 进入方向 + 已用转弯数 ———— 我以某个方向进入某个格子时，已经拐了几次弯
    // - 状态包含位置与进入方向，visited 记录到达该状态的最小转弯数，避免同方向重复扩展。
    // - 每步前进一格，若方向变化则转弯数+1；超过 MaxBends 直接剪枝，实现 O(N) 级别的可行性判定。
    bool PathSearch(Tile a, Tile b, int maxTurns)
    {
        int extRows = row + 2; // 上下各扩展一行
        int extCols = col + 2; // 左右各扩展一列

        int[] dx = { 1, -1, 0, 0 }; // BFS 四向
        int[] dy = { 0, 0, 1, -1 };

        var visited = new int[extRows, extCols, 4]; // 三维数组 记录 以某个方向到达这个格子，最少用了几次转弯
        for (int i = 0; i < extRows; i++)
            for (int j = 0; j < extCols; j++)
                for (int d = 0; d < 4; d++)
                    visited[i, j, d] = int.MaxValue; // 没来过 = 无限大  后面如果能用更少转弯到达 → 值会被更新成更小的数

        int sx = a.x + 1, sy = a.y + 1;// 因为两边各扩展了一圈 所以棋盘要居中，也就是坐标都+1
        int tx = b.x + 1, ty = b.y + 1;// 目标点同理

        var q = new Queue<Node>(); // BFS 队列
        for (int d = 0; d < 4; d++) // 四个方向各入队一次 作为起点
        {
            visited[sx, sy, d] = 0;
            q.Enqueue(new Node(sx, sy, d, 0));
        }

        while (q.Count > 0) // 队列不空意味着还有路可以走，继续探索
        {
            var cur = q.Dequeue(); // 取出队首状态
            
            int nx = cur.x + dx[cur.dir]; // 往当前方向走一步
            int ny = cur.y + dy[cur.dir]; // 同上

            if (nx < 0 || nx >= extRows || ny < 0 || ny >= extCols) continue;
            if (cur.turns > maxTurns) continue;
            if (nx == tx && ny == ty) return true;

            bool inside = nx > 0 && nx < extRows - 1 && ny > 0 && ny < extCols - 1;
            bool passable = true;
            if (inside)
            {
                int gx = nx - 1, gy = ny - 1;
                Tile t = tiles[gx, gy];
                passable = t == null || t.IsCleared;
            }

            if (!passable) continue;

            for (int nd = 0; nd < 4; nd++)
            {
                int nTurns = cur.turns + (nd == cur.dir ? 0 : 1);
                if (nTurns > maxTurns) continue;
                if (visited[nx, ny, nd] <= nTurns) continue;// 剪枝 如果我以前已经“以同一个方向 nd”到达过 (nx, ny)，而且当时用的转弯数 ≤ 现在要用的转弯数，那这条路没有任何探索价值，直接丢弃。
                visited[nx, ny, nd] = nTurns;
                q.Enqueue(new Node(nx, ny, nd, nTurns));
            }
        }

        return false;
    }

    struct Node // BFS 状态：位置 + 进入方向 + 已用转弯数
    {
        public int x, y, dir, turns; // 我现在在 (x,y)，是从 dir 这个方向过来的，已经拐了 turns 次弯
        public Node(int x, int y, int dir, int turns)
        {
            this.x = x;
            this.y = y;
            this.dir = dir;
            this.turns = turns;
        }
    }
}
