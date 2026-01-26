using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerInfo
{
    public int lev;
    public string name;
    public short atk;
    public bool sex;

    public byte[] GetBytes()
    {
        int indexNum = sizeof(int) +  // lev(int类型)
                       sizeof(int) +  // name字符串转成字节数组后的长度(int类型)，在反序列化时会通过这个长度来截取name字符串对应的字节数组
                       Encoding.UTF8.GetBytes(name).Length + // name字符串转成字节数组后的实际长度
                       sizeof(short) +  // atk(short类型)
                       sizeof(bool); // sex(bool类型)
        byte[] bytes = new byte[indexNum];
        int index = 0;
        BitConverter.GetBytes(lev).CopyTo(bytes, index);
        index += sizeof(int);
        BitConverter.GetBytes(Encoding.UTF8.GetBytes(name).Length).CopyTo(bytes, index);
        index += sizeof(int);
        Encoding.UTF8.GetBytes(name).CopyTo(bytes, index);
        index += Encoding.UTF8.GetBytes(name).Length;
        BitConverter.GetBytes(atk).CopyTo(bytes, index);
        index += sizeof(short);
        BitConverter.GetBytes(sex).CopyTo(bytes, index);
        index += sizeof(bool);
        return bytes;
    }

}

public class Lesson3 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        byte[] bytes = BitConverter.GetBytes(1);


        byte[] bytes1 = Encoding.UTF8.GetBytes("用于测试UTF8编码"); // 使用UTF8编码 

        PlayerInfo info = new PlayerInfo();
        info.lev = 10;
        info.name = "DavidLing";
        info.atk = 888;
        info.sex = false;

        int indexNum = sizeof(int) +  // lev(int类型)
                       sizeof(int) +  // name字符串转成字节数组后的长度(int类型)，在反序列化时会通过这个长度来截取name字符串对应的字节数组
                       Encoding.UTF8.GetBytes(info.name).Length + // name字符串转成字节数组后的实际长度
                       sizeof(short) + // atk(short类型)
                       sizeof(bool); // sex(bool类型)
        // 我们想要把一个类的数据都序列化到一个字节数组中 必须要知道每个具体信息对应的字节长度 这样才能在反序列化时解析出合法的数据

        byte[] playerBytes = new byte[indexNum]; // 根据计算出来的总长度 创建一个字节数组 用来存储序列化后的数据

        int index = 0; // 从playerBytes的第0个位置开始存储数据

        // 序列化
        BitConverter.GetBytes(info.lev).CopyTo(playerBytes, index);
        index += sizeof(int);
        BitConverter.GetBytes(Encoding.UTF8.GetBytes(info.name).Length).CopyTo(playerBytes, index);
        index += sizeof(int);
        Encoding.UTF8.GetBytes(info.name).CopyTo(playerBytes, index);
        index += Encoding.UTF8.GetBytes(info.name).Length;
        BitConverter.GetBytes(info.atk).CopyTo(playerBytes, index);
        index += sizeof(short);
        BitConverter.GetBytes(info.sex).CopyTo(playerBytes, index);
        index += sizeof(bool);
        // 到这里 序列化完成 playerBytes中存储的就是info对象的全部数据



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
