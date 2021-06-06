using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Rig))]
public class RigBlendShapeSynchronizer : MonoBehaviour
{
    public SkinnedMeshRenderer BlendShapeOwner;
    public string BlendShapeName;
    public float BlendShapeFactor = 100;

    private Rig m_Rig;
    private int m_ShapeIndex;

    void Awake()
    {
        this.m_Rig = this.GetComponent<Rig>();
        this.m_ShapeIndex = this.BlendShapeOwner.sharedMesh.GetBlendShapeIndex(BlendShapeName);
    }

    void Update()
    {
        this.BlendShapeOwner.SetBlendShapeWeight(this.m_ShapeIndex, this.m_Rig.weight * BlendShapeFactor);
    }
}