using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using XLua;

[CSharpCallLua]  // 注意 接口和类的区别就是接口前面要加上这个特性标记 并且去unity工具栏编译Xlua配置 才能让XLua识别到
public interface ITestInterface // 用接口来接Lua中的表有个问题 因为接口里是不允许有成员变量的 所以只能用属性来映射Lua表中的字段
{
    int testInt { get; set; } // 接口里不要写public 默认就是public
    bool testBool { get; set; }
    float testFloat { get; set; }
    string testString { get; set; }
    UnityAction testFun { get; set; } // 用UnityAction来映射Lua表中的函数
    // Tips：接口和类 在内部变量or属性的缺失or多余上是不会报错的 多了少了都没关系 只要你用到的那些变量or属性是存在的就行
    // 但是接口改了内部结构后 如果不重新生成一次XLua的配置代码 会导致运行时报错 所以接口改了之后记得去工具栏清理、编译XLua配置
}

public class Lesson8_CallInterface : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        XLuaManager.Instance.Init();
        XLuaManager.Instance.DoLuaFile("Main");
        ITestInterface obj = XLuaManager.Instance.Global.Get<ITestInterface>("testClass");
        Debug.Log("testInt: " + obj.testInt);
        Debug.Log("testBool: " + obj.testBool);
        Debug.Log("testFloat: " + obj.testFloat);
        Debug.Log("testString: " + obj.testString);
        obj.testFun(); // 调用Lua表中的函数

        obj.testInt = 999; // 修改Lua表中的字段值
        ITestInterface obj2 = XLuaManager.Instance.Global.Get<ITestInterface>("testClass");
        Debug.Log("修改后的testInt: " + obj2.testInt); // 再次获取这个Lua表 发现testInt字段值已经被修改了
        Debug.Log("修改后的testInt: " + obj.testInt);
        // ！！！！！！！！！！！！！！！！！！接口是引用类型，修改的是同一个Lua表！！！！！！！！！！！！！！！！！！！！！！！！！


    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
