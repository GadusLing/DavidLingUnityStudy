using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 为什么要写一个Main 脚本给Lua调用呢？ 因为Lua没办法直接访问C# 一定是先从C#调用Lua脚本 然后再把核心逻辑交个Lua去编写
/// 启动U3d -> C#脚本(Main.cs) -> Lua脚本(Main.lua) -> Lua脚本核心逻辑
/// </summary>
public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        XLuaManager.Instance.Init(); // 初始化XLua管理器 单例模式
        XLuaManager.Instance.DoLuaFile("Main"); // 执行Main.lua脚本
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
