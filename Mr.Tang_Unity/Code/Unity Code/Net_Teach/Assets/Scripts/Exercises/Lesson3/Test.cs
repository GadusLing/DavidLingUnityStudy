using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;


public class TestInfo : BaseData
{
    public short lev;
    public Player p;
    public int hp;
    public string name;
    public bool sex;

    public override int GetBytesNum()
    {
        return sizeof(short) + p.GetBytesNum() + sizeof(int) + sizeof(int) + System.Text.Encoding.UTF8.GetByteCount(name) + sizeof(bool);
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteShort(bytes, lev, ref index);
        WriteData(bytes, p, ref index);
        WriteInt(bytes, hp, ref index);
        WriteString(bytes, name, ref index);
        WriteBool(bytes, sex, ref index);
        return bytes;
    }
}

public class Player : BaseData
{
    public int atk;
    public override int GetBytesNum()
    {
        return sizeof(int);
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteInt(bytes, atk, ref index);
        return bytes;
    }
}

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TestInfo info = new TestInfo();
        info.lev = 87;
        info.p = new Player();
        info.p.atk = 9999;
        info.hp = 5000;
        info.name = "David";
        info.sex = true;
        byte[] data = info.Writing();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
