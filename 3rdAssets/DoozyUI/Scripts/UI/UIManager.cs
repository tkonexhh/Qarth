// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using DG.Tweening;
using DoozyUI.Gestures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DoozyUI
{
    [RequireComponent(typeof(Soundy))]
    [RequireComponent(typeof(UINotificationManager))]
    [DisallowMultipleComponent]
    public class UIManager : MonoBehaviour
    {
        protected UIManager() { }

        /// <summary>
        /// The instance
        /// </summary>
        private static UIManager _instance;
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static UIManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    if(applicationIsQuitting)
                    {
                        Debug.LogWarning("[Singleton] Instance '" + typeof(UIManager) + "' already destroyed on application quit. Won't create again - returning null.");
                        return null;
                    }

                    _instance = FindObjectOfType<UIManager>();

                    if(_instance == null)
                    {
                        GameObject singleton = new GameObject("UIManager");
                        _instance = singleton.AddComponent<UIManager>();
                        DontDestroyOnLoad(singleton);
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// The application is quitting.
        /// </summary>
        private static bool applicationIsQuitting = false;
        /// <summary>
        /// Called when [application quit].
        /// </summary>
        private void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }

        #region Context Menu
#if UNITY_EDITOR
        [UnityEditor.MenuItem(DUI.TOOLS_MENU_UIMANAGER, false, DUI.MENU_PRIORITY_UIMANAGER)]
        [UnityEditor.MenuItem(DUI.GAMEOBJECT_MENU_UIMANAGER, false, DUI.MENU_PRIORITY_UIMANAGER)]
        static void CreateUIManager(UnityEditor.MenuCommand menuCommand)
        {
            if(FindObjectOfType<UIManager>() != null)
            {
                Debug.Log("[UI Manager] Cannot add another UIManager to this Scene because you don't need and should not have more than one.");
                UnityEditor.Selection.activeObject = FindObjectOfType<UIManager>();
                return;
            }

            GameObject go = new GameObject("UIManager", typeof(UIManager));
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            UnityEditor.Selection.activeObject = go;
        }
#endif
        #endregion

        /// <summary>
        /// Prints debug messages related to game events at runtime.
        /// </summary>
        public bool debugGameEvents = false;
        /// <summary>
        /// Prints debug messages related to UIButtons at runtime.
        /// </summary>
        public bool debugUIButtons = false;
        /// <summary>
        /// Prints debug messages related to UIElements at runtime.
        /// </summary>
        public bool debugUIElements = false;
        /// <summary>
        /// Prints debug messages related to UINotifications at runtime.
        /// </summary>
        public bool debugUINotifications = false;
        /// <summary>
        /// Prints debug messages related to UICanvases at runtime.
        /// </summary>
        public bool debugUICanvases = false;

        /// <summary>
        /// Should the system disable button clicks when an UIElement is in transition (an In or Out animation is running). Default is true.
        /// </summary>
        public bool autoDisableButtonClicks = true;
        /// <summary>
        /// Internal variable used to keep track if button clicks are disabled or not. This affects all the UIButtons.
        /// <para>This is an additive bool so if == 0 --> false (button clicks are NOT disabled) and if > 0 --> true (button clicks are disabled).</para>
        /// </summary>
        private int buttonClicksDisableLevel = 0;
        /// <summary>
        /// Returns true if button clicks are disabled and false otherwise. This is mosty used when an UIElement is in transition and, in order to prevent accidental clicks, the buttons need to be disabled.
        /// </summary>
        public bool ButtonClicksDisabled
        {
            get
            {
                if(buttonClicksDisableLevel < 0) { buttonClicksDisableLevel = 0; }
                return buttonClicksDisableLevel == 0 ? false : true;
            }
        }

        /// <summary>
        /// Returns true if the UI Navigation is enabled and false otherwise. It is set to false if Scripting Define Symbols, for the current active platform, contain the 'dUI_NavigationDisabled' symbol.
        /// <para>In you want to handle the UI Navigation yourself just disable the UI Navigation from the Control Panel.</para>
        /// </summary>
        public static bool IsNavigationEnabled { get { return UINavigation.IsNavigationEnabled; } }

        /// <summary>
        /// Internal variable used to keep track if the 'Back' button is disabled or not.
        /// <para>This is an additive bool so if == 0 --> false (the 'Back' button is NOT disabled) and if > 0 --> true (the 'Back' button is disabled).</para>
        /// </summary>
        private int backButtonDisableLevel = 0;
        /// <summary>
        /// Returns true if the 'Back' button is disabled and false otherwise.
        /// </summary>
        public bool BackButtonDisabled
        {
            get
            {
                if(backButtonDisableLevel < 0) { backButtonDisableLevel = 0; }
                return backButtonDisableLevel == 0 ? false : true;
            }
        }

        /// <summary>
        /// Returns true if the game has been paused (by the UIManager) and false otherwise.
        /// </summary>
        public bool gamePaused = false;
        /// <summary>
        /// Every time the user pauses the game, this variable stores the current Time.timeScale value. This is needed so that when the game needs to get unpaused, UIManager will know at what timescale should the game return to.
        /// </summary>
        public float currentGameTimeScale = 1;

        /// <summary>
        /// Returns true if the sound is on and false otherwise. This variable knows only if the sound is on for DoozyUI and not for anything else as it checks a PlayerPrefs int value named 'soundState'.
        /// </summary>
        public bool isSoundOn = true;
        /// <summary>
        /// Returns true if the music is on and false otherwise. This variable knows only if the music is on for DoozyUI and not for anything else as it checks a PlayerPrefs int value named 'musicState'.
        /// </summary>
        public bool isMusicOn = true;

        public const KeyCode DEFAULT_BACK_BUTTON_KEY_CODE = KeyCode.Escape;
        public const KeyCode DEFAULT_BACK_BUTTON_KEY_CODE_ALT = KeyCode.Backspace;
        public const string DEFAULT_BACK_BUTTON_VIRTUAL_BUTTON_NAME = "Cancel";
        public const string DEFAULT_BACK_BUTTON_VIRTUAL_BUTTON_NAME_ALT = "Cancel";

        /// <summary>
        /// The controller input mode. If set to ControllerInputMode.None, the 'Back' button will react only to mouse clicks and touches.
        /// </summary>
        public ControllerInputMode backButtonInputMode = ControllerInputMode.VirtualButton;
        /// <summary>
        /// The enables the check for alternate inputs for the 'Back' button.
        /// <para>If the controllerInputMode is set to None, this option does nothing.</para>
        /// <para>If the controllerInputMode is set to KeyCode, this option enables the setting for an alternate button (KeyCode) to register a 'Back' button click.</para>
        /// <para>Is the controllerInputMode is set to VirtualButton, this option enabled the setting for an alternate virtual button (button name set in the InputManager) to register a 'Back' button click.</para>
        /// </summary>
        public bool enableBackButtonAlternateInputs = false;
        /// <summary>
        /// The 'Back' button click key code.
        /// </summary>
        public KeyCode backButtonKeyCode = DEFAULT_BACK_BUTTON_KEY_CODE;
        /// <summary>
        /// The 'Back' button click key code alternate.
        /// </summary>
        public KeyCode backButtonKeyCodeAlt = DEFAULT_BACK_BUTTON_KEY_CODE_ALT;
        /// <summary>
        /// The 'Back' button click virtual button name (set int the InputManager).
        /// </summary>
        public string backButtonVirtualButtonName = DEFAULT_BACK_BUTTON_VIRTUAL_BUTTON_NAME;
        /// <summary>
        /// The 'Back' button virtual button name alternate (set in the InputManager).
        /// </summary>
        public string backButtonVirtualButtonNameAlt = DEFAULT_BACK_BUTTON_VIRTUAL_BUTTON_NAME_ALT;

#if dUI_UseOrientationManager
        /// <summary>
        /// Determines if the Orientation Manager should be used. This value is automatically set to true when the 'dUI_UseOrientationManager' Scripting Define Symbol has been added to the current active platform.
        /// </summary>
        public static bool useOrientationManager = true;
#else
        /// <summary>
        /// Determines if the Orientation Manager should be used. This value is automatically set to false when the 'dUI_UseOrientationManager' Scripting Define Symbol has not been added to the currently active platform.
        /// </summary>
        public static bool useOrientationManager = false;
#endif
        /// <summary>
        /// Types of orientation used by DoozyUI. Unknown is used for initialization purposes.
        /// </summary>
        public enum Orientation { Landscape, Portrait, Unknown }
        /// <summary>
        /// Returns the current orientation of the device. Default is Orientation.Unknown because that triggers an orientation check/update.
        /// </summary>
        public Orientation currentOrientation = Orientation.Unknown;

        /// <summary>
        /// Global static variable that determines if the UINotification look for TextMeshProUGUI component instead of a Text componenet when looking for text.
        /// <para>TextMeshPro support is currently in limbo as we wait to see what Unity does with it.</para>
        /// </summary>
        public static bool usesTMPro = false;
        public bool useTextMeshPro = false;                         //Used to change in the inspector the settings for the static variable

#if dUI_SceneLoaderDisabled
        /// <summary>
        /// Determines if the Scene Loader should be automatically loaded. This value is automatically set to false when the 'dUI_SceneLoaderDisabled' Scripting Define Symbol has been added to the current active platform.
        /// </summary>
        public static bool useSceneLoader = false;
#else
        /// <summary>
        /// Determines if the Scene Loader should be automatically loaded. This value is automatically set to true when the 'dUI_SceneLoaderDisabled' Scripting Define Symbol has not been added to the currently active platform.
        /// </summary>
        public static bool useSceneLoader = true;
#endif


        /// <summary>
        /// Internal dictionary that keeps track of all the registered UICanvases.
        /// </summary>
        private static Dictionary<string, UICanvas> m_canvasDatabase;
        /// <summary>
        /// Returns a registry of all the registered UICanvases.
        /// </summary>
        public static Dictionary<string, UICanvas> CanvasDatabase { get { if(m_canvasDatabase == null) { m_canvasDatabase = new Dictionary<string, UICanvas>(); } return m_canvasDatabase; } }

        /// <summary>
        /// Internal dictionary that keeps track of all the registered UIElements.
        /// </summary>
        private static Dictionary<string, List<UIElement>> m_elementDatabase;
        /// <summary>
        /// Returns a registry of all the registered UIElements.
        /// </summary>
        public static Dictionary<string, List<UIElement>> ElementDatabase { get { if(m_elementDatabase == null) { m_elementDatabase = new Dictionary<string, List<UIElement>>(); } return m_elementDatabase; } }
        /// <summary>
        /// Internal list used as a data container whenever the system needs a list of UIElements to show.
        /// </summary>
        private static List<UIElement> showElementsList = new List<UIElement>();
        /// <summary>
        /// Internal list used as a data container whenever the system needs a list of UIElements to hide.
        /// </summary>
        private static List<UIElement> hideElementsList = new List<UIElement>();
        private static List<UIElement> getUIElementsList = new List<UIElement>();
        private static List<UIElement> getVisibleUIElements = new List<UIElement>();

        /// <summary>
        /// Internal dictionary that keeps track of all the registered UIEffects.
        /// </summary>
        private static Dictionary<string, List<UIEffect>> m_effectDatabase;
        /// <summary>
        /// Returns a registry of all the registered UIEffects.
        /// </summary>
        public static Dictionary<string, List<UIEffect>> EffectDatabase { get { if(m_effectDatabase == null) { m_effectDatabase = new Dictionary<string, List<UIEffect>>(); } return m_effectDatabase; } }
        /// <summary>
        /// Internal list used as a data container whenever the system needs a list of UIEffects to show.
        /// </summary>
        private static List<UIEffect> showEffectsList = new List<UIEffect>();
        /// <summary>
        /// Internal list used as a data container whenever the system needs a list of UIEffects to hide.
        /// </summary>
        private static List<UIEffect> hideEffectsList = new List<UIEffect>();
        private static List<UIEffect> getUIEffectsList = new List<UIEffect>();

        /// <summary>
        /// Internal dictionary that keeps track of all the registered UITrigges that listen for game events.
        /// </summary>
        private static Dictionary<string, List<UITrigger>> m_gameEventsTriggerDatabase;
        /// <summary>
        /// Returns a registry of all the registered UITriggers that listens for game events.
        /// </summary>
        public static Dictionary<string, List<UITrigger>> GameEventsTriggerDatabase { get { if(m_gameEventsTriggerDatabase == null) { m_gameEventsTriggerDatabase = new Dictionary<string, List<UITrigger>>(); } return m_gameEventsTriggerDatabase; } }
        /// <summary>
        /// Internal dictionary that keeps track of all the registered UITrigges that listen for button clicks.
        /// </summary>
        private static Dictionary<string, List<UITrigger>> m_buttonClicksTriggerDatabase;
        /// <summary>
        /// Returns a registry of all the registered UITriggers that listens for button clicks.
        /// </summary>
        public static Dictionary<string, List<UITrigger>> ButtonClicksTriggerDatabase { get { if(m_buttonClicksTriggerDatabase == null) { m_buttonClicksTriggerDatabase = new Dictionary<string, List<UITrigger>>(); } return m_buttonClicksTriggerDatabase; } }
        /// <summary>
        /// Internal dictionary that keeps track of all the registered UITrigges that listen for button double clicks.
        /// </summary>
        private static Dictionary<string, List<UITrigger>> m_buttonDoubleClicksTriggerDatabase;
        /// <summary>
        /// Returns a registry of all the registered UITriggers that listens for button clicks.
        /// </summary>
        public static Dictionary<string, List<UITrigger>> ButtonDoubleClicksTriggerDatabase { get { if(m_buttonDoubleClicksTriggerDatabase == null) { m_buttonDoubleClicksTriggerDatabase = new Dictionary<string, List<UITrigger>>(); } return m_buttonDoubleClicksTriggerDatabase; } }
        /// <summary>
        /// Internal dictionary that keeps track of all the registered UITrigges that listen for button long clicks.
        /// </summary>
        private static Dictionary<string, List<UITrigger>> m_buttonLongClicksTriggerDatabase;
        /// <summary>
        /// Returns a registry of all the registered UITriggers that listens for button clicks.
        /// </summary>
        public static Dictionary<string, List<UITrigger>> ButtonLongClicksTriggerRegistry { get { if(m_buttonLongClicksTriggerDatabase == null) { m_buttonLongClicksTriggerDatabase = new Dictionary<string, List<UITrigger>>(); } return m_buttonLongClicksTriggerDatabase; } }
        /// <summary>
        /// Internal list used as a data container whenever the system needs a list of UITriggers.
        /// </summary>
        private static List<UITrigger> triggerTheTriggersList = new List<UITrigger>();
        private static List<UITrigger> getUITriggersList = new List<UITrigger>();

#if dUI_PlayMaker
        /// <summary>
        /// Internal dictionary that keeps track of all the registered PlaymakerEventDispatchers.
        /// </summary>
        private static List<PlaymakerEventDispatcher> m_playmakerEventDispatcherDatabase;
        /// <summary>
        /// Returns a registry of all the registered PlaymakerEventDispatchers.
        /// </summary>
        public static List<PlaymakerEventDispatcher> PlaymakerEventDispatcherDatabase { get { if (m_playmakerEventDispatcherDatabase == null) { m_playmakerEventDispatcherDatabase = new List<PlaymakerEventDispatcher>(); } return m_playmakerEventDispatcherDatabase; } }
#endif

        /// <summary>
        /// Internal variable that holds a reference to the SceneLoader.
        /// </summary>
        private SceneLoader m_SceneLoader;
        /// <summary>
        /// Returns the SceneLoader reference.
        /// </summary>
        public SceneLoader SceneLoader
        {
            get
            {
                if(m_SceneLoader == null) { m_SceneLoader = FindObjectOfType<SceneLoader>(); } //SceneLoader has not been referenced. Using find to get it.
                if(m_SceneLoader == null) { m_SceneLoader = SceneLoader.AddSceneLoaderToScene(); } //SceneLoader not found. Adding it to the current scene.
                return m_SceneLoader;
            }
        }

        /// <summary>
        /// Internal static reference to the UICanvas named 'MasterCanvas'. There can be only one.
        /// </summary>
        private static UICanvas masterCanvas;

        /// <summary>
        /// Internal variable that holds a reference to the Soundy component.
        /// </summary>
        private static Soundy m_Soundy;
        /// <summary>
        /// Returns the Soundy component.
        /// </summary>
        public static Soundy Soundy { get { if(m_Soundy == null) { m_Soundy = Instance.gameObject.GetComponent<Soundy>() == null ? Instance.gameObject.AddComponent<Soundy>() : Instance.gameObject.GetComponent<Soundy>(); } return m_Soundy; } }
        /// <summary>
        /// Internal variable that holds a reference to the UINotificationManager component.
        /// </summary>
        private static UINotificationManager m_NotificationManager;
        /// <summary>
        /// Returns the UINotificationManager component.
        /// </summary>
        public static UINotificationManager NotificationManager { get { if(m_NotificationManager == null) { m_NotificationManager = Instance.gameObject.GetComponent<UINotificationManager>() == null ? Instance.gameObject.AddComponent<UINotificationManager>() : Instance.gameObject.GetComponent<UINotificationManager>(); } return m_NotificationManager; } }

        /// <summary>
        /// Internal variable that holds a reference to the OrientationManager.
        /// </summary>
        private OrientationManager m_orientationManager;
        /// <summary>
        /// Returns the OrientationManager reference.
        /// </summary>
        public OrientationManager OrientationManager
        {
            get
            {
                if(!useOrientationManager)
                {
                    Debug.Log("[DoozyUI] OrientationManger has not been enabled.");
                    return null;
                }
                if(m_orientationManager == null) //OrientationManager has not been referenced. Using find to get it.
                {
                    m_orientationManager = FindObjectOfType<OrientationManager>();
                }
                if(m_orientationManager == null) //OrientationManager not found. Adding it to the current scene.
                {
                    m_orientationManager = OrientationManager.AddOrientationManagerToScene();
                }
                return m_orientationManager;
            }
        }

        /// <summary>
        /// Internal variable that holds a reference to the TouchManager.
        /// </summary>
        private TouchManager m_touchManager;
        /// <summary>
        /// Returns the TouchManager reference.
        /// </summary>
        public TouchManager TouchManager
        {
            get
            {
                if(m_touchManager == null) //TouchManager has not been referenced. Using find to get it.
                {
                    m_touchManager = FindObjectOfType<TouchManager>();

                    if(m_touchManager == null) //TouchManager not found. Adding it to the current scene.
                    {
                        m_touchManager = TouchManager.AddTouchManagerToScene();
                    }
                }
                return m_touchManager;
            }
        }

        private static int len;
        private static int count;

        void Awake()
        {
            if(_instance != null && _instance != this)
            {
                Debug.Log("[DoozyUI] There cannot be two UIManagers active at the same time. Destryoing this one!");
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);


            DOTween.Init();
            if(useOrientationManager)
            {
                m_orientationManager = OrientationManager;
                OrientationManager.CheckDeviceOrientation(true);
            }
            if(useSceneLoader) { m_SceneLoader = SceneLoader; }
            usesTMPro = useTextMeshPro;
        }

        void Start()
        {
            SoundCheck();
            MusicCheck();
            currentGameTimeScale = Time.timeScale;
        }

        void Update()
        {
            ListenForBackButton();
        }

        /// <summary>
        /// This is the main Game Event trigger.
        /// </summary>
        private void OnGameEvent(string gameEvent)
        {
            if(Instance.debugGameEvents) { Debug.Log("[DoozyUI] [UIManager] [OnGameEvent] ['" + gameEvent + "' game event]"); }

#if dUI_PlayMaker
            SendEventToPlaymaker(gameEvent, DUI.EventType.GameEvent);
#endif

            Instance.TriggerTheTriggers(gameEvent, DUI.EventType.GameEvent);
            if(useSceneLoader) { Instance.SceneLoader.OnGameEvent(gameEvent); }

            switch(gameEvent)
            {
                case "ClearNavigationHistory": //cleares the navigation history (now the back button shows only the quit menu)
                    UINavigation.ClearNavigationHistory();
                    break;
                case "DisableBackButton": //disables the back button functionality (for special cases)
                    DisableBackButton();
                    break;
                case "EnableBackButton": //enables the back button functionality (for special cases) (the back button is enabled by default)
                    EnableBackButton();
                    break;
                case "SoundCheck": //does a sound check and shows the proper sound button state
                    SoundCheck();
                    break;
                case "MusicCheck": //does a music check and shows the proper music button state
                    MusicCheck();
                    break;
            }
        }

        #region Game Events
        /// <summary>
        /// Sends the given game event.
        /// </summary>
        public static void SendGameEvent(string gameEvent) { Instance.OnGameEvent(gameEvent); }
        /// <summary>
        /// Sends the given list of game events.
        /// </summary>
        /// <param name="gameEvents"></param>
        public static void SendGameEvents(List<string> gameEvents) { if(gameEvents != null) { for(int i = 0; i < gameEvents.Count; i++) { Instance.OnGameEvent(gameEvents[i]); } } }
        #endregion

        /// <summary>
        /// This is the main Button Action trigger (previously known as the Button Click trigger).
        /// <para>Note: Only OnClick will the button name be taken into account. All the other actionTypes are used only for navigation purposes.</para>
        /// </summary>
        private void OnButtonAction(string buttonName, UIButton uiButton = null, UIButton.ButtonActionType actionType = UIButton.ButtonActionType.OnClick)
        {
            if(Instance.debugUIButtons) { Debug.Log("[DoozyUI] [UIManager] [OnButtonAction] [" + actionType + "] ['" + buttonName + "' button name]"); }

            if(actionType == UIButton.ButtonActionType.OnClick)
            {
                if(BackButtonDisabled && buttonName.Equals(DUI.BACK_BUTTON_NAME)) { return; } //if the back button is disabled and the user presses the 'Back' button, then we do not send the event further
            }

            switch(actionType)
            {
                case UIButton.ButtonActionType.OnClick:
#if dUI_PlayMaker
                    SendEventToPlaymaker(buttonName, DUI.EventType.ButtonClick);
#endif
                    Instance.TriggerTheTriggers(buttonName, DUI.EventType.ButtonClick);
                    break;
                case UIButton.ButtonActionType.OnDoubleClick:
#if dUI_PlayMaker
                    SendEventToPlaymaker(buttonName, DUI.EventType.ButtonDoubleClick);
#endif
                    Instance.TriggerTheTriggers(buttonName, DUI.EventType.ButtonDoubleClick);
                    break;
                case UIButton.ButtonActionType.OnLongClick:
#if dUI_PlayMaker
                    SendEventToPlaymaker(buttonName, DUI.EventType.ButtonLongClick);
#endif
                    Instance.TriggerTheTriggers(buttonName, DUI.EventType.ButtonLongClick);
                    break;
            }

            if(!IsNavigationEnabled) { return; }

            if(actionType == UIButton.ButtonActionType.OnClick)
            {
                if(buttonName.Equals(DUI.BACK_BUTTON_NAME)) { BackButtonEvent(); return; }

                switch(buttonName)
                {
                    //case "GoToMainMenu": //goes to the main menu of the application and cleares the navigation history (now the back button shows only the quit menu)
                    //    UINavigation.ClearNavigationHistory();
                    //    if (gamePaused)
                    //    {
                    //        TogglePause();
                    //    }
                    //    break;

                    case "TogglePause": TogglePause(); break;
                    case "ToggleSound": ToggleSound(); break;
                    case "ToggleMusic": ToggleMusic(); break;
                    case "ApplicationQuit": ApplicationQuit(); break;
                }
            }

            if(uiButton == null) { return; }

            switch(actionType)
            {
                case UIButton.ButtonActionType.OnPointerEnter: UINavigation.UpdateTheNavigationHistory(uiButton.onPointerEnterNavigation.Copy()); break;
                case UIButton.ButtonActionType.OnPointerExit: UINavigation.UpdateTheNavigationHistory(uiButton.onPointerExitNavigation.Copy()); break;
                case UIButton.ButtonActionType.OnPointerDown: UINavigation.UpdateTheNavigationHistory(uiButton.onPointerDownNavigation.Copy()); break;
                case UIButton.ButtonActionType.OnPointerUp: UINavigation.UpdateTheNavigationHistory(uiButton.onPointerUpNavigation.Copy()); break;
                case UIButton.ButtonActionType.OnClick: UINavigation.UpdateTheNavigationHistory(uiButton.onClickNavigation.Copy()); break;
                case UIButton.ButtonActionType.OnDoubleClick: UINavigation.UpdateTheNavigationHistory(uiButton.onDoubleClickNavigation.Copy()); break;
                case UIButton.ButtonActionType.OnLongClick: UINavigation.UpdateTheNavigationHistory(uiButton.onLongClickNavigation.Copy()); break;
            }
        }

        private void OnToggleAction(UIToggle uiToggle, UIToggle.ToggleActionType actionType)
        {
            if(uiToggle == null) { return; }
            if(Instance.debugUIButtons) { Debug.Log("[DoozyUI] [UIManager] [OnToggleAction] [" + actionType + "] ['" + uiToggle.name + "' gameObject toggle name]"); }
            if(!IsNavigationEnabled) { return; }
            switch(actionType)
            {
                case UIToggle.ToggleActionType.OnPointerEnter: UINavigation.UpdateTheNavigationHistory(uiToggle.IsOn ? uiToggle.onPointerEnterNavigationToggleOn.Copy() : uiToggle.onPointerEnterNavigationToggleOff.Copy()); break;
                case UIToggle.ToggleActionType.OnPointerExit: UINavigation.UpdateTheNavigationHistory(uiToggle.IsOn ? uiToggle.onPointerExitNavigationToggleOn.Copy() : uiToggle.onPointerExitNavigationToggleOff.Copy()); break;
                case UIToggle.ToggleActionType.OnClick: UINavigation.UpdateTheNavigationHistory(uiToggle.IsOn ? uiToggle.onClickNavigationToggleOn.Copy() : uiToggle.onClickNavigationToggleOff.Copy()); break;
            }
        }

        #region Button Actions
        /// <summary>
        /// Use SendButtonAction instead.
        /// </summary>
        [System.Obsolete]
        public static void SendButtonClick(string buttonName, bool addToNavigationHistory = false, List<string> showElements = null, List<string> hideElements = null, List<string> gameEvents = null)
        {
            UIButton b = new UIButton() { buttonName = buttonName };
            b.onClickNavigation = new NavigationPointerData(addToNavigationHistory);
            if(showElements != null) { for(int i = 0; i < showElements.Count; i++) { b.onClickNavigation.show.Add(new NavigationPointer(DUI.UNCATEGORIZED_CATEGORY_NAME, showElements[i])); } }
            if(hideElements != null) { for(int i = 0; i < hideElements.Count; i++) { b.onClickNavigation.hide.Add(new NavigationPointer(DUI.UNCATEGORIZED_CATEGORY_NAME, hideElements[i])); } }
            Instance.OnButtonAction(buttonName, b, UIButton.ButtonActionType.OnClick);
        }
        /// <summary>
        /// Sends a button action with a reference to the UIButton that sent it and what type of action it is.
        /// </summary>
        public void SendButtonAction(UIButton uiButton, UIButton.ButtonActionType actionType)
        {
            OnButtonAction(uiButton.buttonName, uiButton, actionType);
        }
        /// <summary>
        /// Sends a button action with just a button name and what type of action it is. This method is used to simulate a button action since it does not have an UIButton reference.
        /// </summary>
        public void SendButtonAction(string buttonName, UIButton.ButtonActionType actionType)
        {
            OnButtonAction(buttonName, null, actionType);
        }
        /// <summary>
        /// Sends a button action with just a button name and what type of click it is. This method is used to simulate a button action since it does not have an UIButton reference.
        /// </summary>
        public void SendButtonAction(string buttonName, UIButton.ButtonClickType clickType)
        {
            OnButtonAction(buttonName, null, UIButton.GetButtonActionType(clickType));
        }
        #endregion

        #region Toggle Actions
        public void SendToggleAction(UIToggle uiToggle, UIToggle.ToggleActionType actionType)
        {
            OnToggleAction(uiToggle, actionType);
        }
        #endregion

        #region The 'Back' button
        /// <summary>
        /// Listener for the 'Back' button.
        /// </summary>
        void ListenForBackButton()
        {
            if(backButtonInputMode == ControllerInputMode.None) { return; }
            if(BackButtonDisabled) { return; }
            switch(backButtonInputMode)
            {
                case ControllerInputMode.KeyCode: if(Input.GetKeyDown(backButtonKeyCode) || (enableBackButtonAlternateInputs && Input.GetKeyDown(backButtonKeyCodeAlt))) { SendButtonAction(DUI.BACK_BUTTON_NAME, UIButton.ButtonActionType.OnClick); } break;
                case ControllerInputMode.VirtualButton: if(Input.GetButtonDown(backButtonVirtualButtonName) || (enableBackButtonAlternateInputs && Input.GetButtonDown(backButtonVirtualButtonNameAlt))) { SendButtonAction(DUI.BACK_BUTTON_NAME, UIButton.ButtonActionType.OnClick); } break;
            }
        }
        /// <summary>
        /// The 'back' button was pressed (or escape key)
        /// </summary>
        public static void BackButtonEvent()
        {
            if(Instance.BackButtonDisabled) //if the back button is disabled we do not continue
                return;

            //if (Instance.gamePaused) //if the game is paused, we unpause it
            //    TogglePause();

            NavigationPointerData navPointerData = UINavigation.GetLastItemFromNavigationHistory().Copy();

            if(navPointerData == null) { return; }
            count = navPointerData.show != null ? navPointerData.show.Count : 0;
            if(count > 0)
            {
                int localCount = count;
                for(int i = 0; i < localCount; i++)
                {
                    ShowUiElement(navPointerData.show[i].name, navPointerData.show[i].category);
                }
            }
            count = navPointerData.hide != null ? navPointerData.hide.Count : 0;
            if(count > 0)
            {
                int localCount = count;
                for(int i = 0; i < localCount; i++)
                {
                    HideUiElement(navPointerData.hide[i].name, navPointerData.hide[i].category);
                }
            }
        }
        /// <summary>
        /// Disables the 'Back' button functionality
        /// </summary>
        public static void DisableBackButton()
        {
            Instance.backButtonDisableLevel++; //if == 0 --> false (back button is not disabled) if > 0 --> true (back button is disabled)
        }
        /// <summary>
        /// Enables the 'Back' button functionality
        /// </summary>
        public static void EnableBackButton()
        {
            Instance.backButtonDisableLevel--; //if == 0 --> false (back button is not disabled) if > 0 --> true (back button is disabled)
            if(Instance.backButtonDisableLevel < 0) { Instance.backButtonDisableLevel = 0; } //Check so that the backButtonDisableLevel does not go below zero
        }
        /// <summary>
        /// Enables the 'Back' button functionality by resetting the additive bool to zero. backButtonDisableLevel = 0. Use this ONLY for special cases when something wrong happens and the back button is stuck in disabled mode.
        /// </summary>
        public static void EnableBackButtonByForce()
        {
            Instance.backButtonDisableLevel = 0;
        }
        /// <summary>
        /// Disables all the button clicks. This is triggered by the system when an UIElement started a transition (IN/OUT animations).
        /// </summary>
        public static void DisableButtonClicks()
        {
            Instance.buttonClicksDisableLevel++; //if == 0 --> false (button clicks are not disabled) if > 0 --> true (button clicks are disabled)
                                                 //Debug.Log("DisableButtonClicks | buttonClicksDisableLevel: " + Instance.buttonClicksDisableLevel);
        }
        /// <summary>
        /// Enables all the button clicks. This is triggered by the system when an UIElement finished a transition (IN/OUT animations).
        /// </summary>
        public static void EnableButtonClicks()
        {
            Instance.buttonClicksDisableLevel--; //if == 0 --> false (button clicks are not disabled) if > 0 --> true (button clicks are disabled)
            if(Instance.buttonClicksDisableLevel < 0) { Instance.buttonClicksDisableLevel = 0; } //Check so that the buttonClicksDisableLevel does not go below zero
                                                                                                 //Debug.Log("EnableButtonClicks | buttonClicksDisableLevel: " + Instance.buttonClicksDisableLevel);
        }
        /// <summary>
        /// Enables the button clicks by resetting the additive bool to zero. buttonClicksDisableLevel = 0. Use this ONLY for special cases when something unexpected happens and the button clicks are stuck in disabled mode.
        /// </summary>
        public static void EnableButtonClicksByForce()
        {
            Instance.buttonClicksDisableLevel = 0;
            //Debug.Log("EnableButtonClicksByForce | buttonClicksDisableLevel: " + buttonClicksDisableLevel);
        }
        #endregion

        #region UICanvas
        /// <summary>
        /// Returns a reference to an UICanvas that is considered and used as a 'MasterCanvas'. If no such canvas exists, one will get created automatically by default.
        /// </summary>
        /// <param name="createMasterCanvasIfNotFound">Should a 'MasterCanvas' be created if it is missing.</param>
        public static UICanvas GetMasterCanvas(bool createMasterCanvasIfNotFound = true)
        {
            if(masterCanvas != null) { return masterCanvas; } //MasterCanvas has already been found
            if(CanvasDatabase.Count == 0) //CanvasDatabase is empty -> check if there is an UICanvas named MasterCanvas, in the scene, that did not register (sanity check)
            {
                UICanvas[] searchResults = FindObjectsOfType<UICanvas>(); //Look for the MasterCanvas using find (inefficient, but necessary)
                len = searchResults.Length;
                if(searchResults != null && len > 0)
                {
                    for(int i = 0; i < len; i++)
                    {
                        if(searchResults[i].canvasName == UICanvas.MASTER_CANVAS_NAME)
                        {
                            masterCanvas = searchResults[i];
                            return masterCanvas;
                        }
                    }
                }
            }
            else if(CanvasDatabase.ContainsKey(UICanvas.MASTER_CANVAS_NAME)) //Check CanvasDatabase for the MasterCanvas
            {
                masterCanvas = CanvasDatabase[UICanvas.MASTER_CANVAS_NAME];
                return masterCanvas;
            }
            //MasterCanvas not found!
            if(!createMasterCanvasIfNotFound) { return null; }
            //Create a MasterCanvas
            masterCanvas = CreateCanvas(UICanvas.MASTER_CANVAS_NAME);
            return masterCanvas;
        }
        /// <summary>
        /// Retruns a reference to an UICanvas that has the given canvas name. It can also create the canvas you are searching for or just return the 'MasterCanvas' UICanvas.
        /// </summary>
        /// <param name="canvasName">The canvas name you are looking for.</param>
        /// <param name="createCanvasIfNotFound">Should the system create an UICanvas with the canvas name you are looking for?</param>
        /// <param name="returnMasterCanvasIfTargetCanvasNotFound">Should this method return a reference to the 'MasterCanvas' UICanvas if the canvas name you are looking for was not found?</param>
        public static UICanvas GetCanvas(string canvasName, bool createCanvasIfNotFound = false, bool returnMasterCanvasIfTargetCanvasNotFound = true)
        {
            if(string.IsNullOrEmpty(canvasName))
            {
                Debug.Log("[DoozyUI] You cannot get a Canvas without entering a name. The canvasName you provided, when calling UIManager.GetCanvas, was an empty string. Returned null.");
                return null;
            }
            if(CanvasDatabase.ContainsKey(canvasName)) { return CanvasDatabase[canvasName]; }
            if(Instance.debugUICanvases) { Debug.Log("[DoozyUI] There is no UICanvas with the '" + canvasName + "' canvasName in the CanvasDatabase. Returned the Master Canvas instead."); }
            if(createCanvasIfNotFound)
            {
                return CreateCanvas(canvasName);
            }
            if(returnMasterCanvasIfTargetCanvasNotFound)
            {
                return GetMasterCanvas();
            }
            return null;
        }
        /// <summary>
        /// Creates an UICanvas with the given canvas name and retuns the reference to it.
        /// </summary>
        /// <param name="canvasName">The canvas name for the new UICanvas.</param>
        /// <returns></returns>
        public static UICanvas CreateCanvas(string canvasName)
        {
            //Look for the EventSystem
            EventSystem es = GameObject.FindObjectOfType<EventSystem>();
            if(es != null)
            {
                es.transform.SetParent(null);
            }
            else
            {
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }
            if(string.IsNullOrEmpty(canvasName))
            {
                Debug.Log("[DoozyUI] You cannot create a new UICanvas without entering a name. The canvasName you provided, when calling UIManager.CreateCanvas, was an empty string. No canvas was created and this method returned null.");
                return null;
            }
            if(CanvasDatabase.ContainsKey(canvasName))
            {
                if(Instance.debugUICanvases) { Debug.Log("[DoozyUI] Cannot create a new UICanvas with the '" + canvasName + "' canvas name because another UICanvas with the same name already exists in the Canvas Database. Returned the existing one instead."); }
                return CanvasDatabase[canvasName];
            }
            GameObject go = new GameObject(canvasName, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            go.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            UICanvas canvas = go.AddComponent<UICanvas>();
            canvas.canvasName = canvasName;
            canvas.customCanvasName = true;
            return canvas;
        }
        #endregion

        #region UIElement
        /// <summary>
        /// Returns a List of all UIElements that have a given name and category. If no UIElement was found, it will return an empty list.
        /// </summary>
        public static List<UIElement> GetUiElements(string elementName, string elementCategory = DUI.UNCATEGORIZED_CATEGORY_NAME)
        {
            getUIElementsList.Clear();
            count = ElementDatabase != null ? ElementDatabase.Count : 0;
            if(count == 0)
            {
                return getUIElementsList;
            }

            if(ElementDatabase.ContainsKey(elementName))
            {
                count = ElementDatabase[elementName].Count;
                for(int i = 0; i < count; i++)
                {
                    if(ElementDatabase[elementName][i].elementCategory != elementCategory) { continue; }
                    getUIElementsList.Add(ElementDatabase[elementName][i]);
                }
            }

            return getUIElementsList;
        }
        /// <summary>
        /// Returns a List of all the UIElements that are visible on the screen. An UIElement is considered visible if isVisible = true.
        /// <para>If eDatabase is null or empty or if no UIElements are visible, it will return an empty list.</para>
        /// </summary>
        public static List<UIElement> GetVisibleUIElements()
        {
            getVisibleUIElements.Clear();
            count = ElementDatabase != null ? ElementDatabase.Count : 0;
            if(count == 0)
            {
                return getVisibleUIElements;
            }

            foreach(KeyValuePair<string, List<UIElement>> kvp in ElementDatabase)
            {
                count = kvp.Value != null ? kvp.Value.Count : 0;
                if(count == 0) { continue; }
                for(int i = 0; i < count; i++)
                {
                    if(!kvp.Value[i].isVisible) { continue; }
                    getVisibleUIElements.Add(kvp.Value[i]);
                }
            }
            return getVisibleUIElements;
        }

        /// <summary>
        /// Shows all the UIElements that have the given name and category.
        /// </summary>
        public static void ShowUiElement(string elementName, string elementCategory = DUI.UNCATEGORIZED_CATEGORY_NAME)
        {
            ShowUiElement(elementName, elementCategory, false);
        }
        /// <summary>
        /// Shows all the UIElements that have the given name and the DEFAULT CATEGORY name.
        /// </summary>
        /// <param name="instantAction">Should the animation play instantly (in zero seconds)</param>
        public static void ShowUiElement(string elementName, bool instantAction)
        {
            ShowUiElement(elementName, DUI.UNCATEGORIZED_CATEGORY_NAME, false);
        }
        /// <summary>
        /// Shows all the UIElements that have the given name and category.
        /// </summary>
        /// <param name="instantAction">Should the animation play instantly (in zero seconds)</param>
        public static void ShowUiElement(string elementName, string elementCategory, bool instantAction)
        {
            if(elementName.Equals(DUI.DEFAULT_ELEMENT_NAME)) { return; }
            Instance.ExecuteShow(elementName, elementCategory, instantAction);
        }

        /// <summary>
        /// This executes the SHOW actions and forces an Instance if required.
        /// <para/> This is needed in order to call Show at Start
        /// </summary>
        private void ExecuteShow(string elementName, string elementCategory, bool instantAction)
        {
            showElementsList = GetUiElements(elementName, elementCategory);
            count = showElementsList != null ? showElementsList.Count : 0;
            if(count > 0)
            {
                for(int i = 0; i < count; i++)
                {
                    if(showElementsList[i] != null) //this null check has been added to fix the slim chance that we registered a UIElement to the registry and it has been destroyed/deleted (thus now it's null)
                    {
                        showElementsList[i].gameObject.SetActive(true);

                        if(showElementsList[i].gameObject.activeInHierarchy)
                        {
                            if(!useOrientationManager)
                            {
                                if(Instance.debugUIElements) { Debug.Log("[UIManager] Showing the (" + elementCategory + ")(" + elementName + ") UIElement. This is " + (instantAction ? "" : "not ") + "an instant action."); }
                                showElementsList[i].Show(instantAction);
                            }
                            else
                            {
                                if(currentOrientation == Orientation.Unknown)
                                {
                                    OrientationManager.Instance.CheckDeviceOrientation();
                                }

                                if(currentOrientation == Orientation.Landscape)
                                {
                                    if(showElementsList[i].LANDSCAPE)
                                    {
                                        if(Instance.debugUIElements) { Debug.Log("[UIManager] Showing the (" + elementCategory + ")(" + elementName + ") UIElement. This is " + (instantAction ? "" : "not ") + "an instant action."); }
                                        showElementsList[i].Show(instantAction);
                                    }
                                    else if(showElementsList[i].PORTRAIT)
                                    {
                                        showElementsList[i].isVisible = true;
                                        showElementsList[i].Hide(true);
                                    }
                                    else
                                    {
                                        showElementsList[i].isVisible = true;
                                        showElementsList[i].Hide(true);
                                    }
                                }
                                else if(currentOrientation == Orientation.Portrait)
                                {
                                    if(showElementsList[i].PORTRAIT)
                                    {
                                        if(Instance.debugUIElements) { Debug.Log("[UIManager] Showing the (" + elementCategory + ")(" + elementName + ") UIElement. This is " + (instantAction ? "" : "not ") + "an instant action."); }
                                        showElementsList[i].Show(instantAction);
                                    }
                                    else if(showElementsList[i].LANDSCAPE)
                                    {
                                        showElementsList[i].isVisible = true;
                                        showElementsList[i].Hide(true);
                                    }
                                    else
                                    {
                                        showElementsList[i].isVisible = true;
                                        showElementsList[i].Hide(true);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            showEffectsList = GetUiEffects(elementName, elementCategory);
            count = showEffectsList != null ? showEffectsList.Count : 0;
            if(count > 0)
            {
                for(int i = 0; i < count; i++)
                {
                    showEffectsList[i].gameObject.SetActive(true);
                    if(showEffectsList[i].gameObject.activeInHierarchy)
                    {
                        if(useOrientationManager == false)
                        {
                            showEffectsList[i].Show();
                        }
                        else
                        {
                            if(currentOrientation == Orientation.Landscape)
                            {
                                if(showEffectsList[i].targetUIElement.LANDSCAPE)
                                {
                                    showEffectsList[i].Show();
                                }
                                else if(showEffectsList[i].targetUIElement.PORTRAIT)
                                {
                                    showEffectsList[i].isVisible = true;
                                    showEffectsList[i].Hide();
                                }
                                else
                                {
                                    showEffectsList[i].isVisible = true;
                                    showEffectsList[i].Hide();
                                }
                            }
                            else if(currentOrientation == Orientation.Portrait)
                            {
                                if(showEffectsList[i].targetUIElement.PORTRAIT)
                                {
                                    showEffectsList[i].Show();
                                }
                                else if(showEffectsList[i].targetUIElement.LANDSCAPE)
                                {
                                    showEffectsList[i].isVisible = true;
                                    showEffectsList[i].Hide();
                                }
                                else
                                {
                                    showEffectsList[i].isVisible = true;
                                    showEffectsList[i].Hide();
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Hides all the UIElements that have the given name and category.
        /// </summary>
        /// <param name="instantAction">Should the animation play instantly (in zero seconds)</param>
        public static void HideUiElement(string elementName, string elementCategory)
        {
            HideUiElement(elementName, elementCategory, false);
        }
        /// <summary>
        /// Hides all the UIElements that have the given name and the DEFAULT CATEGORY name.
        /// </summary>
        /// <param name="instantAction">Should the animation play instantly (in zero seconds)</param>
        public static void HideUiElement(string elementName, bool instantAction = false)
        {
            HideUiElement(elementName, DUI.UNCATEGORIZED_CATEGORY_NAME, instantAction);
        }
        /// <summary>
        /// Hides all the UIElements that have the given name and category.
        /// </summary>
        /// <param name="instantAction">Should the animation play instantly (in zero seconds)</param>
        public static void HideUiElement(string elementName, string elementCategory, bool instantAction)
        {
            if(elementName.Equals(DUI.DEFAULT_ELEMENT_NAME)) { return; }
            Instance.ExecuteHide(elementName, elementCategory, instantAction);
        }

        /// <summary>
        /// This IEnumerator executes the Hide actions and forces an Instance if required.
        /// </summary>
        private void ExecuteHide(string elementName, string elementCategory, bool instantAction)
        {
            hideElementsList = GetUiElements(elementName, elementCategory);
            count = hideElementsList != null ? hideElementsList.Count : 0;
            if(count > 0)
            {
                for(int i = 0; i < count; i++)
                {
                    if(hideElementsList[i] != null) //this null check has been added to fix the slim chance that we registered a UIElement to the registry and it has been destroyed/deleted (thus now it's null)
                    {
                        if(hideElementsList[i].gameObject.activeInHierarchy)
                        {
                            if(Instance.debugUIElements) { Debug.Log("[UIManager] Hiding the (" + elementCategory + ")(" + elementName + ") UIElement. This is " + (instantAction ? "" : "not ") + "an instant action."); }
                            hideElementsList[i].Hide(instantAction);
                        }
                    }
                }
            }

            hideEffectsList = GetUiEffects(elementName, elementCategory);
            count = hideEffectsList != null ? hideEffectsList.Count : 0;
            if(count > 0)
            {
                for(int i = 0; i < count; i++)
                {
                    if(hideEffectsList[i].gameObject.activeInHierarchy)
                    {
                        hideEffectsList[i].Hide();
                    }
                }
            }
        }
        #endregion

        #region UIEffect
        /// <summary>
        /// Returns a List of all UIEffects that are linked to an UIElement with a given name and category. If no UIEffect was found, it will return an empty list.
        /// </summary>
        public static List<UIEffect> GetUiEffects(string elementName, string elementCategory = DUI.UNCATEGORIZED_CATEGORY_NAME)
        {
            getUIEffectsList.Clear();
            count = EffectDatabase != null ? EffectDatabase.Count : 0;
            if(count == 0)
            {
                return getUIEffectsList;
            }

            if(EffectDatabase.ContainsKey(elementName))
            {
                count = EffectDatabase[elementName].Count;
                for(int i = 0; i < count; i++)
                {
                    if(EffectDatabase[elementName][i].targetUIElement == null) { continue; }
                    if(EffectDatabase[elementName][i].targetUIElement.elementCategory != elementCategory) { continue; }
                    getUIEffectsList.Add(EffectDatabase[elementName][i]);
                }
            }

            return getUIEffectsList;
        }
        #endregion

        #region UITrigger
        /// <summary>
        /// Returns a list of all the UITriggers that are linked to the given triggerValue and of the given triggerType.
        /// </summary>
        /// <param name="triggerValue">This can be either a game event or a button name or the special DUI.DISPATCH_ALL value.</param>
        /// <param name="triggerType">Depending on the triggerType, this method will search in a different registry.</param>
        public List<UITrigger> GetUiTriggers(string triggerValue, DUI.EventType triggerType)
        {
            getUITriggersList.Clear();
            switch(triggerType)
            {
                case DUI.EventType.GameEvent:
                    if(GameEventsTriggerDatabase == null || GameEventsTriggerDatabase.Count == 0) { return null; }
                    if(GameEventsTriggerDatabase.ContainsKey(triggerValue)) { getUITriggersList.AddRange(GameEventsTriggerDatabase[triggerValue]); }
                    if(GameEventsTriggerDatabase.ContainsKey(DUI.DISPATCH_ALL)) { getUITriggersList.AddRange(GameEventsTriggerDatabase[DUI.DISPATCH_ALL]); }
                    break;

                case DUI.EventType.ButtonClick:
                    if(ButtonClicksTriggerDatabase == null || ButtonClicksTriggerDatabase.Count == 0) { return null; }
                    if(ButtonClicksTriggerDatabase.ContainsKey(triggerValue)) { getUITriggersList.AddRange(ButtonClicksTriggerDatabase[triggerValue]); }
                    if(ButtonClicksTriggerDatabase.ContainsKey(DUI.DISPATCH_ALL)) { getUITriggersList.AddRange(ButtonClicksTriggerDatabase[DUI.DISPATCH_ALL]); }
                    break;

                case DUI.EventType.ButtonDoubleClick:
                    if(ButtonDoubleClicksTriggerDatabase == null || ButtonDoubleClicksTriggerDatabase.Count == 0) { return null; }
                    if(ButtonDoubleClicksTriggerDatabase.ContainsKey(triggerValue)) { getUITriggersList.AddRange(ButtonDoubleClicksTriggerDatabase[triggerValue]); }
                    if(ButtonDoubleClicksTriggerDatabase.ContainsKey(DUI.DISPATCH_ALL)) { getUITriggersList.AddRange(ButtonDoubleClicksTriggerDatabase[DUI.DISPATCH_ALL]); }
                    break;

                case DUI.EventType.ButtonLongClick:
                    if(ButtonLongClicksTriggerRegistry == null || ButtonLongClicksTriggerRegistry.Count == 0) { return null; }
                    if(ButtonLongClicksTriggerRegistry.ContainsKey(triggerValue)) { getUITriggersList.AddRange(ButtonLongClicksTriggerRegistry[triggerValue]); }
                    if(ButtonLongClicksTriggerRegistry.ContainsKey(DUI.DISPATCH_ALL)) { getUITriggersList.AddRange(ButtonLongClicksTriggerRegistry[DUI.DISPATCH_ALL]); }
                    break;

                default:
                    return null;
            }

            return getUITriggersList;
        }
        /// <summary>
        /// Triggers all the UITriggers that are listening for the given triggerValue and are of the given triggerType.
        /// </summary>
        public void TriggerTheTriggers(string triggerValue, DUI.EventType triggerType)
        {
            StartCoroutine(ExecuteTriggerTheTriggersInTheNextFrame(triggerValue, triggerType));
        }

        IEnumerator ExecuteTriggerTheTriggersInTheNextFrame(string triggerValue, DUI.EventType triggerType)
        {
            yield return null;
            if(triggerTheTriggersList == null)
            {
                triggerTheTriggersList = new List<UITrigger>();
            }
            else
            {
                triggerTheTriggersList.Clear();
            }
            triggerTheTriggersList = GetUiTriggers(triggerValue, triggerType);
            count = triggerTheTriggersList != null ? triggerTheTriggersList.Count : 0;
            if(count > 0)
            {
                for(int i = 0; i < count; i++)
                {
                    if(triggerTheTriggersList[i] == null) { continue; }
                    triggerTheTriggersList[i].TriggerTheTrigger(triggerValue);
                }
            }
        }
        #endregion

        #region UINotification
        /// <summary>
        /// Every notification that needs to enter the Notification Queue will be added to the notificatioQueue list as the last item.
        /// </summary>
        public void RegisterToNotificationQueue(UINotification.NotificationData nData)
        {
            NotificationManager.RegisterToNotificationQueue(nData);
        }
        /// <summary>
        /// Unregisteres a notification, by removing the notification data that started it.
        /// </summary>
        public void UnregisterFromNotificationQueue(UINotification.NotificationData nData)
        {
            NotificationManager.UnregisterFromNotificationQueue(nData);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        /// <param name="_buttonTexts">The text on the buttons (example: 'OK', 'Cancel', 'Yes', 'No' and so on)</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, string _message, Sprite _icon, string[] _buttonNames, string[] _buttonTexts, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefabName, _lifetime, _addToNotificationQueue, _title, _message, _icon, _buttonNames, _buttonTexts, _buttonCallback, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        /// <param name="_buttonTexts">The text on the buttons (example: 'OK', 'Cancel', 'Yes', 'No' and so on)</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, string _message, Sprite _icon, string[] _buttonNames, string[] _buttonTexts, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefab, _lifetime, _addToNotificationQueue, _title, _message, _icon, _buttonNames, _buttonTexts, _buttonCallback, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefabName, _lifetime, _addToNotificationQueue, _buttonCallback, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefab, _lifetime, _addToNotificationQueue, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefabName, _lifetime, _addToNotificationQueue, _title, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefab, _lifetime, _addToNotificationQueue, _title, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, string _message, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefabName, _lifetime, _addToNotificationQueue, _title, _message, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, string _message, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefab, _lifetime, _addToNotificationQueue, _title, _message, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, string _message, Sprite _icon, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefabName, _lifetime, _addToNotificationQueue, _title, _message, _icon, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, string _message, Sprite _icon, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefab, _lifetime, _addToNotificationQueue, _title, _message, _icon, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, string _message, Sprite _icon, string[] _buttonNames, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefabName, _lifetime, _addToNotificationQueue, _title, _message, _icon, _buttonNames, _buttonCallback, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, string _message, Sprite _icon, string[] _buttonNames, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefab, _lifetime, _addToNotificationQueue, _title, _message, _icon, _buttonNames, _buttonCallback, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, Sprite _icon, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefabName, _lifetime, _addToNotificationQueue, _icon, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, Sprite _icon, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefab, _lifetime, _addToNotificationQueue, _icon, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, Sprite _icon, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefabName, _lifetime, _addToNotificationQueue, _title, _icon, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, Sprite _icon, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefab, _lifetime, _addToNotificationQueue, _title, _icon, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string[] _buttonNames, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefabName, _lifetime, _addToNotificationQueue, _buttonNames, _buttonCallback, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string[] _buttonNames, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefab, _lifetime, _addToNotificationQueue, _buttonNames, _buttonCallback, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        /// <param name="_buttonTexts">The text on the buttons (example: 'OK', 'Cancel', 'Yes', 'No' and so on)</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string[] _buttonNames, string[] _buttonTexts, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefabName, _lifetime, _addToNotificationQueue, _buttonNames, _buttonTexts, _buttonCallback, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        /// <param name="_buttonTexts">The text on the buttons (example: 'OK', 'Cancel', 'Yes', 'No' and so on)</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string[] _buttonNames, string[] _buttonTexts, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefab, _lifetime, _addToNotificationQueue, _buttonNames, _buttonTexts, _buttonCallback, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, string[] _buttonNames, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefabName, _lifetime, _addToNotificationQueue, _title, _buttonNames, _buttonCallback, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, string[] _buttonNames, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefab, _lifetime, _addToNotificationQueue, _title, _buttonNames, _buttonCallback, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        /// <param name="_buttonTexts">The text on the buttons (example: 'OK', 'Cancel', 'Yes', 'No' and so on)</param>
        public static UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, string[] _buttonNames, string[] _buttonTexts, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefabName, _lifetime, _addToNotificationQueue, _title, _buttonNames, _buttonTexts, _buttonCallback, _hideCallback);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        /// <param name="_buttonTexts">The text on the buttons (example: 'OK', 'Cancel', 'Yes', 'No' and so on)</param>
        public static UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, string[] _buttonNames, string[] _buttonTexts, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return NotificationManager.ShowNotification(_prefab, _lifetime, _addToNotificationQueue, _title, _buttonNames, _buttonTexts, _buttonCallback, _hideCallback);
        }
        #endregion

        #region PlaymakerEventDispatcher
#if dUI_PlayMaker
        /// <summary>
        /// This method is obsolete, please use SendEventToPlaymaker instead.
        /// </summary>
        [System.Obsolete]
        public static void DispatchEventToPlaymakerEventDispatchers(string eventValue, DUI.EventType eventType)
        {
            SendEventToPlaymaker(eventValue, eventType);
        }

        /// <summary>
        /// Sends an event that can be either a Game Event or Button Click to all the registered Playmaker Event Dispatchers.
        /// </summary>
        /// <param name="eventValue">The event you want to send</param>
        /// <param name="eventType">For now, use only GameEvent or ButtonClick. The rest do nothing.</param>
        public static void SendEventToPlaymaker(string eventValue, DUI.EventType eventType = DUI.EventType.GameEvent)
        {
            count = PlaymakerEventDispatcherDatabase != null ? PlaymakerEventDispatcherDatabase.Count : 0;
            if (count == 0) { return; }

            for (int i = 0; i < count; i++)
            {
                PlaymakerEventDispatcherDatabase[i].DispatchEvent(eventValue, eventType);
            }
        }
#endif
        #endregion

        #region OrientationManager
        /// <summary>
        /// Updates the current orientation to the new given one.
        /// </summary>
        public void ChangeOrientation(Orientation newOrientation)
        {
            currentOrientation = newOrientation;
            SendGameEvent("DeviceOrientation_" + currentOrientation);
            List<UIElement> visibleUIElements = GetVisibleUIElements(); //we get the list of all the visible UIElement
            count = visibleUIElements != null ? visibleUIElements.Count : 0;
            if(count > 0)
            {
                for(int i = 0; i < count; i++)
                {
                    ShowUiElement(visibleUIElements[i].elementName, visibleUIElements[i].elementCategory, false); //we show instantly all the UIElements with this element name (under the new orientation)
                }
            }
        }
        #endregion

        #region Game Management
        /// <summary>
        /// Pauses or Unpauses the application
        /// </summary>
        public static void TogglePause()
        {
            if(Instance.gamePaused)
            {
                Time.timeScale = Instance.currentGameTimeScale;
                Instance.gamePaused = false;
            }
            else
            {
                Instance.currentGameTimeScale = Time.timeScale;
                Time.timeScale = 0f;
                Instance.gamePaused = true;
            }
        }
        /// <summary>
        /// Exits play mode (if in editor) or quits the application if in build mode
        /// </summary>
        public static void ApplicationQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        #endregion

        #region Sound and Music 
        /// <summary>
        /// Checks the soundState when the game starts in the PlayerPrefs
        /// </summary>
        public static void SoundCheck()
        {
            int soundState = PlayerPrefs.GetInt("soundState", 1); //We check if the sound is ON (1) or OFF (0). By default we assume it's 1.

            if(soundState == 1)
            {
                //Sound ON
                Instance.isSoundOn = true;             //We set the static variable soundON = true
                ShowUiElement("SoundON", false);   //We show the SoundON UIElement
                HideUiElement("SoundOFF", true);  //We hide the SoundOFF UIElement
            }
            else
            {
                //Sound OFF
                Instance.isSoundOn = false;             //We set the static variable soundON = false
                ShowUiElement("SoundOFF", false);   //We show the SoundOFF UIElement
                HideUiElement("SoundON", true);    //We hide the SoundON UIElement
            }
            SendGameEvent("UpdateSoundSettings");
        }
        /// <summary>
        /// Checks the musicState when the game starts in the PlayerPrefs
        /// </summary>
        public static void MusicCheck()
        {
            int musicState = PlayerPrefs.GetInt("musicState", 1); //We check if the music is ON (1) or OFF (0). By default we assume it's 1.

            if(musicState == 1)
            {
                //Music ON
                Instance.isMusicOn = true;             //We set the static variable isMusicOn = true
                ShowUiElement("MusicON", false);   //We show the MusicON UIElement
                HideUiElement("MusicOFF", true);  //We hide the MusicOFF UIElement
            }
            else
            {
                //Music OFF
                Instance.isMusicOn = false;             //We set the static variable isMusicOn = false
                ShowUiElement("MusicOFF", false);   //We show the SoundOFF UIElement
                HideUiElement("MusicON", true);    //We hide the MusicOFF UIElement
            }
            SendGameEvent("UpdateSoundSettings");
        }
        /// <summary>
        /// Toggles the soundState and saves it to the PlayerPrefs
        /// </summary>
        public static void ToggleSound()
        {
            Instance.isSoundOn = !Instance.isSoundOn;

            int soundState = -1;

            if(Instance.isSoundOn == true)
            {
                //Sound ON
                soundState = 1;             //Value if the sound is on
                ShowUiElement("SoundON", false);   //We show the SoundON UIElement
                HideUiElement("SoundOFF", false);  //We hide the SoundOFF UIElement
            }
            else
            {
                soundState = 0;              //Value if the sound is off
                ShowUiElement("SoundOFF", false);   //We show the SoundOFF UIElement
                HideUiElement("SoundON", false);    //We hide the SoundON UIElement
            }

            PlayerPrefs.SetInt("soundState", soundState); //We set the new value in the PlayerPrefs
            PlayerPrefs.Save(); //We save the value

            SendGameEvent("UpdateSoundSettings");
        }
        /// <summary>
        /// Toggles the musicState and saves it to the PlayerPrefs
        /// </summary>
        public static void ToggleMusic()
        {
            Instance.isMusicOn = !Instance.isMusicOn;

            int musicState = -1;

            if(Instance.isMusicOn == true)
            {
                //MUSIC ON
                musicState = 1;             //Value if the music is on
                ShowUiElement("MusicON", false);   //We show the SoundON UIElement
                HideUiElement("MusicOFF", false);  //We hide the MusicON UIElement
            }
            else
            {
                //MUSIC OFF
                musicState = 0;              //Value if the music is off
                ShowUiElement("MusicOFF", false);   //We show the MusicOFF UIElement
                HideUiElement("MusicON", false);    //We hide the MusicON UIElement
            }

            PlayerPrefs.SetInt("musicState", musicState); //We set the new value in the PlayerPrefs
            PlayerPrefs.Save(); //We save the value

            SendGameEvent("UpdateSoundSettings");
        }
        #endregion

        #region PlaySound
        /// <summary>
        /// Plays the given audio clip, through Soundy. You can also use Soundy.PlaySound...
        /// </summary>
        public static void PlaySound(AudioClip aClip) { Soundy.PlaySound(aClip); }
        /// <summary>
        /// Plays the given audio clip at the given volume level, through Soundy. You can also use Soundy.PlaySound...
        /// </summary>
        public static void PlaySound(AudioClip aClip, float volume) { Soundy.PlaySound(aClip, volume); }
        /// <summary>
        /// Plays the given audio clip at the given volume and pitch levels, through Soundy. You can also use Soundy.PlaySound...
        /// </summary>
        public static void PlaySound(AudioClip aClip, float volume, float pitch) { Soundy.PlaySound(aClip, volume, pitch); }
        /// <summary>
        /// Plays the given sound name, through Soundy. You can also use Soundy.PlaySound...
        /// <para>Note: If support for MasterAudio is enabled it will play the sound name from the MasterAudio sounds database.</para>
        /// </summary>
        public static void PlaySound(string soundName) { if(soundName != DUI.DEFAULT_SOUND_NAME && !string.IsNullOrEmpty(soundName.Trim())) { Soundy.PlaySound(soundName); } }
        /// <summary>
        /// Plays the given sound name, through Soundy. You can also use Soundy.PlaySound...
        /// <para>Note: If support for MasterAudio is enabled it will play the sound name from the MasterAudio sounds database.</para>
        /// </summary>
        public static void PlaySound(string soundName, float volume) { if(soundName != DUI.DEFAULT_SOUND_NAME && !string.IsNullOrEmpty(soundName.Trim())) { Soundy.PlaySound(soundName, volume); } }
        /// <summary>
        /// Plays the given sound name, through Soundy. You can also use Soundy.PlaySound...
        /// <para>Note: If support for MasterAudio is enabled it will play the sound name from the MasterAudio sounds database.</para>
        /// </summary>
        public static void PlaySound(string soundName, float volume, float pitch) { if(soundName != DUI.DEFAULT_SOUND_NAME && !string.IsNullOrEmpty(soundName.Trim())) { Soundy.PlaySound(soundName, volume, pitch); } }
        /// <summary>
        /// Plays the given sound name by searching the Resources folder for it, through Soundy. You can also use Soundy.PlaySound...
        /// </summary>
        public static void PlaySoundFromResources(string soundName) { if(soundName != DUI.DEFAULT_SOUND_NAME && !string.IsNullOrEmpty(soundName.Trim())) { Soundy.PlaySoundFromResources(soundName); } }
        /// <summary>
        /// Plays the given sound name by searching the Resources folder for it, through Soundy. You can also use Soundy.PlaySound...
        /// </summary>
        public static void PlaySoundFromResources(string soundName, float volume) { if(soundName != DUI.DEFAULT_SOUND_NAME && !string.IsNullOrEmpty(soundName.Trim())) { Soundy.PlaySoundFromResources(soundName, volume); } }
        /// <summary>
        /// Plays the given sound name by searching the Resources folder for it, through Soundy. You can also use Soundy.PlaySound...
        /// </summary>
        public static void PlaySoundFromResources(string soundName, float volume, float pitch) { if(soundName != DUI.DEFAULT_SOUND_NAME && !string.IsNullOrEmpty(soundName.Trim())) { Soundy.PlaySoundFromResources(soundName, volume, pitch); } }
        #endregion

        #region Miscellaneous
        /// <summary>
        /// Updates the sorting layer for all the canvases on and under the target gameObject
        /// </summary>
        public static void UpdateCanvasSortingLayerName(GameObject targetObject, string sortingLayerName)
        {
            Canvas[] canvas = targetObject.GetComponentsInChildren<Canvas>();
            foreach(Canvas c in canvas)
            {
                c.overrideSorting = true;
                c.sortingLayerName = sortingLayerName;
            }
        }
        /// <summary>
        /// Updates all the sorting layer for all the renderers on and under the target gameObject
        /// </summary>
        public static void UpdateRendererSortingLayerName(GameObject targetObject, string sortingLayerName)
        {
            Renderer[] renderers = targetObject.GetComponentsInChildren<Renderer>();
            foreach(Renderer r in renderers)
            {
                r.sortingLayerName = sortingLayerName;
            }
        }

        internal static void ShowNotification(string nName, float nLifetime, object nAddToQueue)
        {
            throw new NotImplementedException();
        }

        public void GCCollect(object obj, GCCollectionMode collectMode = GCCollectionMode.Default)
        {
            StartCoroutine(GCCollectInTheNextFrame(obj, collectMode));
        }
        IEnumerator GCCollectInTheNextFrame(object obj, GCCollectionMode collectMode)
        {
            yield return null;
            GC.Collect(GC.GetGeneration(obj), collectMode);
        }
        #endregion
    }
}
