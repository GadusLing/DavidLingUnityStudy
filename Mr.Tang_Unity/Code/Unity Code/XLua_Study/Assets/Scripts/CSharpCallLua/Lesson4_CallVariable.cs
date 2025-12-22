using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson4_CallVariable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        XLuaManager.Instance.Init(); // 初始化Lua管理器 单例模式 保证唯一性
        XLuaManager.Instance.DoLuaFile("Main"); // 执行Main.lua脚本

        // 使用lua解析器luaenv 来获取Global属性 —— 大G表中的全局变量
        var i = XLuaManager.Instance.Global.Get<int>("testNumber");
        print("从Lua中获取的全局变量 testNumber = " + i);
        var b = XLuaManager.Instance.Global.Get<bool>("testBool");
        print("从Lua中获取的全局变量 testBool = " + b);
        var f = XLuaManager.Instance.Global.Get<float>("testFloat");
        print("从Lua中获取的全局变量 testFloat = " + f);
        double d = XLuaManager.Instance.Global.Get<double>("testFloat"); // 直接获取float类型的变量 也可以转换成double类型
        print("从Lua中获取的全局变量 testFloat 用double类型接收 = " + d);
        var s = XLuaManager.Instance.Global.Get<string>("testString");
        print("从Lua中获取的全局变量 testString = " + s);

        i = 10;
        var i2 = XLuaManager.Instance.Global.Get<int>("testNumber");
        print("直接修改C#端的i变量后 i = " + i + " Lua端的testNumber变量仍然是 = " + i2);
        // 注意 这里通过get获取的是值类型 只是把Lua端的值复制了一份给C#端的i变量 修改i变量并不会影响Lua端的testNumber变量
        // 想要修改Lua端的全局变量 需要通过Set方法来设置
        XLuaManager.Instance.Global.Set("testNumber", 20); // 修改Lua端的全局变量 testNumber 第一个参数是变量名 第二个参数是要设置的值
        i2 = XLuaManager.Instance.Global.Get<int>("testNumber");
        print("通过Set修改Lua端的testNumber变量后 Lua端的testNumber变量变为 = " + i2);

        var local = XLuaManager.Instance.Global.Get<int>("testLocal");
        //print("从Lua中获取的局部变量 testLocal = " + local);
        // 注意 这里是无法获取到Lua脚本中的局部变量的 因为局部变量的作用域仅限于定义它的代码块内 外部是无法访问到的 所以这里会报错

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
