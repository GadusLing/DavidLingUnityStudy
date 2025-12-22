using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


/// <summary>
/// 单例模式基础类 当写单例模式时(往往是管理器) 只要继承该类即可
/// 使用方法：
/// 1. 继承 BaseManager<T>，T 为子类类型。
/// 2. 通过 BaseManager<T>.Instance 访问单例实例。
/// </summary>
/// <typeparam name="T">子类类型 如 PlayerManager之类的管理器</typeparam>
public abstract class SingletonBase<T> where T : class//, new()
{
    /// <summary>
    /// 私有 静态 的单例实例。
    /// </summary>
    private static T _instance;

    /// <summary>
    /// 锁对象 用于多线程安全
    /// </summary>
    protected static readonly object _lock = new object();

    /// <summary>
    /// 公共 静态 的属性 只能get 不能set
    /// </summary>
    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                lock(_lock)
                {
                    if (_instance == null)
                    {
                        //_instance = new T();
                        Type type = typeof(T);
                        ConstructorInfo info = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
                        if (info != null)
                            _instance = info.Invoke(null) as T;
                        else
                            Debug.LogError("为了安全性问题 在继承 SingletonBase 的类中 必须手动写一个私有的无参构造函数");
                    }
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// 构造函数 设置为 protected，防止外部直接实例化。
    /// </summary>
    protected SingletonBase() { }
}

