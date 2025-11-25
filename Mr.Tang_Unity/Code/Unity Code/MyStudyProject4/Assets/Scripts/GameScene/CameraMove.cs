using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 负责控制摄像机跟踪角色的摄像机移动脚本
/// </summary>
public class CameraMove : MonoBehaviour
{
    /// <summary>
    /// 摄像机要看向的目标位置
    /// </summary>
    public Transform target;
    /// <summary>
    /// 摄像机与目标位置的偏移量
    /// </summary>
    public Vector3 offsetPos;
    /// <summary>
    /// 角色身体的偏移高度，用于调整摄像机具体看向的位置 是看向脚底 还是看身体中间 还是看头顶
    /// </summary>
    public float bodyHeight;
    /// <summary>
    /// 摄像机移动速度
    /// </summary>
    public float moveSpeed;
    /// <summary>
    /// 摄像机旋转速度
    /// </summary>
    public float rotateSpeed;


    /// <summary>
    /// 摄像机目标位置 不在update里每帧声明 避免GC
    /// </summary>
    private Vector3 CameraPos;
    /// <summary>
    /// 摄像机目标旋转 不在update里每帧声明 避免GC
    /// </summary>
    private Quaternion CameraRot;


    void Update()
    {
        if (target == null) return;

        // 根据目标对象位置 和 偏移量 计算出摄像机目标位置
        // 摄像机在人物后方，先计算Z方向位置 targetTrans.forward * offsetPos.z 会得到沿角色朝向的位移向量 这里的Z规定传负数
        // 这样的话就会得到一个向人物背后的位移向量 这样就确定了相机在角色后方的Z轴位置
        CameraPos = target.position + target.forward * offsetPos.z;

        // 再计算相机的Y方向位置 targetTrans.up * offsetPos.y 会得到沿角色上方向的位移向量 这里的Y传正数
        // 这样的话就会得到一个垂直于地面向上方的位移向量 摄像机再加等于这个向量后 就确定了相机在垂直方向Y轴位置
        // 注意Y轴这里用的是Vector3.up 而不是角色的上方向 targetTrans.up 因为角色可能会有翻滚动作 或者被地形垫了个角度出来 又或者绊了个小跟头 
        // 这样角色的Y轴都会发生改变 此时摄像机会有一个急速的上下晃动效果 体验很差 所以Y轴方向统一用世界坐标系的上方向 Vector3.up 就像用刚体时要锁Y轴一样
        CameraPos += Vector3.up * offsetPos.y;

        // 最后计算X方向位置 targetTrans.right * offsetPos.x 会得到沿角色右方向的位移向量 这里的X传正数
        // 这样的话就会得到一个水平向右的位移向量 摄像机再加等于这个向量后 就确定了相机在水平方向X轴位置
        // 这里又变成了targetTrans.right 而不是Vector3.right 因为当角色左右转动时，摄像机是期望跟随角色转动而移动的
        CameraPos += target.right * offsetPos.x;
        
        // 插值移动摄像机到目标位置
        transform.position = Vector3.Lerp(transform.position, CameraPos, moveSpeed * Time.deltaTime);


        // 摄像机旋转和看向人物 用目标位置-摄像机位置 得到两者之间朝向目标的方向向量 然后用LookRotation得到一个四元数表示摄像机要朝向的旋转参数
        CameraRot = Quaternion.LookRotation(target.position + Vector3.up * bodyHeight - transform.position, Vector3.up);
        // 插值旋转摄像机到目标旋转参数
        transform.rotation = Quaternion.Slerp(transform.rotation, CameraRot, rotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 提供给外部设置摄像机跟踪目标的方法
    /// </summary>
    /// <param name="targetTrans">摄像机要跟踪的目标Transform</param>
    public void SetTarget(Transform targetTrans)
    {
        target = targetTrans;
    }
}
