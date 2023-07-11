// ******------------------------------------------------------******
// BewerageMakerEditor.cs
//
// Author:
//       K.Sinan Acar <ksa@puzzledwizard.com>
//
// Copyright (c) 2019 PuzzledWizard
//
// ******------------------------------------------------------******

using UnityEditor;
using UnityEngine;

namespace PW
{
    [CustomEditor(typeof(BewerageMaker))]
    [CanEditMultipleObjects]
    public class BewerageMakerEditor : Editor
    {
        private SerializedProperty cupType;
        private SerializedProperty fillCupSpot;
        private SerializedProperty progressHelperprefab;

        private bool showAnimationSettings;


        private void OnEnable()
        {
            useAnimation = serializedObject.FindProperty("useAnimation");
            preFillAnimationStateName = serializedObject.FindProperty("preFillAnimationStateName");
            fillEndedAnimationState = serializedObject.FindProperty("fillEndedAnimationState");
            preFillProcess = serializedObject.FindProperty("preFillProcess");
            fillingProcess = serializedObject.FindProperty("fillingProcess");
            cupType = serializedObject.FindProperty("cupType");
            progressHelperprefab = serializedObject.FindProperty("progressHelperprefab");
            fillCupSpot = serializedObject.FindProperty("fillCupSpot");
            dummyAnimationTarget = serializedObject.FindProperty("dummyAnimationTarget");
            fillParticle = serializedObject.FindProperty("fillParticle");
            useTweeningAnimation = serializedObject.FindProperty("useTweeningAnimation");
            finalTweenTarget = serializedObject.FindProperty("finalTweenTarget");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(cupType);
            EditorGUILayout.PropertyField(progressHelperprefab);
            EditorGUILayout.PropertyField(fillCupSpot);
            EditorGUILayout.PropertyField(useAnimation);
            //Animation Settings FoldOut
            //Find out that do we even need animation settings for this object?
            showAnimationSettings = EditorGUILayout.Foldout(showAnimationSettings, animStatus);
            if (showAnimationSettings)
                if (useAnimation.boolValue)
                {
                    EditorGUILayout.PropertyField(useTweeningAnimation);

                    EditorGUILayout.PropertyField(dummyAnimationTarget,
                        new GUIContent("Dummy Animation target to use"));


                    if (useTweeningAnimation.boolValue) EditorGUILayout.PropertyField(finalTweenTarget);

                    OnInspectorAdvancedAnimationSettings();
                }

            if (!useAnimation.boolValue) showAnimationSettings = false;

            serializedObject.ApplyModifiedProperties();
        }

        public void OnInspectorUpdate()
        {
            Repaint();
        }

        public void OnInspectorAdvancedAnimationSettings()
        {
            //Prefill Settings 

            EditorGUILayout.PropertyField(preFillProcess, new GUIContent("PreFill Duration"));

            if (!useTweeningAnimation.boolValue)
                EditorGUILayout.PropertyField(preFillAnimationStateName, new GUIContent("PreFill Animation State"));


            //Filling Settings 

            EditorGUILayout.PropertyField(fillingProcess, new GUIContent("Filling Duration"));
            EditorGUILayout.PropertyField(fillParticle, new GUIContent("Filling Animation particle"));

            if (!useTweeningAnimation.boolValue)
                EditorGUILayout.PropertyField(fillEndedAnimationState, new GUIContent("Fıll Ended Animation State"));
        }

        #region AnimationProperties

        private readonly string animStatus = "Animations Settings";


        private SerializedProperty useAnimation;

        private SerializedProperty preFillAnimationStateName;

        private SerializedProperty fillEndedAnimationState;


        private SerializedProperty preFillProcess;
        private SerializedProperty fillingProcess;


        private SerializedProperty dummyAnimationTarget;

        private SerializedProperty fillParticle;

        private SerializedProperty useTweeningAnimation;
        private SerializedProperty finalTweenTarget;

        #endregion
    }
}