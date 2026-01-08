using System.Collections.Generic;
using UnityEngine;

// 纯算法：判定能否连通并返回路径点序列（包含起点终点），坐标用“外扩一圈”的网格
// - 核心: 在搜什么？ 是否存在一条路径，从 A 到 B，只走空格，转弯次数 ≤ maxTurns
// - 坐标系向外扩一圈，使“走出边界再折返”合法，避免边界特判
// - 最小判断单位: 格子 + 进入方向 + 已用转弯数 —— 我以某个方向进入某个格子时，已经拐了几次弯
// - 状态包含位置与进入方向，visited 记录到达该状态的最小转弯数，避免同方向重复扩展
// - 每步前进一格，若方向变化则转弯数+1；超过 maxTurns 直接剪枝，实现 O(N) 级别的可行性判定
public static class LinkPathFinder
{
    // 缓存池优化：避免每次寻路都 repeat new 数组，显著减少 GC（尤其在死局检测的频繁调用中）
    private static int[,,] _cacheVisited;
    private static Node[,,] _cachePrev;
    private static Queue<Node> _cacheQueue;

    private struct Node // BFS 状态节点
    {
        public int x; // 行坐标
        public int y; // 列坐标
        public int dir; // 进入方向 (0:下,1:上,2:右,3:左)
        public int turns; // 已用转弯数
        public Node(int x, int y, int dir, int turns)
        {
            this.x = x;
            this.y = y;
            this.dir = dir;
            this.turns = turns;
        }
    }

