using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MaterialColorBlinker : MonoBehaviour
{
    public Color ColorA = Color.white;
    public Color ColorB = new Color(0.8f, 0.8f, 0.8f);
    public float BlinkSpeed = 1;

    private Material m_Material;
    private float LerpPos = 0;
    private float LerpDir = 1;

    // Start is called before the first frame update
    void Start()
    {
        this.m_Material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        LerpPos += Time.deltaTime * LerpDir * BlinkSpeed;
        if (LerpPos > 1)
        {
            LerpPos = 1;
            LerpDir = -1;
        }
        if (LerpPos < -1)
        {
            LerpPos = -1;
            LerpDir = 1;
        }

        this.m_Material.color = Color.Lerp(ColorA, ColorB, LerpPos);
    }
}
