using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button btnStart;
    public Button btnSetting;
    public Button btnAbout;
    public Button btnQuit;
    public override void Init()
    {
        btnStart.onClick.AddListener(() =>
        {
            // 播放摄像机左转动画
            Camera.main.GetComponent<CameraAnimator>().TurnLeft(() =>
            {
                // 显示选角面板
                UIManager.Instance.ShowPanel<ChooseHeroPanel>();
            });

            // 隐藏当前面板
            UIManager.Instance.HidePanel<BeginPanel>();
        });

        btnSetting.onClick.AddListener(() =>
        {
            // 显示设置面板
            UIManager.Instance.ShowPanel<SettingPanel>();
        });

        btnAbout.onClick.AddListener(() =>
        {
            // 显示关于面板
            //UIManager.Instance.ShowPanel<AboutPanel>();
        });

        btnQuit.onClick.AddListener(() =>
        {
            // 退出游戏
            Application.Quit();
        });
    }
}
