using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Lesson_MD5 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 MD5码是什么
        //MD5（Message-Digest Algorithm）是
        //MD5信息摘要算法的简称
        //它是一种广泛使用的密码散列函数
        //可以生成出一个128位（16个字节）的散列值
        //用于确保信息的完整一致性

        //当我们将数据经过MD5算法计算过后
        //不管我们传入的数据有多大
        //都会生成一个固定长度（128位共16个字节）的信息摘要值

        //相同的数据，每次经过MD5算法计算后的结果都会是一样的
        //如果数据变化，MD5码将会发生变化

        //因此，我们可以利用MD5码作为文件的唯一标识
        //通过它来判断文件内容是否变化
        #endregion

        #region 知识点二 MD5码在热更新资源对比文件中的作用
        //通过资源名或者资源大小我们无法判断资源是否更新
        //所以我们需要利用MD5的唯一性，来判断资源的更新
        #endregion

        #region 知识点三 C#中获取文件的MD5码
        //关键类：
        //新知识
        //MD5 —— MD5类
        //MD5CryptoServiceProvider —— MD5加密服务提供商类

        //老知识
        //FileStream —— 文件流类 数据持久化四部曲 2进制中讲解过
        //StringBuilder —— 字符串拼接类 C#四部曲C#进阶中讲过

        //流程：
        //1.根据文件路径，获取文件的流信息
        //2.利用md5对象根据流信息 计算出MD5码（字节数组形式）
        //3.将字节数组形式的MD5码转为 16进制字符串

        print(GetMD5(Application.dataPath + "/ArtRes/AB/PC/lua"));
        #endregion
    }

    private string GetMD5(string filePath)
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
