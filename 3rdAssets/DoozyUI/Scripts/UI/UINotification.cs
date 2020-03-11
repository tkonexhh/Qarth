// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEngine.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DoozyUI
{
    [AddComponentMenu(DUI.COMPONENT_MENU_UINOTIFICATION, DUI.MENU_PRIORITY_UINOTIFICATION)]
    [DisallowMultipleComponent]
    public class UINotification : MonoBehaviour
    {
        #region Context Menu
#if UNITY_EDITOR
        [UnityEditor.MenuItem(DUI.GAMEOBJECT_MENU_UINOTIFICATION, false, DUI.MENU_PRIORITY_UINOTIFICATION)]
        static void CreateNotification(UnityEditor.MenuCommand menuCommand)
        {
            UICanvas targetCanvas = null;
            GameObject selectedGO = menuCommand.context as GameObject;
            if(selectedGO != null) //check that a gameObject is selected
            {
                targetCanvas = selectedGO.GetComponent<UICanvas>(); //check if the selected gameObject is an UICanvas, otherwise get the root and check
                if(targetCanvas == null)
                {
                    targetCanvas = selectedGO.transform.root.GetComponent<UICanvas>(); //check if there is an UICanvas on the root of the selected gameOhject
                }
            }
            if(targetCanvas == null) //because we did not find any UICanvas on the selected gameObject (or on it's root transform), we get the MasterCanvas; if the MasterCanvas does not exist, it will be created automatically by the system
            {
                targetCanvas = UIManager.GetMasterCanvas();
            }

            GameObject notification = new GameObject("UINotification", typeof(RectTransform), typeof(UINotification));
            UnityEditor.GameObjectUtility.SetParentAndAlign(notification, targetCanvas.gameObject);
            notification.GetComponent<UINotification>().Reset();

            UnityEditor.Undo.RegisterCreatedObjectUndo(notification, "Create " + notification.name);

            GameObject overlay = new GameObject("UIE - Background Overlay", typeof(RectTransform), typeof(UIElement));
            UnityEditor.GameObjectUtility.SetParentAndAlign(overlay, notification);
            overlay.GetComponent<UIElement>().Reset();
            overlay.GetComponent<UIElement>().startHidden = true;
            overlay.GetComponent<UIElement>().inAnimationsPresetCategoryName = "Fade";
            overlay.GetComponent<UIElement>().inAnimationsPresetName = "InFast";
            overlay.GetComponent<UIElement>().loadInAnimationsPresetAtRuntime = true;
            overlay.GetComponent<UIElement>().outAnimationsPresetCategoryName = "Fade";
            overlay.GetComponent<UIElement>().outAnimationsPresetName = "OutFast";
            overlay.GetComponent<UIElement>().loadOutAnimationsPresetAtRuntime = true;
            overlay.GetComponent<UIElement>().Canvas.sortingOrder = 0;
            GameObject overlayBackground = new GameObject("Background", typeof(RectTransform), typeof(Image));
            UnityEditor.GameObjectUtility.SetParentAndAlign(overlayBackground, overlay);
            overlayBackground.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            overlayBackground.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            overlayBackground.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            overlayBackground.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            overlayBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            overlayBackground.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            overlayBackground.GetComponent<Image>().color = ColorExtensions.ColorFrom256(11, 50, 74, 200);

            GameObject notificationContainer = new GameObject("UIE - Notification Container", typeof(RectTransform), typeof(UIElement), typeof(Button));
            UnityEditor.GameObjectUtility.SetParentAndAlign(notificationContainer, notification);
            notificationContainer.GetComponent<Button>().transition = Selectable.Transition.None;
            notificationContainer.GetComponent<UIElement>().Reset();
            notificationContainer.GetComponent<UIElement>().startHidden = true;
            notificationContainer.GetComponent<UIElement>().inAnimationsPresetCategoryName = "Basic";
            notificationContainer.GetComponent<UIElement>().inAnimationsPresetName = "Punch";
            notificationContainer.GetComponent<UIElement>().loadInAnimationsPresetAtRuntime = true;
            notificationContainer.GetComponent<UIElement>().outAnimationsPresetCategoryName = "Basic";
            notificationContainer.GetComponent<UIElement>().outAnimationsPresetName = "Punch";
            notificationContainer.GetComponent<UIElement>().loadOutAnimationsPresetAtRuntime = true;
            notificationContainer.GetComponent<UIElement>().Canvas.sortingOrder = 1;
            notificationContainer.GetComponent<RectTransform>().localScale = Vector3.one;
            notificationContainer.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            notificationContainer.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            notificationContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            notificationContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 200f);
            notificationContainer.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            GameObject notificationContainerBackground = new GameObject("Background", typeof(RectTransform), typeof(Image));
            UnityEditor.GameObjectUtility.SetParentAndAlign(notificationContainerBackground, notificationContainer);
            notificationContainerBackground.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            notificationContainerBackground.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            notificationContainerBackground.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            notificationContainerBackground.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            notificationContainerBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            notificationContainerBackground.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            notificationContainerBackground.GetComponent<Image>().sprite = DUI.Background;
            notificationContainerBackground.GetComponent<Image>().type = Image.Type.Sliced;
            notificationContainerBackground.GetComponent<Image>().fillCenter = true;
            notificationContainerBackground.GetComponent<Image>().color = ColorExtensions.ColorFrom256(11, 50, 74, 255);

            GameObject notificationTitleContainer = new GameObject("Notification Title Container", typeof(RectTransform), typeof(Image));
            UnityEditor.GameObjectUtility.SetParentAndAlign(notificationTitleContainer, notificationContainer);
            notificationTitleContainer.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            notificationTitleContainer.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            notificationTitleContainer.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            notificationTitleContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -16);
            notificationTitleContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(16, 32);
            notificationTitleContainer.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
            notificationTitleContainer.GetComponent<Image>().sprite = DUI.Background;
            notificationTitleContainer.GetComponent<Image>().type = Image.Type.Sliced;
            notificationTitleContainer.GetComponent<Image>().fillCenter = true;
            notificationTitleContainer.GetComponent<Image>().color = ColorExtensions.ColorFrom256(31, 136, 201, 255);

#if dUI_TextMeshPro
            GameObject notificationTitle = new GameObject("Notification Title TMPro", typeof(RectTransform), typeof(TMPro.TextMeshProUGUI));
            UnityEditor.GameObjectUtility.SetParentAndAlign(notificationTitle, notificationTitleContainer);
            notificationTitle.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            notificationTitle.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            notificationTitle.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            notificationTitle.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            notificationTitle.GetComponent<RectTransform>().sizeDelta = new Vector2(-32, -8);
            notificationTitle.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            notificationTitle.GetComponent<TMPro.TextMeshProUGUI>().color = ColorExtensions.ColorFrom256(11, 50, 74, 255);
            notificationTitle.GetComponent<TMPro.TextMeshProUGUI>().fontSize = 18;
            notificationTitle.GetComponent<TMPro.TextMeshProUGUI>().fontStyle = TMPro.FontStyles.Bold;
            notificationTitle.GetComponent<TMPro.TextMeshProUGUI>().alignment = TMPro.TextAlignmentOptions.Center;
            notificationTitle.GetComponent<TMPro.TextMeshProUGUI>().text = "notification title";
#else
            GameObject notificationTitle = new GameObject("Notification Title", typeof(RectTransform), typeof(Text));
            UnityEditor.GameObjectUtility.SetParentAndAlign(notificationTitle, notificationTitleContainer);
            notificationTitle.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            notificationTitle.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            notificationTitle.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            notificationTitle.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            notificationTitle.GetComponent<RectTransform>().sizeDelta = new Vector2(-32, -4);
            notificationTitle.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            notificationTitle.GetComponent<Text>().color = ColorExtensions.ColorFrom256(11, 50, 74, 255);
            notificationTitle.GetComponent<Text>().fontSize = 14;
            notificationTitle.GetComponent<Text>().fontStyle = FontStyle.Bold;
            notificationTitle.GetComponent<Text>().resizeTextForBestFit = true;
            notificationTitle.GetComponent<Text>().resizeTextMinSize = 14;
            notificationTitle.GetComponent<Text>().resizeTextMaxSize = 40;
            notificationTitle.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            notificationTitle.GetComponent<Text>().alignByGeometry = true;
            notificationTitle.GetComponent<Text>().supportRichText = true;
            notificationTitle.GetComponent<Text>().text = "notification title";
#endif


#if dUI_TextMeshPro
            GameObject notificationMessage = new GameObject("Notification Message TMPro", typeof(RectTransform), typeof(TMPro.TextMeshProUGUI));
            UnityEditor.GameObjectUtility.SetParentAndAlign(notificationMessage, notificationContainer);
            notificationMessage.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            notificationMessage.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            notificationMessage.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            notificationMessage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -24);
            notificationMessage.GetComponent<RectTransform>().sizeDelta = new Vector2(-32, -80);
            notificationMessage.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            notificationMessage.GetComponent<TMPro.TextMeshProUGUI>().color = ColorExtensions.ColorFrom256(31, 136, 201, 255);
            notificationMessage.GetComponent<TMPro.TextMeshProUGUI>().fontSize = 14;
            notificationMessage.GetComponent<TMPro.TextMeshProUGUI>().fontStyle = TMPro.FontStyles.Italic;
            notificationMessage.GetComponent<TMPro.TextMeshProUGUI>().alignment = TMPro.TextAlignmentOptions.Center;
            notificationMessage.GetComponent<TMPro.TextMeshProUGUI>().text = "notification message example";
