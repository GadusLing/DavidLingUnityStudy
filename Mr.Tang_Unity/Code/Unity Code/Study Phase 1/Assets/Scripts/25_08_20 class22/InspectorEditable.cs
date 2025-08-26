using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Inspector面板编辑题目：
 * 1. 如何让公共成员不在Inspector面板上设置
 *    如何让私有或保护成员可以在Inspector面板上设置
 * 
 * 2. 为什么加不同的特性，在Inspector窗口上会有不同的效果
 *    请说出你的理解
 */

public class InspectorEditable : MonoBehaviour
{
    // 题目1解答：

    // 1.1 让公共成员不在Inspector面板上显示 - 使用 [HideInInspector]
    [HideInInspector]
    public int publicButHidden = 100;

    // 普通公共成员（会在Inspector中显示）
    public string normalPublicField = "这会显示在Inspector中";

    // 1.2 让私有成员在Inspector面板上显示 - 使用 [SerializeField]
    [SerializeField]
    private int privateButVisible = 50;

    [SerializeField]
    private float privateFloat = 3.14f;

    // 普通私有成员（不会在Inspector中显示）
    private string normalPrivateField = "这不会显示在Inspector中";

    // 保护成员同样可以使用 [SerializeField] 显示
    [SerializeField]
    protected bool protectedButVisible = true;

    // 题目2解答 - 不同特性的效果：

    // [Range] - 创建滑动条
    [Range(0, 100)]
    public int sliderValue = 50;

    // [Header] - 创建标题分组
    [Header("玩家属性")]
    public int playerHealth = 100;
    public float playerSpeed = 5.0f;

    // [Space] - 添加空白间距
    [Space(10)]
    public string playerName = "Player";

    // [Tooltip] - 添加提示信息
    [Tooltip("这是玩家的经验值")]
    public int experience = 0;

    // [TextArea] - 多行文本输入框
    [TextArea(3, 5)]
    public string description = "输入描述信息...";

    // [Multiline] - 简单多行文本
    [Multiline]
    public string simpleMultiline = "简单多行文本";

    void Start()
    {
        // 演示访问这些字段
        Debug.Log($"隐藏的公共字段值: {publicButHidden}");
        Debug.Log($"可见的私有字段值: {privateButVisible}");
        Debug.Log($"普通私有字段值: {normalPrivateField}");
    }

    void Update()
    {
        
    }

    /*
     * 答案总结：
     * 
     * 1. 控制Inspector显示：
     *    - [HideInInspector]: 隐藏公共成员
     *    - [SerializeField]: 显示私有/保护成员
     * 
     * 2. 特性产生不同效果的原理：
     *    Unity的Inspector系统通过反射机制读取这些特性(Attribute)，
     *    然后根据不同的特性类型来渲染不同的UI控件：
     *    
     *    - [Range]: 渲染为滑动条控件
     *    - [Header]: 渲染为标题文本
     *    - [Space]: 添加垂直间距
     *    - [Tooltip]: 添加鼠标悬停提示
     *    - [TextArea]: 渲染为可调整大小的文本区域
     *    - [Multiline]: 渲染为固定的多行文本框
     *    
     *    这些特性实际上是元数据，Unity编辑器在构建Inspector界面时
     *    会读取这些元数据并相应地调整UI显示效果。
     */
}
