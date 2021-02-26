using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Rig))]
public class RigWeightDamper : MonoBehaviour
{
    [SerializeField] [Range(0,1)] private float m_TargetWeight = 1;
    [SerializeField] public float MaxSpeed = 10;
    [SerializeField] public float SmoothTime = 0.05f;
    [SerializeField] private float m_WeightToRestore = 1;

    public float m_Speed;
    private Rig m_Rig;
    
    public void SetWeightKeepRestore(float w)
    {
        this.m_TargetWeight = w;
    }

    public void SetWeight(float w)
    {
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
        this.m_TargetWeight = this.m_WeightToRestore;
    }

    public bool IsMoving()
    {
        return Mathf.Abs(this.m_Speed) > 0.0001f;
    }

    void Start()
    {
        this.m_Rig = GetComponent<Rig>();
    }

    // Update is called once per frame
    public void Update()
    {
        this.m_Rig.weight = Mathf.SmoothDamp(this.m_Rig.weight, this.m_TargetWeight, ref this.m_Speed, this.SmoothTime, this.MaxSpeed);
    }
}
