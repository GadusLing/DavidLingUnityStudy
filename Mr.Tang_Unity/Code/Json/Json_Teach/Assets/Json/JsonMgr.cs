using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

// LitJson和JsonUtility相比会有一些差异，比如在私有字段的处理上，LitJson不能序列化私有字段，而JsonUtility可以通过[SerializeField]属性来序列化私有字段
// 所以这里提供两种Json处理方式，方便根据需求选择使用
public enum JsonType
{
    LitJson,
    JsonUtility
}

public class JsonMgr
{
    private static JsonMgr _instance = new JsonMgr();
    public static JsonMgr Instance => _instance;

    private JsonMgr(){}

    public void SaveData(object data, string fileName, JsonType type = JsonType.LitJson)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        string jsonStr = "";
        switch (type)
        {
            case JsonType.LitJson:
                jsonStr = JsonMapper.ToJson(data);
                break;
            case JsonType.JsonUtility:
                jsonStr = JsonUtility.ToJson(data);
                break;
        }
        System.IO.File.WriteAllText(path, jsonStr);
    }

    // where T : new() 是 C# 泛型约束的一种写法，意思是：类型参数 T 必须有一个无参的公共构造函数
    public T LoadData<T>(string fileName, JsonType type = JsonType.LitJson) where T : new()
    {
        // 先要确定StreamingAssets默认数据文件夹中是否有想要的数据
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";
        if (!System.IO.File.Exists(path))// 如果没有这个文件
        {
            // 就去持久化数据文件夹中找
            path = Application.persistentDataPath + "/" + fileName + ".json";
            if (!System.IO.File.Exists(path)) // 如果还是没有这个文件
            {
                // 就返回 T 类型的默认实例
                return new T();// 泛型约束后在方法里可以安全地用 new T() 创建 T 类型的新实例
            }
        }
        string jsonStr = System.IO.File.ReadAllText(path);
        T data = default(T);
        switch (type)
        {
            case JsonType.LitJson:
                data = JsonMapper.ToObject<T>(jsonStr);
                break;
            case JsonType.JsonUtility:
                data = JsonUtility.FromJson<T>(jsonStr);
                break;
        }
        return data;
    }
}
