using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseData
{
    /// <summary>
    /// 用于子类重写的 获取字节数组容器大小的方法
    /// </summary>
    /// <returns></returns>
    public abstract int GetBytesNum();

    /// <summary>
    /// 把成员变量序列化成字节数组的方法 需要子类重写
    /// </summary>
    /// <returns></returns>
    public abstract byte[] Writing();

    /// <summary>
    /// 存储int类型数据到字节数组的方法
    /// </summary>
    /// <param name="bytes">指定字节数组</param>
    /// <param name="value">要存储的int值</param>
    /// <param name="index">当前索引位置，存储后会更新</param>
    protected void WriteInt(byte[] bytes, int value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += sizeof(int);
    }

    protected void WriteShort(byte[] bytes, short value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += sizeof(short);
    }

    protected void WriteLong(byte[] bytes, long value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += sizeof(long);
    }

    protected void WriteFloat(byte[] bytes, float value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += sizeof(float);
    }

    protected void WriteByte(byte[] bytes, byte value, ref int index)
    {
        bytes[index] = value;
        index += sizeof(byte);
    }

    protected void WriteBool(byte[] bytes, bool value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += sizeof(bool);
    }

    protected void WriteString(byte[] bytes, string value, ref int index)
    {
        byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(value);
        // 先存储字符串的长度
        WriteInt(bytes, strBytes.Length, ref index);
        // 再存储字符串的内容
        strBytes.CopyTo(bytes, index); 
        index += strBytes.Length;
    }

    protected void WriteData(byte[] bytes, BaseData data, ref int index)
    {
        data.Writing().CopyTo(bytes, index);
        index += data.GetBytesNum();
    }
    


}
