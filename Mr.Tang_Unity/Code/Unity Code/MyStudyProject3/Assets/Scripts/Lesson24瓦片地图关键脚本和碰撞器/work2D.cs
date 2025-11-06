using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class work2D : MonoBehaviour
{
    private Rigidbody2D rbd2;    
    private SpriteRenderer sr;
    private float h;

    void Start()
    {
        rbd2 = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");

        if(h != 0)
        {
            rbd2.velocity = new Vector2(h * 5, rbd2.velocity.y);
            sr.flipX = h < 0;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rbd2.AddForce(Vector2.up * 300);
        }
    }
}
