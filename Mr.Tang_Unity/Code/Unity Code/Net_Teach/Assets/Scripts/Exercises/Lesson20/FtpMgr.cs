using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class FtpMgr
{
    private static FtpMgr _instance = new FtpMgr();
    public static FtpMgr Instance => _instance;

    private string FTP_PATH = "ftp://192.168.1.2/";
    private string USER_NAME = "David";
    private string PASSWORD = "123456";

    /// <summary>
    /// 上传文件到FTP服务器
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="localPath">本地文件路径</param>
    /// <param name="action">上传完成后的回调</param>
    public async void UploadFile(string fileName, string localPath, UnityAction action = null)
    {
        await Task.Run(() =>
        {
            try
            {
                // 通过一个线程来执行FTP上传的操作 这样就不会阻塞主线程了
                // 1. 创建一个Ftp连接
                FtpWebRequest req = FtpWebRequest.Create(new Uri(FTP_PATH + fileName)) as FtpWebRequest;
                // 2. 设置信息凭证（如果不支持匿名登录 就必须设置这）
                req.Proxy = null; // 将代理相关信息置空 避免服务器同时有http相关的代理信息和ftp相关的代理信息时 可能会导致连接失败
                req.Credentials = new NetworkCredential(USER_NAME, PASSWORD);
                req.KeepAlive = false; // 传输完成后关闭连接
                req.UseBinary = true; // 二进制传输
                req.Method = WebRequestMethods.Ftp.UploadFile; // 设置操作命令为上传文件
                                                               // 3. 上传
                Stream upLoadStream = req.GetRequestStream(); // 得到用于上传的流对象
                using (FileStream fileStream = File.OpenRead(localPath)) // 打开本地文件
                {
                    byte[] buffer = new byte[1024];
                    int contentLength = fileStream.Read(buffer, 0, buffer.Length); // 从文件中读取字节到缓冲区
                    while (contentLength > 0) // 不停的读取文件中的字节直到读完为止
                    {
                        upLoadStream.Write(buffer, 0, contentLength); // 将读取到的字节写入上传流
                        contentLength = fileStream.Read(buffer, 0, buffer.Length); // 继续读取文件中的字节
                    }
                    fileStream.Close(); // 关闭文件流
                    upLoadStream.Close();
                }
                Debug.Log("上传文件成功");
            }
            catch (Exception e)
            {
                Debug.LogError("上传失败，异常信息：" + e.Message);
            }
        });
        action?.Invoke(); // 上传完成后执行回调函数（如果有的话）
    }
}

