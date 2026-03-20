using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class Lesson24 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 HttpWebRequest类
        //命名空间：System.Net
        //HttpWebRequest是主要用于发送客户端请求的类
        //主要用于：发送HTTP客户端请求给服务器，可以进行消息通信、上传、下载等操作

        //重要方法
        //1.Create 创建新的WebRequest, 用于进行HTTP相关操作
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://192.168.1.2:8081/Http_Server/");
        //2.Abort 如果正在进行文件传输, 用此方法可以终止传输
        req.Abort();
        //3.GetRequestStream 获取用于上传的流
        Stream s = req.GetRequestStream();
        //4.GetResponse 返回HTTP服务器响应
        HttpWebResponse res = (HttpWebResponse)req.GetResponse();
        //5.Begin/EndGetRequestStream 异步获取用于上传的流
        req.BeginGetRequestStream(ar =>
        {
            Stream s2 = req.EndGetRequestStream(ar);
            //在这里可以进行上传数据的操作
        }, null);
        //6.Begin/EndGetResponse 异步获取返回的HTTP服务器响应
        req.BeginGetResponse(ar =>
        {
            HttpWebResponse res2 = (HttpWebResponse)req.EndGetResponse(ar);
            //在这里可以进行处理服务器响应的操作
        }, null);

        //重要成员
        //1.Credentials 通信凭证, 设置为NetworkCredential对象
        req.Credentials = new NetworkCredential("username", "password");
        //2.PreAuthenticate 是否预先发送一个身份验证标头, 一般需要进行身份验证时需要将其设置为true
        req.PreAuthenticate = true;
        //3.Headers 传递HTTP头部信息
        req.Headers.Add("Custom-Header", "HeaderValue");
        //4.ContentLength 发送文件/数据信息时需要先设置内容长度
        req.ContentLength = 1024; //假设要发送1024字节的数据
        //5.ContentType 发送的内容进行内容类型的设置
        req.ContentType = "application/json"; //假设要发送JSON数据
        //6.Method 设置请求方法, 常用的有WebRequestMethods.Http.Get/Post等
        // WebRequestMethods.Http类中的常用命令属性：
        // Get    获取请求，一般用于获取数据
        // Post   提交请求，常用于上传数据，同时可以获取响应
        // Head   获取和Get一致的内容，只是只会返回消息头，不会返回具体内容
        // Put    向指定位置上传最新的内容
        // Connect 表示与代理一起使用的 HTTP CONNECT 协议方法，该代理可以动态切换到隧道
        // MkCol  请求在某个 URI（统一资源标识符）指定的位置新建集合
        req.Method = WebRequestMethods.Http.Get;


        // 了解该类的更多信息：https://docs.microsoft.com/zh-cn/dotnet/api/system.net.httpwebrequest?view=net-6.0
        #endregion

        #region 知识点二 HttpWebResponse类
        //命名空间：System.Net
        //它主要用于获取服务器反馈信息的类
        //我们可以通过HttpWebRequest对象中的GetResponse()方法获取
        //当使用完毕时，要使用Close释放

        //重要方法：
        //1.Close: 释放所有资源
        res.Close();
        //2.GetResponseStream: 返回从FTP服务器下载数据的流
        res.GetResponseStream();
        //重要成员：
        //1.ContentLength: 接受型数据的长度
        res.ContentLength = 2048; //假设服务器返回的数据长度为2048字节
        //2.ContentType: 接受数据的类型
        res.ContentType = "application/json"; //假设服务器返回的数据类型为JSON
        //3.StatusCode: HTTP服务器下发的最新状态
        print(res.StatusCode); // 打印服务器返回的状态码，例如200表示成功，404表示未找到等
        //4.StatusDescription: HTTP服务器下发的状态的文本
        print(res.StatusDescription); // 打印服务器返回的状态描述，例如"OK"表示成功，"Not Found"表示未找到等
        //5.BannerMessage: 连接到服务器后接收到HTTP服务器发送的消息

        //6.ExitMessage: HTTP会话结束时由服务器发送的消息

        //7.LastModified: HTTP服务器上该文件的上次修改日期和时间
        print(res.LastModified);

        #endregion

        #region 知识点三 NetworkCredential, Uri, Stream, FileStream类
        //这些类我们在学习Ftp时已经使用过了
        //在HTTP通讯中使用方式不变
        #endregion

        #region 总结
        //Http相关通讯类的使用和Ftp非常类似
        //只有一些细节上的区别
        //之后我们在学习上传下载时再来着重讲解
        #endregion
    }

    // Update is called once per frame
    void Update()
    {

    }
}




