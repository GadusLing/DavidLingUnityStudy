using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using Spine;
using UnityEngine;

public class Lesson45 : MonoBehaviour
{
    private SkeletonAnimation sa;

    // 便捷特性
    [SpineAnimation] // 动画特性，方便在Inspector面板中选择动画名称
    public string animName;

    [SpineBone] // 骨骼特性，方便在Inspector面板中选择骨骼名称
    public string boneName;

    [SpineSlot] // 插槽特性，方便在Inspector面板中选择插槽名称
    public string slotName;

    [SpineAttachment] // 附件特性，方便在Inspector面板中选择附件名称
    public string attachmentName;
    void Start()
    {
        sa = GetComponent<SkeletonAnimation>();

        // 直接改变SkeletonAnimation的AnimationName属性
        // 这种方式会立即切换动画，并且会中断当前正在播放的动画
        // 适用于需要立即切换动画的场景
        //sa.loop = true;
        //sa.AnimationName = "jump";

        // 通过 AnimationState.SetAnimation API方法来设置动画
        // 这种方式可以更灵活地控制动画的播放
        // 适用于需要更复杂动画控制的场景
        sa.AnimationState.SetAnimation(0, "walk", false);

        // 通过 AnimationState.AddAnimation API方法来添加动画到队列
        // 这种方式允许在当前动画播放完后，自动切换到添加的动画
        // 适用于需要顺序播放多段动画的场景
        sa.AnimationState.AddAnimation(0, "jump", true, 0f);

        // 转向
        sa.Skeleton.ScaleX = -1;

        // 动画事件-播放
        sa.AnimationState.Start += (t) =>
        {
            Debug.Log("动画开始播放: " + sa.AnimationName);
        };

        // 动画事件-中断or清除
        sa.AnimationState.End += (t) =>
        {
            Debug.Log("动画播放中断or清除: " + sa.AnimationName);
        };

        // 动画事件-完成
        sa.AnimationState.Complete += (t) =>
        {
            Debug.Log("动画播放完成: " + sa.AnimationName);
        };

        // 动画事件-自定义事件
        sa.AnimationState.Event += (t, e) =>
        {
            Debug.Log("动画自定义事件,由美术在spine中设置: " + sa.AnimationName);
        };

        // 获取骨骼 设置插槽、附件
        Bone b = sa.skeleton.FindBone(boneName);

        sa.skeleton.SetAttachment(slotName, attachmentName);

        // 在UI中使用
        
        

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
