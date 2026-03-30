using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消息处理器基类
/// 1. 每个消息对应一个处理器类，负责处理该消息的逻辑
/// 2. 处理器类继承自BaseHandler，重写HandleMsg方法
/// </summary>
public abstract class BaseHandler
{
    public BaseMsg message; // 要处理的消息对象


    /// <summary>
    /// 处理消息的方法，子类需要重写该方法
    /// </summary>
    /// <param name="msg">要处理的消息对象</param>
    public abstract void MsgHandle();
}
