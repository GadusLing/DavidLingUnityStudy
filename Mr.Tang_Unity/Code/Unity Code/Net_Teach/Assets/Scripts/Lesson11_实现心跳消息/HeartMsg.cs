using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartMsg : BaseMsg
{
    public override byte[] Writing()
    {
        int index = 0; 
        byte[] bytes = new byte[4 + 4]; // 4字节表头 + 4字节消息长度，心跳消息没有实体数据了，所以总长度就是8字节了
        WriteInt(bytes, GetID(), ref index); // 写入你的专属 1004 标识
        WriteInt(bytes, 0, ref index); // 消息长度为0，因为心跳消息没有实体数据
        
        return bytes;
    }

    public override int GetID() 
    {
        return 999; 
    }
}
