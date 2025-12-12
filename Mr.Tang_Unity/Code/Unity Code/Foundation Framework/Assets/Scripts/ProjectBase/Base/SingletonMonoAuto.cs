using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 继承这种自动创建的 单例模式基类 不需要我们手动去挂载 或者 API去加了
/// 使用方法：
/// 1. 继承 SingletonMonoAuto<T>，T 为子类类型。
/// 2. 通过 SingletonMonoAuto<T>.Instance 访问单例实例。
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonMonoAuto<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get 
        {
            if( _instance == null )
            {
                //创建一个新的 GameObject 并设置对象名字为脚本名
                GameObject obj = new GameObject(typeof(T).Name);
                //这个单例模式对象过场景不销毁 单例模式对象 往往 是存在整个程序生命周期中的
                DontDestroyOnLoad(obj);
                //给这个对象添加 T 类型的组件 并赋值给 _instance
                _instance = obj.AddComponent<T>();
            }
            return _instance;
        }
    }
}
