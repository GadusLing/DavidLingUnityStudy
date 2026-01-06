using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 作为C#的主入口来启动Lua脚本
/// </summary>
public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ToLuaManager.Instance.Init();
        ToLuaManager.Instance.Require("Main");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
