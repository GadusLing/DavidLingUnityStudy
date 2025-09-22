using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookRotation : MonoBehaviour
{
    //1.利用四元数的LookRotation方法，实现LookAt的效果
    //2.将之前摄像机移动的练习题中的LookAt换成LookRotation实现
    //并且通过Slerp来缓慢看向玩家


    public Transform targetSphere;
    private Vector3 targetCamera;

    private Quaternion targetRotation;

    void Start()
    {
    }

    void LateUpdate()
    {
        //transform.MyLookAt(targetSphere);
        targetCamera = targetSphere.position - targetSphere.forward * 4f + Vector3.up * 7f;
        transform.position = Vector3.Lerp(transform.position, targetCamera, Time.deltaTime * 2f);
        targetRotation = Quaternion.LookRotation(targetSphere.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
    }
}
