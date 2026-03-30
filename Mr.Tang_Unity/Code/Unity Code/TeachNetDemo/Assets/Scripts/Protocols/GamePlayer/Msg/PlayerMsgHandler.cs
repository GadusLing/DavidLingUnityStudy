using System.Collections;
using System.Collections.Generic;
using GamePlayer;
using UnityEditor.VersionControl;
using UnityEngine;

public class PlayerMsgHandler : BaseHandler
{
    public override void MsgHandle()
    {
        PlayerMsg msg = message as PlayerMsg; // 将消息对象转换为PlayerMsg类型
        //以后我们处理对应某一个消息的逻辑只需要在消息处理者对象的
        //消息处理方法中写逻辑就行了
        Debug.Log(msg.playerID);
    }
}
