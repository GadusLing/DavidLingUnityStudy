using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

/// <summary>
/// PlayerPrefs数据管理器 - 基于反射的完整实现
/// 统一管理数据的存储和读取，支持基础类型、List、Dictionary、复杂自定义类型
/// 使用反射技术自动拆解和重组复杂数据结构
/// </summary>
public class PlayerPrefsDataMgr
{
    #region 单例模式和缓存
    /// <summary>
    /// 单例实例
    /// </summary>
    private static PlayerPrefsDataMgr _instance = new PlayerPrefsDataMgr();
    
    /// <summary>
    /// 反射字段信息缓存，避免重复反射调用提升性能
    /// </summary>
    private static Dictionary<Type, FieldInfo[]> _fieldCache = new Dictionary<Type, FieldInfo[]>();
    
    /// <summary>
    /// 支持的基础数据类型集合
    /// </summary>
    private static readonly Type[] BasicTypes = { typeof(int), typeof(float), typeof(string), typeof(bool) };
    
    /// <summary>
    /// 私有构造函数，确保单例模式
    /// </summary>
    private PlayerPrefsDataMgr() { }
    
    /// <summary>
    /// 获取单例实例
    /// </summary>
    public static PlayerPrefsDataMgr Instance => _instance;
    #endregion

    #region 公共API - 数据保存
    /// <summary>
    /// 保存数据到PlayerPrefs
    /// 自动识别数据类型并选择合适的存储方式
    /// </summary>
    /// <param name="keyName">存储键名</param>
    /// <param name="data">要保存的数据对象</param>
    public void SaveData(string keyName, object data)
    {
        // 获取数据的运行时类型
        Type dataType = data.GetType();
        
        // 判断是基础类型还是复杂类型
        if (IsBasicType(dataType))
        {
            // 基础类型直接保存
            SaveBasicType(keyName, data, dataType);
        }
        else
        {
            // 复杂类型通过反射拆解保存
            SaveComplexData(keyName, data);
        }

        // 立即写入磁盘
        PlayerPrefs.Save();
    }
    #endregion

    #region 基础类型处理
    /// <summary>
    /// 判断是否为支持的基础数据类型
    /// </summary>
    /// <param name="type">要检查的类型</param>
    /// <returns>是否为基础类型</returns>
    private bool IsBasicType(Type type)
    {
        return System.Array.Exists(BasicTypes, t => t == type);
    }

    /// <summary>
    /// 保存基础类型数据
    /// 根据类型调用对应的PlayerPrefs方法
    /// </summary>
    /// <param name="keyName">存储键名</param>
    /// <param name="data">数据对象</param>
    /// <param name="dataType">数据类型</param>
    private void SaveBasicType(string keyName, object data, Type dataType)
    {
        if (dataType == typeof(int))
            PlayerPrefs.SetInt(keyName, (int)data);
        else if (dataType == typeof(float))
            PlayerPrefs.SetFloat(keyName, (float)data);
        else if (dataType == typeof(string))
            PlayerPrefs.SetString(keyName, (string)data);
        else if (dataType == typeof(bool))
            PlayerPrefs.SetInt(keyName, (bool)data ? 1 : 0); // bool转换为int存储
    }
    #endregion

