using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Lesson4 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        byte[] bytes = BitConverter.GetBytes(99);
        int i = BitConverter.ToInt32(bytes, 0);
        print(i);

        byte[] bytes2 = Encoding.UTF8.GetBytes("Hello World");
        string str = Encoding.UTF8.GetString(bytes2, 0, bytes2.Length); // 第二个参数是起始位置，第三个参数是长度
        print(str);

        PlayerInfo info = new PlayerInfo(); 
        info.lev = 10;
        info.name = "DavidLing";
        info.atk = 888;
        info.sex = false;
        byte[] playerBytes = info.GetBytes();

        PlayerInfo info2 = new PlayerInfo();
        int index = 0;
        info2.lev = BitConverter.ToInt32(playerBytes, index);
        index += sizeof(int);
        int nameLen = BitConverter.ToInt32(playerBytes, index);
        index += sizeof(int);
        info2.name = Encoding.UTF8.GetString(playerBytes, index, nameLen);
        index += nameLen;
        info2.atk = BitConverter.ToInt16(playerBytes, index);
        index += sizeof(short);
        info2.sex = BitConverter.ToBoolean(playerBytes, index);
        index += sizeof(bool);
        print($"lev: {info2.lev}, name: {info2.name}, atk: {info2.atk}, sex: {info2.sex}");


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
