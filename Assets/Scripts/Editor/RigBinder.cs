using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using TheArchitect.Core.Constants;

public class RigBinder: EditorWindow
{
    [MenuItem("Window/Utilities/Rig Binder")]
    public static void FindMissingScripts()
    {
        EditorWindow.GetWindow(typeof(RigBinder));
    }

    void OnGUI()
    {
        GameObject character = Selection.activeGameObject;

        if (character==null)
            return;

        if (GUILayout.Button("RIG BINDING SMALL SKELETON"))
        {
            Common(character);
            SmallLips(character);
        }

        if (GUILayout.Button("RIG BINDING MEDIUM SKELETON"))
        {
            Common(character);
            MediumLips(character);
        }

        if (GUILayout.Button("RIG BINDING LARGE SKELETON"))
        {
            Common(character);
            LargeLips(character);
        }

        if (GUILayout.Button("MULTIPLY LIPS RIG x100"))
        {
            MultiplyLips(character);
        }
    }

    void Common(GameObject character)
    {
        Debug.ClearDeveloperConsole();
        
        Debug.Log($"Using {character.name}");

        Transform t = character.transform.Find(SkeletonPaths.PATH_HEAD);
        t.localRotation = Quaternion.Euler(new Vector3(0, -90, -90));

        t = character.transform.Find(SkeletonPaths.PATH_LEFT_ARM);
        t.localRotation = Quaternion.Euler(new Vector3(18, -38, -17.814f));

        t = character.transform.Find(SkeletonPaths.PATH_RIGHT_ARM);
        t.localRotation = Quaternion.Euler(new Vector3(-18, -38, 17.814f));
        
        t = character.transform.Find(SkeletonPaths.PATH_LEFT_FOREARM);
        t.localRotation = Quaternion.Euler(new Vector3(10, 0, 0));

        t = character.transform.Find(SkeletonPaths.PATH_RIGHT_FOREARM);
        t.localRotation = Quaternion.Euler(new Vector3(-10, 0, 0));

        t = character.transform.Find(SkeletonPaths.PATH_RIGHT_UP_LEG);
        t.localRotation = Quaternion.Euler(new Vector3(0, -5, 0));
        t = character.transform.Find(SkeletonPaths.PATH_RIGHT_LEG);
        t.localRotation = Quaternion.Euler(new Vector3(0, 10, 0));

        t = character.transform.Find(SkeletonPaths.PATH_LEFT_UP_LEG);
        t.localRotation = Quaternion.Euler(new Vector3(0, -5, 0));
        t = character.transform.Find(SkeletonPaths.PATH_LEFT_LEG);
        t.localRotation = Quaternion.Euler(new Vector3(0, 10, 0));

        character.transform.Find(SkeletonPaths.RIG_HEADTRACK).GetComponent<Rig>().weight = 0;
        character.transform.Find(SkeletonPaths.RIG_HEADTRACK).GetComponent<RigWeightDamper>().SetWeight(0);
        t = character.transform.Find($"{SkeletonPaths.RIG_HEADTRACK}/Neck");
        MultiAimConstraint d = t.GetComponent<MultiAimConstraint>();
        d.data.constrainedObject = character.transform.Find(SkeletonPaths.PATH_NECK);
        d.data.aimAxis = MultiAimConstraintData.Axis.X_NEG;

        t = character.transform.Find($"{SkeletonPaths.RIG_HEADTRACK}/Head");
        d = t.GetComponent<MultiAimConstraint>();
        d.data.constrainedObject = character.transform.Find(SkeletonPaths.PATH_HEAD);
        d.data.aimAxis = MultiAimConstraintData.Axis.Z;

        character.transform.Find(SkeletonPaths.RIG_EYETRACK).GetComponent<Rig>().weight = 0;
        character.transform.Find(SkeletonPaths.RIG_EYETRACK).GetComponent<RigWeightDamper>().SetWeight(0);
        t = character.transform.Find($"{SkeletonPaths.RIG_EYETRACK}/LeftEye");
        d = t.GetComponent<MultiAimConstraint>();
        d.data.constrainedObject = character.transform.Find(SkeletonPaths.PATH_LEFT_EYE);
        d.data.aimAxis = MultiAimConstraintData.Axis.Z;
        d.data.limits = new Vector2(-30, 30);

        t = character.transform.Find($"{SkeletonPaths.RIG_EYETRACK}/RightEye");
        d = t.GetComponent<MultiAimConstraint>();
        d.data.constrainedObject = character.transform.Find(SkeletonPaths.PATH_RIGHT_EYE);
        d.data.aimAxis = MultiAimConstraintData.Axis.Z;
        d.data.limits = new Vector2(-30, 30);

        character.transform.Find(SkeletonPaths.RIG_JAW_TALK).GetComponent<Rig>().weight = 0;
        character.transform.Find(SkeletonPaths.RIG_JAW_TALK).GetComponent<RigWeightDamper>().SetWeight(0);
        t = character.transform.Find($"{SkeletonPaths.RIG_JAW_TALK}/Jaw Override");
        OverrideTransform o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find(SkeletonPaths.PATH_JAW);
        o.data.rotation = new Vector3(0, 0, -4);

        // IK LEFT HAND
        t = character.transform.Find($"{SkeletonPaths.RIG_HAND_LEFT}/IK");
        TwoBoneIKConstraint ik = t.GetComponent<TwoBoneIKConstraint>();
        ik.data.root = character.transform.Find($"{SkeletonPaths.PATH_LEFT_ARM}");
        ik.data.mid = character.transform.Find($"{SkeletonPaths.PATH_LEFT_FOREARM}");
        ik.data.tip = character.transform.Find($"{SkeletonPaths.PATH_LEFT_HAND}");

        t = character.transform.Find($"{SkeletonPaths.RIG_HAND_LEFT}/Twist Arm");
        TwistCorrection tc = t.GetComponent<TwistCorrection>();
        WeightedTransformArray array = new WeightedTransformArray(1);
        array[0] = new WeightedTransform(character.transform.Find(SkeletonPaths.PATH_LEFT_ARM_ROLL), 0.5f);
        tc.data.twistNodes = array;

        t = character.transform.Find($"{SkeletonPaths.RIG_HAND_LEFT}/Twist Foream");
        tc = t.GetComponent<TwistCorrection>();
        array = new WeightedTransformArray(1);
        array[0] = new WeightedTransform(character.transform.Find(SkeletonPaths.PATH_LEFT_FOREARM_ROLL), 0.5f);
        tc.data.twistNodes = array;

        // IK RIGHT HAND
        t = character.transform.Find($"{SkeletonPaths.RIG_HAND_RIGHT}/IK");
        ik = t.GetComponent<TwoBoneIKConstraint>();
        ik.data.root = character.transform.Find($"{SkeletonPaths.PATH_RIGHT_ARM}");
        ik.data.mid = character.transform.Find($"{SkeletonPaths.PATH_RIGHT_FOREARM}");
        ik.data.tip = character.transform.Find($"{SkeletonPaths.PATH_RIGHT_HAND}");

        t = character.transform.Find($"{SkeletonPaths.RIG_HAND_RIGHT}/Twist Arm");
        tc = t.GetComponent<TwistCorrection>();
        array = new WeightedTransformArray(1);
        array[0] = new WeightedTransform(character.transform.Find(SkeletonPaths.PATH_RIGHT_ARM_ROLL), 0.5f);
        tc.data.twistNodes = array;

        t = character.transform.Find($"{SkeletonPaths.RIG_HAND_RIGHT}/Twist Foream");
        tc = t.GetComponent<TwistCorrection>();
        array = new WeightedTransformArray(1);
        array[0] = new WeightedTransform(character.transform.Find(SkeletonPaths.PATH_RIGHT_FOREARM_ROLL), 0.5f);
        tc.data.twistNodes = array;
        
        // IK Left Foot
        t = character.transform.Find($"{SkeletonPaths.RIG_FOOT_LEFT}/IK");
        ik = t.GetComponent<TwoBoneIKConstraint>();
        ik.data.root = character.transform.Find($"{SkeletonPaths.PATH_LEFT_UP_LEG}");
        ik.data.mid = character.transform.Find($"{SkeletonPaths.PATH_LEFT_LEG}");
        ik.data.tip = character.transform.Find($"{SkeletonPaths.PATH_LEFT_FOOT}");
        
        // IK Right Foot
        t = character.transform.Find($"{SkeletonPaths.RIG_FOOT_RIGHT}/IK");
        ik = t.GetComponent<TwoBoneIKConstraint>();
        ik.data.root = character.transform.Find($"{SkeletonPaths.PATH_RIGHT_UP_LEG}");
        ik.data.mid = character.transform.Find($"{SkeletonPaths.PATH_RIGHT_LEG}");
        ik.data.tip = character.transform.Find($"{SkeletonPaths.PATH_RIGHT_FOOT}");

        // Boobs

        t = character.transform.Find($"{SkeletonPaths.PATH_RIGHT_BOOB}");
        JiggleBone jb = t.GetComponent<JiggleBone>();
        if (jb==null)
            jb = t.gameObject.AddComponent<JiggleBone>();
        jb.boneAxis = new Vector3(-1 ,0 ,0);
        jb.upAxis = new Vector3(0, 1, 0);
        jb.targetDistance = 0.2f;
        jb.bGravity = 0f;
        jb.bStiffness = new Vector3(0.025f, 0.025f, 0.025f);
        jb.bMass = 1;
        jb.bDamping = 0.75f;
        jb.bDistanceLimit = 5f;

        t = character.transform.Find($"{SkeletonPaths.PATH_LEFT_BOOB}");
        jb = t.GetComponent<JiggleBone>();
        if (jb==null)
            jb = t.gameObject.AddComponent<JiggleBone>();
        jb.boneAxis = new Vector3(-1 ,0 ,0);
        jb.upAxis = new Vector3(0, 1, 0);
        jb.targetDistance = 0.2f;
        jb.bGravity = 0f;
        jb.bStiffness = new Vector3(0.025f, 0.025f, 0.025f);
        jb.bMass = 1;
        jb.bDamping = 0.75f;
        jb.bDistanceLimit = 5f;

        // Hair
        t = character.transform.Find($"{SkeletonPaths.PATH_HAIR}");
        jb = t.GetComponent<JiggleBone>();
        if (jb==null)
            jb = t.gameObject.AddComponent<JiggleBone>();
        jb.boneAxis = new Vector3(0, -1 ,0);
        jb.upAxis = new Vector3(-1, 0, 0);
        jb.targetDistance = 0.1f;
        jb.bGravity = 1f;
        jb.bStiffness = new Vector3(0.003f, 0.25f, 0.003f);
        jb.bMass = 1;
        jb.bDamping = 0.3f;
        jb.bDistanceLimit = 5f;
    }

