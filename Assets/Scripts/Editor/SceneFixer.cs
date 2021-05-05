using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using TheArchitect.Core.Constants;

public class SceneFixer: EditorWindow
{
    [MenuItem("Window/Utilities/SceneFixer")]
    public static void MenuSceneFixer()
    {
        EditorWindow.GetWindow(typeof(SceneFixer));
    }

    void OnGUI()
    {
        GameObject root = Selection.activeGameObject;

        // if (GUILayout.Button("FLATTEN"))
        // {
        //     foreach (var t in  root.GetComponentsInChildren<Transform>())
        //     {
        //         t.SetParent(root.transform);
        //     }
        // }
        if (GUILayout.Button("FIX ROTATION"))
        {
            MeshFilter[] meshes = root.GetComponentsInChildren<MeshFilter>();
            Transform[] parents = new Transform[meshes.Length];
            
            for (int i=0; i< meshes.Length; i++)
            {
                if (meshes[i].GetComponentInParent<LODGroup>() == null) {
                    parents[i] = meshes[i].transform.parent;
                    meshes[i].transform.SetParent(null);
                }
            }

            foreach (var t in meshes)
            {
                if ((t.sharedMesh.name.StartsWith("SM_") || t.sharedMesh.name.StartsWith("ST_")) && t.GetComponentInParent<LODGroup>() == null)
                {
                    Transform p = t.transform.parent;
                    t.transform.SetParent(root.transform);
                    t.transform.Rotate(new Vector3(90, 0, 0));
                    t.transform.SetParent(p, true);
                }
            }

            for (int i=0; i< meshes.Length; i++)
            {
                if (meshes[i].GetComponentInParent<LODGroup>() == null) {
                    meshes[i].transform.SetParent(parents[i], true);
                }
            }
        }
        if (GUILayout.Button("FIX COLLIDER"))
        {
            foreach (var t in  root.GetComponentsInChildren<BoxCollider>())
            {
                t.size = new Vector3(t.size.x, t.size.z, t.size.y);
                t.center = new Vector3(t.center.x, t.center.z, t.center.y);
            }
        }

        if (GUILayout.Button("FIX LOD"))
        {
            foreach (var t in  root.GetComponentsInChildren<LODGroup>())
            {
                foreach (Transform tt in t.transform)
                    tt.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                t.transform.rotation = Quaternion.Euler(new Vector3(0, t.transform.rotation.y, 0));
            }
        }
    }


}