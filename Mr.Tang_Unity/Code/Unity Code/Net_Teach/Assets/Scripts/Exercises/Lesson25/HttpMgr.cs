using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class HttpMgr
{
    private static HttpMgr _instance;
    public static HttpMgr Instance => _instance ?? (_instance = new HttpMgr());
    private HttpMgr() { }
    private string HTTP_PATH = "http://192.168.1.2:8081/Http_Server/";

    private string USER_NAME = "David";
    private string PASSWORD = "123456";

    /// <summary>
    /// 下载文件到本地指定路径
    /// </summary>
    /// <param name="fileName">要下载的文件名</param>
    /// <param name="localPath">本地保存路径</param>
    /// <param name="action">下载完成后的回调</param>
    public async void DownLoadFile(string fileName, string localPath, UnityAction<HttpStatusCode> action)
    {
        HttpStatusCode result = HttpStatusCode.ServiceUnavailable; // 默认状态码为服务不可用
        await Task.Run(() =>
        {
            try
            {
                // 1. HEAD请求判断文件是否存在
                HttpWebRequest headReq = (HttpWebRequest)WebRequest.Create(HTTP_PATH + fileName);
                headReq.Method = WebRequestMethods.Http.Head;
                headReq.Timeout = 2000;
                using (HttpWebResponse headRes = (HttpWebResponse)headReq.GetResponse())
                {
                    if (headRes.StatusCode == HttpStatusCode.OK)
                    {
                        // 2. GET请求下载文件
                        HttpWebRequest getReq = (HttpWebRequest)WebRequest.Create(HTTP_PATH + fileName);
                        getReq.Method = WebRequestMethods.Http.Get;
                        getReq.Timeout = 2000;
                        using (HttpWebResponse getRes = (HttpWebResponse)getReq.GetResponse())
                        {
                            if (getRes.StatusCode == HttpStatusCode.OK)
                            {
                                using (var fileStream = File.Create(localPath))
                                using (Stream stream = getRes.GetResponseStream())
                                {
                                    byte[] bytes = new byte[4096];
                                    int contentLength;
                                    while ((contentLength = stream.Read(bytes, 0, bytes.Length)) > 0)
                                    {
                                        fileStream.Write(bytes, 0, contentLength);
                                    }
                                }
                                result = HttpStatusCode.OK;
                            }
                            else
                            {
                                result = getRes.StatusCode;
                            }
                        }
                    }
                    else
                    {
                        result = headRes.StatusCode;
                    }
                }
            }
            catch (WebException we)
            {
                // 如果有响应，尝试获取状态码
                if (we.Response is HttpWebResponse errorRes)
                {
                    result = errorRes.StatusCode;
                    errorRes.Close();
                }
                else
                {
                    result = HttpStatusCode.InternalServerError;
                }
                Debug.Log("请求发生错误: " + we.Message + " 状态: " + we.Status);
            }
            catch (System.Exception ex)
            {
                result = HttpStatusCode.InternalServerError;
                Debug.Log("未知异常: " + ex.Message);
            }
        });

        action?.Invoke(result);
    }

    /// <summary>
    /// 上传本地文件到服务器
    /// </summary>
    /// <param name="fileName">传到服务器上的文件名</param>
    /// <param name="localFile">本地文件路径</param>
    /// <param name="action">上传完成后的回调</param>
    public async void UpLoadFile(string fileName, string localFile, UnityAction<HttpStatusCode> action)
    {
        HttpStatusCode result = HttpStatusCode.BadRequest; // 默认状态码为错误请求
        await Task.Run(() =>
        {
            try
            {
                if (!File.Exists(localFile)) // 本地文件不存在，直接返回404状态码
                {
                    result = HttpStatusCode.NotFound; // 本地文件不存在 404
                    return;
                }

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(HTTP_PATH);
                req.Method = WebRequestMethods.Http.Post;
                req.ContentType = "multipart/form-data;boundary=DavidLing";
                req.Timeout = 500000;
                req.Credentials = new NetworkCredential(USER_NAME, PASSWORD);
                req.PreAuthenticate = true;
                // 拼接头部信息
                string head = "--DavidLing\r\n" +
                    $"Content-Disposition:form-data;name=\"file\";filename=\"{fileName}\"\r\n" +
                    "Content-Type:application/octet-stream\r\n\r\n";
                // head = string.Format(head, fileName); // Format: 指字符串格式化。string.Format方法用于将字符串中的占位符（如{0}、{1}等）替换为指定的参数值。
                // 当使用{0}、{1}等占位符时，string.Format方法会将这些占位符替换为后续提供的参数值。例如，string.Format("Hello {0}", "World")会返回"Hello World"。
                // 如果使用$ + {}的字符串插值方式，则可以直接在字符串中嵌入变量或表达式，无需使用占位符。例如，string name = "World"; string message = $"Hello {name}";会返回"Hello World"。

                // 将头部信息和结束边界信息转换为字节数组
                byte[] headBytes = System.Text.Encoding.UTF8.GetBytes(head);
                byte[] endBytes = System.Text.Encoding.UTF8.GetBytes("\r\n--DavidLing--\r\n");

                // 用using语句确保文件流和上传流正确关闭，避免资源泄漏
                using (FileStream localFileStream = File.OpenRead(localFile)) // 打开本地文件流
                {
                    req.ContentLength = headBytes.Length + localFileStream.Length + endBytes.Length; // 设置上传内容长度头+内容+尾部边界长度
                    using (Stream upLoadStream = req.GetRequestStream()) // 获取上传流
                    {
                        upLoadStream.Write(headBytes, 0, headBytes.Length); // 写入头部信息
                        byte[] bytes = new byte[2048]; // 定义缓冲区 分批读取文件数据并写入上传流
                        int contentLength;
                        while ((contentLength = localFileStream.Read(bytes, 0, bytes.Length)) > 0)
                        {
                            upLoadStream.Write(bytes, 0, contentLength); // 写入文件内容
                        }
                        upLoadStream.Write(endBytes, 0, endBytes.Length); // 写入结束边界
                    }
                }
                // 获取响应
                using (HttpWebResponse res = req.GetResponse() as HttpWebResponse)
                {
                    result = res.StatusCode;
                }
            }
            catch (WebException we)
            {
                if (we.Response is HttpWebResponse errorRes)
                {
                    result = errorRes.StatusCode;
                    errorRes.Close();
                }
                else
                {
                    result = HttpStatusCode.InternalServerError;
                }
                Debug.Log("上传请求错误: " + we.Message + " 状态: " + we.Status);
            }
            catch (Exception ex)
            {
                result = HttpStatusCode.InternalServerError;
                Debug.Log("上传未知异常: " + ex.Message);
            }
        });
        action?.Invoke(result); // 无论成功还是失败，都通过回调将最终状态码传回去，让外部处理
    }
}
