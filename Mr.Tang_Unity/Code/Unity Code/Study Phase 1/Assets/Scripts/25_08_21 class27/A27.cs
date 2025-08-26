using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A27 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject B = GameObject.Find("B27");
        B27 b27 = B.GetComponent<B27>();
        if (b27) b27.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
