// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using QuickEngine.Extensions;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using DoozyUI.Internal;

namespace DoozyUI
{
    [CustomEditor(typeof(UIButton), true)]
    [DisallowMultipleComponent]
    [CanEditMultipleObjects]
    public class UIButtonEditor : QEditor
    {

        UIButton uiButton { get { return (UIButton)target; } }
        DUIData.Database DatabaseUIElements { get { return DUIData.Instance.DatabaseUIElements; } }
        DUIData.Database DatabaseUIButtons { get { return DUIData.Instance.DatabaseUIButtons; } }
        DUIData.PunchDatabase DatabasePunchAnimations { get { return DUIData.Instance.DatabasePunchAnimations; } }
        DUIData.AnimDatabase DatabaseStateAnimations { get { return DUIData.Instance.DatabaseStateAnimations; } }
        DUIData.LoopDatabase DatabaseLoopAnimations { get { return DUIData.Instance.DatabaseLoopAnimations; } }
        DUIData.SoundsDatabase DatabaseUISounds { get { return DUIData.Instance.DatabaseUISounds; } }

        DUISettings EditorSettings { get { return DUISettings.Instance; } }

#pragma warning disable 0414
        SerializedProperty
        #region ButtonCategory
            buttonCategory,
        #endregion
        #region ButtonName
            buttonName,
        #endregion
        #region Settings
            allowMultipleClicks, disableButtonInterval,
            deselectButtonOnClick,
            controllerInputMode, enableAlternateInputs,
            onClickKeyCode, onClickKeyCodeAlt,
            onClickVirtualButtonName, onClickVirtualButtonNameAlt,
        #endregion
        #region PointerEnter
        useOnPointerEnter, onPointerEnterDisableInterval, onPointerEnterSound, customOnPointerEnterSound, OnPointerEnter,
            onPointerEnterAnimationType,
            onPointerEnterPunchPresetCategory, onPointerEnterPunchPresetName, loadOnPointerEnterPunchPresetAtRuntime,
            onPointerEnterStatePresetCategory, onPointerEnterStatePresetName, loadOnPointerEnterStatePresetAtRuntime,
            onPointerEnterGameEvents,
        #endregion
        #region PointerExit
            useOnPointerExit, onPointerExitDisableInterval, onPointerExitSound, customOnPointerExitSound, OnPointerExit,
            onPointerExitAnimationType,
            onPointerExitPunchPresetCategory, onPointerExitPunchPresetName, loadOnPointerExitPunchPresetAtRuntime,
            onPointerExitStatePresetCategory, onPointerExitStatePresetName, loadOnPointerExitStatePresetAtRuntime,
            onPointerExitGameEvents,
        #endregion
        #region PointerDown
            useOnPointerDown, onPointerDownSound, customOnPointerDownSound, OnPointerDown,
            onPointerDownAnimationType,
            onPointerDownPunchPresetCategory, onPointerDownPunchPresetName, loadOnPointerDownPunchPresetAtRuntime,
            onPointerDownStatePresetCategory, onPointerDownStatePresetName, loadOnPointerDownStatePresetAtRuntime,
            onPointerDownGameEvents,
        #endregion
        #region PointerUp
            useOnPointerUp, onPointerUpSound, customOnPointerUpSound, OnPointerUp,
            onPointerUpAnimationType,
            onPointerUpPunchPresetCategory, onPointerUpPunchPresetName, loadOnPointerUpPunchPresetAtRuntime,
            onPointerUpStatePresetCategory, onPointerUpStatePresetName, loadOnPointerUpStatePresetAtRuntime,
            onPointerUpGameEvents,
        #endregion
        #region OnClick
            useOnClickAnimations, waitForOnClickAnimation, singleClickMode, onClickSound, customOnClickSound, OnClick,
            onClickAnimationType,
            onClickPunchPresetCategory, onClickPunchPresetName, loadOnClickPunchPresetAtRuntime,
            onClickStatePresetCategory, onClickStatePresetName, loadOnClickStatePresetAtRuntime,
            onClickGameEvents,
        #endregion
        #region OnDoubleClick
            useOnDoubleClick, waitForOnDoubleClickAnimation, doubleClickRegisterInterval, onDoubleClickSound, customOnDoubleClickSound, OnDoubleClick,
            onDoubleClickAnimationType,
            onDoubleClickPunchPresetCategory, onDoubleClickPunchPresetName, loadOnDoubleClickPunchPresetAtRuntime,
            onDoubleClickStatePresetCategory, onDoubleClickStatePresetName, loadOnDoubleClickStatePresetAtRuntime,
            onDoubleClickGameEvents,
        #endregion
        #region OnLongClick
            useOnLongClick, waitForOnLongClickAnimation, longClickRegisterInterval, onLongClickSound, customOnLongClickSound, OnLongClick,
            onLongClickAnimationType,
            onLongClickPunchPresetCategory, onLongClickPunchPresetName, loadOnLongClickPunchPresetAtRuntime,
            onLongClickStatePresetCategory, onLongClickStatePresetName, loadOnLongClickStatePresetAtRuntime,
            onLongClickGameEvents,
        #endregion
        #region NormalLoop
            normalLoopPresetCategory, normalLoopPresetName, loadNormalLoopPresetAtRuntime,
            normalLoop,
            normalLoopMove,
            normalLoopMoveEnabled, normalLoopMoveMovement, normalLoopMoveEaseType, normalLoopMoveEase, normalLoopMoveAnimationCurve, normalLoopMoveLoops, normalLoopMoveLoopType, normalLoopMoveStartDelay, normalLoopMoveDuration,
            normalLoopRotate,
            normalLoopRotateEnabled, normalLoopRotateRotation, normalLoopRotateEaseType, normalLoopRotateEase, normalLoopRotateAnimationCurve, normalLoopRotateLoops, normalLoopRotateLoopType, normalLoopRotateStartDelay, normalLoopRotateDuration,
            normalLoopScale,
            normalLoopScaleEnabled, normalLoopScaleMin, normalLoopScaleMax, normalLoopScaleEaseType, normalLoopScaleEase, normalLoopScaleAnimationCurve, normalLoopScaleLoops, normalLoopScaleLoopType, normalLoopScaleStartDelay, normalLoopScaleDuration,
            normalLoopFade,
            normalLoopFadeEnabled, normalLoopFadeMin, normalLoopFadeMax, normalLoopFadeEaseType, normalLoopFadeEase, normalLoopFadeAnimationCurve, normalLoopFadeLoops, normalLoopFadeLoopType, normalLoopFadeStartDelay, normalLoopFadeDuration,
        #endregion
        #region SelectedLoop
            selectedLoopPresetCategory, selectedLoopPresetName, loadSelectedLoopPresetAtRuntime,
            selectedLoop,
            selectedLoopMove,
            selectedLoopMoveEnabled, selectedLoopMoveMovement, selectedLoopMoveEaseType, selectedLoopMoveEase, selectedLoopMoveAnimationCurve, selectedLoopMoveLoops, selectedLoopMoveLoopType, selectedLoopMoveStartDelay, selectedLoopMoveDuration,
            selectedLoopRotate,
            selectedLoopRotateEnabled, selectedLoopRotateRotation, selectedLoopRotateEaseType, selectedLoopRotateEase, selectedLoopRotateAnimationCurve, selectedLoopRotateLoops, selectedLoopRotateLoopType, selectedLoopRotateStartDelay, selectedLoopRotateDuration,
            selectedLoopScale,
            selectedLoopScaleEnabled, selectedLoopScaleMin, selectedLoopScaleMax, selectedLoopScaleEaseType, selectedLoopScaleEase, selectedLoopScaleAnimationCurve, selectedLoopScaleLoops, selectedLoopScaleLoopType, selectedLoopScaleStartDelay, selectedLoopScaleDuration,
            selectedLoopFade,
            selectedLoopFadeEnabled, selectedLoopFadeMin, selectedLoopFadeMax, selectedLoopFadeEaseType, selectedLoopFadeEase, selectedLoopFadeAnimationCurve, selectedLoopFadeLoops, selectedLoopFadeLoopType, selectedLoopFadeStartDelay, selectedLoopFadeDuration;
        #endregion

        AnimBool
            showDisableButtonInterval,
            showOnPointerEnter, showOnPointerEnterEvents, showOnPointerEnterGameEvents, showOnPointerEnterNavigation,
            showOnPointerExit, showOnPointerExitEvents, showOnPointerExitGameEvents, showOnPointerExitNavigation,
            showOnPointerDown, showOnPointerDownEvents, showOnPointerDownGameEvents, showOnPointerDownNavigation,
            showOnPointerUp, showOnPointerUpEvents, showOnPointerUpGameEvents, showOnPointerUpNavigation,
            showOnClick, showOnClickEvents, showOnClickGameEvents, showOnClickNavigation,
            showOnDoubleClick, showOnDoubleClickEvents, showOnDoubleClickGameEvents, showOnDoubleClickNavigation,
            showOnLongClick, showOnLongClickEvents, showOnLongClickGameEvents, showOnLongClickNavigation,
            showNormalAnimation,
            showSelectedAnimation;

#pragma warning restore 0414

        int buttonNameIndex = 0;
        int buttonCategoryIndex = 0;

        Index onPointerEnterSoundIndex = new Index();
        Index onPointerExitSoundIndex = new Index();
        Index onPointerDownSoundIndex = new Index();
        Index onPointerUpSoundIndex = new Index();
        Index onClickSoundIndex = new Index();
        Index onDoubleClickSoundIndex = new Index();
        Index onLongClickSoundIndex = new Index();

        Name newPresetCategoryName = new Name();
        Name newPresetName = new Name();

        AnimBool createNewCategoryName;

        Index onPointerEnterPunchPresetCategoryNameIndex = new Index();
        Index onPointerEnterPunchPresetNameIndex = new Index();
        AnimBool onPointerEnterPunchNewPreset;
        Index onPointerEnterStatePresetCategoryNameIndex = new Index();
        Index onPointerEnterStatePresetNameIndex = new Index();
        AnimBool onPointerEnterStateNewPreset;

        Index onPointerExitPunchPresetCategoryNameIndex = new Index();
        Index onPointerExitPunchPresetNameIndex = new Index();
        AnimBool onPointerExitPunchNewPreset;
        Index onPointerExitStatePresetCategoryNameIndex = new Index();
        Index onPointerExitStatePresetNameIndex = new Index();
        AnimBool onPointerExitStateNewPreset;

        Index onPointerDownPunchPresetCategoryNameIndex = new Index();
        Index onPointerDownPunchPresetNameIndex = new Index();
        AnimBool onPointerDownPunchNewPreset;
        Index onPointerDownStatePresetCategoryNameIndex = new Index();
        Index onPointerDownStatePresetNameIndex = new Index();
        AnimBool onPointerDownStateNewPreset;

        Index onPointerUpPunchPresetCategoryNameIndex = new Index();
        Index onPointerUpPunchPresetNameIndex = new Index();
        AnimBool onPointerUpPunchNewPreset;
        Index onPointerUpStatePresetCategoryNameIndex = new Index();
        Index onPointerUpStatePresetNameIndex = new Index();
        AnimBool onPointerUpStateNewPreset;

        Index onClickPunchPresetCategoryNameIndex = new Index();
        Index onClickPunchPresetNameIndex = new Index();
        AnimBool onClickPunchNewPreset;
        Index onClickStatePresetCategoryNameIndex = new Index();
        Index onClickStatePresetNameIndex = new Index();
        AnimBool onClickStateNewPreset;

        Index onDoubleClickPunchPresetCategoryNameIndex = new Index();
        Index onDoubleClickPunchPresetNameIndex = new Index();
        AnimBool onDoubleClickPunchNewPreset;
        Index onDoubleClickStatePresetCategoryNameIndex = new Index();
        Index onDoubleClickStatePresetNameIndex = new Index();
        AnimBool onDoubleClickStateNewPreset;

        Index onLongClickPunchPresetCategoryNameIndex = new Index();
        Index onLongClickPunchPresetNameIndex = new Index();
        AnimBool onLongClickPunchNewPreset;
        Index onLongClickStatePresetCategoryNameIndex = new Index();
        Index onLongClickStatePresetNameIndex = new Index();
        AnimBool onLongClickStateNewPreset;

        Index normalLoopPresetCategoryIndex = new Index();
        Index normalLoopPresetNameIndex = new Index();
        AnimBool normalLoopNewPreset;

        Index selectedLoopPresetCategoryIndex = new Index();
        Index selectedLoopPresetNameIndex = new Index();
        AnimBool selectedLoopNewPreset;

        EditorNavigationPointerData onPointerEnterEditorNavigationData = new EditorNavigationPointerData();
        EditorNavigationPointerData onPointerExitEditorNavigationData = new EditorNavigationPointerData();
        EditorNavigationPointerData onPointerDownEditorNavigationData = new EditorNavigationPointerData();
        EditorNavigationPointerData onPointerUpEditorNavigationData = new EditorNavigationPointerData();
        EditorNavigationPointerData onClickEditorNavigationData = new EditorNavigationPointerData();
        EditorNavigationPointerData onDoubleClickEditorNavigationData = new EditorNavigationPointerData();
        EditorNavigationPointerData onLongClickEditorNavigationData = new EditorNavigationPointerData();

        float GlobalWidth { get { return DUI.GLOBAL_EDITOR_WIDTH; } }
        int BarHeight { get { return DUI.BAR_HEIGHT; } }
        int MiniBarHeight { get { return DUI.MINI_BAR_HEIGHT; } }

        float tempFloat = 0;
        bool tempBool = false;

