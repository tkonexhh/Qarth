// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEngine.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DoozyUI
{
    [AddComponentMenu(DUI.COMPONENT_MENU_UIBUTTON, DUI.MENU_PRIORITY_UIBUTTON)]
    [RequireComponent(typeof(RectTransform), typeof(Button))]
    [DisallowMultipleComponent]
    public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler
    {
        #region Context Menu
#if UNITY_EDITOR
        [UnityEditor.MenuItem(DUI.GAMEOBJECT_MENU_UIBUTTON, false, DUI.MENU_PRIORITY_UIBUTTON)]
        static void CreateButton(UnityEditor.MenuCommand menuCommand)
        {
            GameObject targetParent = null;
            GameObject selectedGO = menuCommand.context as GameObject;
            if(selectedGO == null || selectedGO.GetComponent<RectTransform>() == null)
            {
                targetParent = UIManager.GetMasterCanvas().gameObject;
            }
            else
            {
                targetParent = selectedGO;
            }

            GameObject go = new GameObject("UIButton", typeof(RectTransform), typeof(Button), typeof(UIButton));
            UnityEditor.GameObjectUtility.SetParentAndAlign(go, targetParent);
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            go.GetComponent<UIButton>().Reset();
            go.GetComponent<RectTransform>().localScale = Vector3.one;
            go.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            go.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(160f, 30f);
            go.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

            GameObject background = new GameObject("Background", typeof(RectTransform), typeof(Image));
            UnityEditor.GameObjectUtility.SetParentAndAlign(background, go);
            background.GetComponent<RectTransform>().localScale = Vector3.one;
            background.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            background.GetComponent<RectTransform>().anchorMax = Vector2.one;
            background.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            background.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            background.GetComponent<Image>().sprite = DUI.UISprite;
            background.GetComponent<Image>().type = Image.Type.Sliced;
            background.GetComponent<Image>().fillCenter = true;
            background.GetComponent<Image>().color = new Color(31f / 255f, 136f / 255f, 201f / 255f);
#if dUI_TextMeshPro
            GameObject label = new GameObject("Label TMPro", typeof(RectTransform), typeof(TMPro.TextMeshProUGUI));
            UnityEditor.GameObjectUtility.SetParentAndAlign(label, go);
            label.GetComponent<RectTransform>().localScale = Vector3.one;
            label.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            label.GetComponent<RectTransform>().anchorMax = Vector2.one;
            label.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            label.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            label.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(2f / 255f, 10f / 255f, 15f / 255f);
            label.GetComponent<TMPro.TextMeshProUGUI>().fontSize = 14;
            label.GetComponent<TMPro.TextMeshProUGUI>().alignment = TMPro.TextAlignmentOptions.Center;
            label.GetComponent<TMPro.TextMeshProUGUI>().text = "UIButton";
#else
            GameObject text = new GameObject("Label", typeof(RectTransform), typeof(Text));
            UnityEditor.GameObjectUtility.SetParentAndAlign(text, go);
            text.GetComponent<RectTransform>().localScale = Vector3.one;
            text.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            text.GetComponent<RectTransform>().anchorMax = Vector2.one;
            text.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            text.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            text.GetComponent<Text>().color = new Color(2f / 255f, 10f / 255f, 15f / 255f);
            text.GetComponent<Text>().fontSize = 14;
            text.GetComponent<Text>().resizeTextForBestFit = false;
            text.GetComponent<Text>().resizeTextMinSize = 12;
            text.GetComponent<Text>().resizeTextMaxSize = 20;
            text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            text.GetComponent<Text>().alignByGeometry = true;
            text.GetComponent<Text>().supportRichText = true;
            text.GetComponent<Text>().text = "UIButton";
#endif

            go.GetComponent<Button>().targetGraphic = background.GetComponent<Image>();

            UnityEditor.Selection.activeObject = go;
        }
#endif
        #endregion

        /// <summary>
        /// All the animationa a button can perform when interacted with.
        /// </summary>
        public enum ButtonAnimationType { Punch, State }
        /// <summary>
        /// All the action types a button can perform.
        /// </summary>
        public enum ButtonActionType { OnPointerEnter, OnPointerExit, OnPointerDown, OnPointerUp, OnClick, OnDoubleClick, OnLongClick }
        /// <summary>
        /// All the click types actions a button can perform.
        /// </summary>
        public enum ButtonClickType { OnClick, OnDoubleClick, OnLongClick }

        /// <summary>
        /// Default value used to disable button after each click. Used when allow multiple clicks is set to false.
        /// </summary>
        public const float BETWEEN_CLICKS_DISABLE_INTERVAL = 0.4f;
        /// <summary>
        /// Default value used to disable the on pointer enter capture functionality after it has been triggered. Useful for certain cases.
        /// </summary>
        public const float ON_POINTER_ENTER_DISABLE_INTERVAL = 0.4f;
        /// <summary>
        /// Default value used to disable the on pointer exit capture functionality after it has been triggered. Useful for certain cases.
        /// </summary>
        public const float ON_POINTER_EXIT_DISABLE_INTERVAL = 0.4f;
        /// <summary>
        /// Default time interval used to register a double click. This is the time interval calculated between two sequencial clicks to determine if either a double click or two separate clicks occured.
        /// </summary>
        public const float DOUBLE_CLICK_REGISTER_INTERVAL = 0.2f;
        /// <summary>
        /// Default time interval used to register a long click. This is the time interval a button has to be pressed down to be considered a long click.
        /// </summary>
        public const float LONG_CLICK_REGISTER_INTERVAL = 0.5f;
        /// <summary>
        /// Special time interval added when deselecting a button. It fixses some anomalies.
        /// </summary>
        public const float DESELECT_BUTTON_DELAY = 0.1f;

        /// <summary>
        /// This is an extra id tag given to the tweener in order to locate the proper tween that manages the normal loop animations.
        /// </summary>
        public const string NORMAL_LOOP_ID = "ButtonNormalLoop";

        /// <summary>
        /// This is an extra id tag given to the tweener in order to locate the proper tween that manages the selected loop animations.
        /// </summary>
        public const string SELECTED_LOOP_ID = "ButtonSelectedLoop";

        /// <summary>
        /// Enables debug logs.
        /// </summary>
        public bool debug = false;

        /// <summary>
        /// The category this button name belongs to. The category is used only for database sorting purposes only. It does not matter when registering a button action.
        /// </summary>
        public string buttonCategory = DUI.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// The name of this button. This is the value the system looks at when this button issues an action.
        /// </summary>
        public string buttonName = DUI.DEFAULT_BUTTON_NAME;

        /// <summary>
        /// Should the button get disabled for a set interval (disableButtonInterval) between each click. By default we allow the user to press the button multiple times.
        /// </summary>
        public bool allowMultipleClicks = true;
        /// <summary>
        /// Should the button get deselected after each click. This is useful if you do not want this button to get selected after a click.
        /// </summary>
        public bool deselectButtonOnClick = true;
        /// <summary>
        /// If allowMultipleClicks is false, then this is the interval that this button will be disabled for between each click.
        /// </summary>
        public float disableButtonInterval = BETWEEN_CLICKS_DISABLE_INTERVAL;

        /// <summary>
        /// Toggles the OnPointerEnter functionality.
        /// </summary>
        public bool useOnPointerEnter = false;
        /// <summary>
        /// Time interval when the on pointer enter functionality is disabled after it has been triggered. Useful in certain cases.
        /// </summary>
        public float onPointerEnterDisableInterval = ON_POINTER_ENTER_DISABLE_INTERVAL;
        /// <summary>
        /// Determines if the on pointer enter functionality is available or not.
        /// </summary>
        private bool onPointerEnterReady = true;
        /// <summary>
        /// The sound name of the sound that gets played on pointer enter.
        /// </summary>
        public string onPointerEnterSound = DUI.DEFAULT_SOUND_NAME;
        /// <summary>
        /// Used by the custom inspector to allow you to type a sound name instead of selecting it from the UISounds Database.
        /// </summary>
        public bool customOnPointerEnterSound = false;
        /// <summary>
        /// UnityEvent invoked when on pointer enter has been captured by the system.
        /// </summary>
        public UnityEvent OnPointerEnter = new UnityEvent();
        /// <summary>
        /// Selects what type of animation this button is using OnPointerEnter.
        /// </summary>
        public ButtonAnimationType onPointerEnterAnimationType = ButtonAnimationType.Punch;
        /// <summary>
        /// Punch Animation Preset Category Name
        /// </summary>
        public string onPointerEnterPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// Punch Animation Preset Name
        /// </summary>
        public string onPointerEnterPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the Punch Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadOnPointerEnterPunchPresetAtRuntime = false;
        /// <summary>
        /// Punch Animation Settings
        /// </summary>
        public Punch onPointerEnterPunch = new Punch();
        /// <summary>
        /// State Animation Preset Category Name
        /// </summary>
        public string onPointerEnterStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// State Animation Preset Name
        /// </summary>
        public string onPointerEnterStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the State Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadOnPointerEnterStatePresetAtRuntime = false;
        /// <summary>
        /// State Animation Settings
        /// </summary>
        public Anim onPointerEnterState = new Anim(Anim.AnimationType.State);
        /// <summary>
        /// A list of game events that are sent when on pointer enter has been triggered.
        /// </summary>
        public List<string> onPointerEnterGameEvents;
        /// <summary>
        /// UINavigation settings.
        /// </summary>
        public NavigationPointerData onPointerEnterNavigation;

        /// <summary>
        /// Toggles the OnPointerExit functionality.
        /// </summary>
        public bool useOnPointerExit = false;
        /// <summary>
        /// Time interval when the on pointer exit functionality is disabled after it has been triggered. Useful in certain cases.
        /// </summary>
        public float onPointerExitDisableInterval = ON_POINTER_EXIT_DISABLE_INTERVAL;
        /// <summary>
        /// Determines if the on pointer exit functionality is available or not.
        /// </summary>
        private bool onPointerExitReady = true;
        /// <summary>
        /// The sound name of the sound that gets played on pointer exit.
        /// </summary>
        public string onPointerExitSound = DUI.DEFAULT_SOUND_NAME;
        /// <summary>
        /// Used by the custom inspector to allow you to type a sound name instead of selecting it from the UISounds Database.
        /// </summary>
        public bool customOnPointerExitSound = false;
        /// <summary>
        /// UnityEvent invoked when on pointer exit has been captured by the system.
        /// </summary>
        public UnityEvent OnPointerExit = new UnityEvent();
        /// <summary>
        /// Selects what type of animation this button is using OnPointerExit.
        /// </summary>
        public ButtonAnimationType onPointerExitAnimationType = ButtonAnimationType.Punch;
        /// <summary>
        /// Punch Animation Preset Category Name
        /// </summary>
        public string onPointerExitPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// Punch Animation Preset Name
        /// </summary>
        public string onPointerExitPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the Punch Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadOnPointerExitPunchPresetAtRuntime = false;
        /// <summary>
        /// Punch Animation Settings
        /// </summary>
        public Punch onPointerExitPunch = new Punch();
        /// <summary>
        /// State Animation Preset Category Name
        /// </summary>
        public string onPointerExitStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// State Animation Preset Name
        /// </summary>
        public string onPointerExitStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the State Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadOnPointerExitStatePresetAtRuntime = false;
        /// <summary>
        /// State Animation Settings
        /// </summary>
        public Anim onPointerExitState = new Anim(Anim.AnimationType.State);
        /// <summary>
        /// A list of game events that are sent when on pointer exit has been triggered.
        /// </summary>
        public List<string> onPointerExitGameEvents;
        /// <summary>
        /// UINavigation settings.
        /// </summary>
        public NavigationPointerData onPointerExitNavigation;

        /// <summary>
        /// Toggles the OnPointerDown functionality.
        /// </summary>
        public bool useOnPointerDown = false;
        /// <summary>
        /// The sound name of the sound that gets played on pointer down.
        /// </summary>
        public string onPointerDownSound = DUI.DEFAULT_SOUND_NAME;
        /// <summary>
        /// Used by the custom inspector to allow you to type a sound name instead of selecting it from the UISounds Database.
        /// </summary>
        public bool customOnPointerDownSound = false;
        /// <summary>
        /// UnityEvent invoked when on pointer down has been captured by the system.
        /// </summary>
        public UnityEvent OnPointerDown = new UnityEvent();
        /// <summary>
        /// Selects what type of animation this button is using OnPointerDown.
        /// </summary>
        public ButtonAnimationType onPointerDownAnimationType = ButtonAnimationType.Punch;
        /// <summary>
        /// Punch Animation Preset Category Name
        /// </summary>
        public string onPointerDownPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// Punch Animation Preset Name
        /// </summary>
        public string onPointerDownPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the Punch Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadOnPointerDownPunchPresetAtRuntime = false;
        /// <summary>
        /// Punch Animation Settings
        /// </summary>
        public Punch onPointerDownPunch = new Punch();
        /// <summary>
        /// State Animation Preset Category Name
        /// </summary>
        public string onPointerDownStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// State Animation Preset Name
        /// </summary>
        public string onPointerDownStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the State Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadOnPointerDownStatePresetAtRuntime = false;
        /// <summary>
        /// State Animation Settings
        /// </summary>
        public Anim onPointerDownState = new Anim(Anim.AnimationType.State);
        /// <summary>
        /// A list of game events that are sent when on pointer down has been triggered.
        /// </summary>
        public List<string> onPointerDownGameEvents;
        /// <summary>
        /// UINavigation settings.
        /// </summary>
        public NavigationPointerData onPointerDownNavigation;

        /// <summary>
        /// Toggles the OnPointerUp functionality.
        /// </summary>
        public bool useOnPointerUp = false;
        /// <summary>
        /// The sound name of the sound that gets played on pointer up.
        /// </summary>
        public string onPointerUpSound = DUI.DEFAULT_SOUND_NAME;
        /// <summary>
        /// Used by the custom inspector to allow you to type a sound name instead of selecting it from the UISounds Database.
        /// </summary>
        public bool customOnPointerUpSound = false;
        /// <summary>
        /// UnityEvent invoked when on pointer up has been captured by the system.
        /// </summary>
        public UnityEvent OnPointerUp = new UnityEvent();
        /// <summary>
        /// Selects what type of animation this button is using OnPointerUp.
        /// </summary>
        public ButtonAnimationType onPointerUpAnimationType = ButtonAnimationType.Punch;
        /// <summary>
        /// Punch Animation Preset Category Name
        /// </summary>
        public string onPointerUpPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// Punch Animation Preset Name
        /// </summary>
        public string onPointerUpPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the Punch Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadOnPointerUpPunchPresetAtRuntime = false;
        /// <summary>
        /// Punch Animation Settings
        /// </summary>
        public Punch onPointerUpPunch = new Punch();
        /// <summary>
        /// State Animation Preset Category Name
        /// </summary>
        public string onPointerUpStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// State Animation Preset Name
        /// </summary>
        public string onPointerUpStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the State Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadOnPointerUpStatePresetAtRuntime = false;
        /// <summary>
        /// State Animation Settings
        /// </summary>
        public Anim onPointerUpState = new Anim(Anim.AnimationType.State);
        /// <summary>
        /// A list of game events that are sent when on pointer up has been triggered.
        /// </summary>
        public List<string> onPointerUpGameEvents;
        /// <summary>
        /// UINavigation settings.
        /// </summary>
        public NavigationPointerData onPointerUpNavigation;

        /// <summary>
        /// Toggles the OnClick functionality. Not recommeded to be disabled. If you disable this functionality, do some tests to be sure that the button behaves as you want it to.
        /// </summary>
        public bool useOnClickAnimations = true;
        /// <summary>
        /// If enabled, the button action and game events are sent after the set animation has finished playing. This is useful if you want to be sure the uses see the button animation.
        /// </summary>
        public bool waitForOnClickAnimation = true;
        /// <summary>
        /// Setting for the OnClick trigger that marks if it should be registered instantly without checking if it's a double click or not.
        /// </summary>
        public enum SingleClickMode
        {
            /// <summary>
            /// The click will get registered instantly without checking if it's a double click or not. 
            /// <para>This is the normal behavior of a single click in any OS.</para>
            /// <para>Use this if you want to make sure a single click will get executed before a double click (dual actions).</para>
            /// <para>(usage example: SingleClick - selects, DoubleClick - executes an action)</para>
            /// </summary>
            Instant,
            /// <summary>
            /// The click will get registered after checking if it's a double click or not.
            /// <para>If it's a double click, the single click will not get triggered.</para>
            /// <para>Use this if you want to make sure the user does not execute a single click before a double click.</para>
            /// <para>The downside is that there is a delay when executing the single click (the delay is the double click register interval), so make sure you take that into account</para>
            /// </summary>
            Delayed
        }
        /// <summary>
        /// Determines if on click is triggered instantly or after it checks if it's a double click or not. Depending on your use case, you might need the Instant or Delayed mode. Default is set to Instant.
        /// </summary>
        public SingleClickMode singleClickMode = SingleClickMode.Instant;
        /// <summary>
        /// The sound name of the sound that gets played on click.
        /// </summary>
        public string onClickSound = DUI.DEFAULT_SOUND_NAME;
        /// <summary>
        /// Used by the custom inspector to allow you to type a sound name instead of selecting it from the UISounds Database.
        /// </summary>
        public bool customOnClickSound = false;
        /// <summary>
        /// UnityEvent invoked when on click has been captured by the system.
        /// </summary>
        public UnityEvent OnClick = new UnityEvent();
        /// <summary>
        /// Selects what type of animation this button is using OnClick.
        /// </summary>
        public ButtonAnimationType onClickAnimationType = ButtonAnimationType.Punch;
        /// <summary>
        /// Punch Animation Preset Category Name
        /// </summary>
        public string onClickPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// Punch Animation Preset Name
        /// </summary>
        public string onClickPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the Punch Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadOnClickPunchPresetAtRuntime = false;
        /// <summary>
        /// Punch Animation Settings
        /// </summary>
        public Punch onClickPunch = new Punch();
        /// <summary>
        /// State Animation Preset Category Name
        /// </summary>
        public string onClickStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// State Animation Preset Name
        /// </summary>
        public string onClickStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the State Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadOnClickStatePresetAtRuntime = false;
        /// <summary>
        /// State Animation Settings
        /// </summary>
        public Anim onClickState = new Anim(Anim.AnimationType.State);
        /// <summary>
        /// A list of game events that are sent when on click has been triggered.
        /// </summary>
        public List<string> onClickGameEvents;
        /// <summary>
        /// UINavigation settings.
        /// </summary>
        public NavigationPointerData onClickNavigation;

        /// <summary>
        /// Toggles the OnDoubleClick functionality.
        /// </summary>
        public bool useOnDoubleClick = false;
        /// <summary>
        /// If enabled, the button action and game events are sent after the set animation has finished playing. This is useful if you want to be sure the uses see the button animation.
        /// </summary>
        public bool waitForOnDoubleClickAnimation = true;
        /// <summary>
        /// Time interval used to register a double click. This is the time interval calculated between two sequential clicks to determine if either a double click, or two separate clicks occurred.
        /// </summary>
        public float doubleClickRegisterInterval = DOUBLE_CLICK_REGISTER_INTERVAL;
        /// <summary>
        /// Internal variable used to calculate the time interval between two sequencial clicks.
        /// </summary>
        private float doubleClickTimeoutCounter = 0f;
        /// <summary>
        /// Internal variabled that is marked as true after one click occured.
        /// </summary>
        private bool clickedOnce = false;
        /// <summary>
        /// The sound name of the sound that gets played on click.
        /// </summary>
        public string onDoubleClickSound = DUI.DEFAULT_SOUND_NAME;
        /// <summary>
        /// Used by the custom inspector to allow you to type a sound name instead of selecting it from the UISounds Database.
        /// </summary>
        public bool customOnDoubleClickSound = false;
        /// <summary>
        /// UnityEvent invoked when on double click has been captured by the system.
        /// </summary>
        public UnityEvent OnDoubleClick = new UnityEvent();
        /// <summary>
        /// Selects what type of animation this button is using OnDoubleClick.
        /// </summary>
        public ButtonAnimationType onDoubleClickAnimationType = ButtonAnimationType.Punch;
        /// <summary>
        /// Punch Animation Preset Category Name
        /// </summary>
        public string onDoubleClickPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// Punch Animation Preset Name
        /// </summary>
        public string onDoubleClickPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the Punch Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadOnDoubleClickPunchPresetAtRuntime = false;
        /// <summary>
        /// Punch Animation Settings
        /// </summary>
        public Punch onDoubleClickPunch = new Punch();
        /// <summary>
        /// State Animation Preset Category Name
        /// </summary>
        public string onDoubleClickStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// State Animation Preset Name
        /// </summary>
        public string onDoubleClickStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the State Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadOnDoubleClickStatePresetAtRuntime = false;
        /// <summary>
        /// State Animation Settings
        /// </summary>
        public Anim onDoubleClickState = new Anim(Anim.AnimationType.State);
        /// <summary>
        /// A list of game events that are sent when on double click has been triggered.
        /// </summary>
        public List<string> onDoubleClickGameEvents;
        /// <summary>
        /// UINavigation settings.
        /// </summary>
        public NavigationPointerData onDoubleClickNavigation;

        /// <summary>
        /// Toggles the OnLongClick functionality.
        /// </summary>
        public bool useOnLongClick = false;
        /// <summary>
        /// If enabled, the button action and game events are sent after the set animation has finished playing. This is useful if you want to be sure the uses see the button animation.
        /// </summary>
        public bool waitForOnLongClickAnimation = true;
        /// <summary>
        /// Time interval used to register a long click. This is the time interval a button has to be pressed down to be considered a long click.
        /// </summary>
        public float longClickRegisterInterval = LONG_CLICK_REGISTER_INTERVAL;
        /// <summary>
        /// Internal variable used to store a reference to the Coroutine that determines if a long click occured or not.
        /// </summary>
        private Coroutine cLongClickRegistered = null;
        /// <summary>
        /// Internal variable used to calculate how long was the button pressed.
        /// </summary>
        private float longClickTimeoutCounter = 0f;
        /// <summary>
        /// Internal variabled that is marked as true after the system determined that a long click occured.
        /// </summary>
        private bool executedLongClick = false;
        /// <summary>
        /// The sound name of the sound that gets played on click.
        /// </summary>
        public string onLongClickSound = DUI.DEFAULT_SOUND_NAME;
        /// <summary>
        /// Used by the custom inspector to allow you to type a sound name instead of selecting it from the UISounds Database.
        /// </summary>
        public bool customOnLongClickSound = false;
        /// <summary>
        /// UnityEvent invoked when on long click has been captured by the system.
        /// </summary>
        public UnityEvent OnLongClick = new UnityEvent();
        /// <summary>
        /// Selects what type of animation this button is using OnLongClick.
        /// </summary>
        public ButtonAnimationType onLongClickAnimationType = ButtonAnimationType.Punch;
        /// <summary>
        /// Punch Animation Preset Category Name
        /// </summary>
        public string onLongClickPunchPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// Punch Animation Preset Name
        /// </summary>
        public string onLongClickPunchPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the Punch Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadOnLongClickPunchPresetAtRuntime = false;
        /// <summary>
        /// Punch Animation Settings
        /// </summary>
        public Punch onLongClickPunch = new Punch();
        /// <summary>
        /// State Animation Preset Category Name
        /// </summary>
        public string onLongClickStatePresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// State Animation Preset Name
        /// </summary>
        public string onLongClickStatePresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the State Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadOnLongClickStatePresetAtRuntime = false;
        /// <summary>
        /// State Animation Settings
        /// </summary>
        public Anim onLongClickState = new Anim(Anim.AnimationType.State);
        /// <summary>
        /// A list of game events that are sent when on long click has been triggered.
        /// </summary>
        public List<string> onLongClickGameEvents;
        /// <summary>
        /// UINavigation settings.
        /// </summary>
        public NavigationPointerData onLongClickNavigation;

        /// <summary>
        /// Loop Animation Preset Category Name
        /// </summary>
        public string normalLoopPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// Loop Animation Preset Name
        /// </summary>
        public string normalLoopPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the Loop Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadNormalLoopPresetAtRuntime = false;
        /// <summary>
        /// Loop Animation Settings
        /// </summary>
        public Loop normalLoop = new Loop();
        /// <summary>
        /// Internal variable that is marked as true when normal loop animation is playing.
        /// </summary>
        private bool normalLoopIsPlaying = false;

        /// <summary>
        /// Loop Animation Preset Category Name
        /// </summary>
        public string selectedLoopPresetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// Loop Animation Preset Name
        /// </summary>
        public string selectedLoopPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        /// <summary>
        /// Should the system load, at runtime, the Loop Preset with the set Preset Category and Preset Name. This overrides any values set in the inspector.
        /// </summary>
        public bool loadSelectedLoopPresetAtRuntime = false;
        /// <summary>
        /// Loop Animation Settings
        /// </summary>
        public Loop selectedLoop = new Loop();
        /// <summary>
        /// Internal variable that is marked as true when selected loop animation is playing.
        /// </summary>
        private bool selectedLoopIsPlaying = false;

        /// <summary>
        /// Returns true if this button is selected, by checking the EventSystem.current.currentSelectedGameObject
        /// </summary>
        public bool IsSelected
        {
            get
            {
                if(EventSystem.current == null)
                {
                    new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                }
                return EventSystem.current == null ? false : EventSystem.current.currentSelectedGameObject == gameObject;
            }
        }
        /// <summary>
        /// Returns true if this button's name is 'Back'
        /// </summary>
        public bool IsBackButton { get { return buttonName.Equals(DUI.BACK_BUTTON_NAME); } }

        /// <summary>
        /// Internal variable that holds a reference to the RectTransform component.
        /// </summary>
        private RectTransform m_rectTransform;
        /// <summary>
        /// Returns the RectTransform component.
        /// </summary>
        public RectTransform RectTransform { get { if(m_rectTransform == null) { m_rectTransform = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>(); } return m_rectTransform; } }

        /// <summary>
        /// Internal variable that holds a reference to the Button component.
        /// </summary>
        private Button m_button;
        /// <summary>
        /// Returns the Button component.
        /// </summary>
        public Button Button { get { if(m_button == null) { m_button = GetComponent<Button>() ?? gameObject.AddComponent<Button>(); } return m_button; } }

        /// <summary>
        /// Returns true if the button's Button component is interactable. This also toggles this button's interactability.
        /// </summary>
        public bool Interactable { get { return Button.interactable; } set { Button.interactable = value; } }

        /// <summary>
        /// Internal variable that holds the start RectTransform.anchoredPosition3D.
        /// </summary>
        private Vector3 startPosition;
        /// <summary>
        /// Internal variable that holds the start RectTransform.localEulerAngles
        /// </summary>
        private Vector3 startRotation;
        /// <summary>
        /// Internal variable that holds the start RectTransform.localScale
        /// </summary>
        private Vector3 startScale;
        /// <summary>
        /// Internal variable that holds the start alpha. It does that by checking if a CanvasGroup component is attached (holding the alpha value) or it just rememebers 1 (as in 100% visibility)
        /// </summary>
        private float startAlpha;
        /// <summary>
        /// Internal variable that holds a reference to the coroutine that disables the button after click.
        /// </summary>
        private Coroutine cDisableButton;
        /// <summary>
        /// Internal variable used to determine if Start has ran. Used to differentiate code from Start to OnEnable.
        /// </summary>
        private bool Initialized = false;

        private bool ButtonClicksDisabled { get { return UIManager.Instance.ButtonClicksDisabled; } }

        private bool isControlledByLayoutGroup = false;

        public const KeyCode DEFAULT_ONCLICK_KEY_CODE = KeyCode.Return;
        public const KeyCode DEFAULT_ONCLICK_KEY_CODE_ALT = KeyCode.Space;
        public const string DEFAULT_ONCLICK_VIRTUAL_BUTTON_NAME = "Submit";
        public const string DEFAULT_ONCLICK_VIRTUAL_BUTTON_NAME_ALT = "Jump";

        /// <summary>
        /// The controller input mode. If set to ControllerInputMode.None, the button will react only to mouse clicks and touches.
        /// </summary>
        public ControllerInputMode controllerInputMode = ControllerInputMode.VirtualButton;
        /// <summary>
        /// The enables the check for alternate inputs for the button.
        /// <para>If the controllerInputMode is set to None, this option does nothing.</para>
        /// <para>If the controllerInputMode is set to KeyCode, this option enables the setting for an alternate button (KeyCode) to register a click.</para>
        /// <para>Is the controllerInputMode is set to VirtualButton, this option enabled the setting for an alternate virtual button (button name set in the InputManager) to register a click.</para>
        /// </summary>
        public bool enableAlternateInputs = true;
        /// <summary>
        /// The on click key code.
        /// </summary>
        public KeyCode onClickKeyCode = DEFAULT_ONCLICK_KEY_CODE;
        /// <summary>
        /// The on click key code alternate.
        /// </summary>
        public KeyCode onClickKeyCodeAlt = DEFAULT_ONCLICK_KEY_CODE_ALT;
        /// <summary>
        /// The on click virtual button name (set int the InputManager).
        /// </summary>
        public string onClickVirtualButtonName = DEFAULT_ONCLICK_VIRTUAL_BUTTON_NAME;
        /// <summary>
        /// The on click virtual button name alternate (set in the InputManager).
        /// </summary>
        public string onClickVirtualButtonNameAlt = DEFAULT_ONCLICK_VIRTUAL_BUTTON_NAME_ALT;

        private void Reset()
        {
            if(DUI.DUISettings == null) { return; }

            allowMultipleClicks = DUI.DUISettings.UIButton_allowMultipleClicks;
            disableButtonInterval = DUI.DUISettings.UIButton_disableButtonInterval;
            deselectButtonOnClick = DUI.DUISettings.UIButton_deselectButtonOnClick;

            useOnPointerEnter = DUI.DUISettings.UIButton_useOnPointerEnter;
            onPointerEnterDisableInterval = DUI.DUISettings.UIButton_onPointerEnterDisableInterval;
            onPointerEnterSound = DUI.DUISettings.UIButton_onPointerEnterSound;
            customOnPointerEnterSound = DUI.DUISettings.UIButton_customOnPointerEnterSound;
            onPointerEnterAnimationType = DUI.DUISettings.UIButton_onPointerEnterAnimationType;
            onPointerEnterPunchPresetCategory = DUI.DUISettings.UIButton_onPointerEnterPunchPresetCategory;
            onPointerEnterPunchPresetName = DUI.DUISettings.UIButton_onPointerEnterPunchPresetName;
            loadOnPointerEnterPunchPresetAtRuntime = DUI.DUISettings.UIButton_loadOnPointerEnterPunchPresetAtRuntime;
            onPointerEnterStatePresetCategory = DUI.DUISettings.UIButton_onPointerEnterStatePresetCategory;
            onPointerEnterStatePresetName = DUI.DUISettings.UIButton_onPointerEnterStatePresetName;
            loadOnPointerEnterStatePresetAtRuntime = DUI.DUISettings.UIButton_loadOnPointerEnterStatePresetAtRuntime;

            useOnPointerExit = DUI.DUISettings.UIButton_useOnPointerExit;
            onPointerExitDisableInterval = DUI.DUISettings.UIButton_onPointerExitDisableInterval;
            onPointerExitSound = DUI.DUISettings.UIButton_onPointerExitSound;
            customOnPointerExitSound = DUI.DUISettings.UIButton_customOnPointerExitSound;
            onPointerExitAnimationType = DUI.DUISettings.UIButton_onPointerExitAnimationType;
            onPointerExitPunchPresetCategory = DUI.DUISettings.UIButton_onPointerExitPunchPresetCategory;
            onPointerExitPunchPresetName = DUI.DUISettings.UIButton_onPointerExitPunchPresetName;
            loadOnPointerExitPunchPresetAtRuntime = DUI.DUISettings.UIButton_loadOnPointerExitPunchPresetAtRuntime;
            onPointerExitStatePresetCategory = DUI.DUISettings.UIButton_onPointerExitStatePresetCategory;
            onPointerExitStatePresetName = DUI.DUISettings.UIButton_onPointerExitStatePresetName;
            loadOnPointerExitStatePresetAtRuntime = DUI.DUISettings.UIButton_loadOnPointerExitStatePresetAtRuntime;

            useOnPointerDown = DUI.DUISettings.UIButton_useOnPointerDown;
            onPointerDownSound = DUI.DUISettings.UIButton_onPointerDownSound;
            customOnPointerDownSound = DUI.DUISettings.UIButton_customOnPointerDownSound;
            onPointerDownAnimationType = DUI.DUISettings.UIButton_onPointerDownAnimationType;
            onPointerDownPunchPresetCategory = DUI.DUISettings.UIButton_onPointerDownPunchPresetCategory;
            onPointerDownPunchPresetName = DUI.DUISettings.UIButton_onPointerDownPunchPresetName;
            loadOnPointerDownPunchPresetAtRuntime = DUI.DUISettings.UIButton_loadOnPointerDownPunchPresetAtRuntime;
            onPointerDownStatePresetCategory = DUI.DUISettings.UIButton_onPointerDownStatePresetCategory;
            onPointerDownStatePresetName = DUI.DUISettings.UIButton_onPointerDownStatePresetName;
            loadOnPointerDownStatePresetAtRuntime = DUI.DUISettings.UIButton_loadOnPointerDownStatePresetAtRuntime;

            useOnPointerUp = DUI.DUISettings.UIButton_useOnPointerUp;
            onPointerUpSound = DUI.DUISettings.UIButton_onPointerUpSound;
            customOnPointerUpSound = DUI.DUISettings.UIButton_customOnPointerUpSound;
            onPointerUpAnimationType = DUI.DUISettings.UIButton_onPointerUpAnimationType;
            onPointerUpPunchPresetCategory = DUI.DUISettings.UIButton_onPointerUpPunchPresetCategory;
            onPointerUpPunchPresetName = DUI.DUISettings.UIButton_onPointerUpPunchPresetName;
            loadOnPointerUpPunchPresetAtRuntime = DUI.DUISettings.UIButton_loadOnPointerUpPunchPresetAtRuntime;
            onPointerUpStatePresetCategory = DUI.DUISettings.UIButton_onPointerUpStatePresetCategory;
            onPointerUpStatePresetName = DUI.DUISettings.UIButton_onPointerUpStatePresetName;
            loadOnPointerUpStatePresetAtRuntime = DUI.DUISettings.UIButton_loadOnPointerUpStatePresetAtRuntime;

            useOnClickAnimations = DUI.DUISettings.UIButton_useOnClickAnimations;
            waitForOnClickAnimation = DUI.DUISettings.UIButton_waitForOnClickAnimation;
            singleClickMode = DUI.DUISettings.UIButton_singleClickMode;
            onClickSound = DUI.DUISettings.UIButton_onClickSound;
            onClickAnimationType = DUI.DUISettings.UIButton_onClickAnimationType;
            customOnClickSound = DUI.DUISettings.UIButton_customOnClickSound;
            onClickPunchPresetCategory = DUI.DUISettings.UIButton_onClickPunchPresetCategory;
            onClickPunchPresetName = DUI.DUISettings.UIButton_onClickPunchPresetName;
            loadOnClickPunchPresetAtRuntime = DUI.DUISettings.UIButton_loadOnClickPunchPresetAtRuntime;
            onClickStatePresetCategory = DUI.DUISettings.UIButton_onClickStatePresetCategory;
            onClickStatePresetName = DUI.DUISettings.UIButton_onClickStatePresetName;
            loadOnClickStatePresetAtRuntime = DUI.DUISettings.UIButton_loadOnClickStatePresetAtRuntime;

            useOnDoubleClick = DUI.DUISettings.UIButton_useOnDoubleClick;
            waitForOnDoubleClickAnimation = DUI.DUISettings.UIButton_waitForOnDoubleClickAnimation;
            doubleClickRegisterInterval = DUI.DUISettings.UIButton_doubleClickRegisterInterval;
            onDoubleClickSound = DUI.DUISettings.UIButton_onDoubleClickSound;
            onDoubleClickAnimationType = DUI.DUISettings.UIButton_onDoubleClickAnimationType;
            customOnDoubleClickSound = DUI.DUISettings.UIButton_customOnDoubleClickSound;
            onDoubleClickPunchPresetCategory = DUI.DUISettings.UIButton_onDoubleClickPunchPresetCategory;
            onDoubleClickPunchPresetName = DUI.DUISettings.UIButton_onDoubleClickPunchPresetName;
            loadOnDoubleClickPunchPresetAtRuntime = DUI.DUISettings.UIButton_loadOnDoubleClickPunchPresetAtRuntime;
            onDoubleClickStatePresetCategory = DUI.DUISettings.UIButton_onDoubleClickStatePresetCategory;
            onDoubleClickStatePresetName = DUI.DUISettings.UIButton_onDoubleClickStatePresetName;
            loadOnDoubleClickStatePresetAtRuntime = DUI.DUISettings.UIButton_loadOnDoubleClickStatePresetAtRuntime;

            useOnLongClick = DUI.DUISettings.UIButton_useOnLongClick;
            waitForOnLongClickAnimation = DUI.DUISettings.UIButton_waitForOnLongClickAnimation;
            longClickRegisterInterval = DUI.DUISettings.UIButton_longClickRegisterInterval;
            onLongClickSound = DUI.DUISettings.UIButton_onLongClickSound;
            onLongClickAnimationType = DUI.DUISettings.UIButton_onLongClickAnimationType;
            customOnLongClickSound = DUI.DUISettings.UIButton_customOnLongClickSound;
            onLongClickPunchPresetCategory = DUI.DUISettings.UIButton_onLongClickPunchPresetCategory;
            onLongClickPunchPresetName = DUI.DUISettings.UIButton_onLongClickPunchPresetName;
            loadOnLongClickPunchPresetAtRuntime = DUI.DUISettings.UIButton_loadOnLongClickPunchPresetAtRuntime;
            onLongClickStatePresetCategory = DUI.DUISettings.UIButton_onLongClickStatePresetCategory;
            onLongClickStatePresetName = DUI.DUISettings.UIButton_onLongClickStatePresetName;
            loadOnLongClickStatePresetAtRuntime = DUI.DUISettings.UIButton_loadOnLongClickStatePresetAtRuntime;

            normalLoopPresetCategory = DUI.DUISettings.UIButton_normalLoopPresetCategory;
            normalLoopPresetName = DUI.DUISettings.UIButton_normalLoopPresetName;
            loadNormalLoopPresetAtRuntime = DUI.DUISettings.UIButton_loadNormalLoopPresetAtRuntime;

            selectedLoopPresetCategory = DUI.DUISettings.UIButton_selectedLoopPresetCategory;
            selectedLoopPresetName = DUI.DUISettings.UIButton_selectedLoopPresetName;
            loadSelectedLoopPresetAtRuntime = DUI.DUISettings.UIButton_loadSelectedLoopPresetAtRuntime;

            onPointerEnterNavigation = new NavigationPointerData();
            onPointerExitNavigation = new NavigationPointerData();
            onPointerDownNavigation = new NavigationPointerData();
            onPointerUpNavigation = new NavigationPointerData();
            onClickNavigation = new NavigationPointerData();
            onDoubleClickNavigation = new NavigationPointerData();
            onLongClickNavigation = new NavigationPointerData();
        }

        private void Awake()
        {
            startPosition = RectTransform.anchoredPosition3D;
            startRotation = RectTransform.localEulerAngles;
            startScale = RectTransform.localScale;
            startAlpha = GetComponent<CanvasGroup>() == null ? 1 : GetComponent<CanvasGroup>().alpha;
            AddActionsListeners();

            LoadRuntimePunchPresets();
            LoadRuntimeStatePresets();
            LoadRuntimeLoopPresets();
        }

        private void Start()
        {
            StartCoroutine("IUpdateStartPosition");
            ResetRectTransform();
            if(IsSelected) { StartSelectedLoop(); } else { StartNormalLoop(); }
            Initialized = true;
        }

        private void OnEnable()
        {
            StartCoroutine("IUpdateStartPosition");
            if(!Initialized) { return; }
            ResetRectTransform();
            if(IsSelected) { StartSelectedLoop(); } else { StartNormalLoop(); }
        }

        /// <summary>
        /// Handles the update of the startPosition when the RectTransfrom is a chid of a LayoutGroup.
        /// </summary>
        IEnumerator IUpdateStartPosition()
        {
            yield return null; //wait for 1 frame
            yield return null; //wait for another frame
            //we need to wait in order for the layout group to calculate the children sizes and positions

            if(transform.parent != null) //check that this is not a root object
            {
                isControlledByLayoutGroup = transform.parent.GetComponent<LayoutGroup>() != null; //check if the parent has a LayoutGroup component attached (thus it is a LayoutController)
                if(isControlledByLayoutGroup) //confirm
                {
                    startPosition = RectTransform.anchoredPosition3D; //get the set position
                }
            }
        }

        void OnDisable()
        {
            StopSelectedLoop();
            StopNormalLoop();
            if(cDisableButton != null)
            {
                StopCoroutine(cDisableButton);
                cDisableButton = null;
                EnableButton();
            }
        }

        private void Update()
        {
            if(controllerInputMode == ControllerInputMode.None) { return; }
            if(!IsSelected) { return; }
            switch(controllerInputMode)
            {
                case ControllerInputMode.KeyCode: if(Input.GetKeyDown(onClickKeyCode) || (enableAlternateInputs && Input.GetKeyDown(onClickKeyCodeAlt))) { RegisterClick(); } break;
                case ControllerInputMode.VirtualButton: if(Input.GetButtonDown(onClickVirtualButtonName) || (enableAlternateInputs && Input.GetButtonDown(onClickVirtualButtonNameAlt))) { RegisterClick(); } break;
            }
        }

        /// <summary>
        /// Loads the animation values of all Punch Presets that are set to load at runtime.
        /// </summary>
        void LoadRuntimePunchPresets()
        {
            if(useOnPointerEnter && onPointerEnterAnimationType == ButtonAnimationType.Punch && loadOnPointerEnterPunchPresetAtRuntime)
            {
                Punch presetPunch = UIAnimatorUtil.GetPunch(onPointerEnterPunchPresetCategory, onPointerEnterPunchPresetName);
                if(presetPunch != null) { onPointerEnterPunch = presetPunch.Copy(); }
            }
            if(useOnPointerExit && onPointerExitAnimationType == ButtonAnimationType.Punch && loadOnPointerExitPunchPresetAtRuntime)
            {
                Punch presetPunch = UIAnimatorUtil.GetPunch(onPointerExitPunchPresetCategory, onPointerExitPunchPresetName);
                if(presetPunch != null) { onPointerExitPunch = presetPunch.Copy(); }
            }
            if(useOnPointerDown && onPointerDownAnimationType == ButtonAnimationType.Punch && loadOnPointerDownPunchPresetAtRuntime)
            {
                Punch presetPunch = UIAnimatorUtil.GetPunch(onPointerDownPunchPresetCategory, onPointerDownPunchPresetName);
                if(presetPunch != null) { onPointerDownPunch = presetPunch.Copy(); }
            }
            if(useOnPointerUp && onPointerUpAnimationType == ButtonAnimationType.Punch && loadOnPointerUpPunchPresetAtRuntime)
            {
                Punch presetPunch = UIAnimatorUtil.GetPunch(onPointerUpPunchPresetCategory, onPointerUpPunchPresetName);
                if(presetPunch != null) { onPointerUpPunch = presetPunch.Copy(); }
            }
            if(useOnClickAnimations && onClickAnimationType == ButtonAnimationType.Punch && loadOnClickPunchPresetAtRuntime)
            {
                Punch presetPunch = UIAnimatorUtil.GetPunch(onClickPunchPresetCategory, onClickPunchPresetName);
                if(presetPunch != null) { onClickPunch = presetPunch.Copy(); }
            }
            if(useOnDoubleClick && onDoubleClickAnimationType == ButtonAnimationType.Punch && loadOnDoubleClickPunchPresetAtRuntime)
            {
                Punch presetPunch = UIAnimatorUtil.GetPunch(onDoubleClickPunchPresetCategory, onDoubleClickPunchPresetName);
                if(presetPunch != null) { onDoubleClickPunch = presetPunch.Copy(); }
            }
            if(useOnLongClick && onLongClickAnimationType == ButtonAnimationType.Punch && loadOnLongClickPunchPresetAtRuntime)
            {
                Punch presetPunch = UIAnimatorUtil.GetPunch(onLongClickPunchPresetCategory, onLongClickPunchPresetName);
                if(presetPunch != null) { onLongClickPunch = presetPunch.Copy(); }
            }
        }
        /// <summary>
        /// Loads the animation values of all State Presets that are set to load at runtime.
        /// </summary>
        void LoadRuntimeStatePresets()
        {
            if(useOnPointerEnter && onPointerEnterAnimationType == ButtonAnimationType.State && loadOnPointerEnterStatePresetAtRuntime)
            {
                Anim presetState = UIAnimatorUtil.GetStateAnim(onPointerEnterStatePresetCategory, onPointerEnterStatePresetName);
                if(presetState != null) { onPointerEnterState = presetState.Copy(); }
            }
            if(useOnPointerExit && onPointerExitAnimationType == ButtonAnimationType.State && loadOnPointerExitStatePresetAtRuntime)
            {
                Anim presetState = UIAnimatorUtil.GetStateAnim(onPointerExitStatePresetCategory, onPointerExitStatePresetName);
                if(presetState != null) { onPointerExitState = presetState.Copy(); }
            }
            if(useOnPointerDown && onPointerDownAnimationType == ButtonAnimationType.State && loadOnPointerDownStatePresetAtRuntime)
            {
                Anim presetState = UIAnimatorUtil.GetStateAnim(onPointerDownStatePresetCategory, onPointerDownStatePresetName);
                if(presetState != null) { onPointerDownState = presetState.Copy(); }
            }
            if(useOnPointerUp && onPointerUpAnimationType == ButtonAnimationType.State && loadOnPointerUpStatePresetAtRuntime)
            {
                Anim presetState = UIAnimatorUtil.GetStateAnim(onPointerUpStatePresetCategory, onPointerUpStatePresetName);
                if(presetState != null) { onPointerUpState = presetState.Copy(); }
            }
            if(useOnClickAnimations && onClickAnimationType == ButtonAnimationType.State && loadOnClickStatePresetAtRuntime)
            {
                Anim presetState = UIAnimatorUtil.GetStateAnim(onClickStatePresetCategory, onClickStatePresetName);
                if(presetState != null) { onClickState = presetState.Copy(); }
            }
            if(useOnDoubleClick && onDoubleClickAnimationType == ButtonAnimationType.State && loadOnDoubleClickStatePresetAtRuntime)
            {
                Anim presetState = UIAnimatorUtil.GetStateAnim(onDoubleClickStatePresetCategory, onDoubleClickStatePresetName);
                if(presetState != null) { onDoubleClickState = presetState.Copy(); }
            }
            if(useOnLongClick && onLongClickAnimationType == ButtonAnimationType.State && loadOnLongClickStatePresetAtRuntime)
            {
                Anim presetState = UIAnimatorUtil.GetStateAnim(onLongClickStatePresetCategory, onLongClickStatePresetName);
                if(presetState != null) { onLongClickState = presetState.Copy(); }
            }
        }
        /// <summary>
        /// Loads the animation values of all Loop Presets that are set to load at runtime.
        /// </summary>
        void LoadRuntimeLoopPresets()
        {
            if(loadNormalLoopPresetAtRuntime)
            {
                Loop presetLoop = UIAnimatorUtil.GetLoop(normalLoopPresetCategory, normalLoopPresetName);
                if(presetLoop != null) { normalLoop = presetLoop.Copy(); }
            }
            if(loadSelectedLoopPresetAtRuntime)
            {
                Loop presetLoop = UIAnimatorUtil.GetLoop(selectedLoopPresetCategory, selectedLoopPresetName);
                if(presetLoop != null) { selectedLoop = presetLoop.Copy(); }
            }
        }

        /// <summary>
        /// Initiates all the listeners for the button's actions.
        /// </summary>
        void AddActionsListeners()
        {
            OnPointerEnter.AddListener(ExecuteOnPoinerEnterActions);
            OnPointerExit.AddListener(ExecuteOnPoinerExitActions);
            OnPointerDown.AddListener(ExecuteOnPointerDownActions);
            OnPointerUp.AddListener(ExecuteOnPointerUpActions);
            //OnClick.AddListener(ExecuteOnClickActions);
            //OnDoubleClick.AddListener(ExecuteOnDoubleClickActions);
            //OnLongClick.AddListener(ExecuteOnLongClickActions);
        }

        /// <summary>
        /// Starts playing the normal loop animations. These are the loop animations that play when the button is NOT selected.
        /// </summary>
        /// <param name="forced">Tries to play the animation even if it has not been enabled.</param>
        void StartNormalLoop(bool forced = false)
        {
            if(normalLoopIsPlaying) { return; }
            if(normalLoop == null || !normalLoop.Enabled) { return; }
            ResetRectTransform();
            UIAnimator.SetupLoops(RectTransform, startPosition, startRotation, startScale, startAlpha, normalLoop,
                                 null, null,
                                 null, null,
                                 null, null,
                                 null, null,
                                 NORMAL_LOOP_ID, true, forced);
            UIAnimator.PlayLoops(RectTransform, NORMAL_LOOP_ID);
            normalLoopIsPlaying = true;
        }
        /// <summary>
        /// Stops playing the normal loop animations and resets the RectTransform to the start values.
        /// </summary>
        void StopNormalLoop()
        {
            if(!normalLoopIsPlaying) { return; }
            UIAnimator.StopLoops(RectTransform, NORMAL_LOOP_ID);
            normalLoopIsPlaying = false;
            ResetRectTransform();
        }
        /// <summary>
        /// Starts playing the selected loop animations. These are the loop animations that play when the button is selected.
        /// </summary>
        /// <param name="forced">Tries to play the animation even if it has not been enabled.</param>
        void StartSelectedLoop(bool forced = false)
        {
            if(selectedLoopIsPlaying) { return; }
            if(selectedLoop == null || !selectedLoop.Enabled) { return; }
            ResetRectTransform();
            UIAnimator.SetupLoops(RectTransform, startPosition, startRotation, startScale, startAlpha, selectedLoop,
                                null, null,
                                null, null,
                                null, null,
                                null, null,
                                SELECTED_LOOP_ID, true, forced);
            UIAnimator.PlayLoops(RectTransform, SELECTED_LOOP_ID);
            selectedLoopIsPlaying = true;
        }
        /// <summary>
        /// Stops playing the selected loop animations and resets the RectTransform to the start values.
        /// </summary>
        void StopSelectedLoop()
        {
            if(!selectedLoopIsPlaying) { return; }
            UIAnimator.StopLoops(RectTransform, SELECTED_LOOP_ID);
            selectedLoopIsPlaying = false;
            ResetRectTransform();
        }

        /// <summary>
        /// Resets the RectTransform to the start values.
        /// </summary>
        void ResetRectTransform()
        {
            UIAnimator.ResetTarget(RectTransform, startPosition, startRotation, startScale, startAlpha);
        }

        /// <summary>
        /// Deselects the button after the set delay.
        /// </summary>
        void DeselectButton(float delay)
        {
            StartCoroutine(iDeselectButton(delay));
        }
        /// <summary>
        /// Executes the button deselection in realtime.
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        IEnumerator iDeselectButton(float delay)
        {
            yield return new WaitForSecondsRealtime(delay + DESELECT_BUTTON_DELAY);
            if(EventSystem.current.currentSelectedGameObject == gameObject) { EventSystem.current.SetSelectedGameObject(null); }
        }

        /// <summary>
        /// Executes all the actions set for OnPointerEnter.
        /// </summary>
        void ExecuteOnPoinerEnterActions()
        {
            if(ButtonClicksDisabled) { return; }
            if(!useOnPointerEnter) { return; }
            StopNormalLoop();
            StopSelectedLoop();
            PlaySound(onPointerEnterSound);
            switch(onPointerEnterAnimationType)
            {
                case ButtonAnimationType.Punch: ExecutePunch(onPointerEnterPunch, false); break;
                case ButtonAnimationType.State: ExecuteState(onPointerEnterState, false); break;
            }
            SendGameEvents(onPointerEnterGameEvents);
            UIManager.Instance.SendButtonAction(this, ButtonActionType.OnPointerEnter);
        }
        /// <summary>
        /// Executes all the actions set for OnPointerExit.
        /// </summary>
        void ExecuteOnPoinerExitActions()
        {
            if(ButtonClicksDisabled) { return; }
            if(!useOnPointerExit) { return; }
            if(IsSelected) { StartSelectedLoop(); } else { StartNormalLoop(); }
            PlaySound(onPointerExitSound);
            switch(onPointerExitAnimationType)
            {
                case ButtonAnimationType.Punch: ExecutePunch(onPointerExitPunch, false); break;
                case ButtonAnimationType.State: ExecuteState(onPointerExitState, false); break;
            }
            SendGameEvents(onPointerExitGameEvents);
            UIManager.Instance.SendButtonAction(this, ButtonActionType.OnPointerExit);
        }
        /// <summary>
        /// Executes all the actions set for OnPointerDown.
        /// </summary>
        void ExecuteOnPointerDownActions()
        {
            if(ButtonClicksDisabled) { return; }
            if(!useOnPointerDown) { return; }
            StopSelectedLoop();
            PlaySound(onPointerDownSound);
            switch(onPointerDownAnimationType)
            {
                case ButtonAnimationType.Punch: ExecutePunch(onPointerDownPunch, false); break;
                case ButtonAnimationType.State: ExecuteState(onPointerDownState, false); break;
            }
            SendGameEvents(onPointerDownGameEvents);
            UIManager.Instance.SendButtonAction(this, ButtonActionType.OnPointerDown);
        }
        /// <summary>
        /// Executes all the actions set for OnPointerUp.
        /// </summary>
        void ExecuteOnPointerUpActions()
        {
            if(ButtonClicksDisabled) { return; }
            if(!useOnPointerUp) { return; }
            PlaySound(onPointerUpSound);
            switch(onPointerUpAnimationType)
            {
                case ButtonAnimationType.Punch: ExecutePunch(onPointerUpPunch, false); break;
                case ButtonAnimationType.State: ExecuteState(onPointerUpState, false); break;
            }
            SendGameEvents(onPointerUpGameEvents);
            UIManager.Instance.SendButtonAction(this, ButtonActionType.OnPointerUp);
        }

        /// <summary>
        /// Executes all the actions set for OnClick.
        /// </summary>
        void ExecuteOnClickActions()
        {
            if(ButtonClicksDisabled) { return; }
            if(!useOnClickAnimations)
            {
                if(!useOnDoubleClick && !useOnLongClick) { return; }
            }
            PlaySound(onClickSound);
            switch(onClickAnimationType)
            {
                case ButtonAnimationType.Punch: ExecutePunch(onClickPunch, deselectButtonOnClick); break;
                case ButtonAnimationType.State: ExecuteState(onClickState, deselectButtonOnClick); break;
            }
            StartCoroutine(iExecuteClickActionsWithDelay());
        }
        /// <summary>
        /// Simulates this button's click action, without playing the set on click sound and punch animation.
        /// </summary>
        public void SendButtonClick()
        {
            UIManager.Instance.SendButtonAction(this, ButtonActionType.OnClick);
            OnClick.Invoke();
        }
        /// <summary>
        /// Simulates this button's click action and plays the set on click sound and punch animation.
        /// </summary>
        public void SendButtonClick(bool playSound, bool animate, bool sendGameEvents, bool forced = false)
        {
            if(playSound) { PlaySound(onClickSound); }
            if(animate) { ExecutePunch(onClickPunch, deselectButtonOnClick, forced); }
            if(sendGameEvents) { SendGameEvents(onClickGameEvents); }
            SendButtonClick();
        }
        IEnumerator iExecuteClickActionsWithDelay()
        {
            if(waitForOnClickAnimation) { yield return new WaitForSecondsRealtime(onClickPunch.TotalDuration); }
            SendGameEvents(onClickGameEvents);
            SendButtonClick();
        }


        /// <summary>
        /// Executes all the actions set for OnDoubleClick.
        /// </summary>
        void ExecuteOnDoubleClickActions()
        {
            if(ButtonClicksDisabled) { return; }
            if(!useOnDoubleClick) { return; }
            PlaySound(onDoubleClickSound);
            switch(onDoubleClickAnimationType)
            {
                case ButtonAnimationType.Punch: ExecutePunch(onDoubleClickPunch, deselectButtonOnClick); break;
                case ButtonAnimationType.State: ExecuteState(onDoubleClickState, deselectButtonOnClick); break;
            }
            StartCoroutine(iExecuteDoubleClickActionsWithDelay());
        }
        /// <summary>
        /// Simulates this button's double click action, without playing the set on double click sound and punch animation.
        /// </summary>
        public void SendButtonDoubleClick()
        {
            UIManager.Instance.SendButtonAction(this, ButtonActionType.OnDoubleClick);
            OnDoubleClick.Invoke();
        }
        /// <summary>
        /// Simulates this button's double click action and plays the set on double click sound and punch animation.
        /// </summary>
        public void SendButtonDoubleClick(bool playSound, bool animate, bool sendGameEvents, bool forced = false)
        {
            if(playSound) { PlaySound(onDoubleClickSound); }
            if(animate) { ExecutePunch(onDoubleClickPunch, deselectButtonOnClick, forced); }
            if(sendGameEvents) { SendGameEvents(onDoubleClickGameEvents); }
            SendButtonDoubleClick();
        }
        IEnumerator iExecuteDoubleClickActionsWithDelay()
        {
            if(waitForOnDoubleClickAnimation) { yield return new WaitForSecondsRealtime(onDoubleClickPunch.TotalDuration); }
            SendGameEvents(onDoubleClickGameEvents);
            SendButtonDoubleClick();
        }


        /// <summary>
        /// Executes all the actions set for OnLongClick.
        /// </summary>
        void ExecuteOnLongClickActions()
        {
            if(ButtonClicksDisabled) { return; }
            if(!useOnLongClick) { return; }
            PlaySound(onLongClickSound);
            switch(onLongClickAnimationType)
            {
                case ButtonAnimationType.Punch: ExecutePunch(onLongClickPunch, deselectButtonOnClick); break;
                case ButtonAnimationType.State: ExecuteState(onLongClickState, deselectButtonOnClick); break;
            }
            StartCoroutine(iExecuteLongClickActionsWithDelay());
        }
        /// <summary>
        /// Simulates this button's long click action, without playing the set on long click sound and punch animation.
        /// </summary>
        public void SendButtonLongClick()
        {
            UIManager.Instance.SendButtonAction(this, ButtonActionType.OnLongClick);
            OnLongClick.Invoke();
        }
        /// <summary>
        /// Simulates this button's long click action and plays the set on long click sound and punch animation.
        /// </summary>
        public void SendButtonLongClick(bool playSound, bool animate, bool sendGameEvents, bool forced = false)
        {
            if(playSound) { PlaySound(onLongClickSound); }
            if(animate) { ExecutePunch(onLongClickPunch, deselectButtonOnClick, forced); }
            if(sendGameEvents) { SendGameEvents(onLongClickGameEvents); }
            SendButtonLongClick();
        }
        IEnumerator iExecuteLongClickActionsWithDelay()
        {
            if(waitForOnLongClickAnimation) { yield return new WaitForSecondsRealtime(onLongClickPunch.TotalDuration); }
            SendGameEvents(onLongClickGameEvents);
            SendButtonLongClick();
        }

        /// <summary>
        /// Plays a sound name by sending it to UIManager.PlaySound(soundName).
        /// </summary>
        void PlaySound(string soundName)
        {
            UIManager.PlaySound(soundName);
        }

        /// <summary>
        /// Executes a given punch animation.
        /// </summary>
        /// <param name="punch">Punch Animation Settings</param>
        /// <param name="deselectButton">Should the button get deselected after the animation finished.</param>
        /// <param name="forced">Soudld the animation play even if it is disabled.</param>
        public void ExecutePunch(Punch punch, bool deselectButton = false, bool forced = false)
        {
            if(punch == null) { return; }
            UIAnimator.PunchMove(RectTransform, startPosition, punch, null, null, forced);
            UIAnimator.PunchRotate(RectTransform, startRotation, punch, null, null, forced);
            UIAnimator.PunchScale(RectTransform, startScale, punch, null, null, forced);
            if(deselectButton) { DeselectButton(punch.TotalDuration); }
        }

        /// <summary>
        /// Executes a given state animation.
        /// </summary>
        /// <param name="anim">State Animation Settings</param>
        /// <param name="deselectButton">Should the button get deselected after the animation finished.</param>
        /// <param name="forced">Should the animation play event if it is disabled.</param>
        public void ExecuteState(Anim anim, bool deselectButton = false, bool forced = false)
        {
            if(anim == null) { return; }
            UIAnimator.Move(RectTransform, startPosition, anim, null, null, false, forced);
            UIAnimator.Rotate(RectTransform, startRotation, anim, null, null, false, forced);
            UIAnimator.Scale(RectTransform, startScale, anim, null, null, false, forced);
            UIAnimator.Fade(RectTransform, startAlpha, anim, null, null, false, forced);
            if(deselectButton) { DeselectButton(anim.TotalDuration); }
        }

        /// <summary>
        /// Sends a list of game events to the UIManager.
        /// </summary>
        /// <param name="gameEvents"></param>
        void SendGameEvents(List<string> gameEvents) { UIManager.SendGameEvents(gameEvents); }

        #region EnableButton / DisableButton
        /// <summary>
        /// Sets Interactable to true.
        /// </summary>
        public void EnableButton() { Interactable = true; }
        /// <summary>
        /// Sets Interactable to false.
        /// </summary>
        public void DisableButton() { Interactable = false; }
        /// <summary>
        /// Sets Interactable to false for the set duration. After that it sets Interactable to true.
        /// </summary>
        public void DisableButton(float duration)
        {
            if(!Interactable) { return; }
            cDisableButton = StartCoroutine(iDisableButton(duration));
        }
        /// <summary>
        /// Executes the button disabling in realtime.
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        IEnumerator iDisableButton(float duration)
        {
            DisableButton();
            yield return new WaitForSecondsRealtime(duration);
            EnableButton();
            cDisableButton = null;
        }
        /// <summary>
        /// Disables the button for the set disableButtonInterval value.
        /// </summary>
        void DisableButtonAfterClick() { DisableButton(disableButtonInterval); }
        #endregion

        #region OnPointerEnter
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if(debug) { Debug.Log("[Doozy][UIButton] - " + name + " | OnPointerEnter triggered"); }
            ExecuteOnPointerEnter();
        }
        /// <summary>
        /// Executes the OnPointerEnter trigger. You can force an execution of this trigger (regardless if it's enabled or not) by calling this method with forced set to TRUE
        /// </summary>
        /// <param name="forced">Fires this trigger regardless if it is enabled or not (default:false)</param>
        public void ExecuteOnPointerEnter(bool forced = false)
        {
            if(ButtonClicksDisabled) { return; }
            if((Interactable && onPointerEnterReady) || forced)
            {
                if(debug) { Debug.Log("[Doozy][UIButton] - " + name + " | Executing OnPointerEnter" + (forced ? "initiated by force" : "")); }
                if(onPointerEnterDisableInterval > 0) { StartCoroutine("DisableOnPointerEnter"); }
                OnPointerEnter.Invoke();
            }
        }
        IEnumerator DisableOnPointerEnter()
        {
            onPointerEnterReady = false;
            yield return new WaitForSecondsRealtime(onPointerEnterDisableInterval);
            onPointerEnterReady = true;
        }
        #endregion
        #region OnPointerExit
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if(debug) { Debug.Log("[Doozy][UIButton] - " + name + " | OnPointerExit triggered"); }
            ExecutePointerExit();
        }
        /// <summary>
        /// Executes the OnPointerExit trigger. You can force an execution of this trigger (regardless if it's enabled or not) by calling this method with forced set to TRUE
        /// </summary>
        /// <param name="forced">Fires this trigger regardless if it is enabled or not (default:false)</param>
        public void ExecutePointerExit(bool forced = false)
        {
            if(ButtonClicksDisabled) { return; }
            if((Interactable && onPointerExitReady) || forced)
            {
                if(debug) { Debug.Log("[Doozy][UIButton] - " + name + " | Executing OnPointerExit" + (forced ? "initiated by force" : "")); }
                if(onPointerExitDisableInterval > 0) { StartCoroutine("DisableOnPointerExit"); }
                OnPointerExit.Invoke();
            }
        }
        IEnumerator DisableOnPointerExit()
        {
            onPointerExitReady = false;
            yield return new WaitForSecondsRealtime(onPointerExitDisableInterval);
            onPointerExitReady = true;
        }
        #endregion
        #region OnPointerDown
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if(debug) { Debug.Log("[Doozy][UIButton] - " + name + " | OnPointerDown triggered"); }
            ExecutePointerDown();
        }
        /// <summary>
        /// Executes the OnPointerDown trigger. You can force an execution of this trigger (regardless if it's enabled or not) by calling this method with forced set to TRUE
        /// </summary>
        /// <param name="forced">Fires this trigger regardless if it is enabled or not (default:false)</param>
        public void ExecutePointerDown(bool forced = false)
        {
            if(ButtonClicksDisabled) { return; }
            if(Interactable || forced)
            {
                if(debug) { Debug.Log("[Doozy][UIButton] - " + name + " | Executing OnPointerDown" + (forced ? "initiated by force" : "")); }
                OnPointerDown.Invoke();
            }

            if(useOnLongClick && Interactable) { RegisterLongClick(); }
        }
        #endregion
        #region OnPointerUp
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if(debug) { Debug.Log("[Doozy][UIButton] - " + name + " | OnPointerUp triggered"); }
            ExecutePointerUp();
        }
        /// <summary>
        /// Executes the OnPointerUp trigger. You can force an execution of this trigger (regardless if it's enabled or not) by calling this method with forced set to TRUE
        /// </summary>
        /// <param name="forced">Fires this trigger regardless if it is enabled or not (default:false)</param>
        public void ExecutePointerUp(bool forced = false)
        {
            if(ButtonClicksDisabled) { return; }
            if(Interactable || forced)
            {
                if(debug) { Debug.Log("[Doozy][UIButton] - " + name + " | Executing OnPointerUp" + (forced ? "initiated by force" : "")); }
                OnPointerUp.Invoke();
            }

            StopLongClickListener();
        }
        #endregion
        #region OnClick
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if(debug) { Debug.Log("[Doozy][UIButton] - " + name + " | OnPointerClick triggered"); }
            RegisterClick();
        }
        void RegisterClick()
        {
            if(executedLongClick) { ResetLongClickListener(); return; }
            StartCoroutine(ClickRegistered());
        }
        IEnumerator ClickRegistered()
        {
            if(!clickedOnce && doubleClickTimeoutCounter < doubleClickRegisterInterval)
            {
                clickedOnce = true;
            }
            else
            {
                clickedOnce = false;
                yield break; //button is pressed twice -> don't allow the second function call to fully execute
            }
            yield return new WaitForEndOfFrame();
            if(singleClickMode == SingleClickMode.Instant) { ExecuteClick(); }
            while(doubleClickTimeoutCounter < doubleClickRegisterInterval)
            {
                if(!clickedOnce)
                {
                    ExecuteDoubleClick();
                    doubleClickTimeoutCounter = 0f;
                    clickedOnce = false;
                    yield break;
                }
                doubleClickTimeoutCounter += Time.unscaledDeltaTime; //increment counter by change in time between frames
                yield return null; //wait for the next frame
            }
            if(singleClickMode == SingleClickMode.Delayed) { ExecuteClick(); }
            doubleClickTimeoutCounter = 0f;
            clickedOnce = false;
            if(!allowMultipleClicks) { DisableButtonAfterClick(); }
        }
        /// <summary>
        /// Executes the OnClick trigger. You can force an execution of this trigger (regardless if it's enabled or not) by calling this method with forced set to TRUE
        /// </summary>
        /// <param name="forced">Fires this trigger regardless if it is enabled or not (default:false)</param>
        public void ExecuteClick(bool forced = false)
        {
            if(ButtonClicksDisabled) { return; }
            if(debug) { Debug.Log("[Doozy][UIButton] - " + name + " | Executing OnClick" + (forced ? "initiated by force" : "")); }
            if(Interactable || forced)
            {
                ExecuteOnClickActions();
                //OnClick.Invoke();
            }
            if(!Interactable & onPointerExitReady)
            {
                ExecuteOnPoinerExitActions();
                //OnPointerExit.Invoke();
            }
        }
        #endregion
        #region OnDoubleClick
        /// <summary>
        /// Executes the OnDoubleClick trigger. You can force an execution of this trigger (regardless if it's enabled or not) by calling this method with forced set to TRUE
        /// </summary>
        /// <param name="forced">Fires this trigger regardless if it is enabled or not (default:false)</param>
        public void ExecuteDoubleClick(bool forced = false)
        {
            if(ButtonClicksDisabled) { return; }
            if(debug) { Debug.Log("[Doozy][UIButton] - " + name + " | Executing OnDoubleClick" + (forced ? "initiated by force" : "")); }
            if(Interactable || forced)
            {
                ExecuteOnDoubleClickActions();
                //OnDoubleClick.Invoke();
            }
            if(!Interactable & onPointerExitReady)
            {
                ExecuteOnPoinerExitActions();
                //OnPointerExit.Invoke();
            }
            if(!allowMultipleClicks) { DisableButtonAfterClick(); }
        }
        #endregion
        #region OnLongClick
        /// <summary>
        /// Executes the OnLongClick trigger. You can force an execution of this trigger (regardless if it's enabled or not) by calling this method with forced set to TRUE
        /// </summary>
        /// <param name="forced">Fires this trigger regardless if it is enabled or not (default:false)</param>
        public void ExecuteLongClick(bool forced = false)
        {
            if(ButtonClicksDisabled) { return; }
            if(debug) { Debug.Log("[Doozy][UIButton] - " + name + " | Executing OnLongClick" + (forced ? "initiated by force" : "")); }
            if(Interactable || forced)
            {
                ExecuteOnLongClickActions();
                //OnLongClick.Invoke();
            }
            if(!Interactable & onPointerExitReady) { OnPointerExit.Invoke(); }
            if(!allowMultipleClicks) { DisableButtonAfterClick(); }
        }
        void RegisterLongClick()
        {
            if(executedLongClick) { return; }
            ResetLongClickListener();
            cLongClickRegistered = StartCoroutine(LongClickRegistered());
        }
        void StopLongClickListener()
        {
            if(executedLongClick) { return; }
            ResetLongClickListener();
        }
        void ResetLongClickListener()
        {
            executedLongClick = false;
            longClickTimeoutCounter = 0f;
            if(cLongClickRegistered == null) { return; }
            StopCoroutine(cLongClickRegistered);
            cLongClickRegistered = null;
        }
        IEnumerator LongClickRegistered()
        {
            while(longClickTimeoutCounter < longClickRegisterInterval)
            {
                longClickTimeoutCounter += Time.unscaledDeltaTime; //increment counter by change in time between frames
                yield return null;
            }
            ExecuteLongClick();
            executedLongClick = true;
        }
        #endregion

        #region OnSelect / OnDeselect
        /// <summary>
        /// Used by ISelectHandler.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSelect(BaseEventData eventData)
        {
            if(eventData.selectedObject == gameObject)
            {
                StopNormalLoop();
                StartSelectedLoop();
            }
        }
        /// <summary>
        /// Used by IDeselectHandler.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDeselect(BaseEventData eventData)
        {
            if(eventData.selectedObject == gameObject)
            {
                StopSelectedLoop();
                StartNormalLoop();
            }
        }
        #endregion

        #region AddGameEvent / RemoveGameEvent
        /// <summary>
        /// Add a game event to the target action type gameEvents list.
        /// </summary>
        public void AddGameEvent(string eventName, ButtonActionType buttonActionType = ButtonActionType.OnClick)
        {
            if(eventName.IsNullOrEmpty()) { return; }
            switch(buttonActionType)
            {
                case ButtonActionType.OnPointerEnter:
                    if(onPointerEnterGameEvents == null) { onPointerEnterGameEvents = new List<string>() { eventName }; break; }
                    if(!onPointerEnterGameEvents.Contains(eventName)) { onPointerEnterGameEvents.Add(eventName); }
                    break;
                case ButtonActionType.OnPointerExit:
                    if(onPointerExitGameEvents == null) { onPointerExitGameEvents = new List<string>() { eventName }; break; }
                    if(!onPointerExitGameEvents.Contains(eventName)) { onPointerExitGameEvents.Add(eventName); }
                    break;
                case ButtonActionType.OnPointerDown:
                    if(onPointerDownGameEvents == null) { onPointerDownGameEvents = new List<string>() { eventName }; break; }
                    if(!onPointerDownGameEvents.Contains(eventName)) { onPointerDownGameEvents.Add(eventName); }
                    break;
                case ButtonActionType.OnPointerUp:
                    if(onPointerUpGameEvents == null) { onPointerUpGameEvents = new List<string>() { eventName }; break; }
                    if(!onPointerUpGameEvents.Contains(eventName)) { onPointerUpGameEvents.Add(eventName); }
                    break;
                case ButtonActionType.OnClick:
                    if(onClickGameEvents == null) { onClickGameEvents = new List<string>() { eventName }; break; }
                    if(!onClickGameEvents.Contains(eventName)) { onClickGameEvents.Add(eventName); }
                    break;
                case ButtonActionType.OnDoubleClick:
                    if(onDoubleClickGameEvents == null) { onDoubleClickGameEvents = new List<string>() { eventName }; break; }
                    if(!onDoubleClickGameEvents.Contains(eventName)) { onDoubleClickGameEvents.Add(eventName); }
                    break;
                case ButtonActionType.OnLongClick:
                    if(onLongClickGameEvents == null) { onLongClickGameEvents = new List<string>() { eventName }; break; }
                    if(!onLongClickGameEvents.Contains(eventName)) { onLongClickGameEvents.Add(eventName); }
                    break;
            }
        }
        /// <summary>
        /// Remove   game event from the target action type gameEvents list.
        /// </summary>
        public void RemoveGameEvent(string eventName, ButtonActionType buttonActionType = ButtonActionType.OnClick)
        {
            if(eventName.IsNullOrEmpty()) { return; }
            switch(buttonActionType)
            {
                case ButtonActionType.OnPointerEnter:
                    if(onPointerEnterGameEvents == null || !onPointerEnterGameEvents.Contains(eventName)) { break; }
                    onPointerEnterGameEvents.Remove(eventName);
                    break;
                case ButtonActionType.OnPointerExit:
                    if(onPointerExitGameEvents == null || !onPointerExitGameEvents.Contains(eventName)) { break; }
                    onPointerExitGameEvents.Remove(eventName);
                    break;
                case ButtonActionType.OnPointerDown:
                    if(onPointerDownGameEvents == null || !onPointerDownGameEvents.Contains(eventName)) { break; }
                    onPointerDownGameEvents.Remove(eventName);
                    break;
                case ButtonActionType.OnPointerUp:
                    if(onPointerUpGameEvents == null || !onPointerUpGameEvents.Contains(eventName)) { break; }
                    onPointerUpGameEvents.Remove(eventName);
                    break;
                case ButtonActionType.OnClick:
                    if(onClickGameEvents == null || !onClickGameEvents.Contains(eventName)) { break; }
                    onClickGameEvents.Remove(eventName);
                    break;
                case ButtonActionType.OnDoubleClick:
                    if(onDoubleClickGameEvents == null || !onDoubleClickGameEvents.Contains(eventName)) { break; }
                    onDoubleClickGameEvents.Remove(eventName);
                    break;
                case ButtonActionType.OnLongClick:
                    if(onLongClickGameEvents == null || !onLongClickGameEvents.Contains(eventName)) { break; }
                    onLongClickGameEvents.Remove(eventName);
                    break;
            }
        }
        #endregion

        public void ResetAnimations()
        {
            StopNormalLoop();
            StopSelectedLoop();
            ResetRectTransform();
            if(EventSystem.current != null && EventSystem.current.currentSelectedGameObject == this) { StartSelectedLoop(); } else { StartNormalLoop(); }
        }

        /// <summary>
        /// Converts enum type from ButtonClickType to ButtonActionType
        /// </summary>
        public static ButtonActionType GetButtonActionType(ButtonClickType clickType)
        {
            switch(clickType)
            {
                case ButtonClickType.OnClick: return ButtonActionType.OnClick;
                case ButtonClickType.OnDoubleClick: return ButtonActionType.OnDoubleClick;
                case ButtonClickType.OnLongClick: return ButtonActionType.OnLongClick;
                default: return ButtonActionType.OnClick;
            }
        }
        /// <summary>
        /// Converts enum type from ButtonActionType to ButtonClickType
        /// </summary>
        public static ButtonClickType GetButtonClickType(ButtonActionType actionType)
        {
            switch(actionType)
            {
                case ButtonActionType.OnClick: return ButtonClickType.OnClick;
                case ButtonActionType.OnDoubleClick: return ButtonClickType.OnDoubleClick;
                case ButtonActionType.OnLongClick: return ButtonClickType.OnLongClick;
                default: return ButtonClickType.OnClick;
            }
        }
    }
}


