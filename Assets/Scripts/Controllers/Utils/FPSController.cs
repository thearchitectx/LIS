using UnityEngine;
using System.Collections;

public class FPSController
{
    float deltaTime = 0.0f;
    [SerializeField] UnityEngine.UI.Text textFps;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{1:0.} fps", msec, fps);
        textFps.text = text;
    }

}