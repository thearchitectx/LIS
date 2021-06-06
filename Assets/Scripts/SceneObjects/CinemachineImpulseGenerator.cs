using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace TheArchitect.SceneObjects
{
    [RequireComponent(typeof (CinemachineImpulseSource))]
    public class CinemachineImpulseGenerator : SceneObject
    {
        public void GenerateImpulseEffect()
        {
            var s = this.GetComponent<CinemachineImpulseSource>();
            s.GenerateImpulse();
            StartCoroutine(_FlashPostProcessVolume());
        }

        private IEnumerator _FlashPostProcessVolume()
        {
            var v = this.GetComponent<PostProcessVolume>();

            while (v.weight > 0)
            {
                v.weight -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

    }

}