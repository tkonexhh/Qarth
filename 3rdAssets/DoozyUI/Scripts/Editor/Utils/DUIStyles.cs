// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using QuickEngine.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace DoozyUI
{
    public class DUIStyles
    {

        public enum SideButton
        {
            CollapseSideBar,
            ExpandSideBar,

            ControlPanel,
            ControlPanelSelected,

            UIElements,
            UIElementsSelected,

            UIButtons,
            UIButtonsSelected,

            UISounds,
            UISoundsSelected,

            UICanvases,
            UICanvasesSelected,

            AnimatorPresets,
            AnimatorPresetsSelected,

            EditorSettings,
            EditorSettingsSelected,

            Help,
            HelpSelected,

            About,
            AboutSelected,

            Twitter,
            Facebook,
            Youtube,
        }

        public enum NotificationWindow
        {
            NotificationTitleBlack,
            NotificationMessageBlack,
            NotificationButtonOk,
            NotificationButtonCancel,
            NotificationButtonYes,
            NotificationButtonNo,
            NotificationButtonContinue
        }

        private static GUISkin skin;
        public static GUISkin Skin { get { if(skin == null) { skin = GetSkin(); } return skin; } }

        /// <summary>
        /// Returns a style that has been added to the skin.
        /// </summary>
        public static GUIStyle GetStyle(string styleName) { return Skin.GetStyle(styleName); }
        /// <summary>
        /// Returns a style that has been added to the skin.
        /// This method is to be used paired with the enums in the Style class.
        /// </summary>
        public static GUIStyle GetStyle<T>(T styleName) { return Skin.GetStyle(QStyles.GetStyleName(styleName)); }


        public static GUIStyle GetStyle(NotificationWindow styleName) { return Skin.GetStyle(styleName.ToString()); }

        private static GUISkin GetSkin()
        {
            GUISkin skin = ScriptableObject.CreateInstance<GUISkin>();
            List<GUIStyle> styles = new List<GUIStyle>();
            styles.AddRange(SideButtonStyles());

            styles.AddRange(NotificationWindowStyles());
            skin.customStyles = styles.ToArray();
            return skin;
        }

        private static void UpdateSkin()
        {
            skin = null;
            skin = GetSkin();
        }

        public static void AddStyle(GUIStyle style)
        {
            if(style == null) { return; }
            List<GUIStyle> customStyles = new List<GUIStyle>();
            customStyles.AddRange(Skin.customStyles);
            if(customStyles.Contains(style)) { return; }
            customStyles.Add(style);
            Skin.customStyles = customStyles.ToArray();
        }

        public static void RemoveStyle(GUIStyle style)
        {
            if(style == null) { return; }
            List<GUIStyle> customStyles = new List<GUIStyle>();
            customStyles.AddRange(Skin.customStyles);
            if(!customStyles.Contains(style)) { return; }
            customStyles.Remove(style);
            Skin.customStyles = customStyles.ToArray();
        }

        private static List<GUIStyle> SideButtonStyles()
        {
            RectOffset expandCollapseBorder = new RectOffset(2, 28, 2, 2);
            RectOffset expandCollapsePadding = new RectOffset(2, 32, 2, 4);

            RectOffset border = new RectOffset(28, 2, 2, 2);
            RectOffset padding = new RectOffset(32, 2, 2, 2);
            int fontSize = 16;

            RectOffset socialButtonBorder = new RectOffset(28, 2, 2, 2);
            RectOffset socialButtonPadding = new RectOffset(28, 2, 2, 2);
            int socialButtonFontSize = 12;

            Color normalGray = QUI.IsProSkin ? QColors.UnityMild.Color : QColors.UnityMild.Color;
            Color activeBlue = QUI.IsProSkin ? QColors.Blue.Color : QColors.BlueDark.Color;
            Color hoverBlue = QUI.IsProSkin ? QColors.BlueDark.Color : QColors.BlueDark.Color;
            Color selectedBlue = QUI.IsProSkin ? QColors.BlueLight.Color : QColors.BlueDark.Color;


            return new List<GUIStyle>()
            {
                SideButtonStyle(QStyles.GetStyleName(SideButton.CollapseSideBar), DUIResources.sideButtonCollapseSideBar, normalGray, activeBlue,  hoverBlue, expandCollapseBorder, expandCollapsePadding),
                SideButtonStyle(QStyles.GetStyleName(SideButton.ExpandSideBar), DUIResources.sideButtonExpandSideBar, normalGray, activeBlue,  hoverBlue, expandCollapseBorder, expandCollapsePadding),

                SideButtonStyle(QStyles.GetStyleName(SideButton.ControlPanel), DUIResources.sideButtonControlPanel, normalGray, activeBlue,  hoverBlue, border, padding, fontSize),
                SideButtonSelectedStyle(QStyles.GetStyleName(SideButton.ControlPanelSelected), DUIResources.sideButtonControlPanelSelected, selectedBlue, border, padding, fontSize),

                SideButtonStyle(QStyles.GetStyleName(SideButton.UIElements), DUIResources.sideButtonUIElements, normalGray, activeBlue,  hoverBlue, border, padding, fontSize),
                SideButtonSelectedStyle(QStyles.GetStyleName(SideButton.UIElementsSelected), DUIResources.sideButtonUIElementsSelected, selectedBlue, border, padding, fontSize),

                SideButtonStyle(QStyles.GetStyleName(SideButton.UIButtons), DUIResources.sideButtonUIButtons, normalGray, activeBlue,  hoverBlue, border, padding, fontSize),
                SideButtonSelectedStyle(QStyles.GetStyleName(SideButton.UIButtonsSelected), DUIResources.sideButtonUIButtonsSelected, selectedBlue, border, padding, fontSize),

                SideButtonStyle(QStyles.GetStyleName(SideButton.UISounds), DUIResources.sideButtonUISounds, normalGray, activeBlue,  hoverBlue, border, padding, fontSize),
                SideButtonSelectedStyle(QStyles.GetStyleName(SideButton.UISoundsSelected), DUIResources.sideButtonUISoundsSelected, selectedBlue, border, padding, fontSize),

                SideButtonStyle(QStyles.GetStyleName(SideButton.UICanvases), DUIResources.sideButtonUICanvases, normalGray, activeBlue,  hoverBlue, border, padding, fontSize),
                SideButtonSelectedStyle(QStyles.GetStyleName(SideButton.UICanvasesSelected), DUIResources.sideButtonUICanvasesSelected, selectedBlue, border, padding, fontSize),

                SideButtonStyle(QStyles.GetStyleName(SideButton.AnimatorPresets), DUIResources.sideButtonAnimatorPresets, normalGray, activeBlue,  hoverBlue, border, padding, fontSize),
                SideButtonSelectedStyle(QStyles.GetStyleName(SideButton.AnimatorPresetsSelected), DUIResources.sideButtonAnimatorPresetsSelected, selectedBlue, border, padding, fontSize),

                SideButtonStyle(QStyles.GetStyleName(SideButton.EditorSettings), DUIResources.sideButtonEditorSettings, normalGray, activeBlue,  hoverBlue, border, padding, fontSize),
                SideButtonSelectedStyle(QStyles.GetStyleName(SideButton.EditorSettingsSelected), DUIResources.sideButtonEditorSettingsSelected, selectedBlue, border, padding, fontSize),

                SideButtonStyle(QStyles.GetStyleName(SideButton.Help), DUIResources.sideButtonHelp, normalGray, activeBlue,  hoverBlue, border, padding, fontSize),
                SideButtonSelectedStyle(QStyles.GetStyleName(SideButton.HelpSelected), DUIResources.sideButtonHelpSelected, selectedBlue, border, padding, fontSize),

                SideButtonStyle(QStyles.GetStyleName(SideButton.About), DUIResources.sideButtonAbout, normalGray, activeBlue,  hoverBlue, border, padding, fontSize),
                SideButtonSelectedStyle(QStyles.GetStyleName(SideButton.AboutSelected), DUIResources.sideButtonAboutSelected, selectedBlue, border, padding, fontSize),

                SideButtonStyle(QStyles.GetStyleName(SideButton.Twitter), DUIResources.sideButtonTwitter,  ColorExtensions.ColorFrom256(128,128,128), ColorExtensions.ColorFrom256(242,242,242),  ColorExtensions.ColorFrom256(0,153,209), socialButtonBorder, socialButtonPadding, socialButtonFontSize),
                SideButtonStyle(QStyles.GetStyleName(SideButton.Facebook), DUIResources.sideButtonFacebook, ColorExtensions.ColorFrom256(128,128,128), ColorExtensions.ColorFrom256(242,242,242), ColorExtensions.ColorFrom256(74,112,186), socialButtonBorder, socialButtonPadding, socialButtonFontSize),
                SideButtonStyle(QStyles.GetStyleName(SideButton.Youtube), DUIResources.sideButtonYoutube, ColorExtensions.ColorFrom256(128,128,128),  ColorExtensions.ColorFrom256(242,242,242),  ColorExtensions.ColorFrom256(255, 102,102), socialButtonBorder, socialButtonPadding, socialButtonFontSize),
            };
        }

        private static GUIStyle SideButtonStyle(string styleName, QTexture qTexture, Color normalTextColor, Color hoverTextColor, Color activeTextColor, RectOffset border, RectOffset padding, int fontSize = 16)
        {
            return new GUIStyle()
            {
                name = styleName,
                normal = { background = qTexture.normal2D, textColor = normalTextColor },
                onNormal = { background = qTexture.normal2D, textColor = normalTextColor },
                hover = { background = qTexture.hover2D, textColor = hoverTextColor },
                onHover = { background = qTexture.hover2D, textColor = hoverTextColor },
                active = { background = qTexture.active2D, textColor = activeTextColor },
                onActive = { background = qTexture.active2D, textColor = activeTextColor },
                border = border,
                padding = padding,
                fontSize = fontSize,
                alignment = TextAnchor.MiddleLeft,
                font = QResources.GetFont(FontName.Sansation.Regular)
            };
        }
        private static GUIStyle SideButtonSelectedStyle(string styleName, QTexture qTexture, Color normalTextColor, RectOffset border, RectOffset padding, int fontSize = 16)
        {
            return new GUIStyle()
            {
                name = styleName,
                normal = { background = qTexture.normal2D, textColor = normalTextColor },
                onNormal = { background = qTexture.normal2D, textColor = normalTextColor },
                border = border,
                padding = padding,
                fontSize = fontSize,
                alignment = TextAnchor.MiddleLeft,
                font = QResources.GetFont(FontName.Sansation.Regular)
            };
        }

        private static List<GUIStyle> NotificationWindowStyles()
        {
            List<GUIStyle> styles = new List<GUIStyle>
            {
                GetBlackTextStyle(NotificationWindow.NotificationTitleBlack, TextAnchor.MiddleRight, FontStyle.Normal, 20, DUIResources.Sansation),
                GetBlackTextStyle(NotificationWindow.NotificationMessageBlack, TextAnchor.MiddleCenter, FontStyle.Normal, 12, DUIResources.Sansation),
                GetButtonStyle(NotificationWindow.NotificationButtonOk, DUIResources.notificationWindowButtonOk),
                GetButtonStyle(NotificationWindow.NotificationButtonCancel, DUIResources.notificationWindowButtonCancel),
                GetButtonStyle(NotificationWindow.NotificationButtonYes, DUIResources.notificationWindowButtonYes),
                GetButtonStyle(NotificationWindow.NotificationButtonNo, DUIResources.notificationWindowButtonNo),
                GetButtonStyle(NotificationWindow.NotificationButtonContinue, DUIResources.notificationWindowButtonContinue)
            };
            return styles;
        }
        private static GUIStyle GetButtonStyle(NotificationWindow styleName, QTexture qTexture)
        {
            return new GUIStyle()
            {
                name = styleName.ToString(),
                normal = { background = qTexture.normal2D },
                onNormal = { background = qTexture.normal2D },
                hover = { background = qTexture.hover2D },
                onHover = { background = qTexture.hover2D },
                active = { background = qTexture.active2D },
                onActive = { background = qTexture.active2D }
            };
        }
        private static GUIStyle GetBlackTextStyle(NotificationWindow styleName, TextAnchor alignment, FontStyle fontStyle, int fontSize, Font font = null)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.name = styleName.ToString();
            style.normal.textColor = QuickEngine.Extensions.ColorExtensions.ColorFrom256(29, 24, 25);
            style.alignment = alignment;
            style.fontStyle = fontStyle;
            style.fontSize = fontSize;
            style.font = font;
            style.wordWrap = true;
            style.richText = true;
            return style;
        }
    }
}
