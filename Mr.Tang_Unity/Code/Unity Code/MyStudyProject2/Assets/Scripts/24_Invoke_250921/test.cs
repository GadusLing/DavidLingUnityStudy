using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // 1.利用延时函数实现一个计秒器
    // 2.请用两种方式延时销毁一个指定对象

    void Start()
    {
        InvokeRepeating("Timer", 1, 1);
        Invoke("DestroyTimer", 5);
        //Destroy(gameObject, 5);
    }

    void Update()
    {
        
    }

    public void Timer()
    {
        Debug.Log(Time.time);
    }

    public void DestroyTimer()
    {
        Destroy(this.gameObject.GetComponent<test>());
    }
}
