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
    [AddComponentMenu(DUI.COMPONENT_MENU_UITOGGLE, DUI.MENU_PRIORITY_UITOGGLE)]
    [RequireComponent(typeof(RectTransform), typeof(Toggle))]
    [DisallowMultipleComponent]
    public class UIToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler
    {
        #region Context Menu
#if UNITY_EDITOR
        [UnityEditor.MenuItem(DUI.GAMEOBJECT_MENU_UITOGGLE, false, DUI.MENU_PRIORITY_UITOGGLE)]
        static void CreateElement(UnityEditor.MenuCommand menuCommand)
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

            GameObject go = new GameObject("UIToggle", typeof(RectTransform), typeof(Toggle), typeof(UIToggle));
            UnityEditor.GameObjectUtility.SetParentAndAlign(go, targetParent);
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            go.GetComponent<UIToggle>().Reset();
            go.GetComponent<RectTransform>().localScale = Vector3.one;
            go.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            go.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 20);
            go.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

            GameObject background = new GameObject("Background", typeof(RectTransform), typeof(Image));
            UnityEditor.GameObjectUtility.SetParentAndAlign(background, go);
            background.GetComponent<RectTransform>().localScale = Vector3.one;
            background.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            background.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            background.GetComponent<RectTransform>().anchoredPosition = new Vector2(10, -10);
            background.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
            background.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            background.GetComponent<Image>().sprite = DUI.UISprite;
            background.GetComponent<Image>().type = Image.Type.Sliced;
            background.GetComponent<Image>().fillCenter = true;
            background.GetComponent<Image>().color = new Color(31f / 255f, 136f / 255f, 201f / 255f);

            GameObject checkmark = new GameObject("Checkmark", typeof(RectTransform), typeof(Image));
            UnityEditor.GameObjectUtility.SetParentAndAlign(checkmark, background);
            checkmark.GetComponent<RectTransform>().localScale = Vector3.one;
            checkmark.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            checkmark.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            checkmark.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            checkmark.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            checkmark.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            checkmark.GetComponent<Image>().sprite = DUI.Checkmark;
            checkmark.GetComponent<Image>().type = Image.Type.Simple;
            checkmark.GetComponent<Image>().fillCenter = true;
            checkmark.GetComponent<Image>().color = new Color(11f / 255f, 50f / 255f, 74f / 255f);

#if dUI_TextMeshPro
            GameObject label = new GameObject("Label TMPro", typeof(RectTransform), typeof(TMPro.TextMeshProUGUI));
            UnityEditor.GameObjectUtility.SetParentAndAlign(label, go);
            label.GetComponent<RectTransform>().localScale = Vector3.one;
            label.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            label.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            label.GetComponent<RectTransform>().anchoredPosition = new Vector2(10, 0);
            label.GetComponent<RectTransform>().sizeDelta = new Vector2(-28, 0);
            label.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            label.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(2f / 255f, 10f / 255f, 15f / 255f);
            label.GetComponent<TMPro.TextMeshProUGUI>().fontSize = 14;
            label.GetComponent<TMPro.TextMeshProUGUI>().alignment = TMPro.TextAlignmentOptions.Left;
            label.GetComponent<TMPro.TextMeshProUGUI>().text = "UIToggle";
#else
            GameObject label = new GameObject("Label", typeof(RectTransform), typeof(Text));
            UnityEditor.GameObjectUtility.SetParentAndAlign(label, go);
            label.GetComponent<RectTransform>().localScale = Vector3.one;
            label.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            label.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            label.GetComponent<RectTransform>().anchoredPosition = new Vector2(10, -0.5f);
            label.GetComponent<RectTransform>().sizeDelta = new Vector2(-28, -3);
            label.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            label.GetComponent<Text>().color = new Color(2f / 255f, 10f / 255f, 15f / 255f);
            label.GetComponent<Text>().fontSize = 14;
            label.GetComponent<Text>().resizeTextForBestFit = false;
            label.GetComponent<Text>().resizeTextMinSize = 12;
            label.GetComponent<Text>().resizeTextMaxSize = 20;
            label.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            label.GetComponent<Text>().alignByGeometry = true;
            label.GetComponent<Text>().supportRichText = true;
            label.GetComponent<Text>().text = "UIToggle";
