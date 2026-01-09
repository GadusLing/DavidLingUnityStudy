using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Lesson1_CallClass
public class Test // 测试ToLua调用没有继承MonoBehaviour的类
{
    public void Speak(string str)
    {
        Debug.Log("测试ToLua调用没有继承MonoBehaviour的类" + str);
    }
}
#endregion

#region Lesson2_CallEnum
public enum E_MyEnum // 测试ToLua调用枚举
{
    Idle,
    Move,
    Atk,
}

#endregion

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
