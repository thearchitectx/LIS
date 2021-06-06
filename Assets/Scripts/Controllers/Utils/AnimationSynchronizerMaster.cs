using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationSynchronizerMaster : MonoBehaviour
{
    public Animator[] Slaves;
    public int LayerIndex;
    #if UNITY_EDITOR
    public int FreezeFrame = -1;
    #endif
    private Animator m_Master;

    void Awake()
    {
        this.m_Master = GetComponent<Animator>();
    }

    void Update()
    {
        #if UNITY_EDITOR
        if (FreezeFrame>0)
            this.m_Master.Play(0, -1, FreezeFrame);
        #endif

        foreach (var slave in this.Slaves)
            slave.Play(0, LayerIndex, this.m_Master.GetCurrentAnimatorStateInfo(LayerIndex).normalizedTime);
    }
}