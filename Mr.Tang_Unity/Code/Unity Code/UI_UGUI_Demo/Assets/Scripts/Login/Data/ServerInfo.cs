using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerInfo
{
    /// <summary>
    /// 服务器ID
    /// </summary>
    public int id;   

    /// <summary>
    /// 服务器名称
    /// </summary>
    public string name;

    /// <summary>
    /// 0：默认不显示  1：流畅  2：繁忙  3：火爆  4：维护
    /// </summary>
    public int state;

    /// <summary>
    /// 是否为新服
    /// </summary>
    public bool isNew;


}
