
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using TheArchitect.Cutscene.Data;
using TheArchitect.Cutscene.Action;

namespace TheArchitect.Cutscene
{
    public enum CutsceneContext {
        Cutscene,
        Exploration
    }

    public class CutsceneLoader
    {
        public static CutsceneInstance Load(CutsceneContext context, string resourcePath)
        {
            string path = $"{Application.streamingAssetsPath}/{context.ToString()}/en/{resourcePath}.xml";
            if (!File.Exists(path)) 
            {
                Debug.LogWarning($"Can't find '{path}'");
                return new CutsceneInstance();
            }

			try {
	            using (StreamReader stream = File.OpenText(path))
	            {
                    XmlSerializer serializer = new XmlSerializer(typeof(CutsceneInstance));
                    XmlReader xmlReader = XmlReader.Create(stream);
                    return (CutsceneInstance)serializer.Deserialize(xmlReader);
                }
			} catch (System.Exception e) {
				Debug.LogWarning(e);
                return new CutsceneInstance();
			}
        }

        public static CutsceneInstance Load(string resourcePath)
        {
            TextAsset asset = Resources.Load<TextAsset>(resourcePath);
            XmlSerializer serializer = new XmlSerializer(typeof(CutsceneInstance));
            StringReader reader = new StringReader(asset.text);

            XmlReader xmlReader = XmlReader.Create(reader);

            CutsceneInstance cutscene = (CutsceneInstance)serializer.Deserialize(xmlReader);
            reader.Close();
            Resources.UnloadAsset(asset);
            
            return cutscene;
        }
    }
}