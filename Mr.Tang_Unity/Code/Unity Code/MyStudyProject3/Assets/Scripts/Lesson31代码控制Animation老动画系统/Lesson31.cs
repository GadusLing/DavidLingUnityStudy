using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson31 : MonoBehaviour
{
    private Animation anim;
    void Start()
    {
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        // 播放动画
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.Play("1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.Play("2");
        }

        // 淡入播放，自动产生过渡效果
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            anim.CrossFade("3");
        }

        // 前一个播放完再播放后一个
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //anim.PlayQueued("2");
            anim.CrossFadeQueued("2");// 这种相比于PlayQueued有过渡效果
        }

        // 停止播放所有动画
        //anim.Stop();

        // 是否在播放某个动画
        if (anim.IsPlaying("1"))
        {
            Debug.Log("动画1正在播放");
        }

        // 设置动画播放模式
        // anim.wrapMode = WrapMode.Loop; // 循环播放
        // anim.wrapMode = WrapMode.Once; // 播放一次
        // anim.wrapMode = WrapMode.PingPong; // 往返播放

    }

    public void AnimationEvent()
    {
        Debug.Log("动画事件触发");
    }

}
