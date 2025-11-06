using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson51 : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetLayerWeight(animator.GetLayerIndex("MyLayer2"), 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            animator.SetTrigger("Fire");
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            animator.SetLayerWeight(animator.GetLayerIndex("MyLayer3"), 1f);
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            animator.SetLayerWeight(animator.GetLayerIndex("MyLayer3"), 0f);
        }

    }
}
