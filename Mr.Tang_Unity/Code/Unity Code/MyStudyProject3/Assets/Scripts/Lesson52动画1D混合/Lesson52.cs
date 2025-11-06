using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson52 : MonoBehaviour
{
    private Animator animator;
    private float dValue = 0.5f;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", Input.GetAxis("Vertical") * dValue);
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            dValue = 1f;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            dValue = 0.5f;
        }
    }
}
