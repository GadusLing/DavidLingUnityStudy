using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CameraAnimator : MonoBehaviour
{
    private Animator animator;
    /// <summary>
    /// 用于记录动画播放完毕后的回调
    /// </summary>
    private UnityAction onAnimEnd;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // 左转
    public void TurnLeft(UnityAction callBack)
    {
        animator.SetTrigger("Left");
        onAnimEnd = callBack;
    }
    // 右转
    public void TurnRight(UnityAction callBack)
    {
        animator.SetTrigger("Right");
        onAnimEnd = callBack;
    }

    // 当动画播放完毕 会调用的方法 事件节点已经在动画animation里添加好了
    public void AnimEnd()
    {
        // 动画播放完毕后可能要做很多不同的操作 显隐不同的面板 所以最好的方式是用回调函数，让外部传入需要执行的操作
        onAnimEnd?.Invoke();
        onAnimEnd = null; // 用完之后记得清空，避免重复调用
    }

}
