using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutinePrinciple : MonoBehaviour
{
    // 请不使用Unity自带的协程协调器开启协程
    // 通过迭代器函数实现每隔一秒执行函数中的一部分逻辑

    void Start()
    {
        MyCoroutineMgr.Instance.MyStartCoroutine(TestCoroutine());
    }


    void Update()
    {
        
    }

    IEnumerator TestCoroutine()
    {
        Debug.Log("第一次執行");
        yield return 1;

        Debug.Log("第二次執行");
        yield return 2;

        Debug.Log("第三次執行");
        yield return 3;

        Debug.Log("第四次執行");
    }

    
}
