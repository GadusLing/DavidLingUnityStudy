using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

public class Lesson6_CallTable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 主要学习目标 tolua如何在C#调用lua中自定义的table
        ToLuaManager.Instance.Init();
        ToLuaManager.Instance.Require("Main");

        // 通过LuaState.GetTable获取lua中的table
        LuaTable table = ToLuaManager.Instance.LuaState.GetTable("testClass");
        Debug.Log(table["testInt"]); // 通过key获取值
        Debug.Log(table["testString"]);
        Debug.Log(table["testFloat"]);
        // LuaFunction func = table["testFun"] as LuaFunction; // 除了这种方式 还可以通过table.GetLuaFunction("testFun")获取函数
        LuaFunction func = table.GetLuaFunction("testFun");
        func.Call();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
