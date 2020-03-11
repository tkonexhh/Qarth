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
    public partial class ControlPanelWindow : QWindow
    {
        AnimBool editVersion;
        string newVersion = "";

        public const string COPYRIGHT = "Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.";

        void InitPageAbout()
        {
            editVersion = new AnimBool(false, Repaint);
        }

        void DrawPageAbout()
        {
            DrawPageHeader("About", QColors.Blue, "", QUI.IsProSkin ? QColors.UnityLight : QColors.UnityMild, DUIResources.pageIconAbout);
            QUI.Space(SPACE_16);
            QUI.DrawTexture(DUIResources.aboutVersion.texture, 400, 200);
            if(WindowSettings.CurrentPageContentWidth > WindowSettings.pageWidth - 230)
            {
                return;
            }
            QUI.Space(-56);
            QUI.BeginHorizontal(WindowSettings.CurrentPageContentWidth);
            {
                QUI.FlexibleSpace();
                QLabel.text = DUIVersion.Instance.version;
                QLabel.style = Style.Text.Subtitle;
                QUI.SetGUIContentColor(QUI.AccentColorBlue);
                if(editVersion.target)
                {
                    newVersion = QUI.TextField(newVersion, 104);
                }
                else
                {
                    QUI.Label(QLabel);
                }
                QUI.ResetColors();
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
            QUI.Space(38);

#if dUI_SOURCE
            QUI.BeginHorizontal(WindowSettings.CurrentPageContentWidth);
            {
                QUI.FlexibleSpace();
                if(QUI.GhostButton(editVersion.target ? "Update Version" : "Edit Version", editVersion.target ? QColors.Color.Green : QColors.Color.Gray, 100))
                {
                    editVersion.target = !editVersion.target;
                    if(editVersion.target)
                    {
                        newVersion = DUIVersion.Instance.version;
                    }
                    else
                    {
                        DUIVersion.Instance.version = newVersion;
                        QUI.SetDirty(DUIVersion.Instance);
                        AssetDatabase.SaveAssets();
                    }
                }

                if(editVersion.faded > 0.2f)
                {
                    if(QUI.GhostButton("Cancel", QColors.Color.Red, 100))
                    {
                        editVersion.target = false;
                        newVersion = "";
                    }
                }
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
#endif
            QUI.Space(SPACE_8);

            DrawNewsArticle("About DoozyUI",
                            "DoozyUI is a complete UI management system for Unity. " +
                              "It manipulates native Unity components and takes full advantage of their intended usage. " +
                              "This assures maximum compatibility with uGUI, best performance and makes the entire system have a predictable behaviour. " +
                              "Also, by working only with native components, the system will be compaible with any ohter asset that uses uGUI correctly. " +
                              "\n\n" +
                              "Easy to use and understand, given the user has some basic knowledge of how Unity's native UI solution (uGUI) works, DoozyUI has flexible components that can be configured in a lot of ways. " +
                              "Functionality and design go hand in hand in order to offer a pleasant user experience (UX) while using the system." +
                              "\n\n" +
                              "Starting with version 2.8, DoozyUI is officialy VR READY, being capable of handling with ease multiple Canvases set to World Space render mode. " +
                              "The system has been redesigned, from the core up, in order to accomodate a higher degree of flexibility that was needed in order for it to handle a lot of different use case scenarios." +
                              "\n\n" +
                              "The asset 'DoozyUI' has been released on the Unity Asset Store under the 'Doozy Entertainment' brand, owned by the Marlink Trading SRL company.",
                            WindowSettings.CurrentPageContentWidth);

        }
    }
}
