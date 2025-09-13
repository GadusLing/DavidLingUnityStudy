using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingsPanel : BasePanel<RankingsPanel>
{
    public CustomGUIButton ButtonClose;
    //控件特别多的情况下，一个一个手拖显然是愚蠢的选择，可以代码配合规律性的命名来获得
    private List<CustomGUILabel> nameLabels = new List<CustomGUILabel>();
    private List<CustomGUILabel> scoreLabels = new List<CustomGUILabel>();
    private List<CustomGUILabel> timeLabels = new List<CustomGUILabel>();

    void Start()
    {
        ButtonClose.clickEvent += () =>
        {
            HidePanel();
            BeginPanel.Instance.ShowPanel();
        };

        //通过代码获取一系列命名有规律的控件
        for (int i = 1; i <= 6; i++)
        {
            var nameLabel = this.transform.Find("Name/NameLabel" + i).GetComponent<CustomGUILabel>();
            nameLabels.Add(nameLabel);
            var scoreLabel = this.transform.Find("Score/ScoreLabel" + i).GetComponent<CustomGUILabel>();
            scoreLabels.Add(scoreLabel);
            var timeLabel = this.transform.Find("Time/TimeLabel" + i).GetComponent<CustomGUILabel>();
            timeLabels.Add(timeLabel);
        }

        //GameDataManager.Instance.AddRankInfo("测试数据", 1000, 65.5f);

        HidePanel();
    }

    public void UpdatePanelInfo()
    {
        var rankInfoList = GameDataManager.Instance.rankData.rankInfos;
        for (int i = 0; i < rankInfoList.Count; i++)
        {
            nameLabels[i].guiContent.text = rankInfoList[i].name;
            scoreLabels[i].guiContent.text = rankInfoList[i].score.ToString();
            int time = (int)rankInfoList[i].time;
            int hour = time / 3600;
            int minute = (time % 3600) / 60;    
            int second = time % 60;
            timeLabels[i].guiContent.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
        }

    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        UpdatePanelInfo();
    }


}
