using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人信息 数据类
/// </summary>
public class EnemyInfo
{
    // 敌人唯一ID
    public int id;

    // 预制体资源路径（或资源名）
    public string prefabRes;

    // 使用的动画控制器 / 状态机名（用于加载或选择动画）
    public string animator;

    // 攻击力
    public int atk;

    // 移动速度
    public float moveSpeed;

    // 旋转速度（角速度）
    public float rotateSpeed;

    // 生命值
    public int hp;

    // 攻击间隔 / 攻击偏移（秒）
    public float atkOffset;
}
