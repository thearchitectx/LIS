using UnityEngine;
public class AnimatorTriggerKeyBinder : MonoBehaviour
{
    [System.Serializable]
    public class KeyTrigger
    {
        [SerializeField] public Animator Animator;
        [SerializeField] public KeyCode Key;
        [SerializeField] public string Trigger;
    }

    [SerializeField] public KeyTrigger[] Bindings;

    void Update() 
    {
        if (Bindings!=null && Time.deltaTime > 0)
        foreach (var b in Bindings)
        {
            if (Input.GetKeyDown(b.Key))
                b.Animator.SetTrigger(b.Trigger);
        }
    }
    
}