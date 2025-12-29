using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

public class Lesson8_CallCoroutine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ToLuaManager.Instance.Init();
        ToLuaManager.Instance.Require("Main");

        LuaFunction function = ToLuaManager.Instance.LuaState.GetFunction("StartDelay");
        function.Call(this.gameObject, 3f);
        function.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
