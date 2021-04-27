using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigWeightAxisInput : MonoBehaviour
{   
    [System.Serializable]
    public class AxisBinding
    {
        [SerializeField] public string Axis = "Mouse X";
        [SerializeField] public float Multiplier = 0.05f;
        [SerializeField] public bool UseDeltaTime = false;
        [SerializeField] public Rig[] Rigs;
    }

    [System.Serializable]
    public class FixedBinding
    {
        [SerializeField] public Rig Rig;
        [SerializeField] public float Weight;
    }

    [SerializeField] public AxisBinding[] m_Bindings;
    [SerializeField] public FixedBinding[] m_FixedBindings;
    [SerializeField] public float m_FixedBindingTransitionSpeed = 1;
    public bool AutoPingPong = false;
    public float AutoPingPongSpeed = 1;

    public void SetAutoPingPong(bool b)
    {
        this.AutoPingPong = b;
        UnityEngine.UI.Image image = this.GetComponentInChildren<UnityEngine.UI.Image>();
        if (image != null)
            image.color = new Color(1,1,1, !b ? 1 : 0.25f);
    }

    void Update()
    {
        if (Time.deltaTime==0)
            return;

        if (Input.GetMouseButtonDown(1))
            SetAutoPingPong(!AutoPingPong);

        foreach (var b in m_Bindings)
        {
            if (AutoPingPong)
            {
                foreach (var rig in b.Rigs) 
                    rig.weight = Mathf.PingPong(Time.time * AutoPingPongSpeed, 1);
            }
            else
            {
                float v = Input.GetAxis(b.Axis);
                foreach (var rig in b.Rigs)
                    rig.weight = Mathf.Clamp01(rig.weight + v * b.Multiplier * (b.UseDeltaTime ? Time.deltaTime : 1));
            }
        }

        foreach (var b in m_FixedBindings)
        {
            if (b!=null)
                b.Rig.weight = Mathf.MoveTowards(b.Rig.weight, b.Weight, Time.deltaTime * m_FixedBindingTransitionSpeed);
        }
    }
}
