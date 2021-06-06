using UnityEngine;
public class AnimatorParamInputBinder : MonoBehaviour
{
    [System.Serializable]
    public class ParamBinding
    {
        [SerializeField] public Animator Animator;
        [SerializeField] public string Button;
        [SerializeField] public KeyCode KeyCode = KeyCode.None;
        [SerializeField] public string ParamName;

        public bool InputMatch()
        {
            return ( this.KeyCode != KeyCode.None && Input.GetKeyDown(this.KeyCode) )
                || ( !string.IsNullOrEmpty(this.Button) && Input.GetButtonDown(this.Button) );
        }
    }

    [System.Serializable]
    public class IntegerBinding : ParamBinding
    {
        [SerializeField] public int Value;
    }

    public ParamBinding[] TriggerBindings;
    public ParamBinding[] BoolToggleBindings;
    public IntegerBinding[] IntegerBindings;

    void Update() 
    {
        if (Time.deltaTime==0)
            return;

        foreach (var b in TriggerBindings)
            if (b.InputMatch())
                b.Animator.SetTrigger(b.ParamName);

        foreach (var b in IntegerBindings)
            if (b.InputMatch())
                b.Animator.SetInteger(b.ParamName, b.Value);

        foreach (var b in BoolToggleBindings)
            if (b.InputMatch())
                b.Animator.SetBool(b.ParamName, !b.Animator.GetBool(b.ParamName));
    }
    
}