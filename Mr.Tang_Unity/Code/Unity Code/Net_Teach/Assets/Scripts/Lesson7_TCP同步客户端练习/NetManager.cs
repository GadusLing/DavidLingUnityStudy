using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class NetManager : MonoBehaviour
{
    private static NetManager _instance;
    public static NetManager Instance => _instance;
    private Socket socket;
    private Queue<string> sendMsgQueue = new Queue<string>(); // 用于发送消息的队列 公共容器 主线程会不断往里放消息 发送线程不断检查这个队列是否有消息需要发送
    private Queue<string> receiveMsgQueue = new Queue<string>(); // 用于接收消息的队列 公共容器 接收线程会不断往里放消息 主线程不断检查这个队列是否有消息需要处理
    // private Thread sendThread; // 发送消息的线程;
    // private Thread receiveThread; // 接收消息的线程; 教学示范，正式开发使用线程池
    byte[] receiveBytes = new byte[1024 * 1024]; // 接收消息的缓冲区 接收线程不断检查这个缓冲区是否有数据可读
    private int receiveNum; // 接收消息的字节数
    private bool isConnected = false; // 是否连接服务器

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject); // 不销毁这个对象，保持网络管理器在场景切换时持续存在
    }

    // Update is called once per frame
    void Update()
    {
        if(receiveMsgQueue.Count > 0) // 如果接收队列中有消息
        {
            string msg = receiveMsgQueue.Dequeue(); // 从接收队列中取出消息
            Debug.Log("收到服务器消息: " + msg); // 打印接收到的消息
        }
    }

    public void Connect(string ip, int port)
    {
        if(socket == null)
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        try
        {
            socket.Connect(ipPoint); // 连接服务器
            isConnected = true; // 设置连接状态为已连接
            ThreadPool.QueueUserWorkItem(_ => SendMsg()); // 使用线程池执行发送消息的线程
            ThreadPool.QueueUserWorkItem(_ => ReceiveMsg()); // 使用线程池执行接收消息的线程
            // sendThread = new Thread(SendMsg); // 创建发送消息的线程
            // sendThread.Start(); // 启动发送消息的线程
            // receiveThread = new Thread(ReceiveMsg); // 创建接收消息的线程
            // receiveThread.Start(); // 启动接收消息的线程
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
        }
    }

    public void Send(string info)
    {
        sendMsgQueue.Enqueue(info); // 将消息放入发送队列
    }

    private void SendMsg()
    {
        while(isConnected)
        {
            if(sendMsgQueue.Count > 0)
            {
                socket.Send(Encoding.UTF8.GetBytes(sendMsgQueue.Dequeue())); // 从队列中取出消息并发送
            }
        }
    }

    private void ReceiveMsg()
    {
        while(isConnected)
        {
            if(socket.Available > 0) // 如果有数据可读
            {
                receiveNum = socket.Receive(receiveBytes);
                receiveMsgQueue.Enqueue(Encoding.UTF8.GetString(receiveBytes, 0, receiveNum)); // 将接收到的消息解析为字符串并放入接收队列
            }
        }
    }

    public void Close()
    {
        if(socket != null)
        {
            socket.Shutdown(SocketShutdown.Both); // 关闭发送和接收
            socket.Close(); // 关闭连接
            isConnected = false; // 设置连接状态为断开
            socket = null;
        }
    }

    private void OnDestroy()
    {
        Close(); // 在对象销毁时关闭网络连接
    }
}
