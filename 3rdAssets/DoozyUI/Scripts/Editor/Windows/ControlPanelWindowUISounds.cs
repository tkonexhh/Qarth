// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using QuickEngine.Extensions;
using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace DoozyUI
{
    public partial class ControlPanelWindow : QWindow
    {
        static string NewUISoundName = "";
        static AnimBool NewUISoundAnimBool = new AnimBool(false);
        static SoundType selectedUISoundsDatabaseFilter = SoundType.All;

        void InitPageUISounds()
        {
            DUIData.Instance.AutomatedScanForUISounds();
            DUIData.Instance.ValidateUISounds();
        }

        void DrawPageUISounds()
        {
            DrawPageHeader("UISounds", QColors.Blue, "Database", QUI.IsProSkin ? QColors.UnityLight : QColors.UnityMild, DUIResources.pageIconUISounds);

            QUI.Space(6);

            DrawPageUISoundsRefreshButton(WindowSettings.CurrentPageContentWidth);

            QUI.Space(SPACE_16);

            DrawSoundsDatabase(WindowSettings.CurrentPageContentWidth);
        }

        void DrawPageUISoundsRefreshButton(float width)
        {
            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8);
                if(QUI.SlicedButton("Refresh the UISounds Database", QColors.Color.Gray, width - 16, 18))
                {
                    DUIData.Instance.ScanForUISounds(true);
                }
                QUI.Space(SPACE_8);
            }
            QUI.EndHorizontal();
        }

        void DrawNewUISoundAndSearch(float width)
        {
            QUI.BeginHorizontal(width);
            {
                #region New UISound
                if(SearchPatternAnimBool.faded < 0.2f)
                {
                    if(QUI.GhostButton("New UISound", QColors.Color.Green, 100 * (1 - SearchPatternAnimBool.faded), 20, NewUISoundAnimBool.value)
                       || DetectKeyCombo_Alt_N())
                    {
                        NewUISoundAnimBool.target = !NewUISoundAnimBool.target;
                        if(NewUISoundAnimBool.target)
                        {
                            NewUISoundName = "";
                            SearchPatternAnimBool.target = false;
                        }
                    }
                }

                if(NewUISoundAnimBool.target)
                {
                    SearchPatternAnimBool.target = false;
                    QUI.SetGUIBackgroundColor(QColors.GreenLight.Color);
                    QUI.SetNextControlName("NewUISoundName");
                    NewUISoundName = EditorGUILayout.TextField(NewUISoundName, GUILayout.Width((width - 149) * NewUISoundAnimBool.faded));
                    QUI.ResetColors();

                    if(!NewUISoundAnimBool.value && Event.current.type == EventType.Layout) //if NewUISoundAnimBool.target is true and NewUISoundAnimBool.value is false -> in transition -> select the text in the control
                    {
                        QUI.FocusControl("NewUISoundName");
                        QUI.FocusTextInControl("NewUISoundName");
                    }

                    if(QUI.ButtonOk()
                       || (DetectKey_Return() && QUI.GetNameOfFocusedControl().Equals("NewUISoundName")))
                    {
                        if(NewUISoundName.IsNullOrEmpty())
                        {
                            EditorUtility.DisplayDialog("Info", "Cannot create an unnamed UISound. Try again.", "Ok");
                        }
                        else
                        {
                            if(DUIData.Instance.DatabaseUISounds.Contains(NewUISoundName))
                            {
                                EditorUtility.DisplayDialog("Info", "A UISound named '" + NewUISoundName + "' already exists in the database. Try again.", "Ok");
                            }
                            else
                            {
                                DUIData.Instance.DatabaseUISounds.CreateUISound(NewUISoundName, selectedUISoundsDatabaseFilter, null);
                                NewUISoundAnimBool.target = false;
                            }
                        }
                    }
                    QUI.Space(1);
                    if(QUI.ButtonCancel()
                        || QUI.DetectKeyDown(Event.current, KeyCode.Escape))
                    {
                        NewUISoundName = string.Empty;
                        NewUISoundAnimBool.target = false;
                    }
                }
                #endregion
                QUI.FlexibleSpace();
                #region Search
                if(SearchPatternAnimBool.value)
                {
                    NewUISoundAnimBool.target = false;
                    QUI.SetGUIBackgroundColor(QColors.OrangeLight.Color);
                    QUI.SetNextControlName("SearchPattern");
                    SearchPattern = EditorGUILayout.TextField(SearchPattern, GUILayout.Width((width - 104) * SearchPatternAnimBool.faded));
                    QUI.ResetColors();

                    if(SearchPatternAnimBool.target && Event.current.type == EventType.Layout) //if SearchPatternAnimBool.target is true and SearchPatternAnimBool.value is false -> in transition -> select the text in the control
                    {
                        QUI.FocusControl("SearchPattern");
                        QUI.FocusTextInControl("SearchPattern");
                    }
                }


                if(NewUISoundAnimBool.faded < 0.2f)
                {
                    if(QUI.GhostButton(SearchPatternAnimBool.value ? "Clear Search" : "Search", QColors.Color.Orange, 100 * (1 - NewUISoundAnimBool.faded), 20, SearchPatternAnimBool.value)
                       || DetectKeyCombo_Alt_S() //Toggle Search
                       || (DetectKey_Escape() && SearchPatternAnimBool.target)) //Clear Search
                    {
                        SearchPatternAnimBool.target = !SearchPatternAnimBool.target;
                        if(SearchPatternAnimBool.target)
                        {
                            SearchPattern = string.Empty;
                            NewUISoundAnimBool.target = false;
                        }
                    }
                }
                #endregion
            }
            QUI.EndHorizontal();
        }

        void DrawUISoundsFilterButtons(float width)
        {
            QUI.BeginHorizontal(width);
            {
                QUI.FlexibleSpace();
                QLabel.text = "Sound Type Filter";
                QLabel.style = Style.Text.Small;
                QUI.Label(QLabel);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
            QUI.BeginHorizontal(width);
            {
                QUI.FlexibleSpace();
                DrawUISoundsFilterButton("All Sounds", SoundType.All);
                QUI.Space(1);
                DrawUISoundsFilterButton("UIButton Sounds", SoundType.UIButtons);
                QUI.Space(1);
                DrawUISoundsFilterButton("UIElement Sounds", SoundType.UIElements);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

        }

        bool DrawUISoundsFilterButton(string buttonText, SoundType soundType)
        {
            if(QUI.GhostButton(buttonText, WindowSettings.selectedUISoundsFilter == soundType ? QColors.Color.Blue : QColors.Color.Gray, 120, 20, 10, WindowSettings.selectedUISoundsFilter == soundType))
            {
                WindowSettings.selectedUISoundsFilter = soundType;
                QUI.SetDirty(WindowSettings);
                AssetDatabase.SaveAssets();
                return true;
            }
            return false;
        }

        const float PAGE_UISOUNDS_SOUNDTYPE_BUTTON_WIDTH = 84;
        const float PAGE_UISOUNDS_SOUNDNAME_FIELD_WIDTH_PERCENT = 0.5f;
        const float PAGE_UISOUNDS_AUDIOCLIP_FIELD_WIDTH_PERCENT = 0.5f;
        const float PAGE_UISOUNDS_BUTTONS_WIDTH = 48f; //16x3 buttons (play stop delete)
        const float PAGE_UISOUNDS_HORIZONTAL_SPACE = 1;

        void DrawSoundsDatabase(float width)
        {
            DrawNewUISoundAndSearch(width);
            QUI.Space(SPACE_8);
            DrawUISoundsFilterButtons(width);
            QUI.Space(SPACE_8);

            for(int i = 0; i < DUIData.Instance.DatabaseUISounds.sounds.Count; i++)
            {
                if(DUIData.Instance.DatabaseUISounds.sounds[i] == null) { continue; }
                if(WindowSettings.selectedUISoundsFilter == SoundType.UIButtons && DUIData.Instance.DatabaseUISounds.sounds[i].soundType == SoundType.UIElements) { continue; }
                if(WindowSettings.selectedUISoundsFilter == SoundType.UIElements && DUIData.Instance.DatabaseUISounds.sounds[i].soundType == SoundType.UIButtons) { continue; }

                QUI.BeginHorizontal(width, 20);
                {
                    if(SearchPatternAnimBool.target)//a search pattern has been entered in the search box
                    {
                        try
                        {
                            if(!Regex.IsMatch(DUIData.Instance.DatabaseUISounds.sounds[i].soundName, SearchPattern, RegexOptions.IgnoreCase))
                            {
                                QUI.EndHorizontal();
                                continue; //this does not match the search pattern --> we do not show this name it
                            }
                        }
                        catch(Exception)
                        { }
                    }

                    QUI.Space(-1);
                    QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, SearchPatternAnimBool.target ? QColors.Color.Orange : QColors.Color.Gray), width + 2, 20);
                    QUI.Space(-2);
                    QUI.Space(-width);

                    SoundType soundType = DUIData.Instance.DatabaseUISounds.sounds[i].soundType;
                    QUI.BeginChangeCheck();
                    soundType = (SoundType)EditorGUILayout.EnumPopup(soundType, GUILayout.Width(PAGE_UISOUNDS_SOUNDTYPE_BUTTON_WIDTH));
                    if(QUI.EndChangeCheck())
                    {
                        Undo.RecordObject(DUIData.Instance, "UpdateSoundType");
                        DUIData.Instance.DatabaseUISounds.sounds[i].soundType = soundType;
                        QUI.SetDirty(DUIData.Instance.DatabaseUISounds.sounds[i]);
                        QUI.SetDirty(DUIData.Instance);
                        AssetDatabase.SaveAssets();
                    }

                    QLabel.text = DUIData.Instance.DatabaseUISounds.sounds[i].soundName;
#if dUI_MasterAudio
                    QLabel.text = "Controlled by MasterAudio";
#endif
                    QUI.Label(QLabel.text, Style.Text.Normal, (width - PAGE_UISOUNDS_SOUNDTYPE_BUTTON_WIDTH - PAGE_UISOUNDS_HORIZONTAL_SPACE * 3 - PAGE_UISOUNDS_BUTTONS_WIDTH * (1 - SearchPatternAnimBool.faded) - 16) * PAGE_UISOUNDS_SOUNDNAME_FIELD_WIDTH_PERCENT);

                    AudioClip audioClip = DUIData.Instance.DatabaseUISounds.sounds[i].audioClip;
                    QUI.BeginChangeCheck();
                    audioClip = (AudioClip)EditorGUILayout.ObjectField("", audioClip, typeof(AudioClip), false, GUILayout.Width((width - PAGE_UISOUNDS_SOUNDTYPE_BUTTON_WIDTH - PAGE_UISOUNDS_HORIZONTAL_SPACE * 3 - PAGE_UISOUNDS_BUTTONS_WIDTH * (1 - SearchPatternAnimBool.faded) - 16) * PAGE_UISOUNDS_AUDIOCLIP_FIELD_WIDTH_PERCENT));
                    if(QUI.EndChangeCheck())
                    {
                        Undo.RecordObject(DUIData.Instance, "UpdateAudioClipReferene");
                        DUIData.Instance.DatabaseUISounds.sounds[i].audioClip = audioClip;
                        QUI.SetDirty(DUIData.Instance.DatabaseUISounds.sounds[i]);
                        QUI.SetDirty(DUIData.Instance);
                        AssetDatabase.SaveAssets();
                    }

                    if(SearchPatternAnimBool.faded < 0.6f)
                    {
                        QUI.Space(PAGE_UISOUNDS_HORIZONTAL_SPACE);

                        if(QUI.ButtonPlay())
                        {
                            DUIUtils.PreviewSound(DUIData.Instance.DatabaseUISounds.sounds[i].soundName);
                        }
                    }

                    if(SearchPatternAnimBool.faded < 0.4f)
                    {
                        QUI.Space(PAGE_UISOUNDS_HORIZONTAL_SPACE);

                        if(QUI.ButtonStop())
                        {
                            DUIUtils.StopSoundPreview(DUIData.Instance.DatabaseUISounds.sounds[i].soundName);
                        }

                        QUI.Space(PAGE_UISOUNDS_HORIZONTAL_SPACE);
                    }

                    if(SearchPatternAnimBool.faded < 0.2f)
                    {
                        if(QUI.ButtonCancel())
                        {
                            if(EditorUtility.DisplayDialog("Delete the '" + DUIData.Instance.DatabaseUISounds.sounds[i].soundName + "' UISound?",
                                                           "Are you sure you want to proceed?" +
                                                                "\n\n" +
                                                                "Operation cannot be undone!",
                                                           "Yes",
                                                           "Cancel"))
                            {
                                DUIData.Instance.DatabaseUISounds.DeleteUISound(DUIData.Instance.DatabaseUISounds.sounds[i].soundName);
                                QUI.SetDirty(DUIData.Instance);
                                AssetDatabase.SaveAssets();
                                QUI.EndHorizontal();
                                QUI.ExitGUI(); //fixes null references on OSX
                                continue;
                            }
                        }
                    }
                    QUI.FlexibleSpace();
                }
                QUI.EndHorizontal();
                QUI.Space(SPACE_2);
            }
        }
    }
}
