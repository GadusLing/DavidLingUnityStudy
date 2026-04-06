using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

public class CreateABCompare
{
    [MenuItem("AB包工具/创建对比文件")]
    public static void CreateABCompareFile()
    {
        DirectoryInfo directory = Directory.CreateDirectory(Application.dataPath + "/ArtRes/AB/PC/"); // 获取目录信息
        FileInfo[] fileInfos = directory.GetFiles("*", SearchOption.AllDirectories); // 获取目录下的所有文件

        string abCompareInfo = ""; // 用于存储对比信息的字符串

        foreach (FileInfo info in fileInfos)
        {
            if (info.Extension == "" ) // 如果没后缀就是AB包
            {
                abCompareInfo += info.Name + " " + info.Length + " " + GetMD5(info.FullName); // 将文件名、文件大小和MD5码拼接成字符串 用空格分隔
                abCompareInfo += "|"; // 用|分隔每个文件的信息 
            }
        }
        abCompareInfo = abCompareInfo.TrimEnd('|'); // 去掉最后一个|分隔符
        File.WriteAllText(Application.dataPath + "/ArtRes/AB/PC/ABCompareInfo.txt", abCompareInfo); // 将对比信息写入文本文件
        AssetDatabase.Refresh(); // 刷新编辑器资源数据库
        Debug.Log("AB包对比文件创建成功！");
    }

    /// <summary>
    /// 获取文件的MD5码
    /// </summary>
    /// <param name="filePath">文件的完整路径</param>
    /// <returns>文件的MD5码</returns>
    public static string GetMD5(string filePath)
    {
        //将文件以流的形式打开
        using (FileStream file = new FileStream(filePath, FileMode.Open))
        {
            //声明一个MD5对象 用于生成MD5码
            MD5 md5 = new MD5CryptoServiceProvider();
            //利用API 得到数据的MD5码 16个字节 数组
            byte[] md5Info = md5.ComputeHash(file);

            //关闭文件流
            file.Close();

            //把16个字节转换为 16进制 拼接成字符串 为了减小md5码的长度
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < md5Info.Length; i++)
                sb.Append(md5Info[i].ToString("x2"));

            return sb.ToString();
        }
    }
}
