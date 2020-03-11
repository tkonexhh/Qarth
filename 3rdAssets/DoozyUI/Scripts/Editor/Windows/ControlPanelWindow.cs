// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using QuickEngine.Extensions;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace DoozyUI
{
    public partial class ControlPanelWindow : QWindow
    {
        public static ControlPanelWindow Instance;

        static bool _utility = false;
        static string _title = "DoozyUI";
        static bool _focus = true;

        int inspectorUpdateFrame = 0; // Prevent OnInspector() forcing a repaint every time it's called.

        const float TWITTER_BUTTON_WIDTH_PERCENTAGE = 0.27f;
        const float FACEBOOK_BUTTON_WIDTH_PERCENTAGE = 0.34f;
        const float YOUTUBE_BUTTON_WIDTH_PERCENTAGE = 0.31f;

        float GlobalWidth { get { return DUI.GLOBAL_EDITOR_WIDTH; } }
        int BigBarHeight { get { return DUI.BIG_BAR_HEIGHT; } }
        int BarHeight { get { return DUI.BAR_HEIGHT; } }
        int MiniBarHeight { get { return DUI.MINI_BAR_HEIGHT; } }

        float tempFloat = 0;

        GUIStyle _pageHeaderTitleStyle;
        GUIStyle PageHeaderTitleStyle
        {
            get
            {
                if(_pageHeaderTitleStyle == null)
                {
                    _pageHeaderTitleStyle = new GUIStyle(GUI.skin.label)
                    {
                        font = QResources.GetFont(FontName.Sansation.Regular),
                        fontSize = 26,
                        alignment = TextAnchor.MiddleLeft,
                        wordWrap = false,
                        margin = new RectOffset(0, 0, 0, 0),
                        border = new RectOffset(0, 0, 0, 0),
                        padding = new RectOffset(0, 0, 0, 0),
                    };
                }
                return _pageHeaderTitleStyle;
            }
        }

        GUIStyle _pageHeaderSubtitleStyle;
        GUIStyle PageHeaderSubtitleStyle
        {
            get
            {
                if(_pageHeaderSubtitleStyle == null)
                {
                    _pageHeaderSubtitleStyle = new GUIStyle(GUI.skin.label)
                    {
                        font = QResources.GetFont(FontName.Sansation.Light),
                        fontSize = 16,
                        alignment = TextAnchor.MiddleLeft,
                        wordWrap = false,
                        margin = new RectOffset(0, 0, 0, 0),
                        border = new RectOffset(0, 0, 0, 0),
                        padding = new RectOffset(0, 0, 0, 0)
                    };
                }
                return _pageHeaderSubtitleStyle;
            }
        }

        public static bool Selected = false;

        public static bool refreshData = true;

        public static bool ExecuteInitPage { get { return PreviousPage != WindowSettings.currentPage || refreshData; } }

        public enum AnimatorPreset { In, Out, Loop, Punch, State }

        public enum Page
        {
            None,

            General,

            UIElements,
            UIButtons,
            UISounds,
            UICanvases,
            AnimatorPresets,

            EditorSettings,

            Help,
            About
        }

        public static Page PreviousPage = Page.None;

        static Vector2 SectionScrollPosition = Vector2.zero;
#pragma warning disable 0414 //value is never used
        static Vector2 SectionLastScrollPosition = Vector2.zero;
#pragma warning restore 0414

        public static ControlPanelWindowSettings WindowSettings { get { return ControlPanelWindowSettings.Instance; } }
        public static DUISettings EditorSettings { get { return DUISettings.Instance; } }

        static Dictionary<string, AnimBool> InAnimationsAnimatorPresetsAnimBool;
        static Dictionary<string, AnimBool> OutAnimationsAnimatorPresetsAnimBool;
        static Dictionary<string, AnimBool> LoopsAnimatorPresetsAnimBool;
        static Dictionary<string, AnimBool> PunchesAnimatorPresetsAnimBool;

        static string NewCategoryName = "";
        static AnimBool NewCategoryAnimBool = new AnimBool(false);

        static string RenameCategoryName = "";
        static AnimBool RenameCategoryAnimBool = new AnimBool(false);
        static string RenameCategoryTargetCategoryName = "";

        string SearchPattern = string.Empty;
        static AnimBool SearchPatternAnimBool = new AnimBool(false);

        string tempLabel = string.Empty;
        Rect tempRect;

        [MenuItem("Tools/DoozyUI/Control Panel &d", false, 0)]
        static void Init()
        {
            Instance = GetWindow<ControlPanelWindow>(_utility, _title, _focus);
            Instance.SetupWindow();
        }

        void SetupWindow()
        {
            titleContent = new GUIContent(_title);
            WindowSettings.sidebarIsExpanded.speed = 2;
            WindowSettings.sidebarIsExpanded.valueChanged.RemoveAllListeners();
            WindowSettings.sidebarIsExpanded.valueChanged.AddListener(Repaint);
            WindowSettings.sidebarIsExpanded.valueChanged.AddListener(UpdateWindowSize);

            WindowSettings.pageWidthExtraSpace.valueChanged.RemoveAllListeners();
            WindowSettings.pageWidthExtraSpace.valueChanged.AddListener(Repaint);
            WindowSettings.pageWidthExtraSpace.valueChanged.AddListener(UpdateWindowSize);

            UpdateWindowSize();
            CenterWindow();
        }

        void UpdateWindowSize()
        {
            minSize = new Vector2(WindowSettings.windowMinimumWidth, WindowSettings.windowMinimumHeight);
            maxSize = new Vector2(minSize.x, Screen.currentResolution.height);
            //maxSize = minSize;
        }

        public static void OpenWindow(Page section)
        {
            Init();
            //#if UNITY_EDITOR_OSX
            //            QUI.ExitGUI();
            //#endif
            WindowSettings.currentPage = section;
            refreshData = true;
        }

        private void OnEnable()
        {
            autoRepaintOnSceneChange = true;
            requiresContantRepaint = true;

            GenerateInfoMessages();

            PreviousPage = Page.None;
        }

        enum InfoMessageName
        {
            NoResults,
            EmptyList,
            EmptyCategory,
            AddCategoryToStart,
        }

        void GenerateInfoMessages()
        {
            infoMessage = new Dictionary<string, InfoMessage>()
            {
                {InfoMessageName.NoResults.ToString(), new InfoMessage(){ title = "No results found...", message = "", type = InfoMessageType.Info, show = new AnimBool(true, Repaint) } },
                {InfoMessageName.EmptyList.ToString(), new InfoMessage(){ title = "List is empty...", message = "", type = InfoMessageType.Info, show = new AnimBool(true, Repaint) } },
                {InfoMessageName.EmptyCategory.ToString(), new InfoMessage(){ title = "Category is empty...", message = "", type = InfoMessageType.Info, show = new AnimBool(true, Repaint) } },
                {InfoMessageName.AddCategoryToStart.ToString(), new InfoMessage(){ title = "Add a new category to get started...", message = "", type = InfoMessageType.Info, show = new AnimBool(true, Repaint) } },
            };
        }

        void DetectKeys()
        {
            //if(Event.current.isKey) { Debug.Log(Event.current.keyCode); }

            //CLOSE WINDOW
            if(QUI.DetectKeyDownCombo(Event.current, EventModifiers.Alt, KeyCode.X)) { Close(); }

            //COLLAPSE/EXPAND WINDOW
            if(QUI.DetectKeyDownCombo(Event.current, EventModifiers.Alt, KeyCode.BackQuote)) { ToggleExpandCollapseSideBar(); }

            //TAB SELECTORS
            if(QUI.DetectKeyDownCombo(Event.current, EventModifiers.Alt, KeyCode.Alpha1)) { WindowSettings.currentPage = Page.General; }
            if(QUI.DetectKeyDownCombo(Event.current, EventModifiers.Alt, KeyCode.Alpha2)) { WindowSettings.currentPage = Page.UIElements; }
            if(QUI.DetectKeyDownCombo(Event.current, EventModifiers.Alt, KeyCode.Alpha3)) { WindowSettings.currentPage = Page.UIButtons; }
            if(QUI.DetectKeyDownCombo(Event.current, EventModifiers.Alt, KeyCode.Alpha4)) { WindowSettings.currentPage = Page.UISounds; }
            if(QUI.DetectKeyDownCombo(Event.current, EventModifiers.Alt, KeyCode.Alpha5)) { WindowSettings.currentPage = Page.UICanvases; }
            if(QUI.DetectKeyDownCombo(Event.current, EventModifiers.Alt, KeyCode.Alpha6)) { WindowSettings.currentPage = Page.AnimatorPresets; }
            if(QUI.DetectKeyDownCombo(Event.current, EventModifiers.Alt, KeyCode.Alpha7)) { WindowSettings.currentPage = Page.EditorSettings; }
            if(QUI.DetectKeyDownCombo(Event.current, EventModifiers.Alt, KeyCode.Alpha8)) { WindowSettings.currentPage = Page.Help; }
            if(QUI.DetectKeyDownCombo(Event.current, EventModifiers.Alt, KeyCode.Alpha9)) { WindowSettings.currentPage = Page.About; }
        }

        bool DetectKey_Return() { return QUI.DetectKeyUp(Event.current, KeyCode.Return); }
        bool DetectKey_Escape() { return QUI.DetectKeyUp(Event.current, KeyCode.Escape); }

        bool DetectKeyCombo_Alt_S() { return QUI.DetectKeyUpCombo(Event.current, EventModifiers.Alt, KeyCode.S); } //used to Toggle Search
        bool DetectKeyCombo_Alt_N() { return QUI.DetectKeyUpCombo(Event.current, EventModifiers.Alt, KeyCode.N); } //used to Toggle New Item creation
        bool DetectKeyCombo_Alt_E() { return QUI.DetectKeyUpCombo(Event.current, EventModifiers.Alt, KeyCode.E); } //used to Expand all categories
        bool DetectKeyCombo_Alt_C() { return QUI.DetectKeyUpCombo(Event.current, EventModifiers.Alt, KeyCode.C); } //used to Collapse all categories

        private void OnGUI()
        {
            DetectKeys();

            UpdateWindowSize();
            DrawBackground();

            QUI.BeginHorizontal(position.width);
            {
                DrawSideBar();
                QUI.Space(WindowSettings.pageShadowWidth);
                DrawPages();
            }
            QUI.EndHorizontal();

            //if(SectionScrollPosition != SectionLastScrollPosition) //if the user has scrolled, deselect - this is because control IDs within carousel will change when scrolled so we'd end up with the wrong box selected.
            //{
            //    //this shit generates errors ---> GUI.FocusControl(""); //deselect
            //}

            Repaint(); //without this, the window would seem to be lagging when animating (it is just a Repaint issue - we perfromed memory tests and this is the proper fix)
        }

        private void OnInspectorUpdate()
        {
            if(inspectorUpdateFrame % 10 == 0) //once a second (every 10th frame)
            {
                Repaint(); //force the window to repaint
            }

            inspectorUpdateFrame++; //track what frame we're on, so we can call code less often
        }

        private void OnFocus()
        {
            Selected = true;
        }

        private void OnLostFocus()
        {
            Selected = false;
            switch(WindowSettings.currentPage)
            {
                case Page.None:
                    break;
                case Page.General:
                    break;
                case Page.UIElements:
                    DUIData.Instance.ValidateUIElements(); //when deselecting the window check that the dev didn't mess up the database
                    break;
                case Page.UIButtons:
                    DUIData.Instance.ValidateUIButtons(); //when deselecting the window check that the dev didn't mess up the database
                    break;
                case Page.UISounds:
                    DUIData.Instance.ValidateUISounds(); //when deselecting the window check that the dev didn't mess up the database
                    break;
                case Page.UICanvases:
                    DUIData.Instance.ValidateUICanvases(); //when deselecting the window check that the dev didn't mess up the database
                    break;
                case Page.AnimatorPresets:
                    break;
                case Page.EditorSettings:
                    break;
                case Page.Help:
                    break;
                case Page.About:
                    break;
                default:
                    break;
            }
        }

        private void OnDisable()
        {

        }

        void DrawBackground()
        {
            tempRect = new Rect(0, 0, position.width, position.height);
            QUI.DrawTexture(new Rect(tempRect.x, tempRect.y, WindowSettings.SidebarCurrentWidth, tempRect.height), QResources.backgroundSidebar.texture);
            tempRect.x += WindowSettings.SidebarCurrentWidth;
            QUI.DrawTexture(new Rect(tempRect.x, tempRect.y, WindowSettings.pageShadowWidth, tempRect.height), QResources.backgroundContentShadowLeft.texture);
            tempRect.x += WindowSettings.pageShadowWidth;
            QUI.DrawTexture(new Rect(tempRect.x, tempRect.y, WindowSettings.pageShadowWidth + WindowSettings.CurrentPageContentWidth, tempRect.height), QResources.backgroundContent.texture);
        }

        void DrawSideBar()
        {
            QUI.BeginVertical(WindowSettings.SidebarCurrentWidth);
            {
                DrawSideBarLogo();
                DrawSideBarExpandCollapseButton();
                QUI.Space(WindowSettings.sidebarVerticalSpacing);
                DrawSideButton(Page.General, "General", DUIStyles.GetStyle(DUIStyles.SideButton.ControlPanel), DUIStyles.GetStyle(DUIStyles.SideButton.ControlPanelSelected));
                QUI.Space(WindowSettings.sidebarVerticalSpacing);
                DrawSideButton(Page.UIElements, "UIElements", DUIStyles.GetStyle(DUIStyles.SideButton.UIElements), DUIStyles.GetStyle(DUIStyles.SideButton.UIElementsSelected));
                DrawSideButton(Page.UIButtons, "UIButtons", DUIStyles.GetStyle(DUIStyles.SideButton.UIButtons), DUIStyles.GetStyle(DUIStyles.SideButton.UIButtonsSelected));
                DrawSideButton(Page.UISounds, "UISounds", DUIStyles.GetStyle(DUIStyles.SideButton.UISounds), DUIStyles.GetStyle(DUIStyles.SideButton.UISoundsSelected));
                DrawSideButton(Page.UICanvases, "UICanvases", DUIStyles.GetStyle(DUIStyles.SideButton.UICanvases), DUIStyles.GetStyle(DUIStyles.SideButton.UICanvasesSelected));
                DrawSideButton(Page.AnimatorPresets, "Animator Presets", DUIStyles.GetStyle(DUIStyles.SideButton.AnimatorPresets), DUIStyles.GetStyle(DUIStyles.SideButton.AnimatorPresetsSelected));
                QUI.Space(WindowSettings.sidebarVerticalSpacing);
                DrawSideButton(Page.EditorSettings, "Editor Settings", DUIStyles.GetStyle(DUIStyles.SideButton.EditorSettings), DUIStyles.GetStyle(DUIStyles.SideButton.EditorSettingsSelected));
                QUI.Space(WindowSettings.sidebarVerticalSpacing);
                DrawSideButton(Page.Help, "Help", DUIStyles.GetStyle(DUIStyles.SideButton.Help), DUIStyles.GetStyle(DUIStyles.SideButton.HelpSelected));
                DrawSideButton(Page.About, "About", DUIStyles.GetStyle(DUIStyles.SideButton.About), DUIStyles.GetStyle(DUIStyles.SideButton.AboutSelected));
                QUI.FlexibleSpace();
                DrawSideBarSocial();
            }
            QUI.EndVertical();
        }
        void DrawSideBarLogo()
        {
            if(WindowSettings.sidebarIsExpanded.faded <= 0.05f) { QUI.DrawTexture(DUIResources.sidebarLogo10.texture, WindowSettings.sidebarExpandedWidth, WindowSettings.sidebarLogoHeight); }
            else if(WindowSettings.sidebarIsExpanded.faded <= 0.1f) { QUI.DrawTexture(DUIResources.sidebarLogo9.texture, WindowSettings.sidebarExpandedWidth, WindowSettings.sidebarLogoHeight); }
            else if(WindowSettings.sidebarIsExpanded.faded <= 0.2f) { QUI.DrawTexture(DUIResources.sidebarLogo8.texture, WindowSettings.sidebarExpandedWidth, WindowSettings.sidebarLogoHeight); }
            else if(WindowSettings.sidebarIsExpanded.faded <= 0.3f) { QUI.DrawTexture(DUIResources.sidebarLogo7.texture, WindowSettings.sidebarExpandedWidth, WindowSettings.sidebarLogoHeight); }
            else if(WindowSettings.sidebarIsExpanded.faded <= 0.4f) { QUI.DrawTexture(DUIResources.sidebarLogo6.texture, WindowSettings.sidebarExpandedWidth, WindowSettings.sidebarLogoHeight); }
            else if(WindowSettings.sidebarIsExpanded.faded <= 0.5f) { QUI.DrawTexture(DUIResources.sidebarLogo5.texture, WindowSettings.sidebarExpandedWidth, WindowSettings.sidebarLogoHeight); }
            else if(WindowSettings.sidebarIsExpanded.faded <= 0.6f) { QUI.DrawTexture(DUIResources.sidebarLogo4.texture, WindowSettings.sidebarExpandedWidth, WindowSettings.sidebarLogoHeight); }
            else if(WindowSettings.sidebarIsExpanded.faded <= 0.7f) { QUI.DrawTexture(DUIResources.sidebarLogo3.texture, WindowSettings.sidebarExpandedWidth, WindowSettings.sidebarLogoHeight); }
            else if(WindowSettings.sidebarIsExpanded.faded <= 0.8f) { QUI.DrawTexture(DUIResources.sidebarLogo2.texture, WindowSettings.sidebarExpandedWidth, WindowSettings.sidebarLogoHeight); }
            else if(WindowSettings.sidebarIsExpanded.faded <= 0.9f) { QUI.DrawTexture(DUIResources.sidebarLogo1.texture, WindowSettings.sidebarExpandedWidth, WindowSettings.sidebarLogoHeight); }
            else { QUI.DrawTexture(DUIResources.sidebarLogo0.texture, WindowSettings.sidebarExpandedWidth, WindowSettings.sidebarLogoHeight); }
        }
        void DrawSideBarExpandCollapseButton()
        {
            if(QUI.Button("", DUIStyles.GetStyle(WindowSettings.sidebarIsExpanded.faded < 0.9f ? DUIStyles.SideButton.ExpandSideBar : DUIStyles.SideButton.CollapseSideBar), WindowSettings.SidebarCurrentWidth, WindowSettings.sidebarButtonHeight))
            {
                ToggleExpandCollapseSideBar();
            }
        }
        void DrawSideButton(Page page, string label, GUIStyle style, GUIStyle styleSelected)
        {
            tempLabel = WindowSettings.sidebarIsExpanded.faded < 0.6f ? "" : label;
            if(QUI.Button(tempLabel, WindowSettings.currentPage != page ? style : styleSelected, WindowSettings.SidebarCurrentWidth, WindowSettings.sidebarButtonHeight))
            {
                if(WindowSettings.currentPage != page)
                {
                    WindowSettings.currentPage = page;
                    ResetPageView();
                }
            }
        }
        void DrawSideBarSocial()
        {
            if(WindowSettings.sidebarIsExpanded.faded < 0.3f)
            {
                QUI.BeginVertical(WindowSettings.SidebarCurrentWidth, WindowSettings.sidebarButtonHeight * 3);
            }
            else
            {
                QUI.BeginHorizontal(WindowSettings.SidebarCurrentWidth, WindowSettings.sidebarButtonHeight);
            }
            {
                tempLabel = WindowSettings.sidebarIsExpanded.faded < 0.8f ? "" : "Twitter";
                if(QUI.Button(tempLabel, DUIStyles.GetStyle(DUIStyles.SideButton.Twitter), WindowSettings.sidebarExpandedWidth / 3 >= WindowSettings.SidebarCurrentWidth ? WindowSettings.SidebarCurrentWidth : WindowSettings.SidebarCurrentWidth * TWITTER_BUTTON_WIDTH_PERCENTAGE, WindowSettings.sidebarButtonHeight))
                {
                    Application.OpenURL("https://twitter.com/doozyplay");
                }
                if(WindowSettings.sidebarExpandedWidth / 3 < WindowSettings.SidebarCurrentWidth) { QUI.FlexibleSpace(); }
                tempLabel = WindowSettings.sidebarIsExpanded.faded < 0.8f ? "" : "Facebook";
                if(QUI.Button(tempLabel, DUIStyles.GetStyle(DUIStyles.SideButton.Facebook), WindowSettings.sidebarExpandedWidth / 3 >= WindowSettings.SidebarCurrentWidth ? WindowSettings.SidebarCurrentWidth : WindowSettings.SidebarCurrentWidth * FACEBOOK_BUTTON_WIDTH_PERCENTAGE, WindowSettings.sidebarButtonHeight))
                {
                    Application.OpenURL("https://www.facebook.com/doozyentertainment");
                }
                if(WindowSettings.sidebarExpandedWidth / 3 < WindowSettings.SidebarCurrentWidth) { QUI.FlexibleSpace(); }
                tempLabel = WindowSettings.sidebarIsExpanded.faded < 0.8f ? "" : "YouTube";
                if(QUI.Button(tempLabel, DUIStyles.GetStyle(DUIStyles.SideButton.Youtube), WindowSettings.sidebarExpandedWidth / 3 >= WindowSettings.SidebarCurrentWidth ? WindowSettings.SidebarCurrentWidth : WindowSettings.SidebarCurrentWidth * YOUTUBE_BUTTON_WIDTH_PERCENTAGE, WindowSettings.sidebarButtonHeight))
                {
                    Application.OpenURL("http://www.youtube.com/c/DoozyEntertainment");
                }
                if(WindowSettings.sidebarExpandedWidth / 3 >= WindowSettings.SidebarCurrentWidth) { QUI.FlexibleSpace(); }
            }
            if(WindowSettings.sidebarIsExpanded.faded < 0.3f)
            {
                QUI.EndVertical();
            }
            else
            {
                QUI.EndHorizontal();
            }
        }

        void InitPages()
        {
            switch(WindowSettings.currentPage)
            {
                case Page.None:
                    WindowSettings.pageWidthExtraSpace.target = 0;
                    break;
                case Page.General:
                    WindowSettings.pageWidthExtraSpace.target = -72;
                    if(ExecuteInitPage) { InitPageGeneral(); }
                    break;
                case Page.UIElements:
                    WindowSettings.pageWidthExtraSpace.target = -300;
                    if(ExecuteInitPage) { InitPageUIElements(); }
                    break;
                case Page.UIButtons:
                    WindowSettings.pageWidthExtraSpace.target = -300;
                    if(ExecuteInitPage) { InitPageUIButtons(); }
                    break;
                case Page.UISounds:
                    WindowSettings.pageWidthExtraSpace.target = -200;
                    if(ExecuteInitPage) { InitPageUISounds(); }
                    break;
                case Page.UICanvases:
                    WindowSettings.pageWidthExtraSpace.target = -300;
                    if(ExecuteInitPage) { InitPageUICanvases(); }
                    break;
                case Page.AnimatorPresets:
                    WindowSettings.pageWidthExtraSpace.target = -200;
                    if(ExecuteInitPage) { InitPageAnimatorPresets(); }
                    break;
                case Page.EditorSettings:
                    WindowSettings.pageWidthExtraSpace.target = 20;
                    if(ExecuteInitPage) { InitPageEditorSettings(); }
                    break;
                case Page.Help:
                    WindowSettings.pageWidthExtraSpace.target = -300;
                    if(ExecuteInitPage) { InitPageHelp(); }
                    break;
                case Page.About:
                    WindowSettings.pageWidthExtraSpace.target = -240;
                    if(ExecuteInitPage) { InitPageAbout(); }
                    break;
                default:
                    WindowSettings.pageWidthExtraSpace.target = 0;
                    break;
            }
        }

        void DrawPages()
        {
            InitPages();

            SectionScrollPosition = QUI.BeginScrollView(SectionScrollPosition);
            {
                QUI.BeginVertical(WindowSettings.CurrentPageContentWidth);
                {
                    switch(WindowSettings.currentPage)
                    {
                        case Page.General: DrawPageGeneral(); break;
                        case Page.UIElements: DrawPageUIElements(); break;
                        case Page.UIButtons: DrawPageUIButtons(); break;
                        case Page.UISounds: DrawPageUISounds(); break;
                        case Page.UICanvases: DrawPageUICanvases(); break;
                        case Page.AnimatorPresets: DrawPageAnimatorPresets(); break;
                        case Page.EditorSettings: DrawPageEditorSettings(); break;
                        case Page.Help: DrawPageHelp(); break;
                        case Page.About: DrawPageAbout(); break;
                    }
                    QUI.FlexibleSpace();
                }
                QUI.EndVertical();
                QUI.Space(SPACE_16);
            }
            QUI.EndScrollView();

            if(ExecuteInitPage) //an InitPage has been executed -> stop another InitPage from being executed (for the same Page) until at lease one of the triggering conditions is met again
            {
                PreviousPage = WindowSettings.currentPage;
                refreshData = false;
            }
        }

        void ResetPageView()
        {
            SectionScrollPosition = Vector2.zero; //reset scroll
            SectionLastScrollPosition = SectionScrollPosition;

            SearchPattern = ""; //reset search pattern
            SearchPatternAnimBool.value = false; //reset ui for search pattern

            NewCategoryName = ""; //reset new category name
            NewCategoryAnimBool.value = false; //reset ui for new category

            RenameCategoryName = ""; //reset rename category name
            RenameCategoryAnimBool.value = false; //reset ui for rename category
            RenameCategoryTargetCategoryName = ""; //reset rename category target category name
        }

        void DrawPageHeader(string title, QColor titleColor, string subtitle, QColor subtitleColor, QTexture iconQTexture)
        {
            QUI.Space(SPACE_2);
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), WindowSettings.CurrentPageContentWidth, WindowSettings.pageHeaderHeight);
            QUI.Space(-WindowSettings.pageHeaderHeight + (WindowSettings.pageHeaderHeight - WindowSettings.pageHeaderHeight * 0.8f) / 2);
            QUI.BeginHorizontal(WindowSettings.CurrentPageContentWidth, WindowSettings.pageHeaderHeight * 0.8f);
            {
                QUI.Space((WindowSettings.pageHeaderHeight - WindowSettings.pageHeaderHeight * 0.8f));
                QUI.BeginVertical((WindowSettings.CurrentPageContentWidth) / 2, WindowSettings.pageHeaderHeight * 0.8f);
                {
                    QUI.FlexibleSpace();
                    if(!title.IsNullOrEmpty())
                    {
                        QUI.Space(-SPACE_2);
                        QUI.SetGUIColor(titleColor.Color);
                        QUI.Label(title, PageHeaderTitleStyle, (WindowSettings.CurrentPageContentWidth) / 2, 26);
                        QUI.ResetColors();
                    }

                    if(!subtitle.IsNullOrEmpty())
                    {
                        QUI.Space(-SPACE_2);
                        QUI.SetGUIColor(subtitleColor.Color);
                        QUI.Label(subtitle, PageHeaderSubtitleStyle, (WindowSettings.CurrentPageContentWidth) / 2, 18);
                        QUI.ResetColors();
                    }
                    QUI.FlexibleSpace();
                }
                QUI.EndVertical();
                QUI.FlexibleSpace();
                QUI.DrawTexture(iconQTexture.texture, WindowSettings.pageHeaderHeight * 0.8f, WindowSettings.pageHeaderHeight * 0.8f);
                QUI.Space((WindowSettings.pageHeaderHeight - WindowSettings.pageHeaderHeight * 0.8f) / 2);
            }
            QUI.EndHorizontal();
        }

        void ToggleExpandCollapseSideBar() { WindowSettings.sidebarIsExpanded.target = !WindowSettings.sidebarIsExpanded.target; }

        void DrawStringList(List<string> list, float width, AnimBool aBool)
        {
            QUI.Space(SPACE_2 * aBool.faded);
            if(list.Count == 0)
            {
                if(SearchPatternAnimBool.value)
                {
                    DrawInfoMessage(InfoMessageName.NoResults.ToString(), width);
                }
                else
                {
                    QUI.BeginHorizontal(width);
                    {
                        QUI.Space(SPACE_8 * aBool.faded);
                        DrawInfoMessage(InfoMessageName.EmptyList.ToString(), width - SPACE_8);
                        QUI.Space(-20);
                        QUI.BeginVertical(16, 20);
                        {
                            QUI.Space(SPACE_4);
                            if(QUI.ButtonPlus())
                            {
                                Undo.RecordObject(DUIData.Instance, "Update List");
                                list.Add("");
                                QUI.SetDirty(DUIData.Instance);
                                AssetDatabase.SaveAssets();
                            }
                        }
                        QUI.EndVertical();
                        QUI.FlexibleSpace();
                    }
                    QUI.EndHorizontal();
                }
                return;
            }

            QUI.BeginVertical(width);
            {
                bool matchFoundInThisCategory = false;
                for(int i = 0; i < list.Count; i++)
                {
                    if(SearchPatternAnimBool.target) //a search pattern has been entered in the search box
                    {
                        if(!Regex.IsMatch(list[i], SearchPattern, RegexOptions.IgnoreCase))
                        {
                            continue; //this does not match the search pattern --> we do not show this name it
                        }
                        matchFoundInThisCategory = true;
                    }

                    QUI.BeginHorizontal(width);
                    {
                        QUI.Space(SPACE_8 * aBool.faded);
                        if(SearchPatternAnimBool.target) //in search mode, show a Label
                        {
                            QUI.Label(list[i], Style.Text.Normal, (width - SPACE_8));
                        }
                        else //search is disabled, show a text field
                        {
                            string tempString = list[i];
                            QUI.BeginChangeCheck();
                            tempString = EditorGUILayout.DelayedTextField(tempString, GUILayout.Width(width - SPACE_8 - 18));
                            if(QUI.EndChangeCheck())
                            {
                                if(list.Contains(tempString))
                                {
                                    QUI.DisplayDialog("Attention Required",
                                                      "There is another entry in this list with the same name.",
                                                      "Ok");
                                }
                                else
                                {
                                    Undo.RecordObject(DUIData.Instance, "Update List");
                                    list[i] = tempString;
                                    list.Sort();
                                    QUI.SetDirty(DUIData.Instance);
                                    AssetDatabase.SaveAssets();
                                }
                            }

                            if(QUI.ButtonMinus())
                            {
                                Undo.RecordObject(DUIData.Instance, "Update List");
                                list.RemoveAt(i);
                                QUI.SetDirty(DUIData.Instance);
                                AssetDatabase.SaveAssets();
                            }
                        }
                        QUI.FlexibleSpace();
                    }
                    QUI.EndHorizontal();
                    QUI.Space(SPACE_2 * aBool.faded - (SearchPatternAnimBool.target ? SPACE_2 : 0));
                }

                if(SearchPatternAnimBool.target)
                {
                    if(!matchFoundInThisCategory) //if a search pattern is active and no valid names were found for this category we let the developer know
                    {
                        DrawInfoMessage(InfoMessageName.NoResults.ToString(), width);
                    }
                    QUI.Space(18);
                }
                else //because a search pattern is active, we do not give the developer the option to create a new name
                {
                    QUI.BeginHorizontal(width);
                    {
                        QUI.Space(SPACE_8 * aBool.faded);
                        QUI.Space(width - SPACE_8 - 14);
                        if(QUI.ButtonPlus())
                        {
                            Undo.RecordObject(DUIData.Instance, "Update List");
                            list.Add("");
                            QUI.SetDirty(DUIData.Instance);
                            AssetDatabase.SaveAssets();
                        }
                    }
                    QUI.EndHorizontal();
                }
            }
            QUI.EndVertical();
        }
    }

    public class SourceWindow : QWindow
    {
#if dUI_SOURCE
        public static SourceWindow Instance;

        static bool _utility = true;
        static string _title = "DoozyUI - SOURCE";
        static bool _focus = true;

        static int minWidth = 600;
        static int minHeight = 600;



        [MenuItem("Tools/DoozyUI/Source", false, 1)]
        static void Init()
        {
            Instance = GetWindow<SourceWindow>(_utility, _title, _focus);
            Instance.SetupWindow();
        }

        void SetupWindow()
        {
            titleContent = new GUIContent(_title);
            UpdateWindowSize();
            CenterWindow();
        }

        void UpdateWindowSize()
        {
            minSize = new Vector2(minWidth, minHeight);
            maxSize = new Vector2(minSize.x, Screen.currentResolution.height);
            //maxSize = minSize;
        }

        private void OnEnable()
        {
            autoRepaintOnSceneChange = true;
            requiresContantRepaint = true;
        }

        private void OnGUI()
        {
            UpdateWindowSize();

            float width = position.width;

            QLabel.text = "Pre-release Check";
            QLabel.style = Style.Text.Normal;
            QUI.Label(QLabel);

            QUI.Space(SPACE_2);

            DrawDeleteFile(DUI.RELATIVE_PATH_CONTROL_PANEL_WINDOW_SETTINGS, "ControlPanelWindowSettings", width, ".asset");
            QUI.Space(SPACE_4);
            DrawConvertDUIDataToSourceFiles(DUI.RELATIVE_PATH_DUIDATA, "DUIData", ".asset", width);
            QUI.Space(SPACE_4);
            DrawDeleteFile(DUI.RELATIVE_PATH_SETTINGS, "DUISettings", width, ".asset");
            QUI.Space(SPACE_4);
            DrawPerformDatabaseUpgrade(width);

            QUI.Space(SPACE_16);
            if(QUI.GhostButton("Export DoozyUI", QColors.Color.Blue, 200, 20, false))
            {
                PackageExporter.PackageExporter.ExportAsset(PackageExporter.PackageExporter.ProductName.DoozyUI);
            }
            QUI.Space(SPACE_4);
            if(QUI.GhostButton("Export UIDrawer", QColors.Color.Blue, 200, 20, false))
            {
                PackageExporter.PackageExporter.ExportAsset(PackageExporter.PackageExporter.ProductName.UIDrawer);
            }
        }

        void DrawFixFolderButton(string folderName, string relativePath, float width)
        {
            QUI.BeginHorizontal(width);
            {
                QLabel.text = "Find";
                QLabel.style = Style.Text.Button;
                if(QUI.GhostButton(QLabel.text, QColors.Color.Gray, QLabel.x + 16, 18))
                {
                    // Load object
                    UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(relativePath.Substring(0, relativePath.Length - 1), typeof(UnityEngine.Object));

                    // Select the object in the project folder
                    Selection.activeObject = obj;

                    // Also flash the folder yellow to highlight it
                    EditorGUIUtility.PingObject(obj);
                }

                QUI.Space(SPACE_2);

                QLabel.text = "Fix Folder: " + folderName;
                QLabel.style = Style.Text.Button;
                if(QUI.GhostButton(QLabel.text, QColors.Color.Gray, QLabel.x + 24, 18))
                {
                    DeleteAllFilesUnderTargetPath(relativePath);
                }

                if(CheckThatFolderExists(relativePath))
                {

                }
                else if(CheckThatFolderIsEmpty(relativePath))
                {

                }
                else
                {
                    QUI.Space(SPACE_4);
                    QUI.BeginVertical(16, 16);
                    {
                        QUI.Space(2);
                        QUI.DrawTexture(QResources.iconOk.texture, 14, 14);
                    }
                    QUI.EndVertical();
                }

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }

        bool CheckThatFolderExists(string relativePath)
        {
            if(!System.IO.Directory.Exists(relativePath))
            {
                QUI.Space(SPACE_4);

                QLabel.text = "This folder does not exist. Click to create it.";
                QLabel.style = Style.Text.Help;

                QUI.BeginVertical(16, 16);
                {
                    QUI.Space(2);
                    QUI.DrawTexture(QResources.iconError.texture, 14, 14);
                }
                QUI.EndVertical();

                QUI.Space(-SPACE_2);

                QUI.Label(QLabel);

                return true;
            }
            return false;
        }
        bool CheckThatFolderIsEmpty(string relativePath)
        {
            var fileNames = System.IO.Directory.GetFiles(relativePath);

            if(fileNames.Length > 0)
            {
                QUI.Space(SPACE_4);

                QLabel.text = "This folder contains files. Click to delete them.";
                QLabel.style = Style.Text.Help;

                QUI.BeginVertical(16, 16);
                {
                    QUI.Space(2);
                    QUI.DrawTexture(QResources.iconWarning.texture, 14, 14);
                }
                QUI.EndVertical();

                QUI.Space(-SPACE_2);

                QUI.Label(QLabel);

                return true;
            }

            return false;
        }
        void DeleteAllFilesUnderTargetPath(string relativePath)
        {
            if(!System.IO.Directory.Exists(relativePath))
            {
                System.IO.Directory.CreateDirectory(relativePath);
            }
            else
            {
                var fileNames = System.IO.Directory.GetFiles(relativePath);

                for(int i = 0; i < fileNames.Length; i++)
                {
                    System.IO.File.Delete(fileNames[i]);
                }

                System.IO.Directory.Delete(relativePath);
                System.IO.Directory.CreateDirectory(relativePath);
            }
            AssetDatabase.Refresh();
        }

        void DrawDeleteFile(string relativePath, string fileName, float width, string fileExtension = ".cs")
        {
            QUI.BeginHorizontal(width);
            {
                QLabel.text = "Find";
                QLabel.style = Style.Text.Button;
                if(QUI.GhostButton(QLabel.text, QColors.Color.Gray, QLabel.x + 16, 18))
                {
                    // Load object
                    UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(relativePath + fileName + fileExtension, typeof(UnityEngine.Object));

                    // Select the object in the project folder
                    Selection.activeObject = obj;

                    // Also flash the folder yellow to highlight it
                    EditorGUIUtility.PingObject(obj);
                }

                QUI.Space(SPACE_2);

                QLabel.text = "Delete: " + fileName;
                QLabel.style = Style.Text.Button;
                if(QUI.GhostButton(QLabel.text, QColors.Color.Gray, QLabel.x + 24, 18))
                {
                    AssetDatabase.DeleteAsset(relativePath + fileName + fileExtension);
                }

                if(CheckThatFileExists(relativePath, fileName, fileExtension))
                {

                }
                else
                {
                    QUI.Space(SPACE_4);
                    QUI.BeginVertical(16, 16);
                    {
                        QUI.Space(2);
                        QUI.DrawTexture(QResources.iconOk.texture, 14, 14);
                    }
                    QUI.EndVertical();
                }

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        bool CheckThatFileExists(string relativePath, string fileName, string fileExtension)
        {
            if(System.IO.File.Exists(relativePath + fileName + fileExtension))
            {
                QUI.Space(SPACE_4);

                QLabel.text = "File exists.";
                QLabel.style = Style.Text.Help;

                QUI.BeginVertical(16, 16);
                {
                    QUI.Space(2);
                    QUI.DrawTexture(QResources.iconError.texture, 14, 14);
                }
                QUI.EndVertical();

                QUI.Space(-SPACE_2);

                QUI.Label(QLabel);

                return true;
            }

            return false;
        }

        void DrawConvertDUIDataToSourceFiles(string relativePath, string fileName, string fileExtension, float width)
        {
            QUI.BeginHorizontal(width);
            {
                QLabel.text = "Find";
                QLabel.style = Style.Text.Button;
                if(QUI.GhostButton(QLabel.text, QColors.Color.Gray, QLabel.x + 16, 18))
                {
                    // Load object
                    UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(relativePath + fileName + fileExtension, typeof(UnityEngine.Object));

                    // Select the object in the project folder
                    Selection.activeObject = obj;

                    // Also flash the folder yellow to highlight it
                    EditorGUIUtility.PingObject(obj);
                }

                QUI.Space(SPACE_2);

                QLabel.text = "Create Database Files from: " + fileName;
                QLabel.style = Style.Text.Button;
                if(QUI.GhostButton(QLabel.text, QColors.Color.Gray, QLabel.x + 24, 18))
                {
                    DUIData D = DUIData.Instance;

                    //UIElements Database Files
                    if(D.DatabaseUIElements != null && D.DatabaseUIElements.categories != null && D.DatabaseUIElements.categories.Count > 0)
                    {
                        for(int i = 0; i < D.DatabaseUIElements.categories.Count; i++)
                        {
                            if(D.DatabaseUIElements.categories[i].categoryName.Equals(DUI.UNCATEGORIZED_CATEGORY_NAME)) { continue; }
                            D.CreateUIElementsDatabaseAssetFile(D.DatabaseUIElements.categories[i].categoryName, D.DatabaseUIElements.categories[i].itemNames);
                        }
                    }

                    //UIButtons Database Files
                    if(D.DatabaseUIButtons != null && D.DatabaseUIButtons.categories != null && D.DatabaseUIButtons.categories.Count > 0)
                    {
                        for(int i = 0; i < D.DatabaseUIButtons.categories.Count; i++)
                        {
                            if(D.DatabaseUIButtons.categories[i].categoryName.Equals(DUI.UNCATEGORIZED_CATEGORY_NAME)) { continue; }
                            D.CreateUIButtonsDatabaseAssetFile(D.DatabaseUIButtons.categories[i].categoryName, D.DatabaseUIButtons.categories[i].itemNames);
                        }
                    }

                    AssetDatabase.DeleteAsset(relativePath + fileName + fileExtension); //delete DUIData

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                if(CheckThatFileExists(relativePath, fileName, fileExtension))
                {

                }
                else
                {
                    QUI.Space(SPACE_4);
                    QUI.BeginVertical(16, 16);
                    {
                        QUI.Space(2);
                        QUI.DrawTexture(QResources.iconOk.texture, 14, 14);
                    }
                    QUI.EndVertical();
                }

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }

        void DrawPerformDatabaseUpgrade(float width)
        {
            QUI.BeginHorizontal(width);
            {
                QLabel.text = "Find";
                QLabel.style = Style.Text.Button;
                if(QUI.GhostButton(QLabel.text, QColors.Color.Gray, QLabel.x + 16, 18))
                {
                    // Load object
                    UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(DUI.RELATIVE_PATH_VERSION + "DUIVersion" + ".asset", typeof(UnityEngine.Object));

                    // Select the object in the project folder
                    Selection.activeObject = obj;

                    // Also flash the folder yellow to highlight it
                    EditorGUIUtility.PingObject(obj);
                }

                QUI.Space(SPACE_2);

                QLabel.text = "DoozyUI " + Internal.DUIVersion.Instance.version;
                QLabel.style = Style.Text.Button;
                QUI.Label(QLabel);

                QUI.Space(SPACE_2);

                QLabel.text = "Perform Database Upgrade";
                QLabel.style = Style.Text.Button;
                QUI.BeginChangeCheck();
                Internal.DUIVersion.Instance.performDatabaseUpgrade = QUI.QToggle("Perform Database Upgrade", Internal.DUIVersion.Instance.performDatabaseUpgrade);
                if(QUI.EndChangeCheck())
                {
                    QUI.SetDirty(Internal.DUIVersion.Instance);
                    AssetDatabase.SaveAssets();
                }


                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
#endif
    }
}
