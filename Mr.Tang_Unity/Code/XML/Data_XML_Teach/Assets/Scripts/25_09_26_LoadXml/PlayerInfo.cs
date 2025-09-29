using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.IO;

public class Item
{
    public int id;
    public int num;
}

public class PlayerInfo
{
    public string name;
    public int atk;
    public int def;
    public float moveSpeed;
    public float roundSpeed;
    public Item weapon;
    public List<int> listInt;
    public List<Item> itemList;
    public Dictionary<int, Item> itemDic;

    public void LoadData(string fileName)
    {
        //如果可读可写中从来没有存储过 是不存在这个文件的 那么读取时就先从默认文件中获取内容
        string path = Application.persistentDataPath + "/" + fileName + ".xml";
        if(!File.Exists(path))
        {
            path = Application.streamingAssetsPath + "/" + fileName + ".xml";
        }
        

        XmlDocument xml = new XmlDocument();

        xml.Load(path);
        XmlNode root = xml.SelectSingleNode("PlayerInfo");
        name = root.SelectSingleNode("name").InnerText;
        atk = int.Parse(root.SelectSingleNode("atk").InnerText);
        def = Convert.ToInt32(root.SelectSingleNode("def").InnerText);
        moveSpeed = float.Parse(root.SelectSingleNode("moveSpeed").InnerText);
        roundSpeed = Convert.ToSingle(root.SelectSingleNode("roundSpeed").InnerText);

        weapon = new Item();
        XmlNode weaponNode = root.SelectSingleNode("weapon");
        weapon.id = int.Parse(weaponNode.SelectSingleNode("id").InnerText);
        weapon.num = int.Parse(weaponNode.SelectSingleNode("num").InnerText);
        
        listInt = new List<int>();
        XmlNode listIntNode = root.SelectSingleNode("listInt");
        XmlNodeList intList = listIntNode.SelectNodes("int");
        foreach (XmlNode intNode in intList)
        {
            listInt.Add(int.Parse(intNode.InnerText));
        }

        itemList = new List<Item>();
        XmlNode itemListNode = root.SelectSingleNode("itemList");
        XmlNodeList itemNodes = itemListNode.SelectNodes("Item");
        foreach (XmlNode itemNode in itemNodes)
        {
            Item item = new Item();
            item.id = int.Parse(itemNode.Attributes["id"].Value);
            item.num = int.Parse(itemNode.Attributes["num"].Value);
            itemList.Add(item);
        }

        itemDic = new Dictionary<int, Item>();
        XmlNode itemDicNode = root.SelectSingleNode("itemDic");
        XmlNodeList KeyInt = itemDicNode.SelectNodes("int");
        XmlNodeList Valueitem = itemDicNode.SelectNodes("Item");
        for (int i = 0; i < KeyInt.Count; i++)
        {
            int key = int.Parse(KeyInt[i].InnerText);
            Item item = new Item();
            item.id = int.Parse(Valueitem[i].Attributes["id"].Value);
            item.num = int.Parse(Valueitem[i].Attributes["num"].Value);
            itemDic.Add(key, item);
        }
    }

    public void SaveData(string fileName)
    {
        //决定存储路径
        string path = Application.persistentDataPath + "/" + fileName + ".xml";

        XmlDocument xml = new XmlDocument();
        XmlDeclaration declaration = xml.CreateXmlDeclaration("1.0", "utf-8", "");
        xml.AppendChild(declaration);

        XmlNode root = xml.CreateElement("PlayerInfo");
        xml.AppendChild(root);

        XmlNode nameNode = xml.CreateElement("name");
        nameNode.InnerText = name;
        root.AppendChild(nameNode);

        XmlNode atkNode = xml.CreateElement("atk");
        atkNode.InnerText = atk.ToString();
        root.AppendChild(atkNode);

        XmlNode defNode = xml.CreateElement("def");
        defNode.InnerText = def.ToString();
        root.AppendChild(defNode);

        XmlNode moveSpeedNode = xml.CreateElement("moveSpeed");
        moveSpeedNode.InnerText = moveSpeed.ToString();
        root.AppendChild(moveSpeedNode);

        XmlNode roundSpeedNode = xml.CreateElement("roundSpeed");
        roundSpeedNode.InnerText = roundSpeed.ToString();
        root.AppendChild(roundSpeedNode);

        XmlNode weaponNode = xml.CreateElement("weapon");
        XmlNode weaponIdNode = xml.CreateElement("id");
        weaponIdNode.InnerText = weapon.id.ToString();
        weaponNode.AppendChild(weaponIdNode);
        XmlNode weaponNumNode = xml.CreateElement("num");
        weaponNumNode.InnerText = weapon.num.ToString();
        weaponNode.AppendChild(weaponNumNode);
        root.AppendChild(weaponNode);

        XmlNode listIntNode = xml.CreateElement("listInt");
        foreach (var i in listInt)
        {
            XmlNode intNode = xml.CreateElement("int");
            intNode.InnerText = i.ToString();
            listIntNode.AppendChild(intNode);
        }
        root.AppendChild(listIntNode);

        XmlNode itemListNode = xml.CreateElement("itemList");
        foreach (var item in itemList)
        {
            XmlNode itemNode = xml.CreateElement("Item");
            XmlAttribute idAttr = xml.CreateAttribute("id");
            idAttr.Value = item.id.ToString();
            itemNode.Attributes.Append(idAttr);
            XmlAttribute numAttr = xml.CreateAttribute("num");
            numAttr.Value = item.num.ToString();
            itemNode.Attributes.Append(numAttr);
            itemListNode.AppendChild(itemNode);
        }
        root.AppendChild(itemListNode);

        XmlNode itemDicNode = xml.CreateElement("itemDic");
        foreach (var kv in itemDic)
        {
            XmlNode keyNode = xml.CreateElement("int");
            keyNode.InnerText = kv.Key.ToString();
            itemDicNode.AppendChild(keyNode);

            XmlNode valueNode = xml.CreateElement("Item");
            XmlAttribute idAttr = xml.CreateAttribute("id");
            idAttr.Value = kv.Value.id.ToString();
            valueNode.Attributes.Append(idAttr);
            XmlAttribute numAttr = xml.CreateAttribute("num");
            numAttr.Value = kv.Value.num.ToString();
            valueNode.Attributes.Append(numAttr);
            itemDicNode.AppendChild(valueNode);
        }
        root.AppendChild(itemDicNode);

        xml.Save(path);
    }
}