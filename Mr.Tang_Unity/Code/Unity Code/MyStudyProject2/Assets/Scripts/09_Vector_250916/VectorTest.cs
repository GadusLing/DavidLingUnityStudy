using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorTest : MonoBehaviour
{
    //用向量相关知识，实现摄像机跟随（摄像机不设置为对象子物体）
    //摄像机一直在物体的后方4米，向上偏7米的位置
    public Transform target; // 目标物体
    public Vector3 offset; // 偏移量

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 7, -4); // 设置偏移量
    }

    // 这里记住，所哟摄像机、渲染相关的操作，必须在LateUpdate中进行
    // 因为摄像机的更新要在所有物体更新之后
    // 如果写在update中，可能会出现摄像机位置先于物体位置变换的现象，影响渲染
    void LateUpdate()
    {
        if (target != null)
        {
            // 计算摄像机目标位置
            Vector3 targetPosition = target.position + offset;
            transform.position = targetPosition;
            transform.LookAt(target); // 让摄像机始终看向目标物体
        }
    }
}
