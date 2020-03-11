// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using UnityEditor;
using QuickEditor;

namespace DoozyUI
{
    public partial class DUIResources
    {

        private static string _IMAGES;
        public static string IMAGES { get { if(string.IsNullOrEmpty(_IMAGES)) { _IMAGES = DUI.PATH + "/Images/"; } return _IMAGES; } }

        private static string _BARS;
        public static string BARS { get { if(string.IsNullOrEmpty(_BARS)) { _BARS = IMAGES + "Bars/"; } return _BARS; } }

        private static string _BUTTONS;
        public static string BUTTONS { get { if(string.IsNullOrEmpty(_BUTTONS)) { _BUTTONS = IMAGES + "Buttons/"; } return _BUTTONS; } }

        private static string _CONTROLPANEL;
        public static string CONTROLPANEL { get { if(string.IsNullOrEmpty(_CONTROLPANEL)) { _CONTROLPANEL = IMAGES + "ControlPanel/"; } return _CONTROLPANEL; } }

        private static string _HEADERS;
        public static string HEADERS { get { if(string.IsNullOrEmpty(_HEADERS)) { _HEADERS = IMAGES + "Headers/"; } return _HEADERS; } }

        private static string _ICONS;
        public static string ICONS { get { if(string.IsNullOrEmpty(_ICONS)) { _ICONS = IMAGES + "Icons/"; } return _ICONS; } }

        private static string _PAGEICONS;
        public static string PAGEICONS { get { if(string.IsNullOrEmpty(_PAGEICONS)) { _PAGEICONS = IMAGES + "PageIcons/"; } return _PAGEICONS; } }

        private static string _SIDEBUTTONS;
        public static string SIDEBUTTONS { get { if(string.IsNullOrEmpty(_SIDEBUTTONS)) { _SIDEBUTTONS = IMAGES + "SideButtons/"; } return _SIDEBUTTONS; } }

        private static string _SIDEBARLOGO;
        public static string SIDEBARLOGO { get { if(string.IsNullOrEmpty(_SIDEBARLOGO)) { _SIDEBARLOGO = IMAGES + "SideBarLogo/"; } return _SIDEBARLOGO; } }

        //BARS
        public static QTexture presetCaret0 = new QTexture(BARS, "presetCaret0" + QResources.IsProSkinTag);
        public static QTexture presetCaret1 = new QTexture(BARS, "presetCaret1" + QResources.IsProSkinTag);
        public static QTexture presetCaret2 = new QTexture(BARS, "presetCaret2" + QResources.IsProSkinTag);
        public static QTexture presetCaret3 = new QTexture(BARS, "presetCaret3" + QResources.IsProSkinTag);
        public static QTexture presetCaret4 = new QTexture(BARS, "presetCaret4" + QResources.IsProSkinTag);
        public static QTexture presetCaret5 = new QTexture(BARS, "presetCaret5" + QResources.IsProSkinTag);
        public static QTexture presetCaret6 = new QTexture(BARS, "presetCaret6" + QResources.IsProSkinTag);
        public static QTexture presetCaret7 = new QTexture(BARS, "presetCaret7" + QResources.IsProSkinTag);
        public static QTexture presetCaret8 = new QTexture(BARS, "presetCaret8" + QResources.IsProSkinTag);
        public static QTexture presetCaret9 = new QTexture(BARS, "presetCaret9" + QResources.IsProSkinTag);
        public static QTexture presetCaret10 = new QTexture(BARS, "presetCaret10" + QResources.IsProSkinTag);

        public static QTexture moveCaret0 = new QTexture(BARS, "moveCaret0" + QResources.IsProSkinTag);
        public static QTexture moveCaret1 = new QTexture(BARS, "moveCaret1" + QResources.IsProSkinTag);
        public static QTexture moveCaret2 = new QTexture(BARS, "moveCaret2" + QResources.IsProSkinTag);
        public static QTexture moveCaret3 = new QTexture(BARS, "moveCaret3" + QResources.IsProSkinTag);
        public static QTexture moveCaret4 = new QTexture(BARS, "moveCaret4" + QResources.IsProSkinTag);
        public static QTexture moveCaret5 = new QTexture(BARS, "moveCaret5" + QResources.IsProSkinTag);
        public static QTexture moveCaret6 = new QTexture(BARS, "moveCaret6" + QResources.IsProSkinTag);
        public static QTexture moveCaret7 = new QTexture(BARS, "moveCaret7" + QResources.IsProSkinTag);
        public static QTexture moveCaret8 = new QTexture(BARS, "moveCaret8" + QResources.IsProSkinTag);
        public static QTexture moveCaret9 = new QTexture(BARS, "moveCaret9" + QResources.IsProSkinTag);
        public static QTexture moveCaret10 = new QTexture(BARS, "moveCaret10" + QResources.IsProSkinTag);

