using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObj : MonoBehaviour
{
    public float speed = 10f;
    public float maxAngle = 45f; // 最大旋转角度

    private float currentAngle = 0f; // 当前角度
    private bool rotateRight = true; // 旋转方向
    
    void Update()
    {
        // 计算旋转增量
        float rotationAmount = speed * Time.deltaTime;
        
        if (rotateRight)
        {
            currentAngle += rotationAmount;
            if (currentAngle >= maxAngle)
            {
                currentAngle = maxAngle;
                rotateRight = false; // 切换方向
            }
        }
        else
        {
            currentAngle -= rotationAmount;
            if (currentAngle <= -maxAngle)
            {
                currentAngle = -maxAngle;
                rotateRight = true; // 切换方向
            }
        }
        
        // 使用本地旋转而不是世界旋转
        transform.localRotation = Quaternion.Euler(0, currentAngle, 0);
    }
}
