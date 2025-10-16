using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : BasePanel<SettingPanel>
{
    private UIButton btnClose;
    private UISlider sliderMusic;
    private UISlider sliderSound;
    private UIToggle togMusic;
    private UIToggle togSound;


    public override void Init()
    {
        btnClose = transform.Find("btnClose").GetComponent<UIButton>();
        sliderMusic = transform.Find("sliderMusic").GetComponent<UISlider>();
        sliderSound = transform.Find("sliderSound").GetComponent<UISlider>();
        togMusic = transform.Find("togMusic").GetComponent<UIToggle>();
        togSound = transform.Find("togSound").GetComponent<UIToggle>();

        btnClose.onClick.Add(new EventDelegate(() =>
        {
            HideMe();
        }));

        sliderMusic.onChange.Add(new EventDelegate(() =>
        {
            GameDataMgr.Instance.SetMusicValue(sliderMusic.value);
            BKMusic.Instance.SetBKMusicValue(sliderMusic.value);
        }));

        sliderSound.onChange.Add(new EventDelegate(() =>
        {
            GameDataMgr.Instance.SetSoundValue(sliderSound.value);
        }));

        togMusic.onChange.Add(new EventDelegate(() =>
        {
            GameDataMgr.Instance.SetMusicIsOpen(togMusic.value);
            BKMusic.Instance.SetBKMusicIsOpen(togMusic.value);
        }));

        togSound.onChange.Add(new EventDelegate(() =>
        {
            GameDataMgr.Instance.SetSoundIsOpen(togSound.value);
        }));
        HideMe();
    }

    public override void ShowMe()
    {
        base.ShowMe();
        // 显示自己时根据上次保存的数据更新面板状态
        MusicData musicData = GameDataMgr.Instance.musicData;
        sliderMusic.value = musicData.musicValue;
        sliderSound.value = musicData.soundValue;
        togMusic.value = musicData.isOpenMusic;
        togSound.value = musicData.isOpenSound;
    }

    public override void HideMe()
    {
        GameDataMgr.Instance.SaveMusicData();
        // 隐藏自己时保存当前数据
        base.HideMe();
    }
}
