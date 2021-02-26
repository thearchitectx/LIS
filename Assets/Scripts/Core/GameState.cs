
using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace TheArchitect.Core
{
    public struct Float
    {
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("state")]
        public float State;
    }

    public struct Flag
    {
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("state")]
        public int State;
    }

    public struct TextData
    {
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("value")]
        public string State;
    }

    [XmlRoot("state")]
    public partial class GameState
    {
        public const string TEXT_SPAWN_POINT = "TEXT_SPAWN_POINT";
        
        [XmlAttribute("stage")]
        public string Stage;
        [XmlAttribute("px")]
        public float m_SpawnPosX;
        [XmlAttribute("py")]
        public float m_SpawnPosY;
        [XmlAttribute("pz")]
        public float m_SpawnPosZ;
        [XmlAttribute("rw")]
        public float m_SpawnRotW;
        [XmlAttribute("rx")]
        public float m_SpawnRotX;
        [XmlAttribute("ry")]
        public float m_SpawnRotY;
        [XmlAttribute("rz")]
        public float m_SpawnRotZ;

        [XmlElement("flag")]
        public Flag[] m_Flags = new Flag[0];
        [XmlElement("text")]
        public TextData[] m_Texts = new TextData[0];
        [XmlElement("float")]
        public Float[] m_Floats = new Float[0];
        [XmlElement("obj")]
        public string[] m_Objectives = new string[0];

        [XmlIgnore]
        private Dictionary<string, Flag> m_FlagIndex;
        [XmlIgnore]
        private Dictionary<string, TextData> m_TextIndex;
        [XmlIgnore]
        private Dictionary<string, Float> m_FloatIndex;
        [XmlIgnore]
        private HashSet<string> m_ObjectiveIndex;

        [XmlIgnore]
        public Dictionary<string, Flag> FlagIndex {
            get { 
                if (this.m_FlagIndex == null)
                {
                    this.m_Flags = this.m_Flags != null ? this.m_Flags : new Flag[0];
                    this.m_FlagIndex = new Dictionary<string, Flag>();
                    foreach (var flag in this.m_Flags)
                        this.m_FlagIndex.Add(flag.Name, flag);
                }

                return this.m_FlagIndex;
            }
        }

        [XmlIgnore]
        public Dictionary<string, TextData> TextIndex {
            get {
                if (this.m_TextIndex == null)
                {
                    this.m_Texts = this.m_Texts != null ? this.m_Texts : new TextData[0];
                    this.m_TextIndex = new Dictionary<string, TextData>();
                    foreach (var str in this.m_Texts)
                        this.m_TextIndex.Add(str.Name, str);
                }

                return this.m_TextIndex;
            }
        }

        [XmlIgnore]
        public Dictionary<string, Float> FloatIndex {
            get {
                if (this.m_FloatIndex == null)
                {
                    this.m_Floats = this.m_Floats != null ? this.m_Floats : new Float[0];
                    this.m_FloatIndex = new Dictionary<string, Float>();
                    foreach (var str in this.m_Floats)
                        this.m_FloatIndex.Add(str.Name, str);
                }

                return this.m_FloatIndex;
            }
        }

        [XmlIgnore]
        public HashSet<string> ObjectiveIndex {
            get {
                if (this.m_ObjectiveIndex == null)
                {
                    this.m_Objectives = this.m_Objectives != null ? this.m_Objectives : new string[0];
                    this.m_ObjectiveIndex = new HashSet<string>();
                    foreach (var o in this.m_Objectives)
                        this.m_ObjectiveIndex.Add(o);
                }

                return this.m_ObjectiveIndex;
            }
        }

        public void SetSpawnPosition(Vector3 p)
        {
            this.m_SpawnPosX = p.x;
            this.m_SpawnPosY = p.y;
            this.m_SpawnPosZ = p.z;
        }

        public Vector3 GetSpawnPosition()
        {
            return new Vector3(this.m_SpawnPosX, this.m_SpawnPosY, this.m_SpawnPosZ);
        }

        public void SetSpawnRotation(Quaternion r)
        {
            this.m_SpawnRotX = r.x;
            this.m_SpawnRotY = r.y;
            this.m_SpawnRotZ = r.z;
            this.m_SpawnRotW = r.w;
        }

        public Quaternion GetSpawnRotation()
        {
            return new Quaternion(this.m_SpawnRotX, this.m_SpawnRotY, this.m_SpawnRotZ, this.m_SpawnRotW);
        }
    }
}