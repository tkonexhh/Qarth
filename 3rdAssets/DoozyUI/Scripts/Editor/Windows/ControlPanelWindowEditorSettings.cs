// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace DoozyUI
{
    public partial class ControlPanelWindow : QWindow
    {
        AnimBool HierarchyManagerAnimBool;
        AnimBool UIElementAnimBool;
        AnimBool UIButtonAnimBool;
        AnimBool UIToggleAnimBool;
        //AnimBool UIToggleOnPointerEnterAnimBool;
        //AnimBool UIToggleOnPointerExitAnimBool;
        //AnimBool UIToggleOnClickAnimBool;
        AnimBool UIEffectAnimBool;

        float tempWidth;

        void InitPageEditorSettings()
        {
            HierarchyManagerAnimBool = new AnimBool(false, Repaint);
            UIElementAnimBool = new AnimBool(false, Repaint);
            UIButtonAnimBool = new AnimBool(false, Repaint);
            UIToggleAnimBool = new AnimBool(false, Repaint);
            //UIToggleOnPointerEnterAnimBool = new AnimBool(false, Repaint);
            //UIToggleOnPointerExitAnimBool = new AnimBool(false, Repaint);
            //UIToggleOnClickAnimBool = new AnimBool(false, Repaint);
            UIEffectAnimBool = new AnimBool(false, Repaint);
        }

        void DrawPageEditorSettings()
        {
            tempWidth = WindowSettings.CurrentPageContentWidth - SPACE_8; //this is the width minus the animated indent

            DrawPageHeader("Editor Settings", QColors.Blue, "only for the Unity Editor", QUI.IsProSkin ? QColors.UnityLight : QColors.UnityMild, DUIResources.pageIconEditorSettings);
            QUI.Space(SPACE_16);
            float sectionWidth = WindowSettings.CurrentPageContentWidth;
            QUI.BeginChangeCheck();
            {
                if(DUI.DUISettings == null)
                {
                    QUI.Space(SPACE_16);
                    QUI.LabelWithBackground("DUISettings not found...");
                    return;
                }
                QUI.Space(SPACE_8);
                DrawSettingsHierarchyManager(sectionWidth);
                QUI.Space(SPACE_4);
                DrawSettingsUIElement(sectionWidth);
                QUI.Space(SPACE_4);
                DrawSettingsUIButton(sectionWidth);
                QUI.Space(SPACE_4);
                DrawSettingsUIToggle(sectionWidth);
                QUI.Space(SPACE_4);
                DrawSettingsUIEffect(sectionWidth);
            }
            if(QUI.EndChangeCheck())
            {
                Undo.RecordObject(EditorSettings, "Updated EditorSettings");
                EditorUtility.SetDirty(EditorSettings);
                AssetDatabase.SaveAssets();
            }
        }

        void DrawSettingsHierarchyManager(float width)
        {
            if(QUI.GhostBar("Hierarchy Manager", QColors.Color.Blue, HierarchyManagerAnimBool, width, BigBarHeight))
            {
                HierarchyManagerAnimBool.target = !HierarchyManagerAnimBool.target;
            }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * HierarchyManagerAnimBool.faded);
                if(QUI.BeginFadeGroup(HierarchyManagerAnimBool.faded))
                {
                    QUI.BeginVertical(tempWidth);
                    {
                        QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            EditorSettings.HierarchyManager_Enabled = QUI.QToggle("Enabled", EditorSettings.HierarchyManager_Enabled, BarHeight);
                            QLabel.text = "- shows icons and other relevant info in the Hierarchy";
                            QLabel.style = Style.Text.Help;
                            QUI.Label(QLabel);
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            QUI.DrawTexture(DUIResources.iconUICanvas.texture, BarHeight, BarHeight);
                            QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);
                            EditorSettings.HierarchyManager_UICanvas_ShowIcon = QUI.QToggle("UICanvas Icon", EditorSettings.HierarchyManager_UICanvas_ShowIcon, BarHeight);
                            QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);
                            EditorSettings.HierarchyManager_UICanvas_ShowCanvasName = QUI.QToggle("Canvas Name", EditorSettings.HierarchyManager_UICanvas_ShowCanvasName, BarHeight);
                            QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);
                            EditorSettings.HierarchyManager_UICanvas_ShowSortingLayerNameAndOrder = QUI.QToggle("Sorting Layer Name and Order", EditorSettings.HierarchyManager_UICanvas_ShowSortingLayerNameAndOrder, BarHeight);
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            QUI.DrawTexture(DUIResources.iconUIButton.texture, BarHeight, BarHeight);
                            QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);
                            EditorSettings.HierarchyManager_UIButton_ShowIcon = QUI.QToggle("UIButton Icon", EditorSettings.HierarchyManager_UIButton_ShowIcon, BarHeight);
                            QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);
                            EditorSettings.HierarchyManager_UIButton_ShowButtonCategory = QUI.QToggle("Button Category", EditorSettings.HierarchyManager_UIButton_ShowButtonCategory, BarHeight);
                            QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);
                            EditorSettings.HierarchyManager_UIButton_ShowButtonName = QUI.QToggle("Button Name", EditorSettings.HierarchyManager_UIButton_ShowButtonName, BarHeight);
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            QUI.DrawTexture(DUIResources.iconUIElement.texture, BarHeight, BarHeight);
                            QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);
                            EditorSettings.HierarchyManager_UIElement_ShowIcon = QUI.QToggle("UIElement Icon", EditorSettings.HierarchyManager_UIElement_ShowIcon, BarHeight);
                            QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);
                            EditorSettings.HierarchyManager_UIElement_ShowElementCategory = QUI.QToggle("Element Category", EditorSettings.HierarchyManager_UIElement_ShowElementCategory, BarHeight);
                            QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);
                            EditorSettings.HierarchyManager_UIElement_ShowElementName = QUI.QToggle("Element Name", EditorSettings.HierarchyManager_UIElement_ShowElementName, BarHeight);
                            QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);
                            EditorSettings.HierarchyManager_UIElement_ShowSortingLayerNameAndOrder = QUI.QToggle("Sorting Layer Name and Order", EditorSettings.HierarchyManager_UIElement_ShowSortingLayerNameAndOrder, BarHeight);
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            QUI.DrawTexture(DUIResources.iconUIEffect.texture, BarHeight, BarHeight);
                            QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);
                            EditorSettings.HierarchyManager_UIEffect_ShowIcon = QUI.QToggle("UIEffect Icon", EditorSettings.HierarchyManager_UIEffect_ShowIcon, BarHeight);
                            QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);
                            EditorSettings.HierarchyManager_UIEffect_ShowSortingLayerNameAndOrder = QUI.QToggle("Sorting Layer Name and Order", EditorSettings.HierarchyManager_UIEffect_ShowSortingLayerNameAndOrder, BarHeight);
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            QUI.DrawTexture(DUIResources.iconUITrigger.texture, BarHeight, BarHeight);
                            QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);
                            EditorSettings.HierarchyManager_UITrigger_ShowIcon = QUI.QToggle("UITrigger Icon", EditorSettings.HierarchyManager_UITrigger_ShowIcon, BarHeight);
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            QUI.DrawTexture(DUIResources.iconUINotification.texture, BarHeight, BarHeight);
                            QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);
                            EditorSettings.HierarchyManager_UINotification_ShowIcon = QUI.QToggle("UINotification Icon", EditorSettings.HierarchyManager_UINotification_ShowIcon, BarHeight);
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            QUI.DrawTexture(DUIResources.iconUIManager.texture, BarHeight, BarHeight);
                            QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);
                            EditorSettings.HierarchyManager_UIManager_ShowIcon = QUI.QToggle("UIManager Icon", EditorSettings.HierarchyManager_UIManager_ShowIcon, BarHeight);
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            QUI.DrawTexture(DUIResources.iconSoundy.texture, BarHeight, BarHeight);
                            QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);
                            EditorSettings.HierarchyManager_Soundy_ShowIcon = QUI.QToggle("Soundy Icon", EditorSettings.HierarchyManager_Soundy_ShowIcon, BarHeight);
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            QUI.DrawTexture(DUIResources.iconUINotificationManager.texture, BarHeight, BarHeight);
                            QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);
                            EditorSettings.HierarchyManager_UINotificationManager_ShowIcon = QUI.QToggle("UINotificationManager Icon", EditorSettings.HierarchyManager_UINotificationManager_ShowIcon, BarHeight);
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            QUI.DrawTexture(DUIResources.iconOrientationManager.texture, BarHeight, BarHeight);
                            QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);
                            EditorSettings.HierarchyManager_OrientationManager_ShowIcon = QUI.QToggle("OrientationManager Icon", EditorSettings.HierarchyManager_OrientationManager_ShowIcon, BarHeight);
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            QUI.DrawTexture(DUIResources.iconSceneLoader.texture, BarHeight, BarHeight);
                            QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);
                            EditorSettings.HierarchyManager_SceneLoader_ShowIcon = QUI.QToggle("SceneLoader Icon", EditorSettings.HierarchyManager_SceneLoader_ShowIcon, BarHeight);
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            QUI.DrawTexture(DUIResources.iconPlayMakerEventDispatcher.texture, BarHeight, BarHeight);
                            QUI.Space(SPACE_4 * HierarchyManagerAnimBool.faded);
                            EditorSettings.HierarchyManager_PlaymakerEventDispatcher_ShowIcon = QUI.QToggle("PlaymakerEventDispatcher Icon", EditorSettings.HierarchyManager_PlaymakerEventDispatcher_ShowIcon, BarHeight);
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_16);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();
        }

        void DrawSettingsUIElement(float width)
        {
            if(QUI.GhostBar("UIElement", QColors.Color.Blue, UIElementAnimBool, width, BigBarHeight))
            {
                UIElementAnimBool.target = !UIElementAnimBool.target;
            }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * UIElementAnimBool.faded);
                if(QUI.BeginFadeGroup(UIElementAnimBool.faded))
                {
                    QUI.BeginVertical(tempWidth);
                    {
                        QUI.Space(SPACE_4 * UIElementAnimBool.faded);

                        QUI.DrawIconBar("Inspector Settings", QResources.iconInfo, QColors.Color.Blue, IconPosition.Right, tempWidth);

                        QUI.Space(SPACE_2 * UIElementAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            EditorSettings.UIElement_Inspector_ShowButtonRenameGameObject = QUI.QToggle("Show Rename Button", EditorSettings.UIElement_Inspector_ShowButtonRenameGameObject, 20, Style.Text.Small);

                            QUI.Space(SPACE_4 * UIElementAnimBool.faded);

                            QLabel.text = "Rename Prefix";
                            QLabel.style = Style.Text.Small;
                            tempFloat = QLabel.x + 120 + 8;
                            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIElement_Inspector_ShowButtonRenameGameObject ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                            QUI.Space(-tempFloat - 12);
                            QUI.BeginVertical(QLabel.x, 20);
                            {
                                QUI.Space((20 - 16) / 2);
                                QUI.Label(QLabel);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();
                            EditorSettings.UIElement_Inspector_RenameGameObjectPrefix = EditorGUILayout.DelayedTextField(EditorSettings.UIElement_Inspector_RenameGameObjectPrefix, GUILayout.Width(120));

                            QUI.Space(SPACE_4);
                            QUI.Space(SPACE_4 * UIElementAnimBool.faded);

                            QLabel.text = "Rename Sufix";
                            QLabel.style = Style.Text.Small;
                            tempFloat = QLabel.x + 120 + 8;
                            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIElement_Inspector_ShowButtonRenameGameObject ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                            QUI.Space(-tempFloat - 12);
                            QUI.BeginVertical(QLabel.x, 20);
                            {
                                QUI.Space((20 - 16) / 2);
                                QUI.Label(QLabel);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();
                            EditorSettings.UIElement_Inspector_RenameGameObjectSuffix = EditorGUILayout.DelayedTextField(EditorSettings.UIElement_Inspector_RenameGameObjectSuffix, GUILayout.Width(120));

                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_2 * UIElementAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            EditorSettings.UIElement_Inspector_HideLoopAnimations = QUI.QToggle("Hide in Inspector - Loop Animations", EditorSettings.UIElement_Inspector_HideLoopAnimations, 20, Style.Text.Small);
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_16 * UIElementAnimBool.faded);

                        QUI.DrawIconBar("Default Values", QResources.iconInfo, QColors.Color.Blue, IconPosition.Right, tempWidth);

                        QUI.Space(SPACE_2 * UIElementAnimBool.faded);

                        if(UIManager.useOrientationManager)
                        {
                            QUI.BeginHorizontal(tempWidth);
                            {
                                QUI.LabelWithBackground("Orientation", Style.Text.Small, 18);
                                QUI.Space(SPACE_4 * UIElementAnimBool.faded);
                                EditorSettings.UIElement_LANDSCAPE = QUI.QToggle("LANDSCAPE", EditorSettings.UIElement_LANDSCAPE);
                                QUI.Space(SPACE_4 * UIElementAnimBool.faded);
                                EditorSettings.UIElement_PORTRAIT = QUI.QToggle("PORTRAIT", EditorSettings.UIElement_PORTRAIT);
                                QUI.FlexibleSpace();
                            }
                            QUI.EndHorizontal();

                            QUI.Space(SPACE_4 * UIElementAnimBool.faded);
                        }

                        QUI.BeginHorizontal(tempWidth);
                        {
                            if(EditorSettings.UIElement_animateAtStart) { GUI.enabled = false; }
                            EditorSettings.UIElement_startHidden = QUI.QToggle("hide @START", EditorSettings.UIElement_startHidden);
                            GUI.enabled = true;

                            QUI.Space(SPACE_4 * UIElementAnimBool.faded);

                            if(EditorSettings.UIElement_startHidden) { GUI.enabled = false; }
                            EditorSettings.UIElement_animateAtStart = QUI.QToggle("animate @START", EditorSettings.UIElement_animateAtStart);
                            GUI.enabled = true;

                            QUI.Space(SPACE_4 * UIElementAnimBool.faded);

                            EditorSettings.UIElement_disableWhenHidden = QUI.QToggle("disable when hidden", EditorSettings.UIElement_disableWhenHidden);
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_4 * UIElementAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            QLabel.text = "custom start position";
                            QLabel.style = Style.Text.Normal;

                            EditorSettings.UIElement_useCustomStartAnchoredPosition = QUI.QToggle("custom start position", EditorSettings.UIElement_useCustomStartAnchoredPosition, 20);

                            QUI.Space(SPACE_4 * UIElementAnimBool.faded);

                            tempFloat = width * 0.4f;
                            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIElement_useCustomStartAnchoredPosition ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 12, 20);
                            QUI.Space(-tempFloat - 12);

                            DUI.DUISettings.UIElement_customStartAnchoredPosition = EditorGUILayout.Vector3Field("", DUI.DUISettings.UIElement_customStartAnchoredPosition, GUILayout.Width(tempFloat));

                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_4 * UIElementAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            //EXECUTE LAYOUT FIX
                            QLabel.text = "execute layout fix"; //Label 1 text
                            QLabel.style = Style.Text.Normal;
                            tempFloat = QLabel.x; //calculate the background width by setting it to Label 1 text width

                            QLabel.text = "(useful in some cases)"; //Label 2 text
                            QLabel.style = Style.Text.Help;
                            tempFloat += QLabel.x; //add the second label width to the background width to cover both of them

                            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIElement_executeLayoutFix ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16 + 16, 18); //draw background by adding the Toggle width and the extra spaces
                            QUI.Space(-tempFloat - 12 - 16);

                            QLabel.text = "execute layout fix";
                            QLabel.style = Style.Text.Normal;
                            QUI.Space(SPACE_2);
                            EditorSettings.UIElement_executeLayoutFix = QUI.Toggle(EditorSettings.UIElement_executeLayoutFix); //Toggle
                            QUI.BeginVertical(QLabel.x + 8, QUI.SingleLineHeight);
                            {
                                QUI.Label(QLabel); //Label 1
                                QUI.Space(SPACE_2);
                            }
                            QUI.EndVertical();

                            QUI.Space(-SPACE_4);

                            QLabel.text = "(useful in some cases)";
                            QLabel.style = Style.Text.Help;
                            QUI.BeginVertical(QLabel.x + 8, QUI.SingleLineHeight);
                            {
                                QUI.Label(QLabel); //Label 2
                                QUI.Space(1);
                            }
                            QUI.EndVertical();

                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_8 * UIElementAnimBool.faded);

                        QUI.LabelWithBackground("In Animations");
                        QUI.Space(SPACE_2 * UIElementAnimBool.faded);
                        QUI.BeginHorizontal(tempWidth);
                        {
                            QLabel.text = "Preset Category";
                            QLabel.style = Style.Text.Small;
                            tempFloat = QLabel.x + 120 + 8;
                            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIElement_loadInAnimationsPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                            QUI.Space(-tempFloat - 12);
                            QUI.BeginVertical(QLabel.x, 20);
                            {
                                QUI.Space((20 - 16) / 2);
                                QUI.Label(QLabel);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();
                            EditorSettings.UIElement_inAnimationsPresetCategoryName = EditorGUILayout.DelayedTextField(EditorSettings.UIElement_inAnimationsPresetCategoryName, GUILayout.Width(120));

                            QUI.Space(SPACE_4);
                            QUI.Space(SPACE_4 * UIElementAnimBool.faded);

                            QLabel.text = "Preset Name";
                            QLabel.style = Style.Text.Small;
                            tempFloat = QLabel.x + 120 + 8;
                            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIElement_loadInAnimationsPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                            QUI.Space(-tempFloat - 12);
                            QUI.BeginVertical(QLabel.x, 20);
                            {
                                QUI.Space((20 - 16) / 2);
                                QUI.Label(QLabel);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();
                            EditorSettings.UIElement_inAnimationsPresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIElement_inAnimationsPresetName, GUILayout.Width(120));

                            QUI.Space(SPACE_4);
                            QUI.Space(SPACE_4 * UIElementAnimBool.faded);

                            EditorSettings.UIElement_loadInAnimationsPresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIElement_loadInAnimationsPresetAtRuntime, 20, Style.Text.Small);

                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_8 * UIElementAnimBool.faded);

                        QUI.LabelWithBackground("Out Animations");
                        QUI.Space(SPACE_2 * UIElementAnimBool.faded);
                        QUI.BeginHorizontal(tempWidth);
                        {
                            QLabel.text = "Preset Category";
                            QLabel.style = Style.Text.Small;
                            tempFloat = QLabel.x + 120 + 8;
                            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIElement_loadOutAnimationsPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                            QUI.Space(-tempFloat - 12);
                            QUI.BeginVertical(QLabel.x, 20);
                            {
                                QUI.Space((20 - 16) / 2);
                                QUI.Label(QLabel);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();
                            EditorSettings.UIElement_outAnimationsPresetCategoryName = EditorGUILayout.DelayedTextField(EditorSettings.UIElement_outAnimationsPresetCategoryName, GUILayout.Width(120));

                            QUI.Space(SPACE_4);
                            QUI.Space(SPACE_4 * UIElementAnimBool.faded);

                            QLabel.text = "Preset Name";
                            QLabel.style = Style.Text.Small;
                            tempFloat = QLabel.x + 120 + 8;
                            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIElement_loadOutAnimationsPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                            QUI.Space(-tempFloat - 12);
                            QUI.BeginVertical(QLabel.x, 20);
                            {
                                QUI.Space((20 - 16) / 2);
                                QUI.Label(QLabel);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();
                            EditorSettings.UIElement_outAnimationsPresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIElement_outAnimationsPresetName, GUILayout.Width(120));

                            QUI.Space(SPACE_4);
                            QUI.Space(SPACE_4 * UIElementAnimBool.faded);

                            EditorSettings.UIElement_loadOutAnimationsPresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIElement_loadOutAnimationsPresetAtRuntime, 20, Style.Text.Small);

                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_8 * UIElementAnimBool.faded);

                        QUI.LabelWithBackground("Loop Animations");
                        QUI.Space(SPACE_2 * UIElementAnimBool.faded);
                        QUI.BeginHorizontal(tempWidth);
                        {
                            QLabel.text = "Preset Category";
                            QLabel.style = Style.Text.Small;
                            tempFloat = QLabel.x + 120 + 8;
                            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIElement_loadOutAnimationsPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                            QUI.Space(-tempFloat - 12);
                            QUI.BeginVertical(QLabel.x, 20);
                            {
                                QUI.Space((20 - 16) / 2);
                                QUI.Label(QLabel);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();
                            EditorSettings.UIElement_outAnimationsPresetCategoryName = EditorGUILayout.DelayedTextField(EditorSettings.UIElement_outAnimationsPresetCategoryName, GUILayout.Width(120));

                            QUI.Space(SPACE_4);
                            QUI.Space(SPACE_4 * UIElementAnimBool.faded);

                            QLabel.text = "Preset Name";
                            QLabel.style = Style.Text.Small;
                            tempFloat = QLabel.x + 120 + 8;
                            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIElement_loadOutAnimationsPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                            QUI.Space(-tempFloat - 12);
                            QUI.BeginVertical(QLabel.x, 20);
                            {
                                QUI.Space((20 - 16) / 2);
                                QUI.Label(QLabel);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();
                            EditorSettings.UIElement_outAnimationsPresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIElement_outAnimationsPresetName, GUILayout.Width(120));

                            QUI.Space(SPACE_4);
                            QUI.Space(SPACE_4 * UIElementAnimBool.faded);

                            EditorSettings.UIElement_loadOutAnimationsPresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIElement_loadOutAnimationsPresetAtRuntime, 20, Style.Text.Small);

                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_16);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();
        }

        void DrawSettingsUIButton(float width)
        {
            if(QUI.GhostBar("UIButton", QColors.Color.Blue, UIButtonAnimBool, width, BigBarHeight))
            {
                UIButtonAnimBool.target = !UIButtonAnimBool.target;
            }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * UIButtonAnimBool.faded);
                if(QUI.BeginFadeGroup(UIButtonAnimBool.faded))
                {
                    QUI.BeginVertical(tempWidth);
                    {
                        QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                        QUI.DrawIconBar("Inspector Settings", QResources.iconInfo, QColors.Color.Blue, IconPosition.Right, tempWidth);

                        QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            EditorSettings.UIButton_Inspector_ShowButtonRenameGameObject = QUI.QToggle("Show Rename Button", EditorSettings.UIButton_Inspector_ShowButtonRenameGameObject, 20, Style.Text.Small);

                            QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                            QLabel.text = "Rename Prefix";
                            QLabel.style = Style.Text.Small;
                            tempFloat = QLabel.x + 120 + 8;
                            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_Inspector_ShowButtonRenameGameObject ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                            QUI.Space(-tempFloat - 12);
                            QUI.BeginVertical(QLabel.x, 20);
                            {
                                QUI.Space((20 - 16) / 2);
                                QUI.Label(QLabel);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();
                            EditorSettings.UIButton_Inspector_RenameGameObjectPrefix = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_Inspector_RenameGameObjectPrefix, GUILayout.Width(120));

                            QUI.Space(SPACE_4);
                            QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                            QLabel.text = "Rename Sufix";
                            QLabel.style = Style.Text.Small;
                            tempFloat = QLabel.x + 120 + 8;
                            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_Inspector_ShowButtonRenameGameObject ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                            QUI.Space(-tempFloat - 12);
                            QUI.BeginVertical(QLabel.x, 20);
                            {
                                QUI.Space((20 - 16) / 2);
                                QUI.Label(QLabel);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();
                            EditorSettings.UIButton_Inspector_RenameGameObjectSuffix = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_Inspector_RenameGameObjectSuffix, GUILayout.Width(120));

                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            EditorSettings.UIButton_Inspector_HideOnPointerEnter = QUI.QToggle("Hide in Inspector - OnPointer ENTER", EditorSettings.UIButton_Inspector_HideOnPointerEnter, 20, Style.Text.Small);
                            QUI.Space(SPACE_4 * UIButtonAnimBool.faded);
                            EditorSettings.UIButton_Inspector_HideOnPointerExit = QUI.QToggle("Hide in Inspector - OnPointer EXIT", EditorSettings.UIButton_Inspector_HideOnPointerExit, 20, Style.Text.Small);
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            EditorSettings.UIButton_Inspector_HideOnPointerDown = QUI.QToggle("Hide in Inspector - OnPointer DOWN", EditorSettings.UIButton_Inspector_HideOnPointerDown, 20, Style.Text.Small);
                            QUI.Space(SPACE_4 * UIButtonAnimBool.faded);
                            EditorSettings.UIButton_Inspector_HideOnPointerUp = QUI.QToggle("Hide in Inspector - OnPointer UP", EditorSettings.UIButton_Inspector_HideOnPointerUp, 20, Style.Text.Small);
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            EditorSettings.UIButton_Inspector_HideOnClick = QUI.QToggle("Hide in Inspector - OnClick", EditorSettings.UIButton_Inspector_HideOnClick, 20, Style.Text.Small);
                            QUI.Space(SPACE_4 * UIButtonAnimBool.faded);
                            EditorSettings.UIButton_Inspector_HideOnDoubleClick = QUI.QToggle("Hide in Inspector - OnDoubleClick", EditorSettings.UIButton_Inspector_HideOnDoubleClick, 20, Style.Text.Small);
                            QUI.Space(SPACE_4 * UIButtonAnimBool.faded);
                            EditorSettings.UIButton_Inspector_HideOnLongClick = QUI.QToggle("Hide in Inspector - OnLongClick", EditorSettings.UIButton_Inspector_HideOnLongClick, 20, Style.Text.Small);

                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            EditorSettings.UIButton_Inspector_HideNormalLoop = QUI.QToggle("Hide in Inspector - Normal Loop Animations", EditorSettings.UIButton_Inspector_HideNormalLoop, 20, Style.Text.Small);
                            QUI.Space(SPACE_4 * UIButtonAnimBool.faded);
                            EditorSettings.UIButton_Inspector_HideSelectedLoop = QUI.QToggle("Hide in Inspector - Selected Loop Animations", EditorSettings.UIButton_Inspector_HideSelectedLoop, 20, Style.Text.Small);
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_16 * UIButtonAnimBool.faded);

                        QUI.DrawIconBar("Default Values", QResources.iconInfo, QColors.Color.Blue, IconPosition.Right, tempWidth);

                        QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            EditorSettings.UIButton_allowMultipleClicks = QUI.QToggle("allow multiple clicks", EditorSettings.UIButton_allowMultipleClicks, 20);
                            QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                            QLabel.text = "disable button interval";
                            QLabel.style = Style.Text.Normal;
                            tempFloat = QLabel.x + 40 + 8;
                            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                            QUI.Space(-tempFloat - 12);
                            QUI.BeginVertical(QLabel.x, 20);
                            {
                                QUI.Space((20 - 18) / 2);
                                QUI.Label(QLabel);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();
                            EditorSettings.UIButton_disableButtonInterval = EditorGUILayout.DelayedFloatField(EditorSettings.UIButton_disableButtonInterval, GUILayout.Width(40));

                            QUI.Space(SPACE_4);
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            EditorSettings.UIButton_deselectButtonOnClick = QUI.QToggle("deselect button on click", EditorSettings.UIButton_deselectButtonOnClick, 20);
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_8 * UIButtonAnimBool.faded);

                        DrawSettingsUIButtonOnPointerEnter(tempWidth);
                        QUI.Space(SPACE_8 * UIButtonAnimBool.faded);

                        DrawSettingsUIButtonOnPointerExit(tempWidth);
                        QUI.Space(SPACE_8 * UIButtonAnimBool.faded);

                        DrawSettingsUIButtonOnPointerDown(tempWidth);
                        QUI.Space(SPACE_8 * UIButtonAnimBool.faded);

                        DrawSettingsUIButtonOnPointerUp(tempWidth);
                        QUI.Space(SPACE_8 * UIButtonAnimBool.faded);

                        DrawSettingsUIButtonOnClick(tempWidth);
                        QUI.Space(SPACE_8 * UIButtonAnimBool.faded);

                        DrawSettingsUIButtonOnDoubleClick(tempWidth);
                        QUI.Space(SPACE_8 * UIButtonAnimBool.faded);

                        DrawSettingsUIButtonOnLongClick(tempWidth);
                        QUI.Space(SPACE_16 * UIButtonAnimBool.faded);

                        DrawSettingsUIButtonNormalLoop(tempWidth);
                        QUI.Space(SPACE_8 * UIButtonAnimBool.faded);

                        DrawSettingsUIButtonSelectedLoop(tempWidth);
                        QUI.Space(SPACE_16);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();
        }
        void DrawSettingsUIButtonOnPointerEnter(float width)
        {
            QUI.LabelWithBackground("OnPointer ENTER");

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width);
            {

                EditorSettings.UIButton_useOnPointerEnter = QUI.QToggle("enabled", EditorSettings.UIButton_useOnPointerEnter, 20, Style.Text.Small);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                QLabel.text = "disable interval";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 40 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 18) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerEnterDisableInterval = EditorGUILayout.DelayedFloatField(EditorSettings.UIButton_onPointerEnterDisableInterval, GUILayout.Width(40));

                QUI.Space(SPACE_4);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width - INDENT_24);
            {
                QLabel.text = "Play Sound";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerEnterSound = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onPointerEnterSound, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_customOnPointerEnterSound = QUI.QToggle("custom", EditorSettings.UIButton_customOnPointerEnterSound, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width - INDENT_24);
            {
                QLabel.text = "Animation Type";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();

                EditorSettings.UIButton_onPointerEnterAnimationType = (UIButton.ButtonAnimationType) EditorGUILayout.EnumPopup(EditorSettings.UIButton_onPointerEnterAnimationType, GUILayout.Width(120));

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(tempWidth);
            {
                QLabel.text = "Punch Preset Category";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnPointerEnterPunchPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerEnterPunchPresetCategory = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onPointerEnterPunchPresetCategory, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                QLabel.text = "Punch Preset Name";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnPointerEnterPunchPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerEnterPunchPresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onPointerEnterPunchPresetName, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_loadOnPointerEnterPunchPresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIButton_loadOnPointerEnterPunchPresetAtRuntime, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(tempWidth);
            {
                QLabel.text = "State Preset Category";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnPointerEnterStatePresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerEnterStatePresetCategory = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onPointerEnterStatePresetCategory, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                QLabel.text = "State Preset Name";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnPointerEnterStatePresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerEnterStatePresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onPointerEnterStatePresetName, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_loadOnPointerEnterStatePresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIButton_loadOnPointerEnterStatePresetAtRuntime, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawSettingsUIButtonOnPointerExit(float width)
        {
            QUI.LabelWithBackground("OnPointer EXIT");

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width);
            {

                EditorSettings.UIButton_useOnPointerExit = QUI.QToggle("enabled", EditorSettings.UIButton_useOnPointerExit, 20, Style.Text.Small);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                QLabel.text = "disable interval";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 40 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 18) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerExitDisableInterval = EditorGUILayout.DelayedFloatField(EditorSettings.UIButton_onPointerExitDisableInterval, GUILayout.Width(40));

                QUI.Space(SPACE_4);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width - INDENT_24);
            {
                QLabel.text = "Play Sound";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerExitSound = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onPointerExitSound, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_customOnPointerExitSound = QUI.QToggle("custom", EditorSettings.UIButton_customOnPointerExitSound, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width - INDENT_24);
            {
                QLabel.text = "Animation Type";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();

                EditorSettings.UIButton_onPointerExitAnimationType = (UIButton.ButtonAnimationType)EditorGUILayout.EnumPopup(EditorSettings.UIButton_onPointerExitAnimationType, GUILayout.Width(120));

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(tempWidth);
            {
                QLabel.text = "Punch Preset Category";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnPointerExitPunchPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerExitPunchPresetCategory = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onPointerExitPunchPresetCategory, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                QLabel.text = "Punch Preset Name";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnPointerExitPunchPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerExitPunchPresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onPointerExitPunchPresetName, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_loadOnPointerExitPunchPresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIButton_loadOnPointerExitPunchPresetAtRuntime, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(tempWidth);
            {
                QLabel.text = "State Preset Category";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnPointerExitStatePresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerExitStatePresetCategory = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onPointerExitStatePresetCategory, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                QLabel.text = "State Preset Name";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnPointerExitStatePresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerExitStatePresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onPointerExitStatePresetName, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_loadOnPointerExitStatePresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIButton_loadOnPointerExitStatePresetAtRuntime, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawSettingsUIButtonOnPointerDown(float width)
        {
            QUI.LabelWithBackground("OnPointer DOWN");

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width);
            {
                EditorSettings.UIButton_useOnPointerDown = QUI.QToggle("enabled", EditorSettings.UIButton_useOnPointerDown, 20, Style.Text.Small);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width - INDENT_24);
            {
                QLabel.text = "Play Sound";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerDownSound = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onPointerDownSound, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_customOnPointerDownSound = QUI.QToggle("custom", EditorSettings.UIButton_customOnPointerDownSound, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width - INDENT_24);
            {
                QLabel.text = "Animation Type";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();

                EditorSettings.UIButton_onPointerDownAnimationType = (UIButton.ButtonAnimationType)EditorGUILayout.EnumPopup(EditorSettings.UIButton_onPointerDownAnimationType, GUILayout.Width(120));

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(tempWidth);
            {
                QLabel.text = "Punch Preset Category";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnPointerDownPunchPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerDownPunchPresetCategory = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onPointerDownPunchPresetCategory, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                QLabel.text = "Punch Preset Name";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnPointerDownPunchPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerDownPunchPresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onPointerDownPunchPresetName, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_loadOnPointerDownPunchPresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIButton_loadOnPointerDownPunchPresetAtRuntime, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(tempWidth);
            {
                QLabel.text = "State Preset Category";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnPointerDownStatePresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerDownStatePresetCategory = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onPointerDownStatePresetCategory, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                QLabel.text = "State Preset Name";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnPointerDownStatePresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerDownStatePresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onPointerDownStatePresetName, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_loadOnPointerDownStatePresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIButton_loadOnPointerDownStatePresetAtRuntime, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawSettingsUIButtonOnPointerUp(float width)
        {
            QUI.LabelWithBackground("OnPointer UP");

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width);
            {
                EditorSettings.UIButton_useOnPointerUp = QUI.QToggle("enabled", EditorSettings.UIButton_useOnPointerUp, 20, Style.Text.Small);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width - INDENT_24);
            {
                QLabel.text = "Play Sound";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerUpSound = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onPointerUpSound, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_customOnPointerUpSound = QUI.QToggle("custom", EditorSettings.UIButton_customOnPointerUpSound, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width - INDENT_24);
            {
                QLabel.text = "Animation Type";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();

                EditorSettings.UIButton_onPointerUpAnimationType = (UIButton.ButtonAnimationType)EditorGUILayout.EnumPopup(EditorSettings.UIButton_onPointerUpAnimationType, GUILayout.Width(120));

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(tempWidth);
            {
                QLabel.text = "Punch Preset Category";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnPointerUpPunchPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerUpPunchPresetCategory = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onPointerUpPunchPresetCategory, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                QLabel.text = "Punch Preset Name";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnPointerUpPunchPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerUpPunchPresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onPointerUpPunchPresetName, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_loadOnPointerUpPunchPresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIButton_loadOnPointerUpPunchPresetAtRuntime, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(tempWidth);
            {
                QLabel.text = "State Preset Category";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnPointerUpStatePresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerUpStatePresetCategory = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onPointerUpStatePresetCategory, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                QLabel.text = "State Preset Name";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnPointerUpStatePresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onPointerUpStatePresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onPointerUpStatePresetName, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_loadOnPointerUpStatePresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIButton_loadOnPointerUpStatePresetAtRuntime, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawSettingsUIButtonOnClick(float width)
        {
            QUI.LabelWithBackground("OnClick");

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width);
            {
                EditorSettings.UIButton_useOnClickAnimations = QUI.QToggle("enabled", EditorSettings.UIButton_useOnClickAnimations, 20, Style.Text.Small);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);
                EditorSettings.UIButton_waitForOnClickAnimation = QUI.QToggle("wait for animation", EditorSettings.UIButton_waitForOnClickAnimation, 20, Style.Text.Small);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);
                EditorSettings.UIButton_singleClickMode = (UIButton.SingleClickMode)EditorGUILayout.EnumPopup(EditorSettings.UIButton_singleClickMode, GUILayout.Width(80));
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width - INDENT_24);
            {
                QLabel.text = "Play Sound";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onClickSound = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onClickSound, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_customOnClickSound = QUI.QToggle("custom", EditorSettings.UIButton_customOnClickSound, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width - INDENT_24);
            {
                QLabel.text = "Animation Type";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();

                EditorSettings.UIButton_onClickAnimationType = (UIButton.ButtonAnimationType)EditorGUILayout.EnumPopup(EditorSettings.UIButton_onClickAnimationType, GUILayout.Width(120));

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(tempWidth);
            {
                QLabel.text = "Punch Preset Category";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnClickPunchPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onClickPunchPresetCategory = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onClickPunchPresetCategory, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                QLabel.text = "Punch Preset Name";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnClickPunchPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onClickPunchPresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onClickPunchPresetName, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_loadOnClickPunchPresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIButton_loadOnClickPunchPresetAtRuntime, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(tempWidth);
            {
                QLabel.text = "State Preset Category";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnClickStatePresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onClickStatePresetCategory = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onClickStatePresetCategory, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                QLabel.text = "State Preset Name";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnClickStatePresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onClickStatePresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onClickStatePresetName, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_loadOnClickStatePresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIButton_loadOnClickStatePresetAtRuntime, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawSettingsUIButtonOnDoubleClick(float width)
        {
            QUI.LabelWithBackground("OnDoubleClick");

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width);
            {
                EditorSettings.UIButton_useOnDoubleClick = QUI.QToggle("enabled", EditorSettings.UIButton_useOnDoubleClick, 20, Style.Text.Small);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);
                EditorSettings.UIButton_waitForOnDoubleClickAnimation = QUI.QToggle("wait for animation", EditorSettings.UIButton_waitForOnDoubleClickAnimation, 20, Style.Text.Small);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);
                QLabel.text = "register interval";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 40 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 18) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_doubleClickRegisterInterval = EditorGUILayout.DelayedFloatField(EditorSettings.UIButton_doubleClickRegisterInterval, GUILayout.Width(40));
                QUI.Space(SPACE_4);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width - INDENT_24);
            {
                QLabel.text = "Play Sound";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onDoubleClickSound = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onDoubleClickSound, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_customOnDoubleClickSound = QUI.QToggle("custom", EditorSettings.UIButton_customOnDoubleClickSound, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width - INDENT_24);
            {
                QLabel.text = "Animation Type";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();

                EditorSettings.UIButton_onDoubleClickAnimationType = (UIButton.ButtonAnimationType)EditorGUILayout.EnumPopup(EditorSettings.UIButton_onDoubleClickAnimationType, GUILayout.Width(120));

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(tempWidth);
            {
                QLabel.text = "Punch Preset Category";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnDoubleClickPunchPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onDoubleClickPunchPresetCategory = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onDoubleClickPunchPresetCategory, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                QLabel.text = "Punch Preset Name";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnDoubleClickPunchPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onDoubleClickPunchPresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onDoubleClickPunchPresetName, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_loadOnDoubleClickPunchPresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIButton_loadOnDoubleClickPunchPresetAtRuntime, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(tempWidth);
            {
                QLabel.text = "State Preset Category";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnDoubleClickStatePresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onDoubleClickStatePresetCategory = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onDoubleClickStatePresetCategory, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                QLabel.text = "State Preset Name";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnDoubleClickStatePresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onDoubleClickStatePresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onDoubleClickStatePresetName, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_loadOnDoubleClickStatePresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIButton_loadOnDoubleClickStatePresetAtRuntime, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawSettingsUIButtonOnLongClick(float width)
        {
            QUI.LabelWithBackground("OnLongClick");

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width);
            {
                EditorSettings.UIButton_useOnLongClick = QUI.QToggle("enabled", EditorSettings.UIButton_useOnLongClick, 20, Style.Text.Small);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);
                EditorSettings.UIButton_waitForOnLongClickAnimation = QUI.QToggle("wait for animation", EditorSettings.UIButton_waitForOnLongClickAnimation, 20, Style.Text.Small);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);
                QLabel.text = "register interval";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 40 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 18) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_longClickRegisterInterval = EditorGUILayout.DelayedFloatField(EditorSettings.UIButton_longClickRegisterInterval, GUILayout.Width(40));
                QUI.Space(SPACE_4);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width - INDENT_24);
            {
                QLabel.text = "Play Sound";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onLongClickSound = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onLongClickSound, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_customOnLongClickSound = QUI.QToggle("custom", EditorSettings.UIButton_customOnLongClickSound, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(width - INDENT_24);
            {
                QLabel.text = "Animation Type";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();

                EditorSettings.UIButton_onLongClickAnimationType = (UIButton.ButtonAnimationType)EditorGUILayout.EnumPopup(EditorSettings.UIButton_onLongClickAnimationType, GUILayout.Width(120));

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(tempWidth);
            {
                QLabel.text = "Punch Preset Category";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnLongClickPunchPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onLongClickPunchPresetCategory = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onLongClickPunchPresetCategory, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                QLabel.text = "Punch Preset Name";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnLongClickPunchPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onLongClickPunchPresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onLongClickPunchPresetName, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_loadOnLongClickPunchPresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIButton_loadOnLongClickPunchPresetAtRuntime, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(tempWidth);
            {
                QLabel.text = "State Preset Category";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnLongClickStatePresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onLongClickStatePresetCategory = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onLongClickStatePresetCategory, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                QLabel.text = "State Preset Name";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadOnLongClickStatePresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_onLongClickStatePresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_onLongClickStatePresetName, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_loadOnLongClickStatePresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIButton_loadOnLongClickStatePresetAtRuntime, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawSettingsUIButtonNormalLoop(float width)
        {
            QUI.LabelWithBackground("Normal Loop Animations");

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(tempWidth);
            {
                QLabel.text = "Punch Preset Category";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadNormalLoopPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_normalLoopPresetCategory = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_normalLoopPresetCategory, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                QLabel.text = "Punch Preset Name";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadNormalLoopPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_normalLoopPresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_normalLoopPresetName, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_loadNormalLoopPresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIButton_loadNormalLoopPresetAtRuntime, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawSettingsUIButtonSelectedLoop(float width)
        {
            QUI.LabelWithBackground("Selected Loop Animations");

            QUI.Space(SPACE_2 * UIButtonAnimBool.faded);

            QUI.BeginHorizontal(tempWidth);
            {
                QLabel.text = "Punch Preset Category";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadSelectedLoopPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_selectedLoopPresetCategory = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_selectedLoopPresetCategory, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                QLabel.text = "Punch Preset Name";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIButton_loadSelectedLoopPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIButton_selectedLoopPresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIButton_selectedLoopPresetName, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIButtonAnimBool.faded);

                EditorSettings.UIButton_loadSelectedLoopPresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIButton_loadSelectedLoopPresetAtRuntime, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }

        void DrawSettingsUIToggle(float width)
        {
            if(QUI.GhostBar("UIToggle", QColors.Color.Blue, UIToggleAnimBool, width, BigBarHeight))
            {
                UIToggleAnimBool.target = !UIToggleAnimBool.target;
            }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * UIToggleAnimBool.faded);
                if(QUI.BeginFadeGroup(UIToggleAnimBool.faded))
                {
                    QUI.BeginVertical(tempWidth);
                    {
                        QUI.Space(SPACE_4 * UIToggleAnimBool.faded);

                        QUI.DrawIconBar("Inspector Settings", QResources.iconInfo, QColors.Color.Blue, IconPosition.Right, tempWidth);

                        QUI.Space(SPACE_2 * UIToggleAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            EditorSettings.UIToggle_Inspector_HideOnPointerEnter = QUI.QToggle("Hide in Inspector - OnPointer ENTER", EditorSettings.UIToggle_Inspector_HideOnPointerEnter, 20, Style.Text.Small);
                            QUI.Space(SPACE_4 * UIToggleAnimBool.faded);
                            EditorSettings.UIToggle_Inspector_HideOnPointerExit = QUI.QToggle("Hide in Inspector - OnPointer EXIT", EditorSettings.UIToggle_Inspector_HideOnPointerExit, 20, Style.Text.Small);
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_2 * UIToggleAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            EditorSettings.UIToggle_Inspector_HideOnClick = QUI.QToggle("Hide in Inspector - OnClick", EditorSettings.UIToggle_Inspector_HideOnClick, 20, Style.Text.Small);
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_16 * UIToggleAnimBool.faded);

                        QUI.DrawIconBar("Default Values", QResources.iconInfo, QColors.Color.Blue, IconPosition.Right, tempWidth);

                        QUI.Space(SPACE_2 * UIToggleAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            EditorSettings.UIToggle_allowMultipleClicks = QUI.QToggle("allow multiple clicks", EditorSettings.UIToggle_allowMultipleClicks, 20);
                            QUI.Space(SPACE_4 * UIToggleAnimBool.faded);

                            QLabel.text = "disable button interval";
                            QLabel.style = Style.Text.Normal;
                            tempFloat = QLabel.x + 40 + 8;
                            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                            QUI.Space(-tempFloat - 12);
                            QUI.BeginVertical(QLabel.x, 20);
                            {
                                QUI.Space((20 - 18) / 2);
                                QUI.Label(QLabel);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();
                            EditorSettings.UIToggle_disableButtonInterval = EditorGUILayout.DelayedFloatField(EditorSettings.UIToggle_disableButtonInterval, GUILayout.Width(40));

                            QUI.Space(SPACE_4);
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_2 * UIToggleAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            EditorSettings.UIToggle_deselectButtonOnClick = QUI.QToggle("deselect button on click", EditorSettings.UIToggle_deselectButtonOnClick, 20);
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_8 * UIToggleAnimBool.faded);

                        DrawSettingsUIToggleOnPointerEnter(tempWidth);
                        QUI.Space(SPACE_8 * UIToggleAnimBool.faded);

                        DrawSettingsUIToggleOnPointerExit(tempWidth);
                        QUI.Space(SPACE_8 * UIToggleAnimBool.faded);

                        DrawSettingsUIToggleOnClick(tempWidth);
                        QUI.Space(SPACE_16);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();
        }
        void DrawSettingsUIToggleOnPointerEnter(float width)
        {
            QUI.LabelWithBackground("OnPointer ENTER");

            QUI.Space(SPACE_2 * UIToggleAnimBool.faded);

            QUI.BeginHorizontal(width);
            {

                EditorSettings.UIToggle_useOnPointerEnter = QUI.QToggle("enabled", EditorSettings.UIToggle_useOnPointerEnter, 20, Style.Text.Small);
                QUI.Space(SPACE_4 * UIToggleAnimBool.faded);

                QLabel.text = "disable interval";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 40 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 18) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIToggle_onPointerEnterDisableInterval = EditorGUILayout.DelayedFloatField(EditorSettings.UIToggle_onPointerEnterDisableInterval, GUILayout.Width(40));

                QUI.Space(SPACE_4);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIToggleAnimBool.faded);

            QUI.BeginHorizontal(width - INDENT_24);
            {
                QLabel.text = "Toggle ON - Play Sound";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIToggle_onPointerEnterSoundToggleOn = EditorGUILayout.DelayedTextField(EditorSettings.UIToggle_onPointerEnterSoundToggleOn, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIToggleAnimBool.faded);

                EditorSettings.UIToggle_customOnPointerEnterSoundToggleOn = QUI.QToggle("custom", EditorSettings.UIToggle_customOnPointerEnterSoundToggleOn, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIToggleAnimBool.faded);

            QUI.BeginHorizontal(width - INDENT_24);
            {
                QLabel.text = "Toggle OFF - Play Sound";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIToggle_onPointerEnterSoundToggleOff = EditorGUILayout.DelayedTextField(EditorSettings.UIToggle_onPointerEnterSoundToggleOff, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIToggleAnimBool.faded);

                EditorSettings.UIToggle_customOnPointerEnterSoundToggleOff = QUI.QToggle("custom", EditorSettings.UIToggle_customOnPointerEnterSoundToggleOff, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIToggleAnimBool.faded);

            QUI.BeginHorizontal(tempWidth);
            {
                QLabel.text = "Punch Preset Category";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIToggle_loadOnPointerEnterPunchPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIToggle_onPointerEnterPunchPresetCategory = EditorGUILayout.DelayedTextField(EditorSettings.UIToggle_onPointerEnterPunchPresetCategory, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIToggleAnimBool.faded);

                QLabel.text = "Punch Preset Name";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIToggle_loadOnPointerEnterPunchPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIToggle_onPointerEnterPunchPresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIToggle_onPointerEnterPunchPresetName, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIToggleAnimBool.faded);

                EditorSettings.UIToggle_loadOnPointerEnterPunchPresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIToggle_loadOnPointerEnterPunchPresetAtRuntime, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawSettingsUIToggleOnPointerExit(float width)
        {
            QUI.LabelWithBackground("OnPointer EXIT");

            QUI.Space(SPACE_2 * UIToggleAnimBool.faded);

            QUI.BeginHorizontal(width);
            {

                EditorSettings.UIToggle_useOnPointerExit = QUI.QToggle("enabled", EditorSettings.UIToggle_useOnPointerExit, 20, Style.Text.Small);
                QUI.Space(SPACE_4 * UIToggleAnimBool.faded);

                QLabel.text = "disable interval";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 40 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 18) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIToggle_onPointerExitDisableInterval = EditorGUILayout.DelayedFloatField(EditorSettings.UIToggle_onPointerExitDisableInterval, GUILayout.Width(40));

                QUI.Space(SPACE_4);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIToggleAnimBool.faded);

            QUI.BeginHorizontal(width - INDENT_24);
            {
                QLabel.text = "Toggle ON - Play Sound";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIToggle_onPointerExitSoundToggleOn = EditorGUILayout.DelayedTextField(EditorSettings.UIToggle_onPointerExitSoundToggleOn, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIToggleAnimBool.faded);

                EditorSettings.UIToggle_customOnPointerExitSoundToggleOn = QUI.QToggle("custom", EditorSettings.UIToggle_customOnPointerExitSoundToggleOn, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIToggleAnimBool.faded);

            QUI.BeginHorizontal(width - INDENT_24);
            {
                QLabel.text = "Toggle OFF - Play Sound";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIToggle_onPointerExitSoundToggleOff = EditorGUILayout.DelayedTextField(EditorSettings.UIToggle_onPointerExitSoundToggleOff, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIToggleAnimBool.faded);

                EditorSettings.UIToggle_customOnPointerExitSoundToggleOff = QUI.QToggle("custom", EditorSettings.UIToggle_customOnPointerExitSoundToggleOff, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIToggleAnimBool.faded);

            QUI.BeginHorizontal(tempWidth);
            {
                QLabel.text = "Punch Preset Category";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIToggle_loadOnPointerExitPunchPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIToggle_onPointerExitPunchPresetCategory = EditorGUILayout.DelayedTextField(EditorSettings.UIToggle_onPointerExitPunchPresetCategory, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIToggleAnimBool.faded);

                QLabel.text = "Punch Preset Name";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIToggle_loadOnPointerExitPunchPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIToggle_onPointerExitPunchPresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIToggle_onPointerExitPunchPresetName, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIToggleAnimBool.faded);

                EditorSettings.UIToggle_loadOnPointerExitPunchPresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIToggle_loadOnPointerExitPunchPresetAtRuntime, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawSettingsUIToggleOnClick(float width)
        {
            QUI.LabelWithBackground("OnClick");

            QUI.Space(SPACE_2 * UIToggleAnimBool.faded);

            QUI.BeginHorizontal(width);
            {
                EditorSettings.UIToggle_useOnClickAnimations = QUI.QToggle("enabled", EditorSettings.UIToggle_useOnClickAnimations, 20, Style.Text.Small);
                QUI.Space(SPACE_4 * UIToggleAnimBool.faded);
                EditorSettings.UIToggle_waitForOnClickAnimation = QUI.QToggle("wait for animation", EditorSettings.UIToggle_waitForOnClickAnimation, 20, Style.Text.Small);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIToggleAnimBool.faded);

            QUI.BeginHorizontal(width - INDENT_24);
            {
                QLabel.text = "Toggle ON - Play Sound";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIToggle_onClickSoundToggleOn = EditorGUILayout.DelayedTextField(EditorSettings.UIToggle_onClickSoundToggleOn, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIToggleAnimBool.faded);

                EditorSettings.UIToggle_customOnClickSoundToggleOn = QUI.QToggle("custom", EditorSettings.UIToggle_customOnClickSoundToggleOn, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIToggleAnimBool.faded);

            QUI.BeginHorizontal(width - INDENT_24);
            {
                QLabel.text = "Toggle OFF - Play Sound";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIToggle_onClickSoundToggleOff = EditorGUILayout.DelayedTextField(EditorSettings.UIToggle_onClickSoundToggleOff, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIToggleAnimBool.faded);

                EditorSettings.UIToggle_customOnClickSoundToggleOff = QUI.QToggle("custom", EditorSettings.UIToggle_customOnClickSoundToggleOff, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2 * UIToggleAnimBool.faded);

            QUI.BeginHorizontal(tempWidth);
            {
                QLabel.text = "Punch Preset Category";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIToggle_loadOnClickPunchPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIToggle_onClickPunchPresetCategory = EditorGUILayout.DelayedTextField(EditorSettings.UIToggle_onClickPunchPresetCategory, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIToggleAnimBool.faded);

                QLabel.text = "Punch Preset Name";
                QLabel.style = Style.Text.Small;
                tempFloat = QLabel.x + 120 + 8;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIToggle_loadOnClickPunchPresetAtRuntime ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                QUI.Space(-tempFloat - 12);
                QUI.BeginVertical(QLabel.x, 20);
                {
                    QUI.Space((20 - 16) / 2);
                    QUI.Label(QLabel);
                    QUI.Space((20 - 16) / 2);
                }
                QUI.EndVertical();
                EditorSettings.UIToggle_onClickPunchPresetName = EditorGUILayout.DelayedTextField(EditorSettings.UIToggle_onClickPunchPresetName, GUILayout.Width(120));

                QUI.Space(SPACE_4);
                QUI.Space(SPACE_4 * UIToggleAnimBool.faded);

                EditorSettings.UIToggle_loadOnClickPunchPresetAtRuntime = QUI.QToggle("Load preset at runtime", EditorSettings.UIToggle_loadOnClickPunchPresetAtRuntime, 20, Style.Text.Small);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }

        private void DrawSettingsUIEffect(float width)
        {
            if(QUI.GhostBar("UIEffect", QColors.Color.Blue, UIEffectAnimBool, width, BigBarHeight))
            {
                UIEffectAnimBool.target = !UIEffectAnimBool.target;
            }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * UIEffectAnimBool.faded);
                if(QUI.BeginFadeGroup(UIEffectAnimBool.faded))
                {
                    QUI.BeginVertical(tempWidth);
                    {
                        QUI.Space(SPACE_4 * UIEffectAnimBool.faded);

                        QUI.DrawIconBar("Inspector Settings", QResources.iconInfo, QColors.Color.Blue, IconPosition.Right, tempWidth);

                        QUI.Space(SPACE_2 * UIEffectAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            EditorSettings.UIEffect_Inspector_ShowButtonRenameGameObject = QUI.QToggle("Show Rename Button", EditorSettings.UIEffect_Inspector_ShowButtonRenameGameObject, 20, Style.Text.Small);

                            QUI.Space(SPACE_4 * UIEffectAnimBool.faded);

                            QLabel.text = "Rename Prefix";
                            QLabel.style = Style.Text.Small;
                            tempFloat = QLabel.x + 120 + 8;
                            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIEffect_Inspector_ShowButtonRenameGameObject ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                            QUI.Space(-tempFloat - 12);
                            QUI.BeginVertical(QLabel.x, 20);
                            {
                                QUI.Space((20 - 16) / 2);
                                QUI.Label(QLabel);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();
                            EditorSettings.UIEffect_Inspector_RenameGameObjectPrefix = EditorGUILayout.DelayedTextField(EditorSettings.UIEffect_Inspector_RenameGameObjectPrefix, GUILayout.Width(120));

                            QUI.Space(SPACE_4);
                            QUI.Space(SPACE_4 * UIEffectAnimBool.faded);

                            QLabel.text = "Rename Sufix";
                            QLabel.style = Style.Text.Small;
                            tempFloat = QLabel.x + 120 + 8;
                            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIEffect_Inspector_ShowButtonRenameGameObject ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16, 20);
                            QUI.Space(-tempFloat - 12);
                            QUI.BeginVertical(QLabel.x, 20);
                            {
                                QUI.Space((20 - 16) / 2);
                                QUI.Label(QLabel);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();
                            EditorSettings.UIEffect_Inspector_RenameGameObjectSuffix = EditorGUILayout.DelayedTextField(EditorSettings.UIEffect_Inspector_RenameGameObjectSuffix, GUILayout.Width(120));

                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_16 * UIEffectAnimBool.faded);

                        QUI.DrawIconBar("Default Values", QResources.iconInfo, QColors.Color.Blue, IconPosition.Right, tempWidth);

                        QUI.Space(SPACE_2 * UIEffectAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            EditorSettings.UIEffect_playOnAwake = QUI.QToggle("play on awake", EditorSettings.UIEffect_playOnAwake, 20);
                            QUI.Space(SPACE_4 * UIEffectAnimBool.faded);

                            EditorSettings.UIEffect_stopInstantly = QUI.QToggle("stop instantly on hide", EditorSettings.UIEffect_stopInstantly, 20);
                            QUI.Space(SPACE_4 * UIEffectAnimBool.faded);

                            QLabel.text = "start delay on show";
                            QLabel.style = Style.Text.Normal;
                            tempFloat = QLabel.x + 40 + 8;
                            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat + 16, 20);
                            QUI.Space(-tempFloat - 12);
                            QUI.BeginVertical(QLabel.x, 20);
                            {
                                QUI.Space((20 - 18) / 2);
                                QUI.Label(QLabel);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();
                            EditorSettings.UIEffect_startDelay = EditorGUILayout.DelayedFloatField(EditorSettings.UIEffect_startDelay, GUILayout.Width(40));

                            QUI.Space(SPACE_4);
                            QUI.FlexibleSpace();
                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_2 * UIEffectAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            QLabel.text = "use a custom layer name";
                            QLabel.style = Style.Text.Normal;
                            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIEffect_useCustomSortingLayerName ? QColors.Color.Blue : QColors.Color.Gray), QLabel.x + 36 + 120, 20);
                            QUI.Space(-QLabel.x - 30 - 120);

                            QUI.BeginVertical(12, 20);
                            {
                                QUI.Space((20 - 16) / 2);
                                EditorSettings.UIEffect_useCustomSortingLayerName = QUI.Toggle(EditorSettings.UIEffect_useCustomSortingLayerName);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();

                            QUI.BeginVertical(QLabel.x, 20);
                            {
                                QUI.Space((20 - 18) / 2);
                                QUI.Label(QLabel);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();

                            EditorSettings.UIEffect_customSortingLayerName = EditorGUILayout.DelayedTextField(EditorSettings.UIEffect_customSortingLayerName, GUILayout.Width(120));

                            QUI.FlexibleSpace();

                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_2 * UIEffectAnimBool.faded);

                        QUI.BeginHorizontal(tempWidth);
                        {
                            QLabel.text = "use a custom order in layer";
                            QLabel.style = Style.Text.Normal;
                            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, EditorSettings.UIEffect_useCustomOrderInLayer ? QColors.Color.Blue : QColors.Color.Gray), QLabel.x + 36 + 40, 20);
                            QUI.Space(-QLabel.x - 30 - 40);

                            QUI.BeginVertical(12, 20);
                            {
                                QUI.Space((20 - 16) / 2);
                                EditorSettings.UIEffect_useCustomOrderInLayer = QUI.Toggle(EditorSettings.UIEffect_useCustomOrderInLayer);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();

                            QUI.BeginVertical(QLabel.x, 20);
                            {
                                QUI.Space((20 - 18) / 2);
                                QUI.Label(QLabel);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();

                            EditorSettings.UIEffect_customOrderInLayer = EditorGUILayout.DelayedIntField(EditorSettings.UIEffect_customOrderInLayer, GUILayout.Width(40));

                            QUI.FlexibleSpace();

                        }
                        QUI.EndHorizontal();

                        QUI.Space(SPACE_2 * UIEffectAnimBool.faded);


                        QUI.BeginHorizontal(tempWidth);
                        {
                            QLabel.text = "Set the effect";
                            QLabel.style = Style.Text.Normal;
                            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), QLabel.x + 36 + 216, 20);
                            QUI.Space(-QLabel.x - 32 - 216);

                            QUI.BeginVertical(QLabel.x, 20);
                            {
                                QUI.Space((20 - 18) / 2);
                                QUI.Label(QLabel);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();

                            EditorSettings.UIEffect_effectPosition = (UIEffect.EffectPosition)EditorGUILayout.EnumPopup(EditorSettings.UIEffect_effectPosition, GUILayout.Width(120));

                            QLabel.text = "by";
                            QLabel.style = Style.Text.Normal;
                            QUI.BeginVertical(QLabel.x, 20);
                            {
                                QUI.Space((20 - 18) / 2);
                                QUI.Label(QLabel);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();

                            EditorSettings.UIEffect_sortingOrderStep = EditorGUILayout.DelayedIntField(EditorSettings.UIEffect_sortingOrderStep, GUILayout.Width(40));

                            QLabel.text = "steps";
                            QLabel.style = Style.Text.Normal;
                            QUI.BeginVertical(QLabel.x, 20);
                            {
                                QUI.Space((20 - 18) / 2);
                                QUI.Label(QLabel);
                                QUI.Space((20 - 16) / 2);
                            }
                            QUI.EndVertical();

                            QUI.FlexibleSpace();

                        }
                        QUI.EndHorizontal();
                        QUI.Space(SPACE_16);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();
        }
    }
}
