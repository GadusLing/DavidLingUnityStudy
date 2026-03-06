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
    public List<int> tags;
    public Dictionary<int, string> items;
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
        // 1. 测试序列化：创建并全量赋值一个 TestInfo
        TestInfo info = new TestInfo();
        info.lev = 87;
        info.p = new Player();
        info.p.atk = 9999;
        info.hp = 5000;
        info.name = "David";
        info.sex = true;
        
        info.tags = new List<int>() { 101, 202, 303 };
        info.items = new Dictionary<int, string>() 
        {
            { 1, "屠龙刀" },
            { 2, "倚天剑" }
        };

        // 执行序列化：获取最终发包用字节数组
        byte[] data = info.Writing();
        Debug.Log($"序列化成功！总共获得字节长度为：{data.Length}");

        // ===================================
        // 2. 测试反序列化：模拟网络另一端收到字节数组的情况
        // ===================================
        
        // 模拟接收端创建了一个空的骨架
        TestInfo receiveInfo = new TestInfo();
        
        // 将刚才得到的字节流读取进新的空实例里
        receiveInfo.Reading(data);

        // 3. 结果验证：打印出来检查两边数据是否一模一样
        Debug.Log("======= 反序列化测试结果 =======");
        Debug.Log($"收到 lev: {receiveInfo.lev}");
        Debug.Log($"收到 hp: {receiveInfo.hp}");
        Debug.Log($"收到 name: {receiveInfo.name}");
        Debug.Log($"收到 sex: {receiveInfo.sex}");
        if (receiveInfo.p != null)
        {
            Debug.Log($"收到 嵌套的Player.atk: {receiveInfo.p.atk}");
        }
        else
        {
            Debug.LogError("嵌套的Player解析失败，对象为null！");
        }

        Debug.Log("--- 测试 List 解析 ---");
        if (receiveInfo.tags != null)
        {
            foreach (var t in receiveInfo.tags) Debug.Log($"收到 list tag: {t}");
        }
        
        Debug.Log("--- 测试 Dictionary 解析 ---");
        if (receiveInfo.items != null)
        {
            foreach (var kv in receiveInfo.items) Debug.Log($"收到 dict item: [{kv.Key}] = {kv.Value}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
