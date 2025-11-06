using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lesson34 : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();

        // // 设置条件参数来切换状态机状态
        // animator.SetBool("条件名", true);
        // // animator.SetTrigger("条件名");
        // animator.SetFloat("条件名", 1.0f);
        // animator.SetInteger("条件名", 2);

        // // 获取某个条件的值
        // animator.GetFloat("条件名");
        // animator.GetBool("条件名");
        // animator.GetInteger("条件名");
        // animator.ResetTrigger("条件名");

        // 直接切换状态
        // animator.Play("状态名");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetBool("New Bool", true);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("New Bool", false);
        }
    }
}
