using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

#region Lesson1_类的使用相关
public class Test
{
    public void Speak(string str)
    {
        Debug.Log("test1 Speak:" + str);
    }
}

namespace MrLing
{
    public class Test2
    {
        public void Speak(string str)
        {
            Debug.Log("Test2 Speak:" + str);
        }
    }
}
#endregion

#region Lesson2_枚举的使用相关
/// <summary>
/// 自定义测试枚举
/// </summary>
public enum E_MyEnum
{
    Idle,
    Move,
    Attack
}
#endregion

#region Lesson3_数组和字典的使用相关
public class Lesson3
{
    public int[] array = new int[5] { 1, 2, 3, 4, 5 };
    public List<int> list = new List<int>();

    public Dictionary<int, string> dict = new Dictionary<int, string>();

}
#endregion

#region Lesson4_使用C#拓展方法相关

//想要在Lua中使用拓展方法一定要在工具类前面加上特定的特性 [XLua.LuaCallCSharp] 建议在lua中要使用的类都加上这个特性 可以提升性能
//如果不加该特性 除了拓展方法对应的类 其它类的使用都不会报错
//但是Lua是通过反射的机制去调用的C#类 效率会比较低 加上该特性 再在 Unity 菜单栏点击 XLua/Generate Code 会生成对应的桥接代码 就不会用反射机制去调用了 性能会提升很多
// “桥接代码” = XLua 自动生成的一层 C# 包装代码
// 作用：让 Lua 调 C# 时不走反射，直接走强类型函数调用，提升性能
[XLua.LuaCallCSharp] 
public static class Tools // 拓展方法怎么写还记得嘛？——在静态类中写静态方法，第一个参数用this修饰，表示要拓展的类型
{
    public static void Move(this Lesson4 obj) // Lesson4 的拓展方法 
    {
        Debug.Log(obj.name + "移动");
    }
}

public class Lesson4
{
    public string name = "小凌哥";

    public void Speak(string str)
    {
        Debug.Log(name + "说: " + str);
    }

    public static void Eat()
    {
        Debug.Log("Eat");
    }
}


#endregion

#region Lesson5_lua中 如何使用ref和out关键字
public class Lesson5
{
    public int RefFun(int a, ref int b, ref int c, int d) // 设置两个不是ref的参数 和 两个ref参数
    {
        b = a + d;
        c = a - d;
        return 100;
    }

    public int OutFun(int a, out int b, out int c, int d) // 设置两个不是out的参数 和 两个out参数
    {
        b = a;
        c = d;
        return 200;
    }

    public int RefOutFun(int a, ref int b, out int c) // 设置一个ref参数 和 一个out参数 一个正常参数
    {
        b = a * 10;
        c = a * 20;
        return 300;
    }

}


#endregion

#region Lesson6_函数重载相关
public class Lesson6
{
    public int Calc()
    {
        return 100;
    }

    public int Calc(int a, int b)
    {
        return a + b;
    }

    public int Calc(int a)
    {
        return a;
    }

    public float Calc(float a)
    {
        return a;
    }

}

#endregion

#region Lesson7_委托和事件相关

public class Lesson7
{
    public UnityAction del;
    public event UnityAction eventAction;

    public void DoEvent()
    {
        if (eventAction != null)
        {
            eventAction();
        }
    }

    public void clearEvent()
    {
        eventAction = null;
    }


}

#endregion

#region Lesson8_二维数组遍历
public class Lesson8
{
    public int[,] array = new int[2, 3]
    {
        {1,2,3 },
        {4,5,6 }
    };

}


#endregion

#region Lesson9_空对象的判断
// 第三种写法 第一种是在lua里写equals 第二种是写一个lua全局isNull函数
// 为Object类写一个扩展方法 IsNull
[XLua.LuaCallCSharp]
public static class Lesson9
{
    public static bool IsNull(this UnityEngine.Object obj)
    {
        return obj == null;
    }
}



#endregion

#region Lesson10_系统类型加特性

public static class Lesson10
{
    [XLua.CSharpCallLua]
    public static List<Type> csharpCallLua = new List<Type>()
    {
        typeof(UnityAction<float>), 
    };

    [XLua.LuaCallCSharp]
    public static List<Type> luaCallCSharp = new List<Type>()
    {
        typeof(GameObject),
        typeof(Rigidbody),
    };
    // 总结：
        // 1. [XLua.CSharpCallLua] 和 [XLua.LuaCallCSharp] 是 XLua 用于配置 Lua 与 C# 互操作的特性。
        // 2. 理论上，推荐每个被 Lua 调用的 C# 类都加上 [XLua.LuaCallCSharp] 特性，可以提升性能。
        // 3. 但对于系统类（如 GameObject）或第三方库的类，我们无法直接在源码上加特性。
        // 4. 所以 XLua 支持在静态类中用 List<Type> 的方式集中声明这些类型，并加上特性，达到同样效果。
        // 5. 这样所有需要导出的类型都能统一管理，方便维护和生成桥接代码。
        // 6. 每次修改这些配置后，都需要在 Unity 菜单栏点击 XLua/Generate Code 重新生成代码。
}

#endregion


#region Lesson12_泛型相关
public class Lesson12
{
    public interface ITest
    {

    }
    public class TestFather // 两个测试类主要用来加约束
    {

    }
    public class TestChild : TestFather, ITest
    {

    }


    public void TestFun1<T>(T a, T b) where T : TestFather
    {
        Debug.Log("testFun1 有参数有泛型约束 T : TestFather");
    }

    public void TestFun2<T>(T a)
    {
        Debug.Log("testFun2 有参数无泛型约束");
    }

    public void TestFun3<T>() where T : TestFather
    {
        Debug.Log("testFun3 无参数有泛型约束 T : TestFather");
    }
    
    public void TestFun4<T>(T a) where T : ITest
    {
        Debug.Log("testFun4 有参数有泛型约束 接口 T : ITest");
    }


}


#endregion


public class LuaCallCSharp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
