using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerInfo p = new PlayerInfo();
        p.LoadData("25_09_25_Practice");
        Debug.Log(p.name);
        Debug.Log(p.atk);
        Debug.Log(p.def);
        Debug.Log(p.moveSpeed);
        Debug.Log(p.roundSpeed);
        Debug.Log(p.weapon.id);
        Debug.Log(p.weapon.num);
        foreach (var i in p.listInt)
        {
            Debug.Log(i);
        }
        foreach (var item in p.itemList)
        {
            Debug.Log(item.id + " " + item.num);
        }
        foreach (var kv in p.itemDic)
        {
            Debug.Log(kv.Key + " " + kv.Value.id + " " + kv.Value.num);
        }

        p.name = "修改后测试";
        p.listInt.Add(99);
        p.itemList.Add(new Item() { id = 99, num = 1 });
        p.itemDic.Add(99, new Item() { id = 99, num = 1 });
        p.SaveData("25_09_25_Practice");
        print(Application.persistentDataPath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
