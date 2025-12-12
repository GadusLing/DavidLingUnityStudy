using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 继承了MonoBehaviour的单例模式基类
/// 当我们需要一个全局唯一的MonoBehaviour实例时，可以使用此类作为基类
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
    /// Unity的Awake方法，用于初始化单例。
    /// </summary>
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            Debug.LogError($"[SingletonMono] 场景中存在多个 {typeof(T).Name} 实例，请确保只有一个实例存在！");
            Destroy(gameObject); // 销毁重复的实例
        }
    }
}
