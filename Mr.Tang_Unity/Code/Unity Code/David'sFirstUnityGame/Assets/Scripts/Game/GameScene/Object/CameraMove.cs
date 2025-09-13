using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform targetPlayer;
    private Vector3 pos;
    public float height = 15f;
    void LateUpdate()
    {
        if (targetPlayer != null)
        {
            pos.x = targetPlayer.position.x;
            pos.z = targetPlayer.position.z;
            pos.y = height;
            transform.position = pos;
        }
    }
}