#else
            GameObject notificationMessage = new GameObject("Notification Message", typeof(RectTransform), typeof(Text));
            UnityEditor.GameObjectUtility.SetParentAndAlign(notificationMessage, notificationContainer);
            notificationMessage.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            notificationMessage.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            notificationMessage.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            notificationMessage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -24);
            notificationMessage.GetComponent<RectTransform>().sizeDelta = new Vector2(-48, -80);
            notificationMessage.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            notificationMessage.GetComponent<Text>().color = ColorExtensions.ColorFrom256(31, 136, 201, 255);
            notificationMessage.GetComponent<Text>().fontSize = 14;
            notificationMessage.GetComponent<Text>().fontStyle = FontStyle.Italic;
            notificationMessage.GetComponent<Text>().resizeTextForBestFit = true;
            notificationMessage.GetComponent<Text>().resizeTextMinSize = 12;
            notificationMessage.GetComponent<Text>().resizeTextMaxSize = 18;
            notificationMessage.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            notificationMessage.GetComponent<Text>().alignByGeometry = true;
            notificationMessage.GetComponent<Text>().supportRichText = true;
            notificationMessage.GetComponent<Text>().text = "notification message example";
#endif

            notification.GetComponent<UINotification>().notificationContainer = notificationContainer.GetComponent<UIElement>();
            notification.GetComponent<UINotification>().overlay = overlay.GetComponent<UIElement>();
            notification.GetComponent<UINotification>().title = notificationTitle;
            notification.GetComponent<UINotification>().message = notificationMessage;
            notification.GetComponent<UINotification>().closeButton = notificationContainer.GetComponent<Button>();

            UnityEditor.Selection.activeObject = notification;
        }