    #region 复杂类型处理
    /// <summary>
    /// 保存复杂类型数据
    /// 通过反射识别类型并选择对应的处理方式
    /// </summary>
    /// <param name="keyName">存储键名</param>
    /// <param name="data">复杂数据对象</param>
    private void SaveComplexData(string keyName, object data)
    {
        // 为什么要用 IList、IDictionary？因为反射时你拿到的是一个 object，你并不知道它的确切类型。
        // 如果你写死成 List<T>，就只能处理这种情况。但如果你用 IList，那么 所有实现了 IList 的集合类 都能被处理
        // （比如 List<T>，ArrayList，甚至你自己写的自定义集合）。
        // 同理，Dictionary<TKey, TValue> 实现了 IDictionary，所以你只要判断是不是 IDictionary，就能处理所有字典类型。
        
        // 检查是否为List泛型集合
        if (IsGenericCollection(data, typeof(IList)))
        {
            SaveListData(keyName, (IList)data);
            return;
        }
        
        // 检查是否为Dictionary泛型集合
        if (IsGenericCollection(data, typeof(IDictionary)))
        {
            SaveDictionaryData(keyName, (IDictionary)data);
            return;
        }

        // 普通复杂类型：通过反射拆解字段
        Type type = data.GetType();
        FieldInfo[] fields = GetFieldsWithCache(type);

        // 遍历所有字段，递归保存每个字段的值
        foreach (FieldInfo field in fields)
        {
            object fieldValue = field.GetValue(data); // 通过反射获取字段值
            string fieldKey = GenerateFieldKey(keyName, field.Name); // 生成字段的存储键名
            SaveData(fieldKey, fieldValue); // 递归调用，保存字段值
        }

        // 保存类型的元数据信息，用于后续读取时重建对象
        SaveTypeMetadata(keyName, type, fields);
    }

    /// <summary>
    /// 判断对象是否为指定类型的泛型集合
    /// </summary>
    /// <param name="data">要检查的数据对象</param>
    /// <param name="collectionType">集合基类型（如IList, IDictionary）</param>
    /// <returns>是否为指定类型的泛型集合</returns>
    private bool IsGenericCollection(object data, Type collectionType)
    {
        Type dataType = data.GetType();
        return collectionType.IsAssignableFrom(dataType) && dataType.IsGenericType;
    }
    #endregion

    #region List集合处理
    /// <summary>
    /// 保存List集合数据
    /// 将List拆解为元素个数、元素类型和各个元素分别存储
    /// </summary>
    /// <param name="keyName">存储键名</param>
    /// <param name="list">List集合对象</param>
    private void SaveListData(string keyName, IList list)
    {
        // 保存集合元素个数
        PlayerPrefs.SetInt(GenerateCountKey(keyName), list.Count);
        
        // 通过反射获取List的泛型参数（元素类型）
        Type[] genericArgs = GetGenericArguments(list.GetType());
        PlayerPrefs.SetString(GenerateElementTypeKey(keyName), genericArgs[0].AssemblyQualifiedName);
        
        // 逐个保存List中的每个元素
        for (int i = 0; i < list.Count; i++)
        {
            string elementKey = GenerateElementKey(keyName, i);
            SaveData(elementKey, list[i]); // 递归保存元素
        }
    }
    #endregion

    #region Dictionary字典处理
    /// <summary>
    /// 保存Dictionary字典数据
    /// 将Dictionary拆解为元素个数、键值类型和各个键值对分别存储
    /// </summary>
    /// <param name="keyName">存储键名</param>
    /// <param name="dict">Dictionary字典对象</param>
    private void SaveDictionaryData(string keyName, IDictionary dict)
    {
        // 保存字典元素个数
        PlayerPrefs.SetInt(GenerateCountKey(keyName), dict.Count);
        
        // 通过反射获取Dictionary的泛型参数（键类型和值类型）
        Type[] genericArgs = GetGenericArguments(dict.GetType());
        PlayerPrefs.SetString(GenerateKeyTypeKey(keyName), genericArgs[0].AssemblyQualifiedName);   // 键类型
        PlayerPrefs.SetString(GenerateValueTypeKey(keyName), genericArgs[1].AssemblyQualifiedName); // 值类型
        
        // 逐个保存Dictionary中的每个键值对
        int index = 0;
        foreach (DictionaryEntry kvp in dict)
        {
            SaveData(GenerateKeyKey(keyName, index), kvp.Key);     // 递归保存键
            SaveData(GenerateValueKey(keyName, index), kvp.Value); // 递归保存值
            index++;
        }
    }
    #endregion

    #region 类型元数据处理
    /// <summary>
    /// 保存类型的元数据信息
    /// 包括字段名列表和完整类型名，用于读取时重建对象
    /// </summary>
    /// <param name="keyName">存储键名</param>
    /// <param name="type">对象类型</param>
    /// <param name="fields">字段信息数组</param>
    private void SaveTypeMetadata(string keyName, Type type, FieldInfo[] fields)
    {
        // 将字段名数组转换为逗号分隔的字符串保存
        string fieldNames = string.Join(",", System.Array.ConvertAll(fields, f => f.Name));
        PlayerPrefs.SetString(GenerateFieldNamesKey(keyName), fieldNames);
        
        // 保存完整的类型名（包含程序集信息）
        PlayerPrefs.SetString(GenerateTypeNameKey(keyName), type.AssemblyQualifiedName);
    }
    #endregion

