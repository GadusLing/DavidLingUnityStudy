
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson4_CallVariable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ToLuaManager.Instance.Init();// 初始化tolua解析器
        ToLuaManager.Instance.Require("Main");// 执行Lua脚本

        // 获取全局变量 tolua中访问全局变量 -> 得到LuaState 解析器 -> 通过 中括号[变量名] 访问全局变量
        Debug.Log("testNumber: " + ToLuaManager.Instance.LuaState["testNumber"]);
        Debug.Log("testBool: " + ToLuaManager.Instance.LuaState["testBool"]);
        Debug.Log("testFloat: " + ToLuaManager.Instance.LuaState["testFloat"]);
        Debug.Log("testString: " + ToLuaManager.Instance.LuaState["testString"]);

        // 那能不能获取本地local变量呢？ 不能 打印出来是空的 只能获取全局变量
        Debug.Log("testLocal: " + ToLuaManager.Instance.LuaState["testLocal"]);

        int value = Convert.ToInt32(ToLuaManager.Instance.LuaState["testNumber"]);
        value = 99;
        Debug.Log("尝试改变从testNumber获取的value值为99 尝试之后发现testNumber的值为: " + ToLuaManager.Instance.LuaState["testNumber"] + " 说明C#获取到的是值的拷贝 改变value并不会影响Lua中的testNumber");
        ToLuaManager.Instance.LuaState["testNumber"] = 100;
        Debug.Log("通过C#直接将testNumber的值改为100 现在Lua中的testNumber的值为: " + ToLuaManager.Instance.LuaState["testNumber"]);

        // 通过C#给Lua中添加新的全局变量 相当于在Lua中的大G表中添加了一个变量
        Debug.Log("addValue: " + ToLuaManager.Instance.LuaState["addValue"]);
        ToLuaManager.Instance.LuaState["addValue"] = 999;
        Debug.Log("通过C#直接将addValue的值改为999 现在Lua中的addValue的值为: " + ToLuaManager.Instance.LuaState["addValue"]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
// 总结：Lua全局变量获取
    // 1. 通过lua解析器中括号+变量名可获取全局变量。
    // 2. 可以直接修改lua中的全局变量值（赋值即可）。
    // 3. 无法获取local修饰的局部变量。
    // 4. 可以在C#中向lua添加全局变量。