using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 继承了MonoBehaviour的单例模式基类
/// 注意 一定要挂载在场景中的某个GameObject上
/// 并且因为继承了MonoBehaviour 所以一定不要 new 来创建实例
/// 非必要情况下，尽量避免使用此单例模式 或者说最好不要用，很容易破坏单例的特性，了解即可
/// 使用方法：
/// 1. 继承 SingletonMono<T>，T 为子类类型。
/// 2. 通过 SingletonMono<T>.Instance 访问单例实例。
/// </summary>
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    /// <summary>
    /// 获取单例实例。
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError($"[SingletonMono] 单例实例未初始化！请确保场景中正确挂载了 {typeof(T).Name}，或者通过代码动态添加。");
            }
            return _instance;
        }
    }

    /// <summary>
    /// Unity的生命周期函数Awake，在初始化时会自动调用，所以在Awake中设置单例实例。
    /// </summary>
    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);// 这里没有直接移除object 是因为object可能有其他组件 不想影响其他组件的使用
            return;
        }
        _instance = this as T;
        DontDestroyOnLoad(this.gameObject);
    }
}