        protected override void SerializedObjectFindProperties()
        {
            base.SerializedObjectFindProperties();

            #region ButtonCategory
            buttonCategory = serializedObject.FindProperty("buttonCategory");
            #endregion
            #region ButtonName
            buttonName = serializedObject.FindProperty("buttonName");
            #endregion
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
            onPointerEnterSound = serializedObject.FindProperty("onPointerEnterSound");
            customOnPointerEnterSound = serializedObject.FindProperty("customOnPointerEnterSound");
            OnPointerEnter = serializedObject.FindProperty("OnPointerEnter");
            onPointerEnterAnimationType = serializedObject.FindProperty("onPointerEnterAnimationType");
            onPointerEnterPunchPresetCategory = serializedObject.FindProperty("onPointerEnterPunchPresetCategory");
            onPointerEnterPunchPresetName = serializedObject.FindProperty("onPointerEnterPunchPresetName");
            loadOnPointerEnterPunchPresetAtRuntime = serializedObject.FindProperty("loadOnPointerEnterPunchPresetAtRuntime");
            onPointerEnterStatePresetCategory = serializedObject.FindProperty("onPointerEnterStatePresetCategory");
            onPointerEnterStatePresetName = serializedObject.FindProperty("onPointerEnterStatePresetName");
            loadOnPointerEnterStatePresetAtRuntime = serializedObject.FindProperty("loadOnPointerEnterStatePresetAtRuntime");
            onPointerEnterGameEvents = serializedObject.FindProperty("onPointerEnterGameEvents");
            #endregion
            #region PointerExit
            useOnPointerExit = serializedObject.FindProperty("useOnPointerExit");
            onPointerExitDisableInterval = serializedObject.FindProperty("onPointerExitDisableInterval");
            onPointerExitSound = serializedObject.FindProperty("onPointerExitSound");
            customOnPointerExitSound = serializedObject.FindProperty("customOnPointerExitSound");
            OnPointerExit = serializedObject.FindProperty("OnPointerExit");
            onPointerExitAnimationType = serializedObject.FindProperty("onPointerExitAnimationType");
            onPointerExitPunchPresetCategory = serializedObject.FindProperty("onPointerExitPunchPresetCategory");
            onPointerExitPunchPresetName = serializedObject.FindProperty("onPointerExitPunchPresetName");
            loadOnPointerExitPunchPresetAtRuntime = serializedObject.FindProperty("loadOnPointerExitPunchPresetAtRuntime");
            onPointerExitStatePresetCategory = serializedObject.FindProperty("onPointerExitStatePresetCategory");
            onPointerExitStatePresetName = serializedObject.FindProperty("onPointerExitStatePresetName");
            loadOnPointerExitStatePresetAtRuntime = serializedObject.FindProperty("loadOnPointerExitStatePresetAtRuntime");
            onPointerExitGameEvents = serializedObject.FindProperty("onPointerExitGameEvents");
            #endregion
            #region PointerDown
            useOnPointerDown = serializedObject.FindProperty("useOnPointerDown");
            onPointerDownSound = serializedObject.FindProperty("onPointerDownSound");
            customOnPointerDownSound = serializedObject.FindProperty("customOnPointerDownSound");
            OnPointerDown = serializedObject.FindProperty("OnPointerDown");
            onPointerDownAnimationType = serializedObject.FindProperty("onPointerDownAnimationType");
            onPointerDownPunchPresetCategory = serializedObject.FindProperty("onPointerDownPunchPresetCategory");
            onPointerDownPunchPresetName = serializedObject.FindProperty("onPointerDownPunchPresetName");
            loadOnPointerDownPunchPresetAtRuntime = serializedObject.FindProperty("loadOnPointerDownPunchPresetAtRuntime");
            onPointerDownStatePresetCategory = serializedObject.FindProperty("onPointerDownStatePresetCategory");
            onPointerDownStatePresetName = serializedObject.FindProperty("onPointerDownStatePresetName");
            loadOnPointerDownStatePresetAtRuntime = serializedObject.FindProperty("loadOnPointerDownStatePresetAtRuntime");
            onPointerDownGameEvents = serializedObject.FindProperty("onPointerDownGameEvents");
            #endregion
            #region PointerUp
            useOnPointerUp = serializedObject.FindProperty("useOnPointerUp");
            onPointerUpSound = serializedObject.FindProperty("onPointerUpSound");
            customOnPointerUpSound = serializedObject.FindProperty("customOnPointerUpSound");
            OnPointerUp = serializedObject.FindProperty("OnPointerUp");
            onPointerUpAnimationType = serializedObject.FindProperty("onPointerUpAnimationType");
            onPointerUpPunchPresetCategory = serializedObject.FindProperty("onPointerUpPunchPresetCategory");
            onPointerUpPunchPresetName = serializedObject.FindProperty("onPointerUpPunchPresetName");
            loadOnPointerUpPunchPresetAtRuntime = serializedObject.FindProperty("loadOnPointerUpPunchPresetAtRuntime");
            onPointerUpStatePresetCategory = serializedObject.FindProperty("onPointerUpStatePresetCategory");
            onPointerUpStatePresetName = serializedObject.FindProperty("onPointerUpStatePresetName");
            loadOnPointerUpStatePresetAtRuntime = serializedObject.FindProperty("loadOnPointerUpStatePresetAtRuntime");
            onPointerUpGameEvents = serializedObject.FindProperty("onPointerUpGameEvents");
            #endregion
            #region OnClick
            useOnClickAnimations = serializedObject.FindProperty("useOnClickAnimations");
            waitForOnClickAnimation = serializedObject.FindProperty("waitForOnClickAnimation");
            singleClickMode = serializedObject.FindProperty("singleClickMode");
            onClickSound = serializedObject.FindProperty("onClickSound");
            customOnClickSound = serializedObject.FindProperty("customOnClickSound");
            OnClick = serializedObject.FindProperty("OnClick");
            onClickAnimationType = serializedObject.FindProperty("onClickAnimationType");
            onClickPunchPresetCategory = serializedObject.FindProperty("onClickPunchPresetCategory");
            onClickPunchPresetName = serializedObject.FindProperty("onClickPunchPresetName");
            loadOnClickPunchPresetAtRuntime = serializedObject.FindProperty("loadOnClickPunchPresetAtRuntime");
            onClickStatePresetCategory = serializedObject.FindProperty("onClickStatePresetCategory");
            onClickStatePresetName = serializedObject.FindProperty("onClickStatePresetName");
            loadOnClickStatePresetAtRuntime = serializedObject.FindProperty("loadOnClickStatePresetAtRuntime");
            onClickGameEvents = serializedObject.FindProperty("onClickGameEvents");
            #endregion
            #region OnDoubleClick
            useOnDoubleClick = serializedObject.FindProperty("useOnDoubleClick");
            waitForOnDoubleClickAnimation = serializedObject.FindProperty("waitForOnDoubleClickAnimation");
            doubleClickRegisterInterval = serializedObject.FindProperty("doubleClickRegisterInterval");
            onDoubleClickSound = serializedObject.FindProperty("onDoubleClickSound");
            customOnDoubleClickSound = serializedObject.FindProperty("customOnDoubleClickSound");
            OnDoubleClick = serializedObject.FindProperty("OnDoubleClick");
            onDoubleClickAnimationType = serializedObject.FindProperty("onDoubleClickAnimationType");
            onDoubleClickPunchPresetCategory = serializedObject.FindProperty("onDoubleClickPunchPresetCategory");
            onDoubleClickPunchPresetName = serializedObject.FindProperty("onDoubleClickPunchPresetName");
            loadOnDoubleClickPunchPresetAtRuntime = serializedObject.FindProperty("loadOnDoubleClickPunchPresetAtRuntime");
            onDoubleClickStatePresetCategory = serializedObject.FindProperty("onDoubleClickStatePresetCategory");
            onDoubleClickStatePresetName = serializedObject.FindProperty("onDoubleClickStatePresetName");
            loadOnDoubleClickStatePresetAtRuntime = serializedObject.FindProperty("loadOnDoubleClickStatePresetAtRuntime");
            onDoubleClickGameEvents = serializedObject.FindProperty("onDoubleClickGameEvents");
            #endregion
            #region OnLongClick
            useOnLongClick = serializedObject.FindProperty("useOnLongClick");
            waitForOnLongClickAnimation = serializedObject.FindProperty("waitForOnLongClickAnimation");
            longClickRegisterInterval = serializedObject.FindProperty("longClickRegisterInterval");
            onLongClickSound = serializedObject.FindProperty("onLongClickSound");
            customOnLongClickSound = serializedObject.FindProperty("customOnLongClickSound");
            OnLongClick = serializedObject.FindProperty("OnLongClick");
            onLongClickAnimationType = serializedObject.FindProperty("onLongClickAnimationType");
            onLongClickPunchPresetCategory = serializedObject.FindProperty("onLongClickPunchPresetCategory");
            onLongClickPunchPresetName = serializedObject.FindProperty("onLongClickPunchPresetName");
            loadOnLongClickPunchPresetAtRuntime = serializedObject.FindProperty("loadOnLongClickPunchPresetAtRuntime");
            onLongClickStatePresetCategory = serializedObject.FindProperty("onLongClickStatePresetCategory");
            onLongClickStatePresetName = serializedObject.FindProperty("onLongClickStatePresetName");
            loadOnLongClickStatePresetAtRuntime = serializedObject.FindProperty("loadOnLongClickStatePresetAtRuntime");
            onLongClickGameEvents = serializedObject.FindProperty("onLongClickGameEvents");
            #endregion
            #region NormalLoop
            normalLoopPresetCategory = serializedObject.FindProperty("normalLoopPresetCategory");
            normalLoopPresetName = serializedObject.FindProperty("normalLoopPresetName");
            loadNormalLoopPresetAtRuntime = serializedObject.FindProperty("loadNormalLoopPresetAtRuntime");
            normalLoop = serializedObject.FindProperty("normalLoop");
            normalLoopMove = normalLoop.FindPropertyRelative("move");
            normalLoopMoveEnabled = normalLoopMove.FindPropertyRelative("enabled");
            normalLoopMoveMovement = normalLoopMove.FindPropertyRelative("movement");
            normalLoopMoveEaseType = normalLoopMove.FindPropertyRelative("easeType");
            normalLoopMoveEase = normalLoopMove.FindPropertyRelative("ease");
            normalLoopMoveAnimationCurve = normalLoopMove.FindPropertyRelative("animationCurve");
            normalLoopMoveLoops = normalLoopMove.FindPropertyRelative("loops");
            normalLoopMoveLoopType = normalLoopMove.FindPropertyRelative("loopType");
            normalLoopMoveStartDelay = normalLoopMove.FindPropertyRelative("startDelay");
            normalLoopMoveDuration = normalLoopMove.FindPropertyRelative("duration");
            normalLoopRotate = normalLoop.FindPropertyRelative("rotate");
            normalLoopRotateEnabled = normalLoopRotate.FindPropertyRelative("enabled");
            normalLoopRotateRotation = normalLoopRotate.FindPropertyRelative("rotation");
            normalLoopRotateEaseType = normalLoopRotate.FindPropertyRelative("easeType");
            normalLoopRotateEase = normalLoopRotate.FindPropertyRelative("ease");
            normalLoopRotateAnimationCurve = normalLoopRotate.FindPropertyRelative("animationCurve");
            normalLoopRotateLoops = normalLoopRotate.FindPropertyRelative("loops");
            normalLoopRotateLoopType = normalLoopRotate.FindPropertyRelative("loopType");
            normalLoopRotateStartDelay = normalLoopRotate.FindPropertyRelative("startDelay");
            normalLoopRotateDuration = normalLoopRotate.FindPropertyRelative("duration");
            normalLoopScale = normalLoop.FindPropertyRelative("scale");
            normalLoopScaleEnabled = normalLoopScale.FindPropertyRelative("enabled");
            normalLoopScaleMin = normalLoopScale.FindPropertyRelative("min");
            normalLoopScaleMax = normalLoopScale.FindPropertyRelative("max");
            normalLoopScaleEaseType = normalLoopScale.FindPropertyRelative("easeType");
            normalLoopScaleEase = normalLoopScale.FindPropertyRelative("ease");
            normalLoopScaleAnimationCurve = normalLoopScale.FindPropertyRelative("animationCurve");
            normalLoopScaleLoops = normalLoopScale.FindPropertyRelative("loops");
            normalLoopScaleLoopType = normalLoopScale.FindPropertyRelative("loopType");
            normalLoopScaleStartDelay = normalLoopScale.FindPropertyRelative("startDelay");
            normalLoopScaleDuration = normalLoopScale.FindPropertyRelative("duration");
            normalLoopFade = normalLoop.FindPropertyRelative("fade");
            normalLoopFadeEnabled = normalLoopFade.FindPropertyRelative("enabled");
            normalLoopFadeMin = normalLoopFade.FindPropertyRelative("min");
            normalLoopFadeMax = normalLoopFade.FindPropertyRelative("max");
            normalLoopFadeEaseType = normalLoopFade.FindPropertyRelative("easeType");
            normalLoopFadeEase = normalLoopFade.FindPropertyRelative("ease");
            normalLoopFadeAnimationCurve = normalLoopFade.FindPropertyRelative("animationCurve");
            normalLoopFadeLoops = normalLoopFade.FindPropertyRelative("loops");
            normalLoopFadeLoopType = normalLoopFade.FindPropertyRelative("loopType");
            normalLoopFadeStartDelay = normalLoopFade.FindPropertyRelative("startDelay");
            normalLoopFadeDuration = normalLoopFade.FindPropertyRelative("duration");
            #endregion
            #region SelectedLoop
            selectedLoopPresetCategory = serializedObject.FindProperty("selectedLoopPresetCategory");
            selectedLoopPresetName = serializedObject.FindProperty("selectedLoopPresetName");
            loadSelectedLoopPresetAtRuntime = serializedObject.FindProperty("loadSelectedLoopPresetAtRuntime");
            selectedLoop = serializedObject.FindProperty("selectedLoop");
            selectedLoopMove = selectedLoop.FindPropertyRelative("move");
            selectedLoopMoveEnabled = selectedLoopMove.FindPropertyRelative("enabled");
            selectedLoopMoveMovement = selectedLoopMove.FindPropertyRelative("movement");
            selectedLoopMoveEaseType = selectedLoopMove.FindPropertyRelative("easeType");
            selectedLoopMoveEase = selectedLoopMove.FindPropertyRelative("ease");
            selectedLoopMoveAnimationCurve = selectedLoopMove.FindPropertyRelative("animationCurve");
            selectedLoopMoveLoops = selectedLoopMove.FindPropertyRelative("loops");
            selectedLoopMoveLoopType = selectedLoopMove.FindPropertyRelative("loopType");
            selectedLoopMoveStartDelay = selectedLoopMove.FindPropertyRelative("startDelay");
            selectedLoopMoveDuration = selectedLoopMove.FindPropertyRelative("duration");
            selectedLoopRotate = selectedLoop.FindPropertyRelative("rotate");
            selectedLoopRotateEnabled = selectedLoopRotate.FindPropertyRelative("enabled");
            selectedLoopRotateRotation = selectedLoopRotate.FindPropertyRelative("rotation");
            selectedLoopRotateEaseType = selectedLoopRotate.FindPropertyRelative("easeType");
            selectedLoopRotateEase = selectedLoopRotate.FindPropertyRelative("ease");
            selectedLoopRotateAnimationCurve = selectedLoopRotate.FindPropertyRelative("animationCurve");
            selectedLoopRotateLoops = selectedLoopRotate.FindPropertyRelative("loops");
            selectedLoopRotateLoopType = selectedLoopRotate.FindPropertyRelative("loopType");
            selectedLoopRotateStartDelay = selectedLoopRotate.FindPropertyRelative("startDelay");
            selectedLoopRotateDuration = selectedLoopRotate.FindPropertyRelative("duration");
            selectedLoopScale = selectedLoop.FindPropertyRelative("scale");
            selectedLoopScaleEnabled = selectedLoopScale.FindPropertyRelative("enabled");
            selectedLoopScaleMin = selectedLoopScale.FindPropertyRelative("min");
            selectedLoopScaleMax = selectedLoopScale.FindPropertyRelative("max");
            selectedLoopScaleEaseType = selectedLoopScale.FindPropertyRelative("easeType");
            selectedLoopScaleEase = selectedLoopScale.FindPropertyRelative("ease");
            selectedLoopScaleAnimationCurve = selectedLoopScale.FindPropertyRelative("animationCurve");
            selectedLoopScaleLoops = selectedLoopScale.FindPropertyRelative("loops");
            selectedLoopScaleLoopType = selectedLoopScale.FindPropertyRelative("loopType");
            selectedLoopScaleStartDelay = selectedLoopScale.FindPropertyRelative("startDelay");
            selectedLoopScaleDuration = selectedLoopScale.FindPropertyRelative("duration");
            selectedLoopFade = selectedLoop.FindPropertyRelative("fade");
            selectedLoopFadeEnabled = selectedLoopFade.FindPropertyRelative("enabled");
            selectedLoopFadeMin = selectedLoopFade.FindPropertyRelative("min");
            selectedLoopFadeMax = selectedLoopFade.FindPropertyRelative("max");
            selectedLoopFadeEaseType = selectedLoopFade.FindPropertyRelative("easeType");
            selectedLoopFadeEase = selectedLoopFade.FindPropertyRelative("ease");
            selectedLoopFadeAnimationCurve = selectedLoopFade.FindPropertyRelative("animationCurve");
            selectedLoopFadeLoops = selectedLoopFade.FindPropertyRelative("loops");
            selectedLoopFadeLoopType = selectedLoopFade.FindPropertyRelative("loopType");
            selectedLoopFadeStartDelay = selectedLoopFade.FindPropertyRelative("startDelay");
            selectedLoopFadeDuration = selectedLoopFade.FindPropertyRelative("duration");
            #endregion
        }

