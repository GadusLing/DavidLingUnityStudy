using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

/// <summary>
/// 学习目的 通过ToLuaManager 管理唯一的一个tolua解析器
/// </summary>
public class ToLuaManager : SingletonMonoAuto<ToLuaManager> // 选择继承了Mono的单例基类 之后需要用到Mono的特性
{
    private LuaState luaState;// 定义唯一的tolua解析器
    public void Init()
    {
        // 自定义Lua加载器最后打包的时候再取消注释 再用 不然要一直copy 到Resources 打AB包 很麻烦
        //new LuaCustomLoader();// 添加自定义的lua加载器
        luaState = new LuaState();// 初始化唯一的tolua解析器
        luaState.Start();

        // 后面有些东西没写 当我们讲到对应知识点时 再来写
        // 委托初始化相关
        // 想要C#和 Lua互相访问使用委托 必须要在初始化时把委托工厂初始化 不然无法访问
        DelegateFactory.Init();

        // 协程相关
        // 如果要让Tolua 支持协程 需要添加一个LuaLooper脚本
        LuaLooper loop = gameObject.AddComponent<LuaLooper>();
        loop.luaState = luaState; // 把我们自己声明的解析器和LuaLooper 关联起来

        // lua协程注册
        LuaCoroutine.Register(luaState, this);

        // Lua使用unity中的类相关
        LuaBinder.Bind(luaState); // 把ToLua生成的绑定类 绑定到解析器中

    }

    /// <summary>
    /// 该属性可以让外部获取到 解析器
    /// </summary>
    public LuaState LuaState
    {
        get 
        { 
            return luaState; 
        }
    }

    /// <summary>
    /// 提供一个外部执行Lua语法字符串的方法
    /// </summary>
    /// <param name="str">lua语法字符串</param>
    /// <param name="chunkName">执行出处</param>
    public void DoString(string str, string chunkName = "ToLuaManager")
    {
        luaState.DoString(str, chunkName);
    }

    /// <summary>
    /// 执行指定名字的lua脚本
    /// </summary>
    /// <param name="fileName">lua脚本名</param>
    public void Require(string fileName)
    {
        luaState.Require(fileName);
    }

    /// <summary>
    /// 销毁lua解析器
    /// </summary>
    public void Dispose()
    {
        if (luaState != null)
        {
            luaState.CheckTop();
            luaState.Dispose();
            luaState = null;
        }
    }
}