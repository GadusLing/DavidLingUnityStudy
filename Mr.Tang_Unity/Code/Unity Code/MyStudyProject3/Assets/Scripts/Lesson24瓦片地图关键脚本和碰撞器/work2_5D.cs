using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class work2_5D : MonoBehaviour
{
    private float h;
    private float v;
    private SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        if(h != 0)
        {
            sr.flipX = h < 0;
            transform.Translate(Vector3.right * h * Time.deltaTime * 5);
        }
        if(v != 0)
        {
            transform.Translate(Vector3.up * v * Time.deltaTime * 5);
        }
    }
}
