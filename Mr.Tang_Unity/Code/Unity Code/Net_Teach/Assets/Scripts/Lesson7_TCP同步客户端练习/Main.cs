using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(NetManager.Instance == null) // 如果NetManager实例不存在
        {
            GameObject obj = new GameObject("NetManager"); // 创建一个新的GameObject
            obj.AddComponent<NetManager>(); // 给这个GameObject添加NetManager组件
        }
        NetManager.Instance.Connect("124.222.36.67", 8080); // 连接到腾讯云服务器，端口为8080
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
