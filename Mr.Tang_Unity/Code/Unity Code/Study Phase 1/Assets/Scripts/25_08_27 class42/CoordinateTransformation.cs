using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateTransformation : MonoBehaviour
{
    /// <summary>
    /// 题目1：在物体A前方创建一个新物体
    /// 要求：
    /// 1. 不考虑物体A的位置
    /// 2. 在物体A的前方坐标(-1,0,1)处创建一个空物体
    /// </summary>
    [ContextMenu("左前方创建方体")]
    private void CreateObjectInFront()
    {
        //Vector3 pos = transform.TransformPoint(new Vector3(-1, 0, 1));
        GameObject newObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newObj.name = "左前方方体";
        newObj.transform.position = transform.TransformPoint(new Vector3(-1, 0, 1));
    }

    /// <summary>
    /// 题目2：在物体A前方创建三个球体
    /// 要求：
    /// 1. 不考虑物体A的位置
    /// 2. 在物体A的前方创建3个球体
    /// 3. 三个球体的相对位置分别为：
    ///    - (0,0,1)
    ///    - (0,0,2)
    ///    - (0,0,3)
    /// </summary>
    [ContextMenu("前方创建三个球")]
    private void CreateThreeSpheresInFront()
    {
        // TODO: 在这里实现题目2的逻辑
        for (int i = 1; i <= 3; i++)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = "球体" + i;
            sphere.transform.position = transform.TransformPoint(new Vector3(0, 0, i));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateObjectInFront();
        CreateThreeSpheresInFront();
    }

}
