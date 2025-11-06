using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson50 : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("Speed", (int)Input.GetAxisRaw("Vertical"));
        if(Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("Jump", true);
        }
        transform.Rotate(Vector3.up, Input.GetAxisRaw("Horizontal") * 150f * Time.deltaTime);
    }

    public void JumpOver()
    {
        animator.SetBool("Jump", false);
    }
}
