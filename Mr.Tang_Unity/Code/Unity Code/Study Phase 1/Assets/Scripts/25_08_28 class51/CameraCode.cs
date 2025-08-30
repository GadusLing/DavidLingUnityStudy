using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCode : MonoBehaviour
{
    /*
     * ========================================
     * 题目1：游戏画面中央有一个立方体，请将该立方体的世界坐标系位置，转换为屏幕坐标，并打印出来
     * ========================================
     * 
     * 实现思路：
     * 1. 获取立方体的世界坐标位置 (transform.position)
     * 2. 使用相机的 WorldToScreenPoint() 方法将世界坐标转换为屏幕坐标
     * 3. 使用 Debug.Log() 打印屏幕坐标
     * 
     * 关键知识点：
     * - Camera.WorldToScreenPoint(Vector3 worldPos) 方法
     * - 屏幕坐标系：左下角为(0,0)，右上角为(Screen.width, Screen.height)
     * - Z值表示距离相机的深度
     */

    /*
     * ========================================
     * 题目2：在屏幕上点击一下鼠标，则在对应的世界坐标位置创建一个Cube出来
     * ========================================
     * 
     * 实现思路：
     * 1. 检测鼠标点击事件 (Input.GetMouseButtonDown(0))
     * 2. 获取鼠标在屏幕上的位置 (Input.mousePosition)
     * 3. 使用相机的 ScreenToWorldPoint() 方法将屏幕坐标转换为世界坐标
     * 4. 在该世界坐标位置实例化一个Cube (GameObject.CreatePrimitive 或 Instantiate)
     * 
     * 关键知识点：
     * - Input.GetMouseButtonDown(0) 检测鼠标左键点击
     * - Input.mousePosition 获取鼠标屏幕坐标
     * - Camera.ScreenToWorldPoint(Vector3 screenPos) 方法
     * - 需要设置合适的Z值来确定深度
     * - GameObject.CreatePrimitive(PrimitiveType.Cube) 创建立方体
     */

    // 用于题目1的立方体引用
    [Header("题目1 - 坐标转换")]
    public GameObject targetCube;

    // 用于题目2的Cube预制体
    [Header("题目2 - 点击生成")]
    public GameObject cubePrefab;
    
    // 生成Cube的距离（从相机出发的距离）
    public float spawnDistance = 10f;

    // Start is called before the first frame update
    void Start()
    {
        // 如果没有指定立方体，尝试在场景中找一个
        if (targetCube == null)
        {
            targetCube = GameObject.FindGameObjectWithTag("Player");
            if (targetCube == null)
            {
                // 创建一个测试用的立方体
                targetCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                targetCube.name = "TestCube";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 题目1：实时转换立方体的世界坐标为屏幕坐标并打印
        ConvertWorldToScreenPosition();
        
        // 题目2：检测鼠标点击并在点击位置生成Cube
        HandleMouseClickToSpawnCube();
    }

    /// <summary>
    /// 题目1实现：将立方体的世界坐标转换为屏幕坐标并打印
    /// </summary>
    private void ConvertWorldToScreenPosition()
    {
        if (targetCube != null)
        {
            // 获取立方体的世界坐标
            Vector3 worldPosition = targetCube.transform.position;
            
            // 将世界坐标转换为屏幕坐标
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            
            // 打印结果（每帧打印会很频繁，可以加间隔控制）
            if (Time.frameCount % 60 == 0) // 每60帧打印一次
            {
                Debug.Log($"立方体世界坐标: {worldPosition}, 屏幕坐标: {screenPosition}");
            }
        }
    }

    /// <summary>
    /// 题目2实现：检测鼠标点击并在对应世界坐标位置生成Cube
    /// </summary>
    private void HandleMouseClickToSpawnCube()
    {
        // 检测鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            // 获取鼠标屏幕坐标
            Vector3 mouseScreenPosition = Input.mousePosition;
            
            // 设置Z值（距离相机的深度）
            mouseScreenPosition.z = spawnDistance;
            
            // 将屏幕坐标转换为世界坐标
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            
            // 在该位置生成Cube
            GameObject newCube;
            if (cubePrefab != null)
            {
                // 如果有预制体，使用预制体
                newCube = Instantiate(cubePrefab, worldPosition, Quaternion.identity);
            }
            else
            {
                // 否则创建基础立方体
                newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                newCube.transform.position = worldPosition;
            }

            // 为生成的Cube添加标识
            newCube.name = $"ClickedCube_{Time.time:F2}";

            Debug.Log($"在屏幕坐标 {Input.mousePosition} 对应的世界坐标 {worldPosition}; 生成了一个Cube");
        }
    }
}
