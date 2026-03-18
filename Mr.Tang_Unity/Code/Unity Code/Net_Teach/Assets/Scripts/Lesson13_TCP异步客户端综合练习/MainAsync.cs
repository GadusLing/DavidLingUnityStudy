using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAsync : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(NetAsyncMgr.Instance == null) // 如果NetAsyncMgr实例不存在
        {
            GameObject obj = new GameObject("NetAsyncMgr"); // 创建一个新的GameObject
            obj.AddComponent<NetAsyncMgr>(); // 给这个GameObject添加NetAsyncMgr组件
        }

        NetAsyncMgr.Instance.Connect("124.222.36.67", 8080); // 连接服务器
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
