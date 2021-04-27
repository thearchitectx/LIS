using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TheArchitect.Core;

namespace TheArchitect.SceneObjects
{
    public class VideoRecorder : SceneObject
    {   
        public const string OUTCOME_CLICK = "click";

        public Image ImageFoV;
        public Text TextHelp;
        public CinemachineVirtualCamera m_Cinemachine;
        private float m_Speed = 10;
        private float m_MinFov = 5;
        private float m_MaxFov = 30;

        void Start()
        {
            var brain = Camera.main.GetComponent<CinemachineBrain>();
            if (brain != null)
                brain.m_CameraActivatedEvent.AddListener( RebindCinemachineEvent );

        }

        private void RebindCinemachineEvent(ICinemachineCamera a, ICinemachineCamera e)
        {
            RebindCinemachine();
        }

        public void RebindCinemachine()
        {
            var brain = Camera.main.GetComponent<CinemachineBrain>();
            
            if (brain != null)
            {
                var cm = brain.ActiveVirtualCamera;
                Debug.Log(cm);
                if (cm is CinemachineStateDrivenCamera)
                    cm = ( (CinemachineStateDrivenCamera) brain.ActiveVirtualCamera).LiveChild;

                Debug.Log("Binding camera:"+ cm);
                this.m_Cinemachine = cm as CinemachineVirtualCamera;
            }
        }

        void OnDestroy()
        {
            if (Camera.main!=null)
            {
                var brain = Camera.main.GetComponent<CinemachineBrain>();
                if (brain != null)
                    brain.m_CameraActivatedEvent.RemoveListener( RebindCinemachineEvent );
            }
        }

        public void MessageCanvasOrder(string order)
        {
            int o;
            if (int.TryParse(order, out o))
                this.GetComponentInChildren<Canvas>().sortingOrder = o;
        }

        public void MessageHelpText(string text)
        {
            TextHelp.text = text;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (Time.deltaTime==0)
                return;

            if (this.m_Cinemachine != null)
            {
                var fov = this.m_Cinemachine.m_Lens.FieldOfView + (Input.GetAxis("Horizontal") * Time.deltaTime * -10);
                this.m_Cinemachine.m_Lens.FieldOfView = Mathf.Clamp(fov, this.m_MinFov, this.m_MaxFov);
                this.ImageFoV.fillAmount = 1 - Mathf.InverseLerp(this.m_MinFov, this.m_MaxFov, fov);
            } else {
                this.ImageFoV.fillAmount = 0;
            }
            
        }

    }

}