#endif

            go.GetComponent<Toggle>().interactable = true;
            go.GetComponent<Toggle>().transition = Selectable.Transition.ColorTint;
            go.GetComponent<Toggle>().targetGraphic = background.GetComponent<Image>();
            go.GetComponent<Toggle>().graphic = checkmark.GetComponent<Image>();
            go.GetComponent<Toggle>().isOn = true;

            UnityEditor.Selection.activeObject = go;
        }

#endif
        #endregion

        /// <summary>
        /// All the action types a button can perform.
        /// </summary>
        public enum ToggleActionType { OnPointerEnter, OnPointerExit, OnClick }

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
        /// Special time interval added when deselecting a button. It fixes some anomalies.
        /// </summary>
        public const float DESELECT_BUTTON_DELAY = 0.1f;

        /// <summary>
        /// Enables debug logs.
        /// </summary>
        public bool debug = false;

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
        /// The sound name of the sound that gets played on pointer enter and the toggle is on.
        /// </summary>
        public string onPointerEnterSoundToggleOn = DUI.DEFAULT_SOUND_NAME;
        /// <summary>
        /// The sound name of the sound that gets played on pointer enter and the toggle is of.
        /// </summary>
        public string onPointerEnterSoundToggleOff = DUI.DEFAULT_SOUND_NAME;
        /// <summary>
        /// Used by the custom inspector to allow you to type a sound name instead of selecting it from the UISounds Database.
        /// </summary>
        public bool customOnPointerEnterSoundToggleOn = false;
        /// <summary>
        /// Used by the custom inspector to allow you to type a sound name instead of selecting it from the UISounds Database.
        /// </summary>
        public bool customOnPointerEnterSoundToggleOff = false;
        /// <summary>
        /// UnityEvent invoked when on pointer enter has been captured by the system and the toggle is on.
        /// </summary>
        public UnityEvent OnPointerEnterToggleOn = new UnityEvent();
        /// <summary>
        /// UnityEvent invoked when on pointer enter has been captured by the system and the toggle is off.
        /// </summary>
        public UnityEvent OnPointerEnterToggleOff = new UnityEvent();
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
        /// A list of game events that are sent when on pointer enter has been triggered and the toggle is on.
        /// </summary>
        public List<string> onPointerEnterGameEventsToggleOn;
        /// <summary>
        /// A list of game events that are sent when on pointer enter has been triggered and the toggle is off.
        /// </summary>
        public List<string> onPointerEnterGameEventsToggleOff;
        /// <summary>
        /// UINavigation settings when the toggle is on.
        /// </summary>
        public NavigationPointerData onPointerEnterNavigationToggleOn;
        /// <summary>
        /// UINavigation settings when the toggle is off.
        /// </summary>
        public NavigationPointerData onPointerEnterNavigationToggleOff;

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
        /// The sound name of the sound that gets played on pointer exit and the toggle is on.
        /// </summary>
        public string onPointerExitSoundToggleOn = DUI.DEFAULT_SOUND_NAME;
        /// <summary>
        /// The sound name of the sound that gets played on pointer exit and the toggle is off.
        /// </summary>
        public string onPointerExitSoundToggleOff = DUI.DEFAULT_SOUND_NAME;
        /// <summary>
        /// Used by the custom inspector to allow you to type a sound name instead of selecting it from the UISounds Database.
        /// </summary>
        public bool customOnPointerExitSoundToggleOn = false;
        /// <summary>
        /// Used by the custom inspector to allow you to type a sound name instead of selecting it from the UISounds Database.
        /// </summary>
        public bool customOnPointerExitSoundToggleOff = false;
        /// <summary>
        /// UnityEvent invoked when on pointer exit has been captured by the system and the toggle is on.
        /// </summary>
        public UnityEvent OnPointerExitToggleOn = new UnityEvent();
        /// <summary>
        /// UnityEvent invoked when on pointer exit has been captured by the system and the toggle is off.
        /// </summary>
        public UnityEvent OnPointerExitToggleOff = new UnityEvent();
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
        /// A list of game events that are sent when on pointer exit has been triggered and the toggle is on.
        /// </summary>
        public List<string> onPointerExitGameEventsToggleOn;
        /// <summary>
        /// A list of game events that are sent when on pointer exit has been triggered and the toggle is off.
        /// </summary>
        public List<string> onPointerExitGameEventsToggleOff;
        /// <summary>
        /// UINavigation settings when the toggle is on.
        /// </summary>
        public NavigationPointerData onPointerExitNavigationToggleOn;
        /// <summary>
        /// UINavigation settings when the toggle is off.
        /// </summary>
        public NavigationPointerData onPointerExitNavigationToggleOff;

        /// <summary>
        /// Toggles the OnClick functionality. Not recommeded to be disabled. If you disable this functionality, do some tests to be sure that the button behaves as you want it to.
        /// </summary>
        public bool useOnClick = true;
        /// <summary>
        /// If enabled, the button action and game events are sent after the on click punch animation has finished playing. This is useful if you want be sure the uses sees the button animation.
        /// </summary>
        public bool waitForOnClick = true;
        /// <summary>
        /// The sound name of the sound that gets played on click and the toggle is on.
        /// </summary>
        public string onClickSoundToggleOn = DUI.DEFAULT_SOUND_NAME;
        /// <summary>
        /// The sound name of the sound that gets played on click and the toggle is off.
        /// </summary>
        public string onClickSoundToggleOff = DUI.DEFAULT_SOUND_NAME;
        /// <summary>
        /// Used by the custom inspector to allow you to type a sound name instead of selecting it from the UISounds Database.
        /// </summary>
        public bool customOnClickSoundToggleOn = false;
        /// <summary>
        /// Used by the custom inspector to allow you to type a sound name instead of selecting it from the UISounds Database.
        /// </summary>
        public bool customOnClickSoundToggleOff = false;
        /// <summary>
        /// UnityEvent invoked when on click has been captured by the system and the toggle is on.
        /// </summary>
        public UnityEvent OnClickToggleOn = new UnityEvent();
        /// <summary>
        /// UnityEvent invoked when on click has been captured by the system and the toggle is off.
        /// </summary>
        public UnityEvent OnClickToggleOff = new UnityEvent();
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
        /// A list of game events that are sent when on click has been triggered and the toggle is on.
        /// </summary>
        public List<string> onClickGameEventsToggleOn;
        /// <summary>
        /// A list of game events that are sent when on click has been triggered and the toggle is off.
        /// </summary>
        public List<string> onClickGameEventsToggleOff;
        /// <summary>
        /// UINavigation settings when the toggle is on.
        /// </summary>
        public NavigationPointerData onClickNavigationToggleOn;
        /// <summary>
        /// UINavigation settings when the toggle is off.
        /// </summary>
        public NavigationPointerData onClickNavigationToggleOff;

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
        /// Internal variable that holds a reference to the RectTransform component.
        /// </summary>
        private RectTransform m_rectTransform;
        /// <summary>
        /// Returns the RectTransform component.
        /// </summary>
        public RectTransform RectTransform { get { if(m_rectTransform == null) { m_rectTransform = GetComponent<RectTransform>() == null ? gameObject.AddComponent<RectTransform>() : GetComponent<RectTransform>(); } return m_rectTransform; } }

        /// <summary>
        /// Internal variable that holds a reference to the Button component.
        /// </summary>
        private Toggle m_toggle;
        /// <summary>
        /// Returns the Button component.
        /// </summary>
        public Toggle Toggle { get { if(m_toggle == null) { m_toggle = GetComponent<Toggle>() ?? gameObject.AddComponent<Toggle>(); } return m_toggle; } }

        /// <summary>
        /// Returns true if the button's Toggle component is interactable. This also toggles this button's interactability.
        /// </summary>
        public bool Interactable { get { return Toggle.interactable; } set { Toggle.interactable = value; } }

        /// <summary>
        /// Returns true if the toggle is on and false otherwise. This also toggles this toggle's on/off state.
        /// </summary>
        public bool IsOn { get { return Toggle.isOn; } set { Toggle.isOn = value; } }

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
        /// The controller input mode. If set to ControllerInputMode.None, the toggle will react only to mouse clicks and touches.
        /// </summary>
        public ControllerInputMode controllerInputMode = ControllerInputMode.VirtualButton;
        /// <summary>
        /// The enables the check for alternate inputs for the toggle.
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

            allowMultipleClicks = DUI.DUISettings.UIToggle_allowMultipleClicks;
            disableButtonInterval = DUI.DUISettings.UIToggle_disableButtonInterval;
            deselectButtonOnClick = DUI.DUISettings.UIToggle_deselectButtonOnClick;

            useOnPointerEnter = DUI.DUISettings.UIToggle_useOnPointerEnter;
            onPointerEnterDisableInterval = DUI.DUISettings.UIToggle_onPointerEnterDisableInterval;
            onPointerEnterSoundToggleOn = DUI.DUISettings.UIToggle_onPointerEnterSoundToggleOn;
            onPointerEnterSoundToggleOff = DUI.DUISettings.UIToggle_onPointerEnterSoundToggleOff;
            customOnPointerEnterSoundToggleOn = DUI.DUISettings.UIToggle_customOnPointerEnterSoundToggleOn;
            customOnPointerEnterSoundToggleOff = DUI.DUISettings.UIToggle_customOnPointerEnterSoundToggleOff;
            onPointerEnterPunchPresetCategory = DUI.DUISettings.UIToggle_onPointerEnterPunchPresetCategory;
            onPointerEnterPunchPresetName = DUI.DUISettings.UIToggle_onPointerEnterPunchPresetName;
            loadOnPointerEnterPunchPresetAtRuntime = DUI.DUISettings.UIToggle_loadOnPointerEnterPunchPresetAtRuntime;

            useOnPointerExit = DUI.DUISettings.UIToggle_useOnPointerExit;
            onPointerExitDisableInterval = DUI.DUISettings.UIToggle_onPointerExitDisableInterval;
            onPointerExitSoundToggleOn = DUI.DUISettings.UIToggle_onPointerExitSoundToggleOn;
            onPointerExitSoundToggleOff = DUI.DUISettings.UIToggle_onPointerExitSoundToggleOff;
            customOnPointerExitSoundToggleOn = DUI.DUISettings.UIToggle_customOnPointerExitSoundToggleOn;
            customOnPointerExitSoundToggleOff = DUI.DUISettings.UIToggle_customOnPointerExitSoundToggleOff;
            onPointerExitPunchPresetCategory = DUI.DUISettings.UIToggle_onPointerExitPunchPresetCategory;
            onPointerExitPunchPresetName = DUI.DUISettings.UIToggle_onPointerExitPunchPresetName;
            loadOnPointerExitPunchPresetAtRuntime = DUI.DUISettings.UIToggle_loadOnPointerExitPunchPresetAtRuntime;

            useOnClick = DUI.DUISettings.UIToggle_useOnClickAnimations;
            waitForOnClick = DUI.DUISettings.UIToggle_waitForOnClickAnimation;
            onClickSoundToggleOn = DUI.DUISettings.UIToggle_onClickSoundToggleOn;
            onClickSoundToggleOff = DUI.DUISettings.UIToggle_onClickSoundToggleOff;
            customOnClickSoundToggleOn = DUI.DUISettings.UIToggle_customOnClickSoundToggleOn;
            customOnClickSoundToggleOff = DUI.DUISettings.UIToggle_customOnClickSoundToggleOff;
            onClickPunchPresetCategory = DUI.DUISettings.UIToggle_onClickPunchPresetCategory;
            onClickPunchPresetName = DUI.DUISettings.UIToggle_onClickPunchPresetName;
            loadOnClickPunchPresetAtRuntime = DUI.DUISettings.UIToggle_loadOnClickPunchPresetAtRuntime;

            onPointerEnterNavigationToggleOn = new NavigationPointerData();
            onPointerEnterNavigationToggleOff = new NavigationPointerData();
            onPointerExitNavigationToggleOn = new NavigationPointerData();
            onPointerExitNavigationToggleOff = new NavigationPointerData();
            onClickNavigationToggleOn = new NavigationPointerData();
            onClickNavigationToggleOff = new NavigationPointerData();
        }

        private void Awake()
        {
            startPosition = RectTransform.anchoredPosition3D;
            startRotation = RectTransform.localEulerAngles;
            startScale = RectTransform.localScale;
            startAlpha = GetComponent<CanvasGroup>() == null ? 1 : GetComponent<CanvasGroup>().alpha;
            AddPunchListeners();

            LoadRuntimePunchPresets();
        }

        private void Start()
        {
            StartCoroutine("IUpdateStartPosition");
            ResetRectTransform();
            Initialized = true;
        }

        private void OnEnable()
        {
            StartCoroutine("IUpdateStartPosition");
            if(!Initialized) { return; }
            ResetRectTransform();
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

        private void OnDisable()
        {
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
            if(loadOnPointerEnterPunchPresetAtRuntime)
            {
                Punch presetPunch = UIAnimatorUtil.GetPunch(onPointerEnterPunchPresetCategory, onPointerEnterPunchPresetName);
                if(presetPunch != null) { onPointerEnterPunch = presetPunch.Copy(); }
            }
            if(loadOnPointerExitPunchPresetAtRuntime)
            {
                Punch presetPunch = UIAnimatorUtil.GetPunch(onPointerExitPunchPresetCategory, onPointerExitPunchPresetName);
                if(presetPunch != null) { onPointerExitPunch = presetPunch.Copy(); }
            }
            if(loadOnClickPunchPresetAtRuntime)
            {
                Punch presetPunch = UIAnimatorUtil.GetPunch(onClickPunchPresetCategory, onClickPunchPresetName);
                if(presetPunch != null) { onClickPunch = presetPunch.Copy(); }
            }
        }

        /// <summary>
        /// Initiates all the listeners for the button's actions.
        /// </summary>
        void AddPunchListeners()
        {
            OnPointerEnterToggleOn.AddListener(ExecuteOnPoinerEnterActions);
            OnPointerEnterToggleOff.AddListener(ExecuteOnPoinerEnterActions);
            OnPointerExitToggleOn.AddListener(ExecuteOnPoinerExitActions);
            OnPointerExitToggleOff.AddListener(ExecuteOnPoinerExitActions);
            OnClickToggleOn.AddListener(ExecuteOnClickActions);
            OnClickToggleOff.AddListener(ExecuteOnClickActions);
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
            if(IsOn)
            {
                PlaySound(onPointerEnterSoundToggleOn);
                SendGameEvents(onPointerEnterGameEventsToggleOn);
            }
            else
            {
                PlaySound(onPointerEnterSoundToggleOff);
                SendGameEvents(onPointerEnterGameEventsToggleOff);
            }
            ExecutePunch(onPointerEnterPunch, false);
            UIManager.Instance.SendToggleAction(this, ToggleActionType.OnPointerEnter);
        }
        /// <summary>
        /// Executes all the actions set for OnPointerExit.
        /// </summary>
        void ExecuteOnPoinerExitActions()
        {
            if(ButtonClicksDisabled) { return; }
            if(!useOnPointerExit) { return; }
            if(IsOn)
            {
                PlaySound(onPointerExitSoundToggleOn);
                SendGameEvents(onPointerExitGameEventsToggleOn);
            }
            else
            {
                PlaySound(onPointerExitSoundToggleOff);
                SendGameEvents(onPointerExitGameEventsToggleOff);
            }
            ExecutePunch(onPointerExitPunch, false);
            UIManager.Instance.SendToggleAction(this, ToggleActionType.OnPointerExit);
        }

        /// <summary>
        /// Executes all the actions set for OnClick.
        /// </summary>
        void ExecuteOnClickActions()
        {
            if(ButtonClicksDisabled) { return; }
            PlaySound(IsOn ? onClickSoundToggleOn : onClickSoundToggleOff);
            StartCoroutine(iExecuteClickActionsWithDelay());
            ExecutePunch(onClickPunch, deselectButtonOnClick);
        }

        IEnumerator iExecuteClickActionsWithDelay()
        {
            if(waitForOnClick) { yield return new WaitForSecondsRealtime(onClickPunch.TotalDuration); }
            SendGameEvents(IsOn ? onClickGameEventsToggleOn : onClickGameEventsToggleOff);
            UIManager.Instance.SendToggleAction(this, ToggleActionType.OnClick);
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
        void ExecutePunch(Punch punch, bool deselectButton = false, bool forced = false)
        {
            if(punch == null) { return; }
            UIAnimator.PunchMove(RectTransform, startPosition, punch, null, null, forced);
            UIAnimator.PunchRotate(RectTransform, startRotation, punch, null, null, forced);
            UIAnimator.PunchScale(RectTransform, startScale, punch, null, null, forced);
            if(deselectButton) { DeselectButton(punch.TotalDuration); }
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
            if(debug) { Debug.Log("[Doozy][UIToggle] - " + name + " | OnPointerEnter triggered"); }
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
                if(debug) { Debug.Log("[Doozy][UIToggle] - " + name + " | Executing OnPointerEnter" + (forced ? "initiated through forced" : "")); }
                if(onPointerEnterDisableInterval > 0) { StartCoroutine("DisableOnPointerEnter"); }
                if(IsOn)
                {
                    OnPointerEnterToggleOn.Invoke();
                }
                else
                {
                    OnPointerEnterToggleOff.Invoke();
                }
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
            if(debug) { Debug.Log("[Doozy][UIToggle] - " + name + " | OnPointerExit triggered"); }
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
                if(debug) { Debug.Log("[Doozy][UIToggle] - " + name + " | Executing OnPointerExit" + (forced ? "initiated through forced" : "")); }
                if(onPointerExitDisableInterval > 0) { StartCoroutine("DisableOnPointerExit"); }
                if(IsOn)
                {
                    OnPointerExitToggleOn.Invoke();
                }
                else
                {
                    OnPointerExitToggleOff.Invoke();
                }
            }
        }
        IEnumerator DisableOnPointerExit()
        {
            onPointerExitReady = false;
            yield return new WaitForSecondsRealtime(onPointerExitDisableInterval);
            onPointerExitReady = true;
        }
        #endregion
        #region OnClick
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if(debug) { Debug.Log("[Doozy][UIToggle] - " + name + " | OnPointerClick triggered"); }
            RegisterClick();
        }
        void RegisterClick()
        {
            StartCoroutine(ClickRegistered());
        }
        IEnumerator ClickRegistered()
        {
            yield return new WaitForEndOfFrame();
            ExecuteClick();
            if(!allowMultipleClicks) { DisableButtonAfterClick(); }
        }
        /// <summary>
        /// Executes the OnClick trigger. You can force an execution of this trigger (regardless if it's enabled or not) by calling this method with forced set to TRUE
        /// </summary>
        /// <param name="forced">Fires this trigger regardless if it is enabled or not (default:false)</param>
        public void ExecuteClick(bool forced = false)
        {
            if(ButtonClicksDisabled) { return; }
            if(debug) { Debug.Log("[Doozy][UIToggle] - " + name + " | Executing OnClick" + (forced ? "initiated through forced" : "")); }
            if(Interactable || forced)
            {
                if(IsOn)
                {
                    OnClickToggleOn.Invoke();
                }
                else
                {
                    OnClickToggleOff.Invoke();
                }
            }
            if(!Interactable & onPointerExitReady)
            {
                if(IsOn)
                {
                    OnPointerExitToggleOn.Invoke();
                }
                else
                {
                    OnPointerExitToggleOff.Invoke();
                }
            }
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
                //SELECTED
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
                //DESELECTED
            }
        }
        #endregion

        #region AddGameEvent / RemoveGameEvent
        /// <summary>
        /// Add a game event to the target action type gameEvents list.
        /// </summary>
        public void AddGameEvent(string eventName, bool whenToggleIsOn, ToggleActionType toggleActionType = ToggleActionType.OnClick)
        {
            if(eventName.IsNullOrEmpty()) { return; }
            switch(toggleActionType)
            {
                case ToggleActionType.OnPointerEnter:
                    if(whenToggleIsOn)
                    {
                        if(onPointerEnterGameEventsToggleOn == null) { onPointerEnterGameEventsToggleOn = new List<string>() { eventName }; break; }
                        if(!onPointerEnterGameEventsToggleOn.Contains(eventName)) { onPointerEnterGameEventsToggleOn.Add(eventName); }
                    }
                    else
                    {
                        if(onPointerEnterGameEventsToggleOff == null) { onPointerEnterGameEventsToggleOff = new List<string>() { eventName }; break; }
                        if(!onPointerEnterGameEventsToggleOff.Contains(eventName)) { onPointerEnterGameEventsToggleOff.Add(eventName); }
                    }
                    break;
                case ToggleActionType.OnPointerExit:
                    if(whenToggleIsOn)
                    {
                        if(onPointerExitGameEventsToggleOn == null) { onPointerExitGameEventsToggleOn = new List<string>() { eventName }; break; }
                        if(!onPointerExitGameEventsToggleOn.Contains(eventName)) { onPointerExitGameEventsToggleOn.Add(eventName); }
                    }
                    else
                    {
                        if(onPointerExitGameEventsToggleOff == null) { onPointerExitGameEventsToggleOff = new List<string>() { eventName }; break; }
                        if(!onPointerExitGameEventsToggleOff.Contains(eventName)) { onPointerExitGameEventsToggleOff.Add(eventName); }
                    }
                    break;
                case ToggleActionType.OnClick:
                    if(whenToggleIsOn)
                    {
                        if(onClickGameEventsToggleOn == null) { onClickGameEventsToggleOn = new List<string>() { eventName }; break; }
                        if(!onClickGameEventsToggleOn.Contains(eventName)) { onClickGameEventsToggleOn.Add(eventName); }
                    }
                    else
                    {
                        if(onClickGameEventsToggleOff == null) { onClickGameEventsToggleOff = new List<string>() { eventName }; break; }
                        if(!onClickGameEventsToggleOff.Contains(eventName)) { onClickGameEventsToggleOff.Add(eventName); }
                    }
                    break;
            }
        }
        /// <summary>
        /// Remove   game event from the target action type gameEvents list.
        /// </summary>
        public void RemoveGameEvent(string eventName, bool whenToggleIsOn, ToggleActionType toggleActionType = ToggleActionType.OnClick)
        {
            if(eventName.IsNullOrEmpty()) { return; }
            switch(toggleActionType)
            {
                case ToggleActionType.OnPointerEnter:
                    if(whenToggleIsOn)
                    {
                        if(onPointerEnterGameEventsToggleOn == null || !onPointerEnterGameEventsToggleOn.Contains(eventName)) { break; }
                        onPointerEnterGameEventsToggleOn.Remove(eventName);
                    }
                    else
                    {
                        if(onPointerEnterGameEventsToggleOff == null || !onPointerEnterGameEventsToggleOff.Contains(eventName)) { break; }
                        onPointerEnterGameEventsToggleOff.Remove(eventName);
                    }
                    break;
                case ToggleActionType.OnPointerExit:
                    if(whenToggleIsOn)
                    {
                        if(onPointerExitGameEventsToggleOn == null || !onPointerExitGameEventsToggleOn.Contains(eventName)) { break; }
                        onPointerExitGameEventsToggleOn.Remove(eventName);
                    }
                    else
                    {
                        if(onPointerExitGameEventsToggleOff == null || !onPointerExitGameEventsToggleOff.Contains(eventName)) { break; }
                        onPointerExitGameEventsToggleOff.Remove(eventName);
                    }
                    break;
                case ToggleActionType.OnClick:
                    if(whenToggleIsOn)
                    {
                        if(onClickGameEventsToggleOn == null || !onClickGameEventsToggleOn.Contains(eventName)) { break; }
                        onClickGameEventsToggleOn.Remove(eventName);
                    }
                    else
                    {
                        if(onClickGameEventsToggleOff == null || !onClickGameEventsToggleOff.Contains(eventName)) { break; }
                        onClickGameEventsToggleOff.Remove(eventName);
                    }
                    break;
            }
        }
        #endregion
    }
}