        public static QTexture rotateCaret0 = new QTexture(BARS, "rotateCaret0" + QResources.IsProSkinTag);
        public static QTexture rotateCaret1 = new QTexture(BARS, "rotateCaret1" + QResources.IsProSkinTag);
        public static QTexture rotateCaret2 = new QTexture(BARS, "rotateCaret2" + QResources.IsProSkinTag);
        public static QTexture rotateCaret3 = new QTexture(BARS, "rotateCaret3" + QResources.IsProSkinTag);
        public static QTexture rotateCaret4 = new QTexture(BARS, "rotateCaret4" + QResources.IsProSkinTag);
        public static QTexture rotateCaret5 = new QTexture(BARS, "rotateCaret5" + QResources.IsProSkinTag);
        public static QTexture rotateCaret6 = new QTexture(BARS, "rotateCaret6" + QResources.IsProSkinTag);
        public static QTexture rotateCaret7 = new QTexture(BARS, "rotateCaret7" + QResources.IsProSkinTag);
        public static QTexture rotateCaret8 = new QTexture(BARS, "rotateCaret8" + QResources.IsProSkinTag);
        public static QTexture rotateCaret9 = new QTexture(BARS, "rotateCaret9" + QResources.IsProSkinTag);
        public static QTexture rotateCaret10 = new QTexture(BARS, "rotateCaret10" + QResources.IsProSkinTag);

        public static QTexture scaleCaret0 = new QTexture(BARS, "scaleCaret0" + QResources.IsProSkinTag);
        public static QTexture scaleCaret1 = new QTexture(BARS, "scaleCaret1" + QResources.IsProSkinTag);
        public static QTexture scaleCaret2 = new QTexture(BARS, "scaleCaret2" + QResources.IsProSkinTag);
        public static QTexture scaleCaret3 = new QTexture(BARS, "scaleCaret3" + QResources.IsProSkinTag);
        public static QTexture scaleCaret4 = new QTexture(BARS, "scaleCaret4" + QResources.IsProSkinTag);
        public static QTexture scaleCaret5 = new QTexture(BARS, "scaleCaret5" + QResources.IsProSkinTag);
        public static QTexture scaleCaret6 = new QTexture(BARS, "scaleCaret6" + QResources.IsProSkinTag);
        public static QTexture scaleCaret7 = new QTexture(BARS, "scaleCaret7" + QResources.IsProSkinTag);
        public static QTexture scaleCaret8 = new QTexture(BARS, "scaleCaret8" + QResources.IsProSkinTag);
        public static QTexture scaleCaret9 = new QTexture(BARS, "scaleCaret9" + QResources.IsProSkinTag);
        public static QTexture scaleCaret10 = new QTexture(BARS, "scaleCaret10" + QResources.IsProSkinTag);

        public static QTexture fadeCaret0 = new QTexture(BARS, "fadeCaret0" + QResources.IsProSkinTag);
        public static QTexture fadeCaret1 = new QTexture(BARS, "fadeCaret1" + QResources.IsProSkinTag);
        public static QTexture fadeCaret2 = new QTexture(BARS, "fadeCaret2" + QResources.IsProSkinTag);
        public static QTexture fadeCaret3 = new QTexture(BARS, "fadeCaret3" + QResources.IsProSkinTag);
        public static QTexture fadeCaret4 = new QTexture(BARS, "fadeCaret4" + QResources.IsProSkinTag);
        public static QTexture fadeCaret5 = new QTexture(BARS, "fadeCaret5" + QResources.IsProSkinTag);
        public static QTexture fadeCaret6 = new QTexture(BARS, "fadeCaret6" + QResources.IsProSkinTag);
        public static QTexture fadeCaret7 = new QTexture(BARS, "fadeCaret7" + QResources.IsProSkinTag);
        public static QTexture fadeCaret8 = new QTexture(BARS, "fadeCaret8" + QResources.IsProSkinTag);
        public static QTexture fadeCaret9 = new QTexture(BARS, "fadeCaret9" + QResources.IsProSkinTag);
        public static QTexture fadeCaret10 = new QTexture(BARS, "fadeCaret10" + QResources.IsProSkinTag);

