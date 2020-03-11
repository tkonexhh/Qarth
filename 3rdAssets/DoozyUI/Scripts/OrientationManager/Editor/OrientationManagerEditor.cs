// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DoozyUI
{
    [CustomEditor(typeof(OrientationManager), true)]
    [DisallowMultipleComponent]
    public class OrientationManagerEditor : QEditor
    {
        OrientationManager orientationManager { get { return (OrientationManager)target; } }

        SerializedProperty
           onOrientationChange;

        float GlobalWidth { get { return DUI.GLOBAL_EDITOR_WIDTH; } }

        protected override void SerializedObjectFindProperties()
        {
            base.SerializedObjectFindProperties();

            onOrientationChange = serializedObject.FindProperty("onOrientationChange");
        }

        protected override void GenerateInfoMessages()
        {
            base.GenerateInfoMessages();

            infoMessage = new Dictionary<string, InfoMessage>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            requiresContantRepaint = true;

            UpdateOrientationInEditMode();
        }

        public override void OnInspectorGUI()
        {
            DrawHeader(DUIResources.headerOrientationManager.texture, WIDTH_420, HEIGHT_42);
            serializedObject.Update();
            DrawOrientation(GlobalWidth);
            serializedObject.ApplyModifiedProperties();
            QUI.Space(SPACE_4);
        }

        void DrawOrientation(float width)
        {

            QStyles.GetStyle(QStyles.GetStyleName(Style.GhostButton.Purple)).alignment = TextAnchor.MiddleRight;
            if(QUI.GhostButton("update orientation", QColors.Color.Purple, width, 32, 10))
            {
                UpdateOrientationInEditMode();
            }
            QStyles.GetStyle(QStyles.GetStyleName(Style.GhostButton.Purple)).alignment = TextAnchor.MiddleCenter;

            QUI.Space(-32);

            switch(orientationManager.CurrentOrientation)
            {
                case OrientationManager.Orientation.Landscape: QUI.DrawTexture(DUIResources.miniIconOrientationLandscape.texture, 160, 32); break;
                case OrientationManager.Orientation.Portrait: QUI.DrawTexture(DUIResources.miniIconOrientationPortrait.texture, 160, 32); break;
                case OrientationManager.Orientation.Unknown: QUI.DrawTexture(DUIResources.miniIconOrientationUnknown.texture, 160, 32); break;
            }

            QUI.Space(SPACE_2);

            QUI.SetGUIBackgroundColor(QUI.AccentColorPurple);
            QUI.PropertyField(onOrientationChange, true, new GUIContent("OnOrientationChange"), width);
            QUI.ResetColors();
        }

        void UpdateOrientationInEditMode()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) //Update the orientation in EditMode only, not when in PlayMode
            {
                //Debug.Log("[OrientationManager] Cannot manually trigger an orientation update while in PlayMode. The system handles that for you automatically.");
                return;
            } 

            //PORTRAIT
            if (orientationManager.RectTransform.rect.width < orientationManager.RectTransform.rect.height)
            {
                if (orientationManager.CurrentOrientation != OrientationManager.Orientation.Portrait) //Orientation changed to PORTRAIT
                {
                    orientationManager.ChangeOrientation(OrientationManager.Orientation.Portrait);
                }
            }

            //LANDSCAPE
            else
            {
                if (orientationManager.CurrentOrientation != OrientationManager.Orientation.Landscape) //Orientation changed to LANDSCAPE
                {
                    orientationManager.ChangeOrientation(OrientationManager.Orientation.Landscape);
                }
            }
        }
    }
}
