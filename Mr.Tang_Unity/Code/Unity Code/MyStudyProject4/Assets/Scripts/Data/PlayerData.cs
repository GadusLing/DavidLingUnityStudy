using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家数据
/// </summary>
public class PlayerData
{
    public int gold = 200;
    public List<int> ownedRoleIds = new List<int>() { 1, 2 }; // 默认拥有角色ID 1 2
}
