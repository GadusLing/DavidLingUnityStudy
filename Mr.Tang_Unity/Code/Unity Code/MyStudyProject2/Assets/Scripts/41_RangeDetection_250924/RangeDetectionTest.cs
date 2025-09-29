using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class RangeDetectionTest : MonoBehaviour
{
    // 世界坐标原点有一个立方体，键盘WASD键可以控制其前后移动和旋转
    // 请结合所学知识实现
    // 1.按J键在立方体面朝向前方1米处进行立方体范围检测
    // 2.按K键在立方体前面5米范围内进行胶囊范围检测
    // 3.按L键以立方体脚下为原点，半径10米内进行球形范围检测
    private GameObject obj;

    void Start()
    {
        obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;


    }

    // Update is called once per frame
    void Update()
    {
        CtrlCube();
    }

    public void CtrlCube()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        obj.transform.Translate(Vector3.forward * v * Time.deltaTime * 5);
        obj.transform.Rotate(Vector3.up * h * Time.deltaTime * 100);

        if(Input.GetKeyDown(KeyCode.J))
        {
            Vector3 center = obj.transform.position + obj.transform.forward * 1;
            Collider[] colliders = Physics.OverlapBox(center, new Vector3(0.5f, 0.5f, 0.5f), obj.transform.rotation, LayerMask.GetMask("Monster"));
            //                                             这里注意是长宽高的一半                        等价 1 << LayerMask.NameToLayer("Monster")
            foreach (var item in colliders)
            {
                Debug.Log("物体受伤" + item.name);
            }
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            Vector3 point1 = obj.transform.position;
            Vector3 point2 = obj.transform.position + obj.transform.forward * 5;
            Collider[] colliders = Physics.OverlapCapsule(point1, point2, 0.5f, LayerMask.GetMask("Monster"));
            foreach (var item in colliders)
            {
                Debug.Log("物体受伤" + item.name);
            }
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            Vector3 center = obj.transform.position;
            Collider[] colliders = Physics.OverlapSphere(center, 10f, LayerMask.GetMask("Monster"));
            foreach (var item in colliders)
            {
                Debug.Log("物体受伤" + item.name);
            }
        }
    }
}
