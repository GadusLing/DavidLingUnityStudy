using System.Collections;
using System.Collections.Generic;

public class PlayerMsg : BaseMsg
{
    public int playerID; // 玩家ID字段，用于标识具体的玩家
    public PlayerData data; // 这里包含一个PlayerData类型的字段，来存储玩家相关的数据

    public override byte[] Writing()
    {
        // 1. 让父类按它成员变量的实际大小，老老实实打个纯数据的包
        byte[] body = base.Writing(); 
        
        // 2. 4字节表头 + 身体长度
        byte[] bytes = new byte[4 + body.Length]; 
        
        // 3. 游标从0开始，写入你的专属 1001 标识
        int index = 0; 
        WriteInt(bytes, GetID(), ref index); 
        
        // 4. 把刚刚打包好的、长度绝对正确的实体数据粘在后面
        body.CopyTo(bytes, index); 
        
        return bytes;
    }

    public override int GetID() // PlayerMsg的ID为1001(自定义规则)，来区分不同的消息类型
    {
        return 1001;
    }
}