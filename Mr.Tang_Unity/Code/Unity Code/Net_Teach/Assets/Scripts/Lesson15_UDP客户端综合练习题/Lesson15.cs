using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lesson15 : MonoBehaviour
{
    public Button btnSend; //发送消息的按钮
    // Start is called before the first frame update
    void Start()
    {
        btnSend.onClick.AddListener(() => //给发送消息的按钮添加点击事件监听器，当按钮被点击时执行发送消息的操作
        {
            PlayerMsg msg1 = new PlayerMsg() //创建一个PlayerMsg对象，表示玩家消息
            {
                playerID = 1,
                data = new PlayerData() //创建一个PlayerData对象，表示玩家的数据
                {
                    name = "小凌哥客户端发的消息",
                    lev = 10,
                    atk = 100
                }
            };
            UdpNetMgr.Instance.Send(msg1); //通过UdpNetMgr的实例调用Send方法，发送玩家消息到服务器
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
