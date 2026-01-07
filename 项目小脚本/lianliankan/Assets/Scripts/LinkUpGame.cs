using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// 连连看核心控制器
/// 自动管理 Grid 布局、资源加载、游戏流程控制
/// </summary>
public class LinkUpGame : MonoBehaviour
{
    // --- 游戏设置部分 ---
    
    [Header("Settings")]
    public int row = 4; // 定义游戏里有几行格子，默认4行
    public int col = 8; // 定义游戏里有几列格子，默认8列
    public Vector2 tileSize = new Vector2(50, 50); // 每一个格子有多大，默认50x50
    public float lineDuration = 0.3f; // 连线消除时，那条连线显示多久（单位秒），这里是0.3秒
    public float lineWidth = 0.2f; // 设置连线的粗细，0.2比较适中
    public Color lineColor = Color.white; // 设置连线的颜色，默认是白色

    [Header("Events")]
    public UnityEvent onGameWin; // 这是一个“事件”，当游戏赢了的时候触发。目的是方便我们在编辑器里哪怕不写代码，也能拖拽一些胜利后的特效或者弹窗。

    [Header("UI References")]
    public Button startButton; // 游戏开始按钮，需要手动拖拽或自动查找
    public Button closeButton; // 游戏退出/关闭按钮

    [Header("Resources")]
    public string tilePrefabPath = "Tile"; // 告诉代码，格子的预制体（Prefab）在Resources文件夹下的叫啥名字
    public string spritesPath = "Icons"; // 告诉代码，图片素材在Resources文件夹下的叫啥名字
    public string lineRendererPrefabPath = "LinkLineRenderer"; // 连线画笔的预制体路径

    // --- 私有变量（程序内部用的，不想暴露给编辑器） ---
    
    private GameObject tilePrefab; // 用来存加载进来的格子预制体（模具）
    private Sprite[] tileSprites; // 用来存加载进来的一堆小图标图片
    private LinkLineRenderer lineRenderer; // 专门负责画线的组件引用
    private Transform gridParent; // 网格的父物体，所有的格子都要挂在它下面，整齐排列
    private Tile[,] tiles; // 这是一个二维数组，像Excel表格一样存着所有的格子数据，方便按行按列找
    private Tile firstTile, secondTile; // 记录玩家点的第一个格子和第二个格子
    private const int MaxBends = 2; // 连连看规则：最多只能拐弯2次，这是个常量

