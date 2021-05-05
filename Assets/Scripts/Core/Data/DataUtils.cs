using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

namespace TheArchitect.Core.Data
{
    public class DataUtils
    {
        public static T LoadXmlObject<T>(string path)
        {
            #if UNITY_EDITOR
            Debug.Log($"Loading XML object: {path}");
            #endif
            using (StreamReader stream = File.OpenText(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                XmlReader xmlReader = XmlReader.Create(stream);
                return (T) serializer.Deserialize(xmlReader);
            }
        }
    }
}