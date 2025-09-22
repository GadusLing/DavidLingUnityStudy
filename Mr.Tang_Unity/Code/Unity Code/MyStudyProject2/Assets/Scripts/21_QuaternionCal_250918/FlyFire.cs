using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using UnityEngine;

// 一、用目前所学知识，模拟飞机发射不同类型子弹的方法
// 单发，双发，扇形，环形

// 二、用所学3D数学知识实现摄像机跟随效果
// 1.摄像机在人物斜后方，通过角度控制倾斜率
// 2.通过鼠标滚轮可以控制摄像机距离人物的距离（有最大最小限制）
// 3.摄像机看向人物头顶上方一个位置（可调节）
// 4.Vector3.Lerp实现相机跟随人物
// 5.Quaternion.Slerp实现摄像机朝向过渡效果


enum E_FireMode
{
    Single, // 单发
    Double, // 双弹
    Fan,    // 扇形
    Ring    // 环形 
}
public class FlyFire : MonoBehaviour
{
    public float moveSpeed = 10f; // 飞机移动速度
    private E_FireMode currentFireMode = E_FireMode.Single; // 当前发射模式
    public GameObject firepoint; // 子弹发射点
    private GameObject bullet;

    public float fireRate = 0.2f; // 攻击间隔（秒）
    private float lastFireTime = 0f; // 上次攻击时间

    void Start()
    {
        bullet = Resources.Load<GameObject>("Bullet");
    }

    void LateUpdate()
    {
        
    }

    void Update()
    {
        Move();
        ChangeFireMode();
        Fire();
    }

    public void Move()
    {
        float speed = moveSpeed;

        // 按住左Shift加速
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            speed *= 2f;
        }

        // 获取输入
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical");     // W/S

        // 左右移动（x轴），上下移动（y轴）        
        Vector3 moveDir = transform.forward + transform.right * horizontal + transform.up * vertical;

        // 移动
        transform.position += moveDir.normalized * speed * Time.deltaTime;
    }

    public void ChangeFireMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentFireMode = E_FireMode.Single;
            UnityEngine.Debug.Log("单发开火模式");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentFireMode = E_FireMode.Double;
            UnityEngine.Debug.Log("双发开火模式");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentFireMode = E_FireMode.Fan;
            UnityEngine.Debug.Log("扇形开火模式");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentFireMode = E_FireMode.Ring;
            UnityEngine.Debug.Log("环形开火模式");
        }
    }

    public void Fire()
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            // 检查是否可以攻击
            if (Time.time - lastFireTime >= fireRate)
            {
                switch(currentFireMode)
                {
                    case E_FireMode.Single:
                        Instantiate(bullet, firepoint.transform.position, firepoint.transform.rotation);
                        break;
                    case E_FireMode.Double:
                        Instantiate(bullet, firepoint.transform.position + firepoint.transform.right * 0.5f, firepoint.transform.rotation); // 右侧
                        Instantiate(bullet, firepoint.transform.position - firepoint.transform.right * 0.5f, firepoint.transform.rotation); // 左侧
                        // 双发
                        break;
                    case E_FireMode.Fan:
                        Instantiate(bullet, firepoint.transform.position, firepoint.transform.rotation);
                        Instantiate(bullet, firepoint.transform.position, firepoint.transform.rotation * Quaternion.AngleAxis(15, Vector3.up));
                        Instantiate(bullet, firepoint.transform.position, firepoint.transform.rotation * Quaternion.Euler(0, -15, 0));
                        // 扇形
                        break;
                    case E_FireMode.Ring:
                        for(int i = 0; i < 8; i++)
                        {
                            Instantiate(bullet, firepoint.transform.position, firepoint.transform.rotation * Quaternion.AngleAxis(i * 45, Vector3.up));
                        }
                        break;
                }
                lastFireTime = Time.time;
            }
        }
    }
}
