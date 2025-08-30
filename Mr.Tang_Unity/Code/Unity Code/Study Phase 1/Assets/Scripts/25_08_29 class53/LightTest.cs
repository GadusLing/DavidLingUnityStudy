using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTest : MonoBehaviour
{
    /*
     * 练习题目1：通过代码结合点光源模拟一个蜡烛的光源效果
     * - 创建点光源组件
     * - 设置光源颜色为暖黄色（模拟蜡烛火焰）
     * - 调整光源强度和范围
     * - 可选：添加光源闪烁效果模拟蜡烛摇曳
     */

    /*
     * 练习题目2：通过代码结合方向光模拟日天亮度的变化
     * - 创建方向光组件（模拟太阳光）
     * - 设置光源方向和角度
     * - 通过时间变化调整光源强度和颜色
     * - 模拟从日出到日落的光照变化过程
     * - 可以结合天空盒实现更真实的日夜循环效果
     */

    public Light light;
    public float moveSpeed = 0.5f;// 移动速度
    public float intensitySpeed = 0.5f;// 蜡烛光强变化速度
    public float minIntensity = 0.8f;// 最小光强
    public float maxIntensity = 1.0f;// 最大光强
    public Transform lightTransform;
    public float rotateSpeed = 10f;// 旋转速度

    // Update is called once per frame
    void Update()
    {
        light.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        if(light.transform.position.x >= 0.45)
        {
            moveSpeed = -moveSpeed;
        }
        else if(light.transform.position.x < 0.3)
        {
            moveSpeed = -moveSpeed;
        }

        light.intensity += intensitySpeed * Time.deltaTime;
        if(light.intensity >= maxIntensity)
        {
            intensitySpeed = -intensitySpeed;
        }
        else if(light.intensity <= minIntensity)
        {
            intensitySpeed = -intensitySpeed;
        }

        lightTransform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);
    }
}
