using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourcesMgr
{
    // 请写一个简单的资源管理器，提供统一的方法给外部用于资源异步加载外部可以传入委托用于当资源加载结束时使用资源
    private static ResourcesMgr _instance = new ResourcesMgr();
    public static ResourcesMgr Instance => _instance;

    private ResourcesMgr(){}

    public void LoadRes<T>(string name, UnityAction<T> callback) where T : Object
    {
        Resources.LoadAsync<T>(name).completed += (AsyncOperation op) =>
        {
            ResourceRequest request = op as ResourceRequest;
            if(request != null)
            {
                T res = request.asset as T;
                callback?.Invoke(res);
            }
        };
    }
}
 