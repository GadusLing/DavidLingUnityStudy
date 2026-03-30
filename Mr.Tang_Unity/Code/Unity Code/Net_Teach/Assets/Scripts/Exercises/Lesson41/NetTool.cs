using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Google.Protobuf;
using UnityEngine;

public class NetTool
{
    // 序列化Protobuf生成的对象为字节数组
    public static byte[] GetProtoBytes(IMessage msg)
    {
        // byte[] bytes = null; // 存储序列化后的字节数组
        // using (MemoryStream ms = new MemoryStream()) // 创建内存流对象，用于存储序列化后的数据
        // {
        //     msg.WriteTo(ms); // 将Protobuf生成的对象序列化写入内存流
        //     bytes = ms.ToArray(); // 将内存流中的数据转换为字节数组
        // }
        // return bytes;

        return msg.ToByteArray();
    }

    // 反序列化字节数组为Protobuf生成的对象
    public static T GetProtoMsg<T>(byte[] bytes) where T :class, IMessage<T>
    {
        // 得到对应消息的类型，通过反射得到内部的静态成员 然后得到其中的 Parser 属性，最后调用 ParseFrom 方法进行反序列化
        Type type = typeof(T); // 获取泛型参数T的类型信息
        var pInfo = type.GetProperty("Parser", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static); // 获取类型中的静态公共属性 "Parser"
        var parserObj = pInfo.GetValue(null, null); // 获取 "Parser" 属性的值，即 Protobuf 生成的消息类中的静态 Parser 对象
        var mInfo = parserObj.GetType().GetMethod("ParseFrom", new Type[] { typeof(byte[]) }); // 获取 Parser 对象中的 ParseFrom 方法，该方法用于将字节数组反序列化为消息对象
        var result = mInfo.Invoke(parserObj, new object[] { bytes }); // 调用 ParseFrom 方法进行反序列化
        
        return result as T;
    }



}
