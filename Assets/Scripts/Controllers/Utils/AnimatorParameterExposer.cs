using UnityEngine;
public class AnimatorParameterExposer : MonoBehaviour
{
    public void SetTrigger(string trigger)
    {
        transform.GetComponent<Animator>().SetTrigger(trigger);
    }
}