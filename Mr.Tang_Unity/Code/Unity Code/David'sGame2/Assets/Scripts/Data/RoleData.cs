using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Xml.Serialization;

public class RoleData
{
    public List<RoleInfo> roleList = new List<RoleInfo>();
}

public class RoleInfo
{
    [XmlAttribute]
    public string resName; // 模型资源路径
    [XmlAttribute]
    public int HP;
    [XmlAttribute]
    public int speed;
    [XmlAttribute]
    public int volume;
    [XmlAttribute]
    public float scale; // 选角面板的模型缩放参数
}
