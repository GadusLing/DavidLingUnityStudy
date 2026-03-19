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
            }
            catch (Exception e)
            {
                Debug.LogError("上传失败，异常信息：" + e.Message);
            }
        });
        action?.Invoke(); // 上传完成后执行回调函数（如果有的话）
    }

    /// <summary>
    /// 从FTP服务器下载文件
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="localPath">想要存储到本地的文件路径</param>
    /// <param name="action">下载完成后的回调</param>
    public async void DownloadFile(string fileName, string localPath, UnityAction action = null)
    {
        await Task.Run(() =>
        {
            try
            {
                // 1. 创建一个Ftp连接
                FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(new Uri(FTP_PATH + fileName));
                // 2. 设置信息凭证（如果不支持匿名 就必须设置这一步）
                req.Credentials = new NetworkCredential(USER_NAME, PASSWORD);
                req.KeepAlive = false;
                // 3. 设置操作命令
                req.Method = WebRequestMethods.Ftp.DownloadFile;
                // 4. 指定传输类型
                req.UseBinary = true;
                req.Proxy = null; // 将代理相关信息置空 避免服务器同时有http相关的代理信息和ftp相关的代理信息时 可能会导致连接失败
                // 5. 得到用于下载的流对象
                FtpWebResponse res = req.GetResponse() as FtpWebResponse;
                Stream downLoadStream = res.GetResponseStream();
                // 6. 开始下载
                using (FileStream file = File.Create(localPath))
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
                res.Close(); // 关闭FTP响应对象
            }
            catch (Exception e)
            {
                Debug.LogError("下载失败，异常信息：" + e.Message);
            }
        });
        action?.Invoke(); // 下载完成后执行回调函数（如果有的话）
    }

    /// <summary>
    /// 从FTP服务器删除文件
    /// </summary>
    /// <param name="fileName">要删除的文件名</param>
    /// <param name="action">删除完成后的回调，返回删除是否成功</param>
    public async void DeleteFile(string fileName, UnityAction<bool> action = null)
    {
        await Task.Run(() =>
        {
            try
            {
                // 1. 创建一个Ftp连接
                FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(new Uri(FTP_PATH + fileName));
                // 2. 设置信息凭证（如果不支持匿名 就必须设置这一步）
                req.Credentials = new NetworkCredential(USER_NAME, PASSWORD);
                req.KeepAlive = false;
                // 3. 设置操作命令
                req.Method = WebRequestMethods.Ftp.DeleteFile;
                // 4. 指定传输类型
                req.UseBinary = true;
                req.Proxy = null; // 将代理相关信息置空 避免服务器同时有http相关的代理信息和ftp相关的代理信息时 可能会导致连接失败
                // 5. 执行删除操作
                FtpWebResponse res = req.GetResponse() as FtpWebResponse;
                res.Close();

                action?.Invoke(true); // 删除成功回调
            }
            catch (Exception e)
            {
                Debug.LogError("删除失败，异常信息：" + e.Message);
                action?.Invoke(false); // 删除失败回调
            }
        });
    }


    /// <summary>
    /// 获取FTP服务器上文件的大小
    /// </summary>
    /// <param name="fileName">要获取大小的文件名</param>
    /// <param name="action">获取完成后的回调，返回文件大小</param>
    public async void GetFileSize(string fileName, UnityAction<long> action = null)
    {
        await Task.Run(() =>
        {
            try
            {
                // 1. 创建一个Ftp连接
                FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(new Uri(FTP_PATH + fileName));
                // 2. 设置信息凭证（如果不支持匿名 就必须设置这一步）
                req.Credentials = new NetworkCredential(USER_NAME, PASSWORD);
                req.KeepAlive = false;
                // 3. 设置操作命令
                req.Method = WebRequestMethods.Ftp.GetFileSize;
                // 4. 指定传输类型
                req.UseBinary = true;
                req.Proxy = null; // 将代理相关信息置空 避免服务器同时有http相关的代理信息和ftp相关的代理信息时 可能会导致连接失败
                // 5. 执行获取文件大小操作
                FtpWebResponse res = req.GetResponse() as FtpWebResponse;
                action?.Invoke(res.ContentLength); // 获取文件大小成功回调，返回文件大小
                res.Close();

            }
            catch (Exception e)
            {
                Debug.LogError("获取文件大小失败，异常信息：" + e.Message);
                action?.Invoke(0); // 获取文件大小失败回调，返回0表示失败
            }
        });



    }

    /// <summary>
    /// 在FTP服务器上创建文件夹
    /// </summary>
    /// <param name="directoryName">要创建的文件夹名称</param>
    /// <param name="action">创建完成后的回调，返回创建是否成功</param>
    public async void CreateDirectory(string directoryName, UnityAction<bool> action = null)
    {
        await Task.Run(() =>
        {
            try
            {
                // 1. 创建一个Ftp连接
                FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(new Uri(FTP_PATH + directoryName));
                // 2. 设置信息凭证（如果不支持匿名 就必须设置这一步）
                req.Credentials = new NetworkCredential(USER_NAME, PASSWORD);
                req.KeepAlive = false;
                // 3. 设置操作命令
                req.Method = WebRequestMethods.Ftp.MakeDirectory;
                // 4. 指定传输类型
                req.UseBinary = true;
                req.Proxy = null; // 将代理相关信息置空 避免服务器同时有http相关的代理信息和ftp相关的代理信息时 可能会导致连接失败
                // 5. 执行创建文件夹操作
                FtpWebResponse res = req.GetResponse() as FtpWebResponse;
                res.Close();

                action?.Invoke(true); // 创建文件夹成功回调
            }
            catch (Exception e)
            {
                Debug.LogError("创建文件夹失败，异常信息：" + e.Message);
                action?.Invoke(false); // 创建文件夹失败回调
            }
        });
    }

    /// <summary>
    /// 获取FTP服务器上指定目录下的文件列表
    /// </summary>
    /// <param name="directoryPath">要获取文件列表的目录路径</param>
    /// <param name="action">获取完成后的回调，返回文件列表</param>
    public async void GetFileList(string directoryPath, UnityAction<List<string>> action = null)
    {
        await Task.Run(() =>
        {
            try
            {
                // 1. 创建一个Ftp连接
                FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(new Uri(FTP_PATH + directoryPath));
                // 2. 设置信息凭证（如果不支持匿名 就必须设置这一步）
                req.Credentials = new NetworkCredential(USER_NAME, PASSWORD);
                req.KeepAlive = false;
                // 3. 设置操作命令
                req.Method = WebRequestMethods.Ftp.ListDirectory;
                // 4. 指定传输类型
                req.UseBinary = true;
                req.Proxy = null; // 将代理相关信息置空 避免服务器同时有http相关的代理信息和ftp相关的代理信息时 可能会导致连接失败
                // 5. 执行获取文件列表操作
                FtpWebResponse res = req.GetResponse() as FtpWebResponse;
                StreamReader reader = new StreamReader(res.GetResponseStream()); // 把下载的信息流转换成StreamReader方便我们按行读取
                List<string> fileList = new List<string>(); // 用于存储文件名的列表
                string line;
                while ((line = reader.ReadLine()) != null) // 按行读取FTP服务器返回的信息 直到读完为止
                {
                    fileList.Add(line); // 将读取到的文件名添加到文件列表中
                }
                reader.Close();
                res.Close();

                action?.Invoke(fileList); // 获取文件列表成功回调，返回文件列表
            }
            catch (Exception e)
            {
                Debug.LogError("获取文件列表失败，异常信息：" + e.Message);
                action?.Invoke(null); // 获取文件列表失败回调，返回null表示失败
            }
        });
    }


}

