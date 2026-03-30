using System.Collections;
using System.Collections.Generic;
using GameSystem;
using UnityEngine;

public class HeartMsgHandler : BaseHandler
{
	public override void MsgHandle()
	{
		HeartMsg msg = message as HeartMsg; // 将消息对象转换为HeartMsg类型
		//以后我们处理对应某一个消息的逻辑只需要在消息处理者对象的
		//消息处理方法中写逻辑就行了
	}
}