using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
   Enables a set of Rigs when a Capsule Check is true
*/
public class RegionCheckRigActivator : MonoBehaviour
{
    [SerializeField] public LayerMask LayerMask;
    [SerializeField] public RigWeightDamper[] Rigs;
    [SerializeField] public float CapsuleHeight = 1.5f;
    [SerializeField] public float CapsuleRadius = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool targetInRegion = Physics.CheckCapsule(this.transform.position, this.transform.position+ new Vector3(0, CapsuleHeight,0), CapsuleRadius, LayerMask);
        foreach (var r in Rigs)
            r.SetWeight(targetInRegion? 1 : 0);
    }
}
