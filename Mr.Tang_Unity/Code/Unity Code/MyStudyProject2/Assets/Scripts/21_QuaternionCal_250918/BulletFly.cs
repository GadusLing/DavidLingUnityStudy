using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFly : MonoBehaviour
{
    public float speed = 20f;

    void Start()
    {
        Destroy(gameObject, 5f); // 5秒后销毁子弹
    }
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
