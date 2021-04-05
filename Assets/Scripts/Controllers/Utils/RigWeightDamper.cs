using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Rig))]
public class RigWeightDamper : MonoBehaviour
{
    [SerializeField] [Range(0,1)] private float m_TargetWeight = 1;
    [SerializeField] public float TransitionTime = 1;
    [SerializeField] private float m_WeightToRestore = 1;

    private float m_RigWeightWhenChanged;
    private float m_TransitionTimer;
    private Rig m_Rig;
    
    public void SetWeightKeepRestore(float w)
    {
        if (w != this.m_TargetWeight)
        {
            this.m_TransitionTimer = 0;
            this.m_RigWeightWhenChanged = this.m_Rig == null ? 0 :this.m_Rig.weight;
        }
        this.m_TargetWeight = w;
    }

    public void SetWeight(float w)
    {
        if (w != this.m_TargetWeight)
        {
            this.m_TransitionTimer = 0;
            this.m_RigWeightWhenChanged = this.m_Rig == null ? 0 :this.m_Rig.weight;
        }
        this.m_TargetWeight = this.m_WeightToRestore = w;
    }

    public float GetTargetWeight()
    {
        return this.m_TargetWeight;
    }

    public float GetRigWeight()
    {
        return this.m_Rig.weight;
    }

    public void RestoreWeight()
    {
        this.m_TransitionTimer = 0;
        this.m_RigWeightWhenChanged = this.m_Rig.weight;
        this.m_TargetWeight = this.m_WeightToRestore;
    }

    public bool IsMoving()
    {
        return Mathf.Abs(this.m_TargetWeight - this.m_Rig.weight) > 0.00001f;
    }

    void Start()
    {
        this.m_Rig = GetComponent<Rig>();
    }

    // Update is called once per frame
    public void Update()
    {
        this.m_TransitionTimer += Time.deltaTime;
        this.m_Rig.weight = Mathf.SmoothStep(this.m_RigWeightWhenChanged, this.m_TargetWeight, Mathf.Clamp01(this.m_TransitionTimer / this.TransitionTime) );
    }
}
