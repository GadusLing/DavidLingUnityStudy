using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// 开火点数据集合
/// </summary>
public class FireData
{
    public List<FireInfo> fireList = new List<FireInfo>();
}

public class FireInfo
{
    [XmlAttribute]
    public int id; // 开火点唯一ID 方便配置的时候查看数据
    [XmlAttribute]
    public int type; // 开火类型 1顺序 2散射 
    [XmlAttribute]
    public int num; // 数量 该组子弹有多少颗
    [XmlAttribute]
    public float cd; // 每颗子弹的间隔时间
    [XmlAttribute]
    public string ids; // 关联的子弹ID 比如1，10 代表在1-10ID的子弹中随机选择
    [XmlAttribute]
    public float delay; // 每组子弹组间间隔时间
}
