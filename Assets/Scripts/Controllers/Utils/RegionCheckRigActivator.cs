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
    [SerializeField] public Transform Deactivator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Deactivator != null && Deactivator.gameObject.activeSelf)
            return;

        bool targetInRegion = Physics.CheckCapsule(this.transform.position, this.transform.position+ new Vector3(0, CapsuleHeight,0), CapsuleRadius, LayerMask);
        foreach (var r in Rigs)
            r.SetWeight(targetInRegion? 1 : 0);
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawWireSphere(transform.position, CapsuleRadius);
    }
}
