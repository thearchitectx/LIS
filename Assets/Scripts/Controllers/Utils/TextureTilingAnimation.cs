using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureTilingAnimation : MonoBehaviour
{
    public float Speed = 1;
    public float BumpSwingA = 0;
    public float BumpSwingB = 0;
    private Material m_Material;
    private int m_MainTexId;
    private int m_BumpScaleId;
    private int bumpDir;
    private float pos;
    void Start()
    {
        this.m_Material = this.GetComponent<MeshRenderer>().material;
        this.m_MainTexId = Shader.PropertyToID("_MainTex");
        this.m_BumpScaleId = Shader.PropertyToID("_BumpScale");
        this.bumpDir = 1;
    }

    // Update is called once per frame
    void Update()
    {
        pos = Mathf.Repeat(pos + Time.deltaTime * Speed, 2);
        this.m_Material.SetTextureOffset(m_MainTexId, new Vector2(0, pos));

        if (BumpSwingB>0)
        {
            float b = this.m_Material.GetFloat(m_BumpScaleId);
            this.m_Material.SetFloat(m_BumpScaleId, b + Time.deltaTime * Speed * 0.5f * bumpDir);
            if (b >= BumpSwingB)
                bumpDir = -1;
            if (b < BumpSwingA)
                bumpDir = 1;
        }
    }
}
