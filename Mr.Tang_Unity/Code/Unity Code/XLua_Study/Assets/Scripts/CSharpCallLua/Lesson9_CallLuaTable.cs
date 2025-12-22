using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class Lesson9_CallLuaTable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        XLuaManager.Instance.Init();
        XLuaManager.Instance.DoLuaFile("Main");

        LuaTable table = XLuaManager.Instance.Global.Get<LuaTable>("testClass");
        Debug.Log("从Lua中获取到的table 中的testInt: " + table.Get<int>("testInt"));
        Debug.Log("从Lua中获取到的table 中的testBool: " + table.Get<bool>("testBool"));
        Debug.Log("从Lua中获取到的table 中的testFloat: " + table.Get<float>("testFloat"));
        Debug.Log("从Lua中获取到的table 中的testString: " + table.Get<string>("testString"));
        table.Get<LuaFunction>("testFun").Call(); // 调用Lua表中的函数
        // 这样看起来很方便 但是连Xlua官方都不推荐用LuaTable 和 LuaFunction去操作Lua表 因为效率比较低，还会产生垃圾，其次是引用类型不好管理
        table.Set("testInt", 888); // 修改Lua表中的字段值
        LuaTable table2 = XLuaManager.Instance.Global.Get<LuaTable>("testClass");
        Debug.Log("修改后的table2的testInt: " + table2.Get<int>("testInt")); // 再次获取这个Lua表 发现testInt字段值已经被修改了
        Debug.Log("修改后的table的testInt: " + table.Get<int>("testInt")); // 
        // ！！！！！！！！！！！！！！！！！！LuaTable是引用类型，修改的是同一个Lua表！！！！！！！！！！！！！！！！！！！！！！！！！
        table.Dispose(); // 用完LuaTable记得Dispose释放掉 避免内存泄漏
        table2.Dispose();
        // 好麻烦 少用 

        // 关于CsharpCallLua 特性的小总结 什么时候需要加这个特性？ 自定义委托 和 接口类型 什么时候不需要加？ 系统自带的委托类型（Action、Func、UnityAction等） 和 XLua提供的LuaFunction类型
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
