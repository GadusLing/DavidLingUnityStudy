using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class RankData
{
    public List<RankInfo> rankList = new List<RankInfo>();
}

/// <summary>
/// 单条排行榜信息 名字 时间
/// </summary>
public class RankInfo
{
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public int time;

}
