using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UdpAsyncNetMgr : MonoBehaviour
{
    private static UdpAsyncNetMgr instance;
    public static UdpAsyncNetMgr Instance => instance;

    private EndPoint serverIpPoint;

    private Socket socket;

    // 客户端socket是否关闭
    private bool isClose = true;

    // 接收消息的队列，在多线程里面可以安全操作
    private Queue<BaseMsg> receiveQueue = new Queue<BaseMsg>();

    private byte[] cacheBytes = new byte[512];

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (receiveQueue.Count > 0)
        {
            BaseMsg baseMsg = receiveQueue.Dequeue();
            switch (baseMsg)
            {
                case PlayerMsg msg:
                    print(msg.playerID);
                    print(msg.data.name);
                    print(msg.data.atk);
                    print(msg.data.lev);
                    break;
            }
        }
    }

    /// <summary>
    /// 启动客户端socket的方法
    /// </summary>
    /// <param name="ip">远程服务器的IP</param>
    /// <param name="port">远程服务器的port</param>
    public void StartClient(string ip, int port)
    {
        // 如果当前是开启状态，就不再重复开启
        if (!isClose)
            return;

        // 先记录服务器地址，后面发送消息时会用到 
        serverIpPoint = new IPEndPoint(IPAddress.Parse(ip), port);

        IPEndPoint clientIpPort = new IPEndPoint(IPAddress.Any, 8081);
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(clientIpPort);
            isClose = false;
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.SetBuffer(cacheBytes, 0, cacheBytes.Length);
            args.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            args.Completed += ReceiveMsg;
            socket.ReceiveFromAsync(args);
            print("客户端已启动");
        }
        catch (System.Exception e)
        {
            print("启动Socket失败: " + e.Message);
        }
    }

    private void ReceiveMsg(object obj, SocketAsyncEventArgs args)
    {
        int nowIndex;
        int msgID;
        int msgLength;
        if(args.SocketError == SocketError.Success)
        {
            try
            {
                // 要判断消息来源是否合法
                if (args.RemoteEndPoint.Equals(serverIpPoint))
                {
                    // 解析收到的消息
                    nowIndex = 0;
                    // 消息ID
                    msgID = BitConverter.ToInt32(args.Buffer, nowIndex);
                    nowIndex += 4;
                    // 消息长度
                    msgLength = BitConverter.ToInt32(args.Buffer, nowIndex);
                    nowIndex += 4;
                    // 消息体
                    BaseMsg msg = null;
                    switch (msgID)
                    {
                        case 1001:
                            msg = new PlayerMsg();
                            // 反序列化消息体
                            msg.Reading(args.Buffer, nowIndex);
                            break;
                    }
                    if (msg != null)
                        receiveQueue.Enqueue(msg);
                }
                // 处理完消息后继续接收消息
                if (socket != null && !isClose)
                {
                    args.SetBuffer(0, cacheBytes.Length);
                    socket.ReceiveFromAsync(args);
                }
            }
            catch (SocketException s)
            {
                print("接收消息失败: " + s.SocketErrorCode + s.Message);
                Close();
            }
            catch (Exception e)
            {
                print("接收消息失败(可能是反序列化出错): " + e.Message);
                Close();
            }
        }
        else
        {
            print("接收消息失败: " + args.SocketError);
        }
    }

    // 发送消息
    public void Send(BaseMsg msg)
    {
        try
        {
            if(socket != null && !isClose)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                byte[] bytes = msg.Writing();
                args.SetBuffer(bytes, 0, bytes.Length);
                args.Completed += SendToCallBack;
                // 设置远程目标
                args.RemoteEndPoint = serverIpPoint;
                socket.SendToAsync(args);
            }
        }
        catch (SocketException s)
        {
            print("发送消息失败: " + s.SocketErrorCode + s.Message);
        }
        catch (Exception e)
        {
            print("发送消息失败(可能是序列化出错): " + e.Message);
        }
    }

    private void SendToCallBack(object o, SocketAsyncEventArgs args)
    {
        if (args.SocketError != SocketError.Success)
            print("发送消息失败: " + args.SocketError);
    }

    // 关闭socket
    public void Close()
    {
        if (socket != null)
        {
            isClose = true;
            QuitMsg msg = new QuitMsg();
            // 发送一个退出消息给服务器，便于服务器做下线处理
            socket.SendTo(msg.Writing(), serverIpPoint);
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            socket = null;
        }
    }

    private void OnDestroy()
    {
        Close();
    }
}
