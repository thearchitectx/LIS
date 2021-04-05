// using UnityEditor;
// using UnityEditor.Animations;
// using UnityEngine;
// using UnityEngine.Animations.Rigging;
// using TheArchitect.Core.Constants;

// public class UniversalAnimatorUpdater: EditorWindow
// {
//     string[] idles = {
//         "Assets/Characters/Kate/Animation/AN_S_idle_pray.anim",
//         "Assets/Characters/Kate/Animation/AN_S_idle_uneasy.anim",
//     };

//     string[] reacts = {
//         "Assets/Characters/Kate/Animation/AN_S_react_surprise.anim",
//     };

//     [MenuItem("Window/Utilities/Universal Animator Generator")]
//     public static void FindMissingScripts()
//     {
//         EditorWindow.GetWindow(typeof(UniversalAnimatorUpdater));
//     }

//     void OnGUI()
//     {
//         GameObject character = Selection.activeGameObject;

//         if (GUILayout.Button("GENERATE UNIVERSAL ANIMATOR"))
//         {
//             var a = AnimatorController.CreateAnimatorControllerAtPath("Assets/Characters/_Shared/BaseCharacterAnimator.controller");

//             a.layers[0].name = "Idle";
//             var idleStateMachine = a.layers[0].stateMachine;
//             var stateStart = idleStateMachine.AddState("start");
//             var stateIdleHub = idleStateMachine.AddState("idle-hub");
            
//             foreach (var anim in idles)
//             {
//                 AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(anim);
//                 string clipName = anim.Substring(anim.LastIndexOf("/")+1);
//                 clipName = clipName.Replace(".anim", "");
//                 string triggerName = clipName.Substring(clipName.LastIndexOf("_")+1);

//                 a.AddParameter(triggerName, AnimatorControllerParameterType.Trigger);
//                 var state = idleStateMachine.AddState(triggerName);
//                 state.motion = clip;

//                 // From start
//                 var transition = new AnimatorStateTransition() {
//                     duration = 0,
//                     destinationState = state,
//                     hasExitTime = false,
//                 };
//                 transition.AddCondition(AnimatorConditionMode.Equals, 1, triggerName);
//                 stateStart.AddTransition(transition);

//                 // From idle-hub
//                 transition = new AnimatorStateTransition() {
//                     duration = 0.25f,
//                     destinationState = state,
//                     hasExitTime = false,
//                 };
//                 transition.AddCondition(AnimatorConditionMode.Equals, 1, triggerName);
//                 stateIdleHub.AddTransition(transition);

//                 // to idle-hub
//                 transition = new AnimatorStateTransition() {
//                     duration = 0,
//                     destinationState = stateIdleHub,
//                     hasExitTime = false,
//                 };
//                 transition.AddCondition(AnimatorConditionMode.Equals, 1, triggerName);
//                 state.AddTransition(transition);
//             }
            
//         }
//     }

// }