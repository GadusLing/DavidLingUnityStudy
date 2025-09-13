using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JitterObj : MonoBehaviour
{
    [Header("抖动设置")]
    public float jitterIntensity = 0.1f;  // 抖动强度
    public float jitterSpeed = 10f;       // 抖动频率
    public bool enablePositionJitter = true;  // 位置抖动
    public bool enableRotationJitter = true;  // 旋转抖动
    
    [Header("抖动范围")]
    public Vector3 positionJitterRange = new Vector3(0.05f, 0.02f, 0.05f);  // XYZ轴抖动范围
    public Vector3 rotationJitterRange = new Vector3(0.5f, 0.5f, 0.5f);     // 旋转抖动范围（度）
    
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    
    void Start()
    {
        // 记录原始位置和旋转
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        Vector3 targetPosition = originalPosition;
        Quaternion targetRotation = originalRotation;
        
        // 位置抖动
        if (enablePositionJitter)
        {
            float randomX = (Mathf.PerlinNoise(Time.time * jitterSpeed, 0) - 0.5f) * 2f * positionJitterRange.x * jitterIntensity;
            float randomY = (Mathf.PerlinNoise(0, Time.time * jitterSpeed) - 0.5f) * 2f * positionJitterRange.y * jitterIntensity;
            float randomZ = (Mathf.PerlinNoise(Time.time * jitterSpeed, Time.time * jitterSpeed) - 0.5f) * 2f * positionJitterRange.z * jitterIntensity;
            
            targetPosition += new Vector3(randomX, randomY, randomZ);
        }
        
        // 旋转抖动
        if (enableRotationJitter)
        {
            float rotX = (Mathf.PerlinNoise(Time.time * jitterSpeed + 100, 0) - 0.5f) * 2f * rotationJitterRange.x * jitterIntensity;
            float rotY = (Mathf.PerlinNoise(0, Time.time * jitterSpeed + 100) - 0.5f) * 2f * rotationJitterRange.y * jitterIntensity;
            float rotZ = (Mathf.PerlinNoise(Time.time * jitterSpeed + 100, Time.time * jitterSpeed + 100) - 0.5f) * 2f * rotationJitterRange.z * jitterIntensity;
            
            targetRotation = originalRotation * Quaternion.Euler(rotX, rotY, rotZ);
        }
        
        // 应用抖动
        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }
}
