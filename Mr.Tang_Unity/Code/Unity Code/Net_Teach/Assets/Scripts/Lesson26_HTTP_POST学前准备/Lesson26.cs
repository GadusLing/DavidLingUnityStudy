using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

public class Lesson26 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 Get和Post的区别
        // 我们上节课学习的下载数据，主要使用的就是Get请求类型
        // 我们在上传数据时将会使用Post请求类型
        // 那么这两个请求类型他们的主要区别是什么呢？

        // 1.主要用途
        // Get —— 一般从指定的资源请求数据，主要用于获取数据
        // Post —— 一般向指定的资源提交想要被处理的数据，主要用于上传数据

        // 2.相同点
        // Get和Post都可以传递一些额外的参数数据给服务器端

        // 3.不同点
        // 3-1: 在传递参数时，Post相对Get更加的安全，因为Post看不到参数
        //     Get传递的参数都包含在连接中（URL资源定位地址），是暴露式的  ?参数名=参数值&参数名=参数值
        //     Post传递的参数放在请求数据中，不会出现在URL中，是隐藏式的

        // 3-2: Get在传递数据时有大小的限制，因为主要是在连接中拼接参数，而URL的长度是有限制的（最大长度一般为2048个字符）
        //     Post在传递数据时没有限制

        // 3-3: 在浏览器中Get请求能被缓存，Post不能缓存

        // 3-4: 传输次数可能不同
        //     Get：建立连接→请求行、请求头、请求数据→一次传输→获取响应→断开连接
        //     Post：建立连接→请求行、请求头、请求数据第一次传输→请求数据第二次传输→获取响应→断开

        // 对于前端来说，其实Get和Post都能获取和提交数据，后端要处理好逻辑和返回响应信息即可
        // 但是出于前面的这些区别，推荐用Get用于获取，Post用于上传
        // 特别是一些不想暴露在URL中的参数信息，建议使用Post，它更加的安全
        #endregion

        #region 知识点二 Post如何携带额外参数
        // 关键点：将Content-Type设置为 application/x-www-form-urlencoded 键值对类型
        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://192.168.1.2:8081/Http_Server/");
        req.Method = WebRequestMethods.Http.Post;
        req.Timeout = 2000;
        req.ContentType = "application/x-www-form-urlencoded"; // 设置请求数据的格式为键值对类型
        string postData = "username=admin&password=123456"; // 模拟要上传的参数数据
        byte[] postBytes = Encoding.UTF8.GetBytes(postData); // 将字符串转换为字节数组 实际项目中具体的编码格式可能会根据后端的要求进行设置，常见的有UTF-8、GBK等
        req.ContentLength = postBytes.Length; // 设置请求数据的长度
        try
        {
            // 获取请求数据流，写入要上传的数据
            using (var stream = req.GetRequestStream())
            {
                stream.Write(postBytes, 0, postBytes.Length);
            }
            // 获取响应结果
            using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
            {
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    Debug.Log("数据上传成功");
                }
                else
                {
                    Debug.Log("数据上传失败，状态码: " + res.StatusCode);
                }
            }
        }
        catch (WebException we)
        {
            Debug.Log("请求发生错误: " + we.Message + " 状态: " + we.Status);
        }
        #endregion

        #region 知识点三 ContentType的常用类型
        // ContentType的构成：
        // 内容类型;charset=编码格式;boundary=边界字符串
        // 例如：
        // text/html;charset=utf-8;boundary=自定义字符串
        req.ContentType = "application/x-www-form-urlencoded;charset=UTF-8;boundary=Ling";
        req.ContentType = "变化的;charset=变化的;boundary=变化的，可选，特定情况下使用";


        // 其中内容类型有：
        // 文本类型text：
        //   text/plain  没有特定子类型就是它（重要）
        //   text/html
        //   text/css
        //   text/javascript

        // 图片类型image：
        //   image/gif
        //   image/png
        //   image/jpeg
        //   image/bmp
        //   image/webp
        //   image/x-icon
        //   image/vnd.microsoft.icon

        // 音频类型audio：
        //   audio/midi
        //   audio/mpeg
        //   audio/webm
        //   audio/ogg
        //   audio/wav
        // 视频类型video:
        //   video/webm
        //   video/ogg

        // 二进制类型application:
        //   application/octet-stream  没有特定子类型就是它（重要）
        //   application/x-www-form-urlencoded  传递参数时使用键值对形式（重要）
        //   application/pkcs12
        //   application/xhtml+xml
        //   application/xml
        //   application/pdf
        //   application/vnd.ms-powerpoint

        // 复合内容multipart:
        //   multipart/form-data   复合内容，有多种内容组合（重要）
        //   multipart/byteranges  特殊的复合文件

        // 关于ContentType更多内容可以前往
        // https://developer.mozilla.org/zh-CN/docs/Web/HTTP/Headers/Content-Type
        // 关于媒体类型可以前往
        // https://developer.mozilla.org/zh-CN/docs/Web/HTTP/Basics_of_HTTP/MIME_types
        #endregion

        #region 知识点四 ContentType中对于我们来说重要的类型
        
        // 1.通用二进制类型
        //   application/octet-stream
        // 2.通用文本类型
        //   text/plain
        // 3.键值对参数
        //   application/x-www-form-urlencoded
        // 4.复合类型（传递的信息有多种类型组合，比如有键值对参数，有文件信息等等，上传资源服务器时需要用该类型）
        //   multipart/form-data
        #endregion

        #region 总结
        
        // 这节课的重点知识点是
        // 1. Get和Post的区别
        // 2. ContentType的重要类型

        // 注意：
        // HTTP通讯中
        // 客户端发送给服务端的Get和Post请求都需要服务端和客户端约定一些规则进行处理
        // 比如传递的参数的含义、数据如何处理等等，都是需要前后端程序制定对应规则来进行处理的
        // 只是目前没有后端开发的HTTP服务器，所以我们传递过去的参数和数据没有得到对应处理
        // 我们目前只针对HTTP资源服务器上传下载数据进行学习
        // 他们的通讯原理是一致的，都是通过HTTP通讯交换数据
        #endregion
    }

    // Update is called once per frame
    void Update()
    {

    }
}
