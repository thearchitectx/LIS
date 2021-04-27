using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MaterialColorBlinker : MonoBehaviour
{
    public Color32 ColorA = new Color32(0x03, 0x41, 0x00, 0xff);
    public Color32 ColorB = Color.black;
    public float BlinkSpeed = 1;
    public string ShaderColorProperty = "_EmissionColor";
    public string ShaderKeywordProperty = "_EMISSION";

    private Material m_Material;
    private float LerpPos = 0;
    private int m_ColorId;

    // Start is called before the first frame update
    void Start()
    {
        this.m_Material = GetComponent<MeshRenderer>().material;
        this.m_ColorId = Shader.PropertyToID(ShaderColorProperty);
        if (!string.IsNullOrEmpty(ShaderKeywordProperty))
            this.m_Material.EnableKeyword(ShaderKeywordProperty);
    }

    // Update is called once per frame
    void Update()
    {
        this.LerpPos = Mathf.PingPong(Time.time * BlinkSpeed, 1);
        this.m_Material.SetColor(this.m_ColorId, Color.Lerp(ColorA, ColorB, LerpPos));
    }

    void OnDestroy()
    {
        this.m_Material.DisableKeyword("_EMISSION");
    }
}
