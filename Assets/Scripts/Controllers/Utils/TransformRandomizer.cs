using UnityEngine;

/// Randomizes start transform of the object based on a list of locations
public class TransformRandomizer : MonoBehaviour
{
    [System.Serializable]
    public struct StartPoint
    {
        public Vector3 Position;
        public Vector3 Rotation;
    }

    [SerializeField] public StartPoint[] StartPoints;
    #if UNITY_EDITOR
    Mesh m_Mesh;
    #endif

    void Start()
    {
        int i = Random.Range(0, StartPoints.Length);
        this.transform.position = this.StartPoints[i].Position;
        this.transform.rotation = Quaternion.Euler(this.StartPoints[i].Rotation);
        Destroy(this);
    }

    void OnDrawGizmosSelected()
    {
        #if UNITY_EDITOR
        if (m_Mesh == null)
        {
            var mf = this.GetComponent<MeshFilter>();
            if (mf==null)
                mf = this.GetComponentInChildren<MeshFilter>();
            if (mf!=null)
                this.m_Mesh = mf.sharedMesh;
        }
        #endif
        // Display the explosion radius when selected
        Gizmos.color = new Color(1, 0, 0, 0.75F);
        foreach (var s in this.StartPoints)
        {
            #if UNITY_EDITOR
            if (this.m_Mesh!=null)
                Gizmos.DrawMesh(this.m_Mesh, s.Position, Quaternion.Euler(s.Rotation));
            Gizmos.DrawWireSphere(s.Position, 0.25f);
            #endif
        }
    }
}