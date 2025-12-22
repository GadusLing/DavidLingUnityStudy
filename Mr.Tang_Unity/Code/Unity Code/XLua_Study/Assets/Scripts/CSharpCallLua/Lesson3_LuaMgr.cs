using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson3_LuaMgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        XLuaManager.Instance.Init(); // 初始化Lua管理器 单例模式 保证唯一性
        XLuaManager.Instance.DoLuaFile("Main"); // 执行Main.lua脚本
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
