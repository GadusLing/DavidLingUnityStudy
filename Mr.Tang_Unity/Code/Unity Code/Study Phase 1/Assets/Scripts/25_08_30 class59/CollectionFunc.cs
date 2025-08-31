using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CollectionFunc : MonoBehaviour
{
    /*
     * 题目1：在之前Input和Screen中的练习题基础上
     * 加入一个点击鼠标左键可以发射一颗子弹飞出的功能
     */

    /*
     * 题目2：在上一题的基础上，加入子弹碰到到地面会自动消失的功能
     */

    /*
     * 题目3：在上一题的基础上，在场景加入一些立方体，每个立方体被子弹打3
     * 下就会消失
     */

    #region 预制体引用
    [Header("预制体")]
    public GameObject tank;
    public GameObject tankBullet;
    #endregion

    #region 坦克组件引用
    [Header("坦克组件")]
    private GameObject tankInstance;
    private GameObject Tower;
    private GameObject Canon;
    private GameObject[] wheels;
    private Camera cam;
    #endregion

    #region 移动参数
    [Header("移动参数")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    #endregion

    #region 炮台参数
    [Header("炮台参数")]
    public float turretRotationSpeed = 1f;
    public float cannonElevationSpeed = 10f;
    #endregion

    #region 射击参数
    [Header("射击参数")]
    public float bulletSpeed = 20f;
    #endregion

    #region 立方体目标参数
    [Header("立方体目标")]
    public int cubeCount = 10;                    // 立方体数量
    public float spawnRadius = 20f;               // 生成半径
    public Vector2 cubeScaleRange = new Vector2(1f, 2f); // 立方体大小范围
    private List<GameObject> activeCubes = new List<GameObject>(); // 活跃的立方体列表
    #endregion

    #region Unity生命周期
    void Start()
    {
        InitializeTank();
        SetupCamera();
        SpawnCubes(); // 生成立方体
    }

    void Update()
    {
        if (tankInstance == null || cam == null) return;

        HandleTankMovement();
        HandleTurretControl();
        HandleShooting();
        HandleCameraControl();
    }
    #endregion

    #region 初始化方法
    /// <summary>
    /// 初始化坦克
    /// </summary>
    private void InitializeTank()
    {
        // 实例化坦克
        tankInstance = Instantiate(tank, Vector3.zero, Quaternion.identity);

        // 查找子物体
        Tower = FindChild(tankInstance, "TankFree_Tower");
        Canon = FindChild(Tower, "TankFree_Canon");

        // 初始化轮子数组
        wheels = new GameObject[]
        {
            FindChild(tankInstance, "TankFree_Wheel_b_left"),
            FindChild(tankInstance, "TankFree_Wheel_b_right"),
            FindChild(tankInstance, "TankFree_Wheel_f_left"),
            FindChild(tankInstance, "TankFree_Wheel_f_right"),
        };
    }

    /// <summary>
    /// 设置相机
    /// </summary>
    private void SetupCamera()
    {
        // 创建一台新相机
        GameObject camObj = new GameObject("TankCamera");
        cam = camObj.AddComponent<Camera>();

        // 将相机设置为坦克的子物体
        cam.transform.SetParent(tankInstance.transform);

        // 初始位置
        cam.transform.position = tankInstance.transform.position + new Vector3(0, 2.5f, -5);
        cam.transform.LookAt(tankInstance.transform);
    }

    /// <summary>
    /// 在坦克周围生成立方体
    /// </summary>
    private void SpawnCubes()
    {
        for (int i = 0; i < cubeCount; i++)
        {
            // 随机生成位置（在坦克周围的圆形区域内）
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = tankInstance.transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
            
            // 确保立方体在地面上
            spawnPosition.y = 0.5f; // 立方体高度的一半

            // 创建立方体
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = spawnPosition;
            
            // 随机大小
            float scale = Random.Range(cubeScaleRange.x, cubeScaleRange.y);
            cube.transform.localScale = Vector3.one * scale;
            cube.transform.position = new Vector3(cube.transform.position.x, scale * 0.5f, cube.transform.position.z);

            // 添加目标脚本
            CubeTarget cubeTarget = cube.AddComponent<CubeTarget>();
            
            // 随机旋转
            cube.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            
            // 设置名字和标签
            cube.name = $"TargetCube_{i}";
            cube.tag = "Target";

            // 添加到活跃列表
            activeCubes.Add(cube);
        }

        Debug.Log($"生成了 {cubeCount} 个立方体目标");
    }
    #endregion

    #region 坦克移动控制
    /// <summary>
    /// 处理坦克移动
    /// </summary>
    private void HandleTankMovement()
    {
        float moveInput = Input.GetAxis("Vertical");   // W/S
        float turnInput = Input.GetAxis("Horizontal"); // A/D

        // 坦克移动和转向
        tankInstance.transform.Translate(Vector3.forward * moveInput * moveSpeed * Time.deltaTime);
        tankInstance.transform.Rotate(Vector3.up, turnInput * rotationSpeed * Time.deltaTime);

        // 轮子旋转
        RotateWheels(moveInput);
    }

    /// <summary>
    /// 旋转轮子
    /// </summary>
    private void RotateWheels(float moveInput)
    {
        foreach (var wheel in wheels)
        {
            if (wheel != null)
            {
                wheel.transform.Rotate(Vector3.right, moveInput * moveSpeed * 50 * Time.deltaTime);
            }
        }
    }
    #endregion

    #region 炮台控制
    /// <summary>
    /// 处理炮台控制
    /// </summary>
    private void HandleTurretControl()
    {
        // 炮台水平旋转
        float mouseX = Input.GetAxis("Mouse X");
        Tower?.transform.Rotate(Vector3.up, mouseX * turretRotationSpeed);

        // 炮管垂直抬起放下
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        Canon?.transform.Rotate(Vector3.left, scrollInput * cannonElevationSpeed);
    }
    #endregion

    #region 射击系统
    /// <summary>
    /// 处理射击
    /// </summary>
    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0)) // 左键点击
        {
            FireBullet();
        }
    }

    /// <summary>
    /// 发射子弹
    /// </summary>
    private void FireBullet()
    {
        if (Canon == null || tankBullet == null) return;

        // 计算发射位置和旋转
        Vector3 firePosition = Canon.transform.position;
        Quaternion fireRotation = Canon.transform.rotation;

        // 创建子弹
        GameObject bullet = Instantiate(tankBullet, firePosition, fireRotation);

        // 设置子弹物理
        SetupBulletPhysics(bullet);

        // 自动销毁
        Destroy(bullet, 5f);
    }

    /// <summary>
    /// 设置子弹物理属性
    /// </summary>
    private void SetupBulletPhysics(GameObject bullet)
    {
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb == null)
        {
            bulletRb = bullet.AddComponent<Rigidbody>();
        }

        // 确保子弹有碰撞器
        if (bullet.GetComponent<Collider>() == null)
        {
            bullet.AddComponent<SphereCollider>();
        }

        // 添加碰撞检测组件
        BulletCollision bulletCollision = bullet.AddComponent<BulletCollision>();

        // 设置速度
        bulletRb.velocity = Canon.transform.forward * bulletSpeed;
        // 推荐
        // 子弹用 velocity ✅ - 需要立即速度
        // 坦克用 AddForce ✅ -更真实的物理效果
        // UI / 简单动画用 Translate ✅ -精确控制
    }

    /// <summary>
    /// 子弹碰撞检测组件
    /// </summary>
    private class BulletCollision : MonoBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            // 检查是否碰撞到立方体目标
            CubeTarget cubeTarget = collision.gameObject.GetComponent<CubeTarget>();
            if (cubeTarget != null)
            {
                cubeTarget.OnHit();
                Destroy(gameObject); // 子弹击中目标后销毁
                return;
            }

            // 检查是否碰撞到地面
            if (collision.gameObject.CompareTag("Ground") || 
                collision.transform.position.y <= 0.1f) // 简单的高度检查
            {
                Destroy(gameObject);
            }
        }
    }
    #endregion

    #region 相机控制
    /// <summary>
    /// 处理相机控制
    /// </summary>
    private void HandleCameraControl()
    {
        if (Input.GetMouseButton(1)) // 右键按住
        {
            float mX = Input.GetAxis("Mouse X");
            float mY = Input.GetAxis("Mouse Y");

            // 围绕坦克旋转相机
            RotateCameraAroundTank(mX, mY);
        }

        // 始终让相机看向坦克
        cam.transform.LookAt(tankInstance.transform);
    }

    /// <summary>
    /// 围绕坦克旋转相机
    /// </summary>
    private void RotateCameraAroundTank(float mouseX, float mouseY)
    {
        Vector3 tankPosition = tankInstance.transform.position;
        
        // 水平旋转
        cam.transform.RotateAround(tankPosition, Vector3.up, mouseX * 1000f * Time.deltaTime);
        
        // 垂直旋转
        cam.transform.RotateAround(tankPosition, cam.transform.right, -mouseY * 1000f * Time.deltaTime);
    }
    #endregion

    #region 工具方法
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
    #endregion
}
