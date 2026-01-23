using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Lesson1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        byte[] ipAddress = new byte[] { 118, 102, 111, 11 };
        IPAddress ip1 = new IPAddress(ipAddress);

        IPAddress ip2 = new IPAddress(0x76666F0B);

        IPAddress ip3 = IPAddress.Parse("118.102.111.11"); // 推荐使用这种方式

        // 特殊IP地址
        // 127.0.0.1 代表本机地址

        // 获取可用的 IPv6 地址
        // IPAddress.IPv6Any;

        IPEndPoint ipPoint = new IPEndPoint(0x76666F0B, 8080); // 端口号范围 0~65535
        IPEndPoint ipPoint2 = new IPEndPoint(IPAddress.Parse("118.102.111.11"), 8080); // 端口号范围 0~65535



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
