using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// AB包管理器
/// </summary>
public class ABManager : SingletonMonoAuto<ABManager>
{
    /// <summary>
    /// 构造函数 设置为 private，防止外部直接实例化 继承了 SingletonBase 后必须手动写一个私有的无参构造函数 SingletonBase 中会通过反射调用它
    /// </summary>
    private ABManager() { }

    private AssetBundle mainAB = null;// 主包对象 主包中存储了包与包之间的依赖关系 加载包时都需要加载依赖 主包会贯穿加载过程的始终 所以用一个变量来单独存储 防止重复加载

    private AssetBundleManifest manifest = null;// 主包清单对象 通过它可以获取所有包的依赖关系 也是只用加载一次 但贯彻始终 用变量单独存起来

    /// <summary>
    /// AB包不可重复加载 使用字典来存储已经加载的AB包 key为包名 value为包对象
    /// </summary>
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

    private string PathUrl // 不同平台下 AB包的存放路径有不同 有可能放在 streamingAssetsPath 也有可能放在 persistentDataPath
    {
        get
        {
            return Application.streamingAssetsPath + "/";// 根据平台不同 可以快速修改路径
        }
    }

    private string MainABName // 主包名称 不同平台主包名称不同 用平台编译宏来判断平台 设置对应的主包名称 之后有其他平台再继续添加
    {
        get
        {
            #if UNITY_IOS
                return "IOS";
            #elif UNITY_ANDROID
                return "Android";
            #else
                return "PC";
            #endif
        }
    }
    // 同步加载包的方法
    public void LoadAB(string abName)// 加载主包 依赖包 指定包
    {
        if (mainAB == null)// 如果主包还没有加载 就先加载主包和主包清单对象
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainABName);// 加载主包
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");// 加载主包清单对象
        }

        string[] dependABs = manifest.GetAllDependencies(abName);// 通过主包清单对象 获取指定包的所有依赖包名称
        foreach (string dependAB in dependABs)// 遍历所有依赖包名称
        {
            if (!abDic.ContainsKey(dependAB))// 如果字典中还没有加载该依赖包
            {
                AssetBundle dependABObj = AssetBundle.LoadFromFile(PathUrl + dependAB);// 就去加载该依赖包
                abDic.Add(dependAB, dependABObj);// 加载完成后 存入字典
            }
        }
        if (!abDic.ContainsKey(abName))// 最后再加载指定资源包本身
        {
            AssetBundle abObj = AssetBundle.LoadFromFile(PathUrl + abName);
            abDic.Add(abName, abObj);
        }
    }

    public Object LoadRes(string abName, string resName)// 同步加载 不指定类型 慎用
    {
        LoadAB(abName);// 复用加载包的方法 来确保包和依赖包都被加载了

        Object obj = abDic[abName].LoadAsset(resName);// 加载指定资源 不过没有类型区分 会导致同名但不同类型的资源加载错误 慎用
        if(obj is GameObject)
        {
            return GameObject.Instantiate(obj);// 这里做一个优化 如果是游戏对象 我们通常都需要实例化使用 在此处实例化 这样在业务代码中用一个GameObject接收 将返回过去的object as GameObject 就能直接用了
        }
        else
        {
            return obj;// 否则直接返回资源对象
        }
    }

    public Object LoadRes(string abName, string resName, System.Type type)// 同步加载 重载一个带类型参数的加载方法
    {
        LoadAB(abName);// 复用加载包的方法 来确保包和依赖包都被加载了

        Object obj = abDic[abName].LoadAsset(resName, type);// 加载指定资源 带类型参数 因为Lua不支持泛型 所以之后用Lua热更 会常用这种方式 外部需要typeof + as
        if (obj is GameObject)
        {
            return GameObject.Instantiate(obj);// 这里做一个优化 如果是游戏对象 我们通常都需要实例化使用 在此处实例化 这样在业务代码中用一个GameObject接收 将返回过去的object as GameObject 就能直接用了
        }
        else
        {
            return obj;// 否则直接返回资源对象
        }
    }

    public T LoadRes<T>(string abName, string resName) where T : Object// 同步加载 重载泛型方式
    {
        LoadAB(abName);// 复用加载包的方法 来确保包和依赖包都被加载了

        T obj = abDic[abName].LoadAsset<T>(resName);// 加载指定资源 带泛型参数
        if (obj is GameObject)
        {
            return GameObject.Instantiate(obj) as T;// 这里做一个优化 如果是游戏对象 我们通常都需要实例化使用 在此处实例化 这样在业务代码中用一个GameObject接收 将返回过去的object as GameObject 就能直接用了
        }
        else
        {
            return obj;// 否则直接返回资源对象
        }
    }

    // 异步加载包的方法
    // 注意 这里的异步加载 不是指AB包的异步加载 而是指加载AB包内的资源时使用异步加载
    public void LoadResAsync(string abName, string resName, UnityAction<Object> callback)// 异步加载 不指定类型 慎用
    {
        StartCoroutine(LoadResAsyncCoroutine(abName, resName, callback));// 启动协程来异步加载资源
    }
    private IEnumerator LoadResAsyncCoroutine(string abName, string resName, UnityAction<Object> callback)// 真正的异步加载资源的协程方法
    {
        LoadAB(abName);// 复用加载包的方法 来确保包和依赖包都被加载了
        AssetBundleRequest request = abDic[abName].LoadAssetAsync(resName);// 异步加载 AB 包里的资源 Unity 内部的异步加载（底层多线程）
        yield return request;// 协程停在这里 等待request加载完成 才继续往下走

        Object obj = request.asset;// 获取加载完成的资源对象
        if (obj is GameObject)
        {
            callback(GameObject.Instantiate(obj));// 这里做一个优化 如果是游戏对象 我们通常都需要实例化使用 在此处实例化 这样在业务代码中用一个GameObject接收 将返回过去的object as GameObject 就能直接用了
        }
        else
        {
            callback(obj);// 否则直接通过回调返回资源对象
        }
    }

    public void LoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callback)// 异步加载 重载一个带类型参数的加载方法
    {
        StartCoroutine(LoadResAsyncCoroutine(abName, resName, type, callback));// 启动协程来异步加载资源
    }
    private IEnumerator LoadResAsyncCoroutine(string abName, string resName, System.Type type, UnityAction<Object> callback)// 真正的异步加载资源的协程方法
    {
        LoadAB(abName);// 复用加载包的方法 来确保包和依赖包都被加载了
        AssetBundleRequest request = abDic[abName].LoadAssetAsync(resName, type);// 异步加载 AB 包里的资源 Unity 内部的异步加载（底层多线程）
        yield return request;// 协程停在这里 等待request加载完成 才继续往下走

        Object obj = request.asset;// 获取加载完成的资源对象
        if (obj is GameObject)
        {
            callback(GameObject.Instantiate(obj));// 这里做一个优化 如果是游戏对象 我们通常都需要实例化使用 在此处实例化 这样在业务代码中用一个GameObject接收 将返回过去的object as GameObject 就能直接用了
        }
        else
        {
            callback(obj);// 否则直接通过回调返回资源对象
        }
    }

    public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callback) where T : Object// 异步加载  重载泛型方式
    {
        StartCoroutine(LoadResAsyncCoroutine<T>(abName, resName, callback));// 启动协程来异步加载资源
    }
    private IEnumerator LoadResAsyncCoroutine<T>(string abName, string resName, UnityAction<T> callback) where T : Object// 真正的异步加载资源的协程方法
    {
        LoadAB(abName);// 复用加载包的方法 来确保包和依赖包都被加载了
        AssetBundleRequest request = abDic[abName].LoadAssetAsync<T>(resName);// 异步加载 AB 包里的资源 Unity 内部的异步加载（底层多线程）
        yield return request;// 协程停在这里 等待request加载完成 才继续往下走

        Object obj = request.asset;// 获取加载完成的资源对象
        if (obj is GameObject)
        {
            callback(GameObject.Instantiate(obj) as T);// 这里做一个优化 如果是游戏对象 我们通常都需要实例化使用 在此处实例化 这样在业务代码中用一个GameObject接收 将返回过去的object as GameObject 就能直接用了
        }
        else
        {
            callback(obj as T);// 否则直接通过回调返回资源对象
        }
    }


    public void UnloadAB(string abName)// 单个包卸载
    {
        if (abDic.ContainsKey(abName))// 如果字典中有该包
        {
            abDic[abName].Unload(false);// 卸载该包 true表示同时卸载包中所有资源 false表示只卸载包对象 资源留在内存中 避免场景上的资源被误卸载
            abDic.Remove(abName);// 从字典中移除该包记录
        }
    }

    public void ClearAB()// 卸载所有包
    {
        AssetBundle.UnloadAllAssetBundles(false);// 卸载所有包 true表示同时卸载包中所有资源 false表示只卸载包对象 资源留在内存中 避免场景上的资源被误卸载
        abDic.Clear();// 清空字典
        mainAB = null;// 主包对象置空
        manifest = null;// 主包清单对象置空
    }
}