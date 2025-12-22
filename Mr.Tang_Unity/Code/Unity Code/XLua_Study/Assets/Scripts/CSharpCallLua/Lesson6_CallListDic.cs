using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson6_CallListDic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        XLuaManager.Instance.Init(); // 初始化Lua管理器 单例模式 保证唯一性
        XLuaManager.Instance.DoLuaFile("Main"); // 执行Main.lua脚本

        // List中是同一种类型元素的集合
        List<int> list = XLuaManager.Instance.Global.Get<List<int>>("testList"); // 获取Lua中的list表
        foreach(var item in list)
        {
            Debug.Log("从Lua中获取的List元素: " + item);
        }
        list[0] = 999; // 尝试在C#中修改List中的元素 看看Lua中会不会变化
        List<int> list2 = XLuaManager.Instance.Global.Get<List<int>>("testList"); // 再次获取Lua中的list表
        Debug.Log("修改后 再次从Lua中获取的List元素: " + list2[0]); // 并没有改变 说明是值拷贝 不是引用拷贝

        // List中是混合类型元素的集合 什么类型都有 那就用object类型来接收 因为object是所有类型的基类 什么类型都可以接收
        List<object> list3 = XLuaManager.Instance.Global.Get<List<object>>("testList2"); // 获取Lua中的list表
        foreach(var item in list3)
        {
            Debug.Log("从Lua中获取的混合类型List元素: " + item);
        }

        // Dictionary 键值都指定类型
        Dictionary<string, int> dic = XLuaManager.Instance.Global.Get<Dictionary<string, int>>("testDic"); // 获取Lua中的dic表
        foreach(var kvp in dic)
        {
            Debug.Log("从Lua中获取的Dictionary元素: Key = " + kvp.Key + ", Value = " + kvp.Value);
        }
        dic["1"] = 888; // 尝试在C#中修改Dictionary中的元素 看看Lua中会不会变化
        Dictionary<string, int> dic2 = XLuaManager.Instance.Global.Get<Dictionary<string, int>>("testDic"); // 再次获取Lua中的dic表
        Debug.Log("修改后 再次从Lua中获取的Dictionary元素: Key = 1, Value = " + dic2["1"]); // 并没有改变 说明是值拷贝 不是引用拷贝

        // Dictionary 键值不确定类型 混合类型 键值都用object来接收
        Dictionary<object, object> dic3 = XLuaManager.Instance.Global.Get<Dictionary<object, object>>("testDic2"); // 获取Lua中的dic表
        foreach(var kvp in dic3)
        {
            Debug.Log("从Lua中获取的混合类型Dictionary元素: Key = " + kvp.Key + ", Value = " + kvp.Value);
        }








    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
