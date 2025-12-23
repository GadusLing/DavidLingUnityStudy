using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

public class Lesson2_Loader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        new LuaCustomLoader(); // 实例化自定义加载器 使其生效 
        // 之所以new了就能用 是因为父类LuaFileUtils是一个单例对象 并且在LuaFileUtils的构造函数中写了instance = this;
        // 这意味着 每当 new 一个 LuaFileUtils 或其子类对象时，instance 就会被赋值为当前 new 出来的对象（即 this）
        // 所以这里new LuaCustomLoader() 就相当于把 LuaFileUtils 的单例 instance 替换成了我们自定义的加载器对象

        //主要学习目的 学会通过toLua的解析器 自定义解析路径
        LuaState luaState = new LuaState();
        luaState.Start();

        //luaState.Require("Lesson2_Loader"); // 这样写是不行的 因为tolua只会去默认路径Lua下找脚本 不会递归子目录
        //所以有两种解决方案
        // 第一种 如果该文件属于Lua文件夹下 那么可以直接加父目录名称
        luaState.Require("CSharpCallLua/Lesson2_Loader");

        // 第二种 使用lua解析器中的方法 AddSearchPath 添加搜索路径
        // luaState.AddSearchPath(Application.dataPath + "/Lua/CSharpCallLua");
        // 这样就可以直接加载脚本了
        // luaState.Require("Lesson2_Loader");
        // 有加就有减，也可以通过 RemoveSearchPath 来移除路径
        // luaState.RemoveSeachPath(Application.dataPath + "/Lua/CSharpCallLua"); // 用的很少，而且作者好像这里拼写search掉了个r，了解即可

        // // tolua自定义解析方式和xlua不同，xlua是通过 AddLoader 来添加一个委托函数来实现自定义解析

        // // XLua自定义加载器示例：
        // LuaEnv luaEnv = new LuaEnv();
        // luaEnv.AddLoader((ref string filepath) => {
        //     string absPath = Application.dataPath + "/Lua/" + filepath.Replace('.', '/') + ".lua";// 将 require 的路径转为实际文件路径
        //     if (System.IO.File.Exists(absPath))// 判断文件是否存在
        //     {
        //         return System.IO.File.ReadAllBytes(absPath);// 返回文件内容的字节数组
        //     }
        //     // 未找到则返回 null
        //     return null;
        // });
        // luaEnv.DoString("require 'CSharpCallLua.Lesson2_Loader'");// 加载 Lua 脚本

        // 而tolua解析器自定义解析方式和lua源码中LuaFileUtils 的public virtual byte[] ReadFile(string fileName) 方法有关
        // 这是个虚方法，那么也就意味着我们可以继承LuaFileUtils类 重写ReadFile方法来实现自定义解析
        // 我们新创建一个LuaCustomLoader.cs脚本 来实现这个功能
    }

    // Update is called once per frame
    void Update()
    {
        
    } 
}
