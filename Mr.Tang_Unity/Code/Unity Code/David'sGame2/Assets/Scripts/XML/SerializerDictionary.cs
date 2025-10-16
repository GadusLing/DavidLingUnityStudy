using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class SerializerDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
{
    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
        XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

        bool wasEmpty = reader.IsEmptyElement;
        
        reader.Read();

        if (wasEmpty)
            return;

        while (reader.NodeType != XmlNodeType.EndElement)
        {

            TKey key = (TKey)keySerializer.Deserialize(reader);
            TValue value = (TValue)valueSerializer.Deserialize(reader);

            this.Add(key, value);
        }
        reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
        XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
        XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

        foreach (KeyValuePair<TKey, TValue> kvp in this)
        {
            keySerializer.Serialize(writer, kvp.Key);
            valueSerializer.Serialize(writer, kvp.Value);
        }
    }
}
