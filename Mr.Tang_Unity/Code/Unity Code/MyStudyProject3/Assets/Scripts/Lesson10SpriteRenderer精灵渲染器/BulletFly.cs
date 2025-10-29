using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFly : MonoBehaviour
{
    public Vector3 curDir;
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    public void changeMoveDir(Vector3 dir)
    {
        curDir = dir;
    }
    void Update()
    {
        transform.Translate(curDir * 10f * Time.deltaTime);
    }
}
