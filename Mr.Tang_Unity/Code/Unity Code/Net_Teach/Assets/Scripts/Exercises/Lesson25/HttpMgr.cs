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
        HttpStatusCode result = HttpStatusCode.ServiceUnavailable;
        await Task.Run(() =>
        {
            try
            {
                if (!File.Exists(localFile)) // 本地文件不存在，直接返回404状态码
                {
                    result = HttpStatusCode.NotFound; // 本地文件不存在 404
                    return;
                }
                // 1. 创建请求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HTTP_PATH + fileName);
                request.Method = WebRequestMethods.Http.Put;
                request.Timeout = 5000;
                request.AllowWriteStreamBuffering = false;
                // 2. 读取本地文件
                byte[] fileBytes = File.ReadAllBytes(localFile);
                request.ContentLength = fileBytes.Length;
                request.ContentType = "application/octet-stream";
                // 3. 写入请求流
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(fileBytes, 0, fileBytes.Length);
                }
                // 4. 获取响应
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    result = response.StatusCode;
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
            catch (System.Exception ex)
            {
                result = HttpStatusCode.InternalServerError;
                Debug.Log("上传未知异常: " + ex.Message);
            }
        });
        action?.Invoke(result);
    }
}
