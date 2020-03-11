// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using DoozyUI.Internal;
using QuickEditor;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace DoozyUI
{
    [CustomEditor(typeof(UIToggle), true)]
    [DisallowMultipleComponent]
    [CanEditMultipleObjects]
    public class UIToggleEditor : QEditor
    {
        UIToggle uiToggle { get { return (UIToggle)target; } }
        DUIData.PunchDatabase DatabasePunchAnimations { get { return DUIData.Instance.DatabasePunchAnimations; } }
        DUIData.SoundsDatabase DatabaseUISounds { get { return DUIData.Instance.DatabaseUISounds; } }

        DUISettings EditorSettings { get { return DUISettings.Instance; } }

#pragma warning disable 0414

        SerializedProperty
            allowMultipleClicks, disableButtonInterval,
            deselectButtonOnClick,
            controllerInputMode, enableAlternateInputs,
            onClickKeyCode, onClickKeyCodeAlt,
            onClickVirtualButtonName, onClickVirtualButtonNameAlt,

            useOnPointerEnter, onPointerEnterDisableInterval,
            onPointerEnterSoundToggleOn, onPointerEnterSoundToggleOff,
            customOnPointerEnterSoundToggleOn, customOnPointerEnterSoundToggleOff,
            OnPointerEnterToggleOn, OnPointerEnterToggleOff,
            onPointerEnterPunchPresetCategory, onPointerEnterPunchPresetName, loadOnPointerEnterPunchPresetAtRuntime,
            onPointerEnterPunch,
            onPointerEnterPunchMove, onPointerEnterPunchMoveEnabled, onPointerEnterPunchMovePunch, onPointerEnterPunchMoveStartDelay, onPointerEnterPunchMoveDuration, onPointerEnterPunchMoveVibrato, onPointerEnterPunchMoveElasticity,
            onPointerEnterPunchRotate, onPointerEnterPunchRotateEnabled, onPointerEnterPunchRotatePunch, onPointerEnterPunchRotateStartDelay, onPointerEnterPunchRotateDuration, onPointerEnterPunchRotateVibrato, onPointerEnterPunchRotateElasticity,
            onPointerEnterPunchScale, onPointerEnterPunchScaleEnabled, onPointerEnterPunchScalePunch, onPointerEnterPunchScaleStartDelay, onPointerEnterPunchScaleDuration, onPointerEnterPunchScaleVibrato, onPointerEnterPunchScaleElasticity,
            onPointerEnterGameEventsToggleOn, onPointerEnterGameEventsToggleOff,

            useOnPointerExit, onPointerExitDisableInterval,
            onPointerExitSoundToggleOn, onPointerExitSoundToggleOff,
            customOnPointerExitSoundToggleOn, customOnPointerExitSoundToggleOff,
            OnPointerExitToggleOn, OnPointerExitToggleOff,
            onPointerExitPunchPresetCategory, onPointerExitPunchPresetName, loadOnPointerExitPunchPresetAtRuntime,
            onPointerExitPunch,
            onPointerExitPunchMove, onPointerExitPunchMoveEnabled, onPointerExitPunchMovePunch, onPointerExitPunchMoveStartDelay, onPointerExitPunchMoveDuration, onPointerExitPunchMoveVibrato, onPointerExitPunchMoveElasticity,
            onPointerExitPunchRotate, onPointerExitPunchRotateEnabled, onPointerExitPunchRotatePunch, onPointerExitPunchRotateStartDelay, onPointerExitPunchRotateDuration, onPointerExitPunchRotateVibrato, onPointerExitPunchRotateElasticity,
            onPointerExitPunchScale, onPointerExitPunchScaleEnabled, onPointerExitPunchScalePunch, onPointerExitPunchScaleStartDelay, onPointerExitPunchScaleDuration, onPointerExitPunchScaleVibrato, onPointerExitPunchScaleElasticity,
            onPointerExitGameEventsToggleOn, onPointerExitGameEventsToggleOff,

            useOnClick, waitForOnClick,
            onClickSoundToggleOn, onClickSoundToggleOff,
            customOnClickSoundToggleOn, customOnClickSoundToggleOff,
            OnClickToggleOn, OnClickToggleOff,
            onClickPunchPresetCategory, onClickPunchPresetName, loadOnClickPunchPresetAtRuntime,
            onClickPunch,
            onClickPunchMove, onClickPunchMoveEnabled, onClickPunchMovePunch, onClickPunchMoveStartDelay, onClickPunchMoveDuration, onClickPunchMoveVibrato, onClickPunchMoveElasticity,
            onClickPunchRotate, onClickPunchRotateEnabled, onClickPunchRotatePunch, onClickPunchRotateStartDelay, onClickPunchRotateDuration, onClickPunchRotateVibrato, onClickPunchRotateElasticity,
            onClickPunchScale, onClickPunchScaleEnabled, onClickPunchScalePunch, onClickPunchScaleStartDelay, onClickPunchScaleDuration, onClickPunchScaleVibrato, onClickPunchScaleElasticity,
            onClickGameEventsToggleOn, onClickGameEventsToggleOff;

        AnimBool
           showOnPointerEnter, showOnPointerEnterPreset, showOnPointerEnterPunchMove, showOnPointerEnterPunchRotate, showOnPointerEnterPunchScale, showOnPointerEnterEvents, showOnPointerEnterGameEvents, showOnPointerEnterNavigation,
           showOnPointerExit, showOnPointerExitPreset, showOnPointerExitPunchMove, showOnPointerExitPunchRotate, showOnPointerExitPunchScale, showOnPointerExitEvents, showOnPointerExitGameEvents, showOnPointerExitNavigation,
           showOnClick, showOnClickPreset, showOnClickPunchMove, showOnClickPunchRotate, showOnClickPunchScale, showOnClickEvents, showOnClickGameEvents, showOnClickNavigation;

#pragma warning restore 0414

        Index onPointerEnterSoundIndexToggleOn = new Index();
        Index onPointerEnterSoundIndexToggleOff = new Index();
        Index onPointerExitSoundIndexToggleOn = new Index();
        Index onPointerExitSoundIndexToggleOff = new Index();
        Index onClickSoundIndexToggleOn = new Index();
        Index onClickSoundIndexToggleOff = new Index();

        Name newPresetCategoryName = new Name();
        Name newPresetName = new Name();

        AnimBool createNewCategoryName;

        Index onPointerEnterPunchPresetCategoryNameIndex = new Index();
        Index onPointerEnterPunchPresetNameIndex = new Index();
        AnimBool onPointerEnterPunchNewPreset;

        Index onPointerExitPunchPresetCategoryNameIndex = new Index();
        Index onPointerExitPunchPresetNameIndex = new Index();
        AnimBool onPointerExitPunchNewPreset;

        Index onClickPunchPresetCategoryNameIndex = new Index();
        Index onClickPunchPresetNameIndex = new Index();
        AnimBool onClickPunchNewPreset;

        EditorNavigationPointerData onPointerEnterEditorNavigationDataToggleOn = new EditorNavigationPointerData();
        EditorNavigationPointerData onPointerEnterEditorNavigationDataToggleOff = new EditorNavigationPointerData();
        EditorNavigationPointerData onPointerExitEditorNavigationDataToggleOn = new EditorNavigationPointerData();
        EditorNavigationPointerData onPointerExitEditorNavigationDataToggleOff = new EditorNavigationPointerData();
        EditorNavigationPointerData onClickEditorNavigationDataToggleOn = new EditorNavigationPointerData();
        EditorNavigationPointerData onClickEditorNavigationDataToggleOff = new EditorNavigationPointerData();

        float GlobalWidth { get { return DUI.GLOBAL_EDITOR_WIDTH; } }
        int BarHeight { get { return DUI.BAR_HEIGHT; } }
        int MiniBarHeight { get { return DUI.MINI_BAR_HEIGHT; } }

        float tempFloat = 0;
        bool tempBool = false;

        enum AnimType { OnPointerEnter, OnPointerExit, OnClick }

        enum NavigationType { Show, Hide }

        protected override void SerializedObjectFindProperties()
        {
            base.SerializedObjectFindProperties();

            #region Settings
            allowMultipleClicks = serializedObject.FindProperty("allowMultipleClicks");
            disableButtonInterval = serializedObject.FindProperty("disableButtonInterval");
            deselectButtonOnClick = serializedObject.FindProperty("deselectButtonOnClick");
            controllerInputMode = serializedObject.FindProperty("controllerInputMode");
            enableAlternateInputs = serializedObject.FindProperty("enableAlternateInputs");
            onClickKeyCode = serializedObject.FindProperty("onClickKeyCode");
            onClickKeyCodeAlt = serializedObject.FindProperty("onClickKeyCodeAlt");
            onClickVirtualButtonName = serializedObject.FindProperty("onClickVirtualButtonName");
            onClickVirtualButtonNameAlt = serializedObject.FindProperty("onClickVirtualButtonNameAlt");
            #endregion
            #region PointerEnter
            useOnPointerEnter = serializedObject.FindProperty("useOnPointerEnter");
            onPointerEnterDisableInterval = serializedObject.FindProperty("onPointerEnterDisableInterval");
            onPointerEnterSoundToggleOn = serializedObject.FindProperty("onPointerEnterSoundToggleOn");
            onPointerEnterSoundToggleOff = serializedObject.FindProperty("onPointerEnterSoundToggleOff");
            customOnPointerEnterSoundToggleOn = serializedObject.FindProperty("customOnPointerEnterSoundToggleOn");
            customOnPointerEnterSoundToggleOff = serializedObject.FindProperty("customOnPointerEnterSoundToggleOff");
            OnPointerEnterToggleOn = serializedObject.FindProperty("OnPointerEnterToggleOn");
            OnPointerEnterToggleOff = serializedObject.FindProperty("OnPointerEnterToggleOff");
            onPointerEnterPunchPresetCategory = serializedObject.FindProperty("onPointerEnterPunchPresetCategory");
            onPointerEnterPunchPresetName = serializedObject.FindProperty("onPointerEnterPunchPresetName");
            loadOnPointerEnterPunchPresetAtRuntime = serializedObject.FindProperty("loadOnPointerEnterPunchPresetAtRuntime");
            onPointerEnterPunch = serializedObject.FindProperty("onPointerEnterPunch");
            onPointerEnterPunchMove = onPointerEnterPunch.FindPropertyRelative("move");
            onPointerEnterPunchMoveEnabled = onPointerEnterPunchMove.FindPropertyRelative("enabled");
            onPointerEnterPunchMovePunch = onPointerEnterPunchMove.FindPropertyRelative("punch");
            onPointerEnterPunchMoveStartDelay = onPointerEnterPunchMove.FindPropertyRelative("startDelay");
            onPointerEnterPunchMoveDuration = onPointerEnterPunchMove.FindPropertyRelative("duration");
            onPointerEnterPunchMoveVibrato = onPointerEnterPunchMove.FindPropertyRelative("vibrato");
            onPointerEnterPunchMoveElasticity = onPointerEnterPunchMove.FindPropertyRelative("elasticity");
            onPointerEnterPunchRotate = onPointerEnterPunch.FindPropertyRelative("rotate");
            onPointerEnterPunchRotateEnabled = onPointerEnterPunchRotate.FindPropertyRelative("enabled");
            onPointerEnterPunchRotatePunch = onPointerEnterPunchRotate.FindPropertyRelative("punch");
            onPointerEnterPunchRotateStartDelay = onPointerEnterPunchRotate.FindPropertyRelative("startDelay");
            onPointerEnterPunchRotateDuration = onPointerEnterPunchRotate.FindPropertyRelative("duration");
            onPointerEnterPunchRotateVibrato = onPointerEnterPunchRotate.FindPropertyRelative("vibrato");
            onPointerEnterPunchRotateElasticity = onPointerEnterPunchRotate.FindPropertyRelative("elasticity");
            onPointerEnterPunchScale = onPointerEnterPunch.FindPropertyRelative("scale");
            onPointerEnterPunchScaleEnabled = onPointerEnterPunchScale.FindPropertyRelative("enabled");
            onPointerEnterPunchScalePunch = onPointerEnterPunchScale.FindPropertyRelative("punch");
            onPointerEnterPunchScaleStartDelay = onPointerEnterPunchScale.FindPropertyRelative("startDelay");
            onPointerEnterPunchScaleDuration = onPointerEnterPunchScale.FindPropertyRelative("duration");
            onPointerEnterPunchScaleVibrato = onPointerEnterPunchScale.FindPropertyRelative("vibrato");
            onPointerEnterPunchScaleElasticity = onPointerEnterPunchScale.FindPropertyRelative("elasticity");
            onPointerEnterGameEventsToggleOn = serializedObject.FindProperty("onPointerEnterGameEventsToggleOn");
            onPointerEnterGameEventsToggleOff = serializedObject.FindProperty("onPointerEnterGameEventsToggleOff");
            #endregion
            #region PointerExit
            useOnPointerExit = serializedObject.FindProperty("useOnPointerExit");
            onPointerExitDisableInterval = serializedObject.FindProperty("onPointerExitDisableInterval");
            onPointerExitSoundToggleOn = serializedObject.FindProperty("onPointerExitSoundToggleOn");
            onPointerExitSoundToggleOff = serializedObject.FindProperty("onPointerExitSoundToggleOff");
            customOnPointerExitSoundToggleOn = serializedObject.FindProperty("customOnPointerExitSoundToggleOn");
            customOnPointerExitSoundToggleOff = serializedObject.FindProperty("customOnPointerExitSoundToggleOff");
            OnPointerExitToggleOn = serializedObject.FindProperty("OnPointerExitToggleOn");
            OnPointerExitToggleOff = serializedObject.FindProperty("OnPointerExitToggleOff");
            onPointerExitPunchPresetCategory = serializedObject.FindProperty("onPointerExitPunchPresetCategory");
            onPointerExitPunchPresetName = serializedObject.FindProperty("onPointerExitPunchPresetName");
            loadOnPointerExitPunchPresetAtRuntime = serializedObject.FindProperty("loadOnPointerExitPunchPresetAtRuntime");
            onPointerExitPunch = serializedObject.FindProperty("onPointerExitPunch");
            onPointerExitPunchMove = onPointerExitPunch.FindPropertyRelative("move");
            onPointerExitPunchMoveEnabled = onPointerExitPunchMove.FindPropertyRelative("enabled");
            onPointerExitPunchMovePunch = onPointerExitPunchMove.FindPropertyRelative("punch");
            onPointerExitPunchMoveStartDelay = onPointerExitPunchMove.FindPropertyRelative("startDelay");
            onPointerExitPunchMoveDuration = onPointerExitPunchMove.FindPropertyRelative("duration");
            onPointerExitPunchMoveVibrato = onPointerExitPunchMove.FindPropertyRelative("vibrato");
            onPointerExitPunchMoveElasticity = onPointerExitPunchMove.FindPropertyRelative("elasticity");
            onPointerExitPunchRotate = onPointerExitPunch.FindPropertyRelative("rotate");
            onPointerExitPunchRotateEnabled = onPointerExitPunchRotate.FindPropertyRelative("enabled");
            onPointerExitPunchRotatePunch = onPointerExitPunchRotate.FindPropertyRelative("punch");
            onPointerExitPunchRotateStartDelay = onPointerExitPunchRotate.FindPropertyRelative("startDelay");
            onPointerExitPunchRotateDuration = onPointerExitPunchRotate.FindPropertyRelative("duration");
            onPointerExitPunchRotateVibrato = onPointerExitPunchRotate.FindPropertyRelative("vibrato");
            onPointerExitPunchRotateElasticity = onPointerExitPunchRotate.FindPropertyRelative("elasticity");
            onPointerExitPunchScale = onPointerExitPunch.FindPropertyRelative("scale");
            onPointerExitPunchScaleEnabled = onPointerExitPunchScale.FindPropertyRelative("enabled");
            onPointerExitPunchScalePunch = onPointerExitPunchScale.FindPropertyRelative("punch");
            onPointerExitPunchScaleStartDelay = onPointerExitPunchScale.FindPropertyRelative("startDelay");
            onPointerExitPunchScaleDuration = onPointerExitPunchScale.FindPropertyRelative("duration");
            onPointerExitPunchScaleVibrato = onPointerExitPunchScale.FindPropertyRelative("vibrato");
            onPointerExitPunchScaleElasticity = onPointerExitPunchScale.FindPropertyRelative("elasticity");
            onPointerExitGameEventsToggleOn = serializedObject.FindProperty("onPointerExitGameEventsToggleOn");
            onPointerExitGameEventsToggleOff = serializedObject.FindProperty("onPointerExitGameEventsToggleOff");
            #endregion
            #region OnClick
            useOnClick = serializedObject.FindProperty("useOnClick");
            waitForOnClick = serializedObject.FindProperty("waitForOnClick");
            onClickSoundToggleOn = serializedObject.FindProperty("onClickSoundToggleOn");
            onClickSoundToggleOff = serializedObject.FindProperty("onClickSoundToggleOff");
            customOnClickSoundToggleOn = serializedObject.FindProperty("customOnClickSoundToggleOn");
            customOnClickSoundToggleOff = serializedObject.FindProperty("customOnClickSoundToggleOff");
            OnClickToggleOn = serializedObject.FindProperty("OnClickToggleOn");
            OnClickToggleOff = serializedObject.FindProperty("OnClickToggleOff");
            onClickPunchPresetCategory = serializedObject.FindProperty("onClickPunchPresetCategory");
            onClickPunchPresetName = serializedObject.FindProperty("onClickPunchPresetName");
            loadOnClickPunchPresetAtRuntime = serializedObject.FindProperty("loadOnClickPunchPresetAtRuntime");
            onClickPunch = serializedObject.FindProperty("onClickPunch");
            onClickPunchMove = onClickPunch.FindPropertyRelative("move");
            onClickPunchMoveEnabled = onClickPunchMove.FindPropertyRelative("enabled");
            onClickPunchMovePunch = onClickPunchMove.FindPropertyRelative("punch");
            onClickPunchMoveStartDelay = onClickPunchMove.FindPropertyRelative("startDelay");
            onClickPunchMoveDuration = onClickPunchMove.FindPropertyRelative("duration");
            onClickPunchMoveVibrato = onClickPunchMove.FindPropertyRelative("vibrato");
            onClickPunchMoveElasticity = onClickPunchMove.FindPropertyRelative("elasticity");
            onClickPunchRotate = onClickPunch.FindPropertyRelative("rotate");
            onClickPunchRotateEnabled = onClickPunchRotate.FindPropertyRelative("enabled");
            onClickPunchRotatePunch = onClickPunchRotate.FindPropertyRelative("punch");
            onClickPunchRotateStartDelay = onClickPunchRotate.FindPropertyRelative("startDelay");
            onClickPunchRotateDuration = onClickPunchRotate.FindPropertyRelative("duration");
            onClickPunchRotateVibrato = onClickPunchRotate.FindPropertyRelative("vibrato");
            onClickPunchRotateElasticity = onClickPunchRotate.FindPropertyRelative("elasticity");
            onClickPunchScale = onClickPunch.FindPropertyRelative("scale");
            onClickPunchScaleEnabled = onClickPunchScale.FindPropertyRelative("enabled");
            onClickPunchScalePunch = onClickPunchScale.FindPropertyRelative("punch");
            onClickPunchScaleStartDelay = onClickPunchScale.FindPropertyRelative("startDelay");
            onClickPunchScaleDuration = onClickPunchScale.FindPropertyRelative("duration");
            onClickPunchScaleVibrato = onClickPunchScale.FindPropertyRelative("vibrato");
            onClickPunchScaleElasticity = onClickPunchScale.FindPropertyRelative("elasticity");
            onClickGameEventsToggleOn = serializedObject.FindProperty("onClickGameEventsToggleOn");
            onClickGameEventsToggleOff = serializedObject.FindProperty("onClickGameEventsToggleOff");
            #endregion
        }

        protected override void GenerateInfoMessages()
        {
            base.GenerateInfoMessages();

            infoMessage.Add("OnPointerEnterLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "Runtime Preset: " + onPointerEnterPunchPresetCategory.stringValue + " / " + onPointerEnterPunchPresetName.stringValue,
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(loadOnPointerEnterPunchPresetAtRuntime.boolValue, Repaint)
                            });

            infoMessage.Add("OnPointerExitLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "Runtime Preset: " + onPointerExitPunchPresetCategory.stringValue + " / " + onPointerExitPunchPresetName.stringValue,
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(loadOnPointerExitPunchPresetAtRuntime.boolValue, Repaint)
                            });

            infoMessage.Add("OnClickLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "Runtime Preset: " + onClickPunchPresetCategory.stringValue + " / " + onClickPunchPresetName.stringValue,
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(loadOnClickPunchPresetAtRuntime.boolValue, Repaint)
                            });
        }

        protected override void InitAnimBools()
        {
            base.InitAnimBools();

            showOnPointerEnter = new AnimBool(false, Repaint);
            showOnPointerEnterPreset = new AnimBool(false, Repaint);
            showOnPointerEnterPunchMove = new AnimBool(false, Repaint);
            showOnPointerEnterPunchRotate = new AnimBool(false, Repaint);
            showOnPointerEnterPunchScale = new AnimBool(false, Repaint);
            showOnPointerEnterEvents = new AnimBool(false, Repaint);
            showOnPointerEnterGameEvents = new AnimBool(false, Repaint);
            showOnPointerEnterNavigation = new AnimBool(false, Repaint);

            showOnPointerExit = new AnimBool(false, Repaint);
            showOnPointerExitPreset = new AnimBool(false, Repaint);
            showOnPointerExitPunchMove = new AnimBool(false, Repaint);
            showOnPointerExitPunchRotate = new AnimBool(false, Repaint);
            showOnPointerExitPunchScale = new AnimBool(false, Repaint);
            showOnPointerExitEvents = new AnimBool(false, Repaint);
            showOnPointerExitGameEvents = new AnimBool(false, Repaint);
            showOnPointerExitNavigation = new AnimBool(false, Repaint);

            showOnClick = new AnimBool(false, Repaint);
            showOnClickPreset = new AnimBool(false, Repaint);
            showOnClickPunchMove = new AnimBool(false, Repaint);
            showOnClickPunchRotate = new AnimBool(false, Repaint);
            showOnClickPunchScale = new AnimBool(false, Repaint);
            showOnClickEvents = new AnimBool(false, Repaint);
            showOnClickGameEvents = new AnimBool(false, Repaint);
            showOnClickNavigation = new AnimBool(false, Repaint);

            createNewCategoryName = new AnimBool(false, Repaint);
            onPointerEnterPunchNewPreset = new AnimBool(false, Repaint);
            onPointerExitPunchNewPreset = new AnimBool(false, Repaint);
            onClickPunchNewPreset = new AnimBool(false, Repaint);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            requiresContantRepaint = true;

            SyncData();
        }

        void SyncData()
        {
            DUIData.Instance.AutomatedScanForUISounds(); //Scan for any new UISounds
            DUIData.Instance.ValidateUISounds(); //validate UISounds Database

            DUIData.Instance.ValidatePunchAnimations(); //validate PUNCH Animations

            DUIData.Instance.ValidateUIElements(); //validate the database (used by the Navigation Manager)
            UpdateAllNavigationData();
        }
        void UpdateAllNavigationData()
        {
            if(!UIManager.IsNavigationEnabled)
            {
                return;
            }

            //ON POINTER ENTER
            if(useOnPointerEnter.boolValue)
            {
                DUIUtils.UpdateNavigationDataList(uiToggle.onPointerEnterNavigationToggleOn.show, onPointerEnterEditorNavigationDataToggleOn.showIndex);
                DUIUtils.UpdateNavigationDataList(uiToggle.onPointerEnterNavigationToggleOn.hide, onPointerEnterEditorNavigationDataToggleOn.hideIndex);

                DUIUtils.UpdateNavigationDataList(uiToggle.onPointerEnterNavigationToggleOff.show, onPointerEnterEditorNavigationDataToggleOff.showIndex);
                DUIUtils.UpdateNavigationDataList(uiToggle.onPointerEnterNavigationToggleOff.hide, onPointerEnterEditorNavigationDataToggleOff.hideIndex);
            }

            //ON POINTER EXIT
            if(useOnPointerExit.boolValue)
            {
                DUIUtils.UpdateNavigationDataList(uiToggle.onPointerExitNavigationToggleOn.show, onPointerExitEditorNavigationDataToggleOn.showIndex);
                DUIUtils.UpdateNavigationDataList(uiToggle.onPointerExitNavigationToggleOn.hide, onPointerExitEditorNavigationDataToggleOn.hideIndex);

                DUIUtils.UpdateNavigationDataList(uiToggle.onPointerExitNavigationToggleOff.show, onPointerExitEditorNavigationDataToggleOff.showIndex);
                DUIUtils.UpdateNavigationDataList(uiToggle.onPointerExitNavigationToggleOff.hide, onPointerExitEditorNavigationDataToggleOff.hideIndex);
            }

            //ON CLICK
            if(useOnClick.boolValue)
            {
                DUIUtils.UpdateNavigationDataList(uiToggle.onClickNavigationToggleOn.show, onClickEditorNavigationDataToggleOn.showIndex);
                DUIUtils.UpdateNavigationDataList(uiToggle.onClickNavigationToggleOn.hide, onClickEditorNavigationDataToggleOn.hideIndex);

                DUIUtils.UpdateNavigationDataList(uiToggle.onClickNavigationToggleOff.show, onClickEditorNavigationDataToggleOff.showIndex);
                DUIUtils.UpdateNavigationDataList(uiToggle.onClickNavigationToggleOff.hide, onClickEditorNavigationDataToggleOff.hideIndex);
            }
        }

        public override void OnInspectorGUI()
        {
            DrawHeader(DUIResources.headerUIToggle.texture, WIDTH_420, HEIGHT_42);

            if(IsEditorLocked) { return; }

            serializedObject.Update();

            DrawDatabaseButtons(GlobalWidth);
            QUI.Space(SPACE_8);

            DrawSettings(GlobalWidth);
            QUI.Space(SPACE_4);

            //OnPointer ENTER
            if(EditorSettings.UIToggle_Inspector_HideOnPointerEnter)
            {
                useOnPointerEnter.boolValue = false;
            }
            else
            {
                DrawOnPointerEnter(GlobalWidth);
                QUI.Space(SPACE_4);
            }

            //OnPointer EXIT
            if(EditorSettings.UIToggle_Inspector_HideOnPointerExit)
            {
                useOnPointerExit.boolValue = false;
            }
            else
            {
                DrawOnPointerExit(GlobalWidth);
                QUI.Space(SPACE_4);
            }

            //OnClick
            if(EditorSettings.UIToggle_Inspector_HideOnClick)
            {
                useOnClick.boolValue = false;
            }
            else
            {
                DrawOnClick(GlobalWidth);
                QUI.Space(SPACE_4);
            }

            serializedObject.ApplyModifiedProperties();

            QUI.Space(SPACE_4);
        }

        void DrawDatabaseButtons(float width)
        {
            QUI.BeginHorizontal(width);
            {
                if(QUI.GhostButton("UISounds Database", QColors.Color.Gray, width, 18))
                {
                    ControlPanelWindow.OpenWindow(ControlPanelWindow.Page.UISounds);
                }
            }
            QUI.EndHorizontal();
        }

        void DrawSettings(float width)
        {
            QUI.BeginHorizontal(width);
            {
                //ALLOW MULTIPLE CLICKS
                QUI.QToggle("allow multiple clicks", allowMultipleClicks);

                QUI.FlexibleSpace();

                if(allowMultipleClicks.boolValue) { GUI.enabled = false; }

                QLabel.text = "disable button interval";
                QLabel.style = Style.Text.Normal;

                tempFloat = QLabel.x; //save first label width
                tempFloat += 40; //add property field width

                QLabel.text = "seconds";
                QLabel.style = Style.Text.Normal;

                tempFloat += QLabel.x; //add second label width
                tempFloat += 8; //add extra space
                tempFloat += 24; //compensate for unity margins

                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, allowMultipleClicks.boolValue ? QColors.Color.Gray : QColors.Color.Blue), tempFloat, 18);
                QUI.Space(-tempFloat + 4);

                QLabel.text = "disable button interval";
                QLabel.style = Style.Text.Normal;
                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(SPACE_2);
                }
                QUI.EndVertical();

                //DISABLE BUTTON INTERVAL
                QUI.PropertyField(disableButtonInterval, 40);

                QLabel.text = "seconds";
                QLabel.style = Style.Text.Normal;
                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(SPACE_2);
                }
                QUI.EndVertical();

                QUI.Space(SPACE_4);

                GUI.enabled = true;
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_4);

            //DESELECT ON BUTTON CLICK
            QUI.BeginHorizontal(width);
            {
                QUI.QToggle("deselect button on click", deselectButtonOnClick);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_4);

            //CONTROLLER INPUT MODE
            QUI.BeginHorizontal(width);
            {
                QUI.QObjectPropertyField("Controller Input Mode", controllerInputMode, 260, 20, false);
                QUI.Space(SPACE_2);
                if((ControllerInputMode)controllerInputMode.enumValueIndex != ControllerInputMode.None)
                {
                    QUI.QToggle("enable alternate inputs", enableAlternateInputs);
                }
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
            if((ControllerInputMode)controllerInputMode.enumValueIndex != ControllerInputMode.None)
            {
                QUI.BeginHorizontal(width);
                {
                    if((ControllerInputMode)controllerInputMode.enumValueIndex == ControllerInputMode.KeyCode)
                    {
                        QUI.QObjectPropertyField("Key Code", onClickKeyCode, width / 2 - 1, 20, false);
                        QUI.Space(SPACE_2);
                        GUI.enabled = enableAlternateInputs.boolValue;
                        QUI.QObjectPropertyField("Alternate", onClickKeyCodeAlt, width / 2 - 1, 20, false);
                        GUI.enabled = true;
                    }
                    else if((ControllerInputMode)controllerInputMode.enumValueIndex == ControllerInputMode.VirtualButton)
                    {
                        QUI.QObjectPropertyField("Virtual Button", onClickVirtualButtonName, width / 2 - 1, 20, false);
                        QUI.Space(SPACE_2);
                        GUI.enabled = enableAlternateInputs.boolValue;
                        QUI.QObjectPropertyField("Alternate", onClickVirtualButtonNameAlt, width / 2 - 1, 20, false);
                        GUI.enabled = true;
                    }
                    QUI.FlexibleSpace();
                }
                QUI.EndHorizontal();
            }
        }

        void DrawOnPointerEnter(float width)
        {
            DUIUtils.DrawBarWithEnableDisableButton("OnPointer ENTER", useOnPointerEnter, showOnPointerEnter, width, BarHeight);
            DUIUtils.DrawLoadPresetInfoMessage("OnPointerEnterLoadPresetAtRuntime", infoMessage, useOnPointerEnter.boolValue && loadOnPointerEnterPunchPresetAtRuntime.boolValue, onPointerEnterPunchPresetCategory.stringValue, onPointerEnterPunchPresetName.stringValue, showOnPointerEnter, "Punch", width);

            if(!useOnPointerEnter.boolValue) { return; }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * showOnPointerEnter.faded);
                if(QUI.BeginFadeGroup(showOnPointerEnter.faded))
                {
                    QUI.BeginVertical(width - SPACE_8);
                    {
                        QUI.Space(SPACE_2 * showOnPointerEnter.faded);
                        DrawOnPointerEnterSettings(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerEnter.faded);
                        DrawOnPointerEnterPreset(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerEnter.faded);
                        DUIUtils.DrawPunch(uiToggle.onPointerEnterPunch, target, width - SPACE_8); //draw in animations - generic method
                        QUI.Space(SPACE_8 * showOnPointerEnter.faded);

                        QUI.DrawIconBar("Toggle On", DUIResources.miniIconToggleOn, QColors.Color.Green, IconPosition.Left, width - SPACE_8);

                        DrawOnPointerEnterSoundToggleOn(width - SPACE_8);
                        QUI.Space(SPACE_4 * showOnPointerEnter.faded);

                        DrawOnPointerEnterEventsToggleOn(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerEnter.faded);

                        DrawOnPointerEnterGameEventsToggleOn(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerEnter.faded);

                        DrawOnPointerEnterNavigationToggleOn(width - SPACE_8);
                        QUI.Space(SPACE_16 * showOnPointerEnter.faded);

                        QUI.DrawIconBar("Toggle Off", DUIResources.miniIconToggleOff, QColors.Color.Red, IconPosition.Left, width - SPACE_8);

                        DrawOnPointerEnterSoundToggleOff(width - SPACE_8);
                        QUI.Space(SPACE_4 * showOnPointerEnter.faded);

                        DrawOnPointerEnterEventsToggleOff(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerEnter.faded);

                        DrawOnPointerEnterGameEventsToggleOff(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerEnter.faded);

                        DrawOnPointerEnterNavigationToggleOff(width - SPACE_8);
                        QUI.Space(SPACE_16);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();
        }
        void DrawOnPointerEnterSettings(float width)
        {
            QLabel.text = "disable interval";
            QLabel.style = Style.Text.Normal;

            tempFloat = QLabel.x; //save first label width
            tempFloat += 40; //add field width

            QLabel.text = "seconds";
            QLabel.style = Style.Text.Normal;

            tempFloat += QLabel.x; //add second label width
            tempFloat += 8; //add extra space
            tempFloat += 16; //compensate for unity margins

            QUI.BeginHorizontal(width);
            {
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat, 20);
                QUI.Space(-tempFloat);

                QUI.Space(SPACE_4);

                QLabel.text = "disable interval";
                QLabel.style = Style.Text.Normal;
                QUI.Label(QLabel);

                QUI.PropertyField(onPointerEnterDisableInterval, 40);

                QLabel.text = "seconds";
                QLabel.style = Style.Text.Normal;
                QUI.Label(QLabel);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawOnPointerEnterPreset(float width)
        {
            QUI.Space(SPACE_2);
            DUIUtils.DrawLoadPresetAtRuntime(loadOnPointerEnterPunchPresetAtRuntime, width);
            DUIUtils.DrawPresetBackground(onPointerEnterPunchNewPreset, createNewCategoryName, width);

            if(onPointerEnterPunchNewPreset.faded < 0.5f) //NORMAL VIEW
            {
                DUIUtils.DrawPunchPresetNormalView(this, onPointerEnterPunchPresetCategoryNameIndex, onPointerEnterPunchPresetCategory, onPointerEnterPunchPresetNameIndex, onPointerEnterPunchPresetName, onPointerEnterPunchNewPreset, width);
                QUI.Space(SPACE_2);
                DrawPunchPresetNormalViewPresetButtons(ToggleAnimType.OnPointerEnter, onPointerEnterPunchPresetCategory, onPointerEnterPunchPresetName, onPointerEnterPunchNewPreset, width);

            }
            else //NEW PRESET VIEW
            {
                DUIUtils.DrawPunchPresetNewPresetView(this, createNewCategoryName, newPresetCategoryName, onPointerEnterPunchPresetCategoryNameIndex, onPointerEnterPunchPresetCategory, onPointerEnterPunchNewPreset, newPresetName, width);
                QUI.Space(SPACE_2);
                DrawPunchPresetNewPresetViewPresetButtons(ToggleAnimType.OnPointerEnter, onPointerEnterPunchPresetCategoryNameIndex, onPointerEnterPunchPresetCategory, onPointerEnterPunchPresetNameIndex, onPointerEnterPunchPresetName, onPointerEnterPunchNewPreset, width);
            }
        }
        void DrawOnPointerEnterSoundToggleOn(float width)
        {
            DUIUtils.DrawSound(this, "Play Sound", customOnPointerEnterSoundToggleOn, onPointerEnterSoundToggleOn, onPointerEnterSoundIndexToggleOn, width);
        }
        void DrawOnPointerEnterEventsToggleOn(float width)
        {
            DUIUtils.DrawUnityEvents(uiToggle.OnPointerEnterToggleOn.GetPersistentEventCount() > 0, showOnPointerEnterEvents, OnPointerEnterToggleOn, "OnPointerEnterToggleOn", width, MiniBarHeight);
        }
        void DrawOnPointerEnterGameEventsToggleOn(float width)
        {
            DrawGameEvents("OnPointerEnter", showOnPointerEnterGameEvents, onPointerEnterGameEventsToggleOn, width);
        }
        void DrawOnPointerEnterNavigationToggleOn(float width)
        {
            DUIUtils.DrawNavigation(this, uiToggle.onPointerEnterNavigationToggleOn, onPointerEnterEditorNavigationDataToggleOn, showOnPointerEnterNavigation, UpdateAllNavigationData, false, false, width, MiniBarHeight);
        }
        void DrawOnPointerEnterSoundToggleOff(float width)
        {
            DUIUtils.DrawSound(this, "Play Sound", customOnPointerEnterSoundToggleOff, onPointerEnterSoundToggleOff, onPointerEnterSoundIndexToggleOff, width);
        }
        void DrawOnPointerEnterEventsToggleOff(float width)
        {
            DUIUtils.DrawUnityEvents(uiToggle.OnPointerEnterToggleOff.GetPersistentEventCount() > 0, showOnPointerEnterEvents, OnPointerEnterToggleOff, "OnPointerEnterToggleOff", width, MiniBarHeight);
        }
        void DrawOnPointerEnterGameEventsToggleOff(float width)
        {
            DrawGameEvents("OnPointerEnter", showOnPointerEnterGameEvents, onPointerEnterGameEventsToggleOff, width);
        }
        void DrawOnPointerEnterNavigationToggleOff(float width)
        {
            DUIUtils.DrawNavigation(this, uiToggle.onPointerEnterNavigationToggleOff, onPointerEnterEditorNavigationDataToggleOff, showOnPointerEnterNavigation, UpdateAllNavigationData, false, false, width, MiniBarHeight);
        }

        void DrawOnPointerExit(float width)
        {
            DUIUtils.DrawBarWithEnableDisableButton("OnPointer EXIT", useOnPointerExit, showOnPointerExit, width, BarHeight);
            DUIUtils.DrawLoadPresetInfoMessage("OnPointerExitLoadPresetAtRuntime", infoMessage, useOnPointerExit.boolValue && loadOnPointerExitPunchPresetAtRuntime.boolValue, onPointerExitPunchPresetCategory.stringValue, onPointerExitPunchPresetName.stringValue, showOnPointerExit, "Punch", width);

            if(!useOnPointerExit.boolValue) { return; }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * showOnPointerExit.faded);
                if(QUI.BeginFadeGroup(showOnPointerExit.faded))
                {
                    QUI.BeginVertical(width - SPACE_8);
                    {
                        QUI.Space(SPACE_2 * showOnPointerExit.faded);
                        DrawOnPointerExitSettings(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerExit.faded);
                        DrawOnPointerExitPreset(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerExit.faded);
                        DUIUtils.DrawPunch(uiToggle.onPointerExitPunch, target, width - SPACE_8); //draw in animations - generic method
                        QUI.Space(SPACE_8 * showOnPointerExit.faded);

                        QUI.DrawIconBar("Toggle On", DUIResources.miniIconToggleOn, QColors.Color.Green, IconPosition.Left, width - SPACE_8);

                        DrawOnPointerExitSoundToggleOn(width - SPACE_8);
                        QUI.Space(SPACE_4 * showOnPointerExit.faded);

                        DrawOnPointerExitEventsToggleOn(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerExit.faded);

                        DrawOnPointerExitGameEventsToggleOn(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerExit.faded);

                        DrawOnPointerExitNavigationToggleOn(width - SPACE_8);
                        QUI.Space(SPACE_16 * showOnPointerExit.faded);

                        QUI.DrawIconBar("Toggle Off", DUIResources.miniIconToggleOff, QColors.Color.Red, IconPosition.Left, width - SPACE_8);

                        DrawOnPointerExitSoundToggleOff(width - SPACE_8);
                        QUI.Space(SPACE_4 * showOnPointerExit.faded);

                        DrawOnPointerExitEventsToggleOff(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerExit.faded);

                        DrawOnPointerExitGameEventsToggleOff(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerExit.faded);

                        DrawOnPointerExitNavigationToggleOff(width - SPACE_8);
                        QUI.Space(SPACE_16);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();
        }
        void DrawOnPointerExitSettings(float width)
        {
            QLabel.text = "disable interval";
            QLabel.style = Style.Text.Normal;

            tempFloat = QLabel.x; //save first label width
            tempFloat += 40; //add field width

            QLabel.text = "seconds";
            QLabel.style = Style.Text.Normal;

            tempFloat += QLabel.x; //add second label width
            tempFloat += 8; //add extra space
            tempFloat += 16; //compensate for unity margins

            QUI.BeginHorizontal(width);
            {
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat, 20);
                QUI.Space(-tempFloat);

                QUI.Space(SPACE_4);

                QLabel.text = "disable interval";
                QLabel.style = Style.Text.Normal;
                QUI.Label(QLabel);

                QUI.PropertyField(onPointerExitDisableInterval, 40);

                QLabel.text = "seconds";
                QLabel.style = Style.Text.Normal;
                QUI.Label(QLabel);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawOnPointerExitPreset(float width)
        {
            QUI.Space(SPACE_2);
            DUIUtils.DrawLoadPresetAtRuntime(loadOnPointerExitPunchPresetAtRuntime, width);
            DUIUtils.DrawPresetBackground(onPointerExitPunchNewPreset, createNewCategoryName, width);

            if(onPointerExitPunchNewPreset.faded < 0.5f) //NORMAL VIEW
            {
                DUIUtils.DrawPunchPresetNormalView(this, onPointerExitPunchPresetCategoryNameIndex, onPointerExitPunchPresetCategory, onPointerExitPunchPresetNameIndex, onPointerExitPunchPresetName, onPointerExitPunchNewPreset, width);
                QUI.Space(SPACE_2);
                DrawPunchPresetNormalViewPresetButtons(ToggleAnimType.OnPointerExit, onPointerExitPunchPresetCategory, onPointerExitPunchPresetName, onPointerExitPunchNewPreset, width);

            }
            else //NEW PRESET VIEW
            {
                DUIUtils.DrawPunchPresetNewPresetView(this, createNewCategoryName, newPresetCategoryName, onPointerExitPunchPresetCategoryNameIndex, onPointerExitPunchPresetCategory, onPointerExitPunchNewPreset, newPresetName, width);
                QUI.Space(SPACE_2);
                DrawPunchPresetNewPresetViewPresetButtons(ToggleAnimType.OnPointerExit, onPointerExitPunchPresetCategoryNameIndex, onPointerExitPunchPresetCategory, onPointerExitPunchPresetNameIndex, onPointerExitPunchPresetName, onPointerExitPunchNewPreset, width);
            }
        }
        void DrawOnPointerExitSoundToggleOn(float width)
        {
            DUIUtils.DrawSound(this, "Play Sound", customOnPointerExitSoundToggleOn, onPointerExitSoundToggleOn, onPointerExitSoundIndexToggleOn, width);
        }
        void DrawOnPointerExitEventsToggleOn(float width)
        {
            DUIUtils.DrawUnityEvents(uiToggle.OnPointerExitToggleOn.GetPersistentEventCount() > 0, showOnPointerExitEvents, OnPointerExitToggleOn, "OnPointerExitToggleOn", width, MiniBarHeight);
        }
        void DrawOnPointerExitGameEventsToggleOn(float width)
        {
            DrawGameEvents("OnPointerExit", showOnPointerExitGameEvents, onPointerExitGameEventsToggleOn, width);
        }
        void DrawOnPointerExitNavigationToggleOn(float width)
        {
            DUIUtils.DrawNavigation(this, uiToggle.onPointerExitNavigationToggleOn, onPointerExitEditorNavigationDataToggleOn, showOnPointerExitNavigation, UpdateAllNavigationData, false, false, width, MiniBarHeight);
        }
        void DrawOnPointerExitSoundToggleOff(float width)
        {
            DUIUtils.DrawSound(this, "Play Sound", customOnPointerExitSoundToggleOff, onPointerExitSoundToggleOff, onPointerExitSoundIndexToggleOff, width);
        }
        void DrawOnPointerExitEventsToggleOff(float width)
        {
            DUIUtils.DrawUnityEvents(uiToggle.OnPointerExitToggleOff.GetPersistentEventCount() > 0, showOnPointerExitEvents, OnPointerExitToggleOff, "OnPointerExitToggleOff", width, MiniBarHeight);
        }
        void DrawOnPointerExitGameEventsToggleOff(float width)
        {
            DrawGameEvents("OnPointerExit", showOnPointerExitGameEvents, onPointerExitGameEventsToggleOff, width);
        }
        void DrawOnPointerExitNavigationToggleOff(float width)
        {
            DUIUtils.DrawNavigation(this, uiToggle.onPointerExitNavigationToggleOff, onPointerExitEditorNavigationDataToggleOff, showOnPointerExitNavigation, UpdateAllNavigationData, false, false, width, MiniBarHeight);
        }

        void DrawOnClick(float width)
        {
            DUIUtils.DrawBarWithEnableDisableButton("OnClick", useOnClick, showOnClick, width, BarHeight);
            DUIUtils.DrawLoadPresetInfoMessage("OnClickLoadPresetAtRuntime", infoMessage, useOnClick.boolValue && loadOnClickPunchPresetAtRuntime.boolValue, onClickPunchPresetCategory.stringValue, onClickPunchPresetName.stringValue, showOnClick, "Punch", width);

            if(!useOnClick.boolValue) { return; }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * showOnClick.faded);
                if(QUI.BeginFadeGroup(showOnClick.faded))
                {
                    QUI.BeginVertical(width - SPACE_8);
                    {
                        QUI.Space(SPACE_2 * showOnClick.faded);
                        DrawOnClickSettings(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnClick.faded);
                        DrawOnClickPreset(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnClick.faded);
                        DUIUtils.DrawPunch(uiToggle.onClickPunch, target, width - SPACE_8); //draw in animations - generic method
                        QUI.Space(SPACE_8 * showOnClick.faded);

                        QUI.DrawIconBar("Toggle On", DUIResources.miniIconToggleOn, QColors.Color.Green, IconPosition.Left, width - SPACE_8);

                        DrawOnClickSoundToggleOn(width - SPACE_8);
                        QUI.Space(SPACE_4 * showOnClick.faded);

                        DrawOnClickEventsToggleOn(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnClick.faded);

                        DrawOnClickGameEventsToggleOn(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnClick.faded);

                        DrawOnClickNavigationToggleOn(width - SPACE_8);
                        QUI.Space(SPACE_16 * showOnClick.faded);

                        QUI.DrawIconBar("Toggle Off", DUIResources.miniIconToggleOff, QColors.Color.Red, IconPosition.Left, width - SPACE_8);

                        DrawOnClickSoundToggleOff(width - SPACE_8);
                        QUI.Space(SPACE_4 * showOnClick.faded);

                        DrawOnClickEventsToggleOff(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnClick.faded);

                        DrawOnClickGameEventsToggleOff(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnClick.faded);

                        DrawOnClickNavigationToggleOff(width - SPACE_8);
                        QUI.Space(SPACE_16);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();
        }
        void DrawOnClickSettings(float width)
        {
            QUI.BeginHorizontal(width);
            {
                QUI.QToggle("wait for animation", waitForOnClick);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawOnClickPreset(float width)
        {
            QUI.Space(SPACE_2);
            DUIUtils.DrawLoadPresetAtRuntime(loadOnClickPunchPresetAtRuntime, width);
            DUIUtils.DrawPresetBackground(onClickPunchNewPreset, createNewCategoryName, width);

            if(onClickPunchNewPreset.faded < 0.5f) //NORMAL VIEW
            {
                DUIUtils.DrawPunchPresetNormalView(this, onClickPunchPresetCategoryNameIndex, onClickPunchPresetCategory, onClickPunchPresetNameIndex, onClickPunchPresetName, onClickPunchNewPreset, width);
                QUI.Space(SPACE_2);
                DrawPunchPresetNormalViewPresetButtons(ToggleAnimType.OnClick, onClickPunchPresetCategory, onClickPunchPresetName, onClickPunchNewPreset, width);

            }
            else //NEW PRESET VIEW
            {
                DUIUtils.DrawPunchPresetNewPresetView(this, createNewCategoryName, newPresetCategoryName, onClickPunchPresetCategoryNameIndex, onClickPunchPresetCategory, onClickPunchNewPreset, newPresetName, width);
                QUI.Space(SPACE_2);
                DrawPunchPresetNewPresetViewPresetButtons(ToggleAnimType.OnClick, onClickPunchPresetCategoryNameIndex, onClickPunchPresetCategory, onClickPunchPresetNameIndex, onClickPunchPresetName, onClickPunchNewPreset, width);
            }
        }
        void DrawOnClickSoundToggleOn(float width)
        {
            DUIUtils.DrawSound(this, "Play Sound", customOnClickSoundToggleOn, onClickSoundToggleOn, onClickSoundIndexToggleOn, width);
        }
        void DrawOnClickEventsToggleOn(float width)
        {
            DUIUtils.DrawUnityEvents(uiToggle.OnClickToggleOn.GetPersistentEventCount() > 0, showOnClickEvents, OnClickToggleOn, "OnClickToggleOn", width, MiniBarHeight);
        }
        void DrawOnClickGameEventsToggleOn(float width)
        {
            DrawGameEvents("OnClick", showOnClickGameEvents, onClickGameEventsToggleOn, width);
        }
        void DrawOnClickNavigationToggleOn(float width)
        {
            DUIUtils.DrawNavigation(this, uiToggle.onClickNavigationToggleOn, onClickEditorNavigationDataToggleOn, showOnClickNavigation, UpdateAllNavigationData, true, false, width, MiniBarHeight);
        }
        void DrawOnClickSoundToggleOff(float width)
        {
            DUIUtils.DrawSound(this, "Play Sound", customOnClickSoundToggleOff, onClickSoundToggleOff, onClickSoundIndexToggleOff, width);
        }
        void DrawOnClickEventsToggleOff(float width)
        {
            DUIUtils.DrawUnityEvents(uiToggle.OnClickToggleOff.GetPersistentEventCount() > 0, showOnClickEvents, OnClickToggleOff, "OnClickToggleOff", width, MiniBarHeight);
        }
        void DrawOnClickGameEventsToggleOff(float width)
        {
            DrawGameEvents("OnClick", showOnClickGameEvents, onClickGameEventsToggleOff, width);
        }
        void DrawOnClickNavigationToggleOff(float width)
        {
            DUIUtils.DrawNavigation(this, uiToggle.onClickNavigationToggleOff, onClickEditorNavigationDataToggleOff, showOnClickNavigation, UpdateAllNavigationData, true, false, width, MiniBarHeight);
        }

        void DrawPunchPresetNormalViewPresetButtons(ToggleAnimType toggleAnimType, SerializedProperty presetCategoryName, SerializedProperty presetName, AnimBool newPreset, float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), width, 22);

            QUI.Space(-20);

            tempFloat = (width - SPACE_16) / 3; //button width
            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_4);
                if(QUI.GhostButton("Load Preset", QColors.Color.Blue, tempFloat * (1 - newPreset.faded), MiniBarHeight))
                {
                    LoadPunchPreset(toggleAnimType, presetCategoryName, presetName);
                }
                QUI.Space(SPACE_4);
                if(newPreset.faded > 0)
                {
                    QUI.FlexibleSpace();
                }
                if(QUI.GhostButton("New Preset", QColors.Color.Green, tempFloat * (1 - newPreset.faded), MiniBarHeight))
                {
                    NewPunchPreset(newPreset, presetCategoryName);
                }
                if(newPreset.faded > 0)
                {
                    QUI.FlexibleSpace();
                }
                QUI.Space(SPACE_4);
                if(QUI.GhostButton("Delete Preset", QColors.Color.Red, tempFloat * (1 - newPreset.faded), MiniBarHeight))
                {
                    DeletePunchPreset(toggleAnimType, onPointerEnterPunchPresetCategory, onPointerEnterPunchPresetName);
                }
                QUI.Space(SPACE_4);
            }
            QUI.EndHorizontal();
        }
        void DrawPunchPresetNewPresetViewPresetButtons(ToggleAnimType toggleAnimType, Index presetCategoryIndex, SerializedProperty presetCategoryName, Index presetNameIndex, SerializedProperty presetName, AnimBool newPreset, float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), width, 22);

            QUI.Space(-20);

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_4);
                QUI.BeginChangeCheck();
                createNewCategoryName.target = QUI.Toggle(createNewCategoryName.target);
                if(QUI.EndChangeCheck())
                {
                    if(createNewCategoryName.target == false) //if the dev decided not to create a new category name, restore the selected new category name
                    {
                        newPresetCategoryName.name = presetCategoryName.stringValue;
                    }
                }
                QLabel.text = "Create a new category";
                QLabel.style = Style.Text.Normal;
                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(SPACE_2);
                }
                QUI.EndVertical();

                QUI.FlexibleSpace();

                tempFloat = (width - 24) / 4; //button width
                if(QUI.GhostButton("Save Preset", QColors.Color.Green, tempFloat * newPreset.faded, MiniBarHeight)
                    || (QUI.DetectKeyUp(Event.current, KeyCode.Return) && (QUI.GetNameOfFocusedControl().Equals("newPresetName") || QUI.GetNameOfFocusedControl().Equals("newPresetCategoryName"))))
                {
                    tempBool = false; //save the new preset -> if a new preset was saved
                    switch(toggleAnimType)
                    {
                        case ToggleAnimType.OnPointerEnter: tempBool = SavePunchPreset(toggleAnimType, onPointerEnterPunchPresetCategory, onPointerEnterPunchPresetName); break;
                        case ToggleAnimType.OnPointerExit: tempBool = SavePunchPreset(toggleAnimType, onPointerExitPunchPresetCategory, onPointerExitPunchPresetName); break;
                        case ToggleAnimType.OnClick: tempBool = SavePunchPreset(toggleAnimType, onClickPunchPresetCategory, onClickPunchPresetName); break;
                    }

                    if(tempBool) //save the new preset -> if a new preset was saved -> update the indexes
                    {
                        presetCategoryIndex.index = DatabasePunchAnimations.CategoryNameIndex(presetCategoryName.stringValue); //update the index
                        presetNameIndex.index = DatabasePunchAnimations.ItemNameIndex(presetCategoryName.stringValue, presetName.stringValue); //update the index
                    }
                }

                QUI.Space(SPACE_4);

                if(QUI.GhostButton("Cancel", QColors.Color.Red, tempFloat * newPreset.faded, MiniBarHeight)
                     || QUI.DetectKeyUp(Event.current, KeyCode.Escape))
                {
                    ResetNewPresetState();
                }

                QUI.Space(SPACE_4);

                QUI.Space(tempFloat * (1 - newPreset.faded));
            }
            QUI.EndHorizontal();

            QUI.Space(-SPACE_4);
        }

        void LoadPunchPreset(ToggleAnimType toggleAnimType, SerializedProperty presetCategory, SerializedProperty presetName)
        {
            Punch punch = UIAnimatorUtil.GetPunch(presetCategory.stringValue, presetName.stringValue);
            UIToggle targetToggle;

            if(serializedObject.isEditingMultipleObjects)
            {
                Undo.RecordObjects(targets, "LoadPreset");
                switch(toggleAnimType)
                {
                    case ToggleAnimType.OnPointerEnter: for(int i = 0; i < targets.Length; i++) { targetToggle = (UIToggle)targets[i]; targetToggle.onPointerEnterPunch = punch.Copy(); } break;
                    case ToggleAnimType.OnPointerExit: for(int i = 0; i < targets.Length; i++) { targetToggle = (UIToggle)targets[i]; targetToggle.onPointerExitPunch = punch.Copy(); } break;
                    case ToggleAnimType.OnClick: for(int i = 0; i < targets.Length; i++) { targetToggle = (UIToggle)targets[i]; targetToggle.onClickPunch = punch.Copy(); } break;
                }
            }
            else
            {
                Undo.RecordObject(target, "LoadPreset");
                targetToggle = (UIToggle)target;
                switch(toggleAnimType)
                {
                    case ToggleAnimType.OnPointerEnter: targetToggle.onPointerEnterPunch = punch.Copy(); break;
                    case ToggleAnimType.OnPointerExit: targetToggle.onPointerExitPunch = punch.Copy(); break;
                    case ToggleAnimType.OnClick: targetToggle.onClickPunch = punch.Copy(); break;
                }
            }

            QUI.ExitGUI();
        }
        void NewPunchPreset(AnimBool newPreset, SerializedProperty presetCategory)
        {
            ResetNewPresetState();
            newPreset.target = true;
            newPresetCategoryName.name = presetCategory.stringValue;
        }
        void DeletePunchPreset(ToggleAnimType toggleAnimType, SerializedProperty presetCategory, SerializedProperty presetName)
        {
            if(QUI.DisplayDialog("Delete Preset",
                                            "Are you sure you want to delete the '" + presetName.stringValue + "' preset from the '" + presetCategory.stringValue + "' preset category?",
                                            "Yes",
                                            "No"))
            {

                DatabasePunchAnimations.GetCategory(presetCategory.stringValue).DeletePunchData(presetName.stringValue, UIAnimatorUtil.RELATIVE_PATH_PUNCH_DATA);
                if(DatabasePunchAnimations.GetCategory(presetCategory.stringValue).IsEmpty()) //category is empty -> remove it from the database (sanity check)
                {
                    DatabasePunchAnimations.RemoveCategory(presetCategory.stringValue, UIAnimatorUtil.RELATIVE_PATH_PUNCH_DATA, true);
                }

                if(serializedObject.isEditingMultipleObjects)
                {
                    UIToggle toggle;
                    switch(toggleAnimType)
                    {
                        case ToggleAnimType.OnPointerEnter:
                            for(int i = 0; i < targets.Length; i++)
                            {
                                toggle = (UIToggle)targets[i];
                                if(toggle.onPointerEnterPunchPresetCategory.Equals(presetCategory.stringValue)
                                   && toggle.onPointerEnterPunchPresetName.Equals(presetName.stringValue))
                                {
                                    toggle.onPointerEnterPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                                    toggle.onPointerEnterPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                }
                            }
                            break;
                        case ToggleAnimType.OnPointerExit:
                            for(int i = 0; i < targets.Length; i++)
                            {
                                toggle = (UIToggle)targets[i];
                                if(toggle.onPointerExitPunchPresetCategory.Equals(presetCategory.stringValue)
                                   && toggle.onPointerExitPunchPresetName.Equals(presetName.stringValue))
                                {
                                    toggle.onPointerExitPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                                    toggle.onPointerExitPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                }
                            }
                            break;
                        case ToggleAnimType.OnClick:
                            for(int i = 0; i < targets.Length; i++)
                            {
                                toggle = (UIToggle)targets[i];
                                if(toggle.onClickPunchPresetCategory.Equals(presetCategory.stringValue)
                                   && toggle.onClickPunchPresetName.Equals(presetName.stringValue))
                                {
                                    toggle.onClickPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                                    toggle.onClickPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                }
                            }
                            break;
                    }
                }

                presetCategory.stringValue = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                presetName.stringValue = UIAnimatorUtil.DEFAULT_PRESET_NAME;

                serializedObject.ApplyModifiedProperties();
            }
        }
        bool SavePunchPreset(ToggleAnimType toggleAnimType, SerializedProperty presetCategory, SerializedProperty presetName)
        {
            if(createNewCategoryName.target && string.IsNullOrEmpty(newPresetCategoryName.name.Trim()))
            {
                QUI.DisplayDialog("Info",
                                  "The new preset category name cannot be an empty string.",
                                  "Ok");
                return false; //return false that a new preset was not created
            }

            if(string.IsNullOrEmpty(newPresetName.name.Trim()))
            {
                QUI.DisplayDialog("Info",
                                  "The new preset name cannot be an empty string.",
                                  "Ok");
                return false; //return false that a new preset was not created
            }

            if(DatabasePunchAnimations.Contains(newPresetCategoryName.name, newPresetName.name)) //test if the database contains the new preset name
            {
                QUI.DisplayDialog("Info",
                                  "There is another preset with the '" + newPresetName + "' preset name in the '" + newPresetCategoryName.name + "' preset category." +
                                    "\n\n" +
                                    "Try a different preset name maybe?",
                                  "Ok");
                return false; //return false that a new preset was not created
            }

            if(!DatabasePunchAnimations.ContainsCategory(newPresetCategoryName.name)) //test if the database contains the new category
            {
                DatabasePunchAnimations.AddCategory(newPresetCategoryName.name, UIAnimatorUtil.RELATIVE_PATH_PUNCH_DATA, true); //create the new category 
            }

            switch(toggleAnimType) //create the new preset
            {
                case ToggleAnimType.OnPointerEnter: DatabasePunchAnimations.GetCategory(newPresetCategoryName.name).AddPunchData(UIAnimatorUtil.CreatePunchPreset(newPresetCategoryName.name, newPresetName.name, uiToggle.onPointerEnterPunch.Copy())); break;
                case ToggleAnimType.OnPointerExit: DatabasePunchAnimations.GetCategory(newPresetCategoryName.name).AddPunchData(UIAnimatorUtil.CreatePunchPreset(newPresetCategoryName.name, newPresetName.name, uiToggle.onPointerExitPunch.Copy())); break;
                case ToggleAnimType.OnClick: DatabasePunchAnimations.GetCategory(newPresetCategoryName.name).AddPunchData(UIAnimatorUtil.CreatePunchPreset(newPresetCategoryName.name, newPresetName.name, uiToggle.onClickPunch.Copy())); break;
            }

            if(serializedObject.isEditingMultipleObjects)
            {
                UIToggle toggle;
                switch(toggleAnimType)
                {
                    case ToggleAnimType.OnPointerEnter:
                        for(int i = 0; i < targets.Length; i++)
                        {
                            toggle = (UIToggle)targets[i];
                            toggle.onPointerEnterPunchPresetCategory = newPresetCategoryName.name;
                            toggle.onPointerEnterPunchPresetName = newPresetName.name;
                        }
                        break;
                    case ToggleAnimType.OnPointerExit:
                        for(int i = 0; i < targets.Length; i++)
                        {
                            toggle = (UIToggle)targets[i];
                            toggle.onPointerExitPunchPresetCategory = newPresetCategoryName.name;
                            toggle.onPointerExitPunchPresetName = newPresetName.name;
                        }
                        break;
                    case ToggleAnimType.OnClick:
                        for(int i = 0; i < targets.Length; i++)
                        {
                            toggle = (UIToggle)targets[i];
                            toggle.onClickPunchPresetCategory = newPresetCategoryName.name;
                            toggle.onClickPunchPresetName = newPresetName.name;
                        }
                        break;
                }
            }

            presetCategory.stringValue = newPresetCategoryName.name;
            presetName.stringValue = newPresetName.name;

            serializedObject.ApplyModifiedProperties();
            ResetNewPresetState();
            return true; //return true that a new preset has been created
        }
        void ResetNewPresetState()
        {
            onPointerEnterPunchNewPreset.target = false;
            onPointerExitPunchNewPreset.target = false;
            onClickPunchNewPreset.target = false;

            createNewCategoryName.target = false;
            newPresetCategoryName.name = string.Empty;
            newPresetName.name = string.Empty;

            QUI.ResetKeyboardFocus();
        }

        void DrawGameEvents(string tag, AnimBool show, SerializedProperty gameEvents, float width)
        {
            QUI.DrawCollapsableList("Game Events", show, gameEvents.arraySize > 0 ? QColors.Color.Blue : QColors.Color.Gray, gameEvents, width, MiniBarHeight, "Not sending any Game Events on " + tag + "... Click [+] to start...");
        }
    }
}
