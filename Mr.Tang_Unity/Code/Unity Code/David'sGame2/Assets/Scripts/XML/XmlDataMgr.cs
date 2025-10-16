using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Xml.Serialization;

public class XmlDataMgr
{
    private static XmlDataMgr _instance = new XmlDataMgr();

    public static XmlDataMgr Instance => _instance;

    private XmlDataMgr() { }

    public void SaveData(object data, string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".xml";
        using(StreamWriter writer = new StreamWriter(path))
        {
            XmlSerializer s = new XmlSerializer(data.GetType());
            s.Serialize(writer, data);
        }
    }

    public object LoadData(Type type, string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".xml";
        if(!File.Exists(path))
        {
            path = Application.streamingAssetsPath + "/" + fileName + ".xml";
            if(!File.Exists(path))
            {
                //如果根本不存在文件两个路径都找过了
                //那么直接new一个默认对象给外部，内部都是初始值
                return Activator.CreateInstance(type);
            }
        }
        using (StreamReader reader = new StreamReader(path))
        {
            XmlSerializer s = new XmlSerializer(type);
            return s.Deserialize(reader);
        }
    }
}
