using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorInitializer : MonoBehaviour
{
    [System.Serializable]
    public struct AnimatorParameterInitialization {
        public AnimatorControllerParameterType type;
        public string name;
        public string value;
    }

    public AnimatorParameterInitialization[] parameters;
    public bool randomizeStartTime = false;

    void Start()
    {
        Animator a = GetComponent<Animator>();
        foreach (var p in parameters)
        {
            if (p.type==AnimatorControllerParameterType.Int)
            {
                int v = 0;
                int.TryParse(p.value, out v);
                a.SetInteger(p.name, v );
            }
            else if (p.type==AnimatorControllerParameterType.Float)
            {
                float v = 0;
                float.TryParse(p.value, out v);
                a.SetFloat(p.name, v );
            }
            else if (p.type==AnimatorControllerParameterType.Bool)
            {
                bool v = bool.TryParse(p.value, out v);
                a.SetBool(p.name, v);
            }
            else
            {
                a.SetTrigger(p.value);
            }
        }

        if (randomizeStartTime)
            for (var i = 0; i < a.layerCount; i++)
            {
                var si = a.GetCurrentAnimatorStateInfo(i);
                a.Play(si.fullPathHash, i, UnityEngine.Random.value);
            }

        Destroy(this, 2);        
    }
}