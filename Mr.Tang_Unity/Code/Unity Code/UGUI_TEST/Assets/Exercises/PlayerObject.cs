using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerObject : MonoBehaviour
{
    public Vector3 curMoveDir = Vector3.zero;
    public float moveSpeed = 10f;
    public float rotateSpeed = 40f;
    void Start()
    {

    }

    void Update()
    {
        if(curMoveDir != Vector3.zero)
        {
            // transform.Translate(curMoveDir.normalized * Time.deltaTime * moveSpeed, Space.World);
            // transform.forward = curMoveDir.normalized; // 比较粗糙的写法，没用四元数插值，会产生瞬移

            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(curMoveDir), rotateSpeed * Time.deltaTime);
        }

    }

    public void Fire()
    {
        if(MusicData.SoundIsOpen)
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Fire"), transform.position, MusicData.SoundVolume);

        Instantiate(Resources.Load<GameObject>("Bullet"), transform.position + transform.forward * 2, transform.rotation);
    }

    public void Move(Vector2 dir)
    {
        curMoveDir = new Vector3(dir.x, 0, dir.y);
    }
}
