using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// 专门管理游戏数据的类
/// </summary>
public class GameDataMgr
{
    private static GameDataMgr _instance = new GameDataMgr();
    public static GameDataMgr Instance => _instance;
    /// <summary>
    /// 记录选择的角色数据 用于之后在游戏场景中生成角色
    /// </summary>
    public RoleInfo curChooseRole;
    /// <summary>
    /// 音乐音效数据（本地）
    /// </summary>
    public MusicData musicData => GetGameData<MusicData>();
    /// <summary>
    /// 玩家数据（本地）
    /// </summary>
    public PlayerData playerData => GetGameData<PlayerData>();
    /// <summary>
    /// 角色配置信息列表（配置）
    /// </summary>
    public List<RoleInfo> roleInfoList => GetGameData<List<RoleInfo>>();
    /// <summary>
    /// 场景配置信息列表（配置）
    /// </summary>
    public List<SceneInfo> sceneInfoList => GetGameData<List<SceneInfo>>();
    /// <summary>
    /// 敌人配置信息列表（配置）
    /// </summary>
    public List<EnemyInfo> enemyInfoList => GetGameData<List<EnemyInfo>>();
    /// <summary>
    /// 塔防配置信息列表（配置）
    /// </summary>
    public List<TowerInfo> towerInfoList => GetGameData<List<TowerInfo>>();


    // 缓存所有类型的数据实例
    private Dictionary<Type, object> _dataDic = new Dictionary<Type, object>();
    
    /// <summary>
    /// 根据类型获取对应的文件名
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private string GetFileNameForType(Type type)
    {
        // 判断这个 Type 是否是泛型类型（例如 List<RoleInfo>、Dictionary<string,int> 都会是真）
        // type.GetGenericTypeDefinition() == typeof(List<>)当为泛型类型时，GetGenericTypeDefinition() 返回“开泛型定义”（比如 List<>、Dictionary<,>）
        // 把它和 typeof(List<>) 比较，等于在判断“这个泛型是不是一个 List<T>”
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            return type.GetGenericArguments()[0].Name;
            // 如果确认是 List<T>，GetGenericArguments() 返回类型参数数组（List<T> 就是 T），取第 0 个就是具体的 T（例如 RoleInfo）
        return type.Name;// 再用 .Name 得到类型名字符串（"RoleInfo"）
    }
    
    private GameDataMgr()
    {
        // 原来手动加载的逻辑现在由 GetData 负责，构造里可以不再显式赋值
        // musicData = LoadGameData<MusicData>();
    }

    /// <summary>
    /// 通用保存（传入要保存的对象）
    /// </summary>
    public void SaveGameData<T>(T data)
    {
        string fileName = GetFileNameForType(typeof(T));
        JsonMgr.Instance.SaveData(data, fileName);
        _dataDic[typeof(T)] = data;
    }

    /// <summary>
    /// 无参保存：保存缓存中的该类型数据（如果没有则先从文件加载）
    /// </summary>
    public void SaveGameData<T>() where T : class, new()
    {
        T data = GetGameData<T>();
        SaveGameData<T>(data);
    }

    /// <summary>
    /// 获取缓存中的数据，如果没有则从文件加载并缓存，始终返回非 null（会 new 一个默认实例）
    /// </summary>
    public T GetGameData<T>() where T : class, new()
    {
        var type = typeof(T);
        if (_dataDic.TryGetValue(type, out var obj) && obj is T existing)
            return existing;

        // 从文件加载（JsonMgr 可能返回 null），若为 null 则 new 一个默认实例
        string fileName = GetFileNameForType(type);
        T data = JsonMgr.Instance.LoadData<T>(fileName);
        if (data == null)
            data = new T();

        _dataDic[type] = data;
        return data;
    }

    /// <summary>
    /// 显式从文件加载并返回（同时更新缓存）
    /// </summary>
    public T LoadGameData<T>() where T : class, new()
    {
        string fileName = GetFileNameForType(typeof(T));
        T data = JsonMgr.Instance.LoadData<T>(fileName);
        if (data == null)
            data = new T();
        _dataDic[typeof(T)] = data;
        return data;
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="soundResName">直接传音效名称 不用写详细路径 在函数内部处理了</param>
    public void PlaySound(string soundResName)
    {
        GameObject musicObj = new GameObject();
        AudioSource audioSource = musicObj.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>($"Audio/Sounds/{soundResName}");
        audioSource.volume = musicData.soundVolume;
        audioSource.mute = !musicData.soundOpen;
        audioSource.Play();
        GameObject.Destroy(musicObj, audioSource.clip.length);

    }
    
}
