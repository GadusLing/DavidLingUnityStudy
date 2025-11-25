using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfo
{
    /// <summary>
    /// 唯一ID
    /// </summary>
    public int id;

    /// <summary>
    /// 塔的名称
    /// </summary>
    public string name;

    /// <summary>
    /// 建造花费（金币）
    /// </summary>
    public int cost;

    /// <summary>
    /// 攻击力
    /// </summary>
    public int atk;

    /// <summary>
    /// 射程（可为小数）
    /// </summary>
    public float atkRange;

    /// <summary>
    /// 攻击间隔（秒）
    /// </summary>
    public float atkInterval;

    /// <summary>
    /// 升级后对应的塔ID（0 表示无下一等级）
    /// </summary>
    public int nextLevelId;

    /// <summary>
    /// UI 显示的图片资源路径（Resources 下路径，不带扩展名）
    /// </summary>
    public string imgRes;

    /// <summary>
    /// 塔预制体资源路径（Resources 下路径，不带扩展名）
    /// </summary>
    public string prefabRes;

    /// <summary>
    /// 攻击类型（例如 1=单体、2=群体等，根据项目约定）
    /// </summary>
    public int atkType;

    /// <summary>
    /// 攻击特效资源路径（Resources 下路径，不带扩展名）
    /// </summary>
    public string atkEff;
}
