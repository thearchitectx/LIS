using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Core;
using TheArchitect.Core.Constants;
using TheArchitect.Core.Controllers;
using TheArchitect.Core.Data.Variables;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{

    public class BGMAction : CutsceneAction
    {
        private const string OBJ_NAME = "BGMAction";

        [XmlAttribute("loop")]
        public string Loop;
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("volume")]
        public float Volume = 1;
        [XmlAttribute("fade-speed")]
        public float FadeSpeed = 1;
        [XmlAttribute("on-root")]
        public bool OnRoot = false;
        
        public override void ResetState()
        {
        }

        private Transform FindClipObject(CutsceneController controller)
        {
            if (OnRoot)
            {
                var g = GameObject.Find((string.IsNullOrEmpty(Name) ? OBJ_NAME : Name));
                return  g != null ? g.transform : null;
            }
            else
            {
                return controller.FindProxy((string.IsNullOrEmpty(Name) ? OBJ_NAME : Name));
            }
        }

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            if (string.IsNullOrEmpty(Loop))
            {
                var t = FindClipObject(controller);
                Debug.Log($"{t} {t.GetComponent<AudioSource>()}");
                AudioSource source;
                if (t!=null && (source = t.GetComponent<AudioSource>() ) !=null)
                    controller.StartCoroutine(FadeVolume(source, 0, true));
            }
            else
            {
                var t = FindClipObject(controller);
                if (t == null)
                {
                    GameObject tempAudioSource = new GameObject(string.IsNullOrEmpty(Name) ? OBJ_NAME : Name);
                    if (!OnRoot)
                        tempAudioSource.transform.SetParent(controller.transform);

                    AudioClip audioClip = Resources.Load<AudioClip>(string.Format("SFX/{0}", this.Loop));
                    AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();
                    audioSource.clip = audioClip;
                    audioSource.volume = 0;
                    audioSource.spatialBlend = 0.0f;
                    audioSource.loop = true;
                    controller.StartCoroutine(FadeVolume(audioSource, Volume, false));
                    audioSource.Play();
                } else {
                    controller.StartCoroutine(FadeVolume(t.GetComponent<AudioSource>(), Volume, false));
                }
                

            }
            return OUTPUT_NEXT; 
        }

        private System.Collections.IEnumerator FadeVolume(AudioSource audioSource, float volume, bool destroyOnEnd)
        {
            while (audioSource.volume!=volume)
            {
                audioSource.volume = Mathf.MoveTowards(audioSource.volume, volume, Time.deltaTime * FadeSpeed);
                yield return new WaitForEndOfFrame();
            }

            if (destroyOnEnd)
                GameObject.Destroy(audioSource.gameObject);
        }
    }
}