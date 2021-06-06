using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Animations.Rigging;

/// Changes an AudioSource volume over time based on a Rig weight movement
public class RigWeightPostProccessControl : MonoBehaviour
{
    public Rig Rig;
    public PostProcessVolume Volume;
    public AnimationCurve ActivationCurve;

    private float m_LastWeight = 0;
    private float m_Time = -1;
    
    void Start()
    {
        this.m_LastWeight = this.Rig.weight;
    }

    void Update()
    {
        if (this.m_LastWeight != this.Rig.weight)
        {
            this.m_Time = Mathf.Repeat(this.m_Time + Time.deltaTime, 1);
        }
        else
        {
            this.m_Time =  Mathf.Clamp01(this.m_Time + Time.deltaTime);
        }

        float f = this.ActivationCurve.Evaluate(this.m_Time);
        Volume.weight = f;

        this.m_LastWeight = this.Rig.weight;
    }
}
