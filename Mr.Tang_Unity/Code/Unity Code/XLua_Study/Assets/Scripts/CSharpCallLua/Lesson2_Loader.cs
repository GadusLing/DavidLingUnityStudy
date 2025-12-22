using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

public class Lesson2_Loader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LuaEnv env = new LuaEnv();
        //env.DoString("require('Main')");// 上节课说了这种方式后面要加.txt 不方便管理和维护Lua脚本文件

        // 所以需要自定义Loader来加载Lua脚本文件
        
        // xlua 提供的一个“路径重定向”的方法：
        // 允许我们自定义加载 Lua 文件的规则。
        // 当我们执行 Lua 语言 require 时（相当于执行一个 lua 脚本），
        // 它就会执行我们自定义传入的这个函数（MyCustomLoader），
        // 这样我们可以灵活决定 Lua 文件的查找路径、后缀、加密等逻辑。
        env.AddLoader(MyCustomLoader); // 注册自定义Loader，重定向Lua脚本加载方式
        // AddLoader 让你可以完全自定义 Lua 脚本的加载方式，require 时会自动调用 优先用你注册的 Loader 查找脚本，只有都找不到时才用默认resources方式
        
        env.DoString("require('Main')"); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private byte[] MyCustomLoader(ref string filepath) // 这个函数会自动执行 传入的参数是require执行的脚本文件名
    {
        // 通过这个函数内部的逻辑，决定 Lua 文件的查找路径、后缀、加密等逻辑
        string path = Application.dataPath + "/Lua/" + filepath + ".lua"; // 自定义Lua脚本路径和后缀名 这样是不是就不用去手动改Lua脚本的后缀名了？
        Debug.Log(path);

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


}
