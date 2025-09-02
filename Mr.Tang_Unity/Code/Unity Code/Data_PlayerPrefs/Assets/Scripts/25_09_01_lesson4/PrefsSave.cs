using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefsSave : MonoBehaviour
{
    /*
     * 题目1：
     * 将知识点一中的练习题，改为可以支持存储多个玩家信息
     */

    /*
     * 题目2：
     * 要在游戏中做一个排行榜功能
     * 排行榜主要记录玩家名（可重复），玩家得分，玩家通关的间
     * 请用PlayerPrefs存储读取排行榜相关信息
     */

    // 装备信息类
    [System.Serializable]
    public class EquipInfo
    {
        public int ID { get; set; }
        public int Num { get; set; }
    }

    // 玩家信息类 - 支持多个玩家存储
    [System.Serializable]
    public class PlayerInfo
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public List<EquipInfo> equipList = new List<EquipInfo>();
    }

    // 玩家数据管理器
    public class PlayerDataManager
    {
        private static PlayerDataManager instance;
        public static PlayerDataManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlayerDataManager();
                return instance;
            }
        }

        // 保存多个玩家信息
        public void SaveAllPlayers(List<PlayerInfo> players)
        {
            // 保存玩家数量
            PlayerPrefs.SetInt("PlayerCount", players.Count);

            for (int i = 0; i < players.Count; i++)
            {
                SavePlayer(players[i], i);
            }

            PlayerPrefs.Save();
            Debug.Log($"成功保存了 {players.Count} 个玩家的信息");
        }

        // 保存单个玩家信息（带索引）
        public void SavePlayer(PlayerInfo player, int playerIndex)
        {
            string prefix = "Player_" + playerIndex + "_";
            
            PlayerPrefs.SetString(prefix + "Name", player.Name);
            PlayerPrefs.SetInt(prefix + "Age", player.Age);
            PlayerPrefs.SetInt(prefix + "Attack", player.Attack);
            PlayerPrefs.SetInt(prefix + "Defense", player.Defense);

            // 保存装备信息
            PlayerPrefs.SetInt(prefix + "EquipCount", player.equipList.Count);
            for (int i = 0; i < player.equipList.Count; i++)
            {
                PlayerPrefs.SetInt(prefix + "EquipID_" + i, player.equipList[i].ID);
                PlayerPrefs.SetInt(prefix + "EquipNum_" + i, player.equipList[i].Num);
            }

            PlayerPrefs.Save();
            Debug.Log($"保存玩家 {player.Name} 的信息（索引：{playerIndex}）");
        }

        // 读取所有玩家信息
        public List<PlayerInfo> LoadAllPlayers()
        {
            List<PlayerInfo> players = new List<PlayerInfo>();
            int playerCount = PlayerPrefs.GetInt("PlayerCount", 0);

            for (int i = 0; i < playerCount; i++)
            {
                PlayerInfo player = LoadPlayer(i);
                if (player != null)
                {
                    players.Add(player);
                }
            }

            Debug.Log($"成功读取了 {players.Count} 个玩家的信息");
            return players;
        }

        // 读取单个玩家信息（按索引）
        public PlayerInfo LoadPlayer(int playerIndex)
        {
            string prefix = "Player_" + playerIndex + "_";
            
            // 检查该索引的玩家是否存在
            if (!PlayerPrefs.HasKey(prefix + "Name"))
            {
                return null;
            }

            PlayerInfo player = new PlayerInfo();
            player.Name = PlayerPrefs.GetString(prefix + "Name", "未命名");
            player.Age = PlayerPrefs.GetInt(prefix + "Age", 18);
            player.Attack = PlayerPrefs.GetInt(prefix + "Attack", 10);
            player.Defense = PlayerPrefs.GetInt(prefix + "Defense", 5);

            // 读取装备信息
            int equipCount = PlayerPrefs.GetInt(prefix + "EquipCount", 0);
            player.equipList = new List<EquipInfo>();
            
            for (int i = 0; i < equipCount; i++)
            {
                EquipInfo equip = new EquipInfo();
                equip.ID = PlayerPrefs.GetInt(prefix + "EquipID_" + i, 0);
                equip.Num = PlayerPrefs.GetInt(prefix + "EquipNum_" + i, 0);
                player.equipList.Add(equip);
            }

            return player;
        }

        // 按玩家名称查找玩家
        public PlayerInfo FindPlayerByName(string playerName)
        {
            List<PlayerInfo> allPlayers = LoadAllPlayers();
            foreach (var player in allPlayers)
            {
                if (player.Name == playerName)
                {
                    return player;
                }
            }
            return null;
        }

        // 添加新玩家
        public void AddPlayer(PlayerInfo newPlayer)
        {
            List<PlayerInfo> allPlayers = LoadAllPlayers();
            allPlayers.Add(newPlayer);
            SaveAllPlayers(allPlayers);
        }

        // 删除玩家（按索引）
        public void DeletePlayer(int playerIndex)
        {
            List<PlayerInfo> allPlayers = LoadAllPlayers();
            if (playerIndex >= 0 && playerIndex < allPlayers.Count)
            {
                allPlayers.RemoveAt(playerIndex);
                SaveAllPlayers(allPlayers);
                Debug.Log($"删除了索引为 {playerIndex} 的玩家");
            }
        }

        // 更新玩家信息（按索引）
        public void UpdatePlayer(int playerIndex, PlayerInfo updatedPlayer)
        {
            List<PlayerInfo> allPlayers = LoadAllPlayers();
            if (playerIndex >= 0 && playerIndex < allPlayers.Count)
            {
                allPlayers[playerIndex] = updatedPlayer;
                SaveAllPlayers(allPlayers);
                Debug.Log($"更新了索引为 {playerIndex} 的玩家信息");
            }
        }

        // 清除所有玩家数据
        public void ClearAllPlayers()
        {
            int playerCount = PlayerPrefs.GetInt("PlayerCount", 0);
            
            for (int i = 0; i < playerCount; i++)
            {
                string prefix = "Player_" + i + "_";
                PlayerPrefs.DeleteKey(prefix + "Name");
                PlayerPrefs.DeleteKey(prefix + "Age");
                PlayerPrefs.DeleteKey(prefix + "Attack");
                PlayerPrefs.DeleteKey(prefix + "Defense");
                
                int equipCount = PlayerPrefs.GetInt(prefix + "EquipCount", 0);
                for (int j = 0; j < equipCount; j++)
                {
                    PlayerPrefs.DeleteKey(prefix + "EquipID_" + j);
                    PlayerPrefs.DeleteKey(prefix + "EquipNum_" + j);
                }
                PlayerPrefs.DeleteKey(prefix + "EquipCount");
            }
            
            PlayerPrefs.DeleteKey("PlayerCount");
            PlayerPrefs.Save();
            Debug.Log("清除了所有玩家数据");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 测试多个玩家存储功能
        TestMultiPlayerStorage();
    }

    void TestMultiPlayerStorage()
    {
        Debug.Log("=== 开始测试多个玩家存储功能 ===");
        
        // 创建多个玩家
        List<PlayerInfo> players = new List<PlayerInfo>();

        // 玩家1
        PlayerInfo player1 = new PlayerInfo();
        player1.Name = "战士小王";
        player1.Age = 20;
        player1.Attack = 15;
        player1.Defense = 10;
        player1.equipList.Add(new EquipInfo { ID = 1001, Num = 1 }); // 长剑
        player1.equipList.Add(new EquipInfo { ID = 2001, Num = 1 }); // 盔甲
        players.Add(player1);

        // 玩家2
        PlayerInfo player2 = new PlayerInfo();
        player2.Name = "法师小李";
        player2.Age = 22;
        player2.Attack = 20;
        player2.Defense = 5;
        player2.equipList.Add(new EquipInfo { ID = 1002, Num = 1 }); // 法杖
        player2.equipList.Add(new EquipInfo { ID = 3001, Num = 3 }); // 药水
        players.Add(player2);

        // 玩家3
        PlayerInfo player3 = new PlayerInfo();
        player3.Name = "盗贼小张";
        player3.Age = 19;
        player3.Attack = 12;
        player3.Defense = 8;
        player3.equipList.Add(new EquipInfo { ID = 1003, Num = 2 }); // 双匕首
        players.Add(player3);

        // 保存所有玩家
        PlayerDataManager.Instance.SaveAllPlayers(players);

        // 读取并显示所有玩家信息
        Debug.Log("\n=== 读取所有玩家信息 ===");
        List<PlayerInfo> loadedPlayers = PlayerDataManager.Instance.LoadAllPlayers();
        
        for (int i = 0; i < loadedPlayers.Count; i++)
        {
            PlayerInfo player = loadedPlayers[i];
            Debug.Log($"玩家 {i}: {player.Name}, 年龄: {player.Age}, 攻击: {player.Attack}, 防御: {player.Defense}");
            Debug.Log($"装备数量: {player.equipList.Count}");
            foreach (var equip in player.equipList)
            {
                Debug.Log($"  装备ID: {equip.ID}, 数量: {equip.Num}");
            }
        }

        // 测试按名称查找玩家
        Debug.Log("\n=== 测试按名称查找玩家 ===");
        PlayerInfo foundPlayer = PlayerDataManager.Instance.FindPlayerByName("法师小李");
        if (foundPlayer != null)
        {
            Debug.Log($"找到玩家: {foundPlayer.Name}, 攻击力: {foundPlayer.Attack}");
        }

        // 测试添加新玩家
        Debug.Log("\n=== 测试添加新玩家 ===");
        PlayerInfo newPlayer = new PlayerInfo();
        newPlayer.Name = "弓箭手小刘";
        newPlayer.Age = 21;
        newPlayer.Attack = 14;
        newPlayer.Defense = 7;
        newPlayer.equipList.Add(new EquipInfo { ID = 1004, Num = 1 }); // 弓
        PlayerDataManager.Instance.AddPlayer(newPlayer);

        // 再次读取验证
        List<PlayerInfo> updatedPlayers = PlayerDataManager.Instance.LoadAllPlayers();
        Debug.Log($"添加新玩家后，总玩家数: {updatedPlayers.Count}");
    }

    // Update is called once per frame
    void Update()
    {
        // 测试快捷键
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("按下S键 - 重新测试存储");
            TestMultiPlayerStorage();
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("按下C键 - 清除所有数据");
            PlayerDataManager.Instance.ClearAllPlayers();
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("按下L键 - 读取所有玩家");
            List<PlayerInfo> players = PlayerDataManager.Instance.LoadAllPlayers();
            Debug.Log($"当前存储的玩家数量: {players.Count}");
        }
    }
}
