using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Transform target; // 目标物体（飞机）
    public float offsetH = 1f; // 摄像机相对于目标的偏移高度

    public float tiltAngle = 45f; // 摄像机相对于目标的倾斜角度

    public float distance = 5f; // 摄像机与目标的距离
    public float distanceMin = 3f; // 摄像机与目标的最小距离限制
    public float distanceMax = 10f; // 摄像机与目标的最大距离限制

    private Vector3 curPos; // 摄像机当前的位置
    private Vector3 curDir; // 摄像机当前的朝向

    void Start()
    {
        
    }
    
    void Update()
    {

    }

    void LateUpdate()
    {
        distance += Input.GetAxis("Mouse ScrollWheel") * 8f;
        distance = Mathf.Clamp(distance, distanceMin, distanceMax);

        curDir = Quaternion.AngleAxis(tiltAngle, target.right) * -target.forward;
        curPos = target.position + target.up * offsetH + curDir * distance;
        transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10f);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-curDir), Time.deltaTime * 2f);
    }

    public void CtrlTiltAngle()
    {

    }

    public void CtrlDistance()
    {

    }


}
