using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏排行榜系统
/// 功能：存储和管理玩家的游戏记录（玩家名、得分、通关时间）
/// 使用PlayerPrefs进行数据持久化存储
/// 作者：Unity学习项目
/// 日期：2025-09-01
/// </summary>
public class RankingSystem : MonoBehaviour
{
    #region 排行榜数据结构

    /// <summary>
    /// 排行榜记录信息
    /// </summary>
    [System.Serializable]
    public class RankRecord
    {
        public string PlayerName { get; set; }    // 玩家名称（允许重复）
        public int Score { get; set; }            // 游戏得分
        public float ClearTime { get; set; }      // 通关时间（秒）
        public string FormattedTime { get; set; } // 格式化的时间显示

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="playerName">玩家名称</param>
        /// <param name="score">游戏得分</param>
        /// <param name="clearTime">通关时间（秒）</param>
        public RankRecord(string playerName, int score, float clearTime)
        {
            PlayerName = playerName;
            Score = score;
            ClearTime = clearTime;
            FormattedTime = FormatTime(clearTime);
        }

        /// <summary>
        /// 将秒数转换为 MM:SS 格式
        /// </summary>
        /// <param name="timeInSeconds">时间（秒）</param>
        /// <returns>格式化后的时间字符串</returns>
        private string FormatTime(float timeInSeconds)
        {
            //Mathf.FloorToInt(float f) 是 Unity 自带的方法，作用是 向下取整并转换成整数
            int minutes = Mathf.FloorToInt(timeInSeconds / 60);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60);
            return string.Format("{0:00}:{1:00}", minutes, seconds);
            //string.Format 会扫描第一个参数的字符串模板，比如 "{0:00}:{1:00}"，并用后面的参数替换对应位置的值
            //{n:格式}  "{0:00}" 就是告诉 C#：“拿第一个参数，把它格式化成两位数字，不够补 0” 然后用:把两个{}连起来
            //最后显示出来就会是03:05这种
        }
    }

    /// <summary>
    /// 排序方式枚举
    /// </summary>
    public enum SortType
    {
        ScoreDescending,    // 按得分降序（高分在前）
        ScoreAscending,     // 按得分升序（低分在前）  
        TimeAscending,      // 按时间升序（用时短在前）
        TimeDescending      // 按时间降序（用时长在前）
    }

    #endregion

    #region 单例模式

    private static RankingSystem instance;
    public static RankingSystem Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("RankingSystem");
                instance = go.AddComponent<RankingSystem>();
                DontDestroyOnLoad(go);
                //DontDestroyOnLoad(go) 的作用就是 让这个 GameObject 在场景切换时不被销毁
                //在 Unity 中，只要你做全局管理器/全局单例（比如怪物管理器、角色管理器、音效管理器、排行榜系统）
                //基本都会加这个东西，否则场景切换就会被销毁，单例就失效了
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            //instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region 常量定义

    private const string RANK_COUNT_KEY = "RankingSystem_Count";
    private const string RANK_PREFIX = "RankingSystem_";

    #endregion

    #region 核心功能方法

    /// <summary>
    /// 添加新的排行榜记录
    /// </summary>
    /// <param name="playerName">玩家名称</param>
    /// <param name="score">游戏得分</param>
    /// <param name="clearTime">通关时间（秒）</param>
    public void AddRecord(string playerName, int score, float clearTime)
    {
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("玩家名称不能为空！");
            return;
        }

        List<RankRecord> allRecords = LoadAllRecords();
        RankRecord newRecord = new RankRecord(playerName, score, clearTime);
        allRecords.Add(newRecord);
        
        SaveAllRecords(allRecords);
        
        Debug.Log($"排行榜记录已添加：{playerName} | 得分:{score} | 用时:{newRecord.FormattedTime}");
    }

    /// <summary>
    /// 获取排序后的排行榜
    /// </summary>
    /// <param name="sortType">排序方式</param>
    /// <param name="maxCount">最大返回数量，-1表示返回全部</param>
    /// <returns>排序后的排行榜记录列表</returns>
    public List<RankRecord> GetRanking(SortType sortType = SortType.ScoreDescending, int maxCount = 10)
    {
        List<RankRecord> records = LoadAllRecords();
        
        // 根据排序类型进行排序
        switch (sortType)
        {
            case SortType.ScoreDescending:
                records.Sort((a, b) => b.Score.CompareTo(a.Score));
                break;
            case SortType.ScoreAscending:
                records.Sort((a, b) => a.Score.CompareTo(b.Score));
                break;
            case SortType.TimeAscending:
                records.Sort((a, b) => a.ClearTime.CompareTo(b.ClearTime));
                break;
            case SortType.TimeDescending:
                records.Sort((a, b) => b.ClearTime.CompareTo(a.ClearTime));
                break;
        }

        // 限制返回数量
        if (maxCount > 0 && records.Count > maxCount)
        {
            records = records.GetRange(0, maxCount);
        }

        return records;
    }

    /// <summary>
    /// 获取指定玩家的所有记录
    /// </summary>
    /// <param name="playerName">玩家名称</param>
    /// <returns>该玩家的所有记录</returns>
    public List<RankRecord> GetPlayerRecords(string playerName)
    {
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("玩家名称不能为空！");
            return new List<RankRecord>();
        }

        List<RankRecord> allRecords = LoadAllRecords();
        List<RankRecord> playerRecords = new List<RankRecord>();

        foreach (var record in allRecords)
        {
            //第一个参数：要比较的字符串（这里是 playerName，传入的玩家名字）
            //第二个参数：比较方式 忽略大小写 来比较
            if (record.PlayerName.Equals(playerName, System.StringComparison.OrdinalIgnoreCase))
            {
                playerRecords.Add(record);
            }
        }

        return playerRecords;
    }

    /// <summary>
    /// 获取指定玩家的最高分记录
    /// </summary>
    /// <param name="playerName">玩家名称</param>
    /// <returns>最高分记录，如果没有记录则返回null</returns>
    public RankRecord GetPlayerBestScore(string playerName)
    {
        List<RankRecord> playerRecords = GetPlayerRecords(playerName);
        if (playerRecords.Count == 0) return null;

        RankRecord bestRecord = playerRecords[0];
        foreach (var record in playerRecords)
        {
            if (record.Score > bestRecord.Score)
            {
                bestRecord = record;
            }
        }
        return bestRecord;
    }

    /// <summary>
    /// 获取指定玩家的最快通关记录
    /// </summary>
    /// <param name="playerName">玩家名称</param>
    /// <returns>最快通关记录，如果没有记录则返回null</returns>
    public RankRecord GetPlayerBestTime(string playerName)
    {
        List<RankRecord> playerRecords = GetPlayerRecords(playerName);
        if (playerRecords.Count == 0) return null;

        RankRecord bestRecord = playerRecords[0];
        foreach (var record in playerRecords)
        {
            if (record.ClearTime < bestRecord.ClearTime)
            {
                bestRecord = record;
            }
        }
        return bestRecord;
    }

    /// <summary>
    /// 获取排行榜记录总数
    /// </summary>
    /// <returns>记录总数</returns>
    public int GetRecordCount()
    {
        return PlayerPrefs.GetInt(RANK_COUNT_KEY, 0);
    }

    /// <summary>
    /// 清除所有排行榜数据
    /// </summary>
    public void ClearAllRecords()
    {
        int recordCount = GetRecordCount();
        
        // 删除所有记录的键值
        for (int i = 0; i < recordCount; i++)
        {
            string prefix = RANK_PREFIX + i + "_";
            PlayerPrefs.DeleteKey(prefix + "PlayerName");
            PlayerPrefs.DeleteKey(prefix + "Score");
            PlayerPrefs.DeleteKey(prefix + "ClearTime");
        }
        
        // 删除计数器
        PlayerPrefs.DeleteKey(RANK_COUNT_KEY);
        PlayerPrefs.Save();
        
        Debug.Log("所有排行榜数据已清除");
    }

    #endregion

    #region 数据存储相关

    /// <summary>
    /// 保存所有记录到PlayerPrefs
    /// </summary>
    /// <param name="records">要保存的记录列表</param>
    private void SaveAllRecords(List<RankRecord> records)
    {
        // 保存记录数量
        PlayerPrefs.SetInt(RANK_COUNT_KEY, records.Count);

        // 保存每条记录
        for (int i = 0; i < records.Count; i++)
        {
            string prefix = RANK_PREFIX + i + "_";
            PlayerPrefs.SetString(prefix + "PlayerName", records[i].PlayerName);
            PlayerPrefs.SetInt(prefix + "Score", records[i].Score);
            PlayerPrefs.SetFloat(prefix + "ClearTime", records[i].ClearTime);
        }

        PlayerPrefs.Save();
    }

    /// <summary>
    /// 从PlayerPrefs加载所有记录
    /// </summary>
    /// <returns>加载的记录列表</returns>
    private List<RankRecord> LoadAllRecords()
    {
        List<RankRecord> records = new List<RankRecord>();
        int recordCount = GetRecordCount();

        for (int i = 0; i < recordCount; i++)
        {
            string prefix = RANK_PREFIX + i + "_";
            
            // 检查记录是否存在
            if (PlayerPrefs.HasKey(prefix + "PlayerName"))
            {
                string playerName = PlayerPrefs.GetString(prefix + "PlayerName", "");
                int score = PlayerPrefs.GetInt(prefix + "Score", 0);
                float clearTime = PlayerPrefs.GetFloat(prefix + "ClearTime", 0f);
                
                RankRecord record = new RankRecord(playerName, score, clearTime);
                records.Add(record);
            }
        }

        return records;
    }

    #endregion

    #region 调试和显示功能

    /// <summary>
    /// 在控制台显示排行榜
    /// </summary>
    /// <param name="sortType">排序方式</param>
    /// <param name="maxCount">显示数量</param>
    public void PrintRanking(SortType sortType = SortType.ScoreDescending, int maxCount = 10)
    {
        List<RankRecord> ranking = GetRanking(sortType, maxCount);
        
        string sortTypeName = GetSortTypeName(sortType);
        Debug.Log($"\n========== 排行榜 ({sortTypeName}) ==========");
        
        if (ranking.Count == 0)
        {
            Debug.Log("暂无排行榜数据");
            return;
        }

        for (int i = 0; i < ranking.Count; i++)
        {
            RankRecord record = ranking[i];
            Debug.Log($"第{i + 1}名: {record.PlayerName} | 得分:{record.Score} | 用时:{record.FormattedTime}");
        }
        Debug.Log("=======================================\n");
    }

    /// <summary>
    /// 获取排序类型的中文名称
    /// </summary>
    /// <param name="sortType">排序类型</param>
    /// <returns>中文名称</returns>
    private string GetSortTypeName(SortType sortType)
    {
        switch (sortType)
        {
            case SortType.ScoreDescending: return "按得分降序";
            case SortType.ScoreAscending: return "按得分升序";
            case SortType.TimeAscending: return "按用时升序";
            case SortType.TimeDescending: return "按用时降序";
            default: return "未知排序";
        }
    }

    #endregion

    #region 测试功能

    /// <summary>
    /// 测试排行榜功能
    /// </summary>
    [ContextMenu("测试排行榜功能")]
    public void TestRankingSystem()
    {
        Debug.Log("=== 开始测试排行榜系统 ===");

        // 添加测试数据
        AddRecord("张三", 1200, 125.5f);    // 2分05秒
        AddRecord("李四", 980, 98.2f);      // 1分38秒
        AddRecord("王五", 1500, 156.8f);    // 2分36秒
        AddRecord("张三", 1350, 102.3f);    // 张三的另一次游戏
        AddRecord("赵六", 890, 89.1f);      // 1分29秒
        AddRecord("钱七", 1100, 134.7f);    // 2分14秒

        // 显示不同排序的排行榜
        PrintRanking(SortType.ScoreDescending, 5);
        PrintRanking(SortType.TimeAscending, 5);

        // 查询指定玩家记录
        Debug.Log("=== 张三的所有记录 ===");
        List<RankRecord> zhangSanRecords = GetPlayerRecords("张三");
        foreach (var record in zhangSanRecords)
        {
            Debug.Log($"张三 - 得分:{record.Score} | 用时:{record.FormattedTime}");
        }

        // 查询最佳记录
        Debug.Log("=== 各玩家最佳记录 ===");
        string[] playerNames = { "张三", "李四", "王五" };
        foreach (string playerName in playerNames)
        {
            RankRecord bestScore = GetPlayerBestScore(playerName);
            RankRecord bestTime = GetPlayerBestTime(playerName);
            
            if (bestScore != null && bestTime != null)
            {
                Debug.Log($"{playerName} - 最高分:{bestScore.Score} | 最快用时:{bestTime.FormattedTime}");
            }
        }

        Debug.Log($"当前排行榜总记录数: {GetRecordCount()}");
    }

    /// <summary>
    /// 清除测试数据
    /// </summary>
    [ContextMenu("清除排行榜数据")]
    public void ClearTestData()
    {
        ClearAllRecords();
    }

    #endregion
}
