using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RankItem : MonoBehaviour
{
    private UILabel labRanking;
    private UILabel labName;
    private UILabel labTime;

    void Awake()
    {
        labRanking = transform.Find("labRanking").GetComponent<UILabel>();
        labName = transform.Find("labName").GetComponent<UILabel>();
        labTime = transform.Find("labTime").GetComponent<UILabel>();
    }

    /// <summary>
    /// 根据排行单条数据对控件进行初始化 排名 名字 时间-x时x分x秒
    /// </summary>
    /// <param name="ranking"></param>
    /// <param name="name"></param>
    /// <param name="time"></param>
    public void InitInfo(int ranking, string name, float time)
    {
        labRanking.text = ranking.ToString();
        labName.text = name;
        TimeSpan span = TimeSpan.FromSeconds(time);
        labTime.text = $"{(int)span.TotalHours:D2}h{span.Minutes:D2}m{span.Seconds:D2}s";
    }

}
