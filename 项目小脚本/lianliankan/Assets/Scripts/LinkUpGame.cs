using UnityEngine; // 使用Unity引擎
using UnityEngine.UI; // 使用UI命名空间
using System.Collections.Generic; // 使用泛型集合

// 连连看核心逻辑：保持格子位置不变，用空状态代替销毁；支持直线、单折、双折、三折（最多3转弯即4段路径）；增加调试日志便于定位问题。

public class LinkUpGame : MonoBehaviour // 主控制器
{
    public GameObject tilePrefab; // 方块预制体
    public Transform gridParent; // Grid父节点
    public int row = 8, col = 8; // 行列尺寸
    public Sprite[] tileSprites; // 方块贴图
    public GameObject clearEffectPrefab; // 消除特效预制体（可选）
    public float clearEffectLifetime = 1.2f; // 特效存活时间
    public float clearEffectZ = 0f; // 特效在世界坐标的Z值

    private Tile[,] tiles; // 网格数据
    private Tile firstTile, secondTile; // 当前选择的两块

    void Start() // 启动时生成棋盘
    {
        GenerateTiles(); // 构建网格
    }

    void GenerateTiles() // 生成全部方块
    {
        tiles = new Tile[row, col]; // 创建网格数组
        List<int> types = new List<int>(); // 存放成对类型
        for (int i = 0; i < row * col / 2; i++) // 填充成对编号
        {
            int t = i % tileSprites.Length; // 循环取图索引
            types.Add(t); // 第一张
            types.Add(t); // 配对的第二张
        }
        for (int i = 0; i < types.Count; i++) // Fisher-Yates乱序
        {
            int tmp = types[i]; // 临时存
            int r = Random.Range(i, types.Count); // 随机位置
            types[i] = types[r]; // 交换
            types[r] = tmp; // 完成交换
        }

        for (int x = 0; x < row; x++) // 遍历行
        {
            for (int y = 0; y < col; y++) // 遍历列
            {
                GameObject go = Instantiate(tilePrefab, gridParent); // 实例化方块
                Tile tile = go.GetComponent<Tile>(); // 拿到Tile组件
                int type = types[x * col + y]; // 取对应类型
                tile.Init(x, y, type, this); // 初始化位置与控制器
                tile.image.sprite = tileSprites[type]; // 设置图片
                tile.button.onClick.AddListener(tile.OnClick); // 注册点击
                tiles[x, y] = tile; // 存入网格
            }
        }
    }

    public void OnTileClick(Tile tile) // 被Tile点击时
    {
        if (tile.IsCleared) // 已消除的不处理
        {
            Debug.Log($"[Click] ({tile.x},{tile.y}) already cleared"); // 调试日志
            return; // 直接返回
        }
        if (firstTile == null) // 首次选择
        {
            firstTile = tile; // 记录第一块
            Debug.Log($"[Select A] ({tile.x},{tile.y}) type={tile.type}"); // 记录第一选
        }
        else if (secondTile == null && tile != firstTile) // 选择第二块且不是同一块
        {
            secondTile = tile; // 记录第二块
            Debug.Log($"[Select B] ({tile.x},{tile.y}) type={tile.type}"); // 记录第二选
            bool can = CanLink(firstTile, secondTile); // 判断可连
            Debug.Log($"[LinkTest] A=({firstTile.x},{firstTile.y}) B=({secondTile.x},{secondTile.y}) result={can}"); // 输出结果
            if (can) // 判断能否连线
            {
                firstTile.SetCleared(); // 标记第一块为空但保留位置
                secondTile.SetCleared(); // 标记第二块为空但保留位置
                tiles[firstTile.x, firstTile.y] = firstTile; // 位置保持
                tiles[secondTile.x, secondTile.y] = secondTile; // 位置保持
                Debug.Log($"[Cleared] ({firstTile.x},{firstTile.y}) & ({secondTile.x},{secondTile.y})"); // 记录清除
                PlayClearEffect(firstTile); // 播放特效A
                PlayClearEffect(secondTile); // 播放特效B
            }
            firstTile = null; // 重置选择
            secondTile = null; // 重置选择
        }
    }

