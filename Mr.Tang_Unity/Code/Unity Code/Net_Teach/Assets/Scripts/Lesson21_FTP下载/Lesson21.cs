using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class Lesson21 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一——使用FTP下载文件关键点
        // 1. 通信凭证
        //    进行ftp连接操作时需要的账号密码
        // 2. 操作命令 WebRequestMethods.Ftp
        //    设置你想要进行的Ftp操作
        // 3. 文件流相关 FileStream 和 Stream
        //    上传和下载时都会使用的文件流
        //    下载文件流使用FtpWebResponse类获取
        // 4. 保证FTP服务器已经开启
        //    并且能够正常访问
        #endregion

        #region 知识点二——FTP下载
        try
        {
            // 1. 创建一个Ftp连接
            // 这里和上传不同 上传的文件名是我们自己指定的 但是下载的文件名必须是服务器上已经存在的文件
            FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://192.168.1.2/test.txt"));
            // 2. 设置信息凭证（如果不支持匿名 就必须设置这一步）
            req.Credentials = new NetworkCredential("David", "123456");
            // 请求完毕后 是否去关闭连接，如果要进行多次操作 可以设置为false
            req.KeepAlive = false;
            // 3. 设置操作命令
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            // 4. 指定传输类型
            req.UseBinary = true;
            req.Proxy = null; // 将代理相关信息置空 避免服务器同时有http相关的代理信息和ftp相关的代理信息时 可能会导致连接失败
            // 5. 得到用于下载的流对象
            // 相当于把请求发送给FTP服务器 返回值 就会携带我们想要的信息
            FtpWebResponse res = req.GetResponse() as FtpWebResponse;
            // 这就是下载的流
            Stream downLoadStream = res.GetResponseStream();
            // 6. 开始下载
            print(Application.persistentDataPath);
            using (FileStream file = File.Create(Application.persistentDataPath + "/test.txt"))
            {
                byte[] bytes = new byte[1024];
                int contentLength = downLoadStream.Read(bytes, 0, bytes.Length);
                while (contentLength > 0)
                {
                    file.Write(bytes, 0, contentLength);
                    contentLength = downLoadStream.Read(bytes, 0, bytes.Length);
                }
                file.Close();
                downLoadStream.Close();
            }
            print("下载完成了");
        }
        catch (Exception e)
        {
            print("下载失败，异常信息：" + e.Message);
        }
        #endregion

        #region 总结
        // C#已经把ftp相关操作封装的很好了
        // 我们只需要熟悉API，直接使用他们进行FTP下载即可
        // 我们主要做的操作是
        // 把下载文件的FTP流读出字节数据写入到本地文件流中
        #endregion
    }

    // Update is called once per frame
    void Update()
    {

    }


}
