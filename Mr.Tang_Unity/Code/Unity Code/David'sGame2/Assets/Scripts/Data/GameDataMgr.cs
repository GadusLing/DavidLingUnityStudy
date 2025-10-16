using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMgr
{
    private static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance => instance;
    public MusicData musicData;// 音乐音效数据
    public RankData rankData; // 排行榜数据
    public RoleData roleData; // 角色数据
    public BulletData bulletData; // 子弹数据
    public FireData fireData; // 开火点数据
    public int currentRoleIndex = 0; // 当前选择的角色索引
    private GameDataMgr()
    {
        // 通过XML文件加载音乐音效数据
        musicData = XmlDataMgr.Instance.LoadData(typeof(MusicData), "MusicData") as MusicData;
        // 通过XML文件加载排行榜数据
        rankData = XmlDataMgr.Instance.LoadData(typeof(RankData), "RankData") as RankData;
        // 通过XML文件加载角色数据
        roleData = XmlDataMgr.Instance.LoadData(typeof(RoleData), "RoleData") as RoleData;
        // 通过XML文件加载子弹数据
        bulletData = XmlDataMgr.Instance.LoadData(typeof(BulletData), "BulletData") as BulletData;
        // 通过XML文件加载开火点数据
        fireData = XmlDataMgr.Instance.LoadData(typeof(FireData), "FireData") as FireData;
    }

    #region MusicData音乐音效数据相关函数
    /// <summary>
    /// 保存音乐音效数据
    /// </summary>
    public void SaveMusicData()
    {
        XmlDataMgr.Instance.SaveData(musicData, "MusicData");
    }

    /// <summary>
    /// 提供给外部，设置音乐开关
    /// </summary>
    /// <param name="isOpen"></param>
    public void SetMusicIsOpen(bool isOpen)
    {
        // 记录当前音乐开关状态数据
        musicData.isOpenMusic = isOpen;
        // 改变音乐开关

    }

    /// <summary>
    /// 提供给外部，设置音效开关
    /// </summary>
    /// <param name="isOpen"></param>
    public void SetSoundIsOpen(bool isOpen)
    {
        // 记录当前音效开关状态数据
        musicData.isOpenSound = isOpen;
        // 改变音效开关

    }

    public void SetMusicValue(float value)
    {
        // 记录当前音乐音量数据
        musicData.musicValue = value;
        // 改变音乐音量

    }

    public void SetSoundValue(float value)
    {
        // 记录当前音效音量数据
        musicData.soundValue = value;
        // 改变音效音量

    }
    #endregion

    #region RankData排行榜数据相关函数
    /// <summary>
    /// 保存排行榜数据 (添加一条新的数据，并根据时间排序，只保留前20名)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="time"></param>
    public void SaveRankData(string name, int time)
    {
        // 添加一条新的排行榜数据
        RankInfo rankInfo = new RankInfo();
        rankInfo.name = name;
        rankInfo.time = time;
        rankData.rankList.Add(rankInfo);
        // 按照时间排序
        rankData.rankList.Sort((a, b) => b.time.CompareTo(a.time));// 时间越大排名越靠前
        // 只保留前20名
        if (rankData.rankList.Count > 20)
        {
            //第一个参数是起始下标（从第 20 个元素开始），第二个参数是要移除的元素数量（总数减去 20 个，等于删除第 20 个之后的所有元素）。
            rankData.rankList.RemoveRange(20, rankData.rankList.Count - 20);
        }
        // 保存到XML文件
        XmlDataMgr.Instance.SaveData(rankData, "RankData");
    }

    #endregion

    #region RoleData角色数据相关函数
    public RoleInfo GetCurrentRoleInfo()
    {
        return roleData.roleList[currentRoleIndex];
    }

    #endregion

    #region BulletData子弹数据相关函数

    #endregion

    #region FireData开火点数据相关函数

    #endregion
}
