using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lesson7 : MonoBehaviour
{
    public Button btn;
    public InputField input;
    void Start()
    {
        btn.onClick.AddListener(() => // 给按钮添加点击事件监听器
        {
            PlayerMsg msg = new PlayerMsg(); // 创建一个PlayerMsg对象
            msg.playerID = 123; // 设置玩家ID
            msg.data = new PlayerData(); // 创建一个PlayerData对象并赋值给msg.data
            msg.data.name = input.text; // 设置玩家姓名
            msg.data.atk = 50; // 设置玩家攻击力
            msg.data.lev = 10; // 设置玩家等级
            NetManager.Instance.Send(msg); // 通过NetManager发送消息
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
