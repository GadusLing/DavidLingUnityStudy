using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererTest : MonoBehaviour
{
    //1.请写一个方法，传入一个中心点，传入一个半径，用LineRender画一个圆出来
    //2.请实现，在Game窗口长按鼠标用LineRender画出鼠标移动的轨迹
    private LineRenderer line2;
    void Start()
    {
        //DrawCircle(new Vector3(0, 0, 0), 5, 90);
        // line2 = gameObject.AddComponent<LineRenderer>();
        // line2.loop = false;
        // line2.startWidth = 0.5f;
        // line2.endWidth = 0.5f;

    }

    // Update is called once per frame
    void Update()
    {
        DrawLineByMouse();
    }

    public void DrawCircle(Vector3 center, float radius, int pointNum)
    {
        GameObject obj = new GameObject("Circle");
        LineRenderer line = obj.AddComponent<LineRenderer>();

        line.loop = true;
        line.positionCount = pointNum;


        float angle = 360f / pointNum;
        for (int i = 0; i < pointNum; i++)
        {
            //知识点
            //1.点 + 向量相当于平移点，得到的是平移后的新位置
            //2.四元数 * 向量相当于在旋转向量
            //Quaternion.AngleAxis(float angle, Vector3 axis)   angle: 旋转角度（以度为单位）  axis: 旋转轴向量
            line.SetPosition(i, center + Quaternion.AngleAxis(angle * i, Vector3.up) * Vector3.forward * radius);
        }
    }

    public void DrawLineByMouse()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject obj = new GameObject();
            line2 = obj.AddComponent<LineRenderer>();
            line2.loop = false;
            line2.startWidth = 0.5f;
            line2.endWidth = 0.5f;
            line2.positionCount = 0;
        }

        if (Input.GetMouseButton(0))
        {
            line2.positionCount++;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            line2.SetPosition(line2.positionCount - 1, worldPos);
        }
    }
}
