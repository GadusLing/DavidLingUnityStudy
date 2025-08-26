using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celestialmotion : MonoBehaviour
{
    public Transform Sun;
    public Transform Earth;
    public Transform Moon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 三个球体的自转
        Sun.Rotate(Vector3.up * 10 * Time.deltaTime);
        Earth.Rotate(Vector3.up * 50 * Time.deltaTime);
        Moon.Rotate(Vector3.up * 100 * Time.deltaTime);

        // 公转，地球围绕太阳，月球围绕地球
        Earth.RotateAround(Sun.position, Vector3.up, 10 * Time.deltaTime);
        Moon.RotateAround(Earth.position, Vector3.up, 30 * Time.deltaTime);

    }
}
