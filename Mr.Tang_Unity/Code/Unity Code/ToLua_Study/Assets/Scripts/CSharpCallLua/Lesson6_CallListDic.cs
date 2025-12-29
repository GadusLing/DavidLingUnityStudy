using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

public class Lesson6_CallListDic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 主要学习目标 tolua如何在C#调用table表现的List和Dictionary
        ToLuaManager.Instance.Init();
        ToLuaManager.Instance.Require("Main");

        // List 相关
        // 不同于Xlua可以通过Get获取表映射到不同的结构中 Tolua中 C#得到Lua中的表 都只有一种方法 通过LuaTable来获取
        LuaTable table = ToLuaManager.Instance.LuaState.GetTable("testList");
        Debug.Log("testList count: " + table.Length); // 获取table的长度
        for (int i = 1; i <= table.Length; i++) // 注意Lua的table是从1开始索引的
        {
            Debug.Log("testList " + i + ": " + table[i]); // 通过索引获取值
        }

        LuaTable table2 = ToLuaManager.Instance.LuaState.GetTable("testList2");
        Debug.Log("testList2 count: " + table2.Length); // 获取table的长度
        for (int i = 1; i <= table2.Length; i++) // 注意Lua的table是从1开始索引的
        {
            Debug.Log("testList2 " + i + ": " + table2[i]); // 通过索引获取值
        }
        // 还有一种方式遍历 通过ToArray转换成数组
        object[] arr = table2.ToArray();
        for (int i = 0; i < arr.Length; i++)
        {
            Debug.Log("testList2 array " + i + ": " + arr[i]);
        }

        // 那么现在问题来了，如果直接改[i]的值 会怎么样呢？
        table[1] = 999; // 直接赋值 是值 还是引用呢
        LuaTable tableTmp = ToLuaManager.Instance.LuaState.GetTable("testList");
        Debug.Log("After modify testList 1: " + tableTmp[1]); // 结果发现是引用 修改table的值 会影响到Lua中的table

        // Dictionary 相关
        LuaTable dic = ToLuaManager.Instance.LuaState.GetTable("testDic");
        Debug.Log("testDic key1: " + dic["name"]); // 通过key获取值
        Debug.Log("testDic key2: " + dic["age"]);
        Debug.Log("testDic key3: " + dic["isMale"]);

        LuaTable dic2 = ToLuaManager.Instance.LuaState.GetTable("testDic2");
        Debug.Log("testDic2 key1: " + dic2["Lua"]); // 通过key获取值
        Debug.Log("testDic2 key2: " + dic2["CSharp"]);
        Debug.Log("testDic2 key3: " + dic2["Unity3D"]);
        //Debug.Log("testDic2 key4: " + dic2["true"]);// 想要通过[]中括号得到值 只能获取int 和 string类型的key 其他类型的key需要转换成string才能获取到对应的值
        LuaDictTable<object, object> luaDic2 = dic2.ToDictTable<object, object>(); // 通过ToDictTable转换成字典
        Debug.Log("testDic2 key4: " + luaDic2[true]); // 通过字典的key获取值

        // 小结 如果键确定是int或者string类型的 可以直接通过LuaTable的[]获取值 否则需要转换成字典来获取值
        // dic遍历建议使用迭代器遍历
        IEnumerator<LuaDictEntry<object, object>> ie = luaDic2.GetEnumerator();
        while (ie.MoveNext())
        {
            var entry = ie.Current;
            Debug.Log("testDic2 iterator key: " + entry.Key + " value: " + entry.Value);
        }
        // 字典也是引用吗？
        dic["name"] = "改改看"; // 直接赋值 是值 还是引用呢
        LuaTable dicTmp = ToLuaManager.Instance.LuaState.GetTable("testDic");
        Debug.Log("After modify testDic name: " + dicTmp["name"]); // 结果发现是引用 修改dic的值 会影响到Lua中的dic


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
