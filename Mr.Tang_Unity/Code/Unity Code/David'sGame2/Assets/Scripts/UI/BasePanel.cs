using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BasePanel 是所有 UI 面板的抽象基类，采用泛型单例模式。
/// 继承此类可实现面板的显示、隐藏和初始化等通用功能。
/// </summary>

public abstract class BasePanel<T> : MonoBehaviour where T : class
{
    /// <summary>
    /// 泛型单例实例，保证每个面板只有一个实例。
    /// </summary>
    private static T instance;
    public static T Instance => instance;
    protected virtual void Awake()
    {
        instance = this as T;
    }

    void Start()
    {
        Init();
    }


    public abstract void Init();

    public virtual void ShowMe()
    {
        gameObject.SetActive(true);
    }

    public virtual void HideMe()
    {
        gameObject.SetActive(false);
    }
}
