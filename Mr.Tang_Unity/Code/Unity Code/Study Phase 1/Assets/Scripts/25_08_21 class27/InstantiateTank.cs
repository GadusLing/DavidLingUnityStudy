using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateTank : MonoBehaviour
{
    public GameObject Tank;
    // Start is called before the first frame update
    void Start()
    {
        GameObject tankobj = Instantiate(Tank);
        tankobj.name = "�޵д�̹��";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
