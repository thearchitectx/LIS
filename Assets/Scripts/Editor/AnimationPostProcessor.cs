using UnityEngine;
using UnityEditor;
using System.Collections;
 
public class AnimationPostProcessor : AssetPostprocessor {
 
    static readonly string[] positionBones = {"Hips", "EyesBrown1", "EyesBrown1", "Lip", "Cheek", "Snear", "Laugh" };

    void OnPostprocessAnimation(GameObject go, AnimationClip ac)
    {
        // Only operate on FBX files
        if (assetPath.IndexOf(".fbx") == -1) {
            return;
        }

        foreach (var curve in AnimationUtility.GetCurveBindings(ac))
        {
            if (curve.propertyName.StartsWith("blendShape"))
            {
                ac.SetCurve(curve.path, curve.type, curve.propertyName, null);
                continue;
            }
            if (curve.propertyName.StartsWith("m_LocalScale"))
            {
                ac.SetCurve(curve.path, curve.type, "m_LocalScale", null);
            }

            bool keepPositionCurve = false;
            foreach (var b in positionBones)
            {
                if (curve.path.EndsWith(b))
                {
                    keepPositionCurve = true;
                    break;
                }
            }
            if (!keepPositionCurve)
                ac.SetCurve(curve.path, curve.type, "m_LocalPosition", null);
        }
               
    }
 
}