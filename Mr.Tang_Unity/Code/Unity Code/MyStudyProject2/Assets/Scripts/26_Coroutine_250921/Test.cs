using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    // 1.利用协程制作一个计秒器
    // 2.请在场景中创建100000个随机位置的立方体，让其不会明显卡顿
    void Start()
    {
        StartCoroutine(MyCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // for(int i = 0; i < 100000; i++)// 像这种在一帧瞬间创建十万个，帧运行时间超时，就会感觉明显的卡顿
            // {
            //     GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //     cube.transform.position = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
            // }
            StartCoroutine(CreatCube(100000));
        }
    }

    IEnumerator CreatCube(int count)
    {
        for (int i = 0; i < count; i++)// 协程分帧创建立方体，就不会明显卡顿
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
            if(i % 1000 == 0)
                yield return null;
        }
    }
    IEnumerator MyCoroutine()
    {
        while (true)
        {
            Debug.Log(Time.time);
            yield return new WaitForSeconds(1);
        }
    }
}
