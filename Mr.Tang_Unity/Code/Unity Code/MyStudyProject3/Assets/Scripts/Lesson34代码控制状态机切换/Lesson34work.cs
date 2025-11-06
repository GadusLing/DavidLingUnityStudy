using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson34work : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("isMoving", true);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("isMoving", false);
        }
        if(animator.GetBool("isMoving"))
        {
            // 用世界坐标系前进
            transform.Translate(Vector3.forward * Time.deltaTime * -30, Space.World);
        }
    }
}
