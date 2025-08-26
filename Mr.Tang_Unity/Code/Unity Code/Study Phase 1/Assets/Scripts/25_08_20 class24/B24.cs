using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B24 : MonoBehaviour
{
    public A24 a; // ÒýÓÃA½Å±¾
    // Start is called before the first frame update
    void Start()
    {
        if (a) a.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
