using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Lesson55 : MonoBehaviour
{
    private Animator animator;
    // public Transform pos;
    // public Transform pos1;
    public Transform headpos;
    private float changeAngleX;
    private float changeAngleY;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        changeAngleX += Input.GetAxis("Mouse X");
        changeAngleX = Mathf.Clamp(changeAngleX, -30, 30);

        changeAngleY += Input.GetAxis("Mouse Y");
        changeAngleY = Mathf.Clamp(changeAngleY, -30, 30);
    }

    void OnAnimatorIK(int layerIndex)
    {
        // // 头部IK相关
        // animator.SetLookAtWeight(1, 0f, 1f);
        // animator.SetLookAtPosition(pos.position);

        // animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        // //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        // animator.SetIKPosition(AvatarIKGoal.RightHand, pos1.position);
        // //animator.SetIKRotation(AvatarIKGoal.RightHand, pos1.rotation);

        animator.SetLookAtWeight(1, 1f, 1f);
        Vector3 pos = Quaternion.AngleAxis(changeAngleX, Vector3.up) * (headpos.position + headpos.forward * 10);
        pos = Quaternion.AngleAxis(changeAngleY, Vector3.right) * pos;
        animator.SetLookAtPosition(pos);
    }

    void OnAnimatorMove()
    {
         
    }
}