    #region 公共API - 数据读取
    /// <summary>
    /// 从PlayerPrefs加载数据
    /// 根据指定类型自动选择合适的读取方式
    /// </summary>
    /// <param name="keyName">存储键名</param>
    /// <param name="dataType">期望的数据类型</param>
    /// <returns>读取到的数据对象</returns>
    public object LoadData(string keyName, Type dataType)
    {
        if (IsBasicType(dataType))
        {
            return LoadBasicType(keyName, dataType);
        }
        else
        {
            return LoadComplexData(keyName, dataType);
        }
    }

    /// <summary>
    /// 泛型方式获取数据
    /// 提供类型安全的数据获取接口
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="keyName">存储键名</param>
    /// <param name="defaultValue">默认值（当数据不存在时返回）</param>
    /// <returns>获取到的数据</returns>
    public T GetData<T>(string keyName, T defaultValue)
    {
        Type type = typeof(T);
        
        if (IsBasicType(type))
        {
            return GetBasicData<T>(keyName, defaultValue);
        }
        else
        {
            // 检查复杂类型数据是否存在
            if (!ValidateTypeData(keyName) && !PlayerPrefs.HasKey(GenerateCountKey(keyName)))
            {
                return defaultValue;
            }
            object result = LoadData(keyName, type);
            return result != null ? (T)result : defaultValue;
        }
    }
    #endregion

    #region 基础类型读取
    /// <summary>
    /// 读取基础类型数据
    /// </summary>
    /// <param name="keyName">存储键名</param>
    /// <param name="dataType">数据类型</param>
    /// <returns>读取到的基础类型数据</returns>
    private object LoadBasicType(string keyName, Type dataType)
    {
        if (dataType == typeof(int))
            return PlayerPrefs.GetInt(keyName);
        else if (dataType == typeof(float))
            return PlayerPrefs.GetFloat(keyName);
        else if (dataType == typeof(string))
            return PlayerPrefs.GetString(keyName);
        else if (dataType == typeof(bool))
            return PlayerPrefs.GetInt(keyName) == 1; // int转换回bool
        
        return null;
    }

    /// <summary>
    /// 泛型方式获取基础类型数据
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="keyName">存储键名</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns>获取到的数据</returns>
    private T GetBasicData<T>(string keyName, T defaultValue)
    {
        Type type = typeof(T);
        
        if (type == typeof(int))
        {
            int value = PlayerPrefs.GetInt(keyName, (int)(object)defaultValue);
            return (T)(object)value;
        }
        else if (type == typeof(float))
        {
            float value = PlayerPrefs.GetFloat(keyName, (float)(object)defaultValue);
            return (T)(object)value;
        }
        else if (type == typeof(string))
        {
            string value = PlayerPrefs.GetString(keyName, (string)(object)defaultValue);
            return (T)(object)value;
        }
        else if (type == typeof(bool))
        {
            int value = PlayerPrefs.GetInt(keyName, (bool)(object)defaultValue ? 1 : 0);
            return (T)(object)(value == 1);
        }
        
        return defaultValue;
    }
    #endregion

    #region 复杂类型读取
    /// <summary>
    /// 读取复杂类型数据
    /// 通过反射重建对象结构
    /// </summary>
    /// <param name="keyName">存储键名</param>
    /// <param name="dataType">期望的数据类型</param>
    /// <returns>重建的复杂对象</returns>
    private object LoadComplexData(string keyName, Type dataType)
    {
        // 检查是否为List类型
        if (typeof(IList).IsAssignableFrom(dataType) && dataType.IsGenericType)
        {
            return LoadListData(keyName, dataType);
        }
        
