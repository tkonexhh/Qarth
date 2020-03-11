// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEngine.Core;
using System;
using UnityEngine;

namespace DoozyUI
{
    [Serializable]
    public class DUISettings : ScriptableObject
    {
        private static DUISettings _instance;
        public static DUISettings Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = Q.GetResource<DUISettings>(DUI.RESOURCES_PATH_SETTINGS, DUI.SETTINGS_FILENAME);

#if UNITY_EDITOR
                    if(_instance == null)
                    {
                        _instance = Q.CreateAsset<DUISettings>(DUI.RELATIVE_PATH_SETTINGS, DUI.SETTINGS_FILENAME);
                    }
#endif
                }
                return _instance;
            }
        }

        //Internal Settings
        public bool InternalSettings_ExecutedUpgrade = false;

        //Hierarchy Manager
        public bool HierarchyManager_Enabled = false;
        public bool HierarchyManager_PlaymakerEventDispatcher_ShowIcon = false;
        public bool HierarchyManager_UITrigger_ShowIcon = false;
        public bool HierarchyManager_UIManager_ShowIcon = false;
        public bool HierarchyManager_Soundy_ShowIcon = false;
        public bool HierarchyManager_UINotificationManager_ShowIcon = false;
        public bool HierarchyManager_OrientationManager_ShowIcon = false;
        public bool HierarchyManager_SceneLoader_ShowIcon = false;

        public bool HierarchyManager_UICanvas_Enabled { get { return HierarchyManager_UICanvas_ShowIcon || HierarchyManager_UICanvas_ShowCanvasName || HierarchyManager_UICanvas_ShowSortingLayerNameAndOrder; } }
        public bool HierarchyManager_UICanvas_ShowIcon = true;
        public bool HierarchyManager_UICanvas_ShowCanvasName = true;
        public bool HierarchyManager_UICanvas_ShowSortingLayerNameAndOrder = false;

        public bool HierarchyManager_UINotification_ShowIcon = false;

        public bool HierarchyManager_UIElement_Enabled { get { return HierarchyManager_UIElement_ShowIcon || HierarchyManager_UIElement_ShowElementCategory || HierarchyManager_UIElement_ShowElementName || HierarchyManager_UIElement_ShowSortingLayerNameAndOrder; } }
        public bool HierarchyManager_UIElement_ShowIcon = true;
        public bool HierarchyManager_UIElement_ShowElementCategory = true;
        public bool HierarchyManager_UIElement_ShowElementName = true;
        public bool HierarchyManager_UIElement_ShowSortingLayerNameAndOrder = false;

        public bool HierarchyManager_UIButton_Enabled { get { return HierarchyManager_UIButton_ShowIcon || HierarchyManager_UIButton_ShowButtonCategory || HierarchyManager_UIButton_ShowButtonName; } }
        public bool HierarchyManager_UIButton_ShowIcon = true;
        public bool HierarchyManager_UIButton_ShowButtonCategory = true;
        public bool HierarchyManager_UIButton_ShowButtonName = true;

        public bool HierarchyManager_UIEffect_Enabled { get { return HierarchyManager_UIEffect_ShowIcon || HierarchyManager_UIEffect_ShowSortingLayerNameAndOrder; } }
        public bool HierarchyManager_UIEffect_ShowIcon = true;
        public bool HierarchyManager_UIEffect_ShowSortingLayerNameAndOrder = false;


        //UIElement Settings
        public bool UIElement_Inspector_ShowButtonRenameGameObject = true;
        public string UIElement_Inspector_RenameGameObjectPrefix = "UIE - ";
        public string UIElement_Inspector_RenameGameObjectSuffix = "";

        public bool UIElement_LANDSCAPE = true;
        public bool UIElement_PORTRAIT = true;

        public bool UIElement_startHidden = false;
        public bool UIElement_animateAtStart = false;
        public bool UIElement_disableWhenHidden = false;

        public bool UIElement_useCustomStartAnchoredPosition = false;
        public Vector3 UIElement_customStartAnchoredPosition = Vector3.zero;

        public bool UIElement_executeLayoutFix = false;

        public string UIElement_inAnimationsPresetCategoryName = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIElement_inAnimationsPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIElement_loadInAnimationsPresetAtRuntime = false;

        public string UIElement_outAnimationsPresetCategoryName = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIElement_outAnimationsPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIElement_loadOutAnimationsPresetAtRuntime = false;

        public bool UIElement_Inspector_HideLoopAnimations = false;
        public string UIElement_loopAnimationsPresetCategoryName = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIElement_loopAnimationsPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIElement_loadLoopAnimationsPresetAtRuntime = false;


        //UIButton Settings
        public bool UIButton_Inspector_ShowButtonRenameGameObject = true;
        public string UIButton_Inspector_RenameGameObjectPrefix = "UIB - ";
        public string UIButton_Inspector_RenameGameObjectSuffix = "";

        public bool UIButton_allowMultipleClicks = true;
        public float UIButton_disableButtonInterval = UIButton.BETWEEN_CLICKS_DISABLE_INTERVAL;
        public bool UIButton_deselectButtonOnClick = true;

        public bool UIButton_Inspector_HideOnPointerEnter = false;
        public bool UIButton_useOnPointerEnter = false;
        public float UIButton_onPointerEnterDisableInterval = UIButton.ON_POINTER_ENTER_DISABLE_INTERVAL;
        public string UIButton_onPointerEnterSound = DUI.DEFAULT_SOUND_NAME;
        public bool UIButton_customOnPointerEnterSound = false;
        public UIButton.ButtonAnimationType UIButton_onPointerEnterAnimationType = UIButton.ButtonAnimationType.Punch;
        public string UIButton_onPointerEnterPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIButton_onPointerEnterPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIButton_loadOnPointerEnterPunchPresetAtRuntime = false;
        public string UIButton_onPointerEnterStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIButton_onPointerEnterStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIButton_loadOnPointerEnterStatePresetAtRuntime = false;

        public bool UIButton_Inspector_HideOnPointerExit = false;
        public bool UIButton_useOnPointerExit = false;
        public float UIButton_onPointerExitDisableInterval = UIButton.ON_POINTER_EXIT_DISABLE_INTERVAL;
        public string UIButton_onPointerExitSound = DUI.DEFAULT_SOUND_NAME;
        public bool UIButton_customOnPointerExitSound = false;
        public UIButton.ButtonAnimationType UIButton_onPointerExitAnimationType = UIButton.ButtonAnimationType.Punch;
        public string UIButton_onPointerExitPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIButton_onPointerExitPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIButton_loadOnPointerExitPunchPresetAtRuntime = false;
        public string UIButton_onPointerExitStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIButton_onPointerExitStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIButton_loadOnPointerExitStatePresetAtRuntime = false;

        public bool UIButton_Inspector_HideOnPointerDown = false;
        public bool UIButton_useOnPointerDown = false;
        public string UIButton_onPointerDownSound = DUI.DEFAULT_SOUND_NAME;
        public bool UIButton_customOnPointerDownSound = false;
        public UIButton.ButtonAnimationType UIButton_onPointerDownAnimationType = UIButton.ButtonAnimationType.Punch;
        public string UIButton_onPointerDownPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIButton_onPointerDownPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIButton_loadOnPointerDownPunchPresetAtRuntime = false;
        public string UIButton_onPointerDownStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIButton_onPointerDownStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIButton_loadOnPointerDownStatePresetAtRuntime = false;

        public bool UIButton_Inspector_HideOnPointerUp = false;
        public bool UIButton_useOnPointerUp = false;
        public string UIButton_onPointerUpSound = DUI.DEFAULT_SOUND_NAME;
        public bool UIButton_customOnPointerUpSound = false;
        public UIButton.ButtonAnimationType UIButton_onPointerUpAnimationType = UIButton.ButtonAnimationType.Punch;
        public string UIButton_onPointerUpPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIButton_onPointerUpPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIButton_loadOnPointerUpPunchPresetAtRuntime = false;
        public string UIButton_onPointerUpStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIButton_onPointerUpStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIButton_loadOnPointerUpStatePresetAtRuntime = false;

        public bool UIButton_Inspector_HideOnClick = false;
        public bool UIButton_useOnClickAnimations = true;
        public bool UIButton_waitForOnClickAnimation = true;
        public UIButton.SingleClickMode UIButton_singleClickMode = UIButton.SingleClickMode.Instant;
        public string UIButton_onClickSound = DUI.DEFAULT_SOUND_NAME;
        public bool UIButton_customOnClickSound = false;
        public UIButton.ButtonAnimationType UIButton_onClickAnimationType = UIButton.ButtonAnimationType.Punch;
        public string UIButton_onClickPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIButton_onClickPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIButton_loadOnClickPunchPresetAtRuntime = false;
        public string UIButton_onClickStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIButton_onClickStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIButton_loadOnClickStatePresetAtRuntime = false;

        public bool UIButton_Inspector_HideOnDoubleClick = false;
        public bool UIButton_useOnDoubleClick = false;
        public bool UIButton_waitForOnDoubleClickAnimation = true;
        public float UIButton_doubleClickRegisterInterval = UIButton.DOUBLE_CLICK_REGISTER_INTERVAL;
        public string UIButton_onDoubleClickSound = DUI.DEFAULT_SOUND_NAME;
        public bool UIButton_customOnDoubleClickSound = false;
        public UIButton.ButtonAnimationType UIButton_onDoubleClickAnimationType = UIButton.ButtonAnimationType.Punch;
        public string UIButton_onDoubleClickPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIButton_onDoubleClickPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIButton_loadOnDoubleClickPunchPresetAtRuntime = false;
        public string UIButton_onDoubleClickStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIButton_onDoubleClickStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIButton_loadOnDoubleClickStatePresetAtRuntime = false;

        public bool UIButton_Inspector_HideOnLongClick = false;
        public bool UIButton_useOnLongClick = false;
        public bool UIButton_waitForOnLongClickAnimation = true;
        public float UIButton_longClickRegisterInterval = UIButton.LONG_CLICK_REGISTER_INTERVAL;
        public string UIButton_onLongClickSound = DUI.DEFAULT_SOUND_NAME;
        public bool UIButton_customOnLongClickSound = false;
        public UIButton.ButtonAnimationType UIButton_onLongClickAnimationType = UIButton.ButtonAnimationType.Punch;
        public string UIButton_onLongClickPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIButton_onLongClickPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIButton_loadOnLongClickPunchPresetAtRuntime = false;
        public string UIButton_onLongClickStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIButton_onLongClickStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIButton_loadOnLongClickStatePresetAtRuntime = false;

        public bool UIButton_Inspector_HideNormalLoop = false;
        public string UIButton_normalLoopPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIButton_normalLoopPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIButton_loadNormalLoopPresetAtRuntime = false;

        public bool UIButton_Inspector_HideSelectedLoop = false;
        public string UIButton_selectedLoopPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIButton_selectedLoopPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIButton_loadSelectedLoopPresetAtRuntime = false;

        public bool UIButton_addToNavigationHistory = false;


        //UIToggle Settings
        public bool UIToggle_Inspector_ShowButtonRenameGameObject = true;
        public string UIToggle_Inspector_RenameGameObjectPrefix = "UIT - ";
        public string UIToggle_Inspector_RenameGameObjectSuffix = "";

        public bool UIToggle_allowMultipleClicks = true;
        public float UIToggle_disableButtonInterval = UIToggle.BETWEEN_CLICKS_DISABLE_INTERVAL;
        public bool UIToggle_deselectButtonOnClick = true;

        public bool UIToggle_Inspector_HideOnPointerEnter = false;
        public bool UIToggle_useOnPointerEnter = false;
        public float UIToggle_onPointerEnterDisableInterval = UIToggle.ON_POINTER_ENTER_DISABLE_INTERVAL;
        public string UIToggle_onPointerEnterSoundToggleOn = DUI.DEFAULT_SOUND_NAME;
        public string UIToggle_onPointerEnterSoundToggleOff = DUI.DEFAULT_SOUND_NAME;
        public bool UIToggle_customOnPointerEnterSoundToggleOn = false;
        public bool UIToggle_customOnPointerEnterSoundToggleOff = false;
        public string UIToggle_onPointerEnterPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIToggle_onPointerEnterPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIToggle_loadOnPointerEnterPunchPresetAtRuntime = false;

        public bool UIToggle_Inspector_HideOnPointerExit = false;
        public bool UIToggle_useOnPointerExit = false;
        public float UIToggle_onPointerExitDisableInterval = UIToggle.ON_POINTER_EXIT_DISABLE_INTERVAL;
        public string UIToggle_onPointerExitSoundToggleOn = DUI.DEFAULT_SOUND_NAME;
        public string UIToggle_onPointerExitSoundToggleOff = DUI.DEFAULT_SOUND_NAME;
        public bool UIToggle_customOnPointerExitSoundToggleOn = false;
        public bool UIToggle_customOnPointerExitSoundToggleOff = false;
        public string UIToggle_onPointerExitPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIToggle_onPointerExitPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIToggle_loadOnPointerExitPunchPresetAtRuntime = false;

        public bool UIToggle_Inspector_HideOnClick = false;
        public bool UIToggle_useOnClickAnimations = true;
        public bool UIToggle_waitForOnClickAnimation = true;
        public string UIToggle_onClickSoundToggleOn = DUI.DEFAULT_SOUND_NAME;
        public string UIToggle_onClickSoundToggleOff = DUI.DEFAULT_SOUND_NAME;
        public bool UIToggle_customOnClickSoundToggleOn = false;
        public bool UIToggle_customOnClickSoundToggleOff = false;
        public string UIToggle_onClickPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string UIToggle_onClickPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public bool UIToggle_loadOnClickPunchPresetAtRuntime = false;

        //UIEffect Settings
        public bool UIEffect_Inspector_ShowButtonRenameGameObject = false;
        public string UIEffect_Inspector_RenameGameObjectPrefix = "UIEffect for ";
        public string UIEffect_Inspector_RenameGameObjectSuffix = "";

        public bool UIEffect_playOnAwake = false;
        public bool UIEffect_stopInstantly = false;
        public float UIEffect_startDelay = 0f;

        public bool UIEffect_useCustomSortingLayerName = false;
        public string UIEffect_customSortingLayerName = UIEffect.DEFAULT_CUSTOM_SORTING_LAYER_NAME;

        public bool UIEffect_useCustomOrderInLayer = false;
        public int UIEffect_customOrderInLayer = UIEffect.DEFAULT_CUSTOM_ORDER_IN_LAYER;

        public UIEffect.EffectPosition UIEffect_effectPosition = UIEffect.EffectPosition.InFrontOfTarget;
        public int UIEffect_sortingOrderStep = UIEffect.DEFAULT_DEFAULT_SORTING_ORDER_STEP;
    }
}
