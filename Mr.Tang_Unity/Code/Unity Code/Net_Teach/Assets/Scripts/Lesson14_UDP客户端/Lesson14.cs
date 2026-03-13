using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Lesson14 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 实现UDP客户端通信 收发字符串
        //1.创建套接字
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        // 注意这里的后两个参数和TCP不同，TCP的SocketType是Stream，ProtocolType是Tcp 意思是流式协议，TCP协议；
        // 而UDP的SocketType是Dgram，ProtocolType是Udp 意思是数据报协议，UDP协议

        // 流式协议和数据报协议的区别：
        // 流式协议是面向连接的，数据报协议是无连接的；
        // 流式协议是可靠的，数据报协议是不可靠的；
        // 流式协议是有序的，数据报协议是无序的；
        // 流式协议是面向字节流的，数据报协议是面向消息的。(面向字节流的意思是数据以字节为单位进行传输，面向消息的意思是数据以消息为单位进行传输)


        //2.绑定本机地址
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8080);
        socket.Bind(ipPoint);

        //3.发送到指定目标
        IPEndPoint remoteIpPoint = new IPEndPoint(IPAddress.Parse("124.222.36.67"), 8081);
        socket.SendTo(Encoding.UTF8.GetBytes("小凌哥来辣！"), remoteIpPoint);

        //4.接受消息
        byte[] buffer = new byte[548]; // 548 和 1472 是UDP协议中常用的两种数据报大小，分别对局域网和广域网环境。548字节适合广域网，1472字节适合局域网。
        EndPoint remoteIpPoint2 = new IPEndPoint(IPAddress.Any, 0); // 接收时，会把实际的远程IP地址和端口号填充到这个EndPoint中，主要作为容器，所以参数没有实际意义，填Any和0即可
        int length = socket.ReceiveFrom(buffer, ref remoteIpPoint2);
        string receivedMessage = Encoding.UTF8.GetString(buffer, 0, length);
        Debug.Log("IP " + (remoteIpPoint2 as IPEndPoint).Address.ToString() + "port:" + (remoteIpPoint2 as IPEndPoint).Port + " 发来了: " + receivedMessage);

        
        //5.释放关闭
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
