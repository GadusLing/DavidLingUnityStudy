using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ServerInfo
{
    public SerializerDictionary<int, Server> servers = new SerializerDictionary<int, Server>();
}

public class Server
{
    [XmlAttribute]
    public int id; //服务器ID
    [XmlAttribute]
    public string serverName; //服务器名称
    [XmlAttribute]
    public int state; //0:不显示 1:流畅 2:繁华 3:火爆 4:维护
    [XmlAttribute]
    public bool isNew; //是否新服
}
