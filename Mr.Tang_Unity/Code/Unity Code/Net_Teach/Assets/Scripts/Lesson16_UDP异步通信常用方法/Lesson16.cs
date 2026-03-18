using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

public class Lesson16 : MonoBehaviour
{
    private byte[] cacheBytes = new byte[548]; //创建一个字节数组，作为接收消息的缓冲区，大小为548字节，适合广域网环境

    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 Socket UDP通信中的异步方法
        //通过之前的学习，UDP用到的通信相关方法主要就是
        //SendTo和ReceiveFrom
        //所以在讲解UDP异步通信时也主要是围绕着收发消息相关方法来讲解
        #endregion

        #region 知识点二 UDP通信中Begin相关异步方法
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        //BeginSendTo
        byte[] bytes = Encoding.UTF8.GetBytes("Hello, UDP!"); //将要发送的消息转换为字节数组
        EndPoint endPoint = new IPEndPoint(IPAddress.Parse("124.222.36.67"), 8080); //创建一个EndPoint对象，表示服务器的IP地址和端口号
        socket.BeginSendTo(bytes, 0, bytes.Length, SocketFlags.None, endPoint, SendToOver, socket);

        //BeginReceiveFrom
        socket.BeginReceiveFrom(cacheBytes, 0, cacheBytes.Length, SocketFlags.None, ref endPoint, ReceiveFromOver, (socket, endPoint));
        #endregion

        #region 知识点三 UDP通信中Async相关异步方法
        //SendToAsync
        SocketAsyncEventArgs args = new SocketAsyncEventArgs(); //创建一个SocketAsyncEventArgs对象，作为异步发送操作的参数
        args.SetBuffer(bytes, 0, bytes.Length); //设置要发送的消息的缓冲区，指定起始索引和长度
        args.Completed += SendToAsync; //订阅Completed事件，当异步发送操作完成时会触发这个事件，调用SendToAsync方法作为事件处理程序
        socket.SendToAsync(args); //调用SendToAsync方法，开始异步发送消息到服务器的EndPoint

        //ReceiveFromAsync
        SocketAsyncEventArgs args2 = new SocketAsyncEventArgs(); //创建一个SocketAsyncEventArgs对象，作为异步接收操作的参数
        args2.SetBuffer(cacheBytes, 0, cacheBytes.Length); //设置接收缓冲区，指定起始索引和长度
        args2.Completed += ReceiveFromAsync; //订阅Completed事件，当异步接收操作完成时会触发这个事件，调用ReceiveFromAsync方法作为事件处理程序
        socket.ReceiveAsync(args2); // 调用ReceiveAsync方法，开始异步接收消息到服务器的EndPoint




        #endregion
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SendToOver(IAsyncResult result) // 这里的参数类型是规定的，必须是IAsyncResult类型，表示异步操作的结果对象
    {
        try
        {
            //在这个回调方法中，我们可以通过result.AsyncState来获取之前传入的状态对象，如果需要的话
            Socket s = (Socket)result.AsyncState;
            s.EndSendTo(result); //调用EndSendTo方法，结束异步发送操作，获取发送的字节数，如果发生异常会抛出异常
            print("消息发送成功");
        }
        catch (SocketException s)
        {
            Debug.LogError("发送消息时发生异常: " + s.SocketErrorCode + s.Message); //如果发生异常，打印错误信息
        }
    }

    private void ReceiveFromOver(IAsyncResult result) // 这里的参数类型是规定的，必须是IAsyncResult类型，表示异步操作的结果对象
    {
        try
        {
            //在这个回调方法中，我们可以通过result.AsyncState来获取之前传入的状态对象，如果需要的话
            var (s, e) = ((Socket, EndPoint))result.AsyncState; //将状态对象转换为Socket和EndPoint类型的元组，获取UDP套接字和远程EndPoint
            int bytesRead = s.EndReceiveFrom(result, ref e); //调用EndReceiveFrom方法，结束异步接收操作，获取接收的字节数和发送消息的远程EndPoint，如果发生异常会抛出异常
            string msg = Encoding.UTF8.GetString(cacheBytes, 0, bytesRead); //将接收到的字节数组转换为字符串，指定起始索引和长度
            print("收到消息: " + msg + " 来自: " + e.ToString()); //打印接收到的消息和发送消息的远程EndPoint
            s.BeginReceiveFrom(cacheBytes, 0, cacheBytes.Length, SocketFlags.None, ref e, ReceiveFromOver, (s, e)); //继续异步接收消息，避免只接收一次就停止
        }
        catch (SocketException s)
        {
            Debug.LogError("接收消息时发生异常: " + s.SocketErrorCode + s.Message); //如果发生异常，打印错误信息
        }
        catch (Exception e)
        {
            Debug.LogError("接收消息时发生异常: " + e.Message); //如果发生其他类型的异常，打印错误信息
        }

    }

    private void SendToAsync(object s, SocketAsyncEventArgs args) // 这里的参数类型是规定的，必须是object类型和SocketAsyncEventArgs类型，表示异步发送操作的状态对象和参数对象
    {
        try
        {
            if (args.SocketError == SocketError.Success) //如果异步发送操作成功完成
            {
                print("消息发送成功");
            }
            else
            {
                Debug.LogError("发送消息时发生错误: " + args.SocketError); //如果异步发送操作完成但发生错误，打印错误信息
            }
        }
        catch (SocketException se)
        {
            Debug.LogError("发送消息时发生异常: " + se.SocketErrorCode + se.Message); //如果发生异常，打印错误信息
        }
    }
    
    private void ReceiveFromAsync(object s, SocketAsyncEventArgs args) // 这里的参数类型是规定的，必须是object类型和SocketAsyncEventArgs类型，表示异步接收操作的状态对象和参数对象
    {
        try
        {
            if (args.SocketError == SocketError.Success) //如果异步接收操作成功完成
            {
                string msg = Encoding.UTF8.GetString(args.Buffer, args.Offset, args.BytesTransferred); //将接收到的字节数组转换为字符串，指定起始索引和长度
                print("收到消息: " + msg + " 来自: " + args.RemoteEndPoint.ToString()); //打印接收到的消息和发送消息的远程EndPoint
                Socket socket = (Socket)s; //将状态对象转换为Socket类型，获取UDP套接字
                args.SetBuffer(0, cacheBytes.Length); // 重置缓冲区，准备下一次接收操作
                socket.ReceiveFromAsync(args); //继续异步接收消息，避免只接收一次就停止
            }
            else
            {
                Debug.LogError("接收消息时发生错误: " + args.SocketError); //如果异步接收操作完成但发生错误，打印错误信息
            }
        }
        catch (SocketException se)
        {
            Debug.LogError("接收消息时发生异常: " + se.SocketErrorCode + se.Message); //如果发生异常，打印错误信息
        }
        catch (Exception e)
        {
            Debug.LogError("接收消息时发生异常: " + e.Message); //如果发生其他类型的异常，打印错误信息
        }

    }

}
