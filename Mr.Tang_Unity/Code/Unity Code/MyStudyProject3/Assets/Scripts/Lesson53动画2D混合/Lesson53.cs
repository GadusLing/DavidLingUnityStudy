using System.Collections;
using System.Collections.Generic;
using PlasticGui.Configuration.OAuth;
using UnityEngine;

public class Lesson53 : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("x", Input.GetAxis("Horizontal"));
        animator.SetFloat("y", Input.GetAxis("Vertical"));
    }
}
