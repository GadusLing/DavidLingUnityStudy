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
}

public class Lesson3 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        byte[] bytes = BitConverter.GetBytes(1);


        byte[] bytes1 = Encoding.UTF8.GetBytes("哈哈哈士大夫哈啥哈哈"); // 使用UTF8编码 

        PlayerInfo info = new PlayerInfo();
        info.lev = 10;
        info.name = "DavidLing";
        info.atk = 888;
        info.sex = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
