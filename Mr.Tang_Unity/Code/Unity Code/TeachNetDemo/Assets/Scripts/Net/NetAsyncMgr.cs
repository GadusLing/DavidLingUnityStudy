using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using GamePlayer;
using GameSystem;
using UnityEngine;

public class NetAsyncMgr : MonoBehaviour
{
    private static NetAsyncMgr _instance; // 单例实例
    public static NetAsyncMgr Instance => _instance; // 单例访问器
    private Socket socket; // 用于网络通信的Socket对象
    private byte[] cacheBytes = new byte[1024 * 1024]; // 用于接收消息的缓冲区
    private int cacheNum = 0; // 缓冲区中已经接收的字节数
    private Queue<BaseHandler> receiveQueue = new Queue<BaseHandler>(); // 消息队列
    private int SEND_HEART_MSG_TIME = 2; // 心跳包发送间隔（秒）
    private HeartMsg heartMsg = new HeartMsg(); // 心跳消息对象

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this; // 设置单例实例
            DontDestroyOnLoad(gameObject); // 场景切换时不销毁该对象
            InvokeRepeating("SendHeartMsg", 0, SEND_HEART_MSG_TIME); // 启动心跳包定时器
        }
        else
        {
            Destroy(gameObject); // 已有实例则销毁自己
        }
    }

    void Update()
    {
        // 处理消息队列，每帧检查是否有收到的消息需要处理
        // 目标二：不要每次新加了消息 就在这里去处理对应消息的逻辑
        // 要更加自动化的去处理它们 并且不要在网络层这里来处理
        if (receiveQueue.Count > 0)
        {
            BaseHandler handler = receiveQueue.Dequeue(); // 取出一条消息处理器
            handler.MsgHandle(); // 调用处理器的处理方法
        }
    }

    // 心跳包定时发送
    private void SendHeartMsg()
    {
        // 定时发送心跳包，保持与服务器的连接
        if (socket != null && socket.Connected)
            Send(heartMsg);
    }

    public void Connect(string ip, int port)
    {
        if (socket != null && socket.Connected)
        {
            Debug.Log("已经连接了服务器了，不需要再连接了");
            return;
        }
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port); // 创建服务器终结点
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // 创建Socket对象

        SocketAsyncEventArgs args = new SocketAsyncEventArgs(); // 创建异步事件参数
        args.RemoteEndPoint = ipPoint; // 设置远程终结点
        args.Completed += (socket, args) =>
        {
            if (args.SocketError == SocketError.Success)
            {
                Debug.Log("TCP连接已建立");
                SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs(); // 创建接收消息的异步参数
                receiveArgs.SetBuffer(cacheBytes, 0, cacheBytes.Length); // 设置接收缓冲区
                receiveArgs.Completed += ReceiveCallBack; // 设置接收完成回调
                this.socket.ReceiveAsync(receiveArgs); // 开始异步接收
            }
            else
            {
                Debug.Log("连接服务器失败，错误码: " + args.SocketError);
            }
        };
        socket.ConnectAsync(args); // 异步连接服务器
    }

    private void ReceiveCallBack(object sender, SocketAsyncEventArgs args)
    {
        if (args.SocketError == SocketError.Success)
        {
            int receiveNum = args.BytesTransferred; // 本次接收到的字节数
            if (receiveNum > 0)
            {
                HandleReceiveMsg(receiveNum); // 处理接收到的数据
                args.SetBuffer(cacheNum, args.Buffer.Length - cacheNum); // 重置缓冲区偏移和长度
                if (this.socket != null && this.socket.Connected)
                    socket.ReceiveAsync(args); // 继续异步接收
                else
                    Close(); // 连接断开则关闭
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
            try
            {
                QuitMsg msg = new QuitMsg(); // 创建退出消息
                socket.Send(msg.Writing()); // 发送退出消息
            }
            catch { }
            socket.Shutdown(SocketShutdown.Both); // 关闭发送和接收
            socket.Disconnect(false); // 断开连接
            socket.Close(); // 关闭Socket
            socket = null; // 置空引用
        }
    }

    // 发送BaseMsg消息
    public void Send(BaseMsg msg)
    {
        if (this.socket != null && this.socket.Connected)
        {
            byte[] bytes = msg.Writing(); // 获取消息字节数组
            SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs(); // 创建异步发送参数
            sendArgs.SetBuffer(bytes, 0, bytes.Length); // 设置发送缓冲区
            sendArgs.Completed += (socket, args) =>
            {
                if (args.SocketError != SocketError.Success)
                {
                    Debug.Log("发送消息失败，错误码: " + args.SocketError);
                    Close();
                }
            };
            socket.SendAsync(sendArgs); // 异步发送消息
        }
        else
        {
            Debug.Log("发送消息失败，未连接到服务器");
            Close();
        }
    }

    // 发送原始二进制数据（用于测试粘包、拆包等场景）
    public void SendTest(byte[] bytes)
    {
        SocketAsyncEventArgs args = new SocketAsyncEventArgs(); // 创建异步发送参数
        args.SetBuffer(bytes, 0, bytes.Length); // 设置要发送的字节数组
        args.Completed += (socket, args) =>
        {
            if (args.SocketError != SocketError.Success)
            {
                Debug.Log("发送消息失败，错误码: " + args.SocketError); // 发送失败时打印错误并关闭连接
                Close();
            }
        };
        socket.SendAsync(args); // 异步发送数据
    }

    // 处理接收到的消息 分包 粘包问题的方法
    private void HandleReceiveMsg(int receiveNum) 
    {
        int msgID = 0; // 消息ID
        int msgLength = 0; // 消息长度
        int nowIndex = 0; // 当前解析到的字节索引

        cacheNum += receiveNum; // 更新缓冲区已接收字节数

        while (true)
        {
            msgLength = -1; // 每次循环重置消息长度
            if (cacheNum - nowIndex >= 8) // 至少有8字节才能解析出消息头
            {
                msgID = System.BitConverter.ToInt32(cacheBytes, nowIndex); // 读取消息ID
                nowIndex += 4;
                msgLength = System.BitConverter.ToInt32(cacheBytes, nowIndex); // 读取消息体长度
                nowIndex += 4;
            }

            if (cacheNum - nowIndex >= msgLength && msgLength != -1) // 缓冲区剩余字节足够一条完整消息
            {
                BaseMsg baseMsg = null; // 消息基类
                BaseHandler handler = null; // 消息处理器基类
                // 目标一：不需要手动去添加代码
                // 添加了消息后 根据这个ID 就能自动根据ID得到对应的消息类进行反序列化
                // 要更加自动化
                switch (msgID)
                {
                    case 1001:
                        baseMsg = new PlayerMsg(); // 创建PlayerMsg对象
                        baseMsg.Reading(cacheBytes, nowIndex); // 解析消息体
                        
                        handler = new PlayerMsgHandler(); // 创建对应的消息处理器
                        handler.message = baseMsg; // 将解析好的消息对象赋值给处理器
                        break;
                    // 可扩展更多消息类型
                }
                if (baseMsg != null)
                    receiveQueue.Enqueue(handler); // 入队等待处理
                nowIndex += msgLength; // 移动到下一个消息起始位置
                if (nowIndex == cacheNum)
                {
                    cacheNum = 0; // 缓冲区已全部处理完
                    break;
                }
            }
            else
            {
                if (msgLength != -1)
                    nowIndex -= 8; // 回退到消息头
                System.Array.Copy(cacheBytes, nowIndex, cacheBytes, 0, cacheNum - nowIndex); // 剩余未处理数据前移
                cacheNum = cacheNum - nowIndex; // 更新剩余字节数
                break;
            }
        }
    }

    private void OnDestroy()
    {
        Close(); // 脚本销毁时关闭连接
    }
}
