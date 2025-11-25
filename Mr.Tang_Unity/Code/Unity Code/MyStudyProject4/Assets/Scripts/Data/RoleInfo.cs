using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class RoleInfo
{
    /// <summary>角色ID</summary>
    public int id;

    /// <summary>角色Prefab路径</summary>
    public string PrefabRes;

    /// <summary>攻击力</summary>
    public int atk;

    /// <summary>角色描述Tips</summary>
    public string tips;

    /// <summary>角色解锁价格</summary>
    public int lockMoney;

    /// <summary>角色攻击检测类型 1.近战用范围检测 2.远程用射线检测</summary>
    public int type;

    /// <summary>攻击特效路径</summary>
    public string hitEff;
}