using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源加载管理器
/// </summary>
public class ResManager : SingletonBase<ResManager>
{
    // 同步加载资源
    public T LoadRes<T>(string resPath) where T : Object
    {
        T res = Resources.Load<T>(resPath);
        // 如果对象是一个GameObject类型的资源 则实例化后返回新对象 外部可以直接使用
        if (res is GameObject)
        {
            return GameObject.Instantiate(res) as T;
        }
        else // 类似TextAsset AudioClip等非GameObject类型的资源 直接返回加载的资源
        {
            return res;
        }
    }
    
    /// <summary>
    /// 异步加载资源 负责开启协程
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resPath"></param>
    /// <param name="callback"></param>
    public void LoadResAsync<T>(string resPath, UnityAction<T> callback) where T : Object
    {
        // 开启协程进行异步加载
        MonoManager.Instance.StartCoroutine(LoadResAsyncCoroutine(resPath, callback));
    }
    
    /// <summary>
    /// 异步加载资源的协程 负责具体的异步加载操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resPath"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator LoadResAsyncCoroutine<T>(string resPath, UnityAction<T> callback) where T : Object
    {
        ResourceRequest request = Resources.LoadAsync<T>(resPath);
        yield return request; // 等待异步加载完成

        T res = request.asset as T;
        // 如果对象是一个GameObject类型的资源 则实例化后返回新对象 外部可以直接使用
        if (res is GameObject)
        {
            callback(GameObject.Instantiate(res) as T);
        }
        else // 类似TextAsset AudioClip等非GameObject类型的资源 直接返回加载的资源
        {
            callback(res);
        }
    }
}