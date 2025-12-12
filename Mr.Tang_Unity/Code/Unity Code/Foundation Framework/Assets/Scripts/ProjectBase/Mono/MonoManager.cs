using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class MonoManager : SingletonBase<MonoManager>
{
    private MonoController monoController;
    /// <summary>
    /// 由于继承了单例模式基类，所以在第一次进入时会调用构造函数MonoManager() MonoManager会在场景上创建一个MonoController对象
    /// 并且给对象添加 MonoController 组件，从而实现了 可以使用协程和Update更新的功能
    /// </summary>
    public MonoManager()
    {
        // 保证了 MonoController 对象的唯一性
        GameObject monoManagerGO = new GameObject("MonoController");
        monoController = monoManagerGO.AddComponent<MonoController>();
    }

    /// <summary>
    /// 封装 MonoController 的 AddUpdateListener 方法 给外部使用
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateListener(UnityAction action)
    {
        monoController.AddUpdateListener(action);
    }
    /// <summary>
    /// 封装 MonoController 的 RemoveUpdateListener 方法 给外部使用
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateListener(UnityAction action)
    {
        monoController.RemoveUpdateListener(action);
    }

    public void StartCoroutine(IEnumerator routine)
    {
        monoController.StartCoroutine(routine);
    }

    public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
    {
        return monoController.StartCoroutine(methodName, value);
    }
    /// <summary>
    /// 重写的这个通过string函数名启动协程 只适用于开启monoController自身的函数
    /// </summary>
    /// <param name="methodName"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(string methodName)
    {
        return monoController.StartCoroutine(methodName);
    }
    
}
