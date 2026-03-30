using System.Collections;
using System.Collections.Generic;
using GameSystem;
using UnityEngine;

public class QuitMsgHandler : BaseHandler
{
	public override void MsgHandle()
	{
		QuitMsg msg = message as QuitMsg; // 将消息对象转换为QuitMsg类型
		//以后我们处理对应某一个消息的逻辑只需要在消息处理者对象的
		//消息处理方法中写逻辑就行了
	}
}