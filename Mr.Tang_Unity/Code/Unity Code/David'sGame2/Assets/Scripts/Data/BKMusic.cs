using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKMusic : MonoBehaviour
{
    private static BKMusic instance;
    public static BKMusic Instance => instance;
    private AudioSource bkAudio;
    void Awake()
    {
        instance = this;
        bkAudio = this.GetComponent<AudioSource>();

        // 初始化背景音乐状态
        bkAudio.mute = !GameDataMgr.Instance.musicData.isOpenMusic;
        bkAudio.volume = GameDataMgr.Instance.musicData.musicValue;
    }

    /// <summary>
    /// 提供给外部，设置背景音乐开关
    /// </summary>
    /// <param name="isOpen"></param>
    public void SetBKMusicIsOpen(bool isOpen)
    {
        bkAudio.mute = !isOpen;
    }

    /// <summary>
    /// 提供给外部，设置背景音乐音量
    /// </summary>
    /// <param name="value"></param>
    public void SetBKMusicValue(float value)
    {
        bkAudio.volume = value;
    }
}
