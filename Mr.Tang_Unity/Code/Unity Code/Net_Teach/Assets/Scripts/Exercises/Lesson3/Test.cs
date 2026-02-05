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
}

public class Player : BaseData
{
    public int atk;
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