        // 检查是否为Dictionary类型
        if (typeof(IDictionary).IsAssignableFrom(dataType) && dataType.IsGenericType)
        {
            return LoadDictionaryData(keyName, dataType);
        }

        // 普通复杂类型处理
        if (!ValidateTypeData(keyName))
        {
            Debug.LogError($"数据管理器错误：未找到复杂类型的字段信息，键名：{keyName}");
            return null;
        }

        // 通过反射创建对象实例
        object instance = Activator.CreateInstance(dataType);
        
        // 获取保存的字段名列表
        string fieldNames = PlayerPrefs.GetString(GenerateFieldNamesKey(keyName));
        string[] fieldNameArray = fieldNames.Split(',');
        
        // 获取类型的字段信息
        FieldInfo[] fields = GetFieldsWithCache(dataType);

        // 逐个恢复字段值
        foreach (string fieldName in fieldNameArray)
        {
            FieldInfo field = System.Array.Find(fields, f => f.Name == fieldName);
            if (field != null)
            {
                string fieldKey = GenerateFieldKey(keyName, fieldName);
                object fieldValue = LoadData(fieldKey, field.FieldType); // 递归读取字段值
                field.SetValue(instance, fieldValue); // 通过反射设置字段值
            }
        }

        return instance;
    }
    #endregion

    #region List集合读取
    /// <summary>
    /// 读取List集合数据
    /// 根据保存的元素类型和个数重建List
    /// </summary>
    /// <param name="keyName">存储键名</param>
    /// <param name="listType">List类型</param>
    /// <returns>重建的List对象</returns>
    private object LoadListData(string keyName, Type listType)
    {
        if (!PlayerPrefs.HasKey(GenerateCountKey(keyName)))
        {
            Debug.LogError($"数据管理器错误：未找到列表信息，键名：{keyName}");
            return null;
        }

        // 获取List元素个数
        int count = PlayerPrefs.GetInt(GenerateCountKey(keyName));
        
        // 获取元素类型信息并通过反射创建Type对象
        string elementTypeName = PlayerPrefs.GetString(GenerateElementTypeKey(keyName));
        Type elementType = Type.GetType(elementTypeName);
        
        // 创建List实例
        IList list = (IList)Activator.CreateInstance(listType);
        
        // 逐个读取并添加元素到List中
        for (int i = 0; i < count; i++)
        {
            string elementKey = GenerateElementKey(keyName, i);
            object element = LoadData(elementKey, elementType); // 递归读取元素
            list.Add(element);
        }
        
        return list;
    }
    #endregion

    #region Dictionary字典读取
    /// <summary>
    /// 读取Dictionary字典数据
    /// 根据保存的键值类型和个数重建Dictionary
    /// </summary>
    /// <param name="keyName">存储键名</param>
    /// <param name="dictType">Dictionary类型</param>
    /// <returns>重建的Dictionary对象</returns>
    private object LoadDictionaryData(string keyName, Type dictType)
    {
        if (!PlayerPrefs.HasKey(GenerateCountKey(keyName)))
        {
            Debug.LogError($"数据管理器错误：未找到字典信息，键名：{keyName}");
            return null;
        }

        // 获取字典元素个数
        int count = PlayerPrefs.GetInt(GenerateCountKey(keyName));
        
        // 获取键值类型信息并通过反射创建Type对象
        string keyTypeName = PlayerPrefs.GetString(GenerateKeyTypeKey(keyName));
        string valueTypeName = PlayerPrefs.GetString(GenerateValueTypeKey(keyName));
        Type keyType = Type.GetType(keyTypeName);
        Type valueType = Type.GetType(valueTypeName);
        
        // 创建Dictionary实例
        IDictionary dict = (IDictionary)Activator.CreateInstance(dictType);
        
        // 逐个读取并添加键值对到Dictionary中
        for (int i = 0; i < count; i++)
        {
            object key = LoadData(GenerateKeyKey(keyName, i), keyType);     // 递归读取键
            object value = LoadData(GenerateValueKey(keyName, i), valueType); // 递归读取值
            dict.Add(key, value);
        }
        
        return dict;
    }
    #endregion

