using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigfunctions : MonoBehaviour
{
    public GameObject cube;
    // 实现一个物体按曲线移动（正弦或者余弦曲线）
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // float x = Time.time;
        // float y = Mathf.Sin(x); // 正弦曲线
        // cube.transform.position = new Vector3(x, y, 0);

        float x = Time.time;
        float y = Mathf.Cos(x); // 余弦曲线
        cube.transform.position = new Vector3(x, y, 0);
    }
}
