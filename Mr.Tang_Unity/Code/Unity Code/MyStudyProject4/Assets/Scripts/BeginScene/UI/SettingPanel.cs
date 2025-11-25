using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Button btnClose;
    public Toggle togMusic;
    public Toggle togSound;
    public Slider sliderMusic;
    public Slider sliderSound;
    public override void Init()
    {
        // 初始化面板显示内容 根据本地数据设置UI状态
        var musicData = GameDataMgr.Instance.musicData;
        togMusic.isOn = musicData.musicOpen;
        togSound.isOn = musicData.soundOpen;
        sliderMusic.value = musicData.musicVolume;
        sliderSound.value = musicData.soundVolume;

        // 关闭设置面板
        btnClose.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<SettingPanel>();
            // 关闭面板后保存音乐数据状态
            GameDataMgr.Instance.SaveGameData<MusicData>();
        });

        // 设置BGM开关
        togMusic.onValueChanged.AddListener((isOn) =>
        {
            BKMusic.Instance.SetIsOpen(isOn);
            GameDataMgr.Instance.musicData.musicOpen = isOn;
        });

        // 设置音效开关
        togSound.onValueChanged.AddListener((isOn) =>
        {
            GameDataMgr.Instance.musicData.soundOpen = isOn;
        });

        // 设置BGM音量
        sliderMusic.onValueChanged.AddListener((value) =>
        {
            BKMusic.Instance.ChangeVolume(value);
            GameDataMgr.Instance.musicData.musicVolume = value;
        });

        // 设置音效音量
        sliderSound.onValueChanged.AddListener((value) =>
        {
            GameDataMgr.Instance.musicData.soundVolume = value;
        });
    }
}