        //BUTTONS
        public static QTexture buttonLinkedToUINotification = new QTexture(BUTTONS, "buttonLinkedToUINotification" + QResources.IsProSkinTag);

        //CONTROL PANEL
        public static QTexture buttonEditorInPlayMode = new QTexture(CONTROLPANEL, "buttonEditorInPlayMode" + QResources.IsProSkinTag);
        public static QTexture buttonEditorIsCompiling = new QTexture(CONTROLPANEL, "buttonEditorIsCompiling" + QResources.IsProSkinTag);

        public static QTexture buttonPlayMakerEnabled = new QTexture(CONTROLPANEL, "buttonPlayMakerEnabled" + QResources.IsProSkinTag);
        public static QTexture buttonPlayMakerDisabled = new QTexture(CONTROLPANEL, "buttonPlayMakerDisabled" + QResources.IsProSkinTag);
        public static QTexture buttonMasterAudioEnabled = new QTexture(CONTROLPANEL, "buttonMasterAudioEnabled" + QResources.IsProSkinTag);
        public static QTexture buttonMasterAudioDisabled = new QTexture(CONTROLPANEL, "buttonMasterAudioDisabled" + QResources.IsProSkinTag);
        public static QTexture buttonEnergyBarToolkitEnabled = new QTexture(CONTROLPANEL, "buttonEnergyBarToolkitEnabled" + QResources.IsProSkinTag);
        public static QTexture buttonEnergyBarToolkitDisabled = new QTexture(CONTROLPANEL, "buttonEnergyBarToolkitDisabled" + QResources.IsProSkinTag);
        public static QTexture buttonTextMeshProEnabled = new QTexture(CONTROLPANEL, "buttonTextMeshProEnabled" + QResources.IsProSkinTag);
        public static QTexture buttonTextMeshProDisabled = new QTexture(CONTROLPANEL, "buttonTextMeshProDisabled" + QResources.IsProSkinTag);

        public static QTexture buttonUINavigationEnabled = new QTexture(CONTROLPANEL, "buttonUINavigationEnabled" + QResources.IsProSkinTag);
        public static QTexture buttonUINavigationDisabled = new QTexture(CONTROLPANEL, "buttonUINavigationDisabled" + QResources.IsProSkinTag);
        public static QTexture buttonOrientationManagerEnabled = new QTexture(CONTROLPANEL, "buttonOrientationManagerEnabled" + QResources.IsProSkinTag);
        public static QTexture buttonOrientationManagerDisabled = new QTexture(CONTROLPANEL, "buttonOrientationManagerDisabled" + QResources.IsProSkinTag);
        public static QTexture buttonSceneLoaderEnabled = new QTexture(CONTROLPANEL, "buttonSceneLoaderEnabled" + QResources.IsProSkinTag);
        public static QTexture buttonSceneLoaderDisabled = new QTexture(CONTROLPANEL, "buttonSceneLoaderDisabled" + QResources.IsProSkinTag);

        public static QTexture buttonUIDrawerEnabled = new QTexture(CONTROLPANEL, "buttonUIDrawerEnabled" + QResources.IsProSkinTag);
        public static QTexture buttonUIDrawerDisabled = new QTexture(CONTROLPANEL, "buttonUIDrawerDisabled" + QResources.IsProSkinTag);

        public static QTexture aboutVersion = new QTexture(CONTROLPANEL, "aboutVersion" + QResources.IsProSkinTag); //400x200

