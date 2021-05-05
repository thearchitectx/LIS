using UnityEngine;
using TheArchitect.Core.Constants;
namespace TheArchitect.Core.Controllers
{

    public class CharacterInitializer : MonoBehaviour
    {
        public const int DEFAULT_BLEND_SHAPE_ENHANCED = 100;
        public const string PREF_BLEND_SHAPE_ENHANCED = "CharacterInitializer.BLEND_SHAPE_ENHANCED";

        public const string PREF_BREAST_STIFFNESS = "CharacterInitializer.BREAST_STIFFNESS";
        public const float DEFAULT_BREAST_STIFFNESS = 0.19f;
        public const string PREF_BREAST_DAMPING = "CharacterInitializer.BREAST_DAMPING";
        public const float DEFAULT_BREAST_DAMPING = 0.09f;
        public const string PREF_BREAST_MASS = "CharacterInitializer.BREAST_MASS";
        public const float DEFAULT_BREAST_MASS = 2f;

        void Start()
        {
            var renderers = this.GetComponentsInChildren<SkinnedMeshRenderer>();

            var weight = PlayerPrefs.GetInt(PREF_BLEND_SHAPE_ENHANCED, DEFAULT_BLEND_SHAPE_ENHANCED);
            foreach (var r in renderers)
                ApplyBlendShape(this.transform, SkeletonPaths.BLENDSHAPE_ENHANCED, weight);
            
            var stiffness = PlayerPrefs.GetFloat(PREF_BREAST_STIFFNESS, DEFAULT_BREAST_STIFFNESS);
            var damping = PlayerPrefs.GetFloat(PREF_BREAST_DAMPING, DEFAULT_BREAST_DAMPING);
            var mass = PlayerPrefs.GetFloat(PREF_BREAST_MASS, DEFAULT_BREAST_MASS);

            SetBreastPhysicsParams(this.transform, stiffness, damping, mass);

            Destroy(this);
        }

        void Update()
        {
            Destroy(this);
        }
        
        public static void ApplyBlendShape(Transform characterRoot, string blendShapeName, int weight)
        {
            foreach (var t in characterRoot.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                int blendShapeIndex = t.sharedMesh.GetBlendShapeIndex(blendShapeName);
                if (blendShapeIndex >= 0)
                    t.SetBlendShapeWeight(blendShapeIndex, weight);
            }
        }


        public static void SetBreastPhysicsParams(Transform characterRoot, float? stiffness, float? damping, float? mass)
        {
            var boobTransform = characterRoot.Find(SkeletonPaths.PATH_LEFT_BOOB);
            var elasticBone = boobTransform != null ? boobTransform.GetComponent<ElasticHandleBone>() : null;
            if (elasticBone!=null && stiffness.HasValue)
                elasticBone.Stiffness = stiffness.Value;
            if (elasticBone!=null && damping.HasValue)
                elasticBone.Damping = damping.Value;
            if (elasticBone!=null && mass.HasValue)
                elasticBone.Mass = mass.Value;

            boobTransform = characterRoot.Find(SkeletonPaths.PATH_RIGHT_BOOB);
            elasticBone = boobTransform != null ? boobTransform.GetComponent<ElasticHandleBone>() : null;
            if (elasticBone!=null && stiffness.HasValue)
                elasticBone.Stiffness = stiffness.Value;
            if (elasticBone!=null && damping.HasValue)
                elasticBone.Damping = damping.Value;
            if (elasticBone!=null && mass.HasValue)
                elasticBone.Mass = mass.Value;
        }
        
    }

}