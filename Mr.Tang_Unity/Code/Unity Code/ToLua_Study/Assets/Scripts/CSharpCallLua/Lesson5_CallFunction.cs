using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using UnityEngine.Events;

public delegate int CustomCallOut(int a, out int b, out bool c, out string d, out int e);// 自定义多返回值委托

public class Lesson5_CallFunction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ToLuaManager.Instance.Init();// 初始化tolua解析器
        ToLuaManager.Instance.Require("Main");// 执行Lua脚本

        #region 无参无返回函数调用
        // 第一种方法 通过GetFunction 获取Lua函数的委托调用
        LuaFunction function = ToLuaManager.Instance.LuaState.GetFunction("testFun"); // 获取Lua中的test函数
        function.Call();// 调用无参无返回的函数
        function.Dispose();// 释放函数资源 避免内存泄漏

        // 第二种方法 通过 中括号+函数名 访问Lua函数 并调用
        LuaFunction function2 = ToLuaManager.Instance.LuaState["testFun"] as LuaFunction;
        function2.Call();// 调用无参无返回的函数
        function2.Dispose();// 释放函数资源 避免内存泄漏

        // 第三种方式 转为委托调用 首先得到一个luafunction 然后转为对应的委托类型
        LuaFunction function3 = ToLuaManager.Instance.LuaState.GetFunction("testFun");
        UnityAction action = function3.ToDelegate<UnityAction>();// 转为无参无返回的委托
        action();// 调用委托
        function3.Dispose();// 释放函数资源 避免内存泄漏
        #endregion

        #region 有参有返回函数调用
        // 第一种方法 通过luaFunction 的 Call 方法调用
        function = ToLuaManager.Instance.LuaState.GetFunction("testFun2"); // 获取Lua中的test函数
        function.BeginPCall(); // 开始调用 有返回值的函数需要使用BeginPCall 和 EndPCall包裹
        function.Push(99); // 压入参数
        function.PCall(); // 调用函数
        int ret = (int)function.CheckNumber(); // 这里需要的是个数字 所以用CheckNumber 获取返回值
        function.EndPCall(); // 结束调用
        Debug.Log("有参有返回函数Call调用结果:" + ret);
        function.Dispose();// 释放函数资源 避免内存泄漏

        // 第二种方法 通过 LuaFunction 的 Invoke 方法调用
        function = ToLuaManager.Instance.LuaState.GetFunction("testFun2"); // 获取Lua中的test函数
        ret = function.Invoke<int, int>(199); // 最后一个泛型参数是返回值类型 前面的是参数类型
        Debug.Log("有参有返回函数Invoke调用结果:" + ret);
        function.Dispose();// 释放函数资源 避免内存泄漏

        // 第三种方法 转为委托调用
        function = ToLuaManager.Instance.LuaState.GetFunction("testFun2");
        Func<int, int> func = function.ToDelegate<Func<int, int>>(); // 转为有参有返回的委托
        ret = func(900); // 调用委托
        Debug.Log("有参有返回函数委托调用结果:" + ret);
        function.Dispose();// 释放函数资源 避免内存泄漏

        // 第四种方法 直接通过解析器调用
        //ToLuaManager.Instance.LuaState.Invoke<int, int>("testFun2", 800, true); // 直接通过函数名调用 有返回值的函数
        Debug.Log("有参有返回函数解析器调用结果:" + ToLuaManager.Instance.LuaState.Invoke<int, int>("testFun2", 800, true));


        #endregion

        #region 多返回值函数调用
        // 第一种方法 通过luaFunction 的 Call 方法调用
        function = ToLuaManager.Instance.LuaState.GetFunction("testFun3"); // 获取Lua中的test函数
        function.BeginPCall(); // 开始调用 有返回值的函数需要使用BeginPCall 和 EndPCall包裹
        function.Push(555); // 压入参数
        function.PCall(); // 调用函数
        // 获取所有返回值
        int a1 = (int)function.CheckNumber();
        int b1 = (int)function.CheckNumber();
        bool c1 = function.CheckBoolean();
        string d1 = function.CheckString();
        int e1 = (int)function.CheckNumber();
        function.EndPCall(); // 结束调用
        Debug.Log("多返回值函数Call调用结果:");
        Debug.Log(a1);
        Debug.Log(b1);
        Debug.Log(c1);
        Debug.Log(d1);
        Debug.Log(e1);
        function.Dispose();// 释放函数资源 避免内存泄漏

        // 第二种方法 通过 out 来接收多个返回值
        function = ToLuaManager.Instance.LuaState.GetFunction("testFun3"); // 获取Lua中的test函数
        CustomCallOut customCallOut = function.ToDelegate<CustomCallOut>(); // 转为有参多返回值的委托
        // 如果是自定义委托用来装lua 一定要做一件事儿
        // 在自定义设置文件 CustomSetting 中加上委托 并用工具栏工具Generate生成一次代码
        int b2, e2;
        bool c2;
        string d2;
        int a2 = customCallOut(777, out b2, out c2, out d2, out e2); // 调用委托
        Debug.Log("多返回值函数委托调用结果:");
        Debug.Log(a2);
        Debug.Log(b2);
        Debug.Log(c2);
        Debug.Log(d2);
        Debug.Log(e2);
        function.Dispose();// 释放函数资源 避免内存泄漏

        #endregion



    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
}
