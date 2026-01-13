using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

// 类
#region Lesson1_CallClass
public class Test // 测试ToLua调用没有继承MonoBehaviour的类
{
    public void Speak(string str)
    {
        Debug.Log("测试ToLua调用没有继承MonoBehaviour的类" + str);
    }
}

namespace DavidLing
{
    public class Test2
    {
        public void Speak(string str)
        {
            Debug.Log("测试ToLua调用没有继承MonoBehaviour且有命名空间的类" + str);
        }
    }
}

#endregion

//枚举
#region Lesson2_CallEnum
public enum E_MyEnum // 测试ToLua调用枚举
{
    Idle,
    Move,
    Atk,
}

#endregion

//数组
#region Lesson3_CallArray List Dictionary
public class Lesson3
{
    public int[] array = new int[] { 1, 2, 3, 4, 5 };

    public List<int> list = new List<int>() { 11, 22, 33, 44, 55 };
    public Dictionary<int, string> dic = new Dictionary<int, string>()
    {
        {1, "one" },
        {2, "two" },
        {3, "three" },
    };
}
#endregion

//拓展方法相关
#region Lesson4 CallFunction
// 复习拓展方法的写法
// C# 扩展方法规则：放在静态类里，方法本身是 static，首参用 this TargetType x 标记被扩展的类型；命名空间需被调用端 using 才能看到。
// 调用时直接 lesson4Instance.Move();，就像实例方法一样。
// 其他小点：扩展方法不能访问目标类型的私有成员；可加更多参数（放在 this 之后）；可对接口、泛型类型写扩展；同名实例方法优先于扩展方法。
public static class Tools
{
    public static void Move(this Lesson4 obj)
    {
        Debug.Log(obj.name + "移动");
    }
}

public class Lesson4
{
    public string name = "小凌哥";
    public void Speak(string str)
    {
        Debug.Log("Lesson4 Speak:" + str);
    }

    public static void Eat()
    {
        Debug.Log("Lesson4 eat");
    }
}

#endregion

#region Lesson5 CallFunction ref out
public class Lesson5
{
    public int RefFunc(int a, ref int b, ref int c, int d)
    {
        b = a + d;
        c = a - d;
        return 100;
    }

    public int OutFunc(int a, out int b, out int c, int d)
    {
        b = a;
        c = d;
        return 200;
    }

    public int RefOutFunc(int a, out int b, ref int c)
    {
        b = a * 10;
        c = a * 20;
        return 300;
    }


    
}


#endregion

#region Lesson6 CallFunction 重载
public class Lesson6
{
    public int Calc()
    {
        Debug.Log("无参Calc");
        return 100;
    }

    public int Calc(int a)
    {
        Debug.Log("单参Calc int");
        return a;
    }

    public float Calc(float a)
    {
        Debug.Log("单参Calc float");
        return a;
    }

    public string Calc(string a)
    {
        Debug.Log("单参Calc string");
        return a;
    }

    public int Calc(int a, int b)
    {
        Debug.Log("双参Calc int int");
        return a + b;
    }

    public int Calc(int a, out int b)
    {
        Debug.Log("带out双参Calc int out int");
        b = 10;
        return a + b;
    }

    // public int Calc(int a, ref int b) // tolua中不要用ref 具体原因参考Lesson6_CallFunction.lua
    // {
    //     b = 10;
    //     return a + b;
    // }

}

#endregion

#region Lesson7 CallDelegate Event
public class Lesson7
{
    public UnityAction del;
    public event UnityAction eventAction;

    public void DoDel()
    {
        if (del != null)
        {
            del();
        }
    }

    public void DoEvent()
    {
        if (eventAction != null)
        {
            eventAction();
        }
    }

    public void ClearEvent()
    {
        eventAction = null;
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
