using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterpolationTest : MonoBehaviour
{   
    //1.用线性插值相关知识，实现摄像机跟随（摄像机不设置为对象子物体）
    // 摄像机一直在物体的后方4米，向上偏7米的位置
    public Transform cube; // 目标物体
    public float distanceBehind = 4f; // 距离物体的后方距离
    public float heightOffset = 7f; // 高度偏移

    private Vector3 startPos;
    private Vector3 endPos;
    private float lerpTime = 0f;
    public float lerpSpeed = 0.5f; // 1秒到终点
    private Vector3 lastCubePos;
    private float slerpTime = 0f;


    void Start()
    {
        startPos = transform.position;
        endPos = cube.position - cube.forward * distanceBehind + Vector3.up * heightOffset;
        lastCubePos = cube.position;
    }

    void LateUpdate()
    {
        // 1. 摄像机跟随
        // 变速跟踪
        Vector3 cameraPosition = cube.position - cube.forward * distanceBehind + Vector3.up * heightOffset;
        // transform.position = Vector3.Lerp(cameraPosition, cube.position, Time.deltaTime * 2f);
        // transform.LookAt(cube);

        //匀速跟踪
        // 检查目标位置是否变化
        if (cube.position != lastCubePos)
        {
            startPos = transform.position;
            endPos = cameraPosition;
            lerpTime = 0f;
            lastCubePos = cube.position;
        }

        if (lerpTime < 1f)
        {
            lerpTime += Time.deltaTime * lerpSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, lerpTime);
            transform.LookAt(cube);
        }
        // 美术效果来看，摄像机插值实现匀速跟踪物体不如变速跟随运镜自然，而且代码还多，不如变速一根，属于吃力不讨好

        //2.球形插值模拟太阳变化
        //从东边10,0,0升起，正午0，10，0西边落下-10，0，0
        // 球形插值模拟太阳变化
        if (slerpTime < 1f)
        {
            slerpTime += Time.deltaTime * 0.2f; // 控制太阳运动速度
            cube.position = Vector3.Slerp(Vector3.right * 10, Vector3.left * 10 + Vector3.up * 0.1f, slerpTime);
        }

    }
}
