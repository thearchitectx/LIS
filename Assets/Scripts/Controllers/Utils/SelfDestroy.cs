using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public void SelfDestroyNow()
    {
        Destroy(this.transform.gameObject);
    }
}