        //HEADERS
        public static QTexture headerUIElement = new QTexture(HEADERS, "headerUIElement" + QResources.IsProSkinTag);
        public static QTexture headerUIButton = new QTexture(HEADERS, "headerUIButton" + QResources.IsProSkinTag);
        public static QTexture headerUICanvas = new QTexture(HEADERS, "headerUICanvas" + QResources.IsProSkinTag);
        public static QTexture headerUINotification = new QTexture(HEADERS, "headerUINotification" + QResources.IsProSkinTag);
        public static QTexture headerUITrigger = new QTexture(HEADERS, "headerUITrigger" + QResources.IsProSkinTag);
        public static QTexture headerUIEffect = new QTexture(HEADERS, "headerUIEffect" + QResources.IsProSkinTag);
        public static QTexture headerUIToggle = new QTexture(HEADERS, "headerUIToggle" + QResources.IsProSkinTag);
        public static QTexture headerPlayMakerEventDispatcher = new QTexture(HEADERS, "headerPlayMakerEventDispatcher" + QResources.IsProSkinTag);
        public static QTexture headerOrientationManager = new QTexture(HEADERS, "headerOrientationManager" + QResources.IsProSkinTag);
        public static QTexture headerUIManager = new QTexture(HEADERS, "headerUIManager" + QResources.IsProSkinTag);
        public static QTexture headerSceneLoader = new QTexture(HEADERS, "headerSceneLoader" + QResources.IsProSkinTag);
        public static QTexture headerSoundy = new QTexture(HEADERS, "headerSoundy" + QResources.IsProSkinTag);
        public static QTexture headerUINotificationManager = new QTexture(HEADERS, "headerUINotificationManager" + QResources.IsProSkinTag);
        public static QTexture headerTouchManager = new QTexture(HEADERS, "headerTouchManager" + QResources.IsProSkinTag);
        public static QTexture headerGestureDetector = new QTexture(HEADERS, "headerGestureDetector" + QResources.IsProSkinTag);

        //ICONS
        public static QTexture iconUIManager = new QTexture(ICONS, "iconUIManager");
        public static QTexture iconSceneLoader = new QTexture(ICONS, "iconSceneLoader");
        public static QTexture iconSoundy = new QTexture(ICONS, "iconSoundy");
        public static QTexture iconOrientationManager = new QTexture(ICONS, "iconOrientationManager");
        public static QTexture iconUINotificationManager = new QTexture(ICONS, "iconUINotificationManager");

        public static QTexture iconUICanvas = new QTexture(ICONS, "iconUICanvas");
        public static QTexture iconUIButton = new QTexture(ICONS, "iconUIButton");
        public static QTexture iconUIElement = new QTexture(ICONS, "iconUIElement");
        public static QTexture iconUINotification = new QTexture(ICONS, "iconUINotification");
        public static QTexture iconUITrigger = new QTexture(ICONS, "iconUITrigger");
        public static QTexture iconUIEffect = new QTexture(ICONS, "iconUIEffect");
        public static QTexture iconUIToggle = new QTexture(ICONS, "iconUIToggle");
        public static QTexture iconUINavigation = new QTexture(ICONS, "iconUINavigation");

        public static QTexture iconUISound = new QTexture(ICONS, "iconUISound");
        public static QTexture iconUIAnimator = new QTexture(ICONS, "iconUIAnimator");
        public static QTexture iconInOutAnimatorPreset = new QTexture(ICONS, "iconInOutAnimatorPreset");
        public static QTexture iconLoopAnimatorPreset = new QTexture(ICONS, "iconLoopAnimatorPreset");
        public static QTexture iconPunchAnimatorPreset = new QTexture(ICONS, "iconPunchAnimatorPreset");
        public static QTexture iconDatabase = new QTexture(ICONS, "iconDatabase");

        public static QTexture iconDUISettings = new QTexture(ICONS, "iconDUISettings");

        public static QTexture iconPlayMakerEventDispatcher = new QTexture(ICONS, "iconPlayMakerEventDispatcher");

        //MINI ICONS
        public static QTexture miniIconToggleOn = new QTexture(ICONS, "miniIconToggleOn" + QResources.IsProSkinTag); //32x32
        public static QTexture miniIconToggleOff = new QTexture(ICONS, "miniIconToggleOff" + QResources.IsProSkinTag); //32x32
        public static QTexture miniIconShow = new QTexture(ICONS, "miniIconShow" + QResources.IsProSkinTag); //32x32
        public static QTexture miniIconHide = new QTexture(ICONS, "miniIconHide" + QResources.IsProSkinTag); //32x32
        public static QTexture miniIconOrientationPortrait = new QTexture(ICONS, "miniIconOrientationPortrait" + QResources.IsProSkinTag); //160x32
        public static QTexture miniIconOrientationLandscape = new QTexture(ICONS, "miniIconOrientationLandscape" + QResources.IsProSkinTag); //160x32
        public static QTexture miniIconOrientationUnknown = new QTexture(ICONS, "miniIconOrientationUnknown" + QResources.IsProSkinTag); //160x32

