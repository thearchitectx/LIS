using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace TheArchitect.Core
{
    public partial class GameState
    {
        public const string SAVE_SUB_DIRECTORY = "saves";
        public const string PICTURES_SUB_DIRECTORY = "saves/pictures";
        public const string STATE_FILE_NAME = "state.xml";
        public const string LABEL_FILE_NAME = "label.txt";
        public const string SCREEN_FILE_NAME = "screen.jpg";

        public void Rename(string root, string slot, string label)
        {
            Directory.CreateDirectory(GetSlotPath(root, slot));
            
            string labelFilePath = $"{GetSlotPath(root, slot)}/{LABEL_FILE_NAME}";
            File.WriteAllText(labelFilePath, label);
        }

        public void Save(string root, string slot, string label)
        {
            this.m_Flags = new Flag[this.FlagIndex.Count];
            this.FlagIndex.Values.CopyTo(this.m_Flags, 0);

            this.m_Floats = this.FloatIndex.Values.Where( f => !float.IsNaN(f.State) ).ToArray();

            this.m_Texts = new TextData[this.TextIndex.Count];
            this.TextIndex.Values.CopyTo(this.m_Texts, 0);

            this.m_Objectives = new string[this.ObjectiveIndex.Count];
            this.m_ObjectiveIndex.CopyTo(this.m_Objectives, 0);

            Directory.CreateDirectory(GetSlotPath(root, slot));

            string saveFilePath = $"{GetSlotPath(root, slot)}/{STATE_FILE_NAME}";
            string labelFilePath = $"{GetSlotPath(root, slot)}/{LABEL_FILE_NAME}";

            using (StreamWriter stream = File.CreateText(saveFilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(GameState));
                XmlWriter xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true });
                serializer.Serialize(xmlWriter, this);
            }

            File.WriteAllText(labelFilePath, label);
        }

        public static string GetSlotPath(string root, string slot)
        {
            return $"{root}/{SAVE_SUB_DIRECTORY}/{slot}";
        }

        public static string GetPicturesPath(string root)
        {
            return $"{root}/{PICTURES_SUB_DIRECTORY}";
        }

        public static GameState Load(string root, string slot)
        {
            string saveFilePath = $"{GetSlotPath(root, slot)}/{STATE_FILE_NAME}";

            Directory.CreateDirectory(GetSlotPath(root, slot));

            using (StreamReader stream = File.OpenText(saveFilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(GameState));
                XmlReader xmlReader = XmlReader.Create(stream);
                return serializer.Deserialize(xmlReader) as GameState;
            }
        }

        public static string LoadLabel(string root, string slot)
        {
            string labelFilePath = $"{GetSlotPath(root, slot)}/{LABEL_FILE_NAME}";
            
            return File.Exists(labelFilePath)
                ? File.ReadAllText(labelFilePath)
                : null;
        }

        public static byte[] LoadImage(string root, string slot)
        {
            string labelFilePath = $"{GetSlotPath(root, slot)}/{SCREEN_FILE_NAME}";
            
            return File.Exists(labelFilePath)
                ? File.ReadAllBytes(labelFilePath)
                : new byte[0];
        }

        public static string LoadLastWrite(string root, string slot)
        {
            string labelFilePath = $"{GetSlotPath(root, slot)}/{STATE_FILE_NAME}";
            System.DateTime d = File.GetLastWriteTime(labelFilePath);
            return File.Exists(labelFilePath)
                ? d.ToString("f") : "";
        }
    }
}