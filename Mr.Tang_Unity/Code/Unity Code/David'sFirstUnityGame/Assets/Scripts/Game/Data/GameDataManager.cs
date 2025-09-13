using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager
{
    private static GameDataManager instance;
    // 现代写法：使用 => 表达式体 和 ?? 空合并运算符
    public static GameDataManager Instance => instance ?? (instance = new GameDataManager());
    
    // 传统写法（老师教的经典写法）：
    // private static GameDataManager instance = new GameDataManager();
    // public static GameDataManager Instance { get => instance; }
    
    // 或者更传统的完整写法：
    // public static GameDataManager Instance
    // {
    //     get
    //     {            
    //         if (instance == null)
    //         {
    //             instance = new GameDataManager();
    //         }
    //         return instance;
    //     }
    // }

    public MusicData musicData;
    public RankInfoList rankData;

    private GameDataManager()
    {
        // 使用泛型方法获取数据，如果不存在则返回新的默认实例
        musicData = PlayerPrefsDataMgr.Instance.LoadData(typeof(MusicData), "Music") as MusicData;

        // 避免第一次进游戏时数据都是默认的false or 0
        if (musicData.notFirstPlay == false)
        {
            musicData.isMusicOn = true;
            musicData.isSoundOn = true;
            musicData.musicVolume = 1f;
            musicData.soundVolume = 1f;
            musicData.notFirstPlay = true;
            PlayerPrefsDataMgr.Instance.SaveData(musicData, "Music");
        }

        rankData = PlayerPrefsDataMgr.Instance.LoadData(typeof(RankInfoList), "Rank") as RankInfoList;
    }

    public void AddRankInfo(string name, int score, float time)
    {
        rankData.rankInfos.Add(new RankInfo(name, score, time));
        // 按分数从大到小排序
        rankData.rankInfos.Sort((a, b) => b.score.CompareTo(a.score));
        // 只保留前6名
        if (rankData.rankInfos.Count > 6)
        {
            rankData.rankInfos.RemoveRange(6, rankData.rankInfos.Count - 6);
        }
        PlayerPrefsDataMgr.Instance.SaveData(rankData, "Rank");
    }

    public void OnOffMusic(bool isOn)
    {
        musicData.isMusicOn = isOn;
        BGMandSound.Instance.OnOffMusic(isOn);
        PlayerPrefsDataMgr.Instance.SaveData(musicData, "Music");
    }
    public void OnOffSound(bool isOn)
    {
        musicData.isSoundOn = isOn;
        PlayerPrefsDataMgr.Instance.SaveData(musicData, "Music");
    }
    public void ChangeMusicVolume(float volume)
    {
        musicData.musicVolume = volume;
        BGMandSound.Instance.changeValue(volume);
        PlayerPrefsDataMgr.Instance.SaveData(musicData, "Music");
    }
    public void ChangeSoundVolume(float volume)
    {
        musicData.soundVolume = volume;
        PlayerPrefsDataMgr.Instance.SaveData(musicData, "Music");
    }
}