        //PAGE ICONS
        public static QTexture pageIconGeneral = new QTexture(PAGEICONS, "pageIconGeneral");
        public static QTexture pageIconUIElements = new QTexture(PAGEICONS, "pageIconUIElements");
        public static QTexture pageIconUIButtons = new QTexture(PAGEICONS, "pageIconUIButtons");
        public static QTexture pageIconUISounds = new QTexture(PAGEICONS, "pageIconUISounds");
        public static QTexture pageIconUICanvases = new QTexture(PAGEICONS, "pageIconUICanvases");
        public static QTexture pageIconAnimatorPresets = new QTexture(PAGEICONS, "pageIconAnimatorPresets");
        public static QTexture pageIconEditorSettings = new QTexture(PAGEICONS, "pageIconEditorSettings");
        public static QTexture pageIconHelp = new QTexture(PAGEICONS, "pageIconHelp");
        public static QTexture pageIconAbout = new QTexture(PAGEICONS, "pageIconAbout");

        //SIDE BUTTONS
        public static QTexture sideButtonExpandSideBar = new QTexture(SIDEBUTTONS, "sideButtonExpandSideBar" + QResources.IsProSkinTag);
        public static QTexture sideButtonCollapseSideBar = new QTexture(SIDEBUTTONS, "sideButtonCollapseSideBar" + QResources.IsProSkinTag);

        public static QTexture sideButtonControlPanel = new QTexture(SIDEBUTTONS, "sideButtonControlPanel" + QResources.IsProSkinTag);
        public static QTexture sideButtonControlPanelSelected = new QTexture(SIDEBUTTONS, "sideButtonControlPanel" + QResources.IsProSkinTag + "Selected");

        public static QTexture sideButtonUIElements = new QTexture(SIDEBUTTONS, "sideButtonUIElements" + QResources.IsProSkinTag);
        public static QTexture sideButtonUIElementsSelected = new QTexture(SIDEBUTTONS, "sideButtonUIElements" + QResources.IsProSkinTag + "Selected");

        public static QTexture sideButtonUIButtons = new QTexture(SIDEBUTTONS, "sideButtonUIButtons" + QResources.IsProSkinTag);
        public static QTexture sideButtonUIButtonsSelected = new QTexture(SIDEBUTTONS, "sideButtonUIButtons" + QResources.IsProSkinTag + "Selected");

        public static QTexture sideButtonUISounds = new QTexture(SIDEBUTTONS, "sideButtonUISounds" + QResources.IsProSkinTag);
        public static QTexture sideButtonUISoundsSelected = new QTexture(SIDEBUTTONS, "sideButtonUISounds" + QResources.IsProSkinTag + "Selected");

        public static QTexture sideButtonUICanvases = new QTexture(SIDEBUTTONS, "sideButtonUICanvases" + QResources.IsProSkinTag);
        public static QTexture sideButtonUICanvasesSelected = new QTexture(SIDEBUTTONS, "sideButtonUICanvases" + QResources.IsProSkinTag + "Selected");

        public static QTexture sideButtonAnimatorPresets = new QTexture(SIDEBUTTONS, "sideButtonAnimatorPresets" + QResources.IsProSkinTag);
        public static QTexture sideButtonAnimatorPresetsSelected = new QTexture(SIDEBUTTONS, "sideButtonAnimatorPresets" + QResources.IsProSkinTag + "Selected");

        public static QTexture sideButtonEditorSettings = new QTexture(SIDEBUTTONS, "sideButtonEditorSettings" + QResources.IsProSkinTag);
        public static QTexture sideButtonEditorSettingsSelected = new QTexture(SIDEBUTTONS, "sideButtonEditorSettings" + QResources.IsProSkinTag + "Selected");

        public static QTexture sideButtonHelp = new QTexture(SIDEBUTTONS, "sideButtonHelp" + QResources.IsProSkinTag);
        public static QTexture sideButtonHelpSelected = new QTexture(SIDEBUTTONS, "sideButtonHelp" + QResources.IsProSkinTag + "Selected");

