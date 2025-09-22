using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossproductTest : MonoBehaviour
{
    //1.判断一个物体B位置在另一个物体A的位置的左上，左下，右上，右下哪个方位
    //2.当一个物体B在物体A左前方20度角或右前方30度范围内，并且离A只有5米距离时，
    //在控制台打印“发现入侵者”
    
    // 自己是A，这个脚本挂A上
    public Transform B; // 目标物体
    public float leftAngle = 20f; // 左前方角度
    public float rightAngle = 30f; // 右前方角度
    public float detectionDistance = 5f; // 检测距离

    float AdB;
    Vector3 AcB;

    void Update()
    {
        // 1.判断B在A的前方or后方
        AdB = Vector3.Dot(transform.forward, B.position - transform.position);
        AcB = Vector3.Cross(transform.forward, B.position - transform.position);
        if (AdB >= 0)// 点乘结果>0为锐角，在前方   =0为直角,应该是正左右，这里也算在前方
        {
            //判断左右
            if (AcB.y >= 0)// 叉乘结果>0，B在A右侧
            {
                Debug.Log("B在A的右前方");
            }
            else
            {
                Debug.Log("B在A的左前方");
            }
        }
        else// 点乘结果<0为钝角，在后方
        {
            //判断左右
            if (AcB.y < 0)// 叉乘结果<0，B在A左侧
            {
                Debug.Log("B在A的左后方");
            }
            else
            {
                Debug.Log("B在A的右后方");
            }
        }

        // 发现入侵者
        if (Vector3.Distance(transform.position, B.position) <= detectionDistance && AdB > 0 && AcB.y > 0 && Vector3.Angle(transform.forward, B.position - transform.position) <= rightAngle)
        {
            Debug.Log("发现入侵者在右前方30度范围内");
        }
        else if (Vector3.Distance(transform.position, B.position) <= detectionDistance && AdB > 0 && AcB.y < 0 && Vector3.Angle(transform.forward, B.position - transform.position) <= leftAngle)
        {
            Debug.Log("发现入侵者在左前方20度范围内");
        }
    }
}
