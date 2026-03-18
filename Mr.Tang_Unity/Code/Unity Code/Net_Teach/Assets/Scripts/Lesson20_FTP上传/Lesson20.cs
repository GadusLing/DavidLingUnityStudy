using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class Lesson20 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        #region 知识点一——使用FTP上传文件关键点
        // 1. 通信凭证
        //    进行ftp连接操作时需要的账号密码

        // 2. 操作命令 WebRequestMethods.Ftp
        //    设置你想要进行的Ftp操作

        // 3. 文件流相关 FileStream 和 Stream
        //    上传和下载时都会使用的文件流

        // 4. 保证FTP服务器已经开启
        //    并且能够正常访问
        #endregion

        #region 知识点二——FTP上传
        try
        {
            // 1. 创建一个Ftp连接
            FtpWebRequest req = FtpWebRequest.Create(new Uri("ftp://192.168.1.2/pic.png")) as FtpWebRequest;
            // 2. 设置信息凭证（如果不支持匿名登录 就必须设置这）
            req.Proxy = null; // 将代理相关信息置空 避免服务器同时有http相关的代理信息和ftp相关的代理信息时 可能会导致连接失败
            req.Credentials = new NetworkCredential("David", "123456");
            // 请设置KeepAlive是否保持连接，如果想要关闭，可以设置为false
            req.KeepAlive = false;
            // 3. 设置操作命令
            req.Method = WebRequestMethods.Ftp.UploadFile; // 上传文件
            // 4. 指定传输类型
            req.UseBinary = true; // 二进制传输
            // 5. 得到用于上传的流对象
            var upLoadStream = req.GetRequestStream();

            // 6. 开始上传
            using (FileStream file = File.OpenRead(Application.streamingAssetsPath + "/test.png"))
            {
                // 我们可以一点一点的把这个文件中的字节数组读取出来 然后存入到上传流中
                byte[] bytes = new byte[1024];

                // 返回值 是真正从文件中读了多少个字节
                int contentLength = file.Read(bytes, 0, bytes.Length);
                // 不停的去读取文件中的字节 除非读取完毕了 不然一直读 并且写入到上传流中
                while (contentLength > 0)
                {
                    // ...这里可以写入上传流的代码（如 upLoadStream.Write(bytes, 0, contentLength);）
                    upLoadStream.Write(bytes, 0, contentLength); // 将读取到的字节写入上传流
                    contentLength = file.Read(bytes, 0, bytes.Length); // 继续读取文件中的字节
                }
                // 出了循环说明文件已经读取完毕了 这个时候我们就可以关闭文件流和上传流了
                file.Close();
                upLoadStream.Close();
                // 上传完毕
                print("上传完毕");
            }
        }
        catch (Exception e)
        {
            print("上传失败，异常信息：" + e.Message);
        }
        #endregion

        #region 总结
        // C#已经把ftp相关操作封装的很好了
        // 我们只需要熟悉API，直接使用他们进行FTP上传即可
        // 我们主要做的操作是
        // 把本地文件流读出字节数据写入到要上传的FTP流中
        // Ftp上传相关API也有异步方法
        // 使用上和以前的TCP相关类似
        // 这里不赘述
        #endregion
    }

    // Update is called once per frame
    void Update()
    {

    }

}
