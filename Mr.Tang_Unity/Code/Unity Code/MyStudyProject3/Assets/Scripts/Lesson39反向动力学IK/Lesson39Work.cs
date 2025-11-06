using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson39Work : MonoBehaviour
{
    public Transform IKPoint;
    private float z;
    private Vector3 mousePos;
    void Start()
    {
        z = Camera.main.WorldToScreenPoint(IKPoint.position).z;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            mousePos = Input.mousePosition;
            mousePos.z = z;
            IKPoint.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }
}
