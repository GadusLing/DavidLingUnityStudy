using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKMusic : MonoBehaviour
{
    private static BKMusic _instance; // 继承自 MonoBehaviour 的单例不要new！初始化要在 Awake 里赋值
    public static BKMusic Instance => _instance;
    private AudioSource bkSource;

    void Awake()
    {
        _instance = this;
        bkSource = GetComponent<AudioSource>();

        // 通过本地数据来设置背景音乐状态
        SetIsOpen(GameDataMgr.Instance.musicData.musicOpen);
        ChangeVolume(GameDataMgr.Instance.musicData.musicVolume);
        GameDataMgr.Instance.SaveGameData<MusicData>();  // 保存当前音乐数据状态
    }

    /// <summary>
    /// 开关背景音乐的方法
    /// </summary>
    public void SetIsOpen(bool isOpen)
    {
        bkSource.mute = !isOpen;
    }
    
    /// <summary>
    /// 调整背景音乐音量的方法
    /// </summary>
    public void ChangeVolume(float volume)
    {
        bkSource.volume = volume;
    }
}
