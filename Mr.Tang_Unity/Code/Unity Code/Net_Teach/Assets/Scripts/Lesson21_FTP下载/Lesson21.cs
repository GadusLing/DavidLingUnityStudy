using System.Collections;
using System.Collections.Generic;
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
        // 1. 创建一个Ftp连接
        // 2. 设置信息凭证（如果不支持匿名 就必须设置这一步）
        // 3. 请求完毕后 是否去关闭连接，如果要进行多次操作 可以设置为false
        // 4. 指定传输类型
        // 5. 得到用于上传的流对象
        // 6. 开始上传
        #endregion
    }

    // Update is called once per frame
    void Update()
    {

    }


}
