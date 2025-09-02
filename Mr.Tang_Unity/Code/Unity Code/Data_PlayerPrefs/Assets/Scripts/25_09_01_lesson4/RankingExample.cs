using UnityEngine;

/// <summary>
/// 排行榜系统使用示例
/// 演示如何在游戏中使用RankingSystem
/// </summary>
public class RankingExample : MonoBehaviour
{
    [Header("测试参数")]
    [SerializeField] private string testPlayerName = "测试玩家";
    [SerializeField] private int testScore = 1000;
    [SerializeField] private float testTime = 120.0f;

    void Start()
    {
        // 可以在这里进行初始化测试
        // TestBasicFunctions();
    }

    void Update()
    {
        // 快捷键测试功能
        if (Input.GetKeyDown(KeyCode.R))
        {
            // 按R键添加一条随机记录
            AddRandomRecord();
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            // 按T键显示排行榜
            ShowRanking();
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            // 按C键清除所有数据
            ClearAllData();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 按空格键运行完整测试
            RunFullTest();
        }
    }

    #region 基础功能示例

    /// <summary>
    /// 添加一条排行榜记录（游戏结束时调用）
    /// </summary>
    /// <param name="playerName">玩家名称</param>
    /// <param name="score">得分</param>
    /// <param name="clearTime">通关时间</param>
    public void AddGameRecord(string playerName, int score, float clearTime)
    {
        RankingSystem.Instance.AddRecord(playerName, score, clearTime);
        Debug.Log($"游戏记录已保存：{playerName} - {score}分 - {clearTime:F1}秒");
    }

    /// <summary>
    /// 显示当前排行榜
    /// </summary>
    public void ShowRanking()
    {
        Debug.Log("=== 当前排行榜 ===");
        RankingSystem.Instance.PrintRanking(RankingSystem.SortType.ScoreDescending, 10);
    }

    /// <summary>
    /// 显示指定玩家的记录
    /// </summary>
    /// <param name="playerName">玩家名称</param>
    public void ShowPlayerRecords(string playerName)
    {
        var records = RankingSystem.Instance.GetPlayerRecords(playerName);
        Debug.Log($"=== {playerName} 的游戏记录 ===");
        
        if (records.Count == 0)
        {
            Debug.Log("该玩家暂无游戏记录");
            return;
        }

        foreach (var record in records)
        {
            Debug.Log($"得分:{record.Score} | 用时:{record.FormattedTime}");
        }
    }

    /// <summary>
    /// 获取排行榜数据（用于UI显示）
    /// </summary>
    /// <returns>排行榜记录列表</returns>
    public System.Collections.Generic.List<RankingSystem.RankRecord> GetRankingData()
    {
        return RankingSystem.Instance.GetRanking(RankingSystem.SortType.ScoreDescending, 10);
    }

    #endregion

    #region 测试功能

    /// <summary>
    /// 基础功能测试
    /// </summary>
    [ContextMenu("测试基础功能")]
    public void TestBasicFunctions()
    {
        Debug.Log("=== 测试排行榜基础功能 ===");
        
        // 添加测试记录
        AddGameRecord(testPlayerName, testScore, testTime);
        
        // 显示排行榜
        ShowRanking();
        
        // 显示玩家记录
        ShowPlayerRecords(testPlayerName);
        
        // 获取记录总数
        int totalRecords = RankingSystem.Instance.GetRecordCount();
        Debug.Log($"排行榜总记录数：{totalRecords}");
    }

    /// <summary>
    /// 添加随机测试记录
    /// </summary>
    public void AddRandomRecord()
    {
        string[] names = { "玩家A", "玩家B", "玩家C", "高手", "新手", "大神" };
        string randomName = names[Random.Range(0, names.Length)];
        int randomScore = Random.Range(500, 2000);
        float randomTime = Random.Range(60f, 300f);
        
        AddGameRecord(randomName, randomScore, randomTime);
    }

    /// <summary>
    /// 清除所有数据
    /// </summary>
    public void ClearAllData()
    {
        RankingSystem.Instance.ClearAllRecords();
        Debug.Log("所有排行榜数据已清除");
    }

    /// <summary>
    /// 运行完整测试
    /// </summary>
    public void RunFullTest()
    {
        Debug.Log("=== 开始完整测试 ===");
        
        // 先清除旧数据
        RankingSystem.Instance.ClearAllRecords();
        
        // 添加多条测试记录
        RankingSystem.Instance.AddRecord("高分玩家", 1800, 90f);
        RankingSystem.Instance.AddRecord("速通玩家", 1200, 45f);
        RankingSystem.Instance.AddRecord("普通玩家", 800, 180f);
        RankingSystem.Instance.AddRecord("高分玩家", 1950, 95f);  // 同名玩家的另一次游戏
        RankingSystem.Instance.AddRecord("新手玩家", 400, 300f);
        
        // 按不同方式显示排行榜
        Debug.Log("\n=== 按得分排行 ===");
        RankingSystem.Instance.PrintRanking(RankingSystem.SortType.ScoreDescending, 5);
        
        Debug.Log("\n=== 按时间排行 ===");
        RankingSystem.Instance.PrintRanking(RankingSystem.SortType.TimeAscending, 5);
        
        // 查询指定玩家的最佳记录
        var bestScore = RankingSystem.Instance.GetPlayerBestScore("高分玩家");
        var bestTime = RankingSystem.Instance.GetPlayerBestTime("高分玩家");
        
        if (bestScore != null && bestTime != null)
        {
            Debug.Log($"\n高分玩家的最佳记录：最高分 {bestScore.Score}，最快时间 {bestTime.FormattedTime}");
        }
        
        Debug.Log("=== 测试完成 ===");
    }

    #endregion

    #region UI事件响应示例

    /// <summary>
    /// UI按钮事件：显示排行榜
    /// </summary>
    public void OnShowRankingButtonClick()
    {
        ShowRanking();
    }

    /// <summary>
    /// UI按钮事件：清除数据
    /// </summary>
    public void OnClearDataButtonClick()
    {
        ClearAllData();
    }

    /// <summary>
    /// UI按钮事件：添加测试数据
    /// </summary>
    public void OnAddTestDataButtonClick()
    {
        AddRandomRecord();
    }

    #endregion

    void OnGUI()
    {
        // 简单的GUI界面显示操作提示
        GUI.Label(new Rect(10, 10, 300, 20), "排行榜系统操作指南：");
        GUI.Label(new Rect(10, 30, 300, 20), "R键：添加随机记录");
        GUI.Label(new Rect(10, 50, 300, 20), "T键：显示排行榜");
        GUI.Label(new Rect(10, 70, 300, 20), "C键：清除所有数据");
        GUI.Label(new Rect(10, 90, 300, 20), "空格键：完整测试");
        
        int recordCount = RankingSystem.Instance.GetRecordCount();
        GUI.Label(new Rect(10, 120, 300, 20), $"当前记录数：{recordCount}");
    }
}
