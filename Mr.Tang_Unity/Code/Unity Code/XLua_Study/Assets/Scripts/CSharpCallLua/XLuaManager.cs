using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using XLua;

/// <summary>
/// 基于XLua的Lua管理器
/// 提供Lua解析器 保证解析器的唯一性
/// </summary>
public class XLuaManager : SingletonBase<XLuaManager>
{
    private XLuaManager() { } // 继承SingletonBase后 必须手动写一个私有的无参构造函数
    public LuaEnv luaEnv; // XLua的Lua解析器

    /// <summary>
    /// 得到Lua中的_G全局表 大G表
    /// </summary>
    public LuaTable Global// 全局表
    {
        get
        {
            if(luaEnv == null)
            {
                Debug.LogError("XLuaManager未初始化，无法获取GlobalTable");
                return null;
            }
            return luaEnv.Global;
        }
    }

    public void Init() // 用于初始化创建Lua解析器
    {
        if(luaEnv != null) // 已经存在Lua解析器了
            return; // 直接返回
        luaEnv = new LuaEnv();
        luaEnv.AddLoader(MyCustomLoader); // 注册自定义Loader，重定向Lua脚本加载方式
        luaEnv.AddLoader(MyCustomABLoader); // 未来我们会通过加载AB包再加载其中的Lua脚本资源 来执行它
        
    }

    private byte[] MyCustomLoader(ref string filepath) // 这个函数会自动执行 传入的参数是require执行的脚本文件名
    {
        // 通过这个函数内部的逻辑，决定 Lua 文件的查找路径、后缀、加密等逻辑
        string path = Application.dataPath + "/Lua/" + filepath + ".lua"; // 自定义Lua脚本路径和后缀名 这样是不是就不用去手动改Lua脚本的后缀名了？

        if (File.Exists(path)) // 判断path路径的文件是否存在
        {
            return File.ReadAllBytes(path); // 返回Lua脚本的二进制数据
        }
        else
        {
            Debug.LogError("没有找到Lua文件: " + path);
        }

        return null;
    }

    // Lua脚本会放在AB包
    // 最终我们会通过加载AB包再加载其中的Lua脚本资源 来执行它
    // AB包中如果要加载本文 后缀还是有一定的限制，.lua不能被识别
    // 打包时 要把.lua文件后缀改为txt
    private byte[] MyCustomABLoader(ref string filepath)
    {
        // Debug.Log("通过AB包加载Lua脚本: ");
        // // 从AB包中加载Lua脚本资源
        // // 加载AB包
        // string path = Application.streamingAssetsPath + "/lua";
        // AssetBundle ab = AssetBundle.LoadFromFile(path);
        // // 加载Lua文件
        // TextAsset tx = ab.LoadAsset<TextAsset>(filepath + ".lua");
        // // 加载lua文件 的二进制数据 (byte数组)
        // if (tx != null)
        //     return tx.bytes;
        // else
        //     Debug.LogError("没有找到Lua文件: " + filepath + " 在AB包中");
        // return null;
        // 优化版 通过ABManager来加载Lua脚本资源
        TextAsset lua = ABManager.Instance.LoadRes<TextAsset>("lua", filepath + ".lua");
        if (lua != null)
            return lua.bytes;
        else
            Debug.LogError("没有找到Lua文件: " + filepath + " 在AB包中");
        return null;
    }

    /// <summary>
    /// 为了不用每次调用都写require 这里封装一个方法
    /// 传入Lua脚本文件名 自动拼接require来执行
    /// </summary>
    public void DoLuaFile(string fileName)
    {
        if (luaEnv == null)
        {
            Debug.LogError("XLuaManager未初始化，无法执行Lua脚本");
            return;
        }
        string str = string.Format("require('{0}')", fileName);
        DoString(str);
    }


    /// <summary>
    /// 用解析器来执行Lua脚本
    /// </summary>
    /// <param name="luaScript"></param>
    public void DoString(string luaScript)
    {
        if(luaEnv == null)
        {
            Debug.LogError("XLuaManager未初始化，无法执行Lua脚本");
            return;
        }
        luaEnv.DoString(luaScript);
        
    }

    /// <summary>
    /// 释放Lua 垃圾
    /// </summary>
    public void Tick()
    {
        if(luaEnv == null)
        {
            Debug.LogError("XLuaManager未初始化，无法执行Lua脚本垃圾回收");
            return;
        }
        luaEnv.Tick();
    }

    /// <summary>
    /// 销毁Lua解析器
    /// </summary>
    public void Dispose()
    {
        if (luaEnv == null)
        {
            Debug.LogWarning("XLuaManager未初始化，无需释放");
            return;
        }
        luaEnv.Dispose();
        luaEnv = null;
    }

}