using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureTilingAnimation : MonoBehaviour
{
    public float Speed = 1;
    private Material m_Material;
    private float pos;
    void Start()
    {
        this.m_Material = this.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        pos = Mathf.Repeat(pos + Time.deltaTime * Speed, 2);
        this.m_Material.SetTextureOffset("_MainTex", new Vector2(0, pos));
    }
}
