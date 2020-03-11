// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using UnityEditor;
using UnityEngine;

namespace DoozyUI
{
    [CustomEditor(typeof(Soundy), true)]
    [DisallowMultipleComponent]
    public class SoundyEditor : QEditor
    {
        Soundy soundy { get { return (Soundy)target; } }

        SerializedProperty
            audioMixerGroup,
            masterVolume, numberOfChannels;

        float GlobalWidth { get { return DUI.GLOBAL_EDITOR_WIDTH; } }

        float tempFloat = 0;

        protected override void SerializedObjectFindProperties()
        {
            base.SerializedObjectFindProperties();

            audioMixerGroup = serializedObject.FindProperty("audioMixerGroup");
            masterVolume = serializedObject.FindProperty("masterVolume");
            numberOfChannels = serializedObject.FindProperty("numberOfChannels");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            requiresContantRepaint = true;
        }

        public override void OnInspectorGUI()
        {
            DrawHeader(DUIResources.headerSoundy.normal, WIDTH_420, HEIGHT_42);

            serializedObject.Update();

            DrawSettings(GlobalWidth);

            serializedObject.ApplyModifiedProperties();
            
            QUI.Space(SPACE_4);
        }

        void DrawSettings(float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, soundy.audioMixerGroup != null ? QColors.Color.Blue : QColors.Color.Gray), width, 20);
            QUI.Space(-20);

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_4);
                QLabel.text = "Audio Mixer Group";
                QLabel.style = Style.Text.Normal;
                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Space(1);
                    QUI.Label(QLabel);
                }
                QUI.EndVertical();
                QUI.PropertyField(audioMixerGroup, width - 8 - QLabel.x - 16);
                QUI.Space(SPACE_4);
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_4);

            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, soundy.audioMixerGroup != null ? QColors.Color.Blue : QColors.Color.Gray), width, 20);
            QUI.Space(-20);

            QLabel.style = Style.Text.Normal;
            QLabel.text = "Master Volume";
            tempFloat = QLabel.x; //save first label width
            QLabel.text = "Sound Channels";
            tempFloat += QLabel.x; //add second label width
            tempFloat += 8; //add extra space

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_4);

                QLabel.text = "Master Volume";
                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Space(1);
                    QUI.Label(QLabel);
                }
                QUI.EndVertical();

                EditorGUILayout.Slider(masterVolume, 0, 1, GUIContent.none, GUILayout.Width(140));

                QUI.FlexibleSpace();

                QLabel.text = "Sound Channels";
                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Space(1);
                    QUI.Label(QLabel);
                }
                QUI.EndVertical();

                QUI.PropertyField(numberOfChannels, 40);

                QUI.Space(SPACE_4);
            }
            QUI.EndHorizontal();

            if(numberOfChannels.intValue < 1) { numberOfChannels.intValue = 1; }
        }
    }
}
