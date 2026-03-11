using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Lesson12 : MonoBehaviour
{
    private byte[] resultBytes = new byte[1024]; // 接收消息的缓冲区
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 异步方法和同步方法的区别
        // 同步方法:
        // 方法中逻辑执行完毕后，再继续执行后面的方法
        // 异步方法:
        // 方法中逻辑可能还没有执行完毕，就继续执行后面的内容

        // 异步方法的本质
        // 往往异步方法当中都会使用多线程执行某部分逻辑
        // 因为我们不需要等待方法中逻辑执行完毕就可以继续执行下面的逻辑了

        // 注意: Unity中的协同程序中的某些异步方法，有的使用的是多线程有的使用的是迭代器分步执行
        // 关于协同程序可以回顾Unity基础当中讲解协同程序原理的知识点
        #endregion

        #region 知识点二 举例说明异步方法原理
        // 我们以一个异步倒计时方法举例
        // 1. 线程回调
        // CountDownAsync(5, () => { print("倒计时结束"); }); // 调用异步倒计时方法，倒计时5秒钟
        // print("异步执行后的逻辑"); // 这行代码会在倒计时结束之前就执行了

        // 2. async和await 会等待线程执行完毕 继续执行后面的逻辑
        // 相对第一种方式 可以让函数分步执行
        CountDownAsync(5); // 调用异步倒计时方法，倒计时5秒钟
        print("异步执行后的逻辑2");


        #endregion

        #region 知识点三 Socket TCP通信中的异步方法（Begin开头方法）
        // 回调函数参数IAsyncResult
        // IAsyncState 调用异步方法时传入的参数 需要转换
        // IAsyncWaitHandle 用于同步等待
        Socket socketTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // 服务器相关
        // BeginAccept
        // EndAccept
        socketTcp.BeginAccept(AcceptCallback, socketTcp); // 参数1：回调函数，参数2：传入回调函数的参数（可以在回调函数中通过IAsyncResult.AsyncState来获取这个参数）

        // 客户端相关
        // BeginConnect
        // EndConnect
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("124.222.36.67"), 8080);
        socketTcp.BeginConnect(ipPoint, (result) => 
        {
            try
            {
                socketTcp.EndConnect(result); // 结束异步连接，获取连接结果
                print("连接服务器成功");
            }
            catch (SocketException ex)
            {
                print("连接服务器失败，错误码: " + ex.SocketErrorCode + ", 错误信息: " + ex.Message);
            }
        }, socketTcp); // 参数1：服务器的IP和端口，参数2：回调函数，参数3：传入回调函数的参数（可以在回调函数中通过IAsyncResult.AsyncState来获取这个参数）

        // 服务器客户端通用
        // 接收消息
        // BeginReceive
        // EndReceive
        socketTcp.BeginReceive(resultBytes, 0, resultBytes.Length, SocketFlags.None, ReceiveCallback, socketTcp);

        // 发送消息
        // BeginSend
        // EndSend
        byte[] sendBytes = Encoding.UTF8.GetBytes("Hello Server");
        socketTcp.BeginSend(sendBytes, 0, sendBytes.Length, SocketFlags.None, (result) =>
        {
            try
            {
                int length = socketTcp.EndSend(result); // 结束异步发送，获取发送结果（发送的字节数）
                print("发送消息成功，发送了 " + length + " 个字节");
            }
            catch (SocketException ex)
            {
                print("发送消息失败，错误码: " + ex.SocketErrorCode + ", 错误信息: " + ex.Message);
            }
        }, socketTcp); // 参数1：要发送的消息的字节数据，参数2：缓冲区的偏移量，参数3：要发送的消息的字节数，参数4：SocketFlags，参数5：回调函数，参数6：传入回调函数的参数（可以在回调函数中通过IAsyncResult.AsyncState来获取这个参数）



        #endregion

        #region 知识点四 Socket TCP通信中的异步方法2（Async结尾方法）
        // 关键变量类型
        // SocketAsyncEventArgs
        // 它会作为Async异步方法的传入值
        // 我们需要通过它进行一些关键参数的赋值

        // 服务器端
        // AcceptAsync
        SocketAsyncEventArgs e = new SocketAsyncEventArgs();
        e.Completed += (socket, args) =>
        {
            if (args.SocketError == SocketError.Success)
            {
                Socket clientSocket = args.AcceptSocket; // 获取连入的客户端Socket
                print("接收连接成功，客户端IP: " + clientSocket.RemoteEndPoint.ToString());
                // 继续监听其他客户端的连接
                (socket as Socket).AcceptAsync(args); // 参数：SocketAsyncEventArgs对象
            }
            else
            {
                print("接收连接失败，错误码: " + args.SocketError);
            }
        };
        socketTcp.AcceptAsync(e); // 参数：SocketAsyncEventArgs对象

        // 客户端
        // ConnectAsync
        SocketAsyncEventArgs e2 = new SocketAsyncEventArgs();
        e2.Completed += (socket, args) =>
        {
            if (args.SocketError == SocketError.Success)
            {
                print("连接服务器成功");
            }
            else
            {
                print("连接服务器失败，错误码: " + args.SocketError);
            }
        };
        socketTcp.ConnectAsync(e2); // 参数：SocketAsyncEventArgs对象

        // 服务器端和客户端
        // 发送消息
        // SendAsync
        SocketAsyncEventArgs e3 = new SocketAsyncEventArgs();
        e3.SetBuffer(Encoding.UTF8.GetBytes("Hello Server2"), 0, "Hello Server2".Length); // 设置要发送的消息的字节数据，参数2：缓冲区的偏移量，参数3：要发送的消息的字节数
        e3.Completed += (socket, args) =>
        {
            if (args.SocketError == SocketError.Success)
            {
                print("发送消息成功，发送了 " + args.BytesTransferred + " 个字节");
            }
            else
            {
                print("发送消息失败，错误码: " + args.SocketError);
            }
        };
        socketTcp.SendAsync(e3); // 参数：SocketAsyncEventArgs对象

        // 接收消息
        // ReceiveAsync
        SocketAsyncEventArgs e4 = new SocketAsyncEventArgs();
        e4.SetBuffer(new byte[1024 * 1024], 0, 1024 * 1024); // 设置接收消息的缓冲区，参数2：缓冲区的偏移量，参数3：缓冲区的大小
        e4.Completed += (socket, args) =>
        {
            if (args.SocketError == SocketError.Success)
            {
                print("接收消息成功，消息内容: " + Encoding.UTF8.GetString(args.Buffer, args.Offset, args.BytesTransferred)); // 将接收到的字节数据转换成字符串并打印出来
                // 继续接收消息
                args.SetBuffer(0, args.Buffer.Length); // 重置缓冲区的偏移量和大小，为下一次接收消息做好准备
                (socket as Socket).ReceiveAsync(args); // 参数：SocketAsyncEventArgs对象
            }
            else
            {
                print("接收消息失败，错误码: " + args.SocketError);
            }
        };
        #endregion

        #region 总结
        // C#中网络通信 异步方法中 主要提供了两种方案
        // 1. Begin开头的API
        // 内部开多线程，通过回调形式返回结果，需要和End相关方法 配合使用

        // 2. Async结尾的API
        // 内部开多线程，通过回调形式返回结果，依赖SocketAsyncEventArgs对象配合使用
        // 可以让我们更加方便的进行操作

        #endregion
            
    }

    private void AcceptCallback(IAsyncResult result)
    {
        try
        {
            // 获取传入的参数
            Socket s = result.AsyncState as Socket; // 获取服务器的socket
            // 通过调用EndAccept就可以得到连入的客户端Socket
            Socket clientSocket = s.EndAccept(result); // 结束异步连接，获取客户端的socket

            s.BeginAccept(AcceptCallback, s); // 继续监听其他客户端的连接
        }
        catch (SocketException ex)
        {
            print(ex.SocketErrorCode);
        }
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            Socket s = result.AsyncState as Socket; // 获取服务器的socket
            int length = s.EndReceive(result); // 结束异步接收，获取接收结果（接收到的字节数）
            print("接收消息成功，消息内容: " + Encoding.UTF8.GetString(resultBytes, 0, length)); // 将接收到的字节数据转换成字符串并打印出来
            // 继续接收消息
            s.BeginReceive(resultBytes, 0, resultBytes.Length, SocketFlags.None, ReceiveCallback, s); // 参数1：接收消息的缓冲区，参数2：缓冲区的偏移量，参数3：缓冲区的大小，参数4：SocketFlags，参数5：回调函数，参数6：传入回调函数的参数（可以在回调函数中通过IAsyncResult.AsyncState来获取这个参数）
        }
        catch (SocketException ex)
        {
            print("接收消息失败，错误码: " + ex.SocketErrorCode + ", 错误信息: " + ex.Message);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void CountDownAsync(int second, UnityAction callback)
    {
        Thread t = new Thread(() =>
        {
            while (true)
            {
                print("倒计时: " + second);
                second--;
                Thread.Sleep(1000); // 等待1秒钟
                if(second == 0)
                    break;
            }
            callback?.Invoke(); // 倒计时结束后执行回调函数
        });
        t.Start(); // 启动线程
        print("开始倒计时");
    }

    public async void CountDownAsync(int second)
    {
        print("开始倒计时");
        await Task.Run(() =>
        {
            while (true)
            {
                print("倒计时: " + second);
                second--;
                Thread.Sleep(1000); // 等待1秒钟
                if (second == 0)
                    break;
            }
        });
        // await和同步的区别是 主线程不会被卡住，而是“等任务好了再回来”，期间主线程可以做别的事

        print("倒计时结束");
    }












}
