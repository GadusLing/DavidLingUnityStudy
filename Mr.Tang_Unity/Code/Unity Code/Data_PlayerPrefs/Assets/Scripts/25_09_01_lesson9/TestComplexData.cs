using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestComplexData : MonoBehaviour
{
    void Start()
    {
        TestBasicTypes();
        TestPersonClass();
        TestListTypes();
        TestDictionaryTypes();
        TestComplexCombination();
    }

    // 测试基础类型
    void TestBasicTypes()
    {
        Debug.Log("=== 测试基础类型 ===");
        
        PlayerPrefsDataMgr.Instance.SaveData("TestInt", 100);
        PlayerPrefsDataMgr.Instance.SaveData("TestFloat", 3.14f);
        PlayerPrefsDataMgr.Instance.SaveData("TestString", "Hello World");
        PlayerPrefsDataMgr.Instance.SaveData("TestBool", true);
        
        int intValue = PlayerPrefsDataMgr.Instance.GetData("TestInt", 0);
        float floatValue = PlayerPrefsDataMgr.Instance.GetData("TestFloat", 0f);
        string stringValue = PlayerPrefsDataMgr.Instance.GetData("TestString", "");
        bool boolValue = PlayerPrefsDataMgr.Instance.GetData("TestBool", false);
        
        Debug.Log($"Int: {intValue}, Float: {floatValue}, String: {stringValue}, Bool: {boolValue}");
    }

    // 测试Person复杂类型
    void TestPersonClass()
    {
        Debug.Log("=== 测试Person复杂类型 ===");
        
        Person person = new Person("张三", 25, true, 175.5f);
        person.hobbies.Add("编程"); // 添加一个新爱好
        
        Debug.Log("保存前的数据：" + person.ToString());
        
        PlayerPrefsDataMgr.Instance.SaveData("PlayerData", person);
        
        Person loadedPerson = PlayerPrefsDataMgr.Instance.GetData<Person>("PlayerData", new Person());
        
        Debug.Log("加载后的数据：" + loadedPerson.ToString());
    }

    // 测试List类型
    void TestListTypes()
    {
        Debug.Log("=== 测试List类型 ===");
        
        List<int> intList = new List<int> { 1, 2, 3, 4, 5 };
        List<string> stringList = new List<string> { "苹果", "香蕉", "橙子" };
        
        PlayerPrefsDataMgr.Instance.SaveData("IntList", intList);
        PlayerPrefsDataMgr.Instance.SaveData("StringList", stringList);
        
        List<int> loadedIntList = PlayerPrefsDataMgr.Instance.GetData("IntList", new List<int>());
        List<string> loadedStringList = PlayerPrefsDataMgr.Instance.GetData("StringList", new List<string>());
        
        Debug.Log($"IntList: [{string.Join(", ", loadedIntList)}]");
        Debug.Log($"StringList: [{string.Join(", ", loadedStringList)}]");
    }

    // 测试Dictionary类型
    void TestDictionaryTypes()
    {
        Debug.Log("=== 测试Dictionary类型 ===");
        
        Dictionary<string, int> scoreDict = new Dictionary<string, int>
        {
            {"数学", 95},
            {"英语", 87},
            {"语文", 92}
        };
        
        PlayerPrefsDataMgr.Instance.SaveData("ScoreDict", scoreDict);
        
        Dictionary<string, int> loadedDict = PlayerPrefsDataMgr.Instance.GetData("ScoreDict", new Dictionary<string, int>());
        
        Debug.Log("成绩字典:");
        foreach (var kvp in loadedDict)
        {
            Debug.Log($"  {kvp.Key}: {kvp.Value}");
        }
    }

    // 测试复杂组合类型
    void TestComplexCombination()
    {
        Debug.Log("=== 测试复杂组合类型 ===");
        
        // 创建一个包含多种类型的复杂数据
        ComplexData complexData = new ComplexData();
        
        PlayerPrefsDataMgr.Instance.SaveData("ComplexData", complexData);
        
        ComplexData loadedComplexData = PlayerPrefsDataMgr.Instance.GetData("ComplexData", new ComplexData());
        
        Debug.Log("复杂数据加载结果:");
        Debug.Log($"  ID: {loadedComplexData.id}");
        Debug.Log($"  Name: {loadedComplexData.name}");
        Debug.Log($"  Tags: [{string.Join(", ", loadedComplexData.tags)}]");
        Debug.Log($"  Settings:");
        foreach (var setting in loadedComplexData.settings)
        {
            Debug.Log($"    {setting.Key}: {setting.Value}");
        }
    }
}

// 测试用的复杂数据类
[System.Serializable]
public class ComplexData
{
    public int id;
    public string name;
    public List<string> tags;
    public Dictionary<string, bool> settings;

    public ComplexData()
    {
        id = 12345;
        name = "测试数据";
        tags = new List<string> { "重要", "测试", "示例" };
        settings = new Dictionary<string, bool>
        {
            {"自动保存", true},
            {"显示提示", false},
            {"启用音效", true}
        };
    }
}