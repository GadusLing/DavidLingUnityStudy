using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class UpLoadAB
{
    // 推荐使用 PascalCase 或私有只读字段，以下为演示常量（仅用于本地测试，生产环境请勿硬编码密码）
    private const string Host = "124.222.36.67";
    private const int Port = 21;
    private const string UserName = "ldw";
    private const string Password = "12345678";

    [UnityEditor.MenuItem("AB包工具/上传AB包和对比文件到FTP服务器")]
    private static void UpLoadAllABFile()
    {
        DirectoryInfo directory = Directory.CreateDirectory(Application.dataPath + "/ArtRes/AB/PC/"); // 获取目录信息
        FileInfo[] fileInfos = directory.GetFiles("*", SearchOption.AllDirectories); // 获取目录下的所有文件

        foreach (FileInfo info in fileInfos)
        {
            if (info.Extension == "" || info.Extension == ".txt") // 如果没后缀就是AB包 或者.txt是对比文件(此文件夹中正常应该独一份)
            {
                UpLoadFileToFTPServer(info.FullName, info.Name); // 上传文件到FTP服务器
            }
        }

    }

    /// <summary>
    /// 上传文件到FTP服务器
    /// 1.创建一个FtpWebRequest对象，指定FTP服务器的URI（统一资源标识符），格式为ftp://host:port/path/filename
    /// 2.设置请求方法为UploadFile，表示上传文件
    /// 3.设置FTP服务器的用户名和密码进行身份验证
    /// 4.设置被动模式和二进制模式以确保正确传输文件
    /// 5.使用文件流读取本地文件，并将其写入FTP请求的流中，分块上传以避免内存占用过大
    /// 6.获取FTP服务器的响应，检查上传是否成功，并处理可能的异常情况
    /// </summary>
    /// <param name="filePath">要上传的文件的完整路径</param>
    /// <param name="fileName">要上传的文件名（可选，如果为null则使用filePath的文件名）</param>

    private async static void UpLoadFileToFTPServer(string filePath, string fileName = null)
    {
        fileName ??= Path.GetFileName(filePath); // 获取文件名（不包含路径）                                           
        string uri = "ftp://" + Host + "/AB/PC/" + Uri.EscapeDataString(fileName);
        await Task.Run(() =>  // 使用异步任务来执行上传操作，避免阻塞主线程
        {
            try
            {
                FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // 设置凭据
                NetworkCredential n = new NetworkCredential(UserName, Password);
                req.Credentials = n; // 设置FTP服务器的用户名和密码进行身份验证
                req.Proxy = null; // 不使用代理
                req.KeepAlive = false; // 上传完成后关闭连接
                req.Method = WebRequestMethods.Ftp.UploadFile; // 设置请求方法为UploadFile，表示上传文件
                req.UseBinary = true; // 设置二进制模式以确保正确传输文件

                // 获取 ftp 的流对象，写入文件内容
                using (Stream upLoadStream = req.GetRequestStream())
                using (FileStream file = File.OpenRead(filePath))
                {
                    byte[] bytes = new byte[2048]; // 定义一个缓冲区，分块上传以避免内存占用过大
                    int contentLength = file.Read(bytes, 0, bytes.Length); // 从文件流中读取数据到缓冲区，并获取实际读取的字节数

                    // 循环上传
                    while (contentLength != 0)
                    {
                        upLoadStream.Write(bytes, 0, contentLength); // 将缓冲区中的数据写入FTP请求的流中
                        contentLength = file.Read(bytes, 0, bytes.Length); // 继续从文件流中读取数据到缓冲区，并获取实际读取的字节数
                    }

                    // 文件与流会在 using 块结束时自动关闭
                }

                // 可以检查服务器返回状态
                using (FtpWebResponse response = (FtpWebResponse)req.GetResponse())
                {
                    Debug.Log($"上传完成: {fileName}, 状态: {response.StatusDescription}");
                }
            }
            catch (WebException ex) // 捕获与FTP相关的异常，如连接失败、认证失败、权限不足等
            {
                var resp = ex.Response as FtpWebResponse;
                if (resp != null)
                    Debug.LogError($"上传失败: {fileName}, FTP 状态: {resp.StatusDescription}");
                else
                    Debug.LogError($"上传失败: {fileName}, 异常: {ex.Message}");
            }
            catch (Exception ex) // 捕获其他可能的异常，如文件访问权限问题、网络问题等
            {
                Debug.LogError($"上传异常: {fileName}, 异常: {ex.Message}");
            }
        });


    }
}
