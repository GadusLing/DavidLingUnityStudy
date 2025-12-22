using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XLua;
using XLua.LuaDLL;

public delegate void CustomCall(); // 定义一个无参无返回值的委托类型
[CSharpCallLua]
public delegate int CustomCall2(int a); // 定义一个有参有返回值的委托类型
[CSharpCallLua]
public delegate int CustomCall3(int a, out int b, out bool c, out string d); // 定义一个有多个返回值的委托类型
[CSharpCallLua]
public delegate int CustomCall4(int a, ref int b, ref bool c, ref string d); // 定义一个有ref参数的委托类型
[CSharpCallLua]
public delegate void CustomCall5(params object[] args); // 定义一个变长参数的委托类型 这里注意，如果你明确知道参数都是某种类型，最好不要用object类型，而是用具体类型的数组，比如int[]、string[]等，这样性能会更好

public class Lesson5_CallFunction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        XLuaManager.Instance.Init(); // 初始化Lua管理器 单例模式 保证唯一性
        XLuaManager.Instance.DoLuaFile("Main"); // 执行Main.lua脚本

        // 无参无返回的获取
        // C#可以用委托类型来接收Lua函数
        CustomCall call = XLuaManager.Instance.Global.Get<CustomCall>("testFun");
        call(); // 调用testFun函数

        UnityAction ua = XLuaManager.Instance.Global.Get<UnityAction>("testFun"); // 用unity自带的委托类型来接收Lua函数也能使用
        ua(); // 调用testFun函数

        Action ac = XLuaManager.Instance.Global.Get<Action>("testFun"); // 用系统自带的委托类型来接收Lua函数也能使用
        ac(); // 调用testFun函数

        LuaFunction lf = XLuaManager.Instance.Global.Get<LuaFunction>("testFun"); // 也可以用XLua提供的LuaFunction类型来接收Lua函数 但是最好少用或者不用 尽量用委托类型
        lf.Call(); // 调用testFun函数
        // 总共有四种方式可以在C#端接收无参无返回的Lua函数 并且调用它们

        // 有参有返回值的获取
        CustomCall2 call2 = XLuaManager.Instance.Global.Get<CustomCall2>("testFun2"); // 注意 这种有参有返回值的函数不同于无参无返回 XLua无法自动推断类型 所以必加上一个[CSharpCallLua]特性标记 
                                                                                      // 并且在XLua的配置文件中注册这个委托类型Unity上方工具栏 -> Xlua -> Generate Code -> CSharpCallLua 中会添加 CustomCall2
        int result = call2(10); // 调用testFun2函数
        Debug.Log("调用Lua函数 testFun2(10) 返回值 = " + result);

        Func<int, int> sFun = XLuaManager.Instance.Global.Get<Func<int, int>>("testFun2"); // 也可以用C#自带的Func委托类型来接收有参有返回值的Lua函数
        result = sFun(20); // 调用testFun2函数
        Debug.Log("调用Lua函数 testFun2(20) 返回值 = " + result);
        //in / out 的作用不是改变程序行为 而是让“本来就安全的代码”不被编译器挡住

        LuaFunction lf2 = XLuaManager.Instance.Global.Get<LuaFunction>("testFun2"); // 也可以用XLua提供的LuaFunction类型来接收Lua函数 但是最好少用或者不用 尽量用委托类型
        object[] res = lf2.Call(30); // 调用testFun2函数 返回值是一个object数组 因为Lua函数可以返回多个值
        Debug.Log("调用Lua函数 testFun2(30) 返回值 = " + res[0]);
        // 总共有三种方式可以在C#端接收有参有返回值的Lua函数 并且调用它们

        // 小总结 无参无返回尽量用UnitAction or Action 有参有返回尽量用Func  自定义委托写的麻烦，LuaFunction少用(说是性能差，没具体研究过)

        // 多返回值的获取
        // 使用 out 和 ref 关键字来接收Lua函数的多个返回值
        CustomCall3 call3 = XLuaManager.Instance.Global.Get<CustomCall3>("testFun3"); // 注意 这种有多个返回值的函数不同于无参无返回 XLua无法自动推断类型 所以必加上一个[CSharpCallLua]特性标记 
                                                                                      // 并且在XLua的配置文件中注册这个委托类型Unity上方工具栏 -> Xlua -> Generate Code -> CSharpCallLua 中会添加 CustomCall3
        int outB;
        bool outC;
        string outD;
        int ret = call3(100, out outB, out outC, out outD); // 调用testFun3函数 这里的out参数 只作为返回值使用 不需要在调用前初始化
        Debug.Log("调用Lua函数 testFun3(100) 返回值 = " + ret + " outB = " + outB + " outC = " + outC + " outD = " + outD);

        CustomCall4 call4 = XLuaManager.Instance.Global.Get<CustomCall4>("testFun3"); // 注意 这种有ref参数的函数不同于无参无返回 XLua无法自动推断类型 所以必加上一个[CSharpCallLua]特性标记 
                                                                                      // 并且在XLua的配置文件中注册这个委托类型Unity上方工具栏 -> Xlua -> Generate Code -> CSharpCallLua 中会添加 CustomCall4
        int refB = 1;
        bool refC = false;
        string refD = "初始值";
        ret = call4(200, ref refB, ref refC, ref refD); // 调用testFun4函数 这里的ref参数和out参数类似 也作为返回值使用 ref可以不在函数内赋值 但是调用前需要初始化 这也是ref和out的区别
        Debug.Log("调用Lua函数 testFun4(200) 返回值 = " + ret + " refB = " + refB + " refC = " + refC + " refD = " + refD);

        LuaFunction lf3 = XLuaManager.Instance.Global.Get<LuaFunction>("testFun3"); // 也可以用XLua提供的LuaFunction类型来接收Lua函数 但是最好少用或者不用 尽量用委托类型
        object[] res3 = lf3.Call(300); // 调用testFun3函数 返回值是一个object数组 因为Lua函数可以返回多个值
        Debug.Log("调用Lua函数 testFun3(300) 返回值 = " + res3[0] + " outB = " + res3[1] + " outC = " + res3[2] + " outD = " + res3[3]);
        // 总共有三种方式可以在C#端接收有多个返回值的Lua函数 并且调用它们

        // 变长参数的获取
        CustomCall5 call5 = XLuaManager.Instance.Global.Get<CustomCall5>("testFun4"); // 注意 这种变长参数的函数不同于无参无返回 XLua无法自动推断类型 所以必加上一个[CSharpCallLua]特性标记 
                                                                                      // 并且在XLua的配置文件中注册这个委托类型Unity上方工具栏 -> Xlua -> Generate Code -> CSharpCallLua 中会添加 CustomCall5
        call5(1, "hello", true, 3.14f, "world", 999); // 调用testFun4函数 传入变长参数
        LuaFunction lf4 = XLuaManager.Instance.Global.Get<LuaFunction>("testFun4"); // 也可以用XLua提供的LuaFunction类型来接收Lua函数 但是最好少用或者不用 尽量用委托类型
        object[] res4 = lf4.Call(2, "xlua", false, 2.71f, "lua", 888); // 调用testFun4函数 返回值是一个object数组 因为Lua函数可以返回多个值
        // 总共有两种方式可以在C#端接收有变长参数的Lua函数 并且调用它们
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
