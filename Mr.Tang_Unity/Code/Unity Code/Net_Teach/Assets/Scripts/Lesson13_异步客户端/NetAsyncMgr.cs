using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetAsyncMgr : MonoBehaviour
{
    private static NetAsyncMgr _instance;
    public static NetAsyncMgr Instance => _instance;
    private Socket socket; // 用于网络通信的Socket对象
    private byte[] cacheBytes = new byte[1024 * 1024]; // 用于接收消息的缓冲区
    private int cacheNum = 0; // 缓冲区中已经接收的字节数
    private bool isVerified = false; // 是否已经通过服务器验证了

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Connect(string ip, int port)
    {
        // 连接服务器的逻辑
        if (socket != null && socket.Connected) // 如果已经连接了服务器了，就不需要再连接了
        {
            Debug.Log("已经连接了服务器了，不需要再连接了");
            return;
        }
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        SocketAsyncEventArgs args = new SocketAsyncEventArgs(); // 创建一个SocketAsyncEventArgs对象
        args.RemoteEndPoint = ipPoint; // 设置远程服务器的IP和端口
        args.Completed += (socket, args) => // 设置连接完成后的回调函数
        {
            if (args.SocketError == SocketError.Success) // 如果连接成功了
            {
                Debug.Log("TCP连接已建立，等待服务器验证...");
                Send("hello"); // 发送一条消息
                // 收消息
                SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs(); // 创建一个SocketAsyncEventArgs对象
                receiveArgs.SetBuffer(cacheBytes, 0, cacheBytes.Length); // 设置接收消息的缓冲区
                receiveArgs.Completed += ReceiveCallBack; // 设置接收消息完成后的回调函数
                this.socket.ReceiveAsync(receiveArgs); // 开始异步接收消息，参数是同一个SocketAsyncEventArgs对象，接下来在这个回调函数中就可以通过args来获取接收到的消息了
            }
            else
            {
                Debug.Log("连接服务器失败，错误码: " + args.SocketError);
            }
        };
        socket.ConnectAsync(args);


    }

    private void ReceiveCallBack(object sender, SocketAsyncEventArgs args)
    {
        if (args.SocketError == SocketError.Success) // 如果接收消息成功了
        {
            int receiveNum = args.BytesTransferred; // 获取接收到的消息的字节数
            if (receiveNum > 0) // 如果接收到的消息的字节数大于0了
            {
                Debug.Log("接收消息成功，接收了 " + receiveNum + " 个字节");
                // 处理接收到的消息，解析成我们需要的数据结构
                // 这里就不进行具体的解析了，直接把接收到的字节数据转换成字符串打印出来了
                Debug.Log("接收到的消息内容: " + Encoding.UTF8.GetString(args.Buffer, 0, receiveNum));
                args.SetBuffer(0, args.Buffer.Length); // 重置缓冲区的偏移量和大小，为下一次接收消息做好准备
                if (this.socket != null && this.socket.Connected) // 只有在连接服务器成功了之后才继续接收消息了
                    socket.ReceiveAsync(args); // 继续异步接收消息，参数是同一个SocketAsyncEventArgs对象，接下来在这个回调函数中就可以通过args来获取接收到的消息了
                else
                    Close();
            }
            else
            {
                Debug.Log("服务器关闭了连接");
                Close();
            }
        }
        else
        {
            Debug.Log("接收消息失败，错误码: " + args.SocketError);
            Close();
        }
    }

    public void Close()
    {
        if (socket != null)
        {
            socket.Shutdown(SocketShutdown.Both); // 关闭发送和接收
            socket.Disconnect(false); // 断开连接，参数表示是否允许重用这个socket
            socket.Close();
            socket = null;
        }
    }

    public void Send(string str)
    {
        if (this.socket != null && this.socket.Connected) // 只有在连接服务器成功了之后才发送消息了
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str); // 将要发送的消息转换成字节数组
            SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs(); // 创建一个SocketAsyncEventArgs对象

            sendArgs.SetBuffer(bytes, 0, bytes.Length); // 设置要发送的消息的缓冲区，参数2：缓冲区的偏移量，参数3：要发送的消息的字节数
            sendArgs.Completed += (socket, args) => // 设置发送消息完成后的回调函数
            {
                if (args.SocketError == SocketError.Success)
                {
                    Debug.Log("发送消息成功，发送了 " + args.BytesTransferred + " 个字节");
                }
                else
                {
                    Debug.Log("发送消息失败，错误码: " + args.SocketError);
                    Close();
                }
            };
            socket.SendAsync(sendArgs); // 开始异步发送消息，参数是同一个SocketAsyncEventArgs对象，接下来在这个回调函数中就可以通过args来获取发送结果了
        }
        else
        {
            Debug.Log("发送消息失败，未连接到服务器");
            Close();
        }
    }
}
