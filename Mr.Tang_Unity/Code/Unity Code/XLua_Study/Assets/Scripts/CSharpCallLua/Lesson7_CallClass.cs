using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CallLuaClass //在这个类中去声明成员变量  名字一定要和Lua那边的一样  一定要是公共的  私有和保护没办法赋值
{
    public int testInt;
    public bool testBool;
    //public float testFloat;
    public string testString;
    public UnityAction testFun; // 声明一个委托类型的成员变量 用于接收Lua传过来的函数
    public CallLuaInClass testInClass; // 声明一个嵌套类 用于接收Lua中的嵌套表


    // 在C#这边自定义的这个类里的变量可以更多也可以更少 只要名字和类型对应上就行 多了无非就是Lua那边没有赋值 忽略 少了就是Lua那边有赋值 但是C#这边用不上 也会忽略
    public void Test()// 多写两个变量 测试Lua那边没有赋值的情况
    {
    }
    public int i;// 多写两个变量 测试Lua那边没有赋值的情况
    
}

public class CallLuaInClass // 这个类用于测试在C#中调用Lua中的嵌套类
{
    public int testInInt;
    public string testInBool;

}


public class Lesson7_CallClass : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        XLuaManager.Instance.Init(); // 初始化Lua管理器 单例模式 保证唯一性
        XLuaManager.Instance.DoLuaFile("Main"); // 执行Main.lua脚本

        // 用类去访问 装载Lua中的表
        CallLuaClass luaClass = XLuaManager.Instance.Global.Get<CallLuaClass>("testClass"); // 通过Global表的Get方法 获取Lua中的myClass表 并转换成C#的CallLuaClass类
        Debug.Log("testInt: " + luaClass.testInt);
        Debug.Log("testBool: " + luaClass.testBool);
        //Debug.Log("testFloat: " + luaClass.testFloat);
        Debug.Log("testString: " + luaClass.testString);
        luaClass.testFun(); // 调用Lua传过来的函数

        luaClass.testInt = 200; // 尝试修改Lua传过来的变量值
        CallLuaClass luaClass2 = XLuaManager.Instance.Global.Get<CallLuaClass>("testClass"); // 再次获取Lua中的myClass表
        Debug.Log("修改后的testInt: " + luaClass2.testInt); // 输出修改后的值 发现并没有变化 说明是值传递 不是引用传递

        // 测试嵌套类
        CallLuaInClass inClass = luaClass.testInClass; // 通过上面获取到的luaClass对象去访问它的嵌套类成员变量
        Debug.Log("testInInt: " + inClass.testInInt);
        Debug.Log("testInBool: " + inClass.testInBool);



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