    void LargeLips(GameObject character)
    {
        character.transform.Find(SkeletonPaths.RIG_LIPS_TALK).GetComponent<Rig>().weight = 0;
        character.transform.Find(SkeletonPaths.RIG_LIPS_TALK).GetComponent<RigWeightDamper>().SetWeight(0);
        
        Transform t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/LeftUpLip");
        OverrideTransform o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/LeftUpLip");
        o.data.position = new Vector3(-0.00002f, 0.00005f, -0.00005f);

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/RightUpLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/RightUpLip");
        o.data.position = new Vector3(-0.00001f, 0.00005f, 0.00005f);

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/UpLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/UpLip");
        o.data.position = new Vector3(0, 0.00005f, 0);
        
        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/DownLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/UpLip");
        o.data.position = new Vector3(0, 0.00008f, 0);

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/LeftLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/LeftLip");
        o.data.position = new Vector3(0, 0.0001f, -0.00008f);

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/RightLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/RightLip");
        o.data.position = new Vector3(0, 0.00005f, 0.00008f);

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/LeftDownLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_JAW}/LeftDownLip");
        o.data.position = new Vector3(0, 0.00005f, -0.00004f);

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/RightDownLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_JAW}/RightDownLip");
        o.data.position = new Vector3(0, 0.00005f, 0.00004f);
    }

    void MultiplyLips(GameObject character)
    {
        Transform t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/LeftUpLip");
        OverrideTransform o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/LeftUpLip");
        o.data.position *= 100;

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/RightUpLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/RightUpLip");
        o.data.position *= 100;

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/UpLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/UpLip");
        o.data.position *= 100;
        
        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/DownLip");
        if (t!=null) {
            o = t.GetComponent<OverrideTransform>();
            o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/UpLip");
            o.data.position *= 100;
        }

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/LeftLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/LeftLip");
        o.data.position *= 100;

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/RightLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/RightLip");
        o.data.position *= 100;

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/LeftDownLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_JAW}/LeftDownLip");
        o.data.position *= 100;

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/RightDownLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_JAW}/RightDownLip");
        o.data.position *= 100;
    }

    void MediumLips(GameObject character)
    {
        character.transform.Find(SkeletonPaths.RIG_LIPS_TALK).GetComponent<Rig>().weight = 0;
        character.transform.Find(SkeletonPaths.RIG_LIPS_TALK).GetComponent<RigWeightDamper>().SetWeight(0);
        
        Transform t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/LeftUpLip");
        OverrideTransform o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/LeftUpLip");
        o.data.position = new Vector3(-0.00002f, -0.00005f, -0.00006f);

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/RightUpLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/RightUpLip");
        o.data.position = new Vector3(-0.00002f, -0.00005f, 0.00006f);

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/UpLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/UpLip");
        o.data.position = new Vector3(0, -0.00002f, 0);

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/LeftLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/LeftLip");
        o.data.position = new Vector3(0, -0.00008f, -0.00008f);

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/RightLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/RightLip");
        o.data.position = new Vector3(0, -0.00008f, 0.00008f);

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/LeftDownLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_JAW}/LeftDownLip");
        o.data.position = new Vector3(0, -0.00004f, -0.00005f);

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/RightDownLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_JAW}/RightDownLip");
        o.data.position = new Vector3(0, -0.00004f, 0.00005f);
    }

    void SmallLips(GameObject character)
    {
        character.transform.Find(SkeletonPaths.RIG_LIPS_TALK).GetComponent<Rig>().weight = 0;
        character.transform.Find(SkeletonPaths.RIG_LIPS_TALK).GetComponent<RigWeightDamper>().SetWeight(0);
        
        Transform t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/LeftUpLip");
        OverrideTransform o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/LeftUpLip");
        o.data.position = new Vector3(-0.00001f, 0.00005f, -0.00005f);

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/RightUpLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/RightUpLip");
        o.data.position = new Vector3(-0.00001f, 0.00005f, 0.00005f);

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/UpLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/UpLip");
        o.data.position = new Vector3(-0.00001f, 0.00005f, 0);

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/LeftLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_HEAD}/Grp_Head/LeftLip");
        o.data.position = new Vector3(0, 0.00005f, -0.00007f);

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/LeftDownLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_JAW}/LeftDownLip");
        o.data.position = new Vector3(0, 0.0001f, -0.00004f);

        t = character.transform.Find($"{SkeletonPaths.RIG_LIPS_TALK}/RightDownLip");
        o = t.GetComponent<OverrideTransform>();
        o.data.constrainedObject = character.transform.Find($"{SkeletonPaths.PATH_JAW}/RightDownLip");
        o.data.position = new Vector3(0, 0.0001f, 0.00004f);
    }
}