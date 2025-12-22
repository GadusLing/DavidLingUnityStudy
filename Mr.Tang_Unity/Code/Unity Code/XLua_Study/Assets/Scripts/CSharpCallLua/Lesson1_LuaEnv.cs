using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua; // 1. 引入 XLua 命名空间
// Lua 解释器的本质是：在 C#（或 Unity）环境中嵌入一个 Lua 虚拟机，直接解释和执行 Lua 脚本。C# 代码和 Lua 代码可以通过接口互相调用
public class Lesson1_LuaEnv : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Lua解析器
        LuaEnv env = new LuaEnv(); // 2. 创建 LuaEnv 实例
        env.DoString("print('Hello Lua from C#')", "Lesson1_LuaEnv"); // 3. 通过 DoString 方法 在C#中也能执行 Lua 打印代码

        env.DoString("require('Main')"); // 直接把代码打到字符串里不方便管理和维护 一般都是通过require加载Lua脚本文件来执行Lua代码
        // 例如：默认寻找脚本的路径是在Resources下 加载方式类似：Resources.Load<TextAsset>("Main")
        // 注意：untiy在Resources下加载只能识别类似.txt 或 .bytes 的后缀名
        // 所以Main.Lua 文件后需要加.txt 或 .bytes 后缀名 变成 Main.lua.txt 或 Main.lua.bytes 才能加载成功
        // 但是这样做肯定不方便 所以后面会介绍更好的Lua脚本管理方式

        env.Tick(); // 4. Tick方法 帮助我们清除Lua中没有手动释放的对象 相当于垃圾回收 一般放在帧更新中定时执行(最好不要每帧执行，定时) 或者 切换场景时执行

        env.Dispose(); // 5. Dispose 方法 释放 Lua 解释器 卸载所有 Lua 脚本和对象 一般在不需要使用 Lua 解释器时调用，比如切换场景时
        // 销毁一般用的很少，因为用解析器时都会保持解析器的唯一性 类似单例 所以一般不会销毁
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
