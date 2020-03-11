// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using UnityEditor;

namespace DoozyUI
{
    public partial class ControlPanelWindow : QWindow
    {
        const int ANIMATOR_PRESETS_TAB_TITLE_EXTRA_WIDTH = 8;
        const int ANIMATOR_PRESETS_TAB_TITLE_HEIGHT = 16;

        const int ANIMATOR_PRESETS_TAB_BUTTON_WIDTH = 72;
        const int ANIMATOR_PRESETS_TAB_BUTTON_HEIGHT = 20;
        const int ANIMATOR_PRESETS_TAB_BUTTON_SPACING = 1;

        const int ANIMATOR_PRESETS_CATEGORY_BAR_HEIGHT = 24;
        const int ANIMATOR_PRESETS_PRESET_BAR_HEIGHT = 20;

        void InitPageAnimatorPresets()
        {
            DUIData.Instance.ValidateInAnimations();
            DUIData.Instance.ValidateOutAnimations();
            DUIData.Instance.ValidateLoopAnimations();
            DUIData.Instance.ValidatePunchAnimations();
            DUIData.Instance.ValidateStateAnimations();
        }

        void DrawPageAnimatorPresets()
        {
            DrawPageHeader("Animator Presets", QColors.Blue, "Live Presets Editor", QUI.IsProSkin ? QColors.UnityLight : QColors.UnityMild, DUIResources.pageIconAnimatorPresets);

            QUI.Space(6);

            DrawPageAnimatorPresetsRefreshButton(WindowSettings.CurrentPageContentWidth);

            QUI.Space(SPACE_16);

            DrawPageAnimatorPresetsTabsButtons(WindowSettings.CurrentPageContentWidth);

            QUI.Space(SPACE_16);

            DrawPageAnimatorPresetsTabs(WindowSettings.CurrentPageContentWidth);
        }

