using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

/// <summary>
/// 所有需要序列化和反序列化数据类的基类，提供自动计算字节数和序列化 已用反射和泛型优化
/// </summary>
public abstract class BaseData
{
    /// <summary>
    /// 获取字节数组容器大小的方法
    /// 使用反射自动计算，子类可直接使用，也可以重写，反射性能一般 若游戏每秒要序列化成千上万个对象，还是建议在子类里 override 重写这两个方法，手动去
    /// </summary>
    /// <returns>字节数组的大小</returns>
    public virtual int GetBytesNum()
    {
        int num = 0; // 用于累计字节数，确定字节数组大小
        // 获取所有公共成员变量
        // 如果想要获取私有、保护成员，可以在GetFields中传入 BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
        FieldInfo[] infos = this.GetType().GetFields(); // FieldInfo是个反射类，包含字段的信息
        // this.GetType() 获取当前对象的类型 .GetFields() 获取该类型的所有字段信息 用FieldInfo[]数组存储

        foreach (FieldInfo info in infos) // 遍历每个字段
        {
            object value = info.GetValue(this); // info代表某个字段，使用info.GetValue(this)获取该字段在当前对象(this)中的值
            // 用object类型接收，因为不知道字段具体是什么类型

            Type type = info.FieldType; // Type是个反射类，包含类型的信息，info.FieldType获取字段的类型

            if (type == typeof(int))
                num += sizeof(int);
            else if (type == typeof(long))
                num += sizeof(long);
            else if (type == typeof(short))
                num += sizeof(short);
            else if (type == typeof(float))
                num += sizeof(float);
            else if (type == typeof(byte))
                num += sizeof(byte);
            else if (type == typeof(bool))
                num += sizeof(bool);
            else if (type == typeof(string))
            {
                // string需要算 长度前缀(4字节) + 实际字节长
                // 如果为空，就只算长度前缀4个字节（长度为0）
                if (value == null)
                    num += sizeof(int);
                else
                    num += sizeof(int) + Encoding.UTF8.GetBytes(value.ToString()).Length;
            }
            else if (type.IsSubclassOf(typeof(BaseData))) // IsSubclassOf 判断是否是BaseData的子类
            {
                // 如果是嵌套的BaseData结构，递归调用 
                num += ((BaseData)value).GetBytesNum();
                // 因为value是object类型，它没有GetBytesNum方法 需要强制转换成BaseData类型才能调用GetBytesNum方法
            }
        }
        return num;
    }

    /// <summary>
    /// 把成员变量序列化成字节数组的方法 
    /// 使用反射自动序列化，子类可直接使用，也可以重写
    /// </summary>
    /// <returns>序列化后的字节数组</returns>
    public virtual byte[] Writing()
    {
        int index = 0; // 用于记录当前写入字节数组的位置
        byte[] bytes = new byte[GetBytesNum()];// 先计算需要的字节数，初始化数组大小

        // 利用反射遍历所有字段进行写入
        // 如果想要获取私有、保护成员，可以在GetFields中传入 BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
        FieldInfo[] infos = this.GetType().GetFields(); // FieldInfo是个反射类，包含字段的信息
        foreach (FieldInfo info in infos)
        {
            object value = info.GetValue(this);// info代表某个字段，使用info.GetValue(this)获取该字段在当前对象(this)中的值
            // 用object类型接收，因为不知道字段具体是什么类型
            Type type = info.FieldType;// Type是个反射类，包含类型的信息，info.FieldType获取字段的类型

            if (type == typeof(int))
                Write(bytes, (int)value, ref index);
            else if (type == typeof(long))
                Write(bytes, (long)value, ref index);
            else if (type == typeof(short))
                Write(bytes, (short)value, ref index);
            else if (type == typeof(float))
                Write(bytes, (float)value, ref index);
            else if (type == typeof(byte))
                WriteByte(bytes, (byte)value, ref index);
            else if (type == typeof(bool))
                Write(bytes, (bool)value, ref index);
            else if (type == typeof(string))
            {
                // 如果是null，就写入空字符串 否则正常写入
                string str = value == null ? "" : (string)value; 
                WriteString(bytes, str, ref index);
            }
            else if (type.IsSubclassOf(typeof(BaseData)))
                WriteData(bytes, (BaseData)value, ref index);
        }
        return bytes;
    }

    /// <summary>
    /// 通用的写入方法 (int, short, long, float, bool...)
    /// </summary>
    protected void Write<T>(byte[] bytes, T value, ref int index)
    {
        // dynamic 在部分Unity环境下可能报错，我们改用具体的类型判断
        byte[] data = null;

        if (value is int i) 
            data = BitConverter.GetBytes(i);
        else if (value is long l) 
            data = BitConverter.GetBytes(l);
        else if (value is short s) 
            data = BitConverter.GetBytes(s);
        else if (value is float f) 
            data = BitConverter.GetBytes(f);
        else if (value is bool b) 
            data = BitConverter.GetBytes(b);

        if (data != null)
        {
            data.CopyTo(bytes, index);
            index += data.Length;
        }
        else
        {
            Debug.LogError("暂不支持的序列化类型: " + typeof(T));
        }
    }

    /// <summary>
    /// 写入单个字节的方法
    /// </summary>
    protected void WriteByte(byte[] bytes, byte value, ref int index)
    {
        bytes[index] = value;
        index += sizeof(byte);
    }

    /// <summary>
    /// 写入字符串的方法
    /// </summary>
    protected void WriteString(byte[] bytes, string value, ref int index)
    {
        byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(value);
        // 先存储字符串的长度
        Write(bytes, strBytes.Length, ref index);
        // 再存储字符串的内容
        strBytes.CopyTo(bytes, index); 
        index += strBytes.Length;
    }

    /// <summary>
    /// 写入嵌套BaseData对象的方法
    /// </summary>
    protected void WriteData(byte[] bytes, BaseData data, ref int index)
    {
        data.Writing().CopyTo(bytes, index);
        index += data.GetBytesNum();
    }

}
