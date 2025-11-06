using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson56 : MonoBehaviour
{
    private Animator animator;
    public Transform targetPos;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Jump");
        }
    }

    private void MatchTarget()
    {
        animator.MatchTarget(targetPos.position, targetPos.rotation, AvatarTarget.LeftFoot, new MatchTargetWeightMask(Vector3.one, 1), 0.4f, 0.64f);
    }
}
