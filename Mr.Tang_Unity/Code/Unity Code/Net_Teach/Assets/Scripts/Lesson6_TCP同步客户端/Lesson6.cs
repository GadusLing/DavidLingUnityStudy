using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Lesson6 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一——回顾客户端需要做的事情
        //1.创建套接字Socket
        //2.用Connect方法与服务端相连
        //3.用Send和Receive相关方法收发数据
        //4.用Shutdown方法释放连接
        //5.关闭套接字
        #endregion

        #region 知识点二——实现客户端基本逻辑
        //1.创建套接字Socket
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //2.用Connect方法与服务端相连
        //确定服务端的IP和端口
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("124.222.36.67"), 8080); // 连入自己的腾讯云服务器
        try
        {
            socket.Connect(ipPoint);
            Debug.Log("连接服务器成功");
        }
        catch (SocketException ex)
        {
            if(ex.ErrorCode == 10061) // 连接被拒绝，可能是服务器未启动
            {
                Debug.LogError("连接服务器失败: 连接被拒绝，请确保服务器已启动并监听正确的端口");
            }
            else
            {
                Debug.LogError("连接服务器失败: " + ex.Message + "，错误码: " + ex.ErrorCode);
            }
            return;
        }
        //3.用Send和Receive相关方法收发数据
        byte[] buffer = new byte[1024];
        int receiveNum = socket.Receive(buffer);
        Debug.Log("接收到来自服务器的数据: " + Encoding.UTF8.GetString(buffer, 0, receiveNum));
        socket.Send(Encoding.UTF8.GetBytes("你好，我是小凌哥的客户端！"));
        //4.用Shutdown方法释放连接
        socket.Shutdown(SocketShutdown.Both);
        //5.关闭套接字
        socket.Close();
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