    bool CanLink(Tile a, Tile b) // 连线判断：同类型且允许最多3转弯（4段）
    {
        if (a.type != b.type) return false; // 类型不同直接失败
        return PathSearch(a, b, 3); // 最多3次转弯
    }

    bool PathSearch(Tile a, Tile b, int maxTurns) // BFS搜索，允许指定转弯次数，含外圈虚拟空白
    {
        int extRows = row + 2; // 含外圈的行数
        int extCols = col + 2; // 含外圈的列数
        int[] dx = { 1, -1, 0, 0 }; // 四方向X
        int[] dy = { 0, 0, 1, -1 }; // 四方向Y

        var visited = new int[extRows, extCols, 4]; // 记录到达某格某方向的最小转弯
        for (int i = 0; i < extRows; i++) // 初始化
        {
            for (int j = 0; j < extCols; j++)
            {
                for (int d = 0; d < 4; d++) visited[i, j, d] = int.MaxValue; // 设为不可达
            }
        }

        int sx = a.x + 1, sy = a.y + 1; // 起点映射到外圈坐标
        int tx = b.x + 1, ty = b.y + 1; // 终点映射到外圈坐标

        var q = new Queue<Node>(); // BFS队列
        for (int d = 0; d < 4; d++) // 起点向四个方向试探
        {
            visited[sx, sy, d] = 0; // 起点转弯为0
            q.Enqueue(new Node(sx, sy, d, 0)); // 入队
        }

        while (q.Count > 0) // 开始BFS
        {
            var cur = q.Dequeue(); // 取队头
            int nx = cur.x + dx[cur.dir]; // 前进一步x
            int ny = cur.y + dy[cur.dir]; // 前进一步y

            if (nx < 0 || nx >= extRows || ny < 0 || ny >= extCols) continue; // 越界忽略
            if (cur.turns > maxTurns) continue; // 超过转弯上限

            if (nx == tx && ny == ty) return true; // 到达目标成功

            bool inside = nx > 0 && nx < extRows - 1 && ny > 0 && ny < extCols - 1; // 是否在原网格内
            bool passable;
            if (!inside) // 外圈视为空
            {
                passable = true; // 外圈可走
            }
            else
            {
                int gx = nx - 1, gy = ny - 1; // 映射回原网格
                Tile t = tiles[gx, gy]; // 当前格子
                passable = t == null || t.IsCleared; // 空或已清除可走
            }

            if (!passable) continue; // 不可走则跳过

            for (int nd = 0; nd < 4; nd++) // 向四个方向扩展
            {
                int nTurns = cur.turns + (nd == cur.dir ? 0 : 1); // 是否产生转弯
                if (nTurns > maxTurns) continue; // 超出上限
                if (visited[nx, ny, nd] <= nTurns) continue; // 已有更优路径
                visited[nx, ny, nd] = nTurns; // 记录最小转弯
                q.Enqueue(new Node(nx, ny, nd, nTurns)); // 入队
            }
        }

        return false; // 搜索失败
    }

    bool InBounds(int x, int y) // 判断坐标是否在网格内
    {
        return x >= 0 && x < row && y >= 0 && y < col; // 简单边界检测
    }

    void PlayClearEffect(Tile tile) // 在方块位置播放特效
    {
        if (clearEffectPrefab == null) return; // 未配置则跳过
        Vector3 worldPos = tile.transform.TransformPoint(Vector3.zero); // UI坐标转世界
        worldPos.z = clearEffectZ; // 固定Z以确保相机可见
        var go = Instantiate(clearEffectPrefab, worldPos, Quaternion.identity); // 生成特效（不挂在Canvas下）
        Destroy(go, clearEffectLifetime); // 超时销毁
        Debug.Log($"[Effect] spawn at {worldPos}"); // 调试日志
    }

    struct Node // BFS节点
    {
        public int x, y, dir, turns; // 坐标、方向、转弯数
        public Node(int x, int y, int dir, int turns) // 构造
        {
            this.x = x; // 记录x
            this.y = y; // 记录y
            this.dir = dir; // 当前方向
            this.turns = turns; // 已用转弯
        }
    }
}
