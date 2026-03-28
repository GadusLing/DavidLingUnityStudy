using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class ProtocolTool
{
    private static readonly string CONFIG_FILE_PATH = $"{Application.dataPath}/Editor/ProtocolTool/ProtocolInfo.xml"; // 配置文件所在路径
    private static GenerateCSharp generateCSharp = new GenerateCSharp(); // 生成C#代码的工具类实例

    [MenuItem("ProtocolTools/生成C#脚本")] // 在Unity编辑器的菜单栏中添加一个选项，点击后会调用下面的方法 
    // MenuItem是UnityEditor命名空间下的一个特性，用于在Unity编辑器的菜单栏中添加自定义选项，参数是菜单路径，点击该选项时会调用对应的方法
    private static void GenerateCSharp()
    {
        // 1. 读取配置文件中的信息（消息ID、消息名称、消息字段等）
        XmlNodeList list = GetXmlNodeList("enum"); // 从配置文件中获取所有"enum"节点的信息，返回一个XmlNodeList对象，包含了所有枚举相关的信息
        // 2. 根据读取到的信息，拼接字符串，动态生成对应的C#代码文件
        generateCSharp.GenerateEnum(list); // 调用生成C#枚举代码的方法
        generateCSharp.GenerateData(GetXmlNodeList("data")); // 调用生成C#数据结构代码的方法
        generateCSharp.GenerateMsg(GetXmlNodeList("message")); // 调用生成C#消息类代码的方法

        // 3. 刷新Unity编辑器，使生成的代码文件能够在编辑器中显示出来
        AssetDatabase.Refresh();
    }

    [MenuItem("ProtocolTools/生成C++脚本")]
    private static void GenerateCSharpPlusPlus()
    {
        // 后续需要可以自己补充
        // 1. 读取配置文件中的信息（消息ID、消息名称、消息字段等）
        // 2. 根据读取到的信息，动态生成对应的C++代码文件
        // 3. 将生成的代码文件保存到指定的目录下（如Assets/Scripts/GeneratedProtocols）
    }

    [MenuItem("ProtocolTools/生成Java脚本")]
    private static void GenerateJava()
    {
        // 后续需要可以自己补充
        // 1. 读取配置文件中的信息（消息ID、消息名称、消息字段等）
        // 2. 根据读取到的信息，动态生成对应的Java代码文件
        // 3. 将生成的代码文件保存到指定的目录下（如Assets/Scripts/GeneratedProtocols）
    }

    // 从配置文件中获取指定节点的信息，返回一个XmlNodeList对象，包含了所有匹配的子节点的信息
    private static XmlNodeList GetXmlNodeList(string nodeName)
    {
        XmlDocument xmlDoc = new XmlDocument(); // 创建一个XmlDocument对象，用于加载和解析XML文件
        xmlDoc.Load(CONFIG_FILE_PATH); // 加载指定路径的XML文件，解析成一个XmlDocument对象
        XmlNode root = xmlDoc.SelectSingleNode("messages"); // 从XmlDocument对象中选择第一个匹配"messages"的节点，返回一个XmlNode对象，代表这个节点
        return root.SelectNodes(nodeName); // 从上面获取到的节点中选择所有匹配nodeName的子节点，返回一个XmlNodeList对象，包含了所有匹配的子节点的信息
    }



}