        public static QTexture sideButtonAbout = new QTexture(SIDEBUTTONS, "sideButtonAbout" + QResources.IsProSkinTag);
        public static QTexture sideButtonAboutSelected = new QTexture(SIDEBUTTONS, "sideButtonAbout" + QResources.IsProSkinTag + "Selected");

        public static QTexture sideButtonTwitter = new QTexture(SIDEBUTTONS, "sideButtonTwitter" + QResources.IsProSkinTag);
        public static QTexture sideButtonFacebook = new QTexture(SIDEBUTTONS, "sideButtonFacebook" + QResources.IsProSkinTag);
        public static QTexture sideButtonYoutube = new QTexture(SIDEBUTTONS, "sideButtonYoutube" + QResources.IsProSkinTag);


        //SIDEBAR LOGO
        public static QTexture sidebarLogo0 = new QTexture(SIDEBARLOGO, "sidebarLogo0" + QResources.IsProSkinTag);
        public static QTexture sidebarLogo1 = new QTexture(SIDEBARLOGO, "sidebarLogo1" + QResources.IsProSkinTag);
        public static QTexture sidebarLogo2 = new QTexture(SIDEBARLOGO, "sidebarLogo2" + QResources.IsProSkinTag);
        public static QTexture sidebarLogo3 = new QTexture(SIDEBARLOGO, "sidebarLogo3" + QResources.IsProSkinTag);
        public static QTexture sidebarLogo4 = new QTexture(SIDEBARLOGO, "sidebarLogo4" + QResources.IsProSkinTag);
        public static QTexture sidebarLogo5 = new QTexture(SIDEBARLOGO, "sidebarLogo5" + QResources.IsProSkinTag);
        public static QTexture sidebarLogo6 = new QTexture(SIDEBARLOGO, "sidebarLogo6" + QResources.IsProSkinTag);
        public static QTexture sidebarLogo7 = new QTexture(SIDEBARLOGO, "sidebarLogo7" + QResources.IsProSkinTag);
        public static QTexture sidebarLogo8 = new QTexture(SIDEBARLOGO, "sidebarLogo8" + QResources.IsProSkinTag);
        public static QTexture sidebarLogo9 = new QTexture(SIDEBARLOGO, "sidebarLogo9" + QResources.IsProSkinTag);
        public static QTexture sidebarLogo10 = new QTexture(SIDEBARLOGO, "sidebarLogo10" + QResources.IsProSkinTag);

        ////////////////////////////////////////// OLD 


        private static Font m_Sansation;
        public static Font Sansation { get { if(m_Sansation == null) { m_Sansation = AssetDatabase.LoadAssetAtPath<Font>(DUI.PATH + "/Fonts/" + "Sansation-Regular.ttf"); } return m_Sansation; } }

        private static string m_ImagesPath;
        public static string ImagesPath { get { if(string.IsNullOrEmpty(m_ImagesPath)) { m_ImagesPath = DUI.PATH + "/Images/"; } return m_ImagesPath; } }


        //NOTIFICATION WINDOW
        private static string m_ImagesNotificationWindowPath;
        public static string ImagesNotificationWindowPath { get { if(string.IsNullOrEmpty(m_ImagesNotificationWindowPath)) { m_ImagesNotificationWindowPath = ImagesPath + "NotificationWindow/"; } return m_ImagesNotificationWindowPath; } }

        public static QTexture notificationWindowBackground = new QTexture(ImagesNotificationWindowPath, "notificationWindowBackground");
        public static QTexture notificationWindowButtonOk = new QTexture(ImagesNotificationWindowPath, "notificationWindowButtonOk");
        public static QTexture notificationWindowButtonCancel = new QTexture(ImagesNotificationWindowPath, "notificationWindowButtonCancel");
        public static QTexture notificationWindowButtonYes = new QTexture(ImagesNotificationWindowPath, "notificationWindowButtonYes");
        public static QTexture notificationWindowButtonNo = new QTexture(ImagesNotificationWindowPath, "notificationWindowButtonNo");
        public static QTexture notificationWindowButtonContinue = new QTexture(ImagesNotificationWindowPath, "notificationWindowButtonContinue");
    }
}
