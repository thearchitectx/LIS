using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Core;
using TheArchitect.Cutscene.Data;
using TheArchitect.Core.Data;
using TheArchitect.Core.Constants;

namespace TheArchitect.Cutscene.Action
{
    public class InjectAction : CutsceneAction
    {
        [XmlAttribute("node")]
        public string Node;

        [XmlIgnore]
        private CutsceneNode InjectedNode = null;

        public override void ResetState()
        {
            InjectedNode = null;
        }

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            if (InjectedNode == null)
            {
                int n = cutscene.FindNode(Node);
                if (n>=0)
                {
                    InjectedNode = cutscene.Nodes[n];
                    InjectedNode.ResetState();
                    return null;
                }
                else
                {
                    Debug.LogWarning($"Can't find node {Node} {n}");
                    return OUTPUT_NEXT;
                }

            }
            else
            {
                var output = InjectedNode.CurrentAction.Update(cutscene, controller);
                if (output==OUTPUT_NEXT && InjectedNode.HasNextAction())
                {
                    InjectedNode.NextAction();
                    return null;
                }
                else
                {
                    return output;
                }
            }


        }

    }

}