using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{
    public static void MyLookAt(this Transform current, Transform target)
    {
        current.transform.rotation = Quaternion.LookRotation(target.position - current.position);
    }
}
