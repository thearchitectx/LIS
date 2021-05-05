using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace TheArchitect.Core.Data
{
    [XmlRoot("objectives")]
    public class ObjectiveList
    {
        [XmlElement("objective")]
        public Objective[] m_Objectives;
        private Dictionary<string, Objective> m_Index;

        public Objective Get(string name)
        {
            if (this.m_Index==null)
            {
                this.m_Index = new Dictionary<string, Objective>();
                if (this.m_Objectives!=null)
                    foreach (var o in this.m_Objectives)
                        this.m_Index.Add(o.Name, o);
            }

            Objective value;
            return this.m_Index.TryGetValue(name, out value) ? value : null;
        }
    }

    public class Objective
    {
        [XmlAttribute("name")]
        public string Name;
        [XmlElement("title")]
        public string Title;
        [XmlElement("description")]
        public string Description;
    }

}