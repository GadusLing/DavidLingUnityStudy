using System;
using System.Collections.Generic;
using UnityEngine;
using GamePlayer;

/// <summary>
/// 消息池类
/// 自动生成于协议工具
/// </summary>
public class MsgPool
{
	private Dictionary<int, Type> msgDic = new Dictionary<int, Type>(); // 消息字典
	private Dictionary<int, Type> handlerDic = new Dictionary<int, Type>(); // 消息处理器字典

	public MsgPool()
	{
		Register(1001, typeof(PlayerMsg), typeof(PlayerMsgHandler));
	}

	private void Register(int msgID, Type msgType, Type handlerType)
	{
		if (!msgDic.ContainsKey(msgID))
		{
			msgDic.Add(msgID, msgType);
			handlerDic.Add(msgID, handlerType);
		}
	}

	public BaseMsg GetMessage(int msgID)
	{
		if (msgDic.ContainsKey(msgID))
		{
			return (BaseMsg)Activator.CreateInstance(msgDic[msgID]);
		}
		else
		{
			Debug.LogError($"消息池中没有找到消息ID为 {msgID} 的消息对象类型！");
			return null;
		}
	}

	public BaseHandler GetHandler(int msgID)
	{
		if (handlerDic.ContainsKey(msgID))
		{
			return (BaseHandler)Activator.CreateInstance(handlerDic[msgID]);
		}
		else
		{
			Debug.LogError($"消息池中没有找到消息ID为 {msgID} 的消息处理器对象类型！");
			return null;
		}
	}
}
