using TheArchitect.Cutscene;
using TheArchitect.Cutscene.Data;
using TheArchitect.Core.Constants;
using System.Xml.Serialization;
using UnityEngine;

namespace TheArchitect.Cutscene.Action
{
    public enum FadeToBlackMode
    {
        [XmlEnum(Name="to")]
        TO,
        [XmlEnum(Name="from")]
        FROM,
        [XmlEnum(Name="to-from")]
        TO_FROM
    }

    public class FadeToBlackAction : CutsceneAction
    {
        public const string PREFAB_NAME = "FadeToBlack";
        [XmlAttribute("speed")]
        public float Speed = 1f;
        [XmlAttribute("mode")]
        public FadeToBlackMode Mode;
        [XmlAttribute("keep")]
        public bool Keep = false;
        private GameObject gameObject;

        public float Time = 1;

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            var existing = controller.transform.Find(PREFAB_NAME);
            if (existing!=null)
                GameObject.Destroy(existing.gameObject);

            GameObject prefab;
            switch (Mode)
            {
                case FadeToBlackMode.TO_FROM: 
                    prefab = Resources.Load<GameObject>(ResourcePaths.PREFAB_FADE_TO_FROM_BLACK); 
                    Time = 0.5f;
                    break;
                case FadeToBlackMode.FROM:
                    prefab = Resources.Load<GameObject>(ResourcePaths.PREFAB_FADE_FROM_BLACK);
                    Time = 1;
                    break;
                default: 
                    prefab = Resources.Load<GameObject>(ResourcePaths.PREFAB_FADE_TO_BLACK);
                    Time = 1;
                    break;
            }

            gameObject = GameObject.Instantiate(prefab);
            gameObject.name = PREFAB_NAME;
            gameObject.transform.SetParent(controller.transform);
            Animator animator = gameObject.GetComponent<Animator>();
            animator.speed = Speed;

            if (!Keep)
            {
                GameObject.Destroy(gameObject, Speed / Time);
            }
            
            return OUTPUT_NEXT;
        }
    }
    
}