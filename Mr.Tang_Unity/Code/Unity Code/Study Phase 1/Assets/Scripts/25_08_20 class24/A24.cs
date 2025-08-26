using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A24 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        B24 b = GetComponent<B24>();
        if(b)
        {
            b.enabled = false; //  ßªÓBΩ≈±æ
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
