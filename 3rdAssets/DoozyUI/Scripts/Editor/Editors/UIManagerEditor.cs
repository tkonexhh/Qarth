// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace DoozyUI
{
    [CustomEditor(typeof(UIManager), true)]
    [DisallowMultipleComponent]
    public class UIManagerEditor : QEditor
    {
        UIManager uiManager { get { return (UIManager)target; } }

        SerializedProperty
            debugGameEvents, debugUIButtons, debugUIElements, debugUINotifications, debugUICanvases,
            autoDisableButtonClicks,
            backButtonInputMode,
            enableBackButtonAlternateInputs,
            backButtonKeyCode, backButtonKeyCodeAlt,
            backButtonVirtualButtonName, backButtonVirtualButtonNameAlt;

        float GlobalWidth { get { return DUI.GLOBAL_EDITOR_WIDTH; } }

        protected override void SerializedObjectFindProperties()
        {
            base.SerializedObjectFindProperties();

            debugGameEvents = serializedObject.FindProperty("debugGameEvents");
            debugUIButtons = serializedObject.FindProperty("debugUIButtons");
            debugUIElements = serializedObject.FindProperty("debugUIElements");
            debugUINotifications = serializedObject.FindProperty("debugUINotifications");
            autoDisableButtonClicks = serializedObject.FindProperty("autoDisableButtonClicks");

            backButtonInputMode = serializedObject.FindProperty("backButtonInputMode");
            enableBackButtonAlternateInputs = serializedObject.FindProperty("enableBackButtonAlternateInputs");
            backButtonKeyCode = serializedObject.FindProperty("backButtonKeyCode");
            backButtonKeyCodeAlt = serializedObject.FindProperty("backButtonKeyCodeAlt");
            backButtonVirtualButtonName = serializedObject.FindProperty("backButtonVirtualButtonName");
            backButtonVirtualButtonNameAlt = serializedObject.FindProperty("backButtonVirtualButtonNameAlt");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            requiresContantRepaint = true;
        }

        public override void OnInspectorGUI()
        {
            DrawHeader(DUIResources.headerUIManager.normal, WIDTH_420, HEIGHT_42);

            serializedObject.Update();

            DrawTopButtons(GlobalWidth);
            QUI.Space(SPACE_4);

            DrawOrientationManagerButton(GlobalWidth);
            QUI.Space(SPACE_4);

            DrawDebugOptions(GlobalWidth);
            QUI.Space(SPACE_4);

            DrawSettings(GlobalWidth);

            if(EditorApplication.isPlaying)
            {
                QUI.Space(SPACE_8);
                DrawBackButtonStatus(GlobalWidth);
                QUI.Space(SPACE_2);
                DrawButtonClicksStatus(GlobalWidth);
            }

            serializedObject.ApplyModifiedProperties();

            QUI.Space(SPACE_4);
        }

        void DrawTopButtons(float width)
        {
            QUI.BeginHorizontal(width);
            {
                if(QUI.GhostButton("Control Panel", QColors.Color.Gray, (width - SPACE_4) / 3, 18))
                {
                    ControlPanelWindow.OpenWindow(ControlPanelWindow.Page.General);
                }
                if(QUI.GhostButton("Editor Settings", QColors.Color.Gray, (width - SPACE_4) / 3, 18))
                {
                    ControlPanelWindow.OpenWindow(ControlPanelWindow.Page.EditorSettings);
                }
                if(QUI.GhostButton("Help", QColors.Color.Gray, (width - SPACE_4) / 3, 18))
                {
                    ControlPanelWindow.OpenWindow(ControlPanelWindow.Page.Help);
                }
            }
            QUI.EndHorizontal();
        }

        void DrawOrientationManagerButton(float width)
        {
            if(!UIManager.useOrientationManager) { return; }
            if(QUI.GhostButton("Add Orientation Manager to Scene", QColors.Color.Gray, width, 18))
            {
                OrientationManager.AddOrientationManagerToScene();
            }
        }

        void DrawDebugOptions(float width)
        {
            QUI.BeginHorizontal(width);
            {
                QUI.LabelWithBackground("Debug");
                QUI.FlexibleSpace();
                QUI.QToggle("GameEvents", debugGameEvents, Style.Text.Small);
                QUI.FlexibleSpace();
                QUI.QToggle("UIButtons", debugUIButtons, Style.Text.Small);
                QUI.FlexibleSpace();
                QUI.QToggle("UIElements", debugUIElements, Style.Text.Small);
                QUI.FlexibleSpace();
                QUI.QToggle("UINotifications", debugUINotifications, Style.Text.Small);
            }
            QUI.EndHorizontal();
        }

        void DrawSettings(float width)
        {
            QUI.QToggle("Auto disable Button Clicks when an UIElement is in trasition", autoDisableButtonClicks);

            QUI.Space(SPACE_4);

            //CONTROLLER INPUT MODE
            QUI.BeginHorizontal(width);
            {
                if((ControllerInputMode)backButtonInputMode.enumValueIndex == ControllerInputMode.None)
                {
                    QUI.SetGUIBackgroundColor(QUI.AccentColorRed);
                }
                QUI.QObjectPropertyField("'Back' button Input Mode", backButtonInputMode, 260, 20, false);
                QUI.ResetColors();
                QUI.Space(SPACE_2);
                if((ControllerInputMode)backButtonInputMode.enumValueIndex != ControllerInputMode.None)
                {
                    QUI.QToggle("enable alternate inputs", enableBackButtonAlternateInputs);
                }
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
            if((ControllerInputMode)backButtonInputMode.enumValueIndex != ControllerInputMode.None)
            {
                QUI.BeginHorizontal(width);
                {
                    if((ControllerInputMode)backButtonInputMode.enumValueIndex == ControllerInputMode.KeyCode)
                    {
                        QUI.QObjectPropertyField("Key Code", backButtonKeyCode, width / 2 - 1, 20, false);
                        QUI.Space(SPACE_2);
                        GUI.enabled = enableBackButtonAlternateInputs.boolValue;
                        if(!enableBackButtonAlternateInputs.boolValue)
                        {
                            QUI.SetGUIBackgroundColor(QUI.AccentColorRed);
                        }
                        QUI.QObjectPropertyField("Alternate", backButtonKeyCodeAlt, width / 2 - 1, 20, false);
                        QUI.ResetColors();
                        GUI.enabled = true;
                    }
                    else if((ControllerInputMode)backButtonInputMode.enumValueIndex == ControllerInputMode.VirtualButton)
                    {
                        QUI.QObjectPropertyField("Virtual Button", backButtonVirtualButtonName, width / 2 - 1, 20, false);
                        QUI.Space(SPACE_2);
                        GUI.enabled = enableBackButtonAlternateInputs.boolValue;
                        if(!enableBackButtonAlternateInputs.boolValue)
                        {
                            QUI.SetGUIBackgroundColor(QUI.AccentColorRed);
                        }
                        QUI.QObjectPropertyField("Alternate", backButtonVirtualButtonNameAlt, width / 2 - 1, 20, false);
                        QUI.ResetColors();
                        GUI.enabled = true;
                    }
                    QUI.FlexibleSpace();
                }
                QUI.EndHorizontal();
            }
        }

        void DrawBackButtonStatus(float width)
        {
            QUI.LabelWithBackground("The 'Back' button is " + (UIManager.Instance.BackButtonDisabled ? "DISABLED" : "ENABLED"));
        }

        void DrawButtonClicksStatus(float width)
        {
            QUI.LabelWithBackground("Button clicks are " + (UIManager.Instance.ButtonClicksDisabled ? "DISABLED" : "ENABLED"));
        }
    }
}
