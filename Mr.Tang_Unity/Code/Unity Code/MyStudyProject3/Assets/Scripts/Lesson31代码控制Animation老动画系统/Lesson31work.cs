using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson31work : MonoBehaviour
{
    private Animation anim;
    void Start()
    {
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            anim.CrossFade("Move");
        }
        else if(Input.GetKeyUp(KeyCode.W))
        {
            anim.CrossFade("Idle");
        }
        // 持续检测按键状态进行移动
        if(Input.GetKey(KeyCode.W)) // 使用 GetKey 而不是 GetKeyDown
        {
            // 用世界坐标系前进
            transform.Translate(Vector3.forward * Time.deltaTime * -30, Space.World);
        }
        
    }
}
