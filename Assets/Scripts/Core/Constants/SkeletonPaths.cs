namespace TheArchitect.Core.Constants
{
    public enum SkeletonType {
        SMALL,
        MEDIUM,
        LARGE
    }

    public enum SkeletonRig {
        HEAD,
        EYES
    }
    
    public struct SkeletonPaths
    {
        public const string RIG_ROOT_TRACK = "ArmObject Track";
        public const string RIG_HEADTRACK = "Rig Head Track";
        public const string RIG_EYETRACK = "Rig Eye Track";
        public const string RIG_JAW_TALK = "Jaw Talk";
        public const string RIG_LIPS_TALK = "Lips Talk";

        public const string BONE_HIPS = "Hips";
        public const string BONE_SPINE = "Spine";
        public const string BONE_SPINE1 = "Spine1";
        public const string BONE_SPINE2 = "Spine2";
        public const string BONE_NECK = "Neck";
        public const string BONE_HEAD = "Head";
        public const string BONE_LEFT_EYE = "LeftEye";
        public const string BONE_RIGHT_EYE = "RightEye";

        public static readonly string PATH_ROOT = "ArmObject";
        public static readonly string PATH_HIPS = "ArmObject/Hips";
        public static readonly string PATH_SPINE = $"{PATH_HIPS}/Spine";
        public static readonly string PATH_SPINE1 = $"{PATH_SPINE}/Spine1";
        public static readonly string PATH_SPINE2 = $"{PATH_SPINE1}/Spine2";
        public static readonly string PATH_NECK = $"{PATH_SPINE2}/Neck";
        public static readonly string PATH_HEAD = $"{PATH_NECK}/Head";
        public static readonly string PATH_LEFT_EYE = $"{PATH_HEAD}/Grp_Head/LeftEye";
        public static readonly string PATH_RIGHT_EYE = $"{PATH_HEAD}/Grp_Head/RightEye";

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
            }

            return null;
        }

        public static string GetPathTo(SkeletonRig rig)
        {
            switch (rig) 
            {
                case SkeletonRig.HEAD: return RIG_HEADTRACK;
                case SkeletonRig.EYES: return RIG_EYETRACK;
            }

            return null;
        }
    }
}