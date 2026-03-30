using System;
using System.Collections;
using System.Collections.Generic;
using GamePlayer;
using UnityEngine;

/// <summary>   
/// 消息池类
/// 1. 消息池负责存储消息ID和对应的消息对象类型的映射关系
/// 2. 当接收到消息时，根据消息ID从消息池中获取对应的消息对象类型，进行反序列化和处理
/// 3. 消息池可以通过配置文件或代码进行初始化，添加新的消息类型时只需要在消息池中添加对应的映射关系即可，无需修改网络层的代码，实现了消息处理的自动化和扩展性
/// </summary>
public class MsgPool
{
    private Dictionary<int, Type> msgDic = new Dictionary<int, Type>(); // 消息字典，存储消息ID和对应的消息对象
    private Dictionary<int, Type> handlerDic = new Dictionary<int, Type>(); // 消息处理器字典，存储消息ID和对应的消息处理器对象

    public MsgPool()
    {
        // 在消息池的构造函数中注册消息ID和对应的消息对象类型以及消息处理器对象类型的映射关系
        Register(1001, typeof(PlayerMsg), typeof(PlayerMsgHandler)); // 注册PlayerMsg消息，消息ID为1001，对应的消息处理器为PlayerMsgHandler
        // 可继续注册其他消息类型
        
    }


    private void Register(int msgID, Type msgType, Type handlerType)
    {
        if (!msgDic.ContainsKey(msgID))
        {
            msgDic.Add(msgID, msgType); // 将消息ID和对应的消息对象类型添加到消息字典中
            handlerDic.Add(msgID, handlerType); // 将消息ID和对应的消息处理器对象类型添加到消息处理器字典中
        }
    }

    /// <summary>
    /// 根据消息ID获取对应的消息对象实例
    /// </summary>
    /// <param name="msgID">消息ID</param>
    /// <returns>对应的消息对象实例</returns>
    public BaseMsg GetMessage(int msgID)
    {
        if (msgDic.ContainsKey(msgID))
        {
            return (BaseMsg)Activator.CreateInstance(msgDic[msgID]); // 根据消息ID从消息字典中获取对应的消息对象类型，并创建实例返回
        }
        else
        {
            Debug.LogError($"消息池中没有找到消息ID为 {msgID} 的消息对象类型！"); // 输出错误日志，提示消息池中没有找到对应的消息对象类型
            return null;
        }
    }

    /// <summary>
    /// 根据消息ID获取对应的消息处理器对象实例
    /// </summary>
    /// <param name="msgID">消息ID</param>
    /// <returns>对应的消息处理器对象实例</returns>
    public BaseHandler GetHandler(int msgID)
    {
        if (handlerDic.ContainsKey(msgID))
        {
            return (BaseHandler)Activator.CreateInstance(handlerDic[msgID]); // 根据消息ID从消息处理器字典中获取对应的消息处理器对象类型，并创建实例返回
        }
        else
        {
            Debug.LogError($"消息池中没有找到消息ID为 {msgID} 的消息处理器对象类型！"); // 输出错误日志，提示消息池中没有找到对应的消息处理器对象类型
            return null;
        }
    }

}
