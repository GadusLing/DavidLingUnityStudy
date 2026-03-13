using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lesson13 : MonoBehaviour
{
    public Button btn; // 发送单条PlayerMsg消息的按钮
    public Button btn1; // 发送两条PlayerMsg消息合并包的按钮
    public Button btn2; // 发送拆包消息的按钮
    public Button btn3; // 发送粘包+拆包消息的按钮
    public InputField input; // 输入框

    void Start()
    {
        // 单条消息发送测试
        btn.onClick.AddListener(() =>
        {
            PlayerMsg ms = new PlayerMsg(); // 创建PlayerMsg消息对象
            ms.playerID = 1111; // 设置玩家ID
            ms.data = new PlayerData(); // 创建玩家数据对象
            ms.data.name = "测试客户端发的消息"; // 设置玩家名字
            ms.data.atk = 22; // 设置攻击力
            ms.data.lev = 10; // 设置等级
            NetAsyncMgr.Instance.Send(ms); // 通过网络管理器发送该消息
        });

        // 粘包测试：一次发送两条消息
        btn1.onClick.AddListener(() =>
        {
            PlayerMsg msg = new PlayerMsg(); // 第一条消息
            msg.playerID = 1001;
            msg.data = new PlayerData();
            msg.data.name = "测试1";
            msg.data.atk = 1;
            msg.data.lev = 1;

            PlayerMsg msg2 = new PlayerMsg(); // 第二条消息
            msg2.playerID = 1002;
            msg2.data = new PlayerData();
            msg2.data.name = "测试2";
            msg2.data.atk = 2;
            msg2.data.lev = 2;

            // 正确做法：先获取实际字节数组长度
            byte[] msgBytes1 = msg.Writing();
            byte[] msgBytes2 = msg2.Writing();
            byte[] bytes = new byte[msgBytes1.Length + msgBytes2.Length];
            msgBytes1.CopyTo(bytes, 0);
            msgBytes2.CopyTo(bytes, msgBytes1.Length);
            NetAsyncMgr.Instance.SendTest(bytes); // 发送合并后的字节数组
        });

        // 拆包测试：一条消息拆成两次发
        btn2.onClick.AddListener(async () =>
        {
            PlayerMsg msg = new PlayerMsg(); // 创建消息
            msg.playerID = 1003;
            msg.data = new PlayerData();
            msg.data.name = "测试1";
            msg.data.atk = 3;
            msg.data.lev = 3;

            byte[] bytes = msg.Writing(); // 获取消息字节数组
            byte[] bytes1 = new byte[10]; // 第一段
            byte[] bytes2 = new byte[bytes.Length - 10]; // 第二段
            System.Array.Copy(bytes, 0, bytes1, 0, 10); // 拆分前10字节
            System.Array.Copy(bytes, 10, bytes2, 0, bytes.Length - 10); // 拆分剩余部分

            NetAsyncMgr.Instance.SendTest(bytes1); // 先发第一段
            await System.Threading.Tasks.Task.Delay(500); // 等待500毫秒
            NetAsyncMgr.Instance.SendTest(bytes2); // 再发第二段
        });

        // 粘包+拆包测试
        btn3.onClick.AddListener(async () =>
        {
            PlayerMsg msg = new PlayerMsg(); // 第一条消息
            msg.playerID = 1001;
            msg.data = new PlayerData();
            msg.data.name = "测试1";
            msg.data.atk = 1;
            msg.data.lev = 1;

            PlayerMsg msg2 = new PlayerMsg(); // 第二条消息
            msg2.playerID = 1002;
            msg2.data = new PlayerData();
            msg2.data.name = "测试2";
            msg2.data.atk = 2;
            msg2.data.lev = 2;

            byte[] bytes1 = msg.Writing(); // 第一条消息字节数组
            byte[] bytes2 = msg2.Writing(); // 第二条消息字节数组

            byte[] bytes2_1 = new byte[10]; // 第二条消息的前10字节
            byte[] bytes2_2 = new byte[bytes2.Length - 10]; // 第二条消息剩余部分
            System.Array.Copy(bytes2, 0, bytes2_1, 0, 10); // 拆分前10字节
            System.Array.Copy(bytes2, 10, bytes2_2, 0, bytes2.Length - 10); // 拆分剩余部分

            // 第一条完整消息+第二条前半段合并
            byte[] bytes = new byte[bytes1.Length + bytes2_1.Length]; // 合并数组
            bytes1.CopyTo(bytes, 0); // 拷贝第一条
            bytes2_1.CopyTo(bytes, bytes1.Length); // 拷贝第二条前半段

            NetAsyncMgr.Instance.SendTest(bytes); // 先发合并包
            await System.Threading.Tasks.Task.Delay(500); // 等待500毫秒
            NetAsyncMgr.Instance.SendTest(bytes2_2); // 再发第二条剩余部分
        });
    }

    void Update()
    {
        // ...existing code...
    }
}
