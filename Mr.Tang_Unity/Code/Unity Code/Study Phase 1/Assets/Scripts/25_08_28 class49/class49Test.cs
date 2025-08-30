using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class class49Test : MonoBehaviour
{
    public GameObject tank;

    private GameObject tankInstance;
    private GameObject Tower;
    private GameObject Canon;
    private GameObject[] wheels;   // 四个轮子
    private Camera cam;            // 自己的相机引用
    private Camera camS;            

    // 坦克移动参数
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    // 炮台旋转参数
    public float turretRotationSpeed = 1f;

    // 炮管抬动参数
    public float cannonElevationSpeed = 10f;

    // 相机跟随参数
    public float cameraFollowSpeed = 5f;
    private Vector3 cameraOffset = new Vector3(0, 5, -9);

    void Start()
    {
        // 实例化坦克
        tankInstance = Instantiate(tank, Vector3.zero, Quaternion.identity);

        // 查找子物体
        Tower = FindChild(tankInstance, "TankFree_Tower");
        Canon = FindChild(Tower, "TankFree_Canon");

        wheels = new GameObject[]
        {
            FindChild(tankInstance, "TankFree_Wheel_b_left"),
            FindChild(tankInstance, "TankFree_Wheel_b_right"),
            FindChild(tankInstance, "TankFree_Wheel_f_left"),
            FindChild(tankInstance, "TankFree_Wheel_f_right"),
        };

        // 创建一台新相机
        GameObject camObj = new GameObject("TankCamera");
        cam = camObj.AddComponent<Camera>();
        // 创建炮管肩部摄影机
        GameObject camSObj = new GameObject("TankSCamera");
        camS = camSObj.AddComponent<Camera>();

        // 将炮管肩部摄影机设置为坦克的子物体
        cam.transform.SetParent(tankInstance.transform);
        camS.transform.SetParent(tankInstance.transform);

        // 设置分屏效果 - 左右分屏
        // 左侧相机：x=0, y=0, width=0.5, height=1
        cam.rect = new Rect(0, 0, 0.5f, 1f);
        // 右侧相机：x=0.5, y=0, width=0.5, height=1
        camS.rect = new Rect(0.5f, 0, 0.5f, 1f);

        // 初始位置
        cam.transform.position = tankInstance.transform.position + new Vector3(0, 5, -9);
        camS.transform.position = tankInstance.transform.position + new Vector3(0.92f, 0.77f, -0.66f);
        cam.transform.LookAt(tankInstance.transform);
    }

    void Update()
    {
        if (tankInstance == null || cam == null) return;

        // ===== 坦克移动 =====
        float moveInput = Input.GetAxis("Vertical");   // W/S
        float turnInput = Input.GetAxis("Horizontal"); // A/D
        tankInstance.transform.Translate(Vector3.forward * moveInput * moveSpeed * Time.deltaTime);
        tankInstance.transform.Rotate(Vector3.up, turnInput * rotationSpeed * Time.deltaTime);

        // ===== 轮子旋转 =====
        foreach (var wheel in wheels)
        {
            wheel.transform.Rotate(Vector3.right, moveInput * moveSpeed * 50 * Time.deltaTime);
        }

        // ===== 炮台旋转 =====
        float mouseX = Input.GetAxis("Mouse X");
        Tower?.transform.Rotate(Vector3.up, mouseX * turretRotationSpeed);

        // ===== 炮管抬起放下 =====
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        Canon?.transform.Rotate(Vector3.left, scrollInput * cannonElevationSpeed);

        // ===== 相机控制 =====
        if (Input.GetMouseButton(1)) // 右键按住
        {
            float mX = Input.GetAxis("Mouse X");
            float mY = Input.GetAxis("Mouse Y");

            // 水平旋转
            cam.transform.RotateAround(tankInstance.transform.position, Vector3.up, mX * 1000f * Time.deltaTime);

            // 垂直旋转
            cam.transform.RotateAround(tankInstance.transform.position, cam.transform.right, -mY * 1000f * Time.deltaTime);
        }

        // 始终让相机看向坦克
        cam.transform.LookAt(tankInstance.transform);
    }

    /// <summary>
    /// 从父物体里查找子物体，如果找不到报错
    /// </summary>
    private GameObject FindChild(GameObject parent, string name)
    {
        var child = parent.transform.Find(name);
        if (child == null)
        {
            Debug.LogError($"未找到名为 '{name}' 的子物体！");
            return null;
        }
        return child.gameObject;
    }
}
