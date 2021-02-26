using UnityEngine;
using UnityEngine.Animations;
using System.Collections;
using System.Xml.Serialization;
using TheArchitect.Core.Constants;
using TheArchitect.Core.Controllers;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{
    public class RigTrackAction : CutsceneAction
    {
        [XmlAttribute("of")]
        public string Of = null;
        [XmlAttribute("eye")]
        public float Eye = float.NaN;
        [XmlAttribute("head")]
        public float Head = float.NaN;
        [XmlAttribute("root")]
        public float Root = float.NaN;
        [XmlAttribute("target")]
        public string Target = null;

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            Transform riggedObj = controller.FindProxy(Of);
            LogIf(riggedObj == null, $"Not found: {Of}");

            Config(controller, riggedObj, Eye, SkeletonPaths.RIG_EYETRACK);
            Config(controller, riggedObj, Head, SkeletonPaths.RIG_HEADTRACK);
            Config(controller, riggedObj, Root, SkeletonPaths.RIG_ROOT_TRACK);


            return OUTPUT_NEXT;
        }

        private void Config(CutsceneController controller, Transform riggedObj, float rigWeight, string rigPath)
        {
            if (!float.IsNaN(rigWeight))
            {
                Transform rig = riggedObj.Find(rigPath);
                LogIf(rig == null, $"{rigPath} not found in {riggedObj.name}");

                RigWeightDamper rwd = rig.GetComponent<RigWeightDamper>();
                LogIf(riggedObj == null, $"RigWeightDamper Not found in {rig.name}");
                rwd.SetWeight(rigWeight);

                if (Target!=null)
                {
                    CopyTransform copyTransform = rig.GetComponentInChildren<CopyTransform>();
                    LogIf(copyTransform == null, $"Copy Transform not found in {rig.name}");

                    copyTransform.m_Source = controller.FindProxy(Target);
                }
            }
        }
    }
}