using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson3_LuaMgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ToLuaManager.Instance.Init();// 初始化tolua解析器
        ToLuaManager.Instance.Require("CSharpCallLua/Lesson2_Loader");// 执行Lua脚本
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
// ================= ToLuaManager 作用总结 =================
// 帮助我们构造一个唯一的toLua解析器，通过ToLuaManager来进行管理
// 主要作用：
//   - 提供了执行lua代码和脚本的方法，提供了销毁的方法，提供了获取解析器属性的方法
//   - 提供了初始化的方法
// 外部使用：
//   - 都是通过ToLuaManager来进行统一的调用
// ===================================================