    /// <summary>
    /// 寻找路径
    /// </summary>
    /// <param name="tiles">原棋盘 [0,row) x [0,col)</param>
    /// <param name="row">棋盘行数</param>
    /// <param name="col">棋盘列数</param>
    /// <param name="tileA">点击的第一个格子</param>
    /// <param name="tileB">点击的第二个格子</param>
    /// <param name="maxTurns">允许的最大转弯次数</param>
    /// <param name="path">返回经过的格点路径（外扩坐标系，起终点都是 +1），用来渲染可自行 -1 映回棋盘</param>
    /// <returns></returns>
    public static bool TryFindPath(Tile[,] tiles, int row, int col, Tile tileA, Tile tileB, int maxTurns, out List<Vector2Int> path)
    {
        path = null; // 先置空，后面找到再填
        if (tileA == null || tileB == null) return false; // 空引用直接失败
        if (tileA.type != tileB.type) return false; // 不同类型直接失败

        int extRows = row + 2; // 上下各扩展一行
        int extCols = col + 2; // 左右各扩展一列

        // BFS 棋盘示例
        // (0,0) (0,1) (0,2)
        // (1,0) (1,1) (1,2)
        // (2,0) (2,1) (2,2)

        int[] dx = { 1, -1, 0, 0 }; // 下 上 右 左
        int[] dy = { 0, 0, 1, -1 };

        // ---------------------------------------------------------
        // 内存优化：检查缓存大小，不够则重新分配，够则复用
        if (_cacheVisited == null || _cacheVisited.GetLength(0) != extRows || _cacheVisited.GetLength(1) != extCols)
        {
            _cacheVisited = new int[extRows, extCols, 4];
            _cachePrev = new Node[extRows, extCols, 4];
            _cacheQueue = new Queue<Node>(extRows * extCols * 4);
        }
        else
        {
            _cacheQueue.Clear(); // 复用队列前清空
        }
        
        var visited = _cacheVisited; // 使用缓存引用
        var prev = _cachePrev;       // 使用缓存引用
        var q = _cacheQueue;         // 使用缓存引用
        // ---------------------------------------------------------

        // var visited = new int[extRows, extCols, 4]; // 三维数组 记录“以某方向到达某格子最少用了几次转弯”
        for (int i = 0; i < extRows; i++) // 遍历扩展网格的行
            for (int j = 0; j < extCols; j++) // 遍历扩展网格的列
                for (int d = 0; d < 4; d++) // 遍历四个方向
                    visited[i, j, d] = int.MaxValue; //  每个状态初始化为无限大 后续遍历了该格子后，会设置为所耗费的拐点数，越小代表距离越近

        // 前驱用于回溯路径：记录 (nx,ny,nd) 是从哪个状态过来的
        // var prev = new Node[extRows, extCols, 4]; // 记录前驱状态 用于回溯路径

        int startX = tileA.x + 1, startY = tileA.y + 1; // 起点坐标 +1 放到扩展网格里 因为外圈扩展了 这是实际的位置
        int endX = tileB.x + 1, endY = tileB.y + 1; // 终点坐标 +1 放到扩展网格里

        // var q = new Queue<Node>(); // BFS 队列
        for (int d = 0; d < 4; d++) // 四个方向各入队一次，作为起点
        {
            visited[startX, startY, d] = 0; // 起点所用转弯数为0
            prev[startX, startY, d] = new Node(-1, -1, -1, 0); // 起点无前驱，用负数表示
            q.Enqueue(new Node(startX, startY, d, 0)); // 入队
        }

        Node? found = null; // 用于记录是否找到终点 初始为 null
        while (q.Count > 0) // 队列不空意味着还有可行格子没有搜索
        {
            var cur = q.Dequeue(); // 取出队首状态

            int nx = cur.x + dx[cur.dir]; // 往当前方向走一步（x）
            int ny = cur.y + dy[cur.dir]; // 往当前方向走一步（y）

            if (nx < 0 || nx >= extRows || ny < 0 || ny >= extCols) continue; // 越边界 丢弃
            if (cur.turns > maxTurns) continue; // 转弯超限 丢弃

            if (nx == endX && ny == endY) // 到达终点
            {
                found = new Node(nx, ny, cur.dir, cur.turns); // 记录终点状态 后续需要从终点开始，沿着每个节点的“前驱”信息（prev）回溯，拼出一条完整的路径
                //只有知道终点的具体状态（包括方向和转弯数），才能正确地回溯出完整路径，因为同一个格子可能有多种到达方式（不同方向、不同转弯数）
                prev[nx, ny, cur.dir] = cur; // 达到nx, ny, cur.dir的前驱是cur
                break; // 找到了就停
            }

            bool inside = nx > 0 && nx < extRows - 1 && ny > 0 && ny < extCols - 1; // 是否在真实棋盘内（不含外圈）
            bool passable = true; // 默认可通过（在外圈都是空）
            if (inside)
            {
                int gx = nx - 1, gy = ny - 1; // 先把扩展网格坐标映射回真实棋盘坐标
                Tile t = tiles[gx, gy]; // 取格子
                passable = t == null || t.IsCleared; // 只有当这个格子是空（t == null）或者已经被消除（t.IsCleared）时，才可以通过
            }

            if (!passable) continue; // 碰到未清除的格子则不可走 

            for (int nd = 0; nd < 4; nd++) // 遍历四个方向（下、上、右、左），尝试从当前格子往每个方向继续走。
            {
                int nTurns = cur.turns + (nd == cur.dir ? 0 : 1); // 计算如果往新方向 nd 走，累计的转弯次数是多少。如果和当前方向一样，不加转弯，否则转弯数加1
                if (nTurns > maxTurns) continue; // 累计转弯次数超超限 丢弃
                if (visited[nx, ny, nd] <= nTurns) continue; // 如果之前已经用更少或相同的转弯数到达过 (nx, ny, nd) 这个状态，就跳过（剪枝）
                visited[nx, ny, nd] = nTurns; // 记录到达 (nx, ny, nd) 这个状态所需的最少转弯数
                prev[nx, ny, nd] = cur; // 记录当前状态 cur 是 (nx, ny, nd) 的前驱，方便后面回溯路径
                q.Enqueue(new Node(nx, ny, nd, nTurns)); // 把新状态加入队列，等待后续继续广度优先搜索
            }
        }

        if (found == null) return false; // 如果found仍然是null，说明没有找到路径，直接返回false

        // 回溯路径（扩展坐标系），包含起点/终点
        var stack = new Stack<Vector2Int>(); // 用栈反向压栈再输出
        var curNode = found.Value; // 从终点开始回溯
        while (curNode.x >= 0 && curNode.y >= 0 && curNode.dir >= 0) // 负数表示到达起点前驱，不是负数就继续回溯
        {
            stack.Push(new Vector2Int(curNode.x, curNode.y)); // 压当前点
            var p = prev[curNode.x, curNode.y, curNode.dir]; // 找前驱
            curNode = p; // 前驱继续回溯
        }

        path = new List<Vector2Int>(stack); // 栈逆序即正向路径
        return true; // 成功
    }
}
