using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

/// Changes an AudioSource volume over time based on a Rig weight movement
public class RigWeightAudioSourceControl : MonoBehaviour
{
    public Rig Rig;
    public AudioSource AudioSource;
    public float VolumeShiftSpeed = 1;

    private float m_LastWeight = 0;
    
    void Start()
    {
        this.m_LastWeight = this.Rig.weight;
    }

    void Update()
    {
        if (this.m_LastWeight != this.Rig.weight)
        {
            this.AudioSource.volume += Time.deltaTime * this.VolumeShiftSpeed;
        }
        else
        {
            this.AudioSource.volume -= Time.deltaTime * this.VolumeShiftSpeed;
        }

        this.m_LastWeight = this.Rig.weight;
    }
}
