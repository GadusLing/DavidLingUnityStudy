using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginPanel : BasePanel<BeginPanel>
{
    private UIButton btnStart;
    private UIButton btnRank;
    private UIButton btnSetting;
    private UIButton btnQuit;
    public override void Init()
    {
        btnStart = transform.Find("btnStart").GetComponent<UIButton>();
        btnRank = transform.Find("btnRank").GetComponent<UIButton>();
        btnSetting = transform.Find("btnSetting").GetComponent<UIButton>();
        btnQuit = transform.Find("btnQuit").GetComponent<UIButton>();

        btnStart.onClick.Add(new EventDelegate(() =>
        {
            // 显示选角面板
            ChoosePanel.Instance.ShowMe();
            // 隐藏自己
            HideMe();
        }));

        btnRank.onClick.Add(new EventDelegate(() =>
        {
            // 显示排行榜面板
            RankPanel.Instance.ShowMe();
        }));

        btnSetting.onClick.Add(new EventDelegate(() =>
        {
            // 显示设置面板
            SettingPanel.Instance.ShowMe();

        }));

        btnQuit.onClick.Add(new EventDelegate(() =>
        {
            Application.Quit();
        }));
    }

}
