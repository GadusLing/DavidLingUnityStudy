using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittlesidekickMove : MonoBehaviour
{
    public GameObject littleSidekick;
    public GameObject BigBrother;

    public float moveSpeed = 5f; // 可以调整速度

    void Update()
    {
        // 插值移动
        littleSidekick.transform.position = Vector3.Lerp(
            littleSidekick.transform.position,
            BigBrother.transform.position,
            Time.deltaTime * moveSpeed
        );
        littleSidekick.transform.LookAt(BigBrother.transform);

        // //匀速运动
        // Vector3 direction = (BigBrother.transform.position - littleSidekick.transform.position).normalized;
        // float distance = Vector3.Distance(littleSidekick.transform.position, BigBrother.transform.position);

        // if (distance > 0.1f) // 避免过近时抖动
        // {
        //     littleSidekick.transform.position += direction * moveSpeed * Time.deltaTime;
        // }
    }
}
