using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场景信息类
/// </summary>
public class SceneInfo
{
    /// <summary>
    /// 场景ID
    /// </summary>
    public int id;
    /// <summary>
    /// 场景图片资源
    /// </summary>
    public string imgRes;
    /// <summary>
    /// 场景名称
    /// </summary>
    public string name;
    /// <summary>
    /// 场景描述
    /// </summary>
    public string tips;
    /// <summary>
    /// 场景实例名称
    /// </summary>
    public string sceneObjName;
    /// <summary>
    /// 场景金币数量
    /// </summary>
    public int sceneGold;
    /// <summary>
    /// 场景初始主塔血量
    /// </summary>
    public int mainTowerHP;
}
