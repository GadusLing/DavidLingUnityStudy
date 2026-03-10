using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson10 : MonoBehaviour
{
	#region 服务端
	// 1. 收发消息时判断 socket 是否已经断开
	// 2. 处理删除记录的 socket 的相关逻辑（会用到线程锁）

	// 3. 自定义退出消息
	// 让服务器端收到该消息就知道是客户端想要主动断开
	// 然后服务器端处理释放 socket 相关工作
	#endregion

	#region 总结
	// 客户端可以通过 Disconnect 方法主动和服务器端断开连接
	// 服务器端可以通过 Connected 属性判断连接状态决定是否释放 Socket

	// 但是由于服务器端 Connected 变量表示的是上一次收发消息是否成功
	// 所以服务器端无法准确判断客户端的连接状态
	// 因此，我们需要自定义一条退出消息用于准确断开和客户端之间的连接
	#endregion

}
