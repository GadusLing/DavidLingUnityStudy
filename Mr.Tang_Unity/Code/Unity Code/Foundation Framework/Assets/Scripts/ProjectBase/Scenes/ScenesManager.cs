using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景切换管理器 负责场景的加载变化 以及加载后的方法调用
/// </summary>
public class ScenesManager : SingletonBase<ScenesManager>
{
    /// <summary>
    /// 加载场景 - 同步加载 一般用于小场景切换甚至不建议用同步切换场景 大场景建议使用异步加载 不然会有明显卡顿
    /// </summary>
    /// <param name="sceneName">场景名称</param>
    /// <param name="callback">加载完成回调</param>
    public void LoadScene(string sceneName, UnityAction callback)
    {
        //加载新场景
        SceneManager.LoadScene(sceneName);
        //调用加载完成回调
        callback?.Invoke();
    }

    /// <summary>
    /// 加载场景 - 异步加载 提供给外部使用的异步加载方法
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="callback"></param>
    public void LoadSceneAsync(string sceneName, UnityAction callback)
    {
        //开启协程异步加载场景
        MonoManager.Instance.StartCoroutine(LoadSceneAsyncCoroutine(sceneName, callback));
    }

    /// <summary>
    /// 加载场景 - 异步加载 协程实现异步加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator LoadSceneAsyncCoroutine(string sceneName, UnityAction callback)
    {
        //异步加载场景
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        //等待加载完成
        while (!ao.isDone)
        {
            // 事件中心向外广播加载进度 外部想用就监听这个事件
            EventCenter.Instance.EventTrigger("LoadingProgress", ao.progress);
            // 每一帧返回加载进度
            yield return ao.progress;
        }
        //加载完成后 执行回调
        callback?.Invoke();
    }
}
