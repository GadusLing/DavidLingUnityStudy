using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lesson17 : MonoBehaviour
{
    public Button btnSend;
    // Start is called before the first frame update
    void Start()
    {
        btnSend.onClick.AddListener(() =>
        {
            PlayerMsg msg = new PlayerMsg();
            msg.data = new PlayerData();
            msg.playerID = 1;
            msg.data.name = "小凌哥的客户端发送消息";
            msg.data.atk = 888;
            msg.data.lev = 666;
            UdpAsyncNetMgr.Instance.Send(msg);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
