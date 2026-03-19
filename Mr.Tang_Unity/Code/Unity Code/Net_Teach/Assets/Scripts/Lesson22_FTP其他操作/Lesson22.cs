using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson22 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一——其它操作指什么？
        // 除了上传和下载，我们可能会对FTP服务器上的内容进行其它操作
        // 比如：
        // 1. 删除文件
        // 2. 获取文件大小
        // 3. 创建文件夹
        // 4. 获取文件列表
        // 等等
        #endregion

        #region 知识点二——进行其它操作
        // 1. 删除文件
        FtpMgr.Instance.DeleteFile("测试删除.txt", (result) =>
        {
            print(result ? "删除成功了" : "删除失败了");
        });
        // 2. 获取文件大小
        FtpMgr.Instance.GetFileSize("test.txt", (size) =>
        {
            print("test.txt的大小是：" + size + "字节");
        });
        // 3. 创建文件夹
        FtpMgr.Instance.CreateDirectory("TestDir", (result) =>
        {
            print(result ? "创建文件夹成功了" : "创建文件夹失败了");
        });
        // 4. 获取文件列表
        FtpMgr.Instance.GetFileList("/", (list) =>
        {
            print("根目录下的文件列表：");
            foreach (var item in list)
            {
                print(item);
            }
        });
        #endregion

        #region 总结
        // FTP对于我们的作用
        // 1. 游戏当中的一些上传和下载功能
        // 2. 原生AB包上传下载
        // 3. 上传下载一些语音内容
        // 只要是上传下载相关的功能 都可以使用Ftp来完成
        #endregion
    }

    // Update is called once per frame
    void Update()
    {

    }
}
