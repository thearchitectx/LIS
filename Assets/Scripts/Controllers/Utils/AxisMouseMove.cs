using UnityEngine;

public class InputAxisMover : MonoBehaviour
{   
    [Header("X Axis")]
    [SerializeField] private float m_MinX;
    [SerializeField] private float m_MaxX;
    [SerializeField] private string m_AxisX = "";
    [SerializeField] private float m_SpeedX = 0.05f;

    [Header("Y Axis")]
    [SerializeField] private float m_MinY;
    [SerializeField] private float m_MaxY;
    [SerializeField] private string m_AxisY = "";
    [SerializeField] private float m_SpeedY = 0.05f;

    [Header("Z Axis")]
    [SerializeField] private float m_MinZ;
    [SerializeField] private float m_MaxZ;
    [SerializeField] private string m_AxisZ = "";
    [SerializeField] private float m_SpeedZ = 0.05f;

    void Update()
    {
        Vector3 v = new Vector3();
        if (this.m_AxisX!="")
        {
            v.x = Mathf.Clamp(
                this.transform.position.x + ( Input.GetAxis(this.m_AxisX) * this.m_SpeedX ),
                this.m_MinX,
                this.m_MaxX
            );
        }
        if (this.m_AxisY!="")
        {
            v.y = Mathf.Clamp(
                this.transform.position.y + ( Input.GetAxis(this.m_AxisY) * this.m_SpeedY ),
                this.m_MinY,
                this.m_MaxY
            );
        }
        if (this.m_AxisZ!="")
        {
            v.y = Mathf.Clamp(
                this.transform.position.z + ( Input.GetAxis(this.m_AxisZ) * this.m_SpeedZ ),
                this.m_MinZ,
                this.m_MaxZ
            );
        }

        this.transform.localPosition = v;
    }
}
