using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public abstract class XmlManager<T>
{
    public virtual string Path { get; set; }
    protected XmlSerializer xmlSerializer;

    public XmlManager()
    {
        
        xmlSerializer = new XmlSerializer(typeof(T));
    }

    public void SaveData(T data)
    {
        using(FileStream fs = new FileStream(Path, FileMode.Create))
        {
            xmlSerializer.Serialize(fs,data);
        }
    }

    public T LoadData()
    {
        using(FileStream fs = new FileStream(Path, FileMode.Open))
        {
            T data = (T)xmlSerializer.Deserialize(fs);
            return data;
        }
    }

    public bool ExistsData()
    {
        return File.Exists(Path);
    }
}
