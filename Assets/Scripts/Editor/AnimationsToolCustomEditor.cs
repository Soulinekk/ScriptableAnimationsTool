using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ParkingGame
{

    namespace AnimationTool
    {
        /*
        [CustomEditor(typeof(AnimationsToolSettings))]
        [CanEditMultipleObjects]
        public class AnimationsToolCustomEditor : Editor
        {
            AnimationsToolSettings animSettings;
            SerializedObject animSettingsSO;
            SerializedProperty calculatePosition;
            int positionsSteps;

            SerializedObject test;

            private void OnEnable()
            {
                if (animSettings != target)
                    animSettings = target as AnimationsToolSettings;
                animSettingsSO = new SerializedObject(targets);
                calculatePosition = animSettingsSO.FindProperty("calculatePosition");
            }

            public override void OnInspectorGUI()
            {
                //AnimationsToolSettings animSettings = (AnimationsToolSettings)target;
                //calculatePosition = EditorGUILayout.BeginToggleGroup("Animate position", calculatePosition);
                //EditorGUILayout.IntField("Animations steps", positionsSteps);
                //EditorGUILayout.EndToggleGroup();

                PositionAnimations();

                serializedObject.ApplyModifiedProperties();
            }

            private void PositionAnimations()
            {
                EditorGUILayout.PropertyField(calculatePosition);
                if (calculatePosition.boolValue)
                {
                    EditorGUILayout.IntField("Animation steps", positionsSteps);
                }
            }
        }*/
    }
}