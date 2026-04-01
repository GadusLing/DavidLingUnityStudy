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
            // 清空字符串变量，为下一个消息类的生成做准备
            nameSpaceStr = "";
            classNameStr = "";
            fieldStr = "";
            writingStr = "";
            getIDStr = "";

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

            // 判断type属性，决定是否生成handler
            string type = msgNode.Attributes["type"] != null ? msgNode.Attributes["type"].Value.ToLower() : "rs";
            if (type == "so") // send only，不生成handler
            {
                continue;
            }
            //生成处理器脚本
            if (File.Exists(filePath + classNameStr + "Handler.cs")) // 如果处理器文件已经存在，则跳过生成 如果要重新生成处理器文件，可以先删除原有的处理器文件，再运行生成工具
            {
                continue; // 跳过当前循环，继续下一个消息类的生成
            }
            string handlerStr = $"using System.Collections;\r\nusing System.Collections.Generic;\r\nusing {nameSpaceStr};\r\nusing UnityEngine;\r\n\r\npublic class {classNameStr}Handler : BaseHandler\r\n{{\r\n\tpublic override void MsgHandle()\r\n\t{{\r\n\t\t{classNameStr} msg = message as {classNameStr}; // 将消息对象转换为{classNameStr}类型\r\n\t\t//以后我们处理对应某一个消息的逻辑只需要在消息处理者对象的\r\n\t\t//消息处理方法中写逻辑就行了\r\n\t}}\r\n}}"; // 拼接完整的消息处理器类代码字符串，格式为 "namespace 命名空间 { public class 类名Handler : BaseHandler { public override void MsgHandle() { 类名 msg = message as 类名; // 将消息对象转换为类名类型 //以后我们处理对应某一个消息的逻辑只需要在消息处理者对象的 //消息处理方法中写逻辑就行了 } } }"
            File.WriteAllText(filePath + classNameStr + "Handler.cs", handlerStr);
        }
        Debug.Log("消息类代码生成完成！"); // 输出日志，提示消息类代码生成完成
    }

    /// <summary>
    /// 生成消息池主要就是ID和消息类型以及消息处理器类型的对应关系
    /// </summary>
    public void GenerateMsgPool(XmlNodeList nodeList)
    {
        List<string> ids = new List<string>(); // 消息ID列表
        List<string> names = new List<string>(); // 消息类名列表
        List<string> nameSpaces = new List<string>(); // 消息命名空间列表
        foreach (XmlNode msgNode in nodeList)
        {
            // 只注册type不是so的消息
            string type = msgNode.Attributes["type"] != null ? msgNode.Attributes["type"].Value.ToLower() : "rs";
            if (type == "so") // send only，不注册
            {
                continue;
            }
            if (!ids.Contains(msgNode.Attributes["id"].Value))
            {
                ids.Add(msgNode.Attributes["id"].Value); // 获取消息ID属性值，添加到消息ID列表中
            }
            else
            {
                Debug.LogError($"消息ID {msgNode.Attributes["id"].Value} 重复了！请检查配置文件中的消息ID，确保每个消息ID都是唯一的！"); // 输出错误日志，提示消息ID重复了
                continue; // 跳过当前循环，继续下一个消息类的生成
            }
            if (!names.Contains(msgNode.Attributes["name"].Value))
            {
                names.Add(msgNode.Attributes["name"].Value); // 获取消息类名属性值，添加到消息类名列表中
            }
            else
            {
                Debug.LogError($"消息类名 {msgNode.Attributes["name"].Value} 重复了！请检查配置文件中的消息类名，确保每个消息类名都是唯一的！建议即使在不同命名空间下也不要重复消息类名。"); // 输出错误日志，提示消息类名重复了
                continue; // 跳过当前循环，继续下一个消息类的生成
            }
            if (!nameSpaces.Contains(msgNode.Attributes["namespace"].Value))
            {
                nameSpaces.Add(msgNode.Attributes["namespace"].Value); // 获取消息命名空间属性值，添加到消息命名空间列表中
            }
            else
            {
                Debug.LogError($"消息命名空间 {msgNode.Attributes["namespace"].Value} 重复了！请检查配置文件中的消息命名空间，确保每个消息命名空间都是唯一的！"); // 输出错误日志，提示消息命名空间重复了
                continue; // 跳过当前循环，继续下一个消息类的生成
            }
        }
        string nameSpaceStr = "";
        for (int i = 0; i < nameSpaces.Count; i++)
        {
            nameSpaceStr += $"using {nameSpaces[i]};\r\n"; // 拼接命名空间字符串，格式为 "using 命名空间;\r\n"
        }
        string registerStr = "";
        for (int i = 0; i < ids.Count; i++)
        {
            registerStr += $"\t\tRegister({ids[i]}, typeof({names[i]}), typeof({names[i]}Handler));\r\n"; // 拼接注册字符串，格式为 "Register(消息ID, typeof(消息类), typeof(消息处理器类));\r\n"
        }
        string msgPoolStr =
            "using System;\r\n" +
            "using System.Collections.Generic;\r\n" +
            "using UnityEngine;\r\n" +
            nameSpaceStr +
            "\r\n" +
            "/// <summary>\r\n" +
            "/// 消息池类\r\n" +
            "/// 自动生成于协议工具\r\n" +
            "/// </summary>\r\n" +
            "public class MsgPool\r\n" +
            "{\r\n" +
            "\tprivate Dictionary<int, Type> msgDic = new Dictionary<int, Type>(); // 消息字典\r\n" +
            "\tprivate Dictionary<int, Type> handlerDic = new Dictionary<int, Type>(); // 消息处理器字典\r\n\r\n" +
            "\tpublic MsgPool()\r\n" +
            "\t{\r\n" +
            registerStr +
            "\t}\r\n\r\n" +
            "\tprivate void Register(int msgID, Type msgType, Type handlerType)\r\n" +
            "\t{\r\n" +
            "\t\tif (!msgDic.ContainsKey(msgID))\r\n" +
            "\t\t{\r\n" +
            "\t\t\tmsgDic.Add(msgID, msgType);\r\n" +
            "\t\t\thandlerDic.Add(msgID, handlerType);\r\n" +
            "\t\t}\r\n" +
            "\t}\r\n\r\n" +
            "\tpublic BaseMsg GetMessage(int msgID)\r\n" +
            "\t{\r\n" +
            "\t\tif (msgDic.ContainsKey(msgID))\r\n" +
            "\t\t{\r\n" +
            "\t\t\treturn (BaseMsg)Activator.CreateInstance(msgDic[msgID]);\r\n" +
            "\t\t}\r\n" +
            "\t\telse\r\n" +
            "\t\t{\r\n" +
            "\t\t\tDebug.LogError($\"消息池中没有找到消息ID为 {msgID} 的消息对象类型！\");\r\n" +
            "\t\t\treturn null;\r\n" +
            "\t\t}\r\n" +
            "\t}\r\n\r\n" +
            "\tpublic BaseHandler GetHandler(int msgID)\r\n" +
            "\t{\r\n" +
            "\t\tif (handlerDic.ContainsKey(msgID))\r\n" +
            "\t\t{\r\n" +
            "\t\t\treturn (BaseHandler)Activator.CreateInstance(handlerDic[msgID]);\r\n" +
            "\t\t}\r\n" +
            "\t\telse\r\n" +
            "\t\t{\r\n" +
            "\t\t\tDebug.LogError($\"消息池中没有找到消息ID为 {msgID} 的消息处理器对象类型！\");\r\n" +
            "\t\t\treturn null;\r\n" +
            "\t\t}\r\n" +
            "\t}\r\n" +
            "}\r\n";
        string filePath = SAVE_PATH + "MsgPool/"; // 拼接文件保存路径，格式为 "保存路径/MsgPool/"
        if (!Directory.Exists(filePath)) // 如果文件保存路径不存在，则创建目录
        {
            Directory.CreateDirectory(filePath); // 创建目录
        }
        File.WriteAllText(filePath + "MsgPool.cs", msgPoolStr); // 将生成的消息池类代码写入文件
        Debug.Log("消息池类代码生成完成！"); // 输出日志，提示消息池类代码生成完成
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
    /// 生成GetID方法字符串，格式为 "public override int GetID() { return 消息ID; }"
    /// </summary> <param name="id">消息ID</param>
    /// <returns>GetID方法字符串</returns>
    private string GetIDStr(string id)
    {
        return $"\t\tpublic override int GetID()\r\n\t\t{{\r\n\t\t\treturn {id};\r\n\t\t}}"; // 拼接GetID方法字符串，格式为 "public override int GetID() { return 消息ID; }"
    }


}
