using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 单例模式基础类 当写单例模式时(往往是管理器) 只要继承该类即可
/// 使用方法：
/// 1. 继承 BaseManager<T>，T 为子类类型。
/// 2. 通过 BaseManager<T>.Instance 访问单例实例。
/// </summary>
/// <typeparam name="T">子类类型 如 PlayerManager之类的管理器</typeparam>
public class SingletonBase<T> where T : SingletonBase<T>, new()
{
    /// <summary>
    /// 私有 静态 的单例实例。
    /// </summary>
    private static T _instance;

    /// <summary>
    /// 公共 静态 的属性 只能get 不能set
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }
            return _instance;
        }
    }

    /// <summary>
    /// 构造函数 设置为 protected，防止外部直接实例化。
    /// </summary>
    protected SingletonBase() { }
}

