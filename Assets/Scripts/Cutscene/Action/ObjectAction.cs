using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{
    public class ObjectAction : CutsceneAction
    {
        [XmlAttribute("name")]
        public string Name = null;
        [XmlAttribute("active")]
        public bool Active = true;
        [XmlAttribute("message")]
        public string Message;
        [XmlAttribute("destroy")]
        public float Destroy = float.NaN;

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            GameObject rootTarget;
            if (Name=="_parent")
                rootTarget = controller.transform.parent.gameObject;
            else
                rootTarget = Name.StartsWith("/")
                    ?   GameObject.Find(Name.Substring(1))
                    :   null;

            Transform target = rootTarget != null
                ? rootTarget.transform
                : controller.FindProxy(Name);

            if (target != null)
            {
                target.gameObject.SetActive(Active);
                
                if (!float.IsNaN(Destroy))
                {
                    GameObject.Destroy(target.gameObject, Destroy);
                }

                if (Message!=null && Message!="")
                {
                    target.gameObject.SendMessage(Message, SendMessageOptions.DontRequireReceiver);
                }
            }

            return OUTPUT_NEXT;
        }

    }
}