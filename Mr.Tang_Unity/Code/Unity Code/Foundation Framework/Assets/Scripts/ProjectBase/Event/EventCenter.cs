using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 事件中心模块 观察者模式
/// 1.事件的注册
/// 2.事件的移除
/// 3.事件的派发
/// </summary>
public class EventCenter : SingletonBase<EventCenter>
{
    /// <summary>
    /// 事件字典  用于存储事件名和对应的监听这个事件的委托方法们
    /// </summary>
    private Dictionary<string, UnityAction<object>> eventDic = new Dictionary<string, UnityAction<object>>();

    /// <summary>
    /// 注册事件 监听
    /// </summary>
    /// <param name="eventName">事件的名字</param>
    /// <param name="action">用来处理事件的委托方法</param>
    public void AddEventListener(string eventName, UnityAction<object> action)
    {
        // 如果没有这个事件 就添加一个新的事件和对应的委托方法
        if (!eventDic.ContainsKey(eventName))
        {
            eventDic.Add(eventName, action);
        }
        // 如果有这个事件 就把对应的委托方法添加到这个事件中去
        else
        {
            eventDic[eventName] += action;
        }
    }

    /// <summary>
    /// 移除事件 取消监听
    /// </summary>
    /// <param name="eventName">事件的名字</param>
    /// <param name="action">用来处理事件的委托方法</param>
    public void RemoveEventListener(string eventName, UnityAction<object> action)
    {
        // tryGetValue 得到的是“副本” 记着不要滥用
        if (eventDic.ContainsKey(eventName))
        {
            eventDic[eventName] -= action;
        }        
    }

    //什么时候用 TryGetValue？ 当你只想“读数据”或“判断并拿出一份值”： 查表 取配置 拿物体引用 不会被你修改
    //什么时候用 ContainsKey？ 当你要“修改字典里的值”： += -= 修改对象内部状态 删除、更新

    /// <summary>
    /// 派发事件 触发事件
    /// </summary>
    /// <param name="eventName">事件的名字</param>
    /// <param name="param">传递的参数</param>
    public void EventTrigger(string eventName, object param = null)
    {
        if (eventDic.ContainsKey(eventName))
        {
            eventDic[eventName]?.Invoke(param);
        }
    }

    /// <summary>
    /// 清空事件中心 用于切场景时 之前场景的物体销毁 但是事件中心还在持有这些物体的委托方法 导致报错
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
}
