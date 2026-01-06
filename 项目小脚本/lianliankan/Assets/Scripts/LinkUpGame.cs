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
            Debug.Log($"[点击] ({tile.x},{tile.y}) 已经消除");
            return;
        }

        if (firstTile == null) // 如果还没选第一个格子 
        {
            firstTile = tile; // 就把当前点击的格子设为第一个格子
            Debug.Log($"[选中 A] ({tile.x},{tile.y}) 类型={tile.type}");
            return;
        }

        if (tile == firstTile) return; // 如果点击的是已经选中的第一个格子，忽略，不能自己跟自己连

        secondTile = tile; // 如果前面都走完了能到这里，就证明这是第二个格子
        Debug.Log($"[选中 B] ({tile.x},{tile.y}) 类型={tile.type}");

        bool can = LinkPathFinder.TryFindPath(tiles, row, col, firstTile, secondTile, MaxBends, out var path); // 判定两个格子能否连通，并尝试拿到路径
        Debug.Log($"[连通测试] A=({firstTile.x},{firstTile.y}) B=({secondTile.x},{secondTile.y}) 结果={can} 路径节点数={(path == null ? -1 : path.Count)}");
        if (can) // 如果能连通
        {
            // 如果挂了渲染器，就把路径交给它画；没挂则跳过
            if (lineRenderer != null && path != null)
            {
                Debug.Log($"[绘制连线] 调用渲染器，路径节点数={path.Count}");
                lineRenderer.ShowPath(path, tiles);
            }

            firstTile.SetCleared(); // 标记第一个格子为已清除
            secondTile.SetCleared(); // 标记第二个格子为已清除
            Debug.Log($"[消除] ({firstTile.x},{firstTile.y}) & ({secondTile.x},{secondTile.y})");

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
        return LinkPathFinder.TryFindPath(tiles, row, col, a, b, MaxBends, out _); // 只要能找到路径即可 out用_忽略返回的路径
    }


    (Tile, Tile)? FindAnyLinkablePair()    // 用于检测当前局面是否死局 在棋盘中找到任意一对可连通的 Tile，找不到返回 null
    {
        for (int x1 = 0; x1 < row; x1++) // 遍历棋盘所有行，作为第一个格子 A 的行号
        {
            for (int y1 = 0; y1 < col; y1++)  //遍历棋盘所有列，作为第一个格子 A 的列号
            {
                Tile a = tiles[x1, y1]; // 拿到第一个格子 A
                if (a == null || a.IsCleared) continue; // 检查 A 是否有效。如果是 null 或者已经消除过(IsCleared)，就跳过，找下一个
                // 开始找第二个格子 B。
                // 注意 x2 = x1：我们要避免重复比较（比如查过 A和B，就不用再查 B和A），所以 B 的行号至少从 A 开始
                for (int x2 = x1; x2 < row; x2++)
                {
                    // 这是一个聪明的小技巧：
                    // 如果 B 和 A 在同一行 (x2 == x1)，那么 B 必须在 A 的后面 (y1 + 1)，否则会重复比较或者自己比自己。
                    // 如果 B 在 A 的下面几行，那么 B 从第 0 列开始遍历即可。
                    int yStart = (x2 == x1) ? y1 + 1 : 0; 
                    
                    for (int y2 = yStart; y2 < col; y2++) // 遍历第二个格子 B 的列号
                    {
                        Tile b = tiles[x2, y2]; // 拿到第二个格子 B

                        if (b == null || b.IsCleared) continue; // 检查 B 是否有效（未被消除）
                        
                        if (a.type != b.type) continue; // 快速剪枝：如果图案类型都不一样，肯定连不上，不用费劲去算路径了，直接跳过
                        
                        // 核心判断：调用寻路算法。如果 A 和 B 能连通...
                        if (CanLink(a, b)) return (a, b); // ...立刻打包返回这一对格子 (a, b)，工作结束！
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
            // 先检查是否所有格子都消完了（游戏胜利）
            bool hasAlive = false;
            foreach (var t in tiles)
            {
                if (t != null && !t.IsCleared)
                {
                    hasAlive = true;
                    break;
                }
            }

            if (!hasAlive)
            {
                Debug.Log("[游戏结束] 恭喜通关！所有格子均已消除。");
                return;
            }

            Debug.Log("[死局] 无可连通对，正在洗牌...");
            int guard = 0; // 防止极端情况下无限洗牌
            do
            {
                ShuffleBoard(); // 洗一次
                guard++;
            } while (FindAnyLinkablePair() == null && guard < 10);

            if (guard >= 10)
            {
                Debug.LogWarning("[死局] 洗牌 10 次后仍无解，保持当前布局，可选择弹出游戏失败提示");
            }
        }
    }

    IEnumerator ClearLineWaitSecond()
    {
        yield return new WaitForSeconds(0.3f); // 等0.3秒，让线渲染出来 配合图片消除的节奏
        lineRenderer.Clear();
    }

    // 函数定义：只洗未清除格子的 type，不动坐标和对象
    void ShuffleBoard()
    {
        // ----------------------
        // 第一步：收集桌面上还活着的“牌”
        // ----------------------
        var aliveTiles = new List<Tile>(); // 准备一个空袋子，装“活着的格子对象”（为了知道要往哪些位置放回数据）
        var types = new List<int>();       // 准备另一个空袋子，装“活着的格子身上的图案类型”（这就是我们要打乱的数据）

        // 遍历整个棋盘...
        for (int x = 0; x < row; x++)
            for (int y = 0; y < col; y++)
            {
                Tile t = tiles[x, y];
                // 如果这个格子存在，并且还没有被消除（!t.IsCleared）
                if (t != null && !t.IsCleared)
                {
                    aliveTiles.Add(t);  // 把这个格子对象记下来：“这个位置有人”
                    types.Add(t.type);  // 把这个格子当前的图案类型收走：“把你的牌交出来”
                }
            }

        // ----------------------
        // 第二步：安全检查（防Bug）
        // ----------------------
        // 如果收上来的牌总数是奇数，肯定出大问题了（连连看必须成对出现），这时候洗牌可能会导致死局或者无法配对。
        if (types.Count % 2 != 0)
        {
            Debug.LogError("[洗牌] 剩余格子数为奇数，放弃洗牌");
            return; // 直接中止，啥也不干
        }

        // ----------------------
        // 第三步：打乱图案数据 (Fisher-Yates 洗牌算法)
        // ----------------------
        // 这个循环就是为了把 types 列表里的数字彻底搞乱
        for (int i = 0; i < types.Count; i++)
        {
            int r = Random.Range(i, types.Count); // 从当前位置 i 到末尾 随机挑一个位置 r
            // 交换 i 和 r 位置的数据。
            // 这是最标准的随机洗牌算法，比“随机交换100次”这种土办法要均匀且高效得多。
            (types[i], types[r]) = (types[r], types[i]); 
        }

        // ----------------------
        // 第四步：把打乱后的图案发回去
        // ----------------------
        // 遍历每一个还活着的格子
        for (int i = 0; i < aliveTiles.Count; i++)
        {
            Tile t = aliveTiles[i]; // 拿到第 i 个格子对象（它的物理位置没变）
            int newType = types[i]; // 拿到第 i 个已经被打乱的新图案类型

            t.type = newType; // 1. 更新数据：把它的类型改成新的
            t.image.sprite = tileSprites[newType]; // 2. 更新画面：把它的图片换成新的类型对应的图片
            
            // 3. 重置状态：确保它看起来是正常的（比如不是灰色的或不可点的）
            // 虽然理论上它本来就是活着的，但这步是保险，防止之前有什么选中状态残留
            t.image.color = Color.white; 
            t.button.interactable = true;
        }

        // ----------------------
        // 第五步：清理残留状态
        // ----------------------
        // 洗牌后，之前选中的格子（如果有）就已经变了，原来的选择就没有意义了，所以要清空玩家当前的选中状态。
        firstTile = null;
        secondTile = null;

        Debug.Log("[洗牌] 棋盘已重洗");
    }

}
