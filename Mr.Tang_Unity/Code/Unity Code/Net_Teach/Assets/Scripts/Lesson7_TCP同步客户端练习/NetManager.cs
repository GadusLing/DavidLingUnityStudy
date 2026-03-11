using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class NetManager : MonoBehaviour
{
    private static NetManager _instance;
    public static NetManager Instance => _instance;
    private Socket socket;
    private Queue<BaseMsg> sendMsgQueue = new Queue<BaseMsg>(); // 用于发送消息的队列 公共容器 主线程会不断往里放消息 发送线程不断检查这个队列是否有消息需要发送
    private Queue<BaseMsg> receiveMsgQueue = new Queue<BaseMsg>(); // 用于接收消息的队列 公共容器 接收线程会不断往里放消息 主线程不断检查这个队列是否有消息需要处理
    // private Thread sendThread; // 发送消息的线程;
    // private Thread receiveThread; // 接收消息的线程; 教学示范，正式开发使用线程池
    //byte[] receiveBytes = new byte[1024 * 1024]; // 接收消息的缓冲区 接收线程不断检查这个缓冲区是否有数据可读
    private int receiveNum; // 接收消息的字节数
    private byte[] cacheBytes = new byte[1024 * 1024]; // 用于保存上一次接收时剩余的字节数据，等待下一次接收时把剩余的字节数据补齐成完整的一条消息，再进行解析
    private int cacheNum = 0; // 保存上一次接收时剩余的字节数据的字节数
    private bool isConnected = false; // 是否连接服务器

    private int SEND_HEART_MSG_TIME = 2; // 发送心跳消息的时间间隔，单位毫秒
    private HeartMsg heartMsg = new HeartMsg(); // 心跳消息对象，避免每次发送心跳消息时都创建一个新的心跳消息对象了


    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject); // 不销毁这个对象，保持网络管理器在场景切换时持续存在

        InvokeRepeating(nameof(SendHeartMsg), 0, SEND_HEART_MSG_TIME); // 每隔2秒发送一次心跳消息
    }

    private void SendHeartMsg()
    {
        if(isConnected) // 只有在连接服务器成功了之后才发送心跳消息
            Send(heartMsg); // 通过封装好的Send方法发送心跳消息
    }

    // Update is called once per frame
    void Update()
    {
        if (receiveMsgQueue.Count > 0) // 如果接收队列中有消息
        {
            BaseMsg msg = receiveMsgQueue.Dequeue(); // 从接收队列中取出消息
            if (msg is PlayerMsg) // 根据消息类型进行处理
            {
                PlayerMsg playerMsg = msg as PlayerMsg;
                Debug.Log("接收到PlayerMsg消息，玩家ID: " + playerMsg.playerID);
                Debug.Log("玩家数据 - 姓名: " + playerMsg.data.name + ", 攻击力: " + playerMsg.data.atk + ", 等级: " + playerMsg.data.lev);
            }
        }
    }

    public void Connect(string ip, int port)
    {
        if (socket == null)
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
        }
        catch (SocketException ex)
        {
            if (ex.ErrorCode == 10061) // 连接被拒绝，可能是服务器未启动
            {
                Debug.LogError("连接服务器失败: 连接被拒绝，请确保服务器已启动并监听正确的端口");
            }
            else
            {
                Debug.LogError("连接服务器失败: " + ex.Message + "，错误码: " + ex.ErrorCode);
            }
        }
    }

    public void Send(BaseMsg msg)
    {
        sendMsgQueue.Enqueue(msg); // 将消息放入发送队列
    }

    public void SendTest(byte[] bytes)
    {
        socket.Send(bytes); // 直接发送字节数据，测试分包黏包用
    }

    private void SendMsg()
    {
        while (isConnected)
        {
            if (sendMsgQueue.Count > 0)
            {
                socket.Send(sendMsgQueue.Dequeue().Writing()); // 从队列中取出消息并发送
            }
        }
    }

    private void ReceiveMsg()
    {
        while (isConnected)
        {
            if (socket.Available > 0) // 如果有数据可读
            {
                byte[] receiveBytes = new byte[1024 * 1024]; // 接收消息的缓冲区 接收线程不断检查这个缓冲区是否有数据可读
                receiveNum = socket.Receive(receiveBytes);
                HandleMsg(receiveBytes, receiveNum); // 处理接收到的消息
                // // 把收到字节数组的前4个字节解析为消息ID，根据消息ID创建对应的消息对象，并调用它的Reading方法来解析剩余的字节数据
                // int msgID = BitConverter.ToInt32(receiveBytes, 0); // 从字节数组的前4个字节解析出消息ID
                // BaseMsg baseMsg = null;
                // switch(msgID)
                // {
                //     case 1001:
                //         PlayerMsg msg = new PlayerMsg();
                //         msg.Reading(receiveBytes, 4); // 从第4个字节开始读取剩余数据
                //         baseMsg = msg; // 将解析出的消息对象赋值给baseMsg变量
                //         break;
                // }
                // if(baseMsg == null) // 如果没有解析出消息对象，说明消息ID不合法，不用解析 直接丢弃这条消息，继续接收下一条消息
                //     continue;
                // receiveMsgQueue.Enqueue(baseMsg); // 将解析出的消息对象放入接收队列
            }
        }
    }

    private void HandleMsg(byte[] receiveBytes, int receiveNum)
    {
        int msgID = 0; // 消息类型ID
        int msgLength = 0; // 消息长度
        int curIndex = 0; // 当前解析到字节数组的哪个位置了，初始值为0，随着解析的进行不断向后移动，直到超过接收字节数，说明已经没有完整的消息可解析了

        // 收到消息时检测之前有没有缓存的分包信息，如果有的话，把缓存的分包信息和当前接收的字节数据拼接起来，组成一个新的字节数组来进行解析，这样就能把之前的分包信息补齐成完整的一条消息了
        receiveBytes.CopyTo(cacheBytes, cacheNum); // 把当前接收的字节数组复制到缓存字节数组中，拼接之前的分包信息和当前接收的字节数据
        cacheNum += receiveNum; // 更新缓存字节数

        while (true) // 不断解析来处理黏包的情况，直到当前索引位置超过接收字节数，说明已经没有完整的消息可解析了
        {
            msgLength = -1; // 每次解析前先把消息长度重置为-1，避免上一次解析时的消息长度对本次解析造成干扰
            if (cacheNum - curIndex >= 8) // 如果接收到的字节数减去当前索引位置还大于等于消息头的8个字节，说明至少有一个完整的消息头了，可以解析消息头了
            {
                msgID = BitConverter.ToInt32(cacheBytes, curIndex); // 从字节数组的当前索引位置解析出消息ID
                curIndex += 4; // 索引向后移动4个字节
                msgLength = BitConverter.ToInt32(cacheBytes, curIndex); // 从字节数组的当前索引位置解析出消息长度
                curIndex += 4; // 索引向后移动4个字节
            }
            
            if(cacheNum - curIndex >= msgLength && msgLength != -1) // 如果接收到的字节数减去当前索引位置还大于等于消息长度，并且消息长度不等于-1，说明有一个完整的消息体了，可以解析消息体了
            {
                // 解析消息体本身
                BaseMsg baseMsg = null;
                switch (msgID)
                {
                    case 1001:
                        PlayerMsg msg = new PlayerMsg();
                        msg.Reading(cacheBytes, curIndex); // 从当前索引位置开始读取剩余数据
                        baseMsg = msg; // 将解析出的消息对象赋值给baseMsg变量
                        break;
                }
                if (baseMsg != null)
                    receiveMsgQueue.Enqueue(baseMsg); // 将解析出的消息对象放入接收队列
                curIndex += msgLength; // 索引向后移动消息长度个字节，为下一条消息的解析做好准备
                if(curIndex == cacheNum) // 如果当前索引位置正好等于接收字节数，说明已经没有完整的消息可解析了，直接跳出循环
                {
                    cacheNum = 0; // 清空缓存字节数
                    break;
                }
            }
            else // 如果接收到的字节数减去当前索引位置还小于消息长度，说明没有完整的消息体了
            {
                // // 保存分包信息的碎片信息，等待下一次接收时把剩余的字节数据补齐成完整的一条消息，再进行解析
                // receiveBytes.CopyTo(cacheBytes, 0); // 把当前接收的字节数组复制到缓存字节数组中
                // cacheNum = receiveNum; // 保存当前接收的字节数到缓存字节数变量中   

                if(msgLength != -1) // 如果消息长度不等于-1，说明已经解析出了消息头了，当前索引位置后面剩余的字节数就是分包信息的碎片了
                {
                    curIndex -= 8; // 索引回退到消息头的起始位置，把消息头和分包信息的碎片一起保存到缓存字节数组中，等待下一次接收时把剩余的字节数据补齐成完整的一条消息，再进行解析
                }
                Array.Copy(cacheBytes, curIndex, cacheBytes, 0, cacheNum - curIndex); // 把当前索引位置后面剩余的字节数据复制到缓存字节数组的起始位置，等待下一次接收时把剩余的字节数据补齐成完整的一条消息，再进行解析
                cacheNum = cacheNum - curIndex; // 更新缓存字节数为当前索引位置后面剩余的字节数，等待下一次接收时把剩余的字节数据补齐成完整的一条消息，再进行解析
                break;
            }
        }
    }

    public void Close()
    {
        if (socket != null)
        {
            print("客户端主动断开连接");
            // QuitMsg quitMsg = new QuitMsg(); // 创建一个退出消息对象
            // socket.Send(quitMsg.Writing()); // 发送退出消息给服务器，通知服务器客户端要断开连接了 这里不能用封装好的Send方法了，因为Send方法会把消息放入发送队列
            // // 发送线程去发送，接下来马上关闭可能会导致发送线程还没来得及发送这个退出消息，连接就已经被关闭了，
            // // 这样服务器就收不到这个退出消息了，所以只能直接调用socket.Send方法来发送退出消息了
            // socket.Shutdown(SocketShutdown.Both); // 关闭发送和接收
            // socket.Disconnect(false); // 断开连接，参数表示是否允许重用这个socket
            // socket.Close(); // 关闭连接
            // isConnected = false; // 设置连接状态为断开
            socket = null;
        }
    }

    private void OnDestroy()
    {
        Close(); // 在对象销毁时关闭网络连接
    }
}
