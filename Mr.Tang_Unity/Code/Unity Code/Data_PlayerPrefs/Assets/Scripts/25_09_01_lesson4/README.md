# Unity 排行榜系统 (RankingSystem)

## 📖 简介
这是一个基于Unity和PlayerPrefs的独立排行榜系统，可以轻松集成到任何Unity项目中。支持存储玩家姓名、得分和通关时间，并提供多种排序和查询功能。

## ✨ 主要特性
- 🏆 支持存储多个玩家的游戏记录
- 📊 支持按得分或通关时间排序
- 🔍 支持按玩家名查询记录
- 💾 使用PlayerPrefs进行数据持久化
- 🎮 提供完整的API接口
- 🧪 内置测试功能

## 📁 文件结构
```
25_09_01_lesson4/
├── RankingSystem.cs      # 核心排行榜系统
├── RankingExample.cs     # 使用示例和测试
└── README.md            # 说明文档
```

## 🚀 快速开始

### 1. 基本使用
```csharp
// 添加一条游戏记录
RankingSystem.Instance.AddRecord("玩家名", 1500, 120.5f);

// 获取排行榜（按得分降序，前10名）
var ranking = RankingSystem.Instance.GetRanking();

// 显示排行榜到控制台
RankingSystem.Instance.PrintRanking();
```

### 2. 高级查询
```csharp
// 获取指定玩家的所有记录
var playerRecords = RankingSystem.Instance.GetPlayerRecords("玩家名");

// 获取玩家最高分记录
var bestScore = RankingSystem.Instance.GetPlayerBestScore("玩家名");

// 获取玩家最快通关记录
var bestTime = RankingSystem.Instance.GetPlayerBestTime("玩家名");
```

### 3. 不同排序方式
```csharp
// 按得分降序（默认）
var topScores = RankingSystem.Instance.GetRanking(RankingSystem.SortType.ScoreDescending);

// 按通关时间升序（速通榜）
var speedRun = RankingSystem.Instance.GetRanking(RankingSystem.SortType.TimeAscending);
```

## 🎮 使用示例

### 游戏结束时添加记录
```csharp
public class GameManager : MonoBehaviour
{
    public void OnGameComplete(string playerName, int finalScore, float playTime)
    {
        // 游戏结束时添加到排行榜
        RankingSystem.Instance.AddRecord(playerName, finalScore, playTime);
        
        // 显示当前排行榜
        RankingSystem.Instance.PrintRanking(RankingSystem.SortType.ScoreDescending, 5);
    }
}
```

### 在UI中显示排行榜
```csharp
public class RankingUI : MonoBehaviour
{
    public void UpdateRankingDisplay()
    {
        var rankings = RankingSystem.Instance.GetRanking(RankingSystem.SortType.ScoreDescending, 10);
        
        for (int i = 0; i < rankings.Count; i++)
        {
            var record = rankings[i];
            Debug.Log($"第{i+1}名: {record.PlayerName} - {record.Score}分 - {record.FormattedTime}");
        }
    }
}
```

## 🔧 API 参考

### 核心方法
| 方法 | 描述 |
|------|------|
| `AddRecord(string, int, float)` | 添加新的游戏记录 |
| `GetRanking(SortType, int)` | 获取排序后的排行榜 |
| `GetPlayerRecords(string)` | 获取指定玩家的所有记录 |
| `GetPlayerBestScore(string)` | 获取玩家最高分记录 |
| `GetPlayerBestTime(string)` | 获取玩家最快通关记录 |
| `GetRecordCount()` | 获取记录总数 |
| `ClearAllRecords()` | 清除所有记录 |
| `PrintRanking(SortType, int)` | 在控制台显示排行榜 |

### 排序类型
| 类型 | 描述 |
|------|------|
| `ScoreDescending` | 按得分降序（高分在前） |
| `ScoreAscending` | 按得分升序（低分在前） |
| `TimeAscending` | 按时间升序（用时短在前） |
| `TimeDescending` | 按时间降序（用时长在前） |

### 数据结构
```csharp
public class RankRecord
{
    public string PlayerName { get; set; }      // 玩家名称
    public int Score { get; set; }              // 游戏得分
    public float ClearTime { get; set; }        // 通关时间（秒）
    public string FormattedTime { get; set; }   // 格式化时间 (MM:SS)
}
```

## 🧪 测试功能

### 内置测试
- 将 `RankingExample.cs` 脚本添加到场景中的GameObject
- 运行游戏后使用以下快捷键：
  - `R键`：添加随机记录
  - `T键`：显示排行榜
  - `C键`：清除所有数据
  - `空格键`：运行完整测试

### Inspector测试
- 在 `RankingSystem` 组件的右键菜单中选择：
  - "测试排行榜功能"：运行完整测试
  - "清除排行榜数据"：清除所有数据

## 💾 数据存储

### PlayerPrefs键值格式
```
RankingSystem_Count              # 记录总数
RankingSystem_0_PlayerName       # 第0条记录的玩家名
RankingSystem_0_Score           # 第0条记录的得分
RankingSystem_0_ClearTime       # 第0条记录的通关时间
...
```

### 数据持久化
- 数据自动保存到PlayerPrefs，支持跨会话持久化
- 支持Windows Registry、Mac Plist、Linux配置文件等不同平台
- 调用 `PlayerPrefs.Save()` 确保数据立即写入

## 🔄 迁移到其他项目

### 简单迁移
1. 复制 `RankingSystem.cs` 到目标项目
2. 在代码中使用 `RankingSystem.Instance` 调用功能
3. 可选：复制 `RankingExample.cs` 作为使用参考

### 自定义扩展
```csharp
// 扩展记录结构
public class CustomRankRecord : RankingSystem.RankRecord
{
    public int Level { get; set; }        // 关卡等级
    public string Difficulty { get; set; } // 难度
    
    public CustomRankRecord(string name, int score, float time, int level, string difficulty) 
        : base(name, score, time)
    {
        Level = level;
        Difficulty = difficulty;
    }
}
```

## ⚠️ 注意事项
1. PlayerPrefs在不同平台有存储限制，建议控制记录数量
2. 玩家名称支持重复，按需求处理重名逻辑
3. 时间以秒为单位存储，显示时自动格式化为MM:SS
4. 系统使用单例模式，确保数据一致性

## 🎯 适用场景
- 游戏排行榜
- 竞速游戏计时
- 得分统计
- 玩家记录追踪
- 成就系统辅助

---
**作者：Unity学习项目**  
**日期：2025-09-01**  
**版本：1.0.0**
