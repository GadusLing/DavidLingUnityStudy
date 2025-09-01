using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basicmethod : MonoBehaviour
{
    /*
     * ��Ŀ1��
     * �����������Ϣ�࣬�����֣����䣬���������������ȳ�Ա
     * ����Ϊ���װ����������һ�������洢���ݣ�һ��������ȡ����
     */

    /*
     * ��Ŀ2��
     * ������װ����Ϣ�࣬װ��������id������������Ա��
     * ��һ���������а���һ��List�洢��ӵ�е�����װ����Ϣ��
     * ������һ��Ļ����ϣ���װ����Ϣ�Ĵ洢�Ͷ�ȡ����
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
            Name = PlayerPrefs.GetString("Name", "δ����");
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

        player.Name = "����";
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
            print("װ��ID��" + eq.ID + " ������" + eq.Num);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