        protected override void GenerateInfoMessages()
        {
            base.GenerateInfoMessages();

            infoMessage.Add("OnPointerEnterLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "",
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("OnPointerExitLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "",
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("OnPointerDownLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "",
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("OnPointerUpLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "",
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("OnClickLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "",
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("OnDoubleClickLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "",
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("OnLongClickLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "",
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("NormalLoopLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "Runtime Preset: " + normalLoopPresetCategory.stringValue + " / " + normalLoopPresetName.stringValue,
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(loadNormalLoopPresetAtRuntime.boolValue, Repaint)
                            });

            infoMessage.Add("SelectedLoopLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "Runtime Preset: " + selectedLoopPresetCategory.stringValue + " / " + selectedLoopPresetName.stringValue,
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(loadSelectedLoopPresetAtRuntime.boolValue, Repaint)
                            });
        }

        protected override void InitAnimBools()
        {
            base.InitAnimBools();

            showDisableButtonInterval = new AnimBool(!allowMultipleClicks.boolValue, Repaint);

            showOnPointerEnter = new AnimBool(false, Repaint);
            showOnPointerEnterEvents = new AnimBool(false, Repaint);
            showOnPointerEnterGameEvents = new AnimBool(false, Repaint);
            showOnPointerEnterNavigation = new AnimBool(false, Repaint);

            showOnPointerExit = new AnimBool(false, Repaint);
            showOnPointerExitEvents = new AnimBool(false, Repaint);
            showOnPointerExitGameEvents = new AnimBool(false, Repaint);
            showOnPointerExitNavigation = new AnimBool(false, Repaint);

            showOnPointerDown = new AnimBool(false, Repaint);
            showOnPointerDownEvents = new AnimBool(false, Repaint);
            showOnPointerDownGameEvents = new AnimBool(false, Repaint);
            showOnPointerDownNavigation = new AnimBool(false, Repaint);

            showOnPointerUp = new AnimBool(false, Repaint);
            showOnPointerUpEvents = new AnimBool(false, Repaint);
            showOnPointerUpGameEvents = new AnimBool(false, Repaint);
            showOnPointerUpNavigation = new AnimBool(false, Repaint);

            showOnClick = new AnimBool(false, Repaint);
            showOnClickEvents = new AnimBool(false, Repaint);
            showOnClickGameEvents = new AnimBool(false, Repaint);
            showOnClickNavigation = new AnimBool(false, Repaint);

            showOnDoubleClick = new AnimBool(false, Repaint);
            showOnDoubleClickEvents = new AnimBool(false, Repaint);
            showOnDoubleClickGameEvents = new AnimBool(false, Repaint);
            showOnDoubleClickNavigation = new AnimBool(false, Repaint);

            showOnLongClick = new AnimBool(false, Repaint);
            showOnLongClickEvents = new AnimBool(false, Repaint);
            showOnLongClickGameEvents = new AnimBool(false, Repaint);
            showOnLongClickNavigation = new AnimBool(false, Repaint);

            showNormalAnimation = new AnimBool(false, Repaint);

            showSelectedAnimation = new AnimBool(false, Repaint);

            createNewCategoryName = new AnimBool(false, Repaint);

            onPointerEnterPunchNewPreset = new AnimBool(false, Repaint);
            onPointerEnterStateNewPreset = new AnimBool(false, Repaint);

            onPointerExitPunchNewPreset = new AnimBool(false, Repaint);
            onPointerExitStateNewPreset = new AnimBool(false, Repaint);

            onPointerDownPunchNewPreset = new AnimBool(false, Repaint);
            onPointerDownStateNewPreset = new AnimBool(false, Repaint);

            onPointerUpPunchNewPreset = new AnimBool(false, Repaint);
            onPointerUpStateNewPreset = new AnimBool(false, Repaint);

            onClickPunchNewPreset = new AnimBool(false, Repaint);
            onClickStateNewPreset = new AnimBool(false, Repaint);

            onDoubleClickPunchNewPreset = new AnimBool(false, Repaint);
            onDoubleClickStateNewPreset = new AnimBool(false, Repaint);

            onLongClickPunchNewPreset = new AnimBool(false, Repaint);
            onLongClickStateNewPreset = new AnimBool(false, Repaint);

            normalLoopNewPreset = new AnimBool(false, Repaint);
            selectedLoopNewPreset = new AnimBool(false, Repaint);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            requiresContantRepaint = true;

            SyncData();
        }

        void SyncData()
        {
            DUIData.Instance.ValidateUIButtons(); //validate the database
            ValidateButtonCategoryAndButtonName();

            DUIData.Instance.AutomatedScanForUISounds(); //Scan for any new UISounds
            DUIData.Instance.ValidateUISounds(); //validate UISounds Database

            DUIData.Instance.ValidatePunchAnimations(); //validate PUNCH Animations
            DUIData.Instance.ValidateStateAnimations(); //validate STATE Animations
            DUIData.Instance.ValidateLoopAnimations(); //validate LOOP Animations

            DUIData.Instance.ValidateUIElements(); //validate the database (used by the Navigation Manager)
            UpdateAllNavigationData();
        }
        void ValidateButtonCategoryAndButtonName()
        {
            //CHECK FOR CUSTOM NAME
            if(uiButton.buttonCategory.Equals(DUI.CUSTOM_NAME)) //category is set to CUSTOM NAME -> get the index for the category name and set the name index to -1
            {
                buttonCategoryIndex = DatabaseUIButtons.CategoryNameIndex(DUI.CUSTOM_NAME); //set the index
                buttonNameIndex = -1; //set the index
                return; //stop here as there is a CUSTOM NAME set
            }

            //SANITY CHECK FOR EMPTY CATEGORY NAME
            if(uiButton.buttonCategory.IsNullOrEmpty()) //category name is empty (sanity check) -> reset both category and name
            {
                uiButton.buttonCategory = DUI.UNCATEGORIZED_CATEGORY_NAME; //reset the value
                buttonCategoryIndex = DatabaseUIButtons.CategoryNameIndex(uiButton.buttonCategory); //set the index

                uiButton.buttonName = DUI.DEFAULT_BUTTON_NAME; //reset the value
                buttonNameIndex = DatabaseUIButtons.ItemNameIndex(uiButton.buttonCategory, uiButton.buttonName); //set the index
                return;
            }

            //CHECK THAT CATEGORY EXISTS IN THE DATABASE
            if(!DatabaseUIButtons.ContainsCategoryName(uiButton.buttonCategory)) //the set category does not exist in the database
            {
                if(QUI.DisplayDialog("Action Required",
                                     "The category '" + uiButton.buttonCategory + "' was not found in the database." +
                                       "\n\n" +
                                       "Do you want to add it to the database?",
                                     "Yes",
                                     "No")) //ask the dev if he wants to add this category to the database
                {
                    DatabaseUIButtons.AddCategory(uiButton.buttonCategory, true); //add the category to the database and save
                    buttonCategoryIndex = DatabaseUIButtons.CategoryNameIndex(uiButton.buttonCategory); //set the index
                }
                else
                {
                    QUI.DisplayDialog("Info",
                                      "Button category has been reset to the default '" + DUI.UNCATEGORIZED_CATEGORY_NAME + "' value.",
                                      "Ok"); //inform the dev that becuase he did not add the category to the database, it has been reset to its default value
                    uiButton.buttonCategory = DUI.UNCATEGORIZED_CATEGORY_NAME; //reset the value
                    buttonCategoryIndex = DatabaseUIButtons.CategoryNameIndex(uiButton.buttonCategory); //set the index
                }
            }

            //CHECK THAT THE NAME EXISTS IN THE CATEGORY
            if(!DatabaseUIButtons.Contains(uiButton.buttonCategory, uiButton.buttonName)) //the set element name does not exist under the set category
            {
                if(QUI.DisplayDialog("Action Required",
                                     "The name '" + uiButton.buttonName + "' was not found in the '" + uiButton.buttonCategory + "' category." +
                                       "\n\n" +
                                       "Do you want to add it to the database?",
                                     "Yes",
                                     "No")) //ask the dev if he wants to add this name to the database
                {
                    DatabaseUIButtons.GetCategory(uiButton.buttonCategory).AddItemName(uiButton.buttonName, true); //add the item name to the database and save
                    buttonNameIndex = DatabaseUIButtons.ItemNameIndex(uiButton.buttonCategory, uiButton.buttonName); //set the index
                }
                else
                {
                    QUI.DisplayDialog("Info",
                                      "Button name has been reset to the default '" + DUI.DEFAULT_BUTTON_NAME + "' value.",
                                      "Ok"); //inform the dev that becuase he did not add the name to the database, it has been reset to its default value
                    uiButton.buttonName = DUI.DEFAULT_BUTTON_NAME; //reset the value
                    buttonNameIndex = DatabaseUIButtons.ItemNameIndex(uiButton.buttonCategory, uiButton.buttonName); //set the index
                }
            }
            else
            {
                buttonCategoryIndex = DatabaseUIButtons.CategoryNameIndex(uiButton.buttonCategory);
                buttonNameIndex = DatabaseUIButtons.ItemNameIndex(uiButton.buttonCategory, uiButton.buttonName);
            }
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
                DUIUtils.UpdateNavigationDataList(uiButton.onPointerEnterNavigation.show, onPointerEnterEditorNavigationData.showIndex);
                DUIUtils.UpdateNavigationDataList(uiButton.onPointerEnterNavigation.hide, onPointerEnterEditorNavigationData.hideIndex);
            }

            //ON POINTER EXIT
            if(useOnPointerExit.boolValue)
            {
                DUIUtils.UpdateNavigationDataList(uiButton.onPointerExitNavigation.show, onPointerExitEditorNavigationData.showIndex);
                DUIUtils.UpdateNavigationDataList(uiButton.onPointerExitNavigation.hide, onPointerExitEditorNavigationData.hideIndex);
            }

            //ON POINTER DOWN
            if(useOnPointerDown.boolValue)
            {
                DUIUtils.UpdateNavigationDataList(uiButton.onPointerDownNavigation.show, onPointerDownEditorNavigationData.showIndex);
                DUIUtils.UpdateNavigationDataList(uiButton.onPointerDownNavigation.hide, onPointerDownEditorNavigationData.hideIndex);
            }

            //ON POINTER UP
            if(useOnPointerUp.boolValue)
            {
                DUIUtils.UpdateNavigationDataList(uiButton.onPointerUpNavigation.show, onPointerUpEditorNavigationData.showIndex);
                DUIUtils.UpdateNavigationDataList(uiButton.onPointerUpNavigation.hide, onPointerUpEditorNavigationData.hideIndex);
            }

            //ON CLICK
            if(useOnClickAnimations.boolValue)
            {
                DUIUtils.UpdateNavigationDataList(uiButton.onClickNavigation.show, onClickEditorNavigationData.showIndex);
                DUIUtils.UpdateNavigationDataList(uiButton.onClickNavigation.hide, onClickEditorNavigationData.hideIndex);
            }

            //ON DOUBLE CLICK
            if(useOnDoubleClick.boolValue)
            {
                DUIUtils.UpdateNavigationDataList(uiButton.onDoubleClickNavigation.show, onDoubleClickEditorNavigationData.showIndex);
                DUIUtils.UpdateNavigationDataList(uiButton.onDoubleClickNavigation.hide, onDoubleClickEditorNavigationData.hideIndex);
            }

            //ON LONG CLICK
            if(useOnLongClick.boolValue)
            {
                DUIUtils.UpdateNavigationDataList(uiButton.onLongClickNavigation.show, onLongClickEditorNavigationData.showIndex);
                DUIUtils.UpdateNavigationDataList(uiButton.onLongClickNavigation.hide, onLongClickEditorNavigationData.hideIndex);
            }
        }

        public override void OnInspectorGUI()
        {
            DrawHeader(DUIResources.headerUIButton.texture, WIDTH_420, HEIGHT_42);

            if(IsEditorLocked) { return; }

            serializedObject.Update();

            DrawDatabaseButtons(GlobalWidth);
            QUI.Space(SPACE_2);

            if(EditorSettings.UIButton_Inspector_ShowButtonRenameGameObject)
            {
                DrawRenameGameObjectButton(GlobalWidth);
                QUI.Space(SPACE_8);
            }

            DrawButtonCategoryAndButtonName(GlobalWidth);
            QUI.Space(SPACE_8);

            DrawSettings(GlobalWidth);
            QUI.Space(SPACE_4);

            //OnPointer ENTER
            if(EditorSettings.UIButton_Inspector_HideOnPointerEnter)
            {
                useOnPointerEnter.boolValue = false;
            }
            else
            {
                DrawOnPointerEnter(GlobalWidth);
                QUI.Space(SPACE_4);
            }

            //OnPointer EXIT
            if(EditorSettings.UIButton_Inspector_HideOnPointerExit)
            {
                useOnPointerExit.boolValue = false;
            }
            else
            {
                DrawOnPointerExit(GlobalWidth);
                QUI.Space(SPACE_4);
            }

            //OnPointer DOWN
            if(EditorSettings.UIButton_Inspector_HideOnPointerDown)
            {
                useOnPointerDown.boolValue = false;
            }
            else
            {
                DrawOnPointerDown(GlobalWidth);
                QUI.Space(SPACE_4);
            }

            //OnPointer UP
            if(EditorSettings.UIButton_Inspector_HideOnPointerUp)
            {
                useOnPointerUp.boolValue = false;
            }
            else
            {
                DrawOnPointerUp(GlobalWidth);
                QUI.Space(SPACE_4);
            }

            //OnClick
            if(EditorSettings.UIButton_Inspector_HideOnClick)
            {
                useOnClickAnimations.boolValue = false;
            }
            else
            {
                DrawOnClick(GlobalWidth);
                QUI.Space(SPACE_4);
            }

            //OnDoubleClick
            if(EditorSettings.UIButton_Inspector_HideOnDoubleClick)
            {
                useOnDoubleClick.boolValue = false;
            }
            else
            {
                DrawOnDoubleClick(GlobalWidth);
                QUI.Space(SPACE_4);
            }

            //OnLongClick
            if(EditorSettings.UIButton_Inspector_HideOnLongClick)
            {
                useOnLongClick.boolValue = false;
            }
            else
            {
                DrawOnLongClick(GlobalWidth);
                QUI.Space(SPACE_4);
            }

            if(!EditorSettings.UIButton_Inspector_HideNormalLoop || !EditorSettings.UIButton_Inspector_HideSelectedLoop)
            {
                QUI.Space(SPACE_8);
            }

            //NormalLoop
            if(EditorSettings.UIButton_Inspector_HideNormalLoop)
            {
                loadNormalLoopPresetAtRuntime.boolValue = false;

                normalLoopMoveEnabled.boolValue = false;
                normalLoopRotateEnabled.boolValue = false;
                normalLoopScaleEnabled.boolValue = false;
                normalLoopFadeEnabled.boolValue = false;
            }
            else
            {
                DrawNormalLoop(GlobalWidth);
                QUI.Space(-SPACE_4);
            }

            //SelectedLoop
            if(EditorSettings.UIButton_Inspector_HideSelectedLoop)
            {
                loadSelectedLoopPresetAtRuntime.boolValue = false;

                selectedLoopMoveEnabled.boolValue = false;
                selectedLoopRotateEnabled.boolValue = false;
                selectedLoopScaleEnabled.boolValue = false;
                selectedLoopFadeEnabled.boolValue = false;
            }
            else
            {
                DrawSelectedLoop(GlobalWidth);
                QUI.Space(-SPACE_4);
            }

            serializedObject.ApplyModifiedProperties();

            QUI.Space(SPACE_4);
        }

        void DrawDatabaseButtons(float width)
        {
            QUI.BeginHorizontal(width);
            {
                if(QUI.GhostButton("UIButtons Database", QColors.Color.Gray, (width - SPACE_2) / 2, 18))
                {
                    ControlPanelWindow.OpenWindow(ControlPanelWindow.Page.UIButtons);
                }
                QUI.Space(SPACE_2);
                if(QUI.GhostButton("UISounds Database", QColors.Color.Gray, (width - SPACE_2) / 2, 18))
                {
                    ControlPanelWindow.OpenWindow(ControlPanelWindow.Page.UISounds);
                }
            }
            QUI.EndHorizontal();
        }
        void DrawRenameGameObjectButton(float width)
        {
            QUI.BeginHorizontal(width);
            {
                if(QUI.GhostButton("Rename GameObject to Button Name", QColors.Color.Gray, width, 18))
                {
                    if(serializedObject.isEditingMultipleObjects)
                    {
                        Undo.RecordObjects(targets, "Rename");
                        for(int i = 0; i < targets.Length; i++)
                        {
                            UIButton iTarget = (UIButton)targets[i];
                            iTarget.gameObject.name = DUI.DUISettings.UIButton_Inspector_RenameGameObjectPrefix + iTarget.buttonName + DUI.DUISettings.UIButton_Inspector_RenameGameObjectSuffix;
                        }
                    }
                    else
                    {
                        Undo.RecordObject(target, "Rename");
                        uiButton.gameObject.name = DUI.DUISettings.UIButton_Inspector_RenameGameObjectPrefix + buttonName.stringValue + DUI.DUISettings.UIButton_Inspector_RenameGameObjectSuffix;
                    }
                }
            }
            QUI.EndHorizontal();
        }

        void DrawButtonCategoryAndButtonName(float width)
        {
            QUI.BeginHorizontal(width);
            {
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), (width - SPACE_2) / 2, 34);
                QUI.Space(SPACE_2);
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), (width - SPACE_2) / 2, 34);
            }
            QUI.EndHorizontal();

            QUI.Space(-36);

            QUI.BeginHorizontal(width);
            {
                QUI.Space(6);

                QLabel.text = "Button Category";
                QLabel.style = Style.Text.Small;
                QUI.BeginHorizontal((width - 6) / 2);
                {
                    QUI.Label(QLabel);
                    QUI.FlexibleSpace();
                }
                QUI.EndHorizontal();

                QLabel.text = "Button Name";
                QLabel.style = Style.Text.Small;
                QUI.BeginHorizontal((width - 6) / 2);
                {
                    QUI.Label(QLabel);
                    QUI.FlexibleSpace();
                }
                QUI.EndHorizontal();
            }
            QUI.EndHorizontal();

            QUI.Space(-SPACE_4);

            QUI.BeginHorizontal(width);
            {
                QUI.Space(4);
                DrawButtonCategory((width - 6) / 2);
                DrawButtonName((width - 6) / 2);
            }
            QUI.EndHorizontal();
        }
        void DrawButtonCategory(float width)
        {
            QUI.BeginHorizontal(width);
            {
                if(EditorApplication.isPlayingOrWillChangePlaymode)
                {
                    QLabel.text = buttonCategory.stringValue;
                    QLabel.style = Style.Text.Help;
                    QUI.Label(QLabel);
                }
                else
                {
                    //buttonCategoryIndex = DatabaseUIButtons.CategoryNameIndex(buttonCategory.stringValue); //set the index
                    QUI.BeginChangeCheck();
                    buttonCategoryIndex = EditorGUILayout.Popup(buttonCategoryIndex, DatabaseUIButtons.categoryNames.ToArray(), GUILayout.Width(width - 5));
                    if(QUI.EndChangeCheck())
                    {
                        buttonCategory.stringValue = DatabaseUIButtons.categoryNames[buttonCategoryIndex];
                        if(buttonCategory.stringValue == DUI.CUSTOM_NAME)
                        {
                            buttonNameIndex = -1;
                        }
                        serializedObject.ApplyModifiedProperties();
                    }
                }

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawButtonName(float width)
        {
            QUI.BeginHorizontal(width);
            {
                if(EditorApplication.isPlayingOrWillChangePlaymode)
                {

                    QLabel.text = buttonName.stringValue;
                    QLabel.style = Style.Text.Help;
                    QUI.Label(QLabel);
                }
                else
                {
                    if(buttonCategory.stringValue.Equals(DUI.CUSTOM_NAME))
                    {
                        QUI.PropertyField(buttonName, width - 5);
                    }
                    else
                    {
                        if(!DatabaseUIButtons.ContainsCategoryName(buttonCategory.stringValue)) //the category does not exist -> reset category and name
                        {
                            LockEditor();
                            QUI.DisplayDialog("Info",
                                                 "Button category has been reset to the default '" + DUI.UNCATEGORIZED_CATEGORY_NAME + "' value." +
                                                   "\n\n" +
                                                   "Button name has been reset to the default '" + DUI.DEFAULT_BUTTON_NAME + "' value.",
                                                 "Ok"); //inform the dev that becuase he did not add the name to the database, it has been reset to its default value
                            buttonCategory.stringValue = DUI.UNCATEGORIZED_CATEGORY_NAME; //reset the category
                            buttonCategoryIndex = DatabaseUIButtons.CategoryNameIndex(buttonCategory.stringValue); //set the index
                            buttonName.stringValue = DUI.DEFAULT_BUTTON_NAME; //reset the name
                            buttonNameIndex = DatabaseUIButtons.ItemNameIndex(buttonCategory.stringValue, buttonName.stringValue); //set the index
                            UnlockEditor();
                        }
                        else if(!DatabaseUIButtons.Contains(buttonCategory.stringValue, buttonName.stringValue)) //category does not contain the set name -> ask de dev is it should be added
                        {
                            LockEditor();
                            if(QUI.DisplayDialog("Action Required",
                                                    "The name '" + buttonName.stringValue + "' was not found in the '" + buttonCategory.stringValue + "' category." +
                                                      "\n\n" +
                                                      "Do you want to add it to the database?",
                                                    "Yes",
                                                    "No")) //ask the dev if he wants to add this name to the database
                            {
                                DatabaseUIButtons.GetCategory(buttonCategory.stringValue).AddItemName(buttonName.stringValue, true); //add the item name to the database and save
                                buttonNameIndex = DatabaseUIButtons.ItemNameIndex(buttonCategory.stringValue, buttonName.stringValue); //set the index
                                UnlockEditor();
                            }
                            else if(!DatabaseUIButtons.GetCategory(buttonCategory.stringValue).IsEmpty()) //select the first item in the category because it's not empty
                            {
                                buttonNameIndex = 0; //set the index
                                buttonName.stringValue = DatabaseUIButtons.GetCategory(buttonCategory.stringValue).itemNames[buttonNameIndex]; //get the name
                                UnlockEditor();
                            }
                            else //reset category and name
                            {
                                LockEditor();
                                QUI.DisplayDialog("Info",
                                                  "Button category has been reset to the default '" + DUI.UNCATEGORIZED_CATEGORY_NAME + "' value." +
                                                    "\n\n" +
                                                    "Button name has been reset to the default '" + DUI.DEFAULT_BUTTON_NAME + "' value.",
                                                  "Ok"); //inform the dev that becuase he did not add the name to the database, it has been reset to its default value
                                buttonCategory.stringValue = DUI.UNCATEGORIZED_CATEGORY_NAME; //reset the category
                                buttonCategoryIndex = DatabaseUIButtons.CategoryNameIndex(buttonCategory.stringValue); //set the index
                                buttonName.stringValue = DUI.DEFAULT_BUTTON_NAME; //reset the name
                                buttonNameIndex = DatabaseUIButtons.ItemNameIndex(buttonCategory.stringValue, buttonName.stringValue); //set the index
                                UnlockEditor();
                            }
                        }
                        else //category contains the set name -> get its index
                        {
                            buttonNameIndex = DatabaseUIButtons.ItemNameIndex(buttonCategory.stringValue, buttonName.stringValue); //set the index
                            serializedObject.ApplyModifiedProperties();
                        }
                        QUI.BeginChangeCheck();
                        buttonNameIndex = EditorGUILayout.Popup(buttonNameIndex, DatabaseUIButtons.GetCategory(buttonCategory.stringValue).itemNames.ToArray(), GUILayout.Width(width - 5));
                        if(QUI.EndChangeCheck())
                        {
                            buttonName.stringValue = DatabaseUIButtons.GetCategory(buttonCategory.stringValue).itemNames[buttonNameIndex];
                            serializedObject.ApplyModifiedProperties();
                        }
                    }
                }

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
            QUI.Space(SPACE_4);
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
            if(loadOnPointerEnterPunchPresetAtRuntime.boolValue) { loadOnPointerEnterStatePresetAtRuntime.boolValue = false; }
            if(loadOnPointerEnterStatePresetAtRuntime.boolValue) { loadOnPointerEnterPunchPresetAtRuntime.boolValue = false; }
            DUIUtils.DrawLoadPresetInfoMessage("OnPointerEnterLoadPresetAtRuntime",
                                               infoMessage,
                                               useOnPointerEnter.boolValue && (loadOnPointerEnterPunchPresetAtRuntime.boolValue || loadOnPointerEnterStatePresetAtRuntime.boolValue),
                                               loadOnPointerEnterPunchPresetAtRuntime.boolValue ? onPointerEnterPunchPresetCategory.stringValue : onPointerEnterStatePresetCategory.stringValue,
                                               loadOnPointerEnterPunchPresetAtRuntime.boolValue ? onPointerEnterPunchPresetName.stringValue : onPointerEnterStatePresetName.stringValue,
                                               showOnPointerEnter,
                                               ((UIButton.ButtonAnimationType)onPointerEnterAnimationType.enumValueIndex).ToString(),
                                               width);

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
                        switch(uiButton.onPointerEnterAnimationType)
                        {
                            case UIButton.ButtonAnimationType.Punch: DUIUtils.DrawPunch(uiButton.onPointerEnterPunch, uiButton, width - SPACE_8); break;
                            case UIButton.ButtonAnimationType.State: DUIUtils.DrawAnim(uiButton.onPointerEnterState, uiButton, width - SPACE_8); break;
                        }
                        QUI.Space(SPACE_4 * showOnPointerEnter.faded);
                        DrawOnPointerEnterSound(width - SPACE_8);
                        QUI.Space(SPACE_4 * showOnPointerEnter.faded);
                        DrawOnPointerEnterEvents(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerEnter.faded);
                        DrawOnPointerEnterGameEvents(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerEnter.faded);
                        DrawOnPointerEnterNavigation(width - SPACE_8);
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
            DrawPreset(onPointerEnterAnimationType,
                       ButtonAnimType.OnPointerEnter,
                       loadOnPointerEnterPunchPresetAtRuntime, onPointerEnterPunchNewPreset, onPointerEnterPunchPresetCategoryNameIndex, onPointerEnterPunchPresetCategory, onPointerEnterPunchPresetNameIndex, onPointerEnterPunchPresetName,
                       loadOnPointerEnterStatePresetAtRuntime, onPointerEnterStateNewPreset, onPointerEnterStatePresetCategoryNameIndex, onPointerEnterStatePresetCategory, onPointerEnterStatePresetNameIndex, onPointerEnterStatePresetName,
                       width);
        }
        void DrawOnPointerEnterSound(float width)
        {
            DUIUtils.DrawSound(this, "Play Sound", customOnPointerEnterSound, onPointerEnterSound, onPointerEnterSoundIndex, width);
        }
        void DrawOnPointerEnterEvents(float width)
        {
            DUIUtils.DrawUnityEvents(uiButton.OnPointerEnter.GetPersistentEventCount() > 0, showOnPointerEnterEvents, OnPointerEnter, "OnPointerEnter", width, MiniBarHeight);
        }
        void DrawOnPointerEnterGameEvents(float width)
        {
            DrawGameEvents("OnPointerEnter", showOnPointerEnterGameEvents, onPointerEnterGameEvents, width);
        }
        void DrawOnPointerEnterNavigation(float width)
        {
            DUIUtils.DrawNavigation(this, uiButton.onPointerEnterNavigation, onPointerEnterEditorNavigationData, showOnPointerEnterNavigation, UpdateAllNavigationData, true, uiButton.IsBackButton, width, MiniBarHeight);
        }

        void DrawOnPointerExit(float width)
        {
            DUIUtils.DrawBarWithEnableDisableButton("OnPointer EXIT", useOnPointerExit, showOnPointerExit, width, BarHeight);
            if(loadOnPointerExitPunchPresetAtRuntime.boolValue) { loadOnPointerExitStatePresetAtRuntime.boolValue = false; }
            if(loadOnPointerExitStatePresetAtRuntime.boolValue) { loadOnPointerExitPunchPresetAtRuntime.boolValue = false; }
            DUIUtils.DrawLoadPresetInfoMessage("OnPointerExitLoadPresetAtRuntime",
                                               infoMessage,
                                               useOnPointerExit.boolValue && (loadOnPointerExitPunchPresetAtRuntime.boolValue || loadOnPointerExitStatePresetAtRuntime.boolValue),
                                               loadOnPointerExitPunchPresetAtRuntime.boolValue ? onPointerExitPunchPresetCategory.stringValue : onPointerExitStatePresetCategory.stringValue,
                                               loadOnPointerExitPunchPresetAtRuntime.boolValue ? onPointerExitPunchPresetName.stringValue : onPointerExitStatePresetName.stringValue,
                                               showOnPointerExit,
                                               ((UIButton.ButtonAnimationType)onPointerExitAnimationType.enumValueIndex).ToString(),
                                               width);

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
                        switch(uiButton.onPointerExitAnimationType)
                        {
                            case UIButton.ButtonAnimationType.Punch: DUIUtils.DrawPunch(uiButton.onPointerExitPunch, uiButton, width - SPACE_8); break;
                            case UIButton.ButtonAnimationType.State: DUIUtils.DrawAnim(uiButton.onPointerExitState, uiButton, width - SPACE_8); break;
                        }
                        QUI.Space(SPACE_4 * showOnPointerExit.faded);
                        DrawOnPointerExitSound(width - SPACE_8);
                        QUI.Space(SPACE_4 * showOnPointerExit.faded);
                        DrawOnPointerExitEvents(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerExit.faded);
                        DrawOnPointerExitGameEvents(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerExit.faded);
                        DrawOnPointerExitNavigation(width - SPACE_8);
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
            DrawPreset(onPointerExitAnimationType,
                       ButtonAnimType.OnPointerExit,
                       loadOnPointerExitPunchPresetAtRuntime, onPointerExitPunchNewPreset, onPointerExitPunchPresetCategoryNameIndex, onPointerExitPunchPresetCategory, onPointerExitPunchPresetNameIndex, onPointerExitPunchPresetName,
                       loadOnPointerExitStatePresetAtRuntime, onPointerExitStateNewPreset, onPointerExitStatePresetCategoryNameIndex, onPointerExitStatePresetCategory, onPointerExitStatePresetNameIndex, onPointerExitStatePresetName,
                       width);
        }
        void DrawOnPointerExitSound(float width)
        {
            DUIUtils.DrawSound(this, "Play Sound", customOnPointerExitSound, onPointerExitSound, onPointerExitSoundIndex, width);
        }
        void DrawOnPointerExitEvents(float width)
        {
            DUIUtils.DrawUnityEvents(uiButton.OnPointerExit.GetPersistentEventCount() > 0, showOnPointerExitEvents, OnPointerExit, "OnPointerExit", width, MiniBarHeight);
        }
        void DrawOnPointerExitGameEvents(float width)
        {
            DrawGameEvents("OnPointerExit", showOnPointerExitGameEvents, onPointerExitGameEvents, width);
        }
        void DrawOnPointerExitNavigation(float width)
        {
            DUIUtils.DrawNavigation(this, uiButton.onPointerExitNavigation, onPointerExitEditorNavigationData, showOnPointerExitNavigation, UpdateAllNavigationData, true, uiButton.IsBackButton, width, MiniBarHeight);
        }

        void DrawOnPointerDown(float width)
        {
            DUIUtils.DrawBarWithEnableDisableButton("OnPointer DOWN", useOnPointerDown, showOnPointerDown, width, BarHeight);
            if(loadOnPointerDownPunchPresetAtRuntime.boolValue) { loadOnPointerDownStatePresetAtRuntime.boolValue = false; }
            if(loadOnPointerDownStatePresetAtRuntime.boolValue) { loadOnPointerDownPunchPresetAtRuntime.boolValue = false; }
            DUIUtils.DrawLoadPresetInfoMessage("OnPointerDownLoadPresetAtRuntime",
                                               infoMessage,
                                               useOnPointerDown.boolValue && (loadOnPointerDownPunchPresetAtRuntime.boolValue || loadOnPointerDownStatePresetAtRuntime.boolValue),
                                               loadOnPointerDownPunchPresetAtRuntime.boolValue ? onPointerDownPunchPresetCategory.stringValue : onPointerDownStatePresetCategory.stringValue,
                                               loadOnPointerDownPunchPresetAtRuntime.boolValue ? onPointerDownPunchPresetName.stringValue : onPointerDownStatePresetName.stringValue,
                                               showOnPointerDown,
                                               ((UIButton.ButtonAnimationType)onPointerDownAnimationType.enumValueIndex).ToString(),
                                               width);

            if(!useOnPointerDown.boolValue) { return; }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * showOnPointerDown.faded);
                if(QUI.BeginFadeGroup(showOnPointerDown.faded))
                {
                    QUI.BeginVertical(width - SPACE_8);
                    {
                        QUI.Space(SPACE_2 * showOnPointerDown.faded);
                        DrawOnPointerDownPreset(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerDown.faded);
                        switch(uiButton.onPointerDownAnimationType)
                        {
                            case UIButton.ButtonAnimationType.Punch: DUIUtils.DrawPunch(uiButton.onPointerDownPunch, uiButton, width - SPACE_8); break;
                            case UIButton.ButtonAnimationType.State: DUIUtils.DrawAnim(uiButton.onPointerDownState, uiButton, width - SPACE_8); break;
                        }
                        QUI.Space(SPACE_4 * showOnPointerDown.faded);
                        DrawOnPointerDownSound(width - SPACE_8);
                        QUI.Space(SPACE_4 * showOnPointerDown.faded);
                        DrawOnPointerDownEvents(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerDown.faded);
                        DrawOnPointerDownGameEvents(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerDown.faded);
                        DrawOnPointerDownNavigation(width - SPACE_8);
                        QUI.Space(SPACE_16);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();

            }
            QUI.EndHorizontal();
        }
        void DrawOnPointerDownPreset(float width)
        {
            DrawPreset(onPointerDownAnimationType,
                       ButtonAnimType.OnPointerDown,
                       loadOnPointerDownPunchPresetAtRuntime, onPointerDownPunchNewPreset, onPointerDownPunchPresetCategoryNameIndex, onPointerDownPunchPresetCategory, onPointerDownPunchPresetNameIndex, onPointerDownPunchPresetName,
                       loadOnPointerDownStatePresetAtRuntime, onPointerDownStateNewPreset, onPointerDownStatePresetCategoryNameIndex, onPointerDownStatePresetCategory, onPointerDownStatePresetNameIndex, onPointerDownStatePresetName,
                       width);
        }
        void DrawOnPointerDownSound(float width)
        {
            DUIUtils.DrawSound(this, "Play Sound", customOnPointerDownSound, onPointerDownSound, onPointerDownSoundIndex, width);
        }
        void DrawOnPointerDownEvents(float width)
        {
            DUIUtils.DrawUnityEvents(uiButton.OnPointerDown.GetPersistentEventCount() > 0, showOnPointerDownEvents, OnPointerDown, "OnPointerDown", width, MiniBarHeight);
        }
        void DrawOnPointerDownGameEvents(float width)
        {
            DrawGameEvents("OnPointerDown", showOnPointerDownGameEvents, onPointerDownGameEvents, width);
        }
        void DrawOnPointerDownNavigation(float width)
        {
            DUIUtils.DrawNavigation(this, uiButton.onPointerDownNavigation, onPointerDownEditorNavigationData, showOnPointerDownNavigation, UpdateAllNavigationData, true, uiButton.IsBackButton, width, MiniBarHeight);
        }

        void DrawOnPointerUp(float width)
        {
            DUIUtils.DrawBarWithEnableDisableButton("OnPointer UP", useOnPointerUp, showOnPointerUp, width, BarHeight);
            if(loadOnPointerUpPunchPresetAtRuntime.boolValue) { loadOnPointerUpStatePresetAtRuntime.boolValue = false; }
            if(loadOnPointerUpStatePresetAtRuntime.boolValue) { loadOnPointerUpPunchPresetAtRuntime.boolValue = false; }
            DUIUtils.DrawLoadPresetInfoMessage("OnPointerUpLoadPresetAtRuntime",
                                               infoMessage,
                                               useOnPointerUp.boolValue && (loadOnPointerUpPunchPresetAtRuntime.boolValue || loadOnPointerUpStatePresetAtRuntime.boolValue),
                                               loadOnPointerUpPunchPresetAtRuntime.boolValue ? onPointerUpPunchPresetCategory.stringValue : onPointerUpStatePresetCategory.stringValue,
                                               loadOnPointerUpPunchPresetAtRuntime.boolValue ? onPointerUpPunchPresetName.stringValue : onPointerUpStatePresetName.stringValue,
                                               showOnPointerUp,
                                               ((UIButton.ButtonAnimationType)onPointerUpAnimationType.enumValueIndex).ToString(),
                                               width);

            if(!useOnPointerUp.boolValue) { return; }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * showOnPointerUp.faded);
                if(QUI.BeginFadeGroup(showOnPointerUp.faded))
                {
                    QUI.BeginVertical(width - SPACE_8);
                    {
                        QUI.Space(SPACE_2 * showOnPointerUp.faded);
                        DrawOnPointerUpPreset(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerUp.faded);
                        switch(uiButton.onPointerUpAnimationType)
                        {
                            case UIButton.ButtonAnimationType.Punch: DUIUtils.DrawPunch(uiButton.onPointerUpPunch, uiButton, width - SPACE_8); break;
                            case UIButton.ButtonAnimationType.State: DUIUtils.DrawAnim(uiButton.onPointerUpState, uiButton, width - SPACE_8); break;
                        }
                        QUI.Space(SPACE_4 * showOnPointerUp.faded);
                        DrawOnPointerUpSound(width - SPACE_8);
                        QUI.Space(SPACE_4 * showOnPointerUp.faded);
                        DrawOnPointerUpEvents(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerUp.faded);
                        DrawOnPointerUpGameEvents(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnPointerUp.faded);
                        DrawOnPointerUpNavigation(width - SPACE_8);
                        QUI.Space(SPACE_16);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();

            }
            QUI.EndHorizontal();
        }
        void DrawOnPointerUpPreset(float width)
        {
            DrawPreset(onPointerUpAnimationType,
                       ButtonAnimType.OnPointerUp,
                       loadOnPointerUpPunchPresetAtRuntime, onPointerUpPunchNewPreset, onPointerUpPunchPresetCategoryNameIndex, onPointerUpPunchPresetCategory, onPointerUpPunchPresetNameIndex, onPointerUpPunchPresetName,
                       loadOnPointerUpStatePresetAtRuntime, onPointerUpStateNewPreset, onPointerUpStatePresetCategoryNameIndex, onPointerUpStatePresetCategory, onPointerUpStatePresetNameIndex, onPointerUpStatePresetName,
                       width);
        }
        void DrawOnPointerUpSound(float width)
        {
            DUIUtils.DrawSound(this, "Play Sound", customOnPointerUpSound, onPointerUpSound, onPointerUpSoundIndex, width);
        }
        void DrawOnPointerUpEvents(float width)
        {
            DUIUtils.DrawUnityEvents(uiButton.OnPointerUp.GetPersistentEventCount() > 0, showOnPointerUpEvents, OnPointerUp, "OnPointerUp", width, MiniBarHeight);
        }
        void DrawOnPointerUpGameEvents(float width)
        {
            DrawGameEvents("OnPointerUp", showOnPointerUpGameEvents, onPointerUpGameEvents, width);
        }
        void DrawOnPointerUpNavigation(float width)
        {
            DUIUtils.DrawNavigation(this, uiButton.onPointerUpNavigation, onPointerUpEditorNavigationData, showOnPointerUpNavigation, UpdateAllNavigationData, true, uiButton.IsBackButton, width, MiniBarHeight);
        }

        void DrawOnClick(float width)
        {
            DUIUtils.DrawBarWithEnableDisableButton("OnClick", useOnClickAnimations, showOnClick, width, BarHeight);
            if(loadOnClickPunchPresetAtRuntime.boolValue) { loadOnClickStatePresetAtRuntime.boolValue = false; }
            if(loadOnClickStatePresetAtRuntime.boolValue) { loadOnClickPunchPresetAtRuntime.boolValue = false; }
            DUIUtils.DrawLoadPresetInfoMessage("OnClickLoadPresetAtRuntime",
                                               infoMessage,
                                               useOnClickAnimations.boolValue && (loadOnClickPunchPresetAtRuntime.boolValue || loadOnClickStatePresetAtRuntime.boolValue),
                                               loadOnClickPunchPresetAtRuntime.boolValue ? onClickPunchPresetCategory.stringValue : onClickStatePresetCategory.stringValue,
                                               loadOnClickPunchPresetAtRuntime.boolValue ? onClickPunchPresetName.stringValue : onClickStatePresetName.stringValue,
                                               showOnClick,
                                               ((UIButton.ButtonAnimationType)onClickAnimationType.enumValueIndex).ToString(),
                                               width);

            if(!useOnClickAnimations.boolValue) { return; }

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
                        switch(uiButton.onClickAnimationType)
                        {
                            case UIButton.ButtonAnimationType.Punch: DUIUtils.DrawPunch(uiButton.onClickPunch, uiButton, width - SPACE_8); break;
                            case UIButton.ButtonAnimationType.State: DUIUtils.DrawAnim(uiButton.onClickState, uiButton, width - SPACE_8); break;
                        }
                        QUI.Space(SPACE_4 * showOnClick.faded);
                        DrawOnClickSound(width - SPACE_8);
                        QUI.Space(SPACE_4 * showOnClick.faded);
                        DrawOnClickEvents(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnClick.faded);
                        DrawOnClickGameEvents(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnClick.faded);
                        DrawOnClickNavigation(width - SPACE_8);
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
                QUI.QToggle("wait for animation", waitForOnClickAnimation);
                QUI.FlexibleSpace();

                QLabel.text = "single click mode";
                QLabel.style = Style.Text.Normal;

                tempFloat = QLabel.x; //save label width
                tempFloat += 80; //add dropdown width
                tempFloat += 8; //add extra space
                tempFloat += 12; //compensate for unity margins

                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat, 20);
                QUI.Space(-tempFloat + SPACE_4);

                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(SPACE_2);
                }
                QUI.EndVertical();
                QUI.PropertyField(singleClickMode, 80);
                QUI.Space(SPACE_4);
            }
            QUI.EndHorizontal();
        }
        void DrawOnClickPreset(float width)
        {
            DrawPreset(onClickAnimationType,
                       ButtonAnimType.OnClick,
                       loadOnClickPunchPresetAtRuntime, onClickPunchNewPreset, onClickPunchPresetCategoryNameIndex, onClickPunchPresetCategory, onClickPunchPresetNameIndex, onClickPunchPresetName,
                       loadOnClickStatePresetAtRuntime, onClickStateNewPreset, onClickStatePresetCategoryNameIndex, onClickStatePresetCategory, onClickStatePresetNameIndex, onClickStatePresetName,
                       width);
        }
        void DrawOnClickSound(float width)
        {
            DUIUtils.DrawSound(this, "Play Sound", customOnClickSound, onClickSound, onClickSoundIndex, width);
        }
        void DrawOnClickEvents(float width)
        {
            DUIUtils.DrawUnityEvents(uiButton.OnClick.GetPersistentEventCount() > 0, showOnClickEvents, OnClick, "OnClick", width, MiniBarHeight);
        }
        void DrawOnClickGameEvents(float width)
        {
            DrawGameEvents("OnClick", showOnClickGameEvents, onClickGameEvents, width);
        }
        void DrawOnClickNavigation(float width)
        {
            DUIUtils.DrawNavigation(this, uiButton.onClickNavigation, onClickEditorNavigationData, showOnClickNavigation, UpdateAllNavigationData, true, uiButton.IsBackButton, width, MiniBarHeight);
        }

