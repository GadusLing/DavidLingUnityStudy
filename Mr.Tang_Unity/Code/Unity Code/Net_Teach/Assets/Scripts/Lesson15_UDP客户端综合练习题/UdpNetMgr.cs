using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class UdpNetMgr : MonoBehaviour
{
    private static UdpNetMgr _instance;
    public static UdpNetMgr Instance => _instance;
    private EndPoint serverEndPoint; // 服务器的IP地址和端口号
    private Socket socket; // UDP客户端套接字
    private bool isRunning = false; // 标志位，表示UDP客户端是否正在运行

    //两个容器 队列
    //接受和发送消息的队列 在多线程里面可以操作
    private Queue<BaseMsg> sendQueue = new Queue<BaseMsg>(); // 发送消息的队列
    private Queue<BaseMsg> receiveQueue = new Queue<BaseMsg>(); // 接收消息的队列
    private byte[] cacheBytes = new byte[548]; // 接收消息的缓冲区，大小为548字节，适合广域网环境

    // Start is called before the first frame update
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
        if (receiveQueue.Count > 0) // 如果接收消息的队列中有消息
        {
            BaseMsg msg = receiveQueue.Dequeue(); // 从接收消息的队列中取出一个消息对象
            switch (msg) // 根据消息ID处理不同类型的消息
            {
                case PlayerMsg playerMsg:
                    print("玩家ID: " + playerMsg.playerID +
                          " 名字: " + playerMsg.data.name +
                          " 等级: " + playerMsg.data.lev +
                          " 攻击: " + playerMsg.data.atk);
                    break;
            }
        }
    }

    /// <summary>
    /// 启动UDP客户端
    /// </summary>
    /// <param name="ip">服务器IP地址</param>
    /// <param name="port">服务器端口号</param>
    public void StartClient(string ip, int port)
    {
        if(isRunning) // 如果UDP客户端已经在运行，直接返回，避免重复启动
        {
            Debug.LogWarning("UDP客户端已经在运行");
            return;
        }
        serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port); // 创建服务器的EndPoint对象，保存服务器的IP地址和端口号
        IPEndPoint clientIpPoint = new IPEndPoint(IPAddress.Any, 8081); // 创建客户端的EndPoint对象，绑定到本地任意IP地址和端口号
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); //创建UDP客户端套接字
            socket.Bind(clientIpPoint); //绑定本地端口
            //开始接收消息
            isRunning = true; //设置标志位，表示UDP客户端正在运行
            print("UDP客户端已启动，正在连接服务器: " + ip + ":" + port);
            ThreadPool.QueueUserWorkItem(_ => ReceiveMsg()); //使用线程池异步接收消息，避免阻塞主线程
            ThreadPool.QueueUserWorkItem(_ => SendMsg()); //使用线程池异步发送消息，避免阻塞主线程
        }
        catch (Exception ex)
        {
            Debug.LogError("启动socket失败: " + ex.Message);
        }
    }

    private void ReceiveMsg() // 接收消息的实现
    {
        EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0); // 创建一个EndPoint对象，用于存储发送消息的远程IP地址和端口号
        int nowIndex;
        int msgID;
        int msgLength;

        while (isRunning)
        {
            if (socket != null && socket.Available > 0) // 如果UDP套接字存在并且有可用的数据可以接收
            {
                try
                {
                    int length = socket.ReceiveFrom(cacheBytes, ref remoteEndPoint); // 从UDP套接字接收数据，并获取发送消息的远程IP地址和端口号
                    if (!remoteEndPoint.Equals(serverEndPoint)) // 如果发送消息的远程IP地址和端口号不是服务器的EndPoint，说明是非法消息，丢弃
                    {
                        continue;
                    }
                    nowIndex = 0;
                    msgID = BitConverter.ToInt32(cacheBytes, nowIndex); // 从接收的字节数组中解析出消息ID
                    nowIndex += 4; // 消息ID占4个字节，移动索引
                    msgLength = BitConverter.ToInt32(cacheBytes, nowIndex); // 从接收的字节数组中解析出消息长度
                    nowIndex += 4; // 消息长度占4个字节，移动索引
                    BaseMsg msg = null; // 定义一个BaseMsg对象，用于存储解析出的消息内容
                    switch (msgID) // 根据消息ID创建对应的消息对象，并从接收的字节数组中解析出消息内容，添加到接收消息的队列中
                    {
                        case 1001:
                            msg = new PlayerMsg();
                            msg.Reading(cacheBytes, nowIndex); // 从接收的字节数组中解析出消息内容，填充到PlayerMsg对象中
                            break;
                    }
                    if (msg != null)
                    {
                        receiveQueue.Enqueue(msg); // 将解析出的消息对象添加到接收消息的队列中，等待主线程处理
                    }
                }
                catch (SocketException s)
                {
                    Debug.LogError("接收消息失败: " + s.SocketErrorCode + s.Message);
                }
                catch (Exception ex)
                {
                    Debug.LogError("接收消息失败(非网络问题): " + ex.Message);
                }
            }
        }
    }

    private void SendMsg() // 发送消息的实现
    {
        while (isRunning)
        {
            if (socket != null && sendQueue.Count > 0) // 如果UDP套接字存在并且发送消息的队列中有消息需要发送
            {
                try
                {
                    socket.SendTo(sendQueue.Dequeue().Writing(), serverEndPoint); // 通过UDP套接字发送消息到服务器的EndPoint
                }
                catch (SocketException s)
                {
                    Debug.LogError("发送消息失败: " + s.SocketErrorCode + s.Message);
                }
            }
        }
    }

    public void Send(BaseMsg msg) // 对外提供的发送消息的方法
    {
        if (msg != null)
        {
            sendQueue.Enqueue(msg); // 将需要发送的消息对象添加到发送消息的队列中，等待发送线程处理
        }
    }

    public void Close() //关闭socket
    {
        if(socket != null)
        {
            isRunning = false; //设置标志位，停止接收和发送线程
            QuitMsg quitMsg = new QuitMsg(); //创建一个QuitMsg对象，表示退出消息
            socket.SendTo(quitMsg.Writing(), serverEndPoint); // 通过UDP套接字发送退出消息到服务器的EndPoint，通知服务器客户端即将关闭
            socket.Shutdown(SocketShutdown.Both); //关闭UDP套接字的发送和接收功能，确保所有数据都被发送和接收完成
            socket.Close(); //关闭UDP套接字，释放资源
            socket = null; //将socket对象置空，避免后续使用时出现空引用异常
        }
    }

    public void OnDestroy() //在Unity中，当GameObject被销毁时会调用OnDestroy方法，可以在这里关闭UDP客户端，释放资源
    {
        Close(); //调用Close方法，关闭UDP客户端，释放资源
    }


}
