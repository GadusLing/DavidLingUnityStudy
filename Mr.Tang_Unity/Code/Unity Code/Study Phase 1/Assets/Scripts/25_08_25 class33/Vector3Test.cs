using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Test : MonoBehaviour
{
    // 题目1：
    // 一个空对象上挂了一个脚本，这个脚本可以让游戏运行时，
    // 在场景中创建出一个层层由Cube构成的金字塔（提示：实例化预设体或者实例化自带几何体方法）

    // 题目2：
    // 以下四种写法，哪些能让对象朝自己的面朝向移动？为什么？（可以画图说明）
    // this.transform.Translate(Vector3.forward, Space.World);
    // this.transform.Translate(Vector3.forward, Space.Self);
    // this.transform.Translate(this.transform.forward, Space.Self);
    // this.transform.Translate(this.transform.forward, Space.World);

    // 题目3：
    // 使用你之前创建的立方体预设体，让其可以朝自己的面朝向向前移动

    public GameObject prefab;
    public GameObject Tank;

    void Start()
    {
        // 在vector3 zero 000 原点处创建金字塔 不旋转
        GameObject newObj = Instantiate(prefab, Vector3.zero, Quaternion.identity);


        Tank = Instantiate(Tank, new Vector3(5, 5, 5), Quaternion.identity);
    }

    void Update()
    {
        Tank.transform.Translate(Tank.transform.forward * Time.deltaTime * 1, Space.World);
    }
}