    // Start是游戏开始时自动运行的函数
    void Start()
    {
        LoadAssets();       // 预加载资源
        SetupEnvironment(); // 预环境搭建（找到Grid等）

        // 尝试自动查找开始按钮（如果 Inspector 没拖）
        if (startButton == null)
        {
            Transform btnTrans = transform.Find("btnStart"); // 开始按钮名字叫 "btnStart" 
            if (btnTrans != null) startButton = btnTrans.GetComponent<Button>();   
        }
        
        // 尝试自动查找关闭按钮
        if (closeButton == null)
        {
            Transform clsTrans = transform.Find("btnClose"); // 关闭按钮名字叫 "btnClose"
            if (clsTrans != null) closeButton = clsTrans.GetComponent<Button>();
        }

        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartBtnClick);
            startButton.gameObject.SetActive(true); // 默认显示开始
        }
        else
        {
            Debug.LogWarning("[LinkUpGame] 没有绑定 Start 按钮，游戏不会自动开始。请在 Inspector 赋值或确保子物体有名为 Start 的按钮。");
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(OnCloseBtnClick);
            closeButton.gameObject.SetActive(false); // 默认隐藏关闭，点了开始才出来
        }
    }

    /// <summary>
    /// 响应开始按钮点击
    /// </summary>
    public void OnStartBtnClick()
    {
        // 开始游戏
        StartGame();

        // 隐藏按钮自己（防止重复点击，且进入游戏画面）
        if (startButton != null) startButton.gameObject.SetActive(false);
        // 显示退出按钮
        if (closeButton != null) closeButton.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// 响应关闭按钮点击
    /// </summary>
    public void OnCloseBtnClick()
    {
        // 清理游戏数据和画面
        CleanupOldGame();
        
        // 恢复按钮状态
        if (startButton != null) startButton.gameObject.SetActive(true);
        if (closeButton != null) closeButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// 公开的启动游戏接口，供外部（UI按钮）调用
    /// </summary>
    public void StartGame()
    {
        // 防御性清理：防止有残留
        CleanupOldGame();

        // 确保环境准备就绪
        if (IsEnvironmentReady())
        {
            GenerateTiles(); // 生成格子
            CheckDeadlockAndShuffleIfNeeded(); // 初始检查
        }
    }

    // 清理旧游戏数据
    private void CleanupOldGame()
    {
        // 1. 停止所有协程（如画线的延迟清除），防止对象被销毁后还在跑
        StopAllCoroutines();

        // 2. 销毁所有格子物体
        if (gridParent != null)
        {
            // 倒序删除所有子物体，因为Destroy是延迟的，正序遍历在这里没问题，但倒序是更安全的习惯
            for (int i = gridParent.childCount - 1; i >= 0; i--)
            {
                Destroy(gridParent.GetChild(i).gameObject);
            }
        }
        
        // 3. 断开核心数据引用，帮助 GC 回收
        firstTile = null;
        secondTile = null;
        tiles = null; // 释放大数组引用
        
        // 4. 清理画线器
        if (lineRenderer != null) lineRenderer.Clear();
    }

    // 负责加载资源，如果要从AB包加载，这里改成AB包接口
    private void LoadAssets()
    {
        // 去Resources文件夹找名字叫tilePrefabPath的东西，拿出来当格子模具
        tilePrefab = Resources.Load<GameObject>(tilePrefabPath);
        // 去Resources文件夹找名字叫spritesPath的文件夹，把里面所有的图片都拿出来存好
        tileSprites = Resources.LoadAll<Sprite>(spritesPath);
    }

    // 负责搭建游戏运行环境（Grid容器和LineRenderer画线器）
    private void SetupEnvironment()
    {
        // 1. 设置网格容器 (Grid)
        // 尝试在自己下面找找有没有叫 "Grid" 的物体
        Transform searchGrid = transform.Find("Grid");
        if (searchGrid != null)
        {
            // 找到了就直接用
            gridParent = searchGrid;
        }
        else
        {
            // 没找到，就新建一个空的GameObject并起名叫"Grid"
            GameObject gridGo = new GameObject("Grid");
            // 把这个新Grid挂在脚本所在的物体下面，保持层级整洁
            gridGo.transform.SetParent(this.transform, false);
            
            // 添加GridLayoutGroup组件，这个组件能自动帮我们把子物体排成方阵
            var glg = gridGo.AddComponent<GridLayoutGroup>();
            // 设置约束为“固定列数”，也就是每行固定几个，剩下的往下排
            glg.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            glg.constraintCount = col; // 列数设为我们定义的col
            glg.cellSize = tileSize; // 使用上面定义的大小
            glg.childAlignment = TextAnchor.MiddleCenter; // 让格子整体居中对齐
            
            // 重要提醒：AddComponent后Transform引用可能变了（虽然一般不怎么变，保险起见重新获取一下）
            gridParent = gridGo.transform; 
            
            // 设置Grid的位置和锚点，让它撑满整个父物体或者居中
            RectTransform rt = gridGo.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero; // 大小偏移归零，配合上面的锚点铺满
            rt.anchoredPosition = Vector2.zero; // 位置归零
        }

        // 2. 设置连线画笔 (LineRenderer)
        // 看看子物体里有没有现成的画线器
        var existingLine = transform.GetComponentInChildren<LinkLineRenderer>();
        if (existingLine != null)
        {
            // 有就直接用
            lineRenderer = existingLine;
        }
        else
        {
            // 没有就去资源里加载一个预制体
            GameObject lineRes = Resources.Load<GameObject>(lineRendererPrefabPath);
            GameObject lineGo;
            
            if (lineRes != null)
            {
                // 如果有预制体，就克隆一个出来
                lineGo = Instantiate(lineRes, transform); 
                lineGo.name = "LinkLineRenderer";
            }
            else
            {
                // 如果连预制体也没有，就代码手搓一个
                lineGo = new GameObject("LinkLineRenderer");
                lineGo.transform.SetParent(transform, false);
                // 给它加个LineRenderer组件，这是Unity自带的画线功能
                var lr = lineGo.AddComponent<LineRenderer>();
                // 给它个默认材质，不然画出来可能是紫色的默认错误色
                lr.material = new Material(Shader.Find("Sprites/Default")); 
                // 挂上我们自己写的LinkLineRenderer辅助脚本
                lineGo.AddComponent<LinkLineRenderer>(); 
            }
            // 获取最终的脚本引用
            lineRenderer = lineGo.GetComponent<LinkLineRenderer>();
            // 把它放在层级的最后面，通常意味着显示在最上层
            lineGo.transform.SetAsLastSibling(); 
        }

        // 应用画线器的设置
        if (lineRenderer != null)
        {
            // 告诉画线器用哪个摄像机（通常是主摄像机）
            lineRenderer.targetCamera = Camera.main; 
            // 获取Unity原本的LineRenderer组件来设置外观
            LineRenderer lrComponent = lineRenderer.GetComponent<LineRenderer>();
            if (lrComponent != null)
            {
                // 设置起止宽度
                lrComponent.startWidth = lineWidth;
                lrComponent.endWidth = lineWidth;
                // 设置起止颜色
                lrComponent.startColor = lineColor;
                lrComponent.endColor = lineColor;
            }
        }
    }

    // 检查环境是不是都OK了，防止后面报错
    private bool IsEnvironmentReady()
    {
        bool ready = true;
        // 如果格子模具没找到，报错
        if (tilePrefab == null) 
        {
            Debug.LogError($"[连连看] 缺少格子预制体（Prefab），请检查 Resources/{tilePrefabPath} 是否存在");
            ready = false;
        }
        // 如果图片没找到，报错
        if (tileSprites == null || tileSprites.Length == 0)
        {
            Debug.LogError($"[连连看] 缺少图片资源，请检查 Resources/{spritesPath} 文件夹里是否有图片");
            ready = false;
        }
        // 如果Grid父节点没创建成功，报错
        if (gridParent == null)
        {
            Debug.LogError("[连连看] Grid 网格容器创建失败");
            ready = false;
        }
        return ready;
    }

    // --- 游戏核心逻辑部分 ---

    // 生成所有的格子
    void GenerateTiles()
    {
        if (tileSprites.Length == 0) return; // 没图就不生成了

        // 初始化那个二维数组
        tiles = new Tile[row, col]; 
        int total = row * col; // 总共多少个格子
        // 连连看必须是偶数个格子，不然有一个消不掉
        if (total % 2 != 0)
        {
            Debug.LogError($"[连连看] 格子总数 {total} 必须是偶数，否则没法完全消完！请修改行列数。");
            return;
        }

        // 准备一个列表，用来装所有的图案ID
        var types = new List<int>(); 
        // 循环生成成对的ID
        for (int i = 0; i < total / 2; i++) 
        {
            int t = i % tileSprites.Length; // 轮流取图，防止图不够用
            types.Add(t); // 加一个
            types.Add(t); // 再加一个，成对出现
        }

        // 洗牌算法：把生成的成对ID打乱
        for (int i = 0; i < types.Count; i++)
        {
            int r = Random.Range(i, types.Count); // 随机找一个位置
            (types[i], types[r]) = (types[r], types[i]); // 交换位置，C#的新语法
        }

        // 开始往Grid里填格子
        for (int x = 0; x < row; x++)
            for (int y = 0; y < col; y++)
            {
                // 实例化一个格子
                GameObject go = Instantiate(tilePrefab, gridParent); 
                // 获取格子上的Tile脚本
                Tile tile = go.GetComponent<Tile>(); 
                // 从刚才打乱的列表里取一个类型ID
                int type = types[x * col + y]; 
                // 初始化这个格子，告诉它坐标、类型、还有谁管它（this）
                tile.Init(x, y, type, this); 
                // 设置图片
                tile.image.sprite = tileSprites[type]; 
                
                // 设置点击事件：先清空旧的，再绑定新的OnTileClick
                tile.button.onClick.RemoveAllListeners(); 
                tile.button.onClick.AddListener(tile.OnClick); 
                // 存进数组里方便后面查
                tiles[x, y] = tile; 
            }
    }

    // 当某个格子被点击时执行的逻辑
    public void OnTileClick(Tile tile)
    {
        if (tile.IsCleared) return; // 如果已经消掉了，点了也没用
        
        // 如果还没选过第一个格子
        if (firstTile == null) 
        {
            firstTile = tile; // 把当前这个记为第一个
            firstTile.SetSelected(true); // 【新增】选中高亮
            return; // 完事，等下一次点击
        }
        // 如果点的还是刚才那个格子（重复点）
        if (tile == firstTile)
        {
            firstTile.SetSelected(false); // 【新增】取消高亮
            firstTile = null;
            return; // 忽略
        }

        // 把这个记为第二个格子
        secondTile = tile; 

        // 核心判断：尝试找路径。LinkPathFinder是个寻路算法类。
        // 传入整个地图(tiles)、大小、起止点、最大拐弯数。
        // out var path 会把找到的路径点返给我们。
        bool can = LinkPathFinder.TryFindPath(tiles, row, col, firstTile, secondTile, MaxBends, out var path); 
        
        if (can) // 如果能连通
        {
            // 如果画线器在，并且有路径
            if (lineRenderer != null && path != null)
            {
                // 画出这条线
                lineRenderer.ShowPath(path, tiles);
                // 开启个协程，等一会再把线灭掉，不然太快看不清
                StartCoroutine(ClearLineWait());
            }

            // 【新增】在消除之前，先关掉选中状态（虽然SetCleared也会关，但手动关一下更保险）
            firstTile.SetSelected(false);

            // 把两个格子都标记为已消除
            firstTile.SetCleared(); 
            secondTile.SetCleared(); 
        }
        else
        {
            // 【新增】匹配失败，两个格子都抖动一下
            firstTile.PlayShakeAnimation();
            secondTile.PlayShakeAnimation();
            
            // 【新增】失败后取消选选中状态
            firstTile.SetSelected(false);
            // secondTile 刚点下去还没highlight，不用管
        }
        
        // 无论连没连上，手里的选定状态都要清空，准备下一轮
        firstTile = null;
        secondTile = null;

        // 每次操作完，检查一下是不是死局，或者赢没赢
        CheckDeadlockAndShuffleIfNeeded();
    }
    
    // --- 死局检测逻辑 ---

    // 简单封装一下：判断两个格子能不能连
    bool CanLink(Tile a, Tile b)
    {
        // 必须类型相同，并且寻路能通（不需要具体路径，所以out _）
        return a.type == b.type && LinkPathFinder.TryFindPath(tiles, row, col, a, b, MaxBends, out _);
    }

    // 全图扫描：看还有没有能消除的一对儿
    bool HasLinkablePair()
    {
        // 暴力遍历所有格子A
        for (int x1 = 0; x1 < row; x1++) 
        {
            for (int y1 = 0; y1 < col; y1++)  
            {
                Tile a = tiles[x1, y1]; 
                if (a == null || a.IsCleared) continue; // 空的或者消掉的跳过
                
                // 遍历所有格子B（从A后面开始找，避免重复比较）
                for (int x2 = x1; x2 < row; x2++)
                {
                    // 如果是同一行，列号从y1+1开始；如果是不同行，从0开始
                    int yStart = (x2 == x1) ? y1 + 1 : 0; 
                    for (int y2 = yStart; y2 < col; y2++) 
                    {
                        Tile b = tiles[x2, y2]; 
                        if (b == null || b.IsCleared) continue; 
                        // 如果类型一样且能连，说明还没死局，直接返回true
                        if (a.type == b.type && CanLink(a, b)) return true;
                    }
                }
            }
        }
        return false; // 找了一圈都没得连，死局了
    }

    // 检查死局并决定是否要洗牌
    void CheckDeadlockAndShuffleIfNeeded()
    {
        // 如果全图没有能连的了（HasLinkablePair返回false）
        if (!HasLinkablePair())
        {
            // 先看看是不是所有格子都消完了（是不是赢了）
            bool hasAlive = false;
            foreach (var t in tiles)
            {
                if (t != null && !t.IsCleared)
                {
                    hasAlive = true; // 只要还有一个活着的，就还没赢
                    break;
                }
            }

            // 如果没有活着的格子了
            if (!hasAlive) 
            {
                Debug.Log("[连连看] 恭喜！游戏胜利！");
                onGameWin?.Invoke(); // 触发胜利事件
                return; // 赢了就不用洗牌了
            }

            // 既然还没赢，又没得连，那就是死局（Deadlock）。需要洗牌。
            Debug.Log("[连连看] 检测到死局（没得消了），正在自动重新洗牌...");
            int guard = 0; // 防止无限循环的保险丝
            do
            {
                ShuffleBoard(); // 执行洗牌
                guard++;
            } while (!HasLinkablePair() && guard < 10); // 洗完牌还在死局就再洗，最多洗10次防止卡死
        }
    }
    
    // 洗牌逻辑
    void ShuffleBoard()
    {
        // 先把场上剩下的活着（没消掉）的格子收集起来
        var aliveTiles = new List<Tile>(); 
        var types = new List<int>();       

        foreach(var t in tiles)
        {
            if (t != null && !t.IsCleared)
            {
                aliveTiles.Add(t);  // 记录是哪个格子
                types.Add(t.type);  // 记录它的图案类型
            }
        }

        // 防御性检查：剩下的必须是偶数个
        if (types.Count % 2 != 0) return; 

        // 再次使用洗牌算法，打乱类型的顺序
        for (int i = 0; i < types.Count; i++)
        {
            int r = Random.Range(i, types.Count); 
            (types[i], types[r]) = (types[r], types[i]); 
        }

        // 把打乱后的类型重新分配给活着的格子
        for (int i = 0; i < aliveTiles.Count; i++)
        {
            Tile t = aliveTiles[i]; 
            int newType = types[i]; 
            t.type = newType; 
            t.image.sprite = tileSprites[newType]; 
            // 确保显示状态是正常的（防止之前点成灰色或者隐藏了）
            t.image.color = Color.white; 
            t.button.interactable = true;
        }
        // 洗牌后记得把选中的状态清空，不然之前的选择可能乱套
        firstTile = null; 
        secondTile = null;
    }

    // 协程：等待一会然后清除连线
    IEnumerator ClearLineWait()
    {
        // 暂停执行，等待 lineDuration 秒
        yield return new WaitForSeconds(lineDuration); 
        // 时间到了，告诉画线器把线清掉
        lineRenderer.Clear();
    }

    // 销毁时自动清理
    void OnDestroy()
    {
        // 停止所有正在运行的协程（比如消除连线的等待）
        StopAllCoroutines();
        
        // 清理引用，帮助GC回收
        tiles = null;
        firstTile = null;
        secondTile = null;
        tileSprites = null;
        
        // 注意：Resources.Load加载的资源一般不需要手动卸载，除非内存非常紧张需要调用 Resources.UnloadUnusedAssets()
        // 如果是AB包加载，这里需要调用相应的卸载接口
        // Debug.Log("[连连看] 游戏控制器已销毁，内存已清理。"); // 可选日志
    }
}
