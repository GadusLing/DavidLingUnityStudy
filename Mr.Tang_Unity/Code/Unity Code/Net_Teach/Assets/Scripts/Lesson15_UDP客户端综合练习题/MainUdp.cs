using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUdp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(UdpNetMgr.Instance == null) // 如果UdpNetMgr实例不存在
        {
            GameObject obj = new GameObject("UdpNetMgr"); // 创建一个新的GameObject
            obj.AddComponent<UdpNetMgr>(); // 给这个GameObject添加UdpNetMgr组件
        }
        UdpNetMgr.Instance.StartClient("124.222.36.67", 8080); // 启动UDP客户端，连接服务器
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
