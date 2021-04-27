using UnityEngine;

namespace TheArchitect.Exploration
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class TouchColliderActivator : MonoBehaviour
    {
        [SerializeField] private Transform m_ActivationChild;
        private int m_LayerMask;
        private Collider m_Collider;
        
        void Start()
        {
            this.m_LayerMask = LayerMask.GetMask("Player");
        }

        public Transform GetActivationChild()
        {
            if (this.m_ActivationChild==null)
                this.m_ActivationChild = this.transform.Find("Activation");

            return this.m_ActivationChild;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ( this.m_LayerMask == (this.m_LayerMask | (1 << other.gameObject.layer)) )
            {
                Debug.Log($"BUM");
                GetActivationChild().transform.gameObject.SetActive(true);
            }
        }
    }
}