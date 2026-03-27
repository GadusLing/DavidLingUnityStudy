using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

namespace GamePlayer
{
    public class PlayerData : BaseData
    {
        int id;
        float atk;
        bool sex;
        long lev;
        List<int> list;
        Dictionary<int, string> dic;
        int[] arrays;
    }
}

public class GenerateCSharp
{
    private string SAVE_PATH = $"{Application.dataPath}/Scripts/Protocols/"; // 生成的代码文件保存路径
    // 生成枚举
    public void GenerateEnum(XmlNodeList nodeList)
    {
        string nameSpaceStr = ""; // 命名空间字符串
        string enumNameStr = ""; // 枚举名称字符串
        string fieldStr = ""; // 枚举字段字符串

        foreach(XmlNode enumNode in nodeList) // 遍历所有枚举节点
        {
            nameSpaceStr = enumNode.Attributes["namespace"].Value; // 获取枚举节点的命名空间属性值
            enumNameStr = enumNode.Attributes["name"].Value; // 获取枚举节点的名称属性值

            XmlNodeList enumFields = enumNode.SelectNodes("field"); // 获取枚举节点的所有子节点（即枚举字段节点）
            foreach (XmlNode enumField in enumFields)
            {
                string fieldName = enumField.Attributes["name"].Value; // 获取枚举字段节点的名称属性值
                string fieldValue = enumField.InnerText; // 获取枚举字段节点的值属性值
                fieldStr += "\t\t" + fieldName; // 拼接枚举字段字符串，格式为 "字段名称"
                if(enumField.InnerText != "")
                {
                    fieldStr += " = " + fieldValue; // 拼接枚举字段字符串，格式为 "字段名称 = 字段值"
                }
                fieldStr += ",\r\n"; // 拼接字符串换行符，格式为 "字段名称 = 字段值,\r\n"
            }
            string enumStr = $"namespace {nameSpaceStr}\r\n{{\r\n\tpublic enum {enumNameStr}\r\n\t{{\r\n{fieldStr}\t}}\r\n}}"; // 拼接完整的枚举代码字符串，格式为 "namespace 命名空间 { public enum 枚举名称 { 字段列表 } }"
            // 保存文件的路径
            string filePath = SAVE_PATH + nameSpaceStr + "/Enum/"; // 拼接文件保存路径，格式为 "保存路径/命名空间/Enum/"
            if (!Directory.Exists(filePath)) // 如果文件保存路径不存在，则创建目录
            {
                Directory.CreateDirectory(filePath); // 创建目录
            }
            File.WriteAllText(filePath + enumNameStr + ".cs", enumStr); // 将生成的枚举代码写入文件

            // 清空字符串变量，为下一个枚举的生成做准备
            nameSpaceStr = "";
            enumNameStr = "";
            fieldStr = "";

        }
        Debug.Log("枚举代码生成完成！"); // 输出日志，提示枚举代码生成完成

    }

    // 生成数据结构类
    public void GenerateData(XmlNodeList nodeList)
    {
        string nameSpaceStr = ""; // 命名空间字符串
        string classNameStr = ""; // 类名称字符串
        string fieldStr = ""; // 类字段字符串

        foreach(XmlNode dataNode in nodeList)
        {
            nameSpaceStr = dataNode.Attributes["namespace"].Value; // 获取数据结构节点的命名空间属性值
            classNameStr = dataNode.Attributes["name"].Value; // 获取数据结构节点的类名属性值
            XmlNodeList dataFields = dataNode.SelectNodes("field"); // 获取数据结构节点的所有子节点（即数据结构字段节点）
            fieldStr = GetFieldsStr(dataFields); // 调用GetFieldsStr方法，获取拼接好的类字段字符串
            string classStr = $"namespace {nameSpaceStr}\r\n{{\r\n\tpublic class {classNameStr}\r\n\t{{\r\n{fieldStr}\t}}\r\n}}"; // 拼接完整的类代码字符串，格式为 "namespace 命名空间 { public class 类名 { 字段列表 } }"
            // 保存文件的路径
            string filePath = SAVE_PATH + nameSpaceStr + "/Data/"; // 拼接文件保存路径，格式为 "保存路径/命名空间/Data/"
            if (!Directory.Exists(filePath)) // 如果文件保存路径不存在，则创建目录
            {
                Directory.CreateDirectory(filePath); // 创建目录
            }
            File.WriteAllText(filePath + classNameStr + ".cs", classStr); // 将生成的类代码写入文件

            // 清空字符串变量，为下一个类的生成做准备
            nameSpaceStr = "";
            classNameStr = "";
            fieldStr = "";
        }

    }

    // 生成消息类
    public void GenerateMsg(XmlNodeList nodeList)
    {

    }

    /// <summary>
    /// 根据数据结构字段节点列表，拼接生成类字段的字符串, 默认字段都是public的，格式为 "public 字段类型 字段名称;\r\n" 例： public int id
    /// </summary>
    /// <param name="fieldNodes">数据结构字段节点列表</param>
    /// <returns>拼接好的类字段字符串</returns>
    private string GetFieldsStr(XmlNodeList fieldNodes)
    {
        string fieldStr = ""; // 类字段字符串
        foreach (XmlNode dataField in fieldNodes)
        {
            string fieldType = dataField.Attributes["type"].Value; // 变量类型
            string fieldName = dataField.Attributes["name"].Value; // 变量名
            if(fieldType == "array")
            {
                string dataType = dataField.Attributes["data"].Value; // 数组元素类型
                fieldType = dataType + "[]"; // 将变量类型修改为数组类型，格式为 "元素类型[]"
            }
            else if(fieldType == "list")
            {
                string T = dataField.Attributes["T"].Value; // List的元素类型
                fieldType = $"List<{T}>"; // 将变量类型修改为List类型，格式为 "List<元素类型>"
            }
            else if(fieldType == "dic")
            {
                string Tkey = dataField.Attributes["Tkey"].Value; // Dictionary的键类型
                string Tvalue = dataField.Attributes["Tvalue"].Value; // Dictionary的值类型
                fieldType = $"Dictionary<{Tkey}, {Tvalue}>"; // 将变量类型修改为Dictionary类型，格式为 "Dictionary<键类型, 值类型>"
            }
            fieldStr += $"\t\tpublic {fieldType} {fieldName};\r\n"; // 拼接类字段字符串，格式为 "public 变量类型 变量名;\r\n"
            
        }
        return fieldStr; // 返回拼接好的类字段字符串
    }
}
