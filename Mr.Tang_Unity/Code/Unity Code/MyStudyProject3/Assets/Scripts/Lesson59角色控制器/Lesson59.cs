using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lesson59 : MonoBehaviour
{
    CharacterController cc;
    Animator animator;
    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        if(cc.isGrounded)
        {
            print("接触地面了");
        }
        // // 关键方法
        // // 受重力作用的移动
        // cc.SimpleMove(Vector3.forward * 10 * Time.deltaTime);
        // // 不受重力的移动
        // cc.Move(Vector3.forward * 10 * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("Speed", (int)Input.GetAxisRaw("Vertical"));

        cc.SimpleMove(transform.forward * 2f * Input.GetAxisRaw("Vertical"));
        //cc.Move(transform.forward * 2f * Time.deltaTime * Input.GetAxisRaw("Vertical"));
        // 注意
        // SimpleMove()会自动乘 Time.deltaTime 传入的是“速度”（m/s），比如上面的*2 就是2m/s
        // 而Move() 不会自动乘以 Time.deltaTime！传入的是“每帧位移量”（m/frame），如果不*deltaTime 那么帧率是60的情况下，Move()的移动速度是：2米 × 60帧 = 每秒 120米
        // 而*deltaTim 以60帧为例，60帧的Time.deltaTime = 1/60 ​≈ 0.0167 秒 每一帧只经过 0.0167 秒，这个2f每帧会*0.0167，也就意味着每帧移动2*0.0167 = 0.0334米
        // 0.0334 * 60(帧) ​≈ 2米，所以Move的移动是这样来的

        if(cc.isGrounded)
        {
            print("接触地面了");
        }

    }

    //当角色控制器想要判断和别的碰撞器产生碰撞时使用该函数
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        print(hit.collider.gameObject.name);
    }

    //对角色控制器没用
    private void OnCollisionEnter(Collision collision)
    {
        print("碰撞触发");
    }

    //可以检测触发器
    private void OnTriggerEnter(Collider other)
    {
        print("触发器触发");
    }
}
