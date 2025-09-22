using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionTest : MonoBehaviour
{
    //四元数Q=[cos(β/2)，sin(β/2)x, sin(β/2)y, sin(β/2)z]

    //四元数Q = Quaternion.AngleAxis(角度，轴):
    //Quaternion q = Quaternion.AngleAxis(60, Vector3.right):

    void Start()
    {
        Quaternion q = new Quaternion(Mathf.Sin(30 * Mathf.Deg2Rad / 2), 0, 0, Mathf.Cos(30 * Mathf.Deg2Rad / 2));

        Quaternion q1 = Quaternion.AngleAxis(30, Vector3.right);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
