using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    //1.使用之前的坦克预设体，用WASD键控制坦克的前景后退，左右转向
    //2.在上一题的基础上，鼠标左右移动控制炮台的转向
    //把这道题的代码保留好，之后的题会用到
    public GameObject tank;

    private GameObject tankInstance;
    private GameObject Tower;

    // 坦克移动参数
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    // 炮台旋转参数
    public float turretRotationSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // 实例化坦克并保存引用
        tankInstance = Instantiate(tank, new Vector3(0, 0, 0), Quaternion.identity);

        // 查找炮台对象
        Tower = tankInstance.transform.Find("TankFree_Tower").gameObject;
        if (Tower == null)
        {
            Debug.LogError("未找到名为'TankFree_Tower'的炮台对象！");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tankInstance == null) return;

        // 获取键盘输入
        float moveInput = Input.GetAxis("Vertical");   // W/S键
        float turnInput = Input.GetAxis("Horizontal"); // A/D键

        // 移动坦克
        tankInstance.transform.Translate(Vector3.forward * moveInput * moveSpeed * Time.deltaTime);
        tankInstance.transform.Rotate(Vector3.up, turnInput * rotationSpeed * Time.deltaTime);

        // 获取鼠标输入并旋转炮台
        if (Tower != null)
        {
            float mouseX = Input.GetAxis("Mouse X");
            Tower.transform.Rotate(Vector3.up, mouseX * turretRotationSpeed);
        }
    }
}