        void DrawPageAnimatorPresetsRefreshButton(float width)
        {
            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8);
                if(QUI.SlicedButton("Refresh the Animator Presets Databases", QColors.Color.Gray, width - 16, 18))
                {
                    DUIData.Instance.ScanForInAnimations(true);
                    DUIData.Instance.ScanForOutAnimations(true);
                    DUIData.Instance.ScanForLoopAnimations(true);
                    DUIData.Instance.ScanForPunchAnimations(true);
                    DUIData.Instance.ScanForStateAnimations(true);
                }
                QUI.Space(SPACE_8);
            }
            QUI.EndHorizontal();
        }

        bool DrawAnimatorPresetsTabButton(AnimatorPreset preset)
        {
            if(QUI.GhostButton(preset.ToString(), WindowSettings.selectedAnimatorPresetTab == preset ? QColors.Color.Blue : QColors.Color.Gray, ANIMATOR_PRESETS_TAB_BUTTON_WIDTH, ANIMATOR_PRESETS_TAB_BUTTON_HEIGHT, WindowSettings.selectedAnimatorPresetTab == preset))
            {
                WindowSettings.selectedAnimatorPresetTab = preset;
                QUI.SetDirty(DUIData.Instance);
                AssetDatabase.SaveAssets();
                return true;
            }
            return false;
        }
        void DrawPageAnimatorPresetsTabsButtons(float width)
        {
            QUI.BeginHorizontal(width);
            {
                QUI.FlexibleSpace();

                float tempWidth = ANIMATOR_PRESETS_TAB_BUTTON_WIDTH * 3 + ANIMATOR_PRESETS_TAB_BUTTON_SPACING * 2 + ANIMATOR_PRESETS_TAB_TITLE_EXTRA_WIDTH;

                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low,
                                                   (WindowSettings.selectedAnimatorPresetTab == AnimatorPreset.In)
                                                    || (WindowSettings.selectedAnimatorPresetTab == AnimatorPreset.Out)
                                                    || (WindowSettings.selectedAnimatorPresetTab == AnimatorPreset.Loop)
                                                   ? QColors.Color.Blue : QColors.Color.Gray),
                        tempWidth,
                        ANIMATOR_PRESETS_TAB_TITLE_HEIGHT);

                QUI.Space(-tempWidth);
                QUI.Space(ANIMATOR_PRESETS_TAB_TITLE_EXTRA_WIDTH / 2);

                QUI.BeginHorizontal(tempWidth - ANIMATOR_PRESETS_TAB_TITLE_EXTRA_WIDTH / 2);
                {
                    QLabel.text = "UIElement Animations";
                    QLabel.style = Style.Text.Small;
                    QUI.Space((tempWidth - QLabel.x) / 2);
                    QUI.Label(QLabel.text, Style.Text.Small, QLabel.x, ANIMATOR_PRESETS_TAB_TITLE_HEIGHT - 4);
                    QUI.Space((tempWidth - QLabel.x) / 2);
                }
                QUI.EndHorizontal();

                QUI.Space(ANIMATOR_PRESETS_TAB_BUTTON_SPACING * 6);

                tempWidth = ANIMATOR_PRESETS_TAB_BUTTON_WIDTH * 2 + ANIMATOR_PRESETS_TAB_BUTTON_SPACING + ANIMATOR_PRESETS_TAB_TITLE_EXTRA_WIDTH;

                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low,
                                                   (WindowSettings.selectedAnimatorPresetTab == AnimatorPreset.Punch)
                                                    || (WindowSettings.selectedAnimatorPresetTab == AnimatorPreset.State)
                                                   ? QColors.Color.Blue : QColors.Color.Gray),
                        tempWidth,
                        ANIMATOR_PRESETS_TAB_TITLE_HEIGHT);

                QUI.Space(-tempWidth);

                QUI.BeginHorizontal(tempWidth - ANIMATOR_PRESETS_TAB_TITLE_EXTRA_WIDTH / 2);
                {
                    QLabel.text = "UIButton Animations";
                    QLabel.style = Style.Text.Small;
                    QUI.Space((tempWidth - QLabel.x) / 2);
                    QUI.Label(QLabel.text, Style.Text.Small, QLabel.x, ANIMATOR_PRESETS_TAB_TITLE_HEIGHT - 4);
                    QUI.Space((tempWidth - QLabel.x) / 2);
                }
                QUI.EndHorizontal();

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
            QUI.Space(ANIMATOR_PRESETS_TAB_BUTTON_SPACING);
            QUI.BeginHorizontal(width);
            {
                QUI.FlexibleSpace();
                QUI.Space(ANIMATOR_PRESETS_TAB_TITLE_EXTRA_WIDTH / 2);

                DrawAnimatorPresetsTabButton(AnimatorPreset.In);
                QUI.Space(ANIMATOR_PRESETS_TAB_BUTTON_SPACING);
                DrawAnimatorPresetsTabButton(AnimatorPreset.Out);
                QUI.Space(ANIMATOR_PRESETS_TAB_BUTTON_SPACING);
                DrawAnimatorPresetsTabButton(AnimatorPreset.Loop);

                QUI.Space(ANIMATOR_PRESETS_TAB_TITLE_EXTRA_WIDTH);
                QUI.Space(ANIMATOR_PRESETS_TAB_BUTTON_SPACING * 6);
                QUI.Space(ANIMATOR_PRESETS_TAB_TITLE_EXTRA_WIDTH);

                DrawAnimatorPresetsTabButton(AnimatorPreset.Punch);
                QUI.Space(ANIMATOR_PRESETS_TAB_BUTTON_SPACING);
                DrawAnimatorPresetsTabButton(AnimatorPreset.State);

                QUI.Space(ANIMATOR_PRESETS_TAB_TITLE_EXTRA_WIDTH / 2);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }

        void DrawPageAnimatorPresetsTabs(float width)
        {
            switch(WindowSettings.selectedAnimatorPresetTab)
            {
                case AnimatorPreset.In: DrawPageAnimatorPresetsAnimData(DUIData.Instance.DatabaseInAnimations, UIAnimatorUtil.RELATIVE_PATH_IN_ANIM_DATA, width); break;
                case AnimatorPreset.Out: DrawPageAnimatorPresetsAnimData(DUIData.Instance.DatabaseOutAnimations, UIAnimatorUtil.RELATIVE_PATH_OUT_ANIM_DATA, width); break;
                case AnimatorPreset.Loop: DrawPageAnimatorPresetsLoopData(DUIData.Instance.DatabaseLoopAnimations, UIAnimatorUtil.RELATIVE_PATH_LOOP_DATA, width); break;
                case AnimatorPreset.Punch: DrawPageAnimatorPresetsPunchData(DUIData.Instance.DatabasePunchAnimations, UIAnimatorUtil.RELATIVE_PATH_PUNCH_DATA, width); break;
                case AnimatorPreset.State: DrawPageAnimatorPresetsAnimData(DUIData.Instance.DatabaseStateAnimations, UIAnimatorUtil.RELATIVE_PATH_STATE_ANIM_DATA, width); break;
            }
        }

        void DrawPageAnimatorPresetsAnimData(DUIData.AnimDatabase database, string relativePath, float width)
        {
            if(database == null || database.categories == null || database.categories.Count == 0)
            {
                //ToDo - draw info message telling the dev that the database is empty
                return;
            }

            for(int categoryIndex = 0; categoryIndex < database.categories.Count; categoryIndex++)
            {
                if(QUI.GhostBar(database.categories[categoryIndex].categoryName, QColors.Color.Blue, database.categories[categoryIndex].isExpanded, width, ANIMATOR_PRESETS_CATEGORY_BAR_HEIGHT))
                {
                    database.categories[categoryIndex].isExpanded.target = !database.categories[categoryIndex].isExpanded.target;
                    database.categories[categoryIndex].ExpandOrCollapseAllAnimData(false);
                }
                QUI.BeginHorizontal(width);
                {
                    QUI.Space(SPACE_8 * database.categories[categoryIndex].isExpanded.faded);
                    if(QUI.BeginFadeGroup(database.categories[categoryIndex].isExpanded.faded))
                    {
                        QUI.BeginVertical(width - SPACE_8);
                        {
                            QUI.Space(SPACE_2);
                            for(int presetIndex = 0; presetIndex < database.categories[categoryIndex].presets.Count; presetIndex++)
                            {
                                QUI.BeginHorizontal(width - SPACE_8);
                                {
                                    if(DUIUtils.PresetGhostBar(database.categories[categoryIndex].presets[presetIndex].presetName, QColors.Color.Blue, database.categories[categoryIndex].presets[presetIndex].isExpanded, width - SPACE_8 - SPACE_2 - SPACE_16, ANIMATOR_PRESETS_PRESET_BAR_HEIGHT))
                                    {
                                        database.categories[categoryIndex].presets[presetIndex].isExpanded.target = !database.categories[categoryIndex].presets[presetIndex].isExpanded.target;
                                    }
                                    QUI.Space(SPACE_2);
                                    if(QUI.ButtonCancel())
                                    {
                                        if(QUI.DisplayDialog("Delete the '" + database.categories[categoryIndex].presets[presetIndex].presetName + "' preset?",
                                                             "Are you sure you want to delete this preset?" +
                                                                "\n\n" +
                                                                "Operation cannot be undone!",
                                                             "Yes",
                                                             "No"))
                                        {
                                            database.categories[categoryIndex].DeleteAnimData(database.categories[categoryIndex].presets[presetIndex].presetName, relativePath);
                                            if(database.categories[categoryIndex].presets.Count == 0) //category is empty -> remove it
                                            {
                                                database.RemoveCategory(database.categories[categoryIndex].categoryName, relativePath, true);
                                            }
                                            QUI.ExitGUI();
                                        }
                                    }
                                }
                                QUI.EndHorizontal();
                                QUI.BeginHorizontal(width - SPACE_8);
                                {
                                    QUI.Space(SPACE_8 * database.categories[categoryIndex].presets[presetIndex].isExpanded.faded);
                                    if(QUI.BeginFadeGroup(database.categories[categoryIndex].presets[presetIndex].isExpanded.faded))
                                    {
                                        QUI.BeginVertical(width - SPACE_16);
                                        {
                                            DUIUtils.DrawAnim(database.categories[categoryIndex].presets[presetIndex].data, database.categories[categoryIndex].presets[presetIndex], width - SPACE_16);
                                            QUI.Space(SPACE_8 * database.categories[categoryIndex].presets[presetIndex].isExpanded.faded); //space added if preset is opened
                                        }
                                        QUI.EndVertical();
                                    }
                                    QUI.EndFadeGroup();
                                }
                                QUI.EndHorizontal();
                                QUI.Space(SPACE_4); //space between presets
                            }
                            QUI.Space(SPACE_4 * database.categories[categoryIndex].isExpanded.faded); //space added if category is opened
                        }
                        QUI.EndVertical();
                    }
                    QUI.EndFadeGroup();

                }
                QUI.EndHorizontal();
                QUI.Space(SPACE_4); //space between categories
            }
        }
        void DrawPageAnimatorPresetsLoopData(DUIData.LoopDatabase database, string relativePath, float width)
        {
            if(database == null || database.categories == null || database.categories.Count == 0)
            {
                //ToDo - draw info message telling the dev that the database is empty
                return;
            }

            for(int categoryIndex = 0; categoryIndex < database.categories.Count; categoryIndex++)
            {
                if(QUI.GhostBar(database.categories[categoryIndex].categoryName, QColors.Color.Blue, database.categories[categoryIndex].isExpanded, width, ANIMATOR_PRESETS_CATEGORY_BAR_HEIGHT))
                {
                    database.categories[categoryIndex].isExpanded.target = !database.categories[categoryIndex].isExpanded.target;
                    database.categories[categoryIndex].ExpandOrCollapseAllAnimData(false);
                }
                QUI.BeginHorizontal(width);
                {
                    QUI.Space(SPACE_8 * database.categories[categoryIndex].isExpanded.faded);
                    if(QUI.BeginFadeGroup(database.categories[categoryIndex].isExpanded.faded))
                    {
                        QUI.BeginVertical(width - SPACE_8);
                        {
                            QUI.Space(SPACE_2);
                            for(int presetIndex = 0; presetIndex < database.categories[categoryIndex].presets.Count; presetIndex++)
                            {
                                QUI.BeginHorizontal(width - SPACE_8);
                                {
                                    if(DUIUtils.PresetGhostBar(database.categories[categoryIndex].presets[presetIndex].presetName, QColors.Color.Blue, database.categories[categoryIndex].presets[presetIndex].isExpanded, width - SPACE_8 - SPACE_2 - SPACE_16, ANIMATOR_PRESETS_PRESET_BAR_HEIGHT))
                                    {
                                        database.categories[categoryIndex].presets[presetIndex].isExpanded.target = !database.categories[categoryIndex].presets[presetIndex].isExpanded.target;
                                    }
                                    QUI.Space(SPACE_2);
                                    if(QUI.ButtonCancel())
                                    {
                                        if(QUI.DisplayDialog("Delete the '" + database.categories[categoryIndex].presets[presetIndex].presetName + "' preset?",
                                                             "Are you sure you want to delete this preset?" +
                                                                "\n\n" +
                                                                "Operation cannot be undone!",
                                                             "Yes",
                                                             "No"))
                                        {
                                            database.categories[categoryIndex].DeleteLoopData(database.categories[categoryIndex].presets[presetIndex].presetName, relativePath + database.categories[categoryIndex].categoryName + "/");
                                            if(database.categories[categoryIndex].presets.Count == 0) //category is empty -> remove it
                                            {
                                                database.RemoveCategory(database.categories[categoryIndex].categoryName, relativePath, true);
                                            }
                                            QUI.ExitGUI();
                                        }
                                    }
                                }
                                QUI.EndHorizontal();
                                QUI.BeginHorizontal(width - SPACE_8);
                                {
                                    QUI.Space(SPACE_8 * database.categories[categoryIndex].presets[presetIndex].isExpanded.faded);
                                    if(QUI.BeginFadeGroup(database.categories[categoryIndex].presets[presetIndex].isExpanded.faded))
                                    {
                                        QUI.BeginVertical(width - SPACE_16);
                                        {
                                            DUIUtils.DrawLoop(database.categories[categoryIndex].presets[presetIndex].data, database.categories[categoryIndex].presets[presetIndex], width - SPACE_16);
                                            QUI.Space(SPACE_8 * database.categories[categoryIndex].presets[presetIndex].isExpanded.faded); //space added if preset is opened
                                        }
                                        QUI.EndVertical();
                                    }
                                    QUI.EndFadeGroup();
                                }
                                QUI.EndHorizontal();
                                QUI.Space(SPACE_4); //space between presets
                            }
                            QUI.Space(SPACE_4 * database.categories[categoryIndex].isExpanded.faded); //space added if category is opened
                        }
                        QUI.EndVertical();
                    }
                    QUI.EndFadeGroup();

                }
                QUI.EndHorizontal();
                QUI.Space(SPACE_4); //space between categories
            }
        }
        void DrawPageAnimatorPresetsPunchData(DUIData.PunchDatabase database, string relativePath, float width)
        {
            if(database == null || database.categories == null || database.categories.Count == 0)
            {
                //ToDo - draw info message telling the dev that the database is empty
                return;
            }

            for(int categoryIndex = 0; categoryIndex < database.categories.Count; categoryIndex++)
            {
                if(QUI.GhostBar(database.categories[categoryIndex].categoryName, QColors.Color.Blue, database.categories[categoryIndex].isExpanded, width, ANIMATOR_PRESETS_CATEGORY_BAR_HEIGHT))
                {
                    database.categories[categoryIndex].isExpanded.target = !database.categories[categoryIndex].isExpanded.target;
                    database.categories[categoryIndex].ExpandOrCollapseAllAnimData(false);
                }
                QUI.BeginHorizontal(width);
                {
                    QUI.Space(SPACE_8 * database.categories[categoryIndex].isExpanded.faded);
                    if(QUI.BeginFadeGroup(database.categories[categoryIndex].isExpanded.faded))
                    {
                        QUI.BeginVertical(width - SPACE_8);
                        {
                            QUI.Space(SPACE_2);
                            for(int presetIndex = 0; presetIndex < database.categories[categoryIndex].presets.Count; presetIndex++)
                            {
                                QUI.BeginHorizontal(width - SPACE_8);
                                {
                                    if(DUIUtils.PresetGhostBar(database.categories[categoryIndex].presets[presetIndex].presetName, QColors.Color.Blue, database.categories[categoryIndex].presets[presetIndex].isExpanded, width - SPACE_8 - SPACE_2 - SPACE_16, ANIMATOR_PRESETS_PRESET_BAR_HEIGHT))
                                    {
                                        database.categories[categoryIndex].presets[presetIndex].isExpanded.target = !database.categories[categoryIndex].presets[presetIndex].isExpanded.target;
                                    }
                                    QUI.Space(SPACE_2);
                                    if(QUI.ButtonCancel())
                                    {
                                        if(QUI.DisplayDialog("Delete the '" + database.categories[categoryIndex].presets[presetIndex].presetName + "' preset?",
                                                             "Are you sure you want to delete this preset?" +
                                                                "\n\n" +
                                                                "Operation cannot be undone!",
                                                             "Yes",
                                                             "No"))
                                        {
                                            database.categories[categoryIndex].DeletePunchData(database.categories[categoryIndex].presets[presetIndex].presetName, relativePath + database.categories[categoryIndex].categoryName + "/");
                                            if(database.categories[categoryIndex].presets.Count == 0) //category is empty -> remove it
                                            {
                                                database.RemoveCategory(database.categories[categoryIndex].categoryName, relativePath, true);
                                            }
                                            QUI.ExitGUI();
                                        }
                                    }
                                }
                                QUI.EndHorizontal();
                                QUI.BeginHorizontal(width - SPACE_8);
                                {
                                    QUI.Space(SPACE_8 * database.categories[categoryIndex].presets[presetIndex].isExpanded.faded);
                                    if(QUI.BeginFadeGroup(database.categories[categoryIndex].presets[presetIndex].isExpanded.faded))
                                    {
                                        QUI.BeginVertical(width - SPACE_16);
                                        {
                                            DUIUtils.DrawPunch(database.categories[categoryIndex].presets[presetIndex].data, database.categories[categoryIndex].presets[presetIndex], width - SPACE_16);
                                            QUI.Space(SPACE_8 * database.categories[categoryIndex].presets[presetIndex].isExpanded.faded); //space added if preset is opened
                                        }
                                        QUI.EndVertical();
                                    }
                                    QUI.EndFadeGroup();
                                }
                                QUI.EndHorizontal();
                                QUI.Space(SPACE_4); //space between presets
                            }
                            QUI.Space(SPACE_4 * database.categories[categoryIndex].isExpanded.faded); //space added if category is opened
                        }
                        QUI.EndVertical();
                    }
                    QUI.EndFadeGroup();

                }
                QUI.EndHorizontal();
                QUI.Space(SPACE_4); //space between categories
            }
        }
    }
}
