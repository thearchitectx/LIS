using UnityEngine;


[RequireComponent(typeof (Light))]
public class LightFadeIn : MonoBehaviour
{
    [SerializeField] private float m_TargetIntensity;
    [SerializeField] private float m_Speed = 1;
    private Light m_Light;

    void Start()
    {
        this.m_Light = GetComponent<Light>();
        this.m_TargetIntensity = this.m_TargetIntensity == 0 ? this.m_Light.intensity : this.m_TargetIntensity;
    }

    void OnEnable()
    {
        this.m_Light = GetComponent<Light>();
        this.m_Light.intensity = 0;
    }

    void Update()
    {
        if (this.m_Light.intensity < this.m_TargetIntensity)
        {
            this.m_Light.intensity += Time.deltaTime * this.m_Speed;
        }
        else
        {
            this.m_Light.intensity = this.m_TargetIntensity;
        }
    }
}