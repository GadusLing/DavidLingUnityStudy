using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleandRotation : MonoBehaviour
{
    // 第一题：
    // 使用你之前创建的坦克预设体，在坦克下面加一个底座（用自带几何体即可）
    // 让其可以原地旋转，类似一个展览台

    // 第二题：
    // 在第一题的基础上，让坦克的炮台可以自动左右来回旋转，炮管可以自动上下抬起

    // 第三题：
    // 请用3个球体，模拟太阳、地球、月球之间的旋转移动

    public GameObject Tank;
    public GameObject TankBase;
    public float rotateSpeed = 30;
    public float turretRotateSpeed = 30;
    public float barrelRotateSpeed = 30;
    public Transform Turret; // 炮台
    public Transform Barrel; // 炮管

    // 炮台左右最大旋转角度（正负45度）
    public float turretMaxAngle = 45f;

    // 炮管上下最大仰角和俯角
    public float barrelMaxUpAngle = -10f; // 最大俯角
    public float barrelMaxDownAngle = 10f;   // 最大仰角


    // 记录当前炮台旋转方向（1为右转，-1为左转）
    private int turretDirection = 1;
    private float currentBarrelAngle = 0f; // 记录当前炮管仰角
    private int barrelDirection = -1;      // -1为上抬，1为下压

    void Start()
    {

    }

    void Update()
    {
        // 让底座围绕Y轴旋转，坦克会跟着一起转
        if (TankBase != null)
            TankBase.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        // 炮台自动左右来回旋转
        AutoRotateTurret();

        // 炮管自动上下抬起
        AutoRotateBarrel();
    }

    /// <summary>
    /// 炮台自动左右来回旋转（-turretMaxAngle ~ turretMaxAngle）
    /// </summary>
    void AutoRotateTurret()
    {
        if (Turret == null) return;

        // 以父物体为参考，获取本地欧拉角
        float y = Turret.localEulerAngles.y;
        // 修正角度到-180~180
        if (y > 180) y -= 360;

        // 判断是否到达最大角度，改变旋转方向
        if (y >= turretMaxAngle && turretDirection > 0)
            turretDirection = -1;
        else if (y <= -turretMaxAngle && turretDirection < 0)
            turretDirection = 1;

        // 按当前方向旋转
        Turret.Rotate(Vector3.up * turretRotateSpeed * turretDirection * Time.deltaTime);
    }

    /// <summary>
    /// 炮管自动上下抬起（barrelMaxDownAngle ~ barrelMaxUpAngle）
    /// </summary>
    void AutoRotateBarrel()
    {
        if (Barrel == null) return;

        // 每帧累加仰角
        currentBarrelAngle += barrelRotateSpeed * barrelDirection * Time.deltaTime;

        // 限制仰角范围
        currentBarrelAngle = Mathf.Clamp(currentBarrelAngle, barrelMaxDownAngle, barrelMaxUpAngle);

        // 判断是否到达限位，改变方向
        if (currentBarrelAngle <= barrelMaxDownAngle && barrelDirection == -1)
            barrelDirection = 1;
        else if (currentBarrelAngle >= barrelMaxUpAngle && barrelDirection == 1)
            barrelDirection = -1;

        // 设置炮管本地旋转（只动x轴）
        Barrel.localRotation = Quaternion.Euler(currentBarrelAngle, 0, 0);
    }
}

