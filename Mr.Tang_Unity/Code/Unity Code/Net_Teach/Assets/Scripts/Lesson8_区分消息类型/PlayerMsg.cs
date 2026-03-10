using System.Collections;
using System.Collections.Generic;

public class PlayerMsg : BaseMsg
{
    public int playerID; 
    public PlayerData data; 

    public override byte[] Writing()
    {
        // 1. 让父类按它成员变量的实际大小，老老实实打个纯数据的包
        byte[] body = base.Writing(); 
        
        // 2. 4字节表头 + 4字节消息长度 + 身体长度
        byte[] bytes = new byte[4 + 4 + body.Length]; 
        
        // 3. 游标从0开始，写入你的专属 1001 标识 和消息长度
        int index = 0; 
        WriteInt(bytes, GetID(), ref index);
        WriteInt(bytes, body.Length, ref index);
        
        // 4. 把刚刚打包好的、长度绝对正确的实体数据粘在后面
        body.CopyTo(bytes, index); 
        
        return bytes;
    }

    public override int GetID() 
    {
        return 1001; 
    }
}