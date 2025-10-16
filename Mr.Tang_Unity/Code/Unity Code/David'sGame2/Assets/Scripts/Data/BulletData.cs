using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// 子弹数据集合
/// </summary>
public class BulletData
{
    public List<BulletInfo> bulletList = new List<BulletInfo>();
}

public class BulletInfo
{
    [XmlAttribute]
    public int id; // 子弹唯一ID 方便配置的时候查看数据
    [XmlAttribute]
    public int type; // 子弹移动规则 1-5代表5种移动规则
    [XmlAttribute]
    public string resName; // 子弹资源路径
    [XmlAttribute]
    public string deadEffect; // 子弹销毁特效路径
    [XmlAttribute]
    public float forwardSpeed; // 子弹朝向速度
    [XmlAttribute]
    public float rightSpeed; // 子弹横向速度，做曲线移动时需要
    [XmlAttribute]
    public float rotateSpeed; // 子弹旋转速度，做跟踪子弹时需要
    [XmlAttribute]
    public float lifeTime; // 子弹存活时间
}
