using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface; // 引入ToLua命名空间

public class Lesson1_LuaState : MonoBehaviour
{
    private LuaState lua; // Lua虚拟机对象

    // Start is called before the first frame update
    void Start()
    {
        // 主要学习目的：学会用ToLua提供的解析器（虚拟机）对象执行Lua代码和脚本

        // 初始化一个ToLua解析器(虚拟机)
        LuaState luaState = new LuaState();
        luaState.Start(); // 启动Lua解析器

        // ==========================
        // 1. 执行Lua代码的三种方式
        // ==========================

        // 1.1 DoString方法：直接执行Lua代码（字符串）
        luaState.DoString("print('Hello Lua! This is C# calling Lua code.')");

        // DoString的第二个参数可以指定代码来源，方便调试
        luaState.DoString("print('你好世界')", "Lesson1_LuaState.cs");

        // 1.2 DoFile方法：执行Lua脚本文件
        // 传入的Lua文件名可加可不加.lua后缀
        // 默认会在Assets文件夹下的Lua文件夹中查找Lua脚本
        //luaState.DoFile("Main.lua");

        // 1.3 Require方法：执行Lua脚本文件
        // 必须不加.lua后缀，否则会报错
        luaState.Require("Main");

        // ==========================
        // 2. 销毁解析器相关操作
        // ==========================

        luaState.CheckTop(); // 先执行CheckTop，检查解析器栈顶是否为空
        luaState.Dispose();  // 再执行Dispose，销毁解析器
        luaState = null;     // 释放解析器引用
    }

    void Update()
    {
        // 此处可根据需要每帧与Lua交互
    }
}

/*
总结：
1. DoString方法可直接执行Lua代码字符串。
2. DoFile和Require方法用于执行Lua脚本文件。
   - DoFile传入的文件名可加可不加.lua后缀。
   - Require方法必须不加.lua后缀，否则会报错。
   - 默认会在Assets/Lua文件夹下查找Lua脚本。
3. 销毁Lua解析器时，先CheckTop再Dispose。
4. LuaState和xlua等其他Lua插件的用法有些许不同，主要体现在命名和规则上。
5. 后续的知识讲解建议通过VSCode来进行。
*/
