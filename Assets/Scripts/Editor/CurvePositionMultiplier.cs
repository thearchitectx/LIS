using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using TheArchitect.Core.Constants;

public class CurvePositionMultiplier: EditorWindow
{
    string mul = "100";

    [MenuItem("Window/Utilities/CurvePositionMultiplier")]
    public static void CurvePositionMultiplierS()
    {
        EditorWindow.GetWindow(typeof(CurvePositionMultiplier));
    }

    void OnGUI()
    {
        mul = EditorGUILayout.TextField("Value", mul);
        float mulFloat;

        if (GUILayout.Button("MULTIPLY POSITIONS"))
        {
            if (!float.TryParse(mul, out mulFloat)) {
                Debug.Log($"Invalid multiplier: {mul}");
                return;
            } else {
                Debug.Log($"Multiplier: {mul}:{mulFloat}");
            }
            string a = Selection.assetGUIDs[0];
            Debug.Log($"{a}: {AssetDatabase.GUIDToAssetPath(a)}");
            AnimationClip anim = AssetDatabase.LoadAssetAtPath<AnimationClip>(AssetDatabase.GUIDToAssetPath(a));
            string s="";
            foreach (var curve in AnimationUtility.GetCurveBindings(anim))
            {
                if ( 
                        ( curve.path == "ArmObject/Hips" || curve.path.StartsWith("ArmObject/Hips/Spine/Spine1/Spine2/Neck/Head/Grp_Head") )
                        && curve.propertyName.StartsWith("m_LocalPosition")
                )
                {
                    s = s + $"\n{curve.propertyName} {curve.path} ";
                
                    var ec = AnimationUtility.GetEditorCurve(anim, curve);
                    AnimationCurve newCurve = new AnimationCurve();
                    foreach (var k in ec.keys)
                    {
                        newCurve.AddKey(new Keyframe(k.time, k.value*mulFloat, k.inTangent, k.outTangent, k.inWeight, k.outWeight));
                    }
                    anim.SetCurve( curve.path, typeof(Transform), curve.propertyName, newCurve);
                }
            }
            Debug.Log(s);

        }

        
    }
}