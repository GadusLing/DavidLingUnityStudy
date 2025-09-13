using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceObj : MonoBehaviour
{
    public float bounceHeight = 2f; // 弹跳高度
    public float bounceSpeed = 2f;  // 弹跳速度
    
    private Vector3 startPosition; // 初始位置
    
    void Start()
    {
        // 记录初始位置
        startPosition = transform.position;
    }

    void Update()
    {
        // 使用正弦函数创建平滑的上下弹跳
        float bounceOffset = (1f - Mathf.Cos(Time.time * bounceSpeed)) * 0.5f * bounceHeight;
        
        
        // 更新Y轴位置，保持X和Z坐标不变
        transform.position = new Vector3(
            startPosition.x, 
            startPosition.y + bounceOffset, 
            startPosition.z
        );
    }
}
