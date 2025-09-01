using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basicmethod : MonoBehaviour
{
    /*
     * 题目1：
     * 现在有玩家信息类，有名字，年龄，攻击力，防御力等成员
     * 现在为其封装两个方法，一个用来存储数据，一个用来读取数据
     */

    /*
     * 题目2：
     * 现在在装备信息类，装备类中有id，数量两个成员。
     * 上一题的玩家类中包含一个List存储了拥有的所有装备信息。
     * 请在上一题的基础上，把装备信息的存储和读取加上
     */


    class playerinfo
    {
        public string Name { get;set; }
        public int Age { get;set; }
        public int Attack { get;set; }
        public int Defense { get;set; }
        public List<equipinfo> equiplist = new List<equipinfo>();

        public void saveinfo()
        {
            PlayerPrefs.SetString("Name", Name);
            PlayerPrefs.SetInt("Age", Age);
            PlayerPrefs.SetInt("Attack", Attack);
            PlayerPrefs.SetInt("Defense", Defense);

            PlayerPrefs.SetInt("EquipCount", equiplist.Count);
            for (int i = 0; i < equiplist.Count; i++)
            {
                PlayerPrefs.SetInt("EquipID_" + i, equiplist[i].ID);
                PlayerPrefs.SetInt("EquipNum_" + i, equiplist[i].Num);
            }

            PlayerPrefs.Save();
        }

        public void readinfo()
        {
            Name = PlayerPrefs.GetString("Name", "未命名");
            Age = PlayerPrefs.GetInt("Age", 18);
            Attack = PlayerPrefs.GetInt("Attack", 10);
            Defense = PlayerPrefs.GetInt("Defense", 5);

            int equipcount = PlayerPrefs.GetInt("EquipCount", 0);
            for (int i = 0; i < equipcount; i++)
            {
                equipinfo equip = new equipinfo();
                equip.ID = PlayerPrefs.GetInt("EquipID_" + i, 0);
                equip.Num = PlayerPrefs.GetInt("EquipNum_" + i, 0);
                equiplist.Add(equip);
            }
        }
    }

    class equipinfo
    {
        public int ID { get;set; }
        public int Num { get;set; }
    }


    // Start is called before the first frame update
    void Start()
    {
        playerinfo player = new playerinfo();
        player.readinfo();
        print(player.Name);
        print(player.Age);
        print(player.Attack);
        print(player.Defense);

        player.Name = "张三";
        player.Age = 20;
        player.Attack = 15;
        player.Defense = 8;
        player.saveinfo();

        equipinfo equip = new equipinfo();
        equip.ID = 1001;
        equip.Num = 1;
        player.equiplist.Add(equip);
        equip = new equipinfo();
        equip.ID = 1002;
        equip.Num = 2;
        player.equiplist.Add(equip);
        player.saveinfo();
        print(player.equiplist.Count);
        foreach (var eq in player.equiplist)
        {
            print("装备ID：" + eq.ID + " 数量：" + eq.Num);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
