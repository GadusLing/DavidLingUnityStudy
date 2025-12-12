using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 要对象池中的对象进行分类管理 每个分类抽屉都要有一个对应的父节点和其下所有对象的列表 所以用PoolData自定义类来组织它们
/// </summary>
public class PoolData
{
    public GameObject fatherObj; // 该类对应的父节点
    public List<GameObject> objList; // 其下所有对象的列表
    public GameObject objBuffer; // 该类对象的缓存 用于扩展池子时创建新对象 缓存下来就不用反复去Resources加载了 提升性能

    // 当前池子最大容量（工业常用：用于限制滥申请）
    public int maxCacheCount = 100;

    // 传入要管理的对象 和 缓存池根节点
    public PoolData(GameObject obj, GameObject poolRoot)
    {
        objBuffer = obj;// 缓存该类对象 用于扩展池子时创建新对象
        fatherObj = new GameObject(obj.name + "_Pool");// 创建该类对应的父节点XXX_Pool
        fatherObj.transform.SetParent(poolRoot.transform, false);// 设置该XXX_Pool的父节点为Pool 形成二级目录的结构 方便管理
        objList = new List<GameObject>();// 创建List——其下所有对象的列表
        ReturnObjToPool(obj);
    }

    /// <summary>
    /// 从抽屉里拿对象出来用
    /// </summary>
    public GameObject GetObjFromPool()
    {
        if (objList.Count == 0) return null; // 防御性检查

        int last = objList.Count - 1;
        GameObject obj = objList[last]; // 拿出该类对象列表里的最后一个对象
        objList.RemoveAt(last); // 从该类对象列表里移除这个对象 避免数组移动开销

        obj.SetActive(true); // 激活对象
        obj.transform.SetParent(null); // 脱离该类对应的父节点 方便外面使用

        return obj;
    }

    /// <summary>
    /// 当对象池为空时 用于一次性创建多个新对象并塞回池子 
    /// 工业级：支持自定义扩展数量，并限制最大数量防止内存爆炸
    /// </summary>
    public void ExpandPool(int count)
    {
        if (objBuffer == null || count <= 0) return;

        for (int i = 0; i < count; i++)
        {
            if (objList.Count >= maxCacheCount)
                break; // 限制最大缓存数量，防止对象池无限增长

            GameObject obj = GameObject.Instantiate(objBuffer);
            obj.name = objBuffer.name;

            ReturnObjToPool(obj);
        }
    }

    /// <summary>
    /// 把对象还到抽屉里 并且明确抽屉名 隐藏对象
    /// </summary>
    public void ReturnObjToPool(GameObject obj)
    {
        if (obj == null) return; // 工业级空引用防御

        obj.transform.SetParent(fatherObj.transform); // 把对象放到该类对应的父节点_Pool下面
        obj.SetActive(false); // 隐藏对象
        objList.Add(obj); // 把对象添加到该类对象列表中
    }

    /// <summary>
    /// 清空当前抽屉的所有缓存
    /// </summary>
    public void Clear()
    {
        for (int i = objList.Count - 1; i >= 0; i--)
        {
            if (objList[i] != null)
                GameObject.Destroy(objList[i]);
        }

        objList.Clear();

        if (fatherObj != null)
            GameObject.Destroy(fatherObj);
    }

}



/// <summary>
/// 缓存池管理器 继承SingletonBase<PoolManager> 构成单例模式
/// </summary>
public class PoolManager : SingletonBase<PoolManager>
{
    /// <summary>
    /// 缓存池字典-衣柜 key:对象的类型 特效-火光、弹痕、还是音效？等等 value: 对象列表
    /// </summary>
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

    /// <summary>
    /// 缓存池根节点-衣柜
    /// </summary>
    private GameObject poolRoot;

    /// <summary>
    /// 每次池子空了扩展的对象数量
    /// </summary>
    private const int ExpandCount = 5;

    /// <summary>
    /// 要用？ 异步从缓存池中获取对象
    /// </summary>
    public void GetObjFromPool(string key, UnityAction<GameObject> callback)
    {
        poolRoot ??= new GameObject("Pool"); // 懒加载创建根节点

        // 如果有这个抽屉
        if (poolDic.TryGetValue(key, out PoolData poolData))
        {
            // 如果有抽屉 但抽屉空了 Expand
            if (poolData.objList.Count == 0)
            {
                poolData.ExpandPool(ExpandCount);
                if (poolData.objList.Count == 0)
                {
                    Debug.LogError($"对象池 [{key}] 已达到最大容量，无法继续扩展！");
                    callback?.Invoke(null);
                    return;
                }
            }

            GameObject obj = poolData.GetObjFromPool();

            // 工业级：防止极端情况返回null
            if (obj == null)
            {
                Debug.LogWarning($"对象池 [{key}] 无法获取对象，正在紧急补充！");
                poolData.ExpandPool(1);
                obj = poolData.GetObjFromPool();
            }

            callback?.Invoke(obj);
        }
        else
        {
            // 用ResManager 异步加载
            ResManager.Instance.LoadResAsync<GameObject>(key, (obj) =>
            {
                obj.name = key;

                // 工业级优化：初始化池子时顺便预扩展
                PoolData newPool = new PoolData(obj, poolRoot);
                poolDic.Add(key, newPool);
                newPool.ExpandPool(ExpandCount);

                callback?.Invoke(newPool.GetObjFromPool());
            });
        }
    }

    /// <summary>
    /// 用完了 将对象返回到缓存池中
    /// </summary>
    public void ReturnObjToPool(string key, GameObject obj)
    {
        if (obj == null) return;

        poolRoot ??= new GameObject("Pool");

        if (!poolDic.ContainsKey(key))
        {
            poolDic.Add(key, new PoolData(obj, poolRoot));
        }

        poolDic[key].ReturnObjToPool(obj);
    }

    /// <summary>
    /// 清空缓存池 用于场景切换时
    /// </summary>
    public void ClearPool()
    {
        foreach (var poolData in poolDic.Values)
        {
            poolData.Clear(); // 清空每个抽屉
        }

        poolDic.Clear();

        if (poolRoot != null)
        {
            GameObject.Destroy(poolRoot);
            poolRoot = null;
        }
    }
}
