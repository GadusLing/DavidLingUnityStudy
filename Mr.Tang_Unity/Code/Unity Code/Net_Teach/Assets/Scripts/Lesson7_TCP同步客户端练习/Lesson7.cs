using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Lesson7 : MonoBehaviour
{
    public Button btn;
    public Button btn1; // 黏包测试
    public Button btn2; // 分包测试
    public Button btn3; // 黏包+分包测试
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

        btn1.onClick.AddListener(() => // 黏包测试
        {
            PlayerMsg msg = new PlayerMsg(); // 创建一个PlayerMsg对象
            msg.playerID = 123; // 设置玩家ID
            msg.data = new PlayerData(); // 创建一个PlayerData对象并赋值给msg.data
            msg.data.name = "黏包测试消息1"; // 设置玩家姓名
            msg.data.atk = 1; // 设置玩家攻击力
            msg.data.lev = 1; // 设置玩家等级

            PlayerMsg msg2 = new PlayerMsg(); // 创建一个PlayerMsg对象
            msg2.playerID = 124; // 设置玩家ID
            msg2.data = new PlayerData(); // 创建一个PlayerData对象并赋值给msg2.data
            msg2.data.name = "黏包测试消息2"; // 设置玩家姓名
            msg2.data.atk = 2; // 设置玩家攻击力
            msg2.data.lev = 2; // 设置玩家等级

            byte[] msg1Bytes = msg.Writing();
            byte[] msg2Bytes = msg2.Writing();
            byte[] bytes = new byte[msg1Bytes.Length + msg2Bytes.Length]; // 创建一个字节数组用于存放两条消息的字节数据
            msg1Bytes.CopyTo(bytes, 0); // 将第一条消息的字节数据复制到字节数组的前半部分
            msg2Bytes.CopyTo(bytes, msg1Bytes.Length); // 将第二条消息的字节数据复制到字节数组的后半部分
            NetManager.Instance.SendTest(bytes); // 直接发送字节数据，测试黏包用

        });

        btn2.onClick.AddListener(async () => // 分包测试
        {
            PlayerMsg msg3 = new PlayerMsg(); // 创建一个PlayerMsg对象
            msg3.playerID = 1003; // 设置玩家ID
            msg3.data = new PlayerData(); // 创建一个PlayerData对象并赋值给msg3.data
            msg3.data.name = "分包测试消息"; // 设置玩家姓名
            msg3.data.atk = 3; // 设置玩家攻击力
            msg3.data.lev = 3; // 设置玩家等级

            byte[] bytes = msg3.Writing(); // 获取消息的字节数据，包含4字节表头 + 4字节消息长度 + 身体长度
            byte[] bytes1 = new byte[10]; // 创建一个字节数组用于存放消息的前10个字节，模拟分包的第一部分
            byte[] bytes2 = new byte[bytes.Length - 10]; // 创建一个字节数组用于存放消息的剩余字节，模拟分包的第二部分
            Array.Copy(bytes, 0, bytes1, 0, bytes1.Length); // 将消息的前10个字节复制到bytes1中，模拟分包的第一部分
            Array.Copy(bytes, bytes1.Length, bytes2, 0, bytes2.Length); // 将消息的剩余字节复制到bytes2中，模拟分包的第二部分
            NetManager.Instance.SendTest(bytes1); // 直接发送bytes1，测试分包的第一部分
            await Task.Delay(500); // 等待500毫秒，模拟网络延迟
            NetManager.Instance.SendTest(bytes2); // 直接发送bytes2，测试分包的第二部分


        });

        btn3.onClick.AddListener(async () => // 黏包+分包测试
        {
            PlayerMsg msg = new PlayerMsg(); // 创建一个PlayerMsg对象
            msg.playerID = 123; // 设置玩家ID
            msg.data = new PlayerData(); // 创建一个PlayerData对象并赋值给msg.data
            msg.data.name = "黏包测试消息1"; // 设置玩家姓名
            msg.data.atk = 1; // 设置玩家攻击力
            msg.data.lev = 1; // 设置玩家等级

            PlayerMsg msg2 = new PlayerMsg(); // 创建一个PlayerMsg对象
            msg2.playerID = 124; // 设置玩家ID
            msg2.data = new PlayerData(); // 创建一个PlayerData对象并赋值给msg2.data
            msg2.data.name = "黏包测试消息2"; // 设置玩家姓名
            msg2.data.atk = 2; // 设置玩家攻击力
            msg2.data.lev = 2; // 设置玩家等级

            byte[] bytes1 = msg.Writing();
            byte[] bytes2 = msg2.Writing();

            byte[] bytes2_1 = new byte[10]; // 创建一个字节数组用于存放消息2的前10个字节，模拟分包的第一部分
            byte[] bytes2_2 = new byte[bytes2.Length - 10]; // 创建一个字节数组用于存放消息2的剩余字节，模拟分包的第二部分
            Array.Copy(bytes2, 0, bytes2_1, 0, bytes2_1.Length); // 将消息2的前10个字节复制到bytes2_1中，模拟分包的第一部分
            Array.Copy(bytes2, bytes2_1.Length, bytes2_2, 0, bytes2_2.Length); // 将消息2的剩余字节复制到bytes2_2中，模拟分包的第二部分
            byte[] bytes = new byte[bytes1.Length + bytes2_1.Length]; // 创建一个字节数组用于存放消息1的字节数据和消息2的分包第一部分，模拟黏包+分包的情况
            bytes1.CopyTo(bytes, 0); // 将消息1的字节数据复制到字节数组的前半部分，模拟分包的第一部分
            bytes2_1.CopyTo(bytes, bytes1.Length); // 将消息2的分包第一部分的字节数据复制到字节数组的后半部分，模拟分包的第二部分


            NetManager.Instance.SendTest(bytes); // 直接发送bytes，测试黏包+分包的情况
            await Task.Delay(500); // 等待500毫秒，模拟网络延迟
            NetManager.Instance.SendTest(bytes2_2); // 直接发送bytes2_2，测试分包的第二部分
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