    #region 反射辅助方法
    /// <summary>
    /// 带缓存的字段信息获取
    /// 避免重复反射调用，提升性能
    /// </summary>
    /// <param name="type">要获取字段的类型</param>
    /// <returns>字段信息数组</returns>
    private FieldInfo[] GetFieldsWithCache(Type type)
    {
        if (!_fieldCache.ContainsKey(type))
        {
            // 通过反射获取类型的所有公共字段
            _fieldCache[type] = type.GetFields();
        }
        return _fieldCache[type];
    }

    /// <summary>
    /// 获取泛型类型的参数类型数组
    /// </summary>
    /// <param name="type">泛型类型</param>
    /// <returns>泛型参数类型数组</returns>
    private Type[] GetGenericArguments(Type type)
    {
        return type.IsGenericType ? type.GetGenericArguments() : null;
    }

    /// <summary>
    /// 验证复杂类型数据是否存在
    /// </summary>
    /// <param name="keyName">存储键名</param>
    /// <returns>数据是否存在</returns>
    private bool ValidateTypeData(string keyName)
    {
        return PlayerPrefs.HasKey(GenerateFieldNamesKey(keyName));
    }
    #endregion

    #region 存储键名生成方法
    /// <summary>
    /// 生成字段存储键名
    /// </summary>
    /// <param name="baseKey">基础键名</param>
    /// <param name="fieldName">字段名</param>
    /// <returns>字段存储键名</returns>
    private string GenerateFieldKey(string baseKey, string fieldName) => $"{baseKey}_{fieldName}";
    
    /// <summary>
    /// 生成List元素存储键名
    /// </summary>
    /// <param name="baseKey">基础键名</param>
    /// <param name="index">元素索引</param>
    /// <returns>元素存储键名</returns>
    private string GenerateElementKey(string baseKey, int index) => $"{baseKey}_Element_{index}";
    
    /// <summary>
    /// 生成Dictionary键存储键名
    /// </summary>
    /// <param name="baseKey">基础键名</param>
    /// <param name="index">键值对索引</param>
    /// <returns>键存储键名</returns>
    private string GenerateKeyKey(string baseKey, int index) => $"{baseKey}_Key_{index}";
    
    /// <summary>
    /// 生成Dictionary值存储键名
    /// </summary>
    /// <param name="baseKey">基础键名</param>
    /// <param name="index">键值对索引</param>
    /// <returns>值存储键名</returns>
    private string GenerateValueKey(string baseKey, int index) => $"{baseKey}_Value_{index}";
    
    /// <summary>
    /// 生成集合元素个数存储键名
    /// </summary>
    /// <param name="baseKey">基础键名</param>
    /// <returns>元素个数存储键名</returns>
    private string GenerateCountKey(string baseKey) => $"{baseKey}_Count";
    
    /// <summary>
    /// 生成List元素类型存储键名
    /// </summary>
    /// <param name="baseKey">基础键名</param>
    /// <returns>元素类型存储键名</returns>
    private string GenerateElementTypeKey(string baseKey) => $"{baseKey}_ElementType";
    
    /// <summary>
    /// 生成Dictionary键类型存储键名
    /// </summary>
    /// <param name="baseKey">基础键名</param>
    /// <returns>键类型存储键名</returns>
    private string GenerateKeyTypeKey(string baseKey) => $"{baseKey}_KeyType";
    
    /// <summary>
    /// 生成Dictionary值类型存储键名
    /// </summary>
    /// <param name="baseKey">基础键名</param>
    /// <returns>值类型存储键名</returns>
    private string GenerateValueTypeKey(string baseKey) => $"{baseKey}_ValueType";
    
    /// <summary>
    /// 生成字段名列表存储键名
    /// </summary>
    /// <param name="baseKey">基础键名</param>
    /// <returns>字段名列表存储键名</returns>
    private string GenerateFieldNamesKey(string baseKey) => $"{baseKey}_FieldNames";
    
    /// <summary>
    /// 生成类型名存储键名
    /// </summary>
    /// <param name="baseKey">基础键名</param>
    /// <returns>类型名存储键名</returns>
    private string GenerateTypeNameKey(string baseKey) => $"{baseKey}_TypeName";
    #endregion
}