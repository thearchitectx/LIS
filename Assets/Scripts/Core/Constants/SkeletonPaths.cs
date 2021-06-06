namespace TheArchitect.Core.Constants
{
    public enum SkeletonType {
        SMALL,
        MEDIUM,
        LARGE
    }

    public enum SkeletonRig {
        HEAD,
        EYES,
        HAND_LEFT,
        HAND_RIGHT,
        FOOT_LEFT,
        FOOT_RIGHT,
    }
    
    public struct SkeletonPaths
    {
        public const string BLENDSHAPE_ENHANCED = "Enhanced";
        
        public const string RIG_ROOT_TRACK = "ArmObject Track";
        public const string RIG_HEADTRACK = "Rig Head Track";
        public const string RIG_EYETRACK = "Rig Eye Track";
        public const string RIG_JAW_TALK = "Jaw Talk";
        public const string RIG_LIPS_TALK = "Lips Talk";
        public const string RIG_HAND_LEFT = "IK Left Hand";
        public const string RIG_HAND_RIGHT = "IK Right Hand";
        public const string RIG_FOOT_LEFT = "IK Left Foot";
        public const string RIG_FOOT_RIGHT = "IK Right Foot";

        public const string BONE_HIPS = "Hips";
        public const string BONE_SPINE = "Spine";
        public const string BONE_SPINE1 = "Spine1";
        public const string BONE_SPINE2 = "Spine2";
        public const string BONE_NECK = "Neck";
        public const string BONE_HEAD = "Head";
        public const string BONE_JAW = "Jaw";
        public const string BONE_DOWN_LIP = "DownLip";
        public const string BONE_LEFT_EYE = "LeftEye";
        public const string BONE_RIGHT_EYE = "RightEye";

        public static readonly string PATH_ROOT = "ArmObject";
        public static readonly string PATH_HIPS = "ArmObject/Hips";
        public static readonly string PATH_SPINE = $"{PATH_HIPS}/Spine";
        public static readonly string PATH_SPINE1 = $"{PATH_SPINE}/Spine1";
        public static readonly string PATH_RIGHT_BOOB = $"{PATH_SPINE1}/RightBoob";
        public static readonly string PATH_LEFT_BOOB = $"{PATH_SPINE1}/LeftBoob";
        public static readonly string PATH_SPINE2 = $"{PATH_SPINE1}/Spine2";
        public static readonly string PATH_NECK = $"{PATH_SPINE2}/Neck";
        public static readonly string PATH_HEAD = $"{PATH_NECK}/Head";
        public static readonly string PATH_HAIR = $"{PATH_HEAD}/Hair";
        public static readonly string PATH_JAW = $"{PATH_HEAD}/Grp_Head/Jaw";
        public static readonly string PATH_DOWN_LIP = $"{PATH_JAW}/DownLip";
        public static readonly string PATH_LEFT_EYE = $"{PATH_HEAD}/Grp_Head/LeftEye";
        public static readonly string PATH_RIGHT_EYE = $"{PATH_HEAD}/Grp_Head/RightEye";
        public static readonly string PATH_LEFT_SHOULDER = $"{PATH_SPINE2}/LeftShoulder";
        public static readonly string PATH_LEFT_ARM = $"{PATH_LEFT_SHOULDER}/LeftArm";
        public static readonly string PATH_LEFT_ARM_ROLL = $"{PATH_LEFT_ARM}/LeftArmRoll";
        public static readonly string PATH_LEFT_FOREARM = $"{PATH_LEFT_ARM_ROLL}/LeftForearm";
        public static readonly string PATH_LEFT_FOREARM_ROLL = $"{PATH_LEFT_FOREARM}/LeftForearmRoll";
        public static readonly string PATH_LEFT_HAND = $"{PATH_LEFT_FOREARM_ROLL}/LeftHand";
        public static readonly string PATH_RIGHT_SHOULDER = $"{PATH_SPINE2}/RightShoulder";
        public static readonly string PATH_RIGHT_ARM = $"{PATH_RIGHT_SHOULDER}/RightArm";
        public static readonly string PATH_RIGHT_ARM_ROLL = $"{PATH_RIGHT_ARM}/RightArmRoll";
        public static readonly string PATH_RIGHT_FOREARM = $"{PATH_RIGHT_ARM_ROLL}/RightForearm";
        public static readonly string PATH_RIGHT_FOREARM_ROLL = $"{PATH_RIGHT_FOREARM}/RightForearmRoll";
        public static readonly string PATH_RIGHT_HAND = $"{PATH_RIGHT_FOREARM_ROLL}/RightHand";
        public static readonly string PATH_RIGHT_UP_LEG = $"{PATH_HIPS}/RightUpLeg";
        public static readonly string PATH_RIGHT_UP_LEG_ROLL = $"{PATH_RIGHT_UP_LEG}/RightUpLegRoll";
        public static readonly string PATH_RIGHT_LEG = $"{PATH_RIGHT_UP_LEG_ROLL}/RightLeg";
        public static readonly string PATH_RIGHT_LEG_ROLL = $"{PATH_RIGHT_LEG}/RightLegRoll";
        public static readonly string PATH_RIGHT_FOOT = $"{PATH_RIGHT_LEG_ROLL}/RightFoot";
        public static readonly string PATH_LEFT_UP_LEG = $"{PATH_HIPS}/LeftUpLeg";
        public static readonly string PATH_LEFT_UP_LEG_ROLL = $"{PATH_LEFT_UP_LEG}/LeftUpLegRoll";
        public static readonly string PATH_LEFT_LEG = $"{PATH_LEFT_UP_LEG_ROLL}/LeftLeg";
        public static readonly string PATH_LEFT_LEG_ROLL = $"{PATH_LEFT_LEG}/LeftLegRoll";
        public static readonly string PATH_LEFT_FOOT = $"{PATH_LEFT_LEG_ROLL}/LeftFoot";

        public static string GetPathTo(string bone)
        {
            switch (bone) 
            {
                case BONE_HIPS: return PATH_HIPS;
                case BONE_SPINE: return PATH_SPINE;
                case BONE_SPINE1: return PATH_SPINE1;
                case BONE_SPINE2: return PATH_SPINE2;
                case BONE_NECK: return PATH_NECK;
                case BONE_HEAD: return PATH_HEAD;
                case BONE_LEFT_EYE: return PATH_LEFT_EYE;
                case BONE_RIGHT_EYE: return PATH_RIGHT_EYE;
                case BONE_DOWN_LIP: return PATH_DOWN_LIP;
            }

            return null;
        }

        public static string GetPathTo(SkeletonRig rig)
        {
            switch (rig) 
            {
                case SkeletonRig.HEAD: return RIG_HEADTRACK;
                case SkeletonRig.EYES: return RIG_EYETRACK;
                case SkeletonRig.HAND_LEFT: return RIG_HAND_LEFT;
                case SkeletonRig.HAND_RIGHT: return RIG_HAND_RIGHT;
                case SkeletonRig.FOOT_LEFT: return RIG_FOOT_LEFT;
                case SkeletonRig.FOOT_RIGHT: return RIG_FOOT_RIGHT;
            }

            return null;
        }
    }
}