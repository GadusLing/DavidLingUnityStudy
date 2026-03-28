using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson37 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 分析数据结构类的构成

        #endregion

        #region 知识点二 制作生成数据结构类步骤

        #endregion

        #region 知识点三 制作生成成员变量声明的功能

        #endregion

        #region 知识点三 测试工具生成的内容是否正确
        GamePlayer.PlayerMsg msg = new GamePlayer.PlayerMsg();
        msg.playerID = 999;
        msg.data = new GamePlayer.PlayerData();
        msg.data.id = 888;
        msg.data.atk = 99.9f;
        msg.data.sex = true;
        msg.data.lev = 99999;
        msg.data.arrays = new int[] { 1, 2, 3, 4, 5 };
        msg.data.list = new List<int>() { 11, 22, 33, 44, 55 };
        msg.data.dic = new Dictionary<int, string>() { { 1, "one" }, { 2, "two" }, { 3, "three" } };
        msg.data.heroType = GamePlayer.E_HERO_TYPE.MAIN;

        byte[] bytes = msg.Writing();
        int index = 0;
        int msgID = BitConverter.ToInt32(bytes, index);
        index += 4;
        int msgLength = BitConverter.ToInt32(bytes, index + 4);
        index += 4;
        
        GamePlayer.PlayerMsg msgR = new GamePlayer.PlayerMsg();
        msgR.Reading(bytes, index);

        print(msgR.playerID);

        #endregion


    }

    // Update is called once per frame
    void Update()
    {

    }
}