#endif
        #endregion

        /// <summary>
        /// Helper class that holds all the Notification settings.
        /// </summary>
        [System.Serializable]
        public class NotificationData
        {
            /// <summary>
            /// The target canvas where this notification will be shown.
            /// </summary>
            public string targetCanvasName = UICanvas.MASTER_CANVAS_NAME;
            /// <summary>
            /// The name of the notification prefab in a 'Resources' folder or the notification name set up in the Inspector of the UI Notification Manager
            /// </summary>
            public string prefabName;
            /// <summary>
            /// The prefab GameObject
            /// </summary>
            public GameObject prefab;
            /// <summary>
            /// The lifetime of the norification. Excluding the IN and OUT animation times, as they are calculated separately.
            /// </summary>
            public float lifetime = DEFAULT_LIFETIME;
            /// <summary>
            /// Should this notification be added to the Norification Queue or should it ignore it? (default: true)
            /// </summary>
            public bool addToNotificationQueue = DEFAULT_ADD_TO_NOTIFICATION_QUEUE;
            /// <summary>
            /// If the notification has a title, this text will appear there.
            /// </summary>
            public string title = DEFAULT_TITLE;
            /// <summary>
            /// If the notification has a message, this text will appear there.
            /// </summary>
            public string message = DEFAULT_MESSAGE;
            /// <summary>
            /// If the notification has a custom icon, this sprite will appear there.
            /// </summary>
            public Sprite icon = DEFAULT_ICON;
            /// <summary>
            /// If the notification has buttons, these are the buttonNames that will be sent on Button Click. If there are 3 buttons available and you enter only 2 buttonNames, only those 2 buttons will be visible and active (the 3rd will not appear, nor work).
            /// </summary>
            public string[] buttonNames = DEFAULT_BUTTON_NAMES;
            /// <summary>
            /// If the notification has buttons and those buttons have a Text or a TextMeshProUGUI compoment attached to them or one of their children, then these are the button text that will appear on the buttons.
            /// If there are 3 buttons available and active, and you enter the button text for only 2 of them, only the first 2 buttons well have a text, and the third will have nothing.
            /// You can leave this null if your buttons show pre-set icons instead of text.
            /// </summary>
            public string[] buttonTexts = DEFAULT_BUTTON_TEXT;
            /// <summary>
            /// Callback action @Hide
            /// </summary>
            public UnityAction hideCallback = null;
            /// <summary>
            /// Callback action for every button
            /// </summary>
            public UnityAction[] buttonCallback = null;
        }

        /// <summary>
        /// Default time interval of how long a notification should be seen on screen before the Hide command is automatically issued.
        /// </summary>
        public const float DEFAULT_LIFETIME = 3f;
        /// <summary>
        /// Default behavior if a notification should be added to the notification queue or be shown right away. 
        /// </summary>
        public const bool DEFAULT_ADD_TO_NOTIFICATION_QUEUE = true;
        /// <summary>
        /// Default notification title.
        /// </summary>
        public const string DEFAULT_TITLE = null;
        /// <summary>
        /// Default notification message.
        /// </summary>
        public const string DEFAULT_MESSAGE = null;
        /// <summary>
        /// Default notification icon.
        /// </summary>
        public const Sprite DEFAULT_ICON = null;
        /// <summary>
        /// Default notification array of button names (the button name is used to distinguish buttons one from the other).
        /// </summary>
        public const string[] DEFAULT_BUTTON_NAMES = null;
        /// <summary>
        /// Default notification array of button texts (the button texts are the text values shown on the buttons).
        /// </summary>
        public const string[] DEFAULT_BUTTON_TEXT = null;

        /// <summary>
        /// Should this notification listen for the 'Back' button? If yes, upon pressing the 'Back' button, the notification will close by automaically calling Hide on itself. Default is true.
        /// </summary>
        public bool listenForBackButton = true;

        /// <summary>
        /// The target canvas where this notification will be shown.
        /// </summary>
        public string targetCanvasName = UICanvas.MASTER_CANVAS_NAME;
        /// <summary>
        /// Used by the custom inspector to allow you to type a canvas name instead of selecting it from the Canvas Names Database.
        /// </summary>
        public bool customTargetCanvasName = false;
        /// <summary>
        /// Reference to the main UIElement that holds everything.
        /// </summary>
        public UIElement notificationContainer;
        /// <summary>
        /// Reference to an UIElement that can be used as a background image.
        /// </summary>
        public UIElement overlay;
        /// <summary>
        /// Reference to a child GameObject that has a Text component attached. This Text component will get its text value set to the notification's title.
        /// </summary>
        public GameObject title = null;
        /// <summary>
        /// Reference to a child GameObject that has a Text component attached. This Text component will get its text value set to the notification's message.
        /// </summary>
        public GameObject message = null;
        /// <summary>
        /// Reference to a child GameObject with an Image attached. This Image component will get its sprite value set to the notification's icon.
        /// </summary>
        public Image icon = null;
        /// <summary>
        /// An array of references of child UIButtons that will be used as the notification's buttons.
        /// </summary>
        public UIButton[] buttons = null;
        /// <summary>
        /// This is a reference to a Button component, that is attached by default to the notification's gameObject. Upon clicking the notification, it will auto close (by calling Hide on itself).
        /// </summary>
        public Button closeButton = null;
        /// <summary>
        /// An array of references to any other child UIElements that need to be controlled by this notification. It allows for a lot of flexibility design wise.
        ///<para>For example, if you have 3 stars with different animations, you can create an UIElement gameObject for each, set up their respective animations and reference them to this array.</para>
        /// </summary>
        public UIElement[] specialElements;
        /// <summary>
        /// An array of references to any child UIEffects used by this notification.
        /// </summary>
        public UIEffect[] effects;

        /// <summary>
        /// Used by the notification when it gets set up.
        /// </summary>
        public NotificationData data;

        /// <summary>
        /// Internal variable that holds a reference to the RectTransform component.
        /// </summary>
        private RectTransform m_rectTransform;
        /// <summary>
        /// Returns the RectTransform component.
        /// </summary>
        public RectTransform RectTransform { get { if(m_rectTransform == null) { m_rectTransform = GetComponent<RectTransform>() == null ? gameObject.AddComponent<RectTransform>() : GetComponent<RectTransform>(); } return m_rectTransform; } }


        /// <summary>
        /// Internal variable that holds an auto-generated element name (the system uses it in order to hide this notification when a one of it's buttons is clicked)
        /// </summary>
        private string notificationName = string.Empty;
        /// <summary>
        /// Internal variable that confirms if the nofitication should auto close (auto hide) if clicked. It's value is automatically set by the system in the setup stage.
        /// </summary>
        private bool closeOnClick = false;

        private int len;

        private void Reset()
        {
            RectTransform.localScale = Vector3.one;
            RectTransform.anchorMin = Vector2.zero;
            RectTransform.anchorMax = Vector2.one;
            RectTransform.sizeDelta = Vector2.zero;
            RectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        void Awake()
        {
            Initialize();
            //RegisterToUIManager();
        }

        /// <summary>
        /// Executes the initial setup of this notification.
        /// </summary>
        public void Initialize()
        {
            if(notificationContainer == null) { return; }
            notificationName = gameObject.name + " (" + gameObject.GetInstanceID() + ")";//generate a unique name for this notification UIElement (need it to be able to hide it when we press one of its buttons)
            notificationContainer.elementCategory = DUI.UNCATEGORIZED_CATEGORY_NAME;
            notificationContainer.elementName = notificationName;
            notificationContainer.linkedToNotification = true;
            notificationContainer.autoRegister = false; //register this element to the registry with the auto generated name (notifiacationName)
            notificationContainer.animateAtStart = false; //stop show animation
            if(overlay != null)
            {
                overlay.elementCategory = DUI.UNCATEGORIZED_CATEGORY_NAME;
                overlay.elementName = notificationName; //use the same elementName so it will get hidden along with the main UIElement
                overlay.linkedToNotification = true;
                overlay.autoRegister = false; //register this element to the registry with the auto generated name (notifiacationName)
                overlay.animateAtStart = false;
            }

            len = specialElements != null ? specialElements.Length : 0;
            if(len > 0)
            {
                for(int i = 0; i < len; i++)
                {
                    if(specialElements[i] == null) { continue; }
                    specialElements[i].elementCategory = DUI.UNCATEGORIZED_CATEGORY_NAME;
                    specialElements[i].elementName = notificationName; //use the same elementName so it will get hidden along with the main UIElement
                    specialElements[i].linkedToNotification = true;
                    specialElements[i].autoRegister = false; //register this element to the registry with the auto generated name (notifiacationName)
                    specialElements[i].animateAtStart = false; //stop show animation
                }
            }

            len = effects != null ? effects.Length : 0;
            if(len > 0)
            {
                for(int i = 0; i < len; i++)
                {
                    if(effects[i] == null) { continue; }
                    if(effects[i].targetUIElement == null) { continue; }
                    effects[i].targetUIElement.elementCategory = DUI.UNCATEGORIZED_CATEGORY_NAME;
                    effects[i].targetUIElement.elementName = notificationName; //use the same elementName so it will get hidden along with the main UIElement
                    effects[i].targetUIElement.linkedToNotification = true;
                    effects[i].autoRegister = false; //register this element to the registry with the auto generated name (notifiacationName)
                    effects[i].playOnAwake = false; //stop show animation
                }
            }
            data = new NotificationData();
        }

        void OnEnable()
        {
            UIManager.DisableBackButton();
            RegisterToUIManager();
        }

        void OnDisable()
        {
            if(notificationContainer == null) { return; }
            UIManager.EnableBackButton();
            UnregisterFromNotificationQueue();
            UnregisterFromUIManager();
        }

        private void OnDestroy()
        {
            //if (notificationContainer == null) { return; }
            //UIManager.EnableBackButton();
            //UnregisterFromNotificationQueue();
            //UnregisterFromUIManager();
        }

        void Update()
        {
            if(listenForBackButton && Input.GetKeyDown(KeyCode.Escape)) //The listener for the 'Back' button event; we do this because we do not want to change the UI state while a notification is on screen (we disabled the back button from the UI Manager and the UI Notifications needs to listen for it now)
            {
                if(closeOnClick) //only if closeOnClick has been enabled (a closeOnClick button has been assigned) we close the notification
                    BackButtonEvent();
            }
        }

        /// <summary>
        /// Shows the notification considering the NotificationData value.
        /// </summary>
        public void ShowNotification(NotificationData ndata, UICanvas targetCanvas)
        {
            foreach(Transform t in transform) //in case we have a child that is not on the proper layer, we set it here so it shows up in the target camera
            {
                t.gameObject.layer = gameObject.layer;
                Canvas c = t.gameObject.GetComponent<Canvas>();
                if(c != null)
                {
                    c.sortingLayerName = targetCanvas.Canvas.sortingLayerName;
                    c.overrideSorting = true;
                    c.sortingOrder += 10000;
                }
                Renderer r = t.gameObject.GetComponent<Renderer>();
                if(r != null)
                {
                    r.sortingLayerName = targetCanvas.Canvas.sortingLayerName;
                    r.sortingOrder += 10000;
                }
            }

            if(notificationContainer != null) { notificationContainer.startHidden = true; }
            if(overlay != null) { overlay.startHidden = true; }

            len = specialElements != null ? specialElements.Length : 0;
            if(len > 0)
            {
                for(int i = 0; i < len; i++)
                {
                    if(specialElements[i] == null) { continue; }
                    specialElements[i].startHidden = true;
                }
            }

            data = ndata;   //we save this data to use it if we need to unregister from the Notification Queue

            if(icon != null && ndata.icon != null) //if this notification has an icon slot and the show notification passed a new icon, we update it
            {
                icon.sprite = ndata.icon;
            }

            if(ndata.buttonTexts == null || ndata.buttonTexts.Length == 0)
                closeOnClick = true;    //if there are no button texts we let the user close this notification just by ckicking it

            if(ndata.buttonNames == null || ndata.buttonNames.Length == 0)
                closeOnClick = true;    //if there are no button names we let the user close this notification just by ckicking it

            if(closeButton != null && closeOnClick) //if we linked the closeOnClickButton, we configure it to close the notification window on click
            {
                UIButton b = closeButton.GetComponent<UIButton>();
                float onClickDelay = 0f;
                if(b != null) //we check if we have an UIButton attached (we do this so that if the button has an onClick animation, we show it and after that we hide the notification)
                {
                    onClickDelay = b.onClickPunch.TotalDuration; //this creates a more pleasent user experience (UX) by letthing the OnClick animation play before hiding the notification
                }

                closeButton.onClick.AddListener(() =>
                {
                    StartCoroutine(HideNotification(onClickDelay));
                    StartCoroutine(DestroyAfterTime(GetOutAnimationsTimeAndDelay() + onClickDelay));
                });
            }

            if(ndata.lifetime > 0)  //We look for the lifetime (if it's -1 we do not auto hide the notification. We wait for the player to hit a button.
            {
                StartCoroutine(HideNotification(GetInAnimationsTimeAndDelay() + ndata.lifetime));
                StartCoroutine(DestroyAfterTime(GetInAnimationsTimeAndDelay() + ndata.lifetime + GetOutAnimationsTimeAndDelay()));
            }

#if dUI_TextMeshPro
            if(title != null)
            {
                if(title.GetComponent<TMPro.TextMeshProUGUI>() != null)
                {
                    title.GetComponent<TMPro.TextMeshProUGUI>().text = ndata.title;
                }
                else if(title.GetComponent<Text>() != null)
                {
                    title.GetComponent<Text>().text = ndata.title;
                }
            }
            if(message != null)
            {
                if(message.GetComponent<TMPro.TextMeshProUGUI>() != null)
                {
                    message.GetComponent<TMPro.TextMeshProUGUI>().text = ndata.message;
                }
                else if(message.GetComponent<Text>() != null)
                {
                    message.GetComponent<Text>().text = ndata.message;
                }
            }
#else
            if (title != null && title.GetComponent<Text>() != null)
            {
                title.GetComponent<Text>().text = ndata.title;
            }
            if (message != null && message.GetComponent<Text>() != null)
            {
                message.GetComponent<Text>().text = ndata.message;
            }
#endif

            if(buttons != null && ndata.buttonNames != null) //If this notification prefab has buttons and buttonNames is not null (those are the buttonNames we are listening for) we start adding the buttons
            {
                for(int i = 0; i < buttons.Length; i++)
                {
                    var index = i;
                    buttons[i].GetComponent<Button>().onClick.AddListener(() =>
                                {
                                    if(ndata.buttonCallback != null && index < ndata.buttonCallback.Length && ndata.buttonCallback[index] != null)
                                    {
                                        ndata.buttonCallback[index].Invoke();
                                    }
                                    StartCoroutine(HideNotification(0.2f));
                                    StartCoroutine(DestroyAfterTime(GetOutAnimationsTimeAndDelay() + 0.2f)); //We destroy this notification after the Out animation finished
                                });

                    if(ndata.buttonNames.Length > i && string.IsNullOrEmpty(ndata.buttonNames[i]) == false) //If we have a buttonName we make the button active and set the buttonName to the UIButton compoenent
                    {
                        buttons[i].gameObject.SetActive(true);
                        buttons[i].buttonName = ndata.buttonNames[i];   //We set the buttonName

                        if(ndata.buttonTexts != null) //We might not have a text for the button (it might be an image or an icon) so we check if we wanted a text on it
                        {
                            if(ndata.buttonTexts.Length > i && !string.IsNullOrEmpty(ndata.buttonTexts[i]))
                            {
#if dUI_TextMeshPro
                                if(buttons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>() != null)
                                {
                                    buttons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = ndata.buttonTexts[i];
                                }
#else
                                if(buttons[i].GetComponentInChildren<Text>() != null)
                                {
                                    buttons[i].GetComponentInChildren<Text>().text = ndata.buttonTexts[i];
                                }
#endif
                            }
                        }

                    }
                    else
                    {
                        buttons[i].gameObject.SetActive(false); //if we still have unused buttons on this notification prefab, we hide them
                    }
                }
            }
            StartCoroutine(ShowNotification());
        }

        /// <summary>
        /// Executes the showing of this notification in the next frame.
        /// </summary>
        IEnumerator ShowNotification()
        {
            yield return null;
            UIManager.ShowUiElement(notificationName);
        }

        /// <summary>
        /// Executes the automated Hide command for this notification in realtime.
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        IEnumerator HideNotification(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            UIManager.HideUiElement(notificationName, DUI.UNCATEGORIZED_CATEGORY_NAME);
        }
        /// <summary>
        /// Hides the notification with a destroy option. Default notification behavior is to get automatically destroyed.
        /// </summary>
        /// <param name="hideAndDestroy"></param>
        public void HideNotification(bool hideAndDestroy = true)
        {
            UIManager.HideUiElement(notificationName, DUI.UNCATEGORIZED_CATEGORY_NAME);
            if(hideAndDestroy) { StartCoroutine(DestroyAfterTime(GetOutAnimationsTimeAndDelay())); }
        }

        /// <summary>
        /// Registers this UINotification to the UIManager by registering all the referenced components (UIElements and UIEffects).
        /// </summary>
        void RegisterToUIManager()
        {
            if(notificationContainer == null)
            {
                Debug.Log("[DoozyUI] The UINotification on [" + gameObject.name + "] gameObject is disabled. It will not work because you didn't link a notification container. This should be a child of the notification gamObject and it should have a UIElement on it. Also, the notification container should have at least one IN and one OUT animations enabled.");
                return;
            }
            notificationContainer.RegisterToUIManager();
            if(overlay != null) { overlay.RegisterToUIManager(); }

            len = specialElements != null ? specialElements.Length : 0;
            if(len > 0)
            {
                for(int i = 0; i < len; i++)
                {
                    if(specialElements[i] != null) { specialElements[i].RegisterToUIManager(); }
                    else { Debug.LogWarning("[DoozyUI] The UINotification on the " + gameObject.name + " gameObject has unassigned array slots for the Special Elements. To fix this, just remove the unused slots in the array or assign UIElements to them."); }
                }
            }

            len = effects != null ? effects.Length : 0;
            if(len > 0)
            {
                for(int i = 0; i < len; i++)
                {
                    if(effects[i] != null) { effects[i].RegisterToUIManager(); }
                    else { Debug.LogWarning("[DoozyUI] The UINotification on the " + gameObject.name + " gameObject has unassigned array slots for the Effects. To fix this, just remove the unused slots in the array or assign UIEffects to them."); }

                }
            }
        }
        /// <summary>
        /// Unregisters this UINotification from the UIManager by unregistering all the referenced components (UIElements and UIEffects).
        /// </summary>
        void UnregisterFromUIManager()
        {
            if(notificationContainer == null)
            {
                Debug.Log("[DoozyUI] The UINotification on [" + gameObject.name + "] gameObject is disabled. It will not work because you didn't link a notification container. This should be a child of the notification gamObject and it should have a UIElement on it. Also, the notification container should have at least one IN and one OUT animations enabled.");
                return;
            }
            notificationContainer.UnregisterFromUIManager();
            if(overlay != null) { overlay.UnregisterFromUIManager(); }

            len = specialElements != null ? specialElements.Length : 0;
            if(len > 0)
            {
                for(int i = 0; i < specialElements.Length; i++)
                {
                    if(specialElements[i] == null) { continue; }
                    specialElements[i].UnregisterFromUIManager();
                }
            }

            len = effects != null ? effects.Length : 0;
            if(len > 0)
            {
                for(int i = 0; i < len; i++)
                {
                    if(effects[i] == null) { continue; }
                    effects[i].UnregisterFromUIManager();
                }
            }
        }
        /// <summary>
        /// Unregisteres this UINotification from the Notification queue.
        /// </summary>
        void UnregisterFromNotificationQueue()
        {
            if(data == null) { return; }
            if(data.addToNotificationQueue && UIManager.Instance != null)
            {
                UIManager.Instance.UnregisterFromNotificationQueue(data);
            }
        }

        /// <summary>
        /// Returns the In Animations TotalDuration of the notification cotainer (UIElement).
        /// </summary>
        float GetInAnimationsTimeAndDelay()
        {
            float timeAndDelay = 0;
            if(notificationContainer == null) { return timeAndDelay; }
            timeAndDelay = notificationContainer.inAnimations.TotalDuration;

            len = specialElements != null ? specialElements.Length : 0;
            if(len > 0)
            {
                for(int i = 0; i < len; i++)
                {
                    if(specialElements[i] == null) { continue; }
                    if(timeAndDelay < specialElements[i].inAnimations.TotalDuration) { timeAndDelay = specialElements[i].inAnimations.TotalDuration; }
                }
            }
            return timeAndDelay;
        }
        /// <summary>
        /// Returns the Out Animations TotalDuration of the notification cotainer (UIElement).
        /// </summary>
        float GetOutAnimationsTimeAndDelay()
        {
            float timeAndDelay = 0;
            if(notificationContainer == null) { return timeAndDelay; }
            timeAndDelay = notificationContainer.outAnimations.TotalDuration;

            len = specialElements != null ? specialElements.Length : 0;
            if(len > 0)
            {
                for(int i = 0; i < len; i++)
                {
                    if(specialElements[i] == null) { continue; }
                    if(timeAndDelay < specialElements[i].outAnimations.TotalDuration) { timeAndDelay = specialElements[i].outAnimations.TotalDuration; }
                }
            }
            return timeAndDelay;
        }

        /// <summary>
        /// Executes the 'Back' button event. It does so by hiding the notification and then destrying it after the notification container (UIElement) finished its out animations (it got hidden).
        /// </summary>
        void BackButtonEvent()
        {
            StartCoroutine(HideNotification(0f));
            StartCoroutine(DestroyAfterTime(GetOutAnimationsTimeAndDelay()));
        }

        /// <summary>
        /// Destroies this notification after the set delay. It is also reposible to enable the 'Back' button functionality back in the UIManager.
        /// </summary>
        IEnumerator DestroyAfterTime(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            UIManager.EnableButtonClicks();
            if(data.hideCallback != null) { data.hideCallback.Invoke(); }
            Destroy(gameObject);
        }
    }
}
