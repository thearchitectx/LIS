using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Core;
using TheArchitect.Core.Constants;
using TheArchitect.Core.Controllers;
using TheArchitect.Core.Data.Variables;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{

    public class SFXAction : CutsceneAction
    {
        [XmlAttribute("clip")]
        public string Clip;
        [XmlAttribute("wait")]
        public bool WaitClip = false;
        
        private float m_Timer = 0;
        private float m_ClipLength = 0;

        public override void ResetState()
        {
            OneTimeAudioPlayer.GetAndCache(Clip);
            this.m_Timer = 0;
            this.m_ClipLength = 0;
        }

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            if (this.m_ClipLength > 0)
            {
                this.m_Timer += Time.deltaTime;
                return this.m_Timer > this.m_ClipLength ? OUTPUT_NEXT : null;
            } 
            else
            {
                this.m_ClipLength = OneTimeAudioPlayer.Play(Clip);
                return !this.WaitClip || this.m_ClipLength <=0 ? OUTPUT_NEXT : null;
            }
        }

    }
}