using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointproductTest : MonoBehaviour
{
    //当一个物体B在物体A前方45度角范围内，并且离A只有5米距离时，在控制台打印“发现入侵者”
    public Transform assassin; // 刺客
    public float detectionAngle = 45f; // 检测角度
    public float detectionDistance = 5f; // 检测距离

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(assassin != null)
        // {
        //     Vector3 directionToAssassin = assassin.position - transform.position; // 刺客位置(终点)-守卫位置(起点) 得到一个新的向量，由守卫指向刺客
        //     float distanceToAssassin = directionToAssassin.magnitude; // 计算与刺客的距离(刚刚得到的那个向量的模长)

        //     // 检测距离
        //     if (distanceToAssassin <= detectionDistance) // 刺客进入视锥范围
        //     {
        //         // 计算与前方的夹角，实际开发中用Angle API
        //         //float angleToAssassin = Vector3.Angle(transform.forward, directionToAssassin);

        //         // 学习的时候用点乘和反余弦自己写
        //         float dot = Vector3.Dot(transform.forward.normalized, directionToAssassin.normalized); 
        //         // 计算点乘dot(就是邻边的长度) = cosβ = 单位向量A * 单位向量B
        //         float angleToAssassin = Mathf.Acos(dot) * Mathf.Rad2Deg; // 反余弦得到夹角(弧度制)，再转为角度制

        //         // 检测角度
        //         if (angleToAssassin <= detectionAngle)
        //         {
        //             Debug.Log("发现入侵者");
        //         }
        //     }
        // }

        //当然实际开发中一个Distance + Angle 一个&&if语句就搞定了
        if(Vector3.Distance(transform.position, assassin.position) <= detectionDistance &&
         Vector3.Angle(transform.forward, assassin.position - transform.position) <= detectionAngle)
        {
            Debug.Log("发现入侵者");
        }
    }
}