        void DrawOnDoubleClick(float width)
        {
            DUIUtils.DrawBarWithEnableDisableButton("OnDoubleClick", useOnDoubleClick, showOnDoubleClick, width, BarHeight);
            if(loadOnDoubleClickPunchPresetAtRuntime.boolValue) { loadOnDoubleClickStatePresetAtRuntime.boolValue = false; }
            if(loadOnDoubleClickStatePresetAtRuntime.boolValue) { loadOnDoubleClickPunchPresetAtRuntime.boolValue = false; }
            DUIUtils.DrawLoadPresetInfoMessage("OnDoubleClickLoadPresetAtRuntime",
                                               infoMessage,
                                               useOnDoubleClick.boolValue && (loadOnDoubleClickPunchPresetAtRuntime.boolValue || loadOnDoubleClickStatePresetAtRuntime.boolValue),
                                               loadOnDoubleClickPunchPresetAtRuntime.boolValue ? onDoubleClickPunchPresetCategory.stringValue : onDoubleClickStatePresetCategory.stringValue,
                                               loadOnDoubleClickPunchPresetAtRuntime.boolValue ? onDoubleClickPunchPresetName.stringValue : onDoubleClickStatePresetName.stringValue,
                                               showOnDoubleClick,
                                               ((UIButton.ButtonAnimationType)onDoubleClickAnimationType.enumValueIndex).ToString(),
                                               width);

            if(!useOnDoubleClick.boolValue) { return; }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * showOnDoubleClick.faded);
                if(QUI.BeginFadeGroup(showOnDoubleClick.faded))
                {
                    QUI.BeginVertical(width - SPACE_8);
                    {
                        QUI.Space(SPACE_2 * showOnDoubleClick.faded);
                        DrawOnDoubleClickSettings(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnDoubleClick.faded);
                        DrawOnDoubleClickPreset(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnDoubleClick.faded);
                        switch(uiButton.onDoubleClickAnimationType)
                        {
                            case UIButton.ButtonAnimationType.Punch: DUIUtils.DrawPunch(uiButton.onDoubleClickPunch, uiButton, width - SPACE_8); break;
                            case UIButton.ButtonAnimationType.State: DUIUtils.DrawAnim(uiButton.onDoubleClickState, uiButton, width - SPACE_8); break;
                        }
                        QUI.Space(SPACE_4 * showOnDoubleClick.faded);
                        DrawOnDoubleClickSound(width - SPACE_8);
                        QUI.Space(SPACE_4 * showOnDoubleClick.faded);
                        DrawOnDoubleClickEvents(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnDoubleClick.faded);
                        DrawOnDoubleClickGameEvents(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnDoubleClick.faded);
                        DrawOnDoubleClickNavigation(width - SPACE_8);
                        QUI.Space(SPACE_16);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();

            }
            QUI.EndHorizontal();
        }
        void DrawOnDoubleClickSettings(float width)
        {
            QUI.BeginHorizontal(width);
            {
                QUI.QToggle("wait for animation", waitForOnDoubleClickAnimation);
                QUI.FlexibleSpace();

                QLabel.text = "register interval";
                QLabel.style = Style.Text.Normal;

                tempFloat = QLabel.x; //save label width
                tempFloat += 40; //add field width
                tempFloat += 8; //add extra space
                tempFloat += 12; //compensate for unity margins

                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat, 20);
                QUI.Space(-tempFloat + SPACE_4);

                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(SPACE_2);
                }
                QUI.EndVertical();
                QUI.PropertyField(doubleClickRegisterInterval, 40);
                QUI.Space(SPACE_4);
            }
            QUI.EndHorizontal();
        }
        void DrawOnDoubleClickPreset(float width)
        {
            DrawPreset(onDoubleClickAnimationType,
                      ButtonAnimType.OnDoubleClick,
                      loadOnDoubleClickPunchPresetAtRuntime, onDoubleClickPunchNewPreset, onDoubleClickPunchPresetCategoryNameIndex, onDoubleClickPunchPresetCategory, onDoubleClickPunchPresetNameIndex, onDoubleClickPunchPresetName,
                      loadOnDoubleClickStatePresetAtRuntime, onDoubleClickStateNewPreset, onDoubleClickStatePresetCategoryNameIndex, onDoubleClickStatePresetCategory, onDoubleClickStatePresetNameIndex, onDoubleClickStatePresetName,
                      width);
        }
        void DrawOnDoubleClickSound(float width)
        {
            DUIUtils.DrawSound(this, "Play Sound", customOnDoubleClickSound, onDoubleClickSound, onDoubleClickSoundIndex, width);
        }
        void DrawOnDoubleClickEvents(float width)
        {
            DUIUtils.DrawUnityEvents(uiButton.OnDoubleClick.GetPersistentEventCount() > 0, showOnDoubleClickEvents, OnDoubleClick, "OnDoubleClick", width, MiniBarHeight);
        }
        void DrawOnDoubleClickGameEvents(float width)
        {
            DrawGameEvents("OnDoubleClick", showOnDoubleClickGameEvents, onDoubleClickGameEvents, width);
        }
        void DrawOnDoubleClickNavigation(float width)
        {
            DUIUtils.DrawNavigation(this, uiButton.onDoubleClickNavigation, onDoubleClickEditorNavigationData, showOnDoubleClickNavigation, UpdateAllNavigationData, true, uiButton.IsBackButton, width, MiniBarHeight);
        }

        void DrawOnLongClick(float width)
        {
            DUIUtils.DrawBarWithEnableDisableButton("OnLongClick", useOnLongClick, showOnLongClick, width, BarHeight);
            if(loadOnLongClickPunchPresetAtRuntime.boolValue) { loadOnLongClickStatePresetAtRuntime.boolValue = false; }
            if(loadOnLongClickStatePresetAtRuntime.boolValue) { loadOnLongClickPunchPresetAtRuntime.boolValue = false; }
            DUIUtils.DrawLoadPresetInfoMessage("OnLongClickLoadPresetAtRuntime",
                                               infoMessage,
                                               useOnLongClick.boolValue && (loadOnLongClickPunchPresetAtRuntime.boolValue || loadOnLongClickStatePresetAtRuntime.boolValue),
                                               loadOnLongClickPunchPresetAtRuntime.boolValue ? onLongClickPunchPresetCategory.stringValue : onLongClickStatePresetCategory.stringValue,
                                               loadOnLongClickPunchPresetAtRuntime.boolValue ? onLongClickPunchPresetName.stringValue : onLongClickStatePresetName.stringValue,
                                               showOnLongClick,
                                               ((UIButton.ButtonAnimationType)onLongClickAnimationType.enumValueIndex).ToString(),
                                               width);

            if(!useOnLongClick.boolValue) { return; }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * showOnLongClick.faded);
                if(QUI.BeginFadeGroup(showOnLongClick.faded))
                {
                    QUI.BeginVertical(width - SPACE_8);
                    {
                        QUI.Space(SPACE_2 * showOnLongClick.faded);
                        DrawOnLongClickSettings(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnLongClick.faded);
                        DrawOnLongClickPreset(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnLongClick.faded);
                        switch(uiButton.onLongClickAnimationType)
                        {
                            case UIButton.ButtonAnimationType.Punch: DUIUtils.DrawPunch(uiButton.onLongClickPunch, uiButton, width - SPACE_8); break;
                            case UIButton.ButtonAnimationType.State: DUIUtils.DrawAnim(uiButton.onLongClickState, uiButton, width - SPACE_8); break;
                        }
                        QUI.Space(SPACE_4 * showOnLongClick.faded);
                        DrawOnLongClickSound(width - SPACE_8);
                        QUI.Space(SPACE_4 * showOnLongClick.faded);
                        DrawOnLongClickEvents(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnLongClick.faded);
                        DrawOnLongClickGameEvents(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOnLongClick.faded);
                        DrawOnLongClickNavigation(width - SPACE_8);
                        QUI.Space(SPACE_16);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();

            }
            QUI.EndHorizontal();
        }
        void DrawOnLongClickSettings(float width)
        {
            QUI.BeginHorizontal(width);
            {
                QUI.QToggle("wait for animation", waitForOnLongClickAnimation);
                QUI.FlexibleSpace();

                QLabel.text = "register interval";
                QLabel.style = Style.Text.Normal;

                tempFloat = QLabel.x; //save label width
                tempFloat += 40; //add field width
                tempFloat += 8; //add extra space
                tempFloat += 12; //compensate for unity margins

                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat, 20);
                QUI.Space(-tempFloat + SPACE_4);

                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(SPACE_2);
                }
                QUI.EndVertical();
                QUI.PropertyField(doubleClickRegisterInterval, 40);
                QUI.Space(SPACE_4);
            }
            QUI.EndHorizontal();
        }
        void DrawOnLongClickPreset(float width)
        {
            DrawPreset(onLongClickAnimationType,
                       ButtonAnimType.OnLongClick,
                       loadOnLongClickPunchPresetAtRuntime, onLongClickPunchNewPreset, onLongClickPunchPresetCategoryNameIndex, onLongClickPunchPresetCategory, onLongClickPunchPresetNameIndex, onLongClickPunchPresetName,
                       loadOnLongClickStatePresetAtRuntime, onLongClickStateNewPreset, onLongClickStatePresetCategoryNameIndex, onLongClickStatePresetCategory, onLongClickStatePresetNameIndex, onLongClickStatePresetName,
                       width);
        }
        void DrawOnLongClickSound(float width)
        {
            DUIUtils.DrawSound(this, "Play Sound", customOnLongClickSound, onLongClickSound, onLongClickSoundIndex, width);
        }
        void DrawOnLongClickEvents(float width)
        {
            DUIUtils.DrawUnityEvents(uiButton.OnLongClick.GetPersistentEventCount() > 0, showOnLongClickEvents, OnLongClick, "OnLongClick", width, MiniBarHeight);
        }
        void DrawOnLongClickGameEvents(float width)
        {
            DrawGameEvents("OnLongClick", showOnLongClickGameEvents, onLongClickGameEvents, width);
        }
        void DrawOnLongClickNavigation(float width)
        {
            DUIUtils.DrawNavigation(this, uiButton.onLongClickNavigation, onLongClickEditorNavigationData, showOnLongClickNavigation, UpdateAllNavigationData, true, uiButton.IsBackButton, width, MiniBarHeight);
        }

        void DrawNormalLoop(float width)
        {
            DrawLoopAnimations(ButtonLoopType.Normal, uiButton.normalLoop.Enabled, loadNormalLoopPresetAtRuntime, showNormalAnimation, normalLoopPresetCategoryIndex, normalLoopPresetCategory, normalLoopPresetNameIndex, normalLoopPresetName, normalLoopNewPreset, width);
        }

        void DrawSelectedLoop(float width)
        {
            DrawLoopAnimations(ButtonLoopType.Selected, uiButton.selectedLoop.Enabled, loadSelectedLoopPresetAtRuntime, showSelectedAnimation, selectedLoopPresetCategoryIndex, selectedLoopPresetCategory, selectedLoopPresetNameIndex, selectedLoopPresetName, selectedLoopNewPreset, width);
        }

        void DrawPunchPresetNormalViewPresetButtons(ButtonAnimType buttonAnimType, SerializedProperty presetCategoryName, SerializedProperty presetName, AnimBool newPreset, float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), width, 22);

            QUI.Space(-20);

            tempFloat = (width - SPACE_16) / 3; //button width
            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_4);
                if(QUI.GhostButton("Load Preset", QColors.Color.Blue, tempFloat * (1 - newPreset.faded), MiniBarHeight))
                {
                    LoadPunchPreset(buttonAnimType, presetCategoryName, presetName);
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
                    DeletePunchPreset(buttonAnimType, onPointerEnterPunchPresetCategory, onPointerEnterPunchPresetName);
                }
                QUI.Space(SPACE_4);
            }
            QUI.EndHorizontal();
        }
        void DrawPunchPresetNewPresetViewPresetButtons(ButtonAnimType buttonAnimType, Index presetCategoryIndex, SerializedProperty presetCategoryName, Index presetNameIndex, SerializedProperty presetName, AnimBool newPreset, float width)
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
                    switch(buttonAnimType)
                    {
                        case ButtonAnimType.OnPointerEnter: tempBool = SavePunchPreset(buttonAnimType, onPointerEnterPunchPresetCategory, onPointerEnterPunchPresetName); break;
                        case ButtonAnimType.OnPointerExit: tempBool = SavePunchPreset(buttonAnimType, onPointerExitPunchPresetCategory, onPointerExitPunchPresetName); break;
                        case ButtonAnimType.OnPointerDown: tempBool = SavePunchPreset(buttonAnimType, onPointerDownPunchPresetCategory, onPointerDownPunchPresetName); break;
                        case ButtonAnimType.OnPointerUp: tempBool = SavePunchPreset(buttonAnimType, onPointerUpPunchPresetCategory, onPointerUpPunchPresetName); break;
                        case ButtonAnimType.OnClick: tempBool = SavePunchPreset(buttonAnimType, onClickPunchPresetCategory, onClickPunchPresetName); break;
                        case ButtonAnimType.OnDoubleClick: tempBool = SavePunchPreset(buttonAnimType, onDoubleClickPunchPresetCategory, onDoubleClickPunchPresetName); break;
                        case ButtonAnimType.OnLongClick: tempBool = SavePunchPreset(buttonAnimType, onLongClickPunchPresetCategory, onLongClickPunchPresetName); break;
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
                    ResetNewPreset();
                }

                QUI.Space(SPACE_4);

                QUI.Space(tempFloat * (1 - newPreset.faded));
            }
            QUI.EndHorizontal();

            QUI.Space(-SPACE_4);
        }

        void DrawStatePresetNormalViewPresetButtons(ButtonAnimType buttonAnimType, SerializedProperty presetCategoryName, SerializedProperty presetName, AnimBool newPreset, float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), width, 22);

            QUI.Space(-20);

            tempFloat = (width - SPACE_16) / 3; //button width
            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_4);
                if(QUI.GhostButton("Load Preset", QColors.Color.Blue, tempFloat * (1 - newPreset.faded), MiniBarHeight))
                {
                    LoadStatePreset(buttonAnimType, presetCategoryName, presetName);
                }
                QUI.Space(SPACE_4);
                if(newPreset.faded > 0)
                {
                    QUI.FlexibleSpace();
                }
                if(QUI.GhostButton("New Preset", QColors.Color.Green, tempFloat * (1 - newPreset.faded), MiniBarHeight))
                {
                    NewStatePreset(newPreset, presetCategoryName);
                }
                if(newPreset.faded > 0)
                {
                    QUI.FlexibleSpace();
                }
                QUI.Space(SPACE_4);
                if(QUI.GhostButton("Delete Preset", QColors.Color.Red, tempFloat * (1 - newPreset.faded), MiniBarHeight))
                {
                    DeleteStatePreset(buttonAnimType, onPointerEnterStatePresetCategory, onPointerEnterStatePresetName);
                }
                QUI.Space(SPACE_4);
            }
            QUI.EndHorizontal();
        }
        void DrawStatePresetNewPresetViewPresetButtons(ButtonAnimType buttonAnimType, Index presetCategoryIndex, SerializedProperty presetCategoryName, Index presetNameIndex, SerializedProperty presetName, AnimBool newPreset, float width)
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
                    switch(buttonAnimType)
                    {
                        case ButtonAnimType.OnPointerEnter: tempBool = SaveStatePreset(buttonAnimType, onPointerEnterStatePresetCategory, onPointerEnterStatePresetName); break;
                        case ButtonAnimType.OnPointerExit: tempBool = SaveStatePreset(buttonAnimType, onPointerExitStatePresetCategory, onPointerExitStatePresetName); break;
                        case ButtonAnimType.OnPointerDown: tempBool = SaveStatePreset(buttonAnimType, onPointerDownStatePresetCategory, onPointerDownStatePresetName); break;
                        case ButtonAnimType.OnPointerUp: tempBool = SaveStatePreset(buttonAnimType, onPointerUpStatePresetCategory, onPointerUpStatePresetName); break;
                        case ButtonAnimType.OnClick: tempBool = SaveStatePreset(buttonAnimType, onClickStatePresetCategory, onClickStatePresetName); break;
                        case ButtonAnimType.OnDoubleClick: tempBool = SaveStatePreset(buttonAnimType, onDoubleClickStatePresetCategory, onDoubleClickStatePresetName); break;
                        case ButtonAnimType.OnLongClick: tempBool = SaveStatePreset(buttonAnimType, onLongClickStatePresetCategory, onLongClickStatePresetName); break;
                    }

                    if(tempBool) //save the new preset -> if a new preset was saved -> update the indexes
                    {
                        presetCategoryIndex.index = DatabaseStateAnimations.CategoryNameIndex(presetCategoryName.stringValue); //update the index
                        presetNameIndex.index = DatabaseStateAnimations.ItemNameIndex(presetCategoryName.stringValue, presetName.stringValue); //update the index
                    }
                }

                QUI.Space(SPACE_4);

                if(QUI.GhostButton("Cancel", QColors.Color.Red, tempFloat * newPreset.faded, MiniBarHeight)
                     || QUI.DetectKeyUp(Event.current, KeyCode.Escape))
                {
                    ResetNewPreset();
                }

                QUI.Space(SPACE_4);

                QUI.Space(tempFloat * (1 - newPreset.faded));
            }
            QUI.EndHorizontal();

            QUI.Space(-SPACE_4);
        }

        void DrawGameEvents(string tag, AnimBool show, SerializedProperty gameEvents, float width)
        {
            QUI.DrawCollapsableList("Game Events", show, gameEvents.arraySize > 0 ? QColors.Color.Blue : QColors.Color.Gray, gameEvents, width, MiniBarHeight, "Not sending any Game Events on " + tag + "... Click [+] to start...");
        }

        void LoadPunchPreset(ButtonAnimType buttonAnimType, SerializedProperty presetCategory, SerializedProperty presetName)
        {
            Punch punch = UIAnimatorUtil.GetPunch(presetCategory.stringValue, presetName.stringValue);
            UIButton targetButton;

            if(serializedObject.isEditingMultipleObjects)
            {
                Undo.RecordObjects(targets, "LoadPreset");
                switch(buttonAnimType)
                {
                    case ButtonAnimType.OnPointerEnter: for(int i = 0; i < targets.Length; i++) { targetButton = (UIButton)targets[i]; targetButton.onPointerEnterPunch = punch.Copy(); } break;
                    case ButtonAnimType.OnPointerExit: for(int i = 0; i < targets.Length; i++) { targetButton = (UIButton)targets[i]; targetButton.onPointerExitPunch = punch.Copy(); } break;
                    case ButtonAnimType.OnPointerDown: for(int i = 0; i < targets.Length; i++) { targetButton = (UIButton)targets[i]; targetButton.onPointerDownPunch = punch.Copy(); } break;
                    case ButtonAnimType.OnPointerUp: for(int i = 0; i < targets.Length; i++) { targetButton = (UIButton)targets[i]; targetButton.onPointerUpPunch = punch.Copy(); } break;
                    case ButtonAnimType.OnClick: for(int i = 0; i < targets.Length; i++) { targetButton = (UIButton)targets[i]; targetButton.onClickPunch = punch.Copy(); } break;
                    case ButtonAnimType.OnDoubleClick: for(int i = 0; i < targets.Length; i++) { targetButton = (UIButton)targets[i]; targetButton.onDoubleClickPunch = punch.Copy(); } break;
                    case ButtonAnimType.OnLongClick: for(int i = 0; i < targets.Length; i++) { targetButton = (UIButton)targets[i]; targetButton.onLongClickPunch = punch.Copy(); } break;
                }
            }
            else
            {
                Undo.RecordObject(target, "LoadPreset");
                targetButton = (UIButton)target;
                switch(buttonAnimType)
                {
                    case ButtonAnimType.OnPointerEnter: targetButton.onPointerEnterPunch = punch.Copy(); break;
                    case ButtonAnimType.OnPointerExit: targetButton.onPointerExitPunch = punch.Copy(); break;
                    case ButtonAnimType.OnPointerDown: targetButton.onPointerDownPunch = punch.Copy(); break;
                    case ButtonAnimType.OnPointerUp: targetButton.onPointerUpPunch = punch.Copy(); break;
                    case ButtonAnimType.OnClick: targetButton.onClickPunch = punch.Copy(); break;
                    case ButtonAnimType.OnDoubleClick: targetButton.onDoubleClickPunch = punch.Copy(); break;
                    case ButtonAnimType.OnLongClick: targetButton.onLongClickPunch = punch.Copy(); break;
                }
            }

            QUI.ExitGUI();
        }
        void NewPunchPreset(AnimBool newPreset, SerializedProperty presetCategory)
        {
            ResetNewPreset();
            newPreset.target = true;
            newPresetCategoryName.name = presetCategory.stringValue;
        }
        void DeletePunchPreset(ButtonAnimType buttonAnimType, SerializedProperty presetCategory, SerializedProperty presetName)
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
                    UIButton button;
                    switch(buttonAnimType)
                    {
                        case ButtonAnimType.OnPointerEnter:
                            for(int i = 0; i < targets.Length; i++)
                            {
                                button = (UIButton)targets[i];
                                if(button.onPointerEnterPunchPresetCategory.Equals(presetCategory.stringValue)
                                   && button.onPointerEnterPunchPresetName.Equals(presetName.stringValue))
                                {
                                    button.onPointerEnterPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                                    button.onPointerEnterPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                }
                            }
                            break;
                        case ButtonAnimType.OnPointerExit:
                            for(int i = 0; i < targets.Length; i++)
                            {
                                button = (UIButton)targets[i];
                                if(button.onPointerExitPunchPresetCategory.Equals(presetCategory.stringValue)
                                   && button.onPointerExitPunchPresetName.Equals(presetName.stringValue))
                                {
                                    button.onPointerExitPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                                    button.onPointerExitPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                }
                            }
                            break;
                        case ButtonAnimType.OnPointerDown:
                            for(int i = 0; i < targets.Length; i++)
                            {
                                button = (UIButton)targets[i];
                                if(button.onPointerDownPunchPresetCategory.Equals(presetCategory.stringValue)
                                   && button.onPointerDownPunchPresetName.Equals(presetName.stringValue))
                                {
                                    button.onPointerDownPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                                    button.onPointerDownPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                }
                            }
                            break;
                        case ButtonAnimType.OnPointerUp:
                            for(int i = 0; i < targets.Length; i++)
                            {
                                button = (UIButton)targets[i];
                                if(button.onPointerUpPunchPresetCategory.Equals(presetCategory.stringValue)
                                   && button.onPointerUpPunchPresetName.Equals(presetName.stringValue))
                                {
                                    button.onPointerUpPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                                    button.onPointerUpPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                }
                            }
                            break;
                        case ButtonAnimType.OnClick:
                            for(int i = 0; i < targets.Length; i++)
                            {
                                button = (UIButton)targets[i];
                                if(button.onClickPunchPresetCategory.Equals(presetCategory.stringValue)
                                   && button.onClickPunchPresetName.Equals(presetName.stringValue))
                                {
                                    button.onClickPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                                    button.onClickPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                }
                            }
                            break;
                        case ButtonAnimType.OnDoubleClick:
                            for(int i = 0; i < targets.Length; i++)
                            {
                                button = (UIButton)targets[i];
                                if(button.onDoubleClickPunchPresetCategory.Equals(presetCategory.stringValue)
                                   && button.onDoubleClickPunchPresetName.Equals(presetName.stringValue))
                                {
                                    button.onDoubleClickPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                                    button.onDoubleClickPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                }
                            }
                            break;
                        case ButtonAnimType.OnLongClick:
                            for(int i = 0; i < targets.Length; i++)
                            {
                                button = (UIButton)targets[i];
                                if(button.onLongClickPunchPresetCategory.Equals(presetCategory.stringValue)
                                   && button.onLongClickPunchPresetName.Equals(presetName.stringValue))
                                {
                                    button.onLongClickPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                                    button.onLongClickPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
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
        bool SavePunchPreset(ButtonAnimType buttonAnimType, SerializedProperty presetCategory, SerializedProperty presetName)
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

            switch(buttonAnimType) //create the new preset
            {
                case ButtonAnimType.OnPointerEnter: DatabasePunchAnimations.GetCategory(newPresetCategoryName.name).AddPunchData(UIAnimatorUtil.CreatePunchPreset(newPresetCategoryName.name, newPresetName.name, uiButton.onPointerEnterPunch.Copy())); break;
                case ButtonAnimType.OnPointerExit: DatabasePunchAnimations.GetCategory(newPresetCategoryName.name).AddPunchData(UIAnimatorUtil.CreatePunchPreset(newPresetCategoryName.name, newPresetName.name, uiButton.onPointerExitPunch.Copy())); break;
                case ButtonAnimType.OnPointerDown: DatabasePunchAnimations.GetCategory(newPresetCategoryName.name).AddPunchData(UIAnimatorUtil.CreatePunchPreset(newPresetCategoryName.name, newPresetName.name, uiButton.onPointerDownPunch.Copy())); break;
                case ButtonAnimType.OnPointerUp: DatabasePunchAnimations.GetCategory(newPresetCategoryName.name).AddPunchData(UIAnimatorUtil.CreatePunchPreset(newPresetCategoryName.name, newPresetName.name, uiButton.onPointerUpPunch.Copy())); break;
                case ButtonAnimType.OnClick: DatabasePunchAnimations.GetCategory(newPresetCategoryName.name).AddPunchData(UIAnimatorUtil.CreatePunchPreset(newPresetCategoryName.name, newPresetName.name, uiButton.onClickPunch.Copy())); break;
                case ButtonAnimType.OnDoubleClick: DatabasePunchAnimations.GetCategory(newPresetCategoryName.name).AddPunchData(UIAnimatorUtil.CreatePunchPreset(newPresetCategoryName.name, newPresetName.name, uiButton.onDoubleClickPunch.Copy())); break;
                case ButtonAnimType.OnLongClick: DatabasePunchAnimations.GetCategory(newPresetCategoryName.name).AddPunchData(UIAnimatorUtil.CreatePunchPreset(newPresetCategoryName.name, newPresetName.name, uiButton.onLongClickPunch.Copy())); break;
            }

            if(serializedObject.isEditingMultipleObjects)
            {
                UIButton button;
                switch(buttonAnimType)
                {
                    case ButtonAnimType.OnPointerEnter:
                        for(int i = 0; i < targets.Length; i++)
                        {
                            button = (UIButton)targets[i];
                            button.onPointerEnterPunchPresetCategory = newPresetCategoryName.name;
                            button.onPointerEnterPunchPresetName = newPresetName.name;
                        }
                        break;
                    case ButtonAnimType.OnPointerExit:
                        for(int i = 0; i < targets.Length; i++)
                        {
                            button = (UIButton)targets[i];
                            button.onPointerExitPunchPresetCategory = newPresetCategoryName.name;
                            button.onPointerExitPunchPresetName = newPresetName.name;
                        }
                        break;
                    case ButtonAnimType.OnPointerDown:
                        for(int i = 0; i < targets.Length; i++)
                        {
                            button = (UIButton)targets[i];
                            button.onPointerDownPunchPresetCategory = newPresetCategoryName.name;
                            button.onPointerDownPunchPresetName = newPresetName.name;
                        }
                        break;
                    case ButtonAnimType.OnPointerUp:
                        for(int i = 0; i < targets.Length; i++)
                        {
                            button = (UIButton)targets[i];
                            button.onPointerUpPunchPresetCategory = newPresetCategoryName.name;
                            button.onPointerUpPunchPresetName = newPresetName.name;
                        }
                        break;
                    case ButtonAnimType.OnClick:
                        for(int i = 0; i < targets.Length; i++)
                        {
                            button = (UIButton)targets[i];
                            button.onClickPunchPresetCategory = newPresetCategoryName.name;
                            button.onClickPunchPresetName = newPresetName.name;
                        }
                        break;
                    case ButtonAnimType.OnDoubleClick:
                        for(int i = 0; i < targets.Length; i++)
                        {
                            button = (UIButton)targets[i];
                            button.onDoubleClickPunchPresetCategory = newPresetCategoryName.name;
                            button.onDoubleClickPunchPresetName = newPresetName.name;
                        }
                        break;
                    case ButtonAnimType.OnLongClick:
                        for(int i = 0; i < targets.Length; i++)
                        {
                            button = (UIButton)targets[i];
                            button.onLongClickPunchPresetCategory = newPresetCategoryName.name;
                            button.onLongClickPunchPresetName = newPresetName.name;
                        }
                        break;
                }
            }

            presetCategory.stringValue = newPresetCategoryName.name;
            presetName.stringValue = newPresetName.name;

            serializedObject.ApplyModifiedProperties();
            ResetNewPreset();
            return true; //return true that a new preset has been created
        }

        void LoadStatePreset(ButtonAnimType buttonAnimType, SerializedProperty presetCategory, SerializedProperty presetName)
        {
            Anim state = UIAnimatorUtil.GetStateAnim(presetCategory.stringValue, presetName.stringValue);
            UIButton targetButton;

            if(serializedObject.isEditingMultipleObjects)
            {
                Undo.RecordObjects(targets, "LoadPreset");
                switch(buttonAnimType)
                {
                    case ButtonAnimType.OnPointerEnter: for(int i = 0; i < targets.Length; i++) { targetButton = (UIButton)targets[i]; targetButton.onPointerEnterState = state.Copy(); } break;
                    case ButtonAnimType.OnPointerExit: for(int i = 0; i < targets.Length; i++) { targetButton = (UIButton)targets[i]; targetButton.onPointerExitState = state.Copy(); } break;
                    case ButtonAnimType.OnPointerDown: for(int i = 0; i < targets.Length; i++) { targetButton = (UIButton)targets[i]; targetButton.onPointerDownState = state.Copy(); } break;
                    case ButtonAnimType.OnPointerUp: for(int i = 0; i < targets.Length; i++) { targetButton = (UIButton)targets[i]; targetButton.onPointerUpState = state.Copy(); } break;
                    case ButtonAnimType.OnClick: for(int i = 0; i < targets.Length; i++) { targetButton = (UIButton)targets[i]; targetButton.onClickState = state.Copy(); } break;
                    case ButtonAnimType.OnDoubleClick: for(int i = 0; i < targets.Length; i++) { targetButton = (UIButton)targets[i]; targetButton.onDoubleClickState = state.Copy(); } break;
                    case ButtonAnimType.OnLongClick: for(int i = 0; i < targets.Length; i++) { targetButton = (UIButton)targets[i]; targetButton.onLongClickState = state.Copy(); } break;
                }
            }
            else
            {
                Undo.RecordObject(target, "LoadPreset");
                targetButton = (UIButton)target;
                switch(buttonAnimType)
                {
                    case ButtonAnimType.OnPointerEnter: targetButton.onPointerEnterState = state.Copy(); break;
                    case ButtonAnimType.OnPointerExit: targetButton.onPointerExitState = state.Copy(); break;
                    case ButtonAnimType.OnPointerDown: targetButton.onPointerDownState = state.Copy(); break;
                    case ButtonAnimType.OnPointerUp: targetButton.onPointerUpState = state.Copy(); break;
                    case ButtonAnimType.OnClick: targetButton.onClickState = state.Copy(); break;
                    case ButtonAnimType.OnDoubleClick: targetButton.onDoubleClickState = state.Copy(); break;
                    case ButtonAnimType.OnLongClick: targetButton.onLongClickState = state.Copy(); break;
                }
            }

            QUI.ExitGUI();
        }
        void NewStatePreset(AnimBool newPreset, SerializedProperty presetCategory)
        {
            ResetNewPreset();
            newPreset.target = true;
            newPresetCategoryName.name = presetCategory.stringValue;
        }
        void DeleteStatePreset(ButtonAnimType buttonAnimType, SerializedProperty presetCategory, SerializedProperty presetName)
        {
            if(QUI.DisplayDialog("Delete Preset",
                                            "Are you sure you want to delete the '" + presetName.stringValue + "' preset from the '" + presetCategory.stringValue + "' preset category?",
                                            "Yes",
                                            "No"))
            {

                DatabaseStateAnimations.GetCategory(presetCategory.stringValue).DeleteAnimData(presetName.stringValue, UIAnimatorUtil.RELATIVE_PATH_STATE_ANIM_DATA);
                if(DatabaseStateAnimations.GetCategory(presetCategory.stringValue).IsEmpty()) //category is empty -> remove it from the database (sanity check)
                {
                    DatabaseStateAnimations.RemoveCategory(presetCategory.stringValue, UIAnimatorUtil.RELATIVE_PATH_STATE_ANIM_DATA, true);
                }

                if(serializedObject.isEditingMultipleObjects)
                {
                    UIButton button;
                    switch(buttonAnimType)
                    {
                        case ButtonAnimType.OnPointerEnter:
                            for(int i = 0; i < targets.Length; i++)
                            {
                                button = (UIButton)targets[i];
                                if(button.onPointerEnterStatePresetCategory.Equals(presetCategory.stringValue)
                                   && button.onPointerEnterStatePresetName.Equals(presetName.stringValue))
                                {
                                    button.onPointerEnterStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                                    button.onPointerEnterStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                }
                            }
                            break;
                        case ButtonAnimType.OnPointerExit:
                            for(int i = 0; i < targets.Length; i++)
                            {
                                button = (UIButton)targets[i];
                                if(button.onPointerExitStatePresetCategory.Equals(presetCategory.stringValue)
                                   && button.onPointerExitStatePresetName.Equals(presetName.stringValue))
                                {
                                    button.onPointerExitStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                                    button.onPointerExitStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                }
                            }
                            break;
                        case ButtonAnimType.OnPointerDown:
                            for(int i = 0; i < targets.Length; i++)
                            {
                                button = (UIButton)targets[i];
                                if(button.onPointerDownStatePresetCategory.Equals(presetCategory.stringValue)
                                   && button.onPointerDownStatePresetName.Equals(presetName.stringValue))
                                {
                                    button.onPointerDownStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                                    button.onPointerDownStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                }
                            }
                            break;
                        case ButtonAnimType.OnPointerUp:
                            for(int i = 0; i < targets.Length; i++)
                            {
                                button = (UIButton)targets[i];
                                if(button.onPointerUpStatePresetCategory.Equals(presetCategory.stringValue)
                                   && button.onPointerUpStatePresetName.Equals(presetName.stringValue))
                                {
                                    button.onPointerUpStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                                    button.onPointerUpStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                }
                            }
                            break;
                        case ButtonAnimType.OnClick:
                            for(int i = 0; i < targets.Length; i++)
                            {
                                button = (UIButton)targets[i];
                                if(button.onClickStatePresetCategory.Equals(presetCategory.stringValue)
                                   && button.onClickStatePresetName.Equals(presetName.stringValue))
                                {
                                    button.onClickStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                                    button.onClickStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                }
                            }
                            break;
                        case ButtonAnimType.OnDoubleClick:
                            for(int i = 0; i < targets.Length; i++)
                            {
                                button = (UIButton)targets[i];
                                if(button.onDoubleClickStatePresetCategory.Equals(presetCategory.stringValue)
                                   && button.onDoubleClickStatePresetName.Equals(presetName.stringValue))
                                {
                                    button.onDoubleClickStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                                    button.onDoubleClickStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                }
                            }
                            break;
                        case ButtonAnimType.OnLongClick:
                            for(int i = 0; i < targets.Length; i++)
                            {
                                button = (UIButton)targets[i];
                                if(button.onLongClickStatePresetCategory.Equals(presetCategory.stringValue)
                                   && button.onLongClickStatePresetName.Equals(presetName.stringValue))
                                {
                                    button.onLongClickStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                                    button.onLongClickStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
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
        bool SaveStatePreset(ButtonAnimType buttonAnimType, SerializedProperty presetCategory, SerializedProperty presetName)
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

            if(DatabaseStateAnimations.Contains(newPresetCategoryName.name, newPresetName.name)) //test if the database contains the new preset name
            {
                QUI.DisplayDialog("Info",
                                  "There is another preset with the '" + newPresetName + "' preset name in the '" + newPresetCategoryName.name + "' preset category." +
                                    "\n\n" +
                                    "Try a different preset name maybe?",
                                  "Ok");
                return false; //return false that a new preset was not created
            }

            if(!DatabaseStateAnimations.ContainsCategory(newPresetCategoryName.name)) //test if the database contains the new category
            {
                DatabaseStateAnimations.AddCategory(newPresetCategoryName.name, UIAnimatorUtil.RELATIVE_PATH_STATE_ANIM_DATA, true); //create the new category 
            }

            switch(buttonAnimType) //create the new preset
            {
                case ButtonAnimType.OnPointerEnter: DatabaseStateAnimations.GetCategory(newPresetCategoryName.name).AddAnimData(UIAnimatorUtil.CreateStateAnimPreset(newPresetCategoryName.name, newPresetName.name, uiButton.onPointerEnterState.Copy())); break;
                case ButtonAnimType.OnPointerExit: DatabaseStateAnimations.GetCategory(newPresetCategoryName.name).AddAnimData(UIAnimatorUtil.CreateStateAnimPreset(newPresetCategoryName.name, newPresetName.name, uiButton.onPointerExitState.Copy())); break;
                case ButtonAnimType.OnPointerDown: DatabaseStateAnimations.GetCategory(newPresetCategoryName.name).AddAnimData(UIAnimatorUtil.CreateStateAnimPreset(newPresetCategoryName.name, newPresetName.name, uiButton.onPointerDownState.Copy())); break;
                case ButtonAnimType.OnPointerUp: DatabaseStateAnimations.GetCategory(newPresetCategoryName.name).AddAnimData(UIAnimatorUtil.CreateStateAnimPreset(newPresetCategoryName.name, newPresetName.name, uiButton.onPointerUpState.Copy())); break;
                case ButtonAnimType.OnClick: DatabaseStateAnimations.GetCategory(newPresetCategoryName.name).AddAnimData(UIAnimatorUtil.CreateStateAnimPreset(newPresetCategoryName.name, newPresetName.name, uiButton.onClickState.Copy())); break;
                case ButtonAnimType.OnDoubleClick: DatabaseStateAnimations.GetCategory(newPresetCategoryName.name).AddAnimData(UIAnimatorUtil.CreateStateAnimPreset(newPresetCategoryName.name, newPresetName.name, uiButton.onDoubleClickState.Copy())); break;
                case ButtonAnimType.OnLongClick: DatabaseStateAnimations.GetCategory(newPresetCategoryName.name).AddAnimData(UIAnimatorUtil.CreateStateAnimPreset(newPresetCategoryName.name, newPresetName.name, uiButton.onLongClickState.Copy())); break;
            }

            if(serializedObject.isEditingMultipleObjects)
            {
                UIButton button;
                switch(buttonAnimType)
                {
                    case ButtonAnimType.OnPointerEnter:
                        for(int i = 0; i < targets.Length; i++)
                        {
                            button = (UIButton)targets[i];
                            button.onPointerEnterStatePresetCategory = newPresetCategoryName.name;
                            button.onPointerEnterStatePresetName = newPresetName.name;
                        }
                        break;
                    case ButtonAnimType.OnPointerExit:
                        for(int i = 0; i < targets.Length; i++)
                        {
                            button = (UIButton)targets[i];
                            button.onPointerExitStatePresetCategory = newPresetCategoryName.name;
                            button.onPointerExitStatePresetName = newPresetName.name;
                        }
                        break;
                    case ButtonAnimType.OnPointerDown:
                        for(int i = 0; i < targets.Length; i++)
                        {
                            button = (UIButton)targets[i];
                            button.onPointerDownStatePresetCategory = newPresetCategoryName.name;
                            button.onPointerDownStatePresetName = newPresetName.name;
                        }
                        break;
                    case ButtonAnimType.OnPointerUp:
                        for(int i = 0; i < targets.Length; i++)
                        {
                            button = (UIButton)targets[i];
                            button.onPointerUpStatePresetCategory = newPresetCategoryName.name;
                            button.onPointerUpStatePresetName = newPresetName.name;
                        }
                        break;
                    case ButtonAnimType.OnClick:
                        for(int i = 0; i < targets.Length; i++)
                        {
                            button = (UIButton)targets[i];
                            button.onClickStatePresetCategory = newPresetCategoryName.name;
                            button.onClickStatePresetName = newPresetName.name;
                        }
                        break;
                    case ButtonAnimType.OnDoubleClick:
                        for(int i = 0; i < targets.Length; i++)
                        {
                            button = (UIButton)targets[i];
                            button.onDoubleClickStatePresetCategory = newPresetCategoryName.name;
                            button.onDoubleClickStatePresetName = newPresetName.name;
                        }
                        break;
                    case ButtonAnimType.OnLongClick:
                        for(int i = 0; i < targets.Length; i++)
                        {
                            button = (UIButton)targets[i];
                            button.onLongClickStatePresetCategory = newPresetCategoryName.name;
                            button.onLongClickStatePresetName = newPresetName.name;
                        }
                        break;
                }
            }

            presetCategory.stringValue = newPresetCategoryName.name;
            presetName.stringValue = newPresetName.name;

            serializedObject.ApplyModifiedProperties();
            ResetNewPreset();
            return true; //return true that a new preset has been created
        }

        void ResetNewPreset()
        {
            onPointerEnterPunchNewPreset.target = false;
            onPointerEnterStateNewPreset.target = false;

            onPointerExitPunchNewPreset.target = false;
            onPointerExitStateNewPreset.target = false;

            onPointerDownPunchNewPreset.target = false;
            onPointerDownStateNewPreset.target = false;

            onPointerUpPunchNewPreset.target = false;
            onPointerUpStateNewPreset.target = false;

            onClickPunchNewPreset.target = false;
            onClickStateNewPreset.target = false;

            onDoubleClickPunchNewPreset.target = false;
            onDoubleClickStateNewPreset.target = false;

            onLongClickPunchNewPreset.target = false;
            onLongClickStateNewPreset.target = false;

            normalLoopNewPreset.target = false;
            selectedLoopNewPreset.target = false;

            createNewCategoryName.target = false;
            newPresetCategoryName.name = string.Empty;
            newPresetName.name = string.Empty;

            QUI.ResetKeyboardFocus();
        }

        void DrawNavigation(NavigationPointerData navigationPointerData, EditorNavigationPointerData editorNavigationPointerData, AnimBool show, float width)
        {
            if(!UIManager.IsNavigationEnabled)
            {
                return;
            }

            if(QUI.GhostBar("Navigation", navigationPointerData.Enabled ? QColors.Color.Blue : QColors.Color.Gray, show, width, MiniBarHeight))
            {
                show.target = !show.target;
            }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * show.faded);
                if(QUI.BeginFadeGroup(show.faded))
                {
                    QUI.BeginVertical(width - SPACE_8);
                    {
                        QUI.Space(SPACE_2);
                        DrawNavigationData(navigationPointerData, editorNavigationPointerData, width - SPACE_8);
                        QUI.Space(SPACE_8);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();

        }
        void DrawNavigationData(NavigationPointerData navData, EditorNavigationPointerData editorNavData, float width = DUI.GLOBAL_EDITOR_WIDTH)
        {
            if(uiButton.IsBackButton)
            {
                navData.addToNavigationHistory = false;
            }

            QUI.BeginHorizontal(width);
            {
                QLabel.text = "Add To Navigation History";
                QLabel.style = Style.Text.Normal;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, navData.addToNavigationHistory ? QColors.Color.Blue : QColors.Color.Gray), QLabel.x + 28, 18);
                QUI.Space(-QLabel.x - 22);

                bool tempBool = navData.addToNavigationHistory;
                QUI.BeginChangeCheck();
                tempBool = QUI.Toggle(tempBool);
                if(QUI.EndChangeCheck())
                {
                    if(!uiButton.IsBackButton)
                    {
                        Undo.RecordObject(uiButton, "ToggleNavHistory");
                        navData.addToNavigationHistory = tempBool;
                    }
                }

                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(2);
                }
                QUI.EndVertical();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2);
            DrawNavigationDataList(NavigationType.Show, navData.show, editorNavData.showIndex, width);
            QUI.Space(SPACE_4);
            DrawNavigationDataList(NavigationType.Hide, navData.hide, editorNavData.hideIndex, width);
        }
        void DrawNavigationDataList(NavigationType navigationType, List<NavigationPointer> list, List<EditorNavigationPointer> listIndex, float width)
        {
            if(listIndex.Count != list.Count) //if the listIndex and the list lists don't have the same count -> update all nav data because something is wrong (this should not happen, it's a sanity check)
            {
                UpdateAllNavigationData();
            }

            QUI.SetGUIBackgroundColor(navigationType == NavigationType.Show ? QUI.AccentColorGreen : QUI.AccentColorRed);
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, navigationType == NavigationType.Show ? QColors.Color.Green : QColors.Color.Red), width, 16);
            QUI.ResetColors();
            QUI.Space(-18);

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8);

                QLabel.text = navigationType == NavigationType.Show ? "SHOW" : "HIDE";
                QLabel.style = Style.Text.Small;
                QUI.Label(QLabel);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.BeginHorizontal(width);
            {
                QUI.BeginVertical(width);
                {
                    if(uiButton.IsBackButton) //this is a Back button
                    {
                        list.Clear(); //clear list
                        listIndex.Clear(); //clear list index
                        QUI.BeginHorizontal(width);
                        {
                            QUI.LabelWithBackground("This is a 'Back' button..."); //tell the dev that this is a back button -> thus the nav is not available (maybe try using an InfoMessage here, instead of a label???)
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();
                    }
                    else //this is NOT a Back button
                    {
                        QUI.Space(-SPACE_2);

                        tempFloat = (width - SPACE_8 - 24) / 2; //column width

                        if(list.Count > 0) //list is not empty -> draw the headers
                        {
                            QUI.BeginHorizontal(width);
                            {
                                QUI.Space(SPACE_8);

                                QLabel.text = "Element Category";
                                QLabel.style = Style.Text.Tiny;
                                QUI.Label(QLabel.text, Style.Text.Tiny, tempFloat - 4);

                                QUI.Space(SPACE_4);

                                QLabel.text = "Element Name";
                                QLabel.style = Style.Text.Tiny;
                                QUI.Label(QLabel.text, Style.Text.Tiny, tempFloat - 8);

                                QUI.FlexibleSpace();
                            }
                            QUI.EndHorizontal();
                            QUI.Space(-SPACE_4);
                        }

                        for(int i = 0; i < list.Count; i++)
                        {
                            QUI.BeginHorizontal(width);
                            {

                                QUI.SetGUIBackgroundColor(navigationType == NavigationType.Show ? QUI.AccentColorGreen : QUI.AccentColorRed);

                                listIndex[i].categoryIndex = DatabaseUIElements.CategoryNameIndex(list[i].category); //set the index
                                QUI.BeginChangeCheck();
                                listIndex[i].categoryIndex = EditorGUILayout.Popup(listIndex[i].categoryIndex, DatabaseUIElements.categoryNames.ToArray(), GUILayout.Width(tempFloat));
                                if(QUI.EndChangeCheck())
                                {
                                    Undo.RecordObject(uiButton, "UpdateNavigation");
                                    list[i].category = DatabaseUIElements.categoryNames[listIndex[i].categoryIndex];

                                    if(list[i].category.Equals(DUI.CUSTOM_NAME))
                                    {
                                        listIndex[i].nameIndex = -1;
                                    }
                                }

                                if(list[i].category.Equals(DUI.CUSTOM_NAME))
                                {
                                    string customName = list[i].name;
                                    QUI.BeginChangeCheck();
                                    customName = EditorGUILayout.DelayedTextField(customName, GUILayout.Width(tempFloat));
                                    if(QUI.EndChangeCheck())
                                    {
                                        Undo.RecordObject(uiButton, "UpdateNavigation");
                                        list[i].name = customName;
                                    }
                                }
                                else
                                {
                                    if(!DatabaseUIElements.ContainsCategoryName(list[i].category)) //the category does not exist -> reset category and name
                                    {
                                        QUI.DisplayDialog("Info",
                                                             "Element category has been reset to the default '" + DUI.UNCATEGORIZED_CATEGORY_NAME + "' value." +
                                                               "\n\n" +
                                                               "Element name has been reset to the default '" + DUI.DEFAULT_ELEMENT_NAME + "' value.",
                                                             "Ok"); //inform the dev that becuase he did not add the name to the database, it has been reset to its default value
                                        list[i].category = DUI.UNCATEGORIZED_CATEGORY_NAME; //reset the category
                                        listIndex[i].categoryIndex = DatabaseUIElements.CategoryNameIndex(list[i].category); //set the index
                                        list[i].name = DUI.DEFAULT_ELEMENT_NAME; //reset the name
                                        listIndex[i].nameIndex = DatabaseUIElements.ItemNameIndex(list[i].category, list[i].name); //set the index
                                    }
                                    else if(!DatabaseUIElements.Contains(list[i].category, list[i].name)) //category does not contain the set name -> ask de dev is it should be added
                                    {
                                        if(QUI.DisplayDialog("Action Required",
                                                                "The name '" + list[i].name + "' was not found in the '" + list[i].category + "' category." +
                                                                  "\n\n" +
                                                                  "Do you want to add it to the database?",
                                                                "Yes",
                                                                "No")) //ask the dev if he wants to add this name to the database
                                        {
                                            DatabaseUIElements.GetCategory(list[i].category).AddItemName(list[i].name, true); //add the item name to the database and save
                                            listIndex[i].nameIndex = DatabaseUIElements.ItemNameIndex(list[i].category, list[i].name); //set the index
                                        }
                                        else if(!DatabaseUIElements.GetCategory(list[i].category).IsEmpty()) //select the first item in the category because it's not empty
                                        {
                                            listIndex[i].nameIndex = 0; //set the index
                                            list[i].name = DatabaseUIElements.GetCategory(list[i].category).itemNames[listIndex[i].nameIndex]; //get the name
                                        }
                                        else //reset category and name
                                        {
                                            QUI.DisplayDialog("Info",
                                                              "Element category has been reset to the default '" + DUI.UNCATEGORIZED_CATEGORY_NAME + "' value." +
                                                                "\n\n" +
                                                                "Element name has been reset to the default '" + DUI.DEFAULT_ELEMENT_NAME + "' value.",
                                                              "Ok"); //inform the dev that becuase he did not add the name to the database, it has been reset to its default value
                                            list[i].category = DUI.UNCATEGORIZED_CATEGORY_NAME; //reset the category
                                            listIndex[i].categoryIndex = DatabaseUIElements.CategoryNameIndex(list[i].category); //set the index
                                            list[i].name = DUI.DEFAULT_ELEMENT_NAME; //reset the name
                                            listIndex[i].nameIndex = DatabaseUIElements.ItemNameIndex(list[i].category, list[i].name); //set the index
                                        }
                                    }
                                    else //category contains the set name -> get its index
                                    {
                                        listIndex[i].nameIndex = DatabaseUIElements.ItemNameIndex(list[i].category, list[i].name); //set the index
                                    }
                                    QUI.BeginChangeCheck();
                                    listIndex[i].nameIndex = EditorGUILayout.Popup(listIndex[i].nameIndex, DatabaseUIElements.GetCategory(list[i].category).itemNames.ToArray(), GUILayout.Width(tempFloat));
                                    if(QUI.EndChangeCheck())
                                    {
                                        Undo.RecordObject(uiButton, "UpdateNavigation");
                                        list[i].name = DatabaseUIElements.GetCategory(list[i].category).itemNames[listIndex[i].nameIndex];
                                    }
                                }

                                QUI.ResetColors();

                                QUI.BeginVertical(16, 16);
                                {
                                    if(QUI.ButtonMinus())
                                    {
                                        Undo.RecordObject(uiButton, "Removed NavigationPointer");
                                        list.RemoveAt(i);
                                        listIndex.RemoveAt(i);
                                        DUIUtils.UpdateNavigationDataList(list, listIndex);
                                        QUI.ExitGUI();
                                    }
                                }
                                QUI.EndVertical();

                                QUI.Space(SPACE_4);
                            }
                            QUI.EndHorizontal();
                        }

                        if(list.Count == 0)
                        {
                            QUI.Space(SPACE_2);
                        }

                        QUI.BeginHorizontal(width);
                        {
                            if(list.Count == 0)
                            {
                                QUI.Space(SPACE_8);
                                QLabel.text = "No UIElements will get " + (navigationType == NavigationType.Show ? "shown" : "hidden") + "... Click [+] to start...";
                                QLabel.style = Style.Text.Help;
                                QUI.Label(QLabel);
                            }

                            QUI.FlexibleSpace();

                            QUI.BeginVertical(16, 16);
                            {
                                if(QUI.ButtonPlus())
                                {
                                    Undo.RecordObject(uiButton, "Added NavigationPointer");
                                    list.Add(new NavigationPointer(DUI.UNCATEGORIZED_CATEGORY_NAME, DUI.DEFAULT_ELEMENT_NAME));
                                    listIndex.Add(new EditorNavigationPointer(DUIData.Instance.DatabaseUIElements.categoryNames.IndexOf(DUI.UNCATEGORIZED_CATEGORY_NAME), DUIData.Instance.DatabaseUIElements.GetCategory(DUI.UNCATEGORIZED_CATEGORY_NAME).itemNames.IndexOf(DUI.DEFAULT_ELEMENT_NAME)));
                                    DUIUtils.UpdateNavigationDataList(list, listIndex);
                                }
                            }
                            QUI.EndVertical();

                            QUI.Space(SPACE_4);
                        }
                        QUI.EndHorizontal();
                    }
                }
                QUI.EndVertical();
            }
            QUI.EndHorizontal();

            if(list.Count > 0) //list is not empty -> add some space
            {
                QUI.Space(SPACE_8);
            }
        }

        void DrawLoopBar(string title, bool enabled, SerializedProperty loadPresetAtRuntime, AnimBool show, AnimBool newPreset, float width, float height)
        {
            if(QUI.GhostBar(title, !enabled && !loadPresetAtRuntime.boolValue ? QColors.Color.Gray : QColors.Color.Blue, show, width - height * 4, height))
            {
                show.target = !show.target;
                if(!show.target) //if closing -> reset any new preset settings
                {
                    if(newPreset.target)
                    {
                        ResetNewPreset();
                        QUI.ExitGUI();
                    }
                }
            }
        }
        void DrawLoopButtons(ButtonLoopType loopType, float width, float height)
        {
            switch(loopType)
            {
                case ButtonLoopType.Normal:
                    if(QUI.GhostButton("M", uiButton.normalLoop.move.enabled ? QColors.Color.Green : QColors.Color.Gray, BarHeight, BarHeight, showNormalAnimation.target))
                    {
                        Undo.RecordObject(target, "ToggleMove" + loopType);
                        uiButton.normalLoop.move.enabled = !uiButton.normalLoop.move.enabled;
                        if(uiButton.normalLoop.move.enabled) { showNormalAnimation.target = true; }
                    }
                    if(QUI.GhostButton("R", uiButton.normalLoop.rotate.enabled ? QColors.Color.Orange : QColors.Color.Gray, BarHeight, BarHeight, showNormalAnimation.target))
                    {
                        Undo.RecordObject(target, "ToggleRotate" + loopType);
                        uiButton.normalLoop.rotate.enabled = !uiButton.normalLoop.rotate.enabled;
                        if(uiButton.normalLoop.rotate.enabled) { showNormalAnimation.target = true; }
                    }
                    if(QUI.GhostButton("S", uiButton.normalLoop.scale.enabled ? QColors.Color.Red : QColors.Color.Gray, BarHeight, BarHeight, showNormalAnimation.target))
                    {
                        Undo.RecordObject(target, "ToggleScale" + loopType);
                        uiButton.normalLoop.scale.enabled = !uiButton.normalLoop.scale.enabled;
                        if(uiButton.normalLoop.scale.enabled) { showNormalAnimation.target = true; }
                    }
                    if(QUI.GhostButton("F", uiButton.normalLoop.fade.enabled ? QColors.Color.Purple : QColors.Color.Gray, BarHeight, BarHeight, showNormalAnimation.target))
                    {
                        Undo.RecordObject(target, "ToggleFade" + loopType);
                        uiButton.normalLoop.fade.enabled = !uiButton.normalLoop.fade.enabled;
                        if(uiButton.normalLoop.fade.enabled) { showNormalAnimation.target = true; }
                    }
                    break;
                case ButtonLoopType.Selected:
                    if(QUI.GhostButton("M", uiButton.selectedLoop.move.enabled ? QColors.Color.Green : QColors.Color.Gray, BarHeight, BarHeight, showSelectedAnimation.target))
                    {
                        Undo.RecordObject(target, "ToggleMove" + loopType);
                        uiButton.selectedLoop.move.enabled = !uiButton.selectedLoop.move.enabled;
                        if(uiButton.selectedLoop.move.enabled) { showSelectedAnimation.target = true; }
                    }
                    if(QUI.GhostButton("R", uiButton.selectedLoop.rotate.enabled ? QColors.Color.Orange : QColors.Color.Gray, BarHeight, BarHeight, showSelectedAnimation.target))
                    {
                        Undo.RecordObject(target, "ToggleRotate" + loopType);
                        uiButton.selectedLoop.rotate.enabled = !uiButton.selectedLoop.rotate.enabled;
                        if(uiButton.selectedLoop.rotate.enabled) { showSelectedAnimation.target = true; }
                    }
                    if(QUI.GhostButton("S", uiButton.selectedLoop.scale.enabled ? QColors.Color.Red : QColors.Color.Gray, BarHeight, BarHeight, showSelectedAnimation.target))
                    {
                        Undo.RecordObject(target, "ToggleScale" + loopType);
                        uiButton.selectedLoop.scale.enabled = !uiButton.selectedLoop.scale.enabled;
                        if(uiButton.selectedLoop.scale.enabled) { showSelectedAnimation.target = true; }
                    }
                    if(QUI.GhostButton("F", uiButton.selectedLoop.fade.enabled ? QColors.Color.Purple : QColors.Color.Gray, BarHeight, BarHeight, showSelectedAnimation.target))
                    {
                        Undo.RecordObject(target, "ToggleFade" + loopType);
                        uiButton.selectedLoop.fade.enabled = !uiButton.selectedLoop.fade.enabled;
                        if(uiButton.selectedLoop.fade.enabled) { showSelectedAnimation.target = true; }
                    }
                    break;
            }
        }

        void DrawLoopAnimations(ButtonLoopType loopType, bool enabled, SerializedProperty loadPresetAtRuntime, AnimBool showAnimation, Index presetCategoryIndex, SerializedProperty presetCategory, Index presetNameIndex, SerializedProperty presetName, AnimBool newPreset, float width)
        {
            QUI.BeginHorizontal(width);
            {
                DrawLoopBar(loopType + " Loop Animations", enabled, loadPresetAtRuntime, showAnimation, newPreset, width, BarHeight);
                DrawLoopButtons(loopType, BarHeight, BarHeight);
            }
            QUI.EndHorizontal();

            DUIUtils.DrawLoadPresetInfoMessage(loopType + "LoopLoadPresetAtRuntime", infoMessage, loadPresetAtRuntime.boolValue, presetCategory.stringValue, presetName.stringValue, showAnimation, loopType.ToString(), width);

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * showAnimation.faded);
                if(QUI.BeginFadeGroup(showAnimation.faded))
                {
                    QUI.BeginVertical(width - SPACE_8);
                    {
                        QUI.Space(SPACE_2 * showAnimation.faded);
                        DrawLoopAnimationsPreset(loopType, loadPresetAtRuntime, presetCategoryIndex, presetCategory, presetNameIndex, presetName, newPreset, width - SPACE_8);
                        QUI.Space(SPACE_2 * showAnimation.faded);
                        DUIUtils.DrawLoop(loopType == ButtonLoopType.Normal ? uiButton.normalLoop : uiButton.selectedLoop, target, width - SPACE_8); //draw loop animations - generic method
                        QUI.Space(SPACE_16 * showAnimation.faded);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();
            QUI.Space(SPACE_8 * (1 - showAnimation.faded));
        }
        void DrawLoopAnimationsPreset(ButtonLoopType loopType, SerializedProperty loadPresetAtRuntime, Index presetCategoryIndex, SerializedProperty presetCategory, Index presetNameIndex, SerializedProperty presetName, AnimBool newPreset, float width)
        {
            QUI.Space(SPACE_2);
            DUIUtils.DrawLoadPresetAtRuntime(loadPresetAtRuntime, width);
            DUIUtils.DrawPresetBackground(newPreset, createNewCategoryName, width);

            if(newPreset.faded < 0.5f) //NORMAL VIEW
            {
                DrawLoopPresetNormalView(presetCategoryIndex, presetCategory, presetNameIndex, presetName, newPreset, width);
                QUI.Space(SPACE_2);
                DrawLoopPresetNormalViewPresetButtons(loopType, presetCategory, presetName, newPreset, width);

            }
            else //NEW PRESET VIEW
            {
                DrawLoopPresetNewPresetView(presetCategoryIndex, presetCategory, newPreset, width);
                QUI.Space(SPACE_2);
                DrawLoopPresetNewPresetViewPresetButtons(loopType, presetCategoryIndex, presetCategory, presetCategoryIndex, presetName, newPreset, width);
            }
        }
        void DrawLoopPresetNormalView(Index presetCategoryIndex, SerializedProperty presetCategory, Index presetNameIndex, SerializedProperty presetName, AnimBool newPreset, float width)
        {
            tempFloat = (width - 6) / 2 - 5; //dropdown lists width
            QUI.BeginHorizontal(width);
            {
                //SELECT PRESET CATEGORY
                if(!DatabaseLoopAnimations.ContainsCategoryName(presetCategory.stringValue)) //if the preset category does not exist -> set it to default
                {
                    presetCategory.stringValue = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                }
                presetCategoryIndex.index = DatabaseLoopAnimations.CategoryNameIndex(presetCategory.stringValue); //update the category index
                QUI.BeginChangeCheck();
                presetCategoryIndex.index = EditorGUILayout.Popup(presetCategoryIndex.index, DatabaseLoopAnimations.categoryNames.ToArray(), GUILayout.Width(tempFloat));
                if(QUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "ChangeCategory");
                    presetCategory.stringValue = DatabaseLoopAnimations.categoryNames[presetCategoryIndex.index]; //set category naame
                    presetNameIndex.index = 0; //set name index to 0
                    presetName.stringValue = DatabaseLoopAnimations.GetCategory(presetCategory.stringValue).presetNames[presetNameIndex.index]; //set preset name according to the new index
                }

                QUI.Space(SPACE_4);

                //SELECT PRESET NAME
                if(!DatabaseLoopAnimations.Contains(presetCategory.stringValue, presetName.stringValue)) //if the preset name does not exist in the set category -> set it to index 0 (first item in the category)
                {
                    presetNameIndex.index = 0; //update the index
                    presetName.stringValue = DatabaseLoopAnimations.GetCategory(presetCategory.stringValue).presetNames[presetNameIndex.index]; //update the preset name value
                }
                else
                {
                    presetNameIndex.index = DatabaseLoopAnimations.ItemNameIndex(presetCategory.stringValue, presetName.stringValue); //update the item index
                }
                QUI.BeginChangeCheck();
                presetNameIndex.index = EditorGUILayout.Popup(presetNameIndex.index, DatabaseLoopAnimations.GetCategory(presetCategory.stringValue).presetNames.ToArray(), GUILayout.Width(tempFloat * (1 - newPreset.faded)));
                if(QUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "ChangePreset");
                    presetName.stringValue = DatabaseLoopAnimations.GetCategory(presetCategory.stringValue).presetNames[presetNameIndex.index]; //set preset name according to the new index
                }

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawLoopPresetNormalViewPresetButtons(ButtonLoopType loopType, SerializedProperty presetCategory, SerializedProperty presetName, AnimBool newPreset, float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), width, 22);

            QUI.Space(-20);

            tempFloat = (width - SPACE_16) / 3; //button width
            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_4);
                if(QUI.GhostButton("Load Preset", QColors.Color.Blue, tempFloat * (1 - newPreset.faded), MiniBarHeight))
                {
                    LoadLoopPreset(loopType, presetCategory, presetName);
                }
                QUI.Space(SPACE_4);
                if(newPreset.faded > 0)
                {
                    QUI.FlexibleSpace();
                }
                if(QUI.GhostButton("New Preset", QColors.Color.Green, tempFloat * (1 - newPreset.faded), MiniBarHeight))
                {
                    NewLoopPreset(presetCategory, newPreset);
                }
                if(newPreset.faded > 0)
                {
                    QUI.FlexibleSpace();
                }
                QUI.Space(SPACE_4);
                if(QUI.GhostButton("Delete Preset", QColors.Color.Red, tempFloat * (1 - newPreset.faded), MiniBarHeight))
                {
                    DeleteLoopPreset(presetCategory, presetName);
                }
                QUI.Space(SPACE_4);
            }
            QUI.EndHorizontal();
        }
        void DrawLoopPresetNewPresetView(Index presetCategoryIndex, SerializedProperty presetCategory, AnimBool newPreset, float width)
        {
            tempFloat = (width - 6) / 2 - 5; //dropdown lists width
            QUI.BeginHorizontal(width);
            {
                if(createNewCategoryName.faded < 0.5f)
                {
                    //SELECT PRESET CATEGORY
                    QUI.BeginChangeCheck();
                    presetCategoryIndex.index = EditorGUILayout.Popup(presetCategoryIndex.index, DatabaseLoopAnimations.categoryNames.ToArray(), GUILayout.Width(tempFloat * (1 - createNewCategoryName.faded)));
                    if(QUI.EndChangeCheck())
                    {
                        Undo.RecordObject(uiButton, "ChangeCategory");
                        presetCategory.stringValue = DatabaseLoopAnimations.categoryNames[presetCategoryIndex.index]; //set category naame
                    }

                    QUI.Space(tempFloat * createNewCategoryName.faded);
                }
                else
                {
                    //CREATE NEW CATEGORY
                    QUI.SetNextControlName("newPresetCategoryName");
                    newPresetCategoryName.name = EditorGUILayout.TextField(newPresetCategoryName.name, GUILayout.Width(tempFloat * createNewCategoryName.faded));

                    if(createNewCategoryName.isAnimating && createNewCategoryName.target && !QUI.GetNameOfFocusedControl().Equals("newPresetCategoryName")) //select the new category name text field
                    {
                        QUI.FocusTextInControl("newPresetCategoryName");
                    }

                    QUI.Space(tempFloat * (1 - createNewCategoryName.faded));
                }

                QUI.Space(SPACE_4);

                //ENTER A NEW PRESET NAME
                QUI.SetNextControlName("newPresetName");
                newPresetName.name = EditorGUILayout.TextField(newPresetName.name, GUILayout.Width(tempFloat * newPreset.faded));

                if((newPreset.isAnimating && newPreset.target && !QUI.GetNameOfFocusedControl().Equals("newPresetName"))
                   || (createNewCategoryName.isAnimating && !createNewCategoryName.target && !QUI.GetNameOfFocusedControl().Equals("newPresetName"))) //select the new preset name text field
                {
                    QUI.FocusTextInControl("newPresetName");
                }

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawLoopPresetNewPresetViewPresetButtons(ButtonLoopType loopType, Index presetCategoryIndex, SerializedProperty presetCategoryName, Index presetNameIndex, SerializedProperty presetName, AnimBool newPreset, float width)
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
                    switch(loopType)
                    {
                        case ButtonLoopType.Normal: tempBool = SaveLoopPreset(loopType, normalLoopPresetCategory, normalLoopPresetName); break;
                        case ButtonLoopType.Selected: tempBool = SaveLoopPreset(loopType, selectedLoopPresetCategory, selectedLoopPresetName); break;
                    }

                    if(tempBool) //save the new preset -> if a new preset was saved -> update the indexes
                    {
                        presetCategoryIndex.index = DatabaseLoopAnimations.CategoryNameIndex(presetCategoryName.stringValue); //update the index
                        presetNameIndex.index = DatabaseLoopAnimations.ItemNameIndex(presetCategoryName.stringValue, presetName.stringValue); //update the index
                    }
                }

                QUI.Space(SPACE_4);

                if(QUI.GhostButton("Cancel", QColors.Color.Red, tempFloat * newPreset.faded, MiniBarHeight)
                     || QUI.DetectKeyUp(Event.current, KeyCode.Escape))
                {
                    ResetNewPreset();
                }

                QUI.Space(SPACE_4);

                QUI.Space(tempFloat * (1 - newPreset.faded));
            }
            QUI.EndHorizontal();

            QUI.Space(-SPACE_4);
        }

        void LoadLoopPreset(ButtonLoopType loopType, SerializedProperty presetCategory, SerializedProperty presetName)
        {
            Loop loop = UIAnimatorUtil.GetLoop(presetCategory.stringValue, presetName.stringValue);
            if(serializedObject.isEditingMultipleObjects)
            {
                Undo.RecordObjects(targets, "LoadPreset");
                switch(loopType)
                {
                    case ButtonLoopType.Normal: for(int i = 0; i < targets.Length; i++) { UIButton iTarget = (UIButton)targets[i]; iTarget.normalLoop = loop.Copy(); } break;
                    case ButtonLoopType.Selected: for(int i = 0; i < targets.Length; i++) { UIButton iTarget = (UIButton)targets[i]; iTarget.selectedLoop = loop.Copy(); } break;
                }
            }
            else
            {
                Undo.RecordObject(target, "LoadPreset");
                switch(loopType)
                {
                    case ButtonLoopType.Normal: uiButton.normalLoop = loop.Copy(); break;
                    case ButtonLoopType.Selected: uiButton.selectedLoop = loop.Copy(); break;
                }
            }
            QUI.ExitGUI();
        }
        void NewLoopPreset(SerializedProperty presetCategory, AnimBool newPreset)
        {
            ResetNewPreset();
            newPreset.target = true;
            newPresetCategoryName.name = presetCategory.stringValue;
        }
        void DeleteLoopPreset(SerializedProperty presetCategory, SerializedProperty presetName)
        {

            if(QUI.DisplayDialog("Delete Preset",
                                            "Are you sure you want to delete the '" + presetName.stringValue + "' preset from the '" + presetCategory.stringValue + "' preset category?",
                                            "Yes",
                                            "No"))
            {
                DatabaseLoopAnimations.GetCategory(presetCategory.stringValue).DeleteLoopData(presetName.stringValue, UIAnimatorUtil.RELATIVE_PATH_LOOP_DATA);
                if(DatabaseLoopAnimations.GetCategory(presetCategory.stringValue).IsEmpty()) //category is empty -> remove it from the database (sanity check)
                {
                    DatabaseLoopAnimations.RemoveCategory(presetCategory.stringValue, UIAnimatorUtil.RELATIVE_PATH_LOOP_DATA, true);
                }
                if(serializedObject.isEditingMultipleObjects)
                {
                    for(int i = 0; i < targets.Length; i++)
                    {
                        UIElement iTarget = (UIElement)targets[i];
                        if(iTarget.loopAnimationsPresetCategoryName.Equals(presetCategory.stringValue) ||
                            iTarget.loopAnimationsPresetName.Equals(presetName.stringValue))
                        {
                            iTarget.loopAnimationsPresetCategoryName = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                            iTarget.loopAnimationsPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                        }
                    }
                }
                presetCategory.stringValue = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                presetName.stringValue = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                serializedObject.ApplyModifiedProperties();
            }
        }
        bool SaveLoopPreset(ButtonLoopType loopType, SerializedProperty presetCategory, SerializedProperty presetName)
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

            if(DatabaseLoopAnimations.Contains(newPresetCategoryName.name, newPresetName.name)) //test if the database contains the new preset name
            {
                QUI.DisplayDialog("Info",
                                  "There is another preset with the '" + newPresetName + "' preset name in the '" + newPresetCategoryName.name + "' preset category." +
                                    "\n\n" +
                                    "Try a different preset name maybe?",
                                  "Ok");
                return false; //return false that a new preset was not created
            }

            if(!DatabaseLoopAnimations.ContainsCategory(newPresetCategoryName.name)) //test if the database contains the new category
            {
                DatabaseLoopAnimations.AddCategory(newPresetCategoryName.name, UIAnimatorUtil.RELATIVE_PATH_LOOP_DATA, true); //create the new category 
            }

            switch(loopType)
            {
                case ButtonLoopType.Normal: DatabaseLoopAnimations.GetCategory(newPresetCategoryName.name).AddLoopData(UIAnimatorUtil.CreateLoopPreset(newPresetCategoryName.name, newPresetName.name, uiButton.normalLoop.Copy())); break;
                case ButtonLoopType.Selected: DatabaseLoopAnimations.GetCategory(newPresetCategoryName.name).AddLoopData(UIAnimatorUtil.CreateLoopPreset(newPresetCategoryName.name, newPresetName.name, uiButton.selectedLoop.Copy())); break;
            }

            if(serializedObject.isEditingMultipleObjects)
            {
                for(int i = 0; i < targets.Length; i++)
                {
                    UIButton iTarget = (UIButton)targets[i];
                    switch(loopType)
                    {
                        case ButtonLoopType.Normal:
                            iTarget.normalLoopPresetCategory = newPresetCategoryName.name;
                            iTarget.normalLoopPresetName = newPresetName.name;
                            break;
                        case ButtonLoopType.Selected:
                            iTarget.selectedLoopPresetCategory = newPresetCategoryName.name;
                            iTarget.selectedLoopPresetName = newPresetName.name;
                            break;
                    }
                }
            }
            else
            {
                presetCategory.stringValue = newPresetCategoryName.name;
                presetName.stringValue = newPresetName.name;
            }

            serializedObject.ApplyModifiedProperties();
            ResetNewPreset();
            return true; //return true that a new preset has been created
        }

        void DrawPreset(SerializedProperty animationType,
                        ButtonAnimType buttonAnimType,
                        SerializedProperty loadPunchPresetAtRuntime, AnimBool punchNewPreset, Index punchPresetCategoryNameIndex, SerializedProperty punchPresetCategory, Index punchPresetNameIndex, SerializedProperty punchPresetName,
                        SerializedProperty loadStatePresetAtRuntime, AnimBool stateNewPreset, Index statePresetCategoryNameIndex, SerializedProperty statePresetCategory, Index statePresetNameIndex, SerializedProperty statePresetName,
                        float width)
        {
            QUI.Space(SPACE_2);
            QUI.QObjectPropertyField("Animation Type", animationType, width, 20, false);
            QUI.Space(SPACE_2);
            switch((UIButton.ButtonAnimationType)animationType.enumValueIndex)
            {
                case UIButton.ButtonAnimationType.Punch:
                    if(loadStatePresetAtRuntime.boolValue) { loadStatePresetAtRuntime.boolValue = false; }
                    DUIUtils.DrawLoadPresetAtRuntime(loadPunchPresetAtRuntime, width, "punch");
                    DUIUtils.DrawPresetBackground(punchNewPreset, createNewCategoryName, width);
                    if(punchNewPreset.faded < 0.5f) //PUNCH - NORMAL VIEW
                    {
                        DUIUtils.DrawPunchPresetNormalView(this, punchPresetCategoryNameIndex, punchPresetCategory, punchPresetNameIndex, punchPresetName, punchNewPreset, width);
                        QUI.Space(SPACE_2);
                        DrawPunchPresetNormalViewPresetButtons(buttonAnimType, punchPresetCategory, punchPresetName, punchNewPreset, width);
                    }
                    else //PUNCH - NEW PRESET VIEW
                    {
                        DUIUtils.DrawPunchPresetNewPresetView(this, createNewCategoryName, newPresetCategoryName, punchPresetCategoryNameIndex, punchPresetCategory, punchNewPreset, newPresetName, width);
                        QUI.Space(SPACE_2);
                        DrawPunchPresetNewPresetViewPresetButtons(buttonAnimType, punchPresetCategoryNameIndex, punchPresetCategory, punchPresetNameIndex, punchPresetName, punchNewPreset, width);
                    }
                    break;
                case UIButton.ButtonAnimationType.State:
                    if(loadPunchPresetAtRuntime.boolValue) { loadPunchPresetAtRuntime.boolValue = false; }
                    DUIUtils.DrawLoadPresetAtRuntime(loadStatePresetAtRuntime, width, "state");
                    DUIUtils.DrawPresetBackground(stateNewPreset, createNewCategoryName, width);
                    if(stateNewPreset.faded < 0.5f) //STATE - NORMAL VIEW
                    {
                        DUIUtils.DrawStatePresetNormalView(this, statePresetCategoryNameIndex, statePresetCategory, statePresetNameIndex, statePresetName, stateNewPreset, width);
                        QUI.Space(SPACE_2);
                        DrawStatePresetNormalViewPresetButtons(buttonAnimType, statePresetCategory, statePresetName, stateNewPreset, width);
                    }
                    else //STATE - NEW PRESET VIEW
                    {
                        DUIUtils.DrawStatePresetNewPresetView(this, createNewCategoryName, newPresetCategoryName, statePresetCategoryNameIndex, statePresetCategory, stateNewPreset, newPresetName, width);
                        QUI.Space(SPACE_2);
                        DrawStatePresetNewPresetViewPresetButtons(buttonAnimType, statePresetCategoryNameIndex, statePresetCategory, statePresetNameIndex, statePresetName, stateNewPreset, width);
                    }
                    break;
            }
        }
    }
}
