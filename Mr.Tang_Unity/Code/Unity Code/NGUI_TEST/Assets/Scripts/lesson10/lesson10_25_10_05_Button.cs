using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lesson10_25_10_05_Button : MonoBehaviour
{
    public Transform firePoint;
    public float speed = 20f;

    void Update()
    {

    }

    public void fire()
    {
        GameObject bullet = Instantiate(Resources.Load<GameObject>("Cartoon_Tank_Free/CTF_Prefabs/СTF_Missile_Red"), firePoint.position, firePoint.rotation);
        // 使用刚体速度让子弹持续飞行
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb == null) rb = bullet.AddComponent<Rigidbody>();
        rb.useGravity = false; // 如果不想受重力影响
        rb.velocity = firePoint.forward * speed;
        // 销毁子弹
        Destroy(bullet, 3f);
    }
}
