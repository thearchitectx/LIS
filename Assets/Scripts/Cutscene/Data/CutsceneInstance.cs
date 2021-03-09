using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Cutscene.Action;

namespace TheArchitect.Cutscene.Data
{
    public class CutsceneInclude
    {
        [XmlAttribute("node-prefix")]
        public string Prefix;
        [XmlText]
        public string Path;
    }

    [XmlRoot("cutscene")]
    public class CutsceneInstance 
    {
        [XmlArray("nodes"), XmlArrayItem("n")]
        public CutsceneNode[] Nodes { get; set; }
        [XmlElement("include")]
        public CutsceneInclude[] Includes { get; set; }

        [XmlIgnore] public string Outcome;
        [XmlIgnore] private int m_CurrentNode;
        [XmlIgnore] public bool Finished
        {
            get { return this.Nodes != null && m_CurrentNode >= this.Nodes.Length; }
        }
        [XmlIgnore] public CutsceneNode CurrentNode 
        {
            get { return m_CurrentNode < this.Nodes.Length ? this.Nodes[m_CurrentNode] : null; } 
        }

        public void IncludeNodes(CutsceneInclude include, CutsceneInstance loadedInclude)
        {
            var n = new CutsceneNode[Nodes.Length + loadedInclude.Nodes.Length];
            Nodes.CopyTo(n, 0);
            loadedInclude.Nodes.CopyTo(n, Nodes.Length);

            for (var i = Nodes.Length; i<n.Length; i++)
                n[i].m_Id = include.Prefix + loadedInclude.Nodes[i-Nodes.Length].m_Id;

            this.Nodes = n;
        }

        public void Update(CutsceneController controller)
        {
            if (Time.deltaTime==0)
                return;
                
            CutsceneNode currentNode = this.Nodes[m_CurrentNode];

            string output = null;
            try {
                output = currentNode.CurrentAction.Update(this, controller);
            } 
            catch (System.Exception e)
            {
                Debug.LogError(e);
                output = CutsceneAction.OUTPUT_NEXT;
            }

            if (output == CutsceneAction.OUTPUT_END)
            {
                this.m_CurrentNode = this.Nodes.Length;
            }
            else if (output == CutsceneAction.OUTPUT_NEXT)
            {
                if (currentNode.HasNextAction())
                {
                    currentNode.NextAction();
                }
                else if (currentNode.Output != null)
                {
                    JumpToNode(currentNode.Output);
                }
                else
                {
                    this.m_CurrentNode = this.Nodes.Length;
                }
            }
            else if (output != null)
            {
                JumpToNode(output);
            }
        }

        public void JumpToNode(string id)
        {
            int n = FindNode(id);
            if (n>-1)
            {
                this.m_CurrentNode = n;
                this.Nodes[n].ResetState();
            }
            else
            {
                UnityEngine.Debug.LogWarning($"Node Id not found: '{id}'");
                this.m_CurrentNode = Nodes.Length;
            }
        }

        public int FindNode(string id)
        {
            for (var i = 0; i < Nodes.Length; i++)
            {
                if (Nodes[i].Id == id) 
                    return i;
            }
            return -1;
        }


        public void AssertValidCutsceneData()
        {
            for (var i = 0; i < this.Nodes.Length; i++) {
                if (this.Nodes[i].Output != null)
                {
                    if (FindNode(Nodes[i].Output)==-1)
                    {
                        throw new System.Exception($"Node default output not found: {Nodes[i].Output}");
                    }
                }

                while (this.Nodes[i].HasNextAction())
                {
                    var action = this.Nodes[i].NextAction();
                    var o = action.Valid(this);
                    if (o != null && o.GetType() == typeof(string))
                    {
                        throw new System.Exception($"Node[{i}]::{action.GetType().Name}: {o}");
                    }
                }
            }
        }

    }

}