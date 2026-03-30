using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateCSharp
{
    private string SAVE_PATH = $"{Application.dataPath}/Scripts/Protocols/"; // 生成的代码文件保存路径
    // 生成枚举
    public void GenerateEnum(XmlNodeList nodeList)
    {
        string nameSpaceStr = ""; // 命名空间字符串
        string enumNameStr = ""; // 枚举名称字符串
        string fieldStr = ""; // 枚举字段字符串

        foreach (XmlNode enumNode in nodeList) // 遍历所有枚举节点
        {
            nameSpaceStr = enumNode.Attributes["namespace"].Value; // 获取枚举节点的命名空间属性值
            enumNameStr = enumNode.Attributes["name"].Value; // 获取枚举节点的名称属性值

            XmlNodeList enumFields = enumNode.SelectNodes("field"); // 获取枚举节点的所有子节点（即枚举字段节点）
            foreach (XmlNode enumField in enumFields)
            {
                string fieldName = enumField.Attributes["name"].Value; // 获取枚举字段节点的名称属性值
                string fieldValue = enumField.InnerText; // 获取枚举字段节点的值属性值
                fieldStr += "\t\t" + fieldName; // 拼接枚举字段字符串，格式为 "字段名称"
                if (enumField.InnerText != "")
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

        foreach (XmlNode dataNode in nodeList)
        {
            nameSpaceStr = dataNode.Attributes["namespace"].Value; // 获取数据结构节点的命名空间属性值
            classNameStr = dataNode.Attributes["name"].Value; // 获取数据结构节点的类名属性值
            XmlNodeList dataFields = dataNode.SelectNodes("field"); // 获取数据结构节点的所有子节点（即数据结构字段节点）
            fieldStr = GetFieldsStr(dataFields); // 调用GetFieldsStr方法，获取拼接好的类字段字符串
            string classStr = $"using System.Collections.Generic;\r\n\r\nnamespace {nameSpaceStr}\r\n{{\r\n\tpublic class {classNameStr} : BaseData\r\n\t{{\r\n{fieldStr}\t}}\r\n}}"; // 拼接完整的类代码字符串，格式为 "namespace 命名空间 { public class 类名 { 字段列表 } }"
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
        Debug.Log("数据结构类代码生成完成！"); // 输出日志，提示数据结构类代码生成完成
    }

    // 生成消息类
    public void GenerateMsg(XmlNodeList nodeList)
    {
        string nameSpaceStr = ""; // 命名空间字符串
        string classNameStr = ""; // 类名称字符串
        string fieldStr = ""; // 类字段字符串
        string writingStr = ""; // 写入方法字符串 在Msg中重写Writing方法是为了处理表头前8字节的写入，表头包含了消息ID和消息体长度，这些信息是BaseMsg类无法预知的，所以需要在生成的消息类中重写Writing方法来实现这个功能
        string getIDStr = ""; // GetID方法字符串 在Msg中重写GetID方法是为了返回每个消息类独有的消息ID，这个ID是在配置文件中定义的，BaseMsg类无法预知，所以需要在生成的消息类中重写GetID方法来实现这个功能
        foreach (XmlNode msgNode in nodeList)
        {
            nameSpaceStr = msgNode.Attributes["namespace"].Value; // 获取消息类节点的命名空间属性值
            classNameStr = msgNode.Attributes["name"].Value; // 获取消息类节点的类名属性值
            XmlNodeList msgFields = msgNode.SelectNodes("field"); // 获取消息类节点的所有子节点（即消息类字段节点）
            fieldStr = GetFieldsStr(msgFields); // 调用GetFieldsStr方法，获取拼接好的类字段字符串
            writingStr = GetWritingStr(); // 生成完整的 Writing() 重写方法，表头写 ID + body长度，body 由 base.Writing() 通过反射自动序列化所有字段
            getIDStr = GetIDStr(msgNode.Attributes["id"].Value); // 获取消息ID属性值，拼接成GetID方法字符串
            string classStr = $"using System.Collections.Generic;\r\n\r\nnamespace {nameSpaceStr}\r\n{{\r\n\tpublic class {classNameStr} : BaseMsg\r\n\t{{\r\n{fieldStr}\r\n{writingStr}\r\n{getIDStr}\r\n\t}}\r\n}}"; // 拼接完整的消息类代码字符串，格式为 "namespace 命名空间 { public class 类名 : BaseMsg { 字段列表 写入方法 GetID方法 } }"
            // 保存文件的路径
            string filePath = SAVE_PATH + nameSpaceStr + "/Msg/"; // 拼接文件保存路径，格式为 "保存路径/命名空间/Msg/"
            if (!Directory.Exists(filePath)) // 如果文件保存路径不存在，则创建目录
            {
                Directory.CreateDirectory(filePath); // 创建目录
            }
            File.WriteAllText(filePath + classNameStr + ".cs", classStr); // 将生成的消息类代码写入文件
            // 清空字符串变量，为下一个消息类的生成做准备
            nameSpaceStr = "";
            classNameStr = "";
            fieldStr = "";
            writingStr = "";
            getIDStr = "";
        }
        Debug.Log("消息类代码生成完成！"); // 输出日志，提示消息类代码生成完成
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
            if (fieldType == "array")
            {
                string dataType = dataField.Attributes["data"].Value; // 数组元素类型
                fieldType = dataType + "[]"; // 将变量类型修改为数组类型，格式为 "元素类型[]"
            }
            else if (fieldType == "list")
            {
                string T = dataField.Attributes["T"].Value; // List的元素类型
                fieldType = $"List<{T}>"; // 将变量类型修改为List类型，格式为 "List<元素类型>"
            }
            else if (fieldType == "dic")
            {
                string Tkey = dataField.Attributes["Tkey"].Value; // Dictionary的键类型
                string Tvalue = dataField.Attributes["Tvalue"].Value; // Dictionary的值类型
                fieldType = $"Dictionary<{Tkey}, {Tvalue}>"; // 将变量类型修改为Dictionary类型，格式为 "Dictionary<键类型, 值类型>"
            }
            else if (fieldType == "enum")
            {
                string dataType = dataField.Attributes["data"].Value; // 枚举类型
                fieldType = dataType; // 将变量类型修改为枚举类型，格式为 "枚举类型"
            }
            fieldStr += $"\t\tpublic {fieldType} {fieldName};\r\n"; // 拼接类字段字符串，格式为 "public 变量类型 变量名;\r\n"

        }
        return fieldStr; // 返回拼接好的类字段字符串
    }

    /// <summary>
    /// 生成完整的 Writing() 重写方法字符串。
    /// 结构与 PlayerMsg.Writing() 一致：base.Writing() 拿到 body → 开 8+body 的数组 → 写表头（ID + body长度）→ 拼 body → 返回。
    /// </summary>
    private string GetWritingStr()
    {
        string str = "";
        str += "\t\tpublic override byte[] Writing()\r\n";
        str += "\t\t{\r\n";
        str += "\t\t\tbyte[] body = base.Writing();\r\n";                       // 1. 父类按反射序列化所有字段，拿到纯数据 body
        str += "\t\t\tbyte[] bytes = new byte[4 + 4 + body.Length];\r\n";      // 2. 4字节消息ID + 4字节body长度 + body本体
        str += "\t\t\tint index = 0;\r\n";
        str += "\t\t\tWriteInt(bytes, GetID(), ref index);\r\n";               // 3. 写消息ID
        str += "\t\t\tWriteInt(bytes, body.Length, ref index);\r\n";            // 4. 写body长度
        str += "\t\t\tbody.CopyTo(bytes, index);\r\n";                          // 5. 把body拼在表头后面
        str += "\t\t\treturn bytes;\r\n";
        str += "\t\t}";
        return str;
    }

    /// <summary>
    /// 生成 GetID() 方法字符串，格式为 "public override int GetID() { return 消息ID; }" 消息ID 从配置文件中读取，每个消息类都有一个独特的消息ID，这个ID是在配置文件中定义的，BaseMsg类无法预知，所以需要在生成的消息类中重写GetID方法来实现这个功能，确保每个消息类都能正确返回它对应的消息ID，以便在协议通信中正确识别和处理不同类型的消息。/ </summary>
/// <param name="id"></param>
/// <returns></returns> <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private string GetIDStr(string id)
    {
        return $"\t\tpublic override int GetID()\r\n\t\t{{\r\n\t\t\treturn {id};\r\n\t\t}}"; // 拼接GetID方法字符串，格式为 "public override int GetID() { return 消息ID; }"
    }


}
