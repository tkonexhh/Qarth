// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using DoozyUI.Internal;
using QuickEditor;
using QuickEngine.Extensions;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UI;

namespace DoozyUI
{
    [CustomEditor(typeof(UIElement), true)]
    [DisallowMultipleComponent]
    [CanEditMultipleObjects]
    public class UIElementEditor : QEditor
    {
        UIElement uiElement { get { return (UIElement)target; } }
        DUIData.Database DatabaseUIElements { get { return DUIData.Instance.DatabaseUIElements; } }
        DUIData.AnimDatabase DatabaseInAnimations { get { return DUIData.Instance.DatabaseInAnimations; } }
        DUIData.AnimDatabase DatabaseOutAnimations { get { return DUIData.Instance.DatabaseOutAnimations; } }
        DUIData.LoopDatabase DatabaseLoopAnimations { get { return DUIData.Instance.DatabaseLoopAnimations; } }
        DUIData.SoundsDatabase DatabaseUISounds { get { return DUIData.Instance.DatabaseUISounds; } }

        DUISettings EditorSettings { get { return DUISettings.Instance; } }

        GUIStyle buttonLinkedToUINotificationStyle;
        GUIStyle ButtonLinkedToUINotificationStyle
        {
            get
            {
                if(buttonLinkedToUINotificationStyle == null)
                {
                    buttonLinkedToUINotificationStyle = new GUIStyle()
                    {
                        normal = { background = DUIResources.buttonLinkedToUINotification.normal2D },
                        onNormal = { background = DUIResources.buttonLinkedToUINotification.normal2D },
                        hover = { background = DUIResources.buttonLinkedToUINotification.hover2D },
                        onHover = { background = DUIResources.buttonLinkedToUINotification.hover2D },
                        active = { background = DUIResources.buttonLinkedToUINotification.active2D },
                        onActive = { background = DUIResources.buttonLinkedToUINotification.active2D },
                        border = new RectOffset(310, 2, 2, 2)
                    };
                }
                return buttonLinkedToUINotificationStyle;
            }
        }

#pragma warning disable 0414

        SerializedProperty
            linkedToNotification, autoRegister,
            elementCategory,
            elementName,
            LANDSCAPE, PORTRAIT,
            startHidden, animateAtStart, disableWhenHidden,
            dontDisableCanvasWhenHidden, disableGraphicRaycaster,
            autoHide, autoHideDelay,
            executeLayoutFix,
            useCustomStartAnchoredPosition, customStartAnchoredPosition,
            deselectAnySelectedButtonOnShow, deselectAnySelectedButtonOnHide,
            enableAutoSelectButtonAfterShow, selectedButton,

            inAnimations,
            OnInAnimationsStart, OnInAnimationsFinish,
            inAnimationsSoundAtStart, customInAnimationsSoundAtStart,
            inAnimationsSoundAtFinish, customInAnimationsSoundAtFinish,
            inAnimationsPresetCategoryName, inAnimationsPresetName, loadInAnimationsPresetAtRuntime,

            outAnimations,
            OnOutAnimationsStart, OnOutAnimationsFinish,
            outAnimationsSoundAtStart, customOutAnimationsSoundAtStart,
            outAnimationsSoundAtFinish, customOutAnimationsSoundAtFinish,
            outAnimationsPresetCategoryName, outAnimationsPresetName, loadOutAnimationsPresetAtRuntime,

            loopAnimations,
            loopAnimationsAutoStart,
            loopAnimationsPresetCategoryName, loopAnimationsPresetName, loadLoopAnimationsPresetAtRuntime;

        AnimBool
            showPlayModeSettings,
            showAutoHideDelay,
            showCustomStartPosition,
            showInAnimations, showInAnimationsEvents, showInAnimationsPreset,
            showOutAnimations, showOutAnimationsEvents, showOutAnimationsPreset,
            showLoopAnimations, showLoopAnimationsPreset;

#pragma warning restore 0414

        bool localShowHide = false;

        Index elementNameIndex = new Index();
        Index elementCategoryIndex = new Index();

        Index inAnimationsSoundAtStartIndex = new Index();
        Index inAnimationsSoundAtFinishIndex = new Index();
        Index outAnimationsSoundAtStartIndex = new Index();
        Index outAnimationsSoundAtFinishIndex = new Index();

        string newPresetCategoryName = "";
        string newPresetName = "";

        AnimBool createNewCategoryName;

        Index inAnimationsPresetCategoryNameIndex = new Index();
        Index inAnimationsPresetNameIndex = new Index();
        AnimBool inAnimationsNewPreset;

        Index outAnimationsPresetCategoryNameIndex = new Index();
        Index outAnimationsPresetNameIndex = new Index();
        AnimBool outAnimationsNewPreset;

        Index loopAnimationsPresetCategoryNameIndex = new Index();
        Index loopAnimationsPresetNameIndex = new Index();
        AnimBool loopAnimationsNewPreset;


        float GlobalWidth { get { return DUI.GLOBAL_EDITOR_WIDTH; } }
        int BarHeight { get { return DUI.BAR_HEIGHT; } }
        int MiniBarHeight { get { return DUI.MINI_BAR_HEIGHT; } }


        float tempFloat = 0;
        bool tempBool = false;

        enum AnimType { In, Out, Loop }

        protected override void SerializedObjectFindProperties()
        {
            base.SerializedObjectFindProperties();

            linkedToNotification = serializedObject.FindProperty("linkedToNotification");
            autoRegister = serializedObject.FindProperty("autoRegister");

            elementCategory = serializedObject.FindProperty("elementCategory");

            elementName = serializedObject.FindProperty("elementName");

            LANDSCAPE = serializedObject.FindProperty("LANDSCAPE");
            PORTRAIT = serializedObject.FindProperty("PORTRAIT");

            startHidden = serializedObject.FindProperty("startHidden");
            animateAtStart = serializedObject.FindProperty("animateAtStart");
            disableWhenHidden = serializedObject.FindProperty("disableWhenHidden");

            dontDisableCanvasWhenHidden = serializedObject.FindProperty("dontDisableCanvasWhenHidden");
            disableGraphicRaycaster = serializedObject.FindProperty("disableGraphicRaycaster");

            autoHide = serializedObject.FindProperty("autoHide");
            autoHideDelay = serializedObject.FindProperty("autoHideDelay");

            executeLayoutFix = serializedObject.FindProperty("executeLayoutFix");

            useCustomStartAnchoredPosition = serializedObject.FindProperty("useCustomStartAnchoredPosition");
            customStartAnchoredPosition = serializedObject.FindProperty("customStartAnchoredPosition");

            deselectAnySelectedButtonOnShow = serializedObject.FindProperty("deselectAnySelectedButtonOnShow");
            deselectAnySelectedButtonOnHide = serializedObject.FindProperty("deselectAnySelectedButtonOnHide");
            enableAutoSelectButtonAfterShow = serializedObject.FindProperty("enableAutoSelectButtonAfterShow");
            selectedButton = serializedObject.FindProperty("selectedButton");

            inAnimations = serializedObject.FindProperty("inAnimations");

            OnInAnimationsStart = serializedObject.FindProperty("OnInAnimationsStart");
            OnInAnimationsFinish = serializedObject.FindProperty("OnInAnimationsFinish");

            inAnimationsSoundAtStart = serializedObject.FindProperty("inAnimationsSoundAtStart");
            customInAnimationsSoundAtStart = serializedObject.FindProperty("customInAnimationsSoundAtStart");
            inAnimationsSoundAtFinish = serializedObject.FindProperty("inAnimationsSoundAtFinish");
            customInAnimationsSoundAtFinish = serializedObject.FindProperty("customInAnimationsSoundAtFinish");

            inAnimationsPresetCategoryName = serializedObject.FindProperty("inAnimationsPresetCategoryName");
            inAnimationsPresetName = serializedObject.FindProperty("inAnimationsPresetName");
            loadInAnimationsPresetAtRuntime = serializedObject.FindProperty("loadInAnimationsPresetAtRuntime");

            outAnimations = serializedObject.FindProperty("outAnimations");

            OnOutAnimationsStart = serializedObject.FindProperty("OnOutAnimationsStart");
            OnOutAnimationsFinish = serializedObject.FindProperty("OnOutAnimationsFinish");

            outAnimationsSoundAtStart = serializedObject.FindProperty("outAnimationsSoundAtStart");
            customOutAnimationsSoundAtStart = serializedObject.FindProperty("customOutAnimationsSoundAtStart");
            outAnimationsSoundAtFinish = serializedObject.FindProperty("outAnimationsSoundAtFinish");
            customOutAnimationsSoundAtFinish = serializedObject.FindProperty("customOutAnimationsSoundAtFinish");

            outAnimationsPresetCategoryName = serializedObject.FindProperty("outAnimationsPresetCategoryName");
            outAnimationsPresetName = serializedObject.FindProperty("outAnimationsPresetName");
            loadOutAnimationsPresetAtRuntime = serializedObject.FindProperty("loadOutAnimationsPresetAtRuntime");

            loopAnimations = serializedObject.FindProperty("loopAnimations");
            loopAnimationsAutoStart = loopAnimations.FindPropertyRelative("autoStart");
            loopAnimationsPresetCategoryName = serializedObject.FindProperty("loopAnimationsPresetCategoryName");
            loopAnimationsPresetName = serializedObject.FindProperty("loopAnimationsPresetName");
            loadLoopAnimationsPresetAtRuntime = serializedObject.FindProperty("loadLoopAnimationsPresetAtRuntime");
        }

        protected override void GenerateInfoMessages()
        {
            base.GenerateInfoMessages();

            infoMessage.Add("LocalShowHide",
                            new InfoMessage()
                            {
                                title = "Local Show/Hide",
                                message = "Only this UIElement will get shown/hidden when using the SHOW/HIDE buttons. Any other UIElements with the same element category and element name will not be animated.",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("GlobalShowHide",
                           new InfoMessage()
                           {
                               title = "Global Show/Hide",
                               message = "All the UIElements with the same element category and element name will get shown/hidden when using the SHOW/HIDE buttons.",
                               type = InfoMessageType.Info,
                               show = new AnimBool(false, Repaint)
                           });

            infoMessage.Add("InAnimationsDisabled",
                            new InfoMessage()
                            {
                                title = "Enable at least one In Animation for SHOW to work.",
                                message = "",
                                type = InfoMessageType.Warning,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("OutAnimationsDisabled",
                            new InfoMessage()
                            {
                                title = "Enable at least one Out Animation for HIDE to work.",
                                message = "",
                                type = InfoMessageType.Warning,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("InAnimationsLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "Runtime Preset: " + inAnimationsPresetCategoryName.stringValue + " / " + inAnimationsPresetName.stringValue,
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(loadInAnimationsPresetAtRuntime.boolValue, Repaint)
                            });

            infoMessage.Add("OutAnimationsLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "Runtime Preset: " + outAnimationsPresetCategoryName.stringValue + " / " + outAnimationsPresetName.stringValue,
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(loadOutAnimationsPresetAtRuntime.boolValue, Repaint)
                            });

            infoMessage.Add("LoopAnimationsLoadPresetAtRuntime",
                            new InfoMessage()
                            {
                                title = "Runtime Preset: " + loopAnimationsPresetCategoryName.stringValue + " / " + loopAnimationsPresetName.stringValue,
                                message = "",
                                type = InfoMessageType.Info,
                                show = new AnimBool(loadLoopAnimationsPresetAtRuntime.boolValue, Repaint)
                            });
        }

        protected override void InitAnimBools()
        {
            base.InitAnimBools();

            showPlayModeSettings = new AnimBool(false, Repaint);

            showAutoHideDelay = new AnimBool(autoHide.boolValue, Repaint);

            showCustomStartPosition = new AnimBool(useCustomStartAnchoredPosition.boolValue, Repaint);

            showInAnimations = new AnimBool(false, Repaint);
            showInAnimationsEvents = new AnimBool(false, Repaint);
            showInAnimationsPreset = new AnimBool(loadInAnimationsPresetAtRuntime.boolValue, Repaint);

            showOutAnimations = new AnimBool(false, Repaint);
            showOutAnimationsEvents = new AnimBool(false, Repaint);
            showOutAnimationsPreset = new AnimBool(loadOutAnimationsPresetAtRuntime.boolValue, Repaint);

            showLoopAnimations = new AnimBool(false, Repaint);
            showLoopAnimationsPreset = new AnimBool(loadLoopAnimationsPresetAtRuntime.boolValue, Repaint);

            createNewCategoryName = new AnimBool(false, Repaint);
            inAnimationsNewPreset = new AnimBool(false, Repaint);
            outAnimationsNewPreset = new AnimBool(false, Repaint);
            loopAnimationsNewPreset = new AnimBool(false, Repaint);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            requiresContantRepaint = true;

            AddMissingComponents();
            CheckIfLinkedToNotification();

            SyncData();
        }

        void AddMissingComponents()
        {
            if(uiElement.GetComponent<Canvas>() == null) { uiElement.gameObject.AddComponent<Canvas>(); }
            if(uiElement.GetComponent<CanvasGroup>() == null) { uiElement.gameObject.AddComponent<CanvasGroup>(); }
            if(uiElement.GetComponent<GraphicRaycaster>() == null) { uiElement.gameObject.AddComponent<GraphicRaycaster>(); }
        }

        void SyncData()
        {
            DUIData.Instance.ValidateUIElements(); //Validate the database
            ValidateElementCategoryAndElementName(); //validate the local variables

            DUIData.Instance.AutomatedScanForUISounds(); //Scan for any new UISounds
            DUIData.Instance.ValidateUISounds(); //validate UISounds Database

            DUIData.Instance.ValidateInAnimations(); //validate IN Animations
            DUIData.Instance.ValidateOutAnimations(); //validate OUT Animations
            DUIData.Instance.ValidateLoopAnimations(); //validate LOOP Animations
        }
        void ValidateElementCategoryAndElementName()
        {
            if(uiElement.linkedToNotification) //if linked to an UINotification -> return
            {
                return;
            }

            //CHECK FOR CUSTOM NAME
            if(uiElement.elementCategory.Equals(DUI.CUSTOM_NAME)) //category is set to CUSTOM NAME -> get the index for the category name and set the name index to -1
            {
                elementCategoryIndex.index = DatabaseUIElements.CategoryNameIndex(DUI.CUSTOM_NAME); //set the index
                elementNameIndex.index = -1; //set the index
                return; //stop here as there is a CUSTOM NAME set
            }

            //SANITY CHECK FOR EMPTY CATEGORY NAME
            if(uiElement.elementCategory.IsNullOrEmpty()) //category name is empty (sanity check) -> reset both category and name
            {
                uiElement.elementCategory = DUI.UNCATEGORIZED_CATEGORY_NAME; //reset the value
                elementCategoryIndex.index = DatabaseUIElements.CategoryNameIndex(uiElement.elementCategory); //set the index

                uiElement.elementName = DUI.DEFAULT_ELEMENT_NAME; //reset the value
                elementNameIndex.index = DatabaseUIElements.ItemNameIndex(uiElement.elementCategory, uiElement.elementName); //set the index
                return;
            }

            //CHECK THAT CATEGORY EXISTS IN THE DATABASE
            if(!DatabaseUIElements.ContainsCategoryName(uiElement.elementCategory)) //the set category does not exist in the database
            {
                if(QUI.DisplayDialog("Action Required",
                                     "The category '" + uiElement.elementCategory + "' was not found in the database." +
                                       "\n\n" +
                                       "Do you want to add it to the database?",
                                     "Yes",
                                     "No")) //ask the dev if he wants to add this category to the database
                {
                    DatabaseUIElements.AddCategory(uiElement.elementCategory, true); //add the category to the database and save
                    elementCategoryIndex.index = DatabaseUIElements.CategoryNameIndex(uiElement.elementCategory); //set the index
                }
                else
                {
                    QUI.DisplayDialog("Info",
                                      "Element category has been reset to the default '" + DUI.UNCATEGORIZED_CATEGORY_NAME + "' value.",
                                      "Ok"); //inform the dev that becuase he did not add the category to the database, it has been reset to its default value
                    uiElement.elementCategory = DUI.UNCATEGORIZED_CATEGORY_NAME; //reset the value
                    elementCategoryIndex.index = DatabaseUIElements.CategoryNameIndex(uiElement.elementCategory); //set the index
                }
            }

            //CHECK THAT THE NAME EXISTS IN THE CATEGORY
            if(!DatabaseUIElements.Contains(uiElement.elementCategory, uiElement.elementName)) //the set element name does not exist under the set category
            {
                if(QUI.DisplayDialog("Action Required",
                                     "The name '" + uiElement.elementName + "' was not found in the '" + uiElement.elementCategory + "' category." +
                                       "\n\n" +
                                       "Do you want to add it to the database?",
                                     "Yes",
                                     "No")) //ask the dev if he wants to add this name to the database
                {
                    DatabaseUIElements.GetCategory(uiElement.elementCategory).AddItemName(uiElement.elementName, true); //add the item name to the database and save
                    elementCategoryIndex.index = DatabaseUIElements.CategoryNameIndex(uiElement.elementCategory); //set the category index
                    elementNameIndex.index = DatabaseUIElements.ItemNameIndex(uiElement.elementCategory, uiElement.elementName); //set the name index
                }
                else
                {
                    QUI.DisplayDialog("Info",
                                      "Element name has been reset to the default '" + DUI.DEFAULT_ELEMENT_NAME + "' value.",
                                      "Ok"); //inform the dev that becuase he did not add the name to the database, it has been reset to its default value
                    uiElement.elementName = DUI.DEFAULT_ELEMENT_NAME; //reset the value
                    elementCategoryIndex.index = DatabaseUIElements.CategoryNameIndex(uiElement.elementCategory); //set the category index
                    elementNameIndex.index = DatabaseUIElements.ItemNameIndex(uiElement.elementCategory, uiElement.elementName); //set the name index
                }
            }
            else
            {
                elementCategoryIndex.index = DatabaseUIElements.CategoryNameIndex(uiElement.elementCategory); //set the category index
                elementNameIndex.index = DatabaseUIElements.ItemNameIndex(uiElement.elementCategory, uiElement.elementName); //set the name index
            }
        }

        public override void OnInspectorGUI()
        {
            DrawHeader(DUIResources.headerUIElement.normal, GlobalWidth, HEIGHT_42);

            if(IsEditorLocked) { return; }

            serializedObject.Update();

            DrawPlayModeSettings(GlobalWidth);

            if(linkedToNotification.boolValue)
            {
                if(!showPlayModeSettings.value)
                {
                    QUI.Space(-SPACE_4);
                }
                else
                {
                    QUI.Space(SPACE_2);
                }
                DrawLinkedToNotification(GlobalWidth);
            }
            else
            {
                DrawDatabaseButtons(GlobalWidth);
                QUI.Space(SPACE_2);
                DrawRenameGameObjectButton(GlobalWidth);
                QUI.Space(SPACE_8);
                DrawElementCategoryAndElementName(GlobalWidth);
            }

            QUI.Space(SPACE_8);

            DrawOrientation(GlobalWidth);
            DrawSettings(GlobalWidth);

            QUI.Space(SPACE_8);

            DrawInAnimations(GlobalWidth);

            //DrawSpecialAnimationsButtons(globalWidth);

            DrawOutAnimations(GlobalWidth);

            if(!EditorSettings.UIElement_Inspector_HideLoopAnimations)
            {
                DrawLoopAnimations(GlobalWidth);
            }

            serializedObject.ApplyModifiedProperties();

            QUI.Space(SPACE_4);
        }

        void DrawDatabaseButtons(float width)
        {
            QUI.BeginHorizontal(width);
            {
                if(QUI.GhostButton("UIElements Database", QColors.Color.Gray, (width - SPACE_2) / 2, 18))
                {
                    ControlPanelWindow.OpenWindow(ControlPanelWindow.Page.UIElements);
                }
                QUI.Space(SPACE_2);
                if(QUI.GhostButton("UISounds Database", QColors.Color.Gray, (width - SPACE_2) / 2, 18))
                {
                    ControlPanelWindow.OpenWindow(ControlPanelWindow.Page.UISounds);
                }
            }
            QUI.EndHorizontal();
        }
        void DrawRenameGameObjectButton(float width)
        {
            if(DUI.DUISettings.UIElement_Inspector_ShowButtonRenameGameObject && !linkedToNotification.boolValue)
            {
                QUI.BeginHorizontal(width);
                {
                    if(QUI.GhostButton("Rename GameObject to Element Name", QColors.Color.Gray, width, 18))
                    {
                        if(serializedObject.isEditingMultipleObjects)
                        {
                            Undo.RecordObjects(targets, "Renamed Multiple Objects");
                            for(int i = 0; i < targets.Length; i++)
                            {
                                UIElement iTarget = (UIElement)targets[i];
                                iTarget.gameObject.name = DUI.DUISettings.UIElement_Inspector_RenameGameObjectPrefix + iTarget.elementName + DUI.DUISettings.UIElement_Inspector_RenameGameObjectSuffix;
                            }
                        }
                        else
                        {
                            uiElement.gameObject.name = DUI.DUISettings.UIElement_Inspector_RenameGameObjectPrefix + elementName.stringValue + DUI.DUISettings.UIElement_Inspector_RenameGameObjectSuffix;
                        }
                    }
                }
                QUI.EndHorizontal();
            }
        }

        void DrawPlayModeSettings(float width)
        {
            showPlayModeSettings.target = EditorApplication.isPlayingOrWillChangePlaymode;
            if(QUI.BeginFadeGroup(showPlayModeSettings.faded))
            {
                QUI.BeginVertical(width);
                {
                    QUI.Space(SPACE_4 * showPlayModeSettings.faded);

                    QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, localShowHide ? QColors.Color.Blue : QColors.Color.Gray), width, 18);
                    QUI.Space(-18);

                    QUI.BeginHorizontal(width);
                    {
                        QUI.Space(SPACE_4);

                        localShowHide = QUI.Toggle(localShowHide);

                        QUI.Space(SPACE_2);

                        QLabel.text = "Local Show/Hide";
                        QLabel.style = Style.Text.Normal;
                        QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                        {
                            QUI.Label(QLabel);
                            QUI.Space(SPACE_2);
                        }
                        QUI.EndVertical();

                        QUI.FlexibleSpace();
                    }
                    QUI.EndHorizontal();

                    infoMessage["LocalShowHide"].show.target = localShowHide;
                    DrawInfoMessage("LocalShowHide", width);
                    infoMessage["GlobalShowHide"].show.target = !localShowHide;
                    DrawInfoMessage("GlobalShowHide", width);

                    QUI.Space(SPACE_4 * showPlayModeSettings.faded);

                    QUI.BeginHorizontal(width);
                    {
                        if(QUI.GhostButton("SHOW", QColors.Color.Blue, (width - SPACE_2) / 2, BarHeight))
                        {
                            if(localShowHide)
                            {
                                uiElement.Show(false);
                            }
                            else
                            {
                                UIManager.ShowUiElement(elementName.stringValue, elementCategory.stringValue, false);
                            }
                        }

                        if(QUI.GhostButton("HIDE", QColors.Color.Blue, (width - SPACE_2) / 2, BarHeight))
                        {
                            if(localShowHide)
                            {
                                uiElement.Hide(false);
                            }
                            else
                            {
                                UIManager.HideUiElement(elementName.stringValue, elementCategory.stringValue, false);
                            }
                        }

                    }
                    QUI.EndHorizontal();

                    QUI.Space(SPACE_16 * showPlayModeSettings.faded);
                }
                QUI.EndVertical();
            }
            QUI.EndFadeGroup();
        }

        void DrawLinkedToNotification(float width)
        {
            if(EditorApplication.isPlayingOrWillChangePlaymode) { return; }
            QUI.Space(SPACE_2);
            if(QUI.Button(ButtonLinkedToUINotificationStyle, width, 32))
            {
                UnlinkFromNotification();
            }
        }

        void DrawElementCategoryAndElementName(float width)
        {
            QUI.BeginHorizontal(width);
            {
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), (width - SPACE_2) / 2, 34);
                QUI.Space(SPACE_2);
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), (width - SPACE_2) / 2, 34);
            }
            QUI.EndHorizontal();

            QUI.Space(-36);

            QUI.BeginHorizontal(width);
            {
                QUI.Space(6);

                QLabel.text = "Element Category";
                QLabel.style = Style.Text.Small;
                QUI.BeginHorizontal((width - 6) / 2);
                {
                    QUI.Label(QLabel);
                    QUI.FlexibleSpace();
                }
                QUI.EndHorizontal();

                QLabel.text = "Element Name";
                QLabel.style = Style.Text.Small;
                QUI.BeginHorizontal((width - 6) / 2);
                {
                    QUI.Label(QLabel);
                    QUI.FlexibleSpace();
                }
                QUI.EndHorizontal();
            }
            QUI.EndHorizontal();

            QUI.Space(-SPACE_4);

            QUI.BeginHorizontal(width);
            {
                QUI.Space(4);
                DrawElementCategory((width - 6) / 2);
                DrawElementName((width - 6) / 2);
            }
            QUI.EndHorizontal();
        }
        void DrawElementCategory(float width)
        {
            QUI.BeginHorizontal(width);
            {
                if(EditorApplication.isPlayingOrWillChangePlaymode)
                {
                    QLabel.text = elementCategory.stringValue;
                    QLabel.style = Style.Text.Help;
                    QUI.Label(QLabel);
                }
                else
                {
                    //elementCategoryIndex.index = DatabaseUIElements.CategoryNameIndex(elementCategory.stringValue); //set the index
                    QUI.BeginChangeCheck();
                    elementCategoryIndex.index = EditorGUILayout.Popup(elementCategoryIndex.index, DatabaseUIElements.categoryNames.ToArray(), GUILayout.Width(width - 5));
                    if(QUI.EndChangeCheck())
                    {
                        elementCategory.stringValue = DatabaseUIElements.categoryNames[elementCategoryIndex.index];
                        if(elementCategory.stringValue == DUI.CUSTOM_NAME)
                        {
                            elementNameIndex.index = -1;
                        }
                        serializedObject.ApplyModifiedProperties();
                    }
                }

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawElementName(float width)
        {
            QUI.BeginHorizontal(width);
            {
                if(EditorApplication.isPlayingOrWillChangePlaymode)
                {
                    QLabel.text = elementName.stringValue;
                    QLabel.style = Style.Text.Help;
                    QUI.Label(QLabel);
                }
                else
                {
                    if(elementCategory.stringValue.Equals(DUI.CUSTOM_NAME))
                    {
                        QUI.PropertyField(elementName, width - 5);
                    }
                    else
                    {
                        if(!DatabaseUIElements.ContainsCategoryName(elementCategory.stringValue)) //the category does not exist -> reset category and name
                        {
                            LockEditor();
                            QUI.DisplayDialog("Info",
                                                 "Element category has been reset to the default '" + DUI.UNCATEGORIZED_CATEGORY_NAME + "' value." +
                                                   "\n\n" +
                                                   "Element name has been reset to the default '" + DUI.DEFAULT_ELEMENT_NAME + "' value.",
                                                 "Ok"); //inform the dev that becuase he did not add the name to the database, it has been reset to its default value
                            elementCategory.stringValue = DUI.UNCATEGORIZED_CATEGORY_NAME; //reset the category
                            elementCategoryIndex.index = DatabaseUIElements.CategoryNameIndex(elementCategory.stringValue); //set the index
                            elementName.stringValue = DUI.DEFAULT_ELEMENT_NAME; //reset the name
                            elementNameIndex.index = DatabaseUIElements.ItemNameIndex(elementCategory.stringValue, elementName.stringValue); //set the index
                            UnlockEditor();
                        }
                        else if(!DatabaseUIElements.Contains(elementCategory.stringValue, elementName.stringValue)) //category does not contain the set name -> ask de dev is it should be added
                        {
                            LockEditor();
                            if(QUI.DisplayDialog("Action Required",
                                                    "The name '" + elementName.stringValue + "' was not found in the '" + elementCategory.stringValue + "' category." +
                                                      "\n\n" +
                                                      "Do you want to add it to the database?",
                                                    "Yes",
                                                    "No")) //ask the dev if he wants to add this name to the database
                            {
                                DatabaseUIElements.GetCategory(elementCategory.stringValue).AddItemName(elementName.stringValue, true); //add the item name to the database and save
                                elementNameIndex.index = DatabaseUIElements.ItemNameIndex(elementCategory.stringValue, elementName.stringValue); //set the index
                                UnlockEditor();
                            }
                            else if(!DatabaseUIElements.GetCategory(elementCategory.stringValue).IsEmpty()) //select the first item in the category because it's not empty
                            {
                                elementNameIndex.index = 0; //set the index
                                elementName.stringValue = DatabaseUIElements.GetCategory(elementCategory.stringValue).itemNames[elementNameIndex.index]; //get the name
                                UnlockEditor();
                            }
                            else //reset category and name
                            {
                                LockEditor();
                                QUI.DisplayDialog("Info",
                                                  "Element category has been reset to the default '" + DUI.UNCATEGORIZED_CATEGORY_NAME + "' value." +
                                                    "\n\n" +
                                                    "Element name has been reset to the default '" + DUI.DEFAULT_ELEMENT_NAME + "' value.",
                                                  "Ok"); //inform the dev that becuase he did not add the name to the database, it has been reset to its default value
                                elementCategory.stringValue = DUI.UNCATEGORIZED_CATEGORY_NAME; //reset the category
                                elementCategoryIndex.index = DatabaseUIElements.CategoryNameIndex(elementCategory.stringValue); //set the index
                                elementName.stringValue = DUI.DEFAULT_ELEMENT_NAME; //reset the name
                                elementNameIndex.index = DatabaseUIElements.ItemNameIndex(elementCategory.stringValue, elementName.stringValue); //set the index
                                UnlockEditor();
                            }
                        }
                        else //category contains the set name -> get its index
                        {
                            elementNameIndex.index = DatabaseUIElements.ItemNameIndex(elementCategory.stringValue, elementName.stringValue); //set the index
                            serializedObject.ApplyModifiedProperties();
                        }
                        QUI.BeginChangeCheck();
                        elementNameIndex.index = EditorGUILayout.Popup(elementNameIndex.index, DatabaseUIElements.GetCategory(elementCategory.stringValue).itemNames.ToArray(), GUILayout.Width(width - 5));
                        if(QUI.EndChangeCheck())
                        {
                            elementName.stringValue = DatabaseUIElements.GetCategory(elementCategory.stringValue).itemNames[elementNameIndex.index];
                            serializedObject.ApplyModifiedProperties();
                        }
                    }
                }

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
            QUI.Space(SPACE_4);
        }

        void DrawOrientation(float width)
        {
            if(!UIManager.useOrientationManager) { return; }

            QUI.BeginHorizontal(width);
            {
                QUI.LabelWithBackground("Orientation", Style.Text.Small, 18);
                QUI.Space(SPACE_8);
                QUI.QToggle("LANDSCAPE", LANDSCAPE);
                QUI.Space(SPACE_8);
                QUI.QToggle("PORTRAIT", PORTRAIT);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
            QUI.Space(SPACE_4);
        }

        void DrawSettings(float width)
        {
            QUI.BeginHorizontal(width);
            {
                if(animateAtStart.boolValue) { GUI.enabled = false; }
                QUI.QToggle("hide @START", startHidden);
                GUI.enabled = true;

                QUI.Space(SPACE_8);

                if(startHidden.boolValue) { GUI.enabled = false; }
                QUI.QToggle("animate @START", animateAtStart);
                GUI.enabled = true;

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_4);

            QUI.BeginHorizontal(width);
            {
                QUI.QToggle("disable when hidden", disableWhenHidden);

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_4);

            QUI.BeginHorizontal(width);
            {
                QUI.QToggle("don't disable Canvas when hidden", dontDisableCanvasWhenHidden);

                QUI.Space(SPACE_4);

                QUI.QToggle("disable the Graphic Raycaster", disableGraphicRaycaster);
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_4);

            #region AUTO HIDE
            QUI.BeginHorizontal(width);
            {
                //AUTO HIDE
                QLabel.text = "auto hide";
                QLabel.style = Style.Text.Normal;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, autoHide.boolValue ? QColors.Color.Blue : QColors.Color.Gray), QLabel.x + 16 + 12 + 174 * showAutoHideDelay.faded, 19);
                QUI.Space(-QLabel.x - 12 - 10 - 174 * showAutoHideDelay.faded);

                QUI.Toggle(autoHide);
                QUI.BeginVertical(QLabel.x + 8, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(SPACE_2);
                }
                QUI.EndVertical();

                showAutoHideDelay.target = autoHide.boolValue;
                if(showAutoHideDelay.faded > 0.2f)
                {
                    QUI.Space(-5);

                    QLabel.text = "after a";
                    QLabel.style = Style.Text.Normal;
                    QUI.BeginVertical(QLabel.x * showAutoHideDelay.faded, QUI.SingleLineHeight);
                    {
                        QUI.Label(QLabel.text, Style.Text.Normal, QLabel.x * showAutoHideDelay.faded);
                        QUI.Space(SPACE_2);
                    }
                    QUI.EndVertical();
                }

                if(showAutoHideDelay.faded > 0.4f)
                {
                    QUI.PropertyField(autoHideDelay, 40 * showAutoHideDelay.faded);
                }

                if(showAutoHideDelay.faded > 0.6f)
                {
                    QLabel.text = "seconds delay";
                    QLabel.style = Style.Text.Normal;
                    QUI.BeginVertical(QLabel.x * showAutoHideDelay.faded, QUI.SingleLineHeight);
                    {
                        QUI.Label(QLabel.text, Style.Text.Normal, QLabel.x * showAutoHideDelay.faded);
                        QUI.Space(SPACE_2);
                    }
                    QUI.EndVertical();
                }
            }
            QUI.EndHorizontal();
            #endregion

            QUI.Space(SPACE_4);

            #region CUSTOM START POSITION
            QUI.BeginHorizontal(width);
            {
                //CUSTOM START POSITION
                QLabel.text = "custom start position";
                QLabel.style = Style.Text.Normal;
                tempFloat = width - QLabel.x - 16 - 12; //extra space after the custom start position label
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, useCustomStartAnchoredPosition.boolValue ? QColors.Color.Blue : QColors.Color.Gray), QLabel.x + 16 + 12 + tempFloat * showCustomStartPosition.faded, 18 + 24 * showCustomStartPosition.faded);
                QUI.Space(-QLabel.x - 12 - 12 - tempFloat * showCustomStartPosition.faded);

                QUI.Toggle(useCustomStartAnchoredPosition);
                QUI.BeginVertical(QLabel.x + 8, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(SPACE_2);
                }
                QUI.EndVertical();

                if(showCustomStartPosition.faded > 0.4f)
                {
                    QUI.PropertyField(customStartAnchoredPosition, (tempFloat - 4) * showCustomStartPosition.faded);
                }
            }
            QUI.EndHorizontal();

            showCustomStartPosition.target = useCustomStartAnchoredPosition.boolValue;

            QUI.Space(-20 * showCustomStartPosition.faded); //lift the buttons on the background

            if(showCustomStartPosition.faded > 0.4f)
            {
                tempFloat = (width - 16 - 16) / 3; //button width (3 buttons) that takes into account spaces
                QUI.BeginHorizontal(width);
                {
                    QUI.Space(20 * showCustomStartPosition.faded);
                    if(QUI.GhostButton("Get Position", QColors.Color.Blue, tempFloat * showCustomStartPosition.faded, 16 * showCustomStartPosition.faded))
                    {
                        customStartAnchoredPosition.vector3Value = uiElement.RectTransform.anchoredPosition3D;
                    }

                    QUI.Space(SPACE_4);

                    if(QUI.GhostButton("Set Position", QColors.Color.Blue, tempFloat * showCustomStartPosition.faded, 16 * showCustomStartPosition.faded))
                    {
                        Undo.RecordObject(uiElement.RectTransform, "SetPosition");
                        uiElement.RectTransform.anchoredPosition3D = customStartAnchoredPosition.vector3Value;
                    }

                    QUI.Space(SPACE_4);

                    if(QUI.GhostButton("Reset Position", QColors.Color.Blue, tempFloat * showCustomStartPosition.faded, 16 * showCustomStartPosition.faded))
                    {
                        customStartAnchoredPosition.vector3Value = Vector3.zero;
                    }

                    QUI.FlexibleSpace();
                }
                QUI.EndHorizontal();
            }

            QUI.Space(SPACE_8 * showCustomStartPosition.faded);
            #endregion

            QUI.Space(SPACE_4);

            #region EXECUTE LAYOUT FIX
            QUI.BeginHorizontal(width);
            {
                //EXECUTE LAYOUT FIX
                QLabel.text = "execute layout fix"; //Label 1 text
                QLabel.style = Style.Text.Normal;
                tempFloat = QLabel.x; //calculate the background width by setting it to Label 1 text width

                QLabel.text = "(useful in some cases)"; //Label 2 text
                QLabel.style = Style.Text.Help;
                tempFloat += QLabel.x; //add the second label width to the background width to cover both of them

                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, executeLayoutFix.boolValue ? QColors.Color.Blue : QColors.Color.Gray), tempFloat + 16 + 16, 18); //draw background by adding the Toggle width and the extra spaces
                QUI.Space(-tempFloat - 12 - 16);

                QLabel.text = "execute layout fix";
                QLabel.style = Style.Text.Normal;
                QUI.Toggle(executeLayoutFix); //Toggle
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
            #endregion

            QUI.Space(SPACE_4);

            #region AUTO SELECTED BUTTON AFTER SHOW

            QUI.BeginHorizontal(width);
            {
                if(!enableAutoSelectButtonAfterShow.boolValue && selectedButton.objectReferenceValue != null)
                {
                    selectedButton.objectReferenceValue = null;
                }

                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, enableAutoSelectButtonAfterShow.boolValue && selectedButton.objectReferenceValue != null ? QColors.Color.Blue : QColors.Color.Gray), width, 20);
                QUI.Space(-width + SPACE_4);

                QUI.Toggle(enableAutoSelectButtonAfterShow);

                GUI.enabled = enableAutoSelectButtonAfterShow.boolValue;


                QUI.Space(SPACE_2);

                QLabel.text = "auto selected button after show";
                QLabel.style = Style.Text.Normal;
                tempFloat = width - QLabel.x; //property field width


                QUI.Label(QLabel);

                QUI.PropertyField(selectedButton, tempFloat - 30);

                GUI.enabled = true;

                QUI.FlexibleSpace();

            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2);

            QUI.BeginHorizontal(width);
            {
                QLabel.text = "if a button is selected, deselect it on";
                QLabel.style = Style.Text.Normal;
                tempFloat = QLabel.x + 14; //property field width

                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, deselectAnySelectedButtonOnShow.boolValue || deselectAnySelectedButtonOnHide.boolValue ? QColors.Color.Blue : QColors.Color.Gray), tempFloat, 20);
                QUI.Space(-tempFloat + 4);

                QUI.Label(QLabel);

                QUI.Space(SPACE_4);

                QUI.QToggle("Show", deselectAnySelectedButtonOnShow, 20);

                QUI.Space(SPACE_2);

                QUI.QToggle("Hide", deselectAnySelectedButtonOnHide, 20);

                QUI.FlexibleSpace();

            }
            QUI.EndHorizontal();

            #endregion
        }

        void DrawInAnimations(float width)
        {
            QUI.BeginHorizontal(width);
            {
                DrawMainBar("In Animations", uiElement.InAnimationsEnabled, loadInAnimationsPresetAtRuntime, showInAnimations, inAnimationsNewPreset, width, BarHeight);
                DrawMainGhostButtons(AnimType.In, BarHeight, BarHeight);
            }
            QUI.EndHorizontal();

            DrawDisabledInfoMessages("InAnimationsDisabled", uiElement.InAnimationsEnabled, loadInAnimationsPresetAtRuntime, showInAnimations, width);
            DrawLoadPresetInfoMessage("InAnimationsLoadPresetAtRuntime", loadInAnimationsPresetAtRuntime, inAnimationsPresetCategoryName.stringValue, inAnimationsPresetName.stringValue, showInAnimations, width);

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * showInAnimations.faded);
                if(QUI.BeginFadeGroup(showInAnimations.faded))
                {
                    QUI.BeginVertical(width - SPACE_8);
                    {
                        QUI.Space(SPACE_2 * showInAnimations.faded);
                        DrawInAnimationsPreset(width - SPACE_8);
                        QUI.Space(SPACE_2 * showInAnimations.faded);
                        DUIUtils.DrawAnim(uiElement.inAnimations, uiElement, width - SPACE_8); //draw in animations - generic method
                        QUI.Space(SPACE_8 * showInAnimations.faded);
                        DrawInAnimationsSounds(width - SPACE_8);
                        QUI.Space(SPACE_8 * showInAnimations.faded);
                        DrawInAnimationsEvents(width - SPACE_8);
                        QUI.Space(SPACE_16 * showInAnimations.faded);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();
            QUI.Space(SPACE_8 * (1 - showInAnimations.faded));
        }
        void DrawInAnimationsPreset(float width)
        {
            QUI.Space(SPACE_2);
            DUIUtils.DrawLoadPresetAtRuntime(loadInAnimationsPresetAtRuntime, width);
            DUIUtils.DrawPresetBackground(inAnimationsNewPreset, createNewCategoryName, width);

            if(inAnimationsNewPreset.faded < 0.5f) //NORMAL VIEW
            {
                DrawPresetNormalView(AnimType.In, width);
                QUI.Space(SPACE_2);
                DrawPresetNormalViewPresetButtons(AnimType.In, inAnimationsNewPreset, width);

            }
            else //NEW PRESET VIEW
            {
                DrawPresetNewPresetView(AnimType.In, inAnimationsNewPreset, width);
                QUI.Space(SPACE_2);
                DrawPresetNewPresetViewPresetButtons(AnimType.In, inAnimationsNewPreset, inAnimationsPresetCategoryName, inAnimationsPresetName, width);
            }
        }
        void DrawInAnimationsSounds(float width)
        {
            Drawsounds(customInAnimationsSoundAtStart, inAnimationsSoundAtStart, inAnimationsSoundAtStartIndex,
                       customInAnimationsSoundAtFinish, inAnimationsSoundAtFinish, inAnimationsSoundAtFinishIndex,
                       width);
        }
        void DrawInAnimationsEvents(float width)
        {
            DrawUnityEvents(uiElement.InAnimationsEnabled && (uiElement.OnInAnimationsStart.GetPersistentEventCount() > 0 || uiElement.OnInAnimationsFinish.GetPersistentEventCount() > 0),
                            showInAnimationsEvents,
                            OnInAnimationsStart,
                            "OnInAnimationsStart",
                            OnInAnimationsFinish,
                            "OnInAnimationsFinish",
                            width);
        }

        void DrawOutAnimations(float width)
        {
            QUI.BeginHorizontal(width);
            {
                DrawMainBar("Out Animations", uiElement.OutAnimationsEnabled, loadOutAnimationsPresetAtRuntime, showOutAnimations, outAnimationsNewPreset, width, BarHeight);
                DrawMainGhostButtons(AnimType.Out, BarHeight, BarHeight);
            }
            QUI.EndHorizontal();

            DrawDisabledInfoMessages("OutAnimationsDisabled", uiElement.OutAnimationsEnabled, loadOutAnimationsPresetAtRuntime, showOutAnimations, width);
            DrawLoadPresetInfoMessage("OutAnimationsLoadPresetAtRuntime", loadOutAnimationsPresetAtRuntime, outAnimationsPresetCategoryName.stringValue, outAnimationsPresetName.stringValue, showOutAnimations, width);

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * showOutAnimations.faded);
                if(QUI.BeginFadeGroup(showOutAnimations.faded))
                {
                    QUI.BeginVertical(width - SPACE_8);
                    {
                        QUI.Space(SPACE_2 * showOutAnimations.faded);
                        DrawOutAnimationsPreset(width - SPACE_8);
                        QUI.Space(SPACE_2 * showOutAnimations.faded);
                        DUIUtils.DrawAnim(uiElement.outAnimations, uiElement, width - SPACE_8); //draw out animations - generic method
                        QUI.Space(SPACE_8 * showOutAnimations.faded);
                        DrawOutAnimationsSounds(width - SPACE_8);
                        QUI.Space(SPACE_8 * showOutAnimations.faded);
                        DrawOutAnimationsEvents(width - SPACE_8);
                        QUI.Space(SPACE_16 * showOutAnimations.faded);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();
            QUI.Space(SPACE_8 * (1 - showOutAnimations.faded));
        }
        void DrawOutAnimationsPreset(float width)
        {
            QUI.Space(SPACE_2);
            DUIUtils.DrawLoadPresetAtRuntime(loadOutAnimationsPresetAtRuntime, width);
            DUIUtils.DrawPresetBackground(outAnimationsNewPreset, createNewCategoryName, width);

            if(outAnimationsNewPreset.faded < 0.5f) //NORMAL VIEW
            {
                DrawPresetNormalView(AnimType.Out, width);
                QUI.Space(SPACE_2);
                DrawPresetNormalViewPresetButtons(AnimType.Out, outAnimationsNewPreset, width);

            }
            else //NEW PRESET VIEW
            {
                DrawPresetNewPresetView(AnimType.Out, outAnimationsNewPreset, width);
                QUI.Space(SPACE_2);
                DrawPresetNewPresetViewPresetButtons(AnimType.Out, outAnimationsNewPreset, outAnimationsPresetCategoryName, outAnimationsPresetName, width);
            }
        }
        void DrawOutAnimationsSounds(float width)
        {
            Drawsounds(customOutAnimationsSoundAtStart, outAnimationsSoundAtStart, outAnimationsSoundAtStartIndex,
                       customOutAnimationsSoundAtFinish, outAnimationsSoundAtFinish, outAnimationsSoundAtFinishIndex,
                       width);
        }
        void DrawOutAnimationsEvents(float width)
        {
            DrawUnityEvents(uiElement.OutAnimationsEnabled && (uiElement.OnOutAnimationsStart.GetPersistentEventCount() > 0 || uiElement.OnOutAnimationsFinish.GetPersistentEventCount() > 0),
                            showOutAnimationsEvents,
                            OnOutAnimationsStart,
                            "OnOutAnimationsStart",
                            OnOutAnimationsFinish,
                            "OnOutAnimationsFinish",
                            width);
        }

        void DrawLoopAnimations(float width)
        {
            QUI.BeginHorizontal(width);
            {
                DrawMainBar("Loop Animations", uiElement.LoopAnimationsEnabled, loadLoopAnimationsPresetAtRuntime, showLoopAnimations, loopAnimationsNewPreset, width, BarHeight);
                DrawMainGhostButtons(AnimType.Loop, BarHeight, BarHeight);
            }
            QUI.EndHorizontal();

            DrawLoadPresetInfoMessage("LoopAnimationsLoadPresetAtRuntime", loadLoopAnimationsPresetAtRuntime, loopAnimationsPresetCategoryName.stringValue, loopAnimationsPresetName.stringValue, showLoopAnimations, width);

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * showLoopAnimations.faded);
                if(QUI.BeginFadeGroup(showLoopAnimations.faded))
                {
                    QUI.BeginVertical(width - SPACE_8);
                    {
                        QUI.Space(SPACE_2 * showLoopAnimations.faded);
                        DrawLoopAnimationsPreset(width - SPACE_8);
                        QUI.Space(SPACE_2 * showLoopAnimations.faded);
                        DUIUtils.DrawLoop(uiElement.loopAnimations, uiElement, width - SPACE_8); //draw loop animations - generic method
                        QUI.Space(SPACE_16 * showLoopAnimations.faded);
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();
            QUI.Space(SPACE_8 * (1 - showLoopAnimations.faded));
        }
        void DrawLoopAnimationsPreset(float width)
        {
            QUI.Space(SPACE_2);
            DUIUtils.DrawLoadPresetAtRuntime(loadLoopAnimationsPresetAtRuntime, width);
            DrawLoopAnimationsAutoStart(width);
            DUIUtils.DrawPresetBackground(loopAnimationsNewPreset, createNewCategoryName, width);

            if(loopAnimationsNewPreset.faded < 0.5f) //NORMAL VIEW
            {
                DrawPresetNormalView(AnimType.Loop, width);
                QUI.Space(SPACE_2);
                DrawPresetNormalViewPresetButtons(AnimType.Loop, loopAnimationsNewPreset, width);

            }
            else //NEW PRESET VIEW
            {
                DrawPresetNewPresetView(AnimType.Loop, loopAnimationsNewPreset, width);
                QUI.Space(SPACE_2);
                DrawPresetNewPresetViewPresetButtons(AnimType.Loop, loopAnimationsNewPreset, loopAnimationsPresetCategoryName, loopAnimationsPresetName, width);
            }
        }
        void DrawLoopAnimationsAutoStart(float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, loopAnimationsAutoStart.boolValue ? QColors.Color.Blue : QColors.Color.Gray), width, 18); //if a preset is set to load at runtime -> set a background color (visual aid for the dev)

            QUI.Space(-18);

            if(loopAnimationsAutoStart.boolValue)
            {
                QUI.SetGUIBackgroundColor(QUI.AccentColorBlue);
            }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_4);
                QUI.Toggle(loopAnimationsAutoStart);
                QLabel.text = "Auto Start";
                QLabel.style = Style.Text.Normal;
                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(SPACE_2);
                }
                QUI.EndVertical();
                QUI.Space(-6);
                QLabel.text = "(loop animation starts automatically)";
                QLabel.style = Style.Text.Help;
                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(1);
                }
                QUI.EndVertical();
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
            QUI.ResetColors();
        }

        void DrawDisabledInfoMessages(string disabledInfoMessageTag, bool enabled, SerializedProperty loadPresetAtRuntime, AnimBool show, float width)
        {
            infoMessage[disabledInfoMessageTag].show.target = !enabled && !loadPresetAtRuntime.boolValue; //check if the animations are disabled
            DrawInfoMessage(disabledInfoMessageTag, width); //draw warning if the animations are disabled
            QUI.Space(SPACE_4 * (1 - show.faded) * infoMessage[disabledInfoMessageTag].show.faded); //space added if the animations are disabled
        }
        void DrawLoadPresetInfoMessage(string loadPresetInfoMessageTag, SerializedProperty loadPresetAtRuntime, string categoryName, string presetName, AnimBool show, float width)
        {
            infoMessage[loadPresetInfoMessageTag].show.target = loadPresetAtRuntime.boolValue; //check if a preset is set to load at runtime
            infoMessage[loadPresetInfoMessageTag].title = "Runtime Preset: " + categoryName + " / " + presetName; //set the preset category and name that are set to load at runtime
            DrawInfoMessage(loadPresetInfoMessageTag, width); //draw info if a preset is set to load at runtime
            QUI.Space(SPACE_4 * (1 - show.faded) * infoMessage[loadPresetInfoMessageTag].show.faded); //space added if a preset is set to load at runtime
        }

        void DrawPresetNormalView(AnimType animType, float width)
        {
            tempFloat = (width - 6) / 2 - 5; //dropdown lists width
            QUI.BeginHorizontal(width);
            {
                switch(animType)
                {
                    case AnimType.In:
                        //SELECT PRESET CATEGORY
                        if(!DatabaseInAnimations.ContainsCategoryName(inAnimationsPresetCategoryName.stringValue)) //if the preset category does not exist -> set it to default
                        {
                            inAnimationsPresetCategoryName.stringValue = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                        }
                        inAnimationsPresetCategoryNameIndex.index = DatabaseInAnimations.CategoryNameIndex(inAnimationsPresetCategoryName.stringValue); //update the category index
                        QUI.BeginChangeCheck();
                        inAnimationsPresetCategoryNameIndex.index = EditorGUILayout.Popup(inAnimationsPresetCategoryNameIndex.index, DatabaseInAnimations.categoryNames.ToArray(), GUILayout.Width(tempFloat));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(uiElement, "ChangeCategory");
                            inAnimationsPresetCategoryName.stringValue = DatabaseInAnimations.categoryNames[inAnimationsPresetCategoryNameIndex.index]; //set category naame
                            inAnimationsPresetNameIndex.index = 0; //set name index to 0
                            inAnimationsPresetName.stringValue = DatabaseInAnimations.GetCategory(inAnimationsPresetCategoryName.stringValue).presetNames[inAnimationsPresetNameIndex.index]; //set preset name according to the new index
                        }

                        QUI.Space(SPACE_4);

                        //SELECT PRESET NAME
                        if(!DatabaseInAnimations.Contains(inAnimationsPresetCategoryName.stringValue, inAnimationsPresetName.stringValue)) //if the preset name does not exist in the set category -> set it to index 0 (first item in the category)
                        {
                            inAnimationsPresetNameIndex.index = 0; //update the index
                            inAnimationsPresetName.stringValue = DatabaseInAnimations.GetCategory(inAnimationsPresetCategoryName.stringValue).presetNames[inAnimationsPresetNameIndex.index]; //update the preset name value
                        }
                        else
                        {
                            inAnimationsPresetNameIndex.index = DatabaseInAnimations.ItemNameIndex(inAnimationsPresetCategoryName.stringValue, inAnimationsPresetName.stringValue); //update the item index
                        }
                        QUI.BeginChangeCheck();
                        inAnimationsPresetNameIndex.index = EditorGUILayout.Popup(inAnimationsPresetNameIndex.index, DatabaseInAnimations.GetCategory(inAnimationsPresetCategoryName.stringValue).presetNames.ToArray(), GUILayout.Width(tempFloat * (1 - inAnimationsNewPreset.faded)));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(uiElement, "ChangePreset");
                            inAnimationsPresetName.stringValue = DatabaseInAnimations.GetCategory(inAnimationsPresetCategoryName.stringValue).presetNames[inAnimationsPresetNameIndex.index]; //set preset name according to the new index
                        }
                        break;
                    case AnimType.Out:
                        //SELECT PRESET CATEGORY
                        if(!DatabaseOutAnimations.ContainsCategoryName(outAnimationsPresetCategoryName.stringValue)) //if the preset category does not exist -> set it to default
                        {
                            outAnimationsPresetCategoryName.stringValue = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                        }
                        outAnimationsPresetCategoryNameIndex.index = DatabaseOutAnimations.CategoryNameIndex(outAnimationsPresetCategoryName.stringValue); //update the category index
                        QUI.BeginChangeCheck();
                        outAnimationsPresetCategoryNameIndex.index = EditorGUILayout.Popup(outAnimationsPresetCategoryNameIndex.index, DatabaseOutAnimations.categoryNames.ToArray(), GUILayout.Width(tempFloat));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(uiElement, "ChangeCategory");
                            outAnimationsPresetCategoryName.stringValue = DatabaseOutAnimations.categoryNames[outAnimationsPresetCategoryNameIndex.index]; //set category naame
                            outAnimationsPresetNameIndex.index = 0; //set name index to 0
                            outAnimationsPresetName.stringValue = DatabaseOutAnimations.GetCategory(outAnimationsPresetCategoryName.stringValue).presetNames[outAnimationsPresetNameIndex.index]; //set preset name according to the new index
                        }

                        QUI.Space(SPACE_4);

                        //SELECT PRESET NAME
                        if(!DatabaseOutAnimations.Contains(outAnimationsPresetCategoryName.stringValue, outAnimationsPresetName.stringValue)) //if the preset name does not exist in the set category -> set it to index 0 (first item in the category)
                        {
                            outAnimationsPresetNameIndex.index = 0; //update the index
                            outAnimationsPresetName.stringValue = DatabaseOutAnimations.GetCategory(outAnimationsPresetCategoryName.stringValue).presetNames[outAnimationsPresetNameIndex.index]; //update the preset name value
                        }
                        else
                        {
                            outAnimationsPresetNameIndex.index = DatabaseOutAnimations.ItemNameIndex(outAnimationsPresetCategoryName.stringValue, outAnimationsPresetName.stringValue); //update the item index
                        }
                        QUI.BeginChangeCheck();
                        outAnimationsPresetNameIndex.index = EditorGUILayout.Popup(outAnimationsPresetNameIndex.index, DatabaseOutAnimations.GetCategory(outAnimationsPresetCategoryName.stringValue).presetNames.ToArray(), GUILayout.Width(tempFloat * (1 - outAnimationsNewPreset.faded)));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(uiElement, "ChangePreset");
                            outAnimationsPresetName.stringValue = DatabaseOutAnimations.GetCategory(outAnimationsPresetCategoryName.stringValue).presetNames[outAnimationsPresetNameIndex.index]; //set preset name according to the new index
                        }
                        break;
                    case AnimType.Loop:
                        //SELECT PRESET CATEGORY
                        if(!DatabaseLoopAnimations.ContainsCategoryName(loopAnimationsPresetCategoryName.stringValue)) //if the preset category does not exist -> set it to default
                        {
                            loopAnimationsPresetCategoryName.stringValue = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                        }
                        loopAnimationsPresetCategoryNameIndex.index = DatabaseLoopAnimations.CategoryNameIndex(loopAnimationsPresetCategoryName.stringValue); //update the category index
                        QUI.BeginChangeCheck();
                        loopAnimationsPresetCategoryNameIndex.index = EditorGUILayout.Popup(loopAnimationsPresetCategoryNameIndex.index, DatabaseLoopAnimations.categoryNames.ToArray(), GUILayout.Width(tempFloat));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(uiElement, "ChangeCategory");
                            loopAnimationsPresetCategoryName.stringValue = DatabaseLoopAnimations.categoryNames[loopAnimationsPresetCategoryNameIndex.index]; //set category naame
                            loopAnimationsPresetNameIndex.index = 0; //set name index to 0
                            loopAnimationsPresetName.stringValue = DatabaseLoopAnimations.GetCategory(loopAnimationsPresetCategoryName.stringValue).presetNames[loopAnimationsPresetNameIndex.index]; //set preset name according to the new index
                        }

                        QUI.Space(SPACE_4);

                        //SELECT PRESET NAME
                        if(!DatabaseLoopAnimations.Contains(loopAnimationsPresetCategoryName.stringValue, loopAnimationsPresetName.stringValue)) //if the preset name does not exist in the set category -> set it to index 0 (first item in the category)
                        {
                            loopAnimationsPresetNameIndex.index = 0; //update the index
                            loopAnimationsPresetName.stringValue = DatabaseLoopAnimations.GetCategory(loopAnimationsPresetCategoryName.stringValue).presetNames[loopAnimationsPresetNameIndex.index]; //update the preset name value
                        }
                        else
                        {
                            loopAnimationsPresetNameIndex.index = DatabaseLoopAnimations.ItemNameIndex(loopAnimationsPresetCategoryName.stringValue, loopAnimationsPresetName.stringValue); //update the item index
                        }
                        QUI.BeginChangeCheck();
                        loopAnimationsPresetNameIndex.index = EditorGUILayout.Popup(loopAnimationsPresetNameIndex.index, DatabaseLoopAnimations.GetCategory(loopAnimationsPresetCategoryName.stringValue).presetNames.ToArray(), GUILayout.Width(tempFloat * (1 - loopAnimationsNewPreset.faded)));
                        if(QUI.EndChangeCheck())
                        {
                            Undo.RecordObject(uiElement, "ChangePreset");
                            loopAnimationsPresetName.stringValue = DatabaseLoopAnimations.GetCategory(loopAnimationsPresetCategoryName.stringValue).presetNames[loopAnimationsPresetNameIndex.index]; //set preset name according to the new index
                        }
                        break;
                }
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawPresetNormalViewPresetButtons(AnimType animType, AnimBool newPreset, float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), width, 22);

            QUI.Space(-20);

            tempFloat = (width - SPACE_16) / 3; //button width
            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_4);
                if(QUI.GhostButton("Load Preset", QColors.Color.Blue, tempFloat * (1 - newPreset.faded), MiniBarHeight))
                {
                    LoadPreset(animType);
                }
                QUI.Space(SPACE_4);
                if(newPreset.faded > 0)
                {
                    QUI.FlexibleSpace();
                }
                if(QUI.GhostButton("New Preset", QColors.Color.Green, tempFloat * (1 - newPreset.faded), MiniBarHeight))
                {
                    NewPreset(animType);
                }
                if(newPreset.faded > 0)
                {
                    QUI.FlexibleSpace();
                }
                QUI.Space(SPACE_4);
                if(QUI.GhostButton("Delete Preset", QColors.Color.Red, tempFloat * (1 - newPreset.faded), MiniBarHeight))
                {
                    DeletePreset(animType);
                }
                QUI.Space(SPACE_4);
            }
            QUI.EndHorizontal();
        }

        void DrawPresetNewPresetView(AnimType animType, AnimBool newPreset, float width)
        {
            tempFloat = (width - 6) / 2 - 5; //dropdown lists width
            QUI.BeginHorizontal(width);
            {
                if(createNewCategoryName.faded < 0.5f)
                {
                    switch(animType)
                    {
                        case AnimType.In:
                            //SELECT PRESET CATEGORY
                            QUI.BeginChangeCheck();
                            inAnimationsPresetCategoryNameIndex.index = EditorGUILayout.Popup(inAnimationsPresetCategoryNameIndex.index, DatabaseInAnimations.categoryNames.ToArray(), GUILayout.Width(tempFloat * (1 - createNewCategoryName.faded)));
                            if(QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(uiElement, "ChangeCategory");
                                inAnimationsPresetCategoryName.stringValue = DatabaseInAnimations.categoryNames[inAnimationsPresetCategoryNameIndex.index]; //set category naame
                            }
                            break;
                        case AnimType.Out:
                            //SELECT PRESET CATEGORY
                            QUI.BeginChangeCheck();
                            outAnimationsPresetCategoryNameIndex.index = EditorGUILayout.Popup(outAnimationsPresetCategoryNameIndex.index, DatabaseOutAnimations.categoryNames.ToArray(), GUILayout.Width(tempFloat * (1 - createNewCategoryName.faded)));
                            if(QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(uiElement, "ChangeCategory");
                                outAnimationsPresetCategoryName.stringValue = DatabaseOutAnimations.categoryNames[outAnimationsPresetCategoryNameIndex.index]; //set category naame
                            }
                            break;
                        case AnimType.Loop:
                            //SELECT PRESET CATEGORY
                            QUI.BeginChangeCheck();
                            loopAnimationsPresetCategoryNameIndex.index = EditorGUILayout.Popup(loopAnimationsPresetCategoryNameIndex.index, DatabaseLoopAnimations.categoryNames.ToArray(), GUILayout.Width(tempFloat * (1 - createNewCategoryName.faded)));
                            if(QUI.EndChangeCheck())
                            {
                                Undo.RecordObject(uiElement, "ChangeCategory");
                                loopAnimationsPresetCategoryName.stringValue = DatabaseLoopAnimations.categoryNames[loopAnimationsPresetCategoryNameIndex.index]; //set category naame
                            }
                            break;
                    }

                    QUI.Space(tempFloat * createNewCategoryName.faded);
                }
                else
                {
                    //CREATE NEW CATEGORY
                    QUI.SetNextControlName("newPresetCategoryName");
                    newPresetCategoryName = EditorGUILayout.TextField(newPresetCategoryName, GUILayout.Width(tempFloat * createNewCategoryName.faded));

                    if(createNewCategoryName.isAnimating && createNewCategoryName.target && !QUI.GetNameOfFocusedControl().Equals("newPresetCategoryName")) //select the new category name text field
                    {
                        QUI.FocusTextInControl("newPresetCategoryName");
                    }

                    QUI.Space(tempFloat * (1 - createNewCategoryName.faded));
                }

                QUI.Space(SPACE_4);

                //ENTER A NEW PRESET NAME
                QUI.SetNextControlName("newPresetName");
                newPresetName = EditorGUILayout.TextField(newPresetName, GUILayout.Width(tempFloat * newPreset.faded));

                if((newPreset.isAnimating && newPreset.target && !QUI.GetNameOfFocusedControl().Equals("newPresetName"))
                   || (createNewCategoryName.isAnimating && !createNewCategoryName.target && !QUI.GetNameOfFocusedControl().Equals("newPresetName"))) //select the new preset name text field
                {
                    QUI.FocusTextInControl("newPresetName");
                }

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawPresetNewPresetViewPresetButtons(AnimType animType, AnimBool newPreset, SerializedProperty presetCategoryName, SerializedProperty presetName, float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), width, 22);

            QUI.Space(-20);

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_4);
                QUI.BeginChangeCheck();
                createNewCategoryName.target = QUI.Toggle(createNewCategoryName.target);
                if(QUI.EndChangeCheck())
                {
                    if(createNewCategoryName.target == false) //if the dev decided not to create a new category name, restore the selected new category name
                    {
                        newPresetCategoryName = presetCategoryName.stringValue;
                    }
                }
                QLabel.text = "Create a new category";
                QLabel.style = Style.Text.Normal;
                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(SPACE_2);
                }
                QUI.EndVertical();

                QUI.FlexibleSpace();

                tempFloat = (width - 24) / 4; //button width
                if(QUI.GhostButton("Save Preset", QColors.Color.Green, tempFloat * newPreset.faded, MiniBarHeight)
                    || (QUI.DetectKeyUp(Event.current, KeyCode.Return) && (QUI.GetNameOfFocusedControl().Equals("newPresetName") || QUI.GetNameOfFocusedControl().Equals("newPresetCategoryName"))))
                {
                    if(SavePreset(animType)) //save the new preset -> if a new preset was saved -> update the indexes
                    {
                        switch(animType)
                        {
                            case AnimType.In:
                                inAnimationsPresetCategoryNameIndex.index = DatabaseInAnimations.CategoryNameIndex(presetCategoryName.stringValue); //update the index
                                inAnimationsPresetNameIndex.index = DatabaseInAnimations.ItemNameIndex(presetCategoryName.stringValue, presetName.stringValue); //update the index
                                break;
                            case AnimType.Out:
                                outAnimationsPresetCategoryNameIndex.index = DatabaseOutAnimations.CategoryNameIndex(presetCategoryName.stringValue); //update the index
                                outAnimationsPresetNameIndex.index = DatabaseOutAnimations.ItemNameIndex(presetCategoryName.stringValue, presetName.stringValue); //update the index
                                break;
                            case AnimType.Loop:
                                loopAnimationsPresetCategoryNameIndex.index = DatabaseLoopAnimations.CategoryNameIndex(presetCategoryName.stringValue); //update the index
                                loopAnimationsPresetNameIndex.index = DatabaseLoopAnimations.ItemNameIndex(presetCategoryName.stringValue, presetName.stringValue); //update the index
                                break;
                        }
                    }
                }

                QUI.Space(SPACE_4);

                if(QUI.GhostButton("Cancel", QColors.Color.Red, tempFloat * newPreset.faded, MiniBarHeight)
                     || QUI.DetectKeyUp(Event.current, KeyCode.Escape))
                {
                    ResetNewPresetState();
                }

                QUI.Space(SPACE_4);

                QUI.Space(tempFloat * (1 - newPreset.faded));
            }
            QUI.EndHorizontal();

            QUI.Space(-SPACE_4);
        }

        void Drawsounds(SerializedProperty customSoundAtStart, SerializedProperty soundAtStart, Index soundAtStartIndex,
                        SerializedProperty customSoundAtFinish, SerializedProperty soundAtFinish, Index soundAtFinishIndex,
                        float width)
        {

            DUIUtils.DrawSound(this, "sound @START", customSoundAtStart, soundAtStart, soundAtStartIndex, width);
            DUIUtils.DrawSound(this, "sound @FINISH", customSoundAtFinish, soundAtFinish, soundAtFinishIndex, width);
        }

        void DrawUnityEvents(bool enabled, AnimBool showEvents, SerializedProperty OnStart, string OnStartTitle, SerializedProperty OnFinish, string OnFinishTitle, float width)
        {
            if(QUI.GhostBar("Unity Events", enabled ? QColors.Color.Blue : QColors.Color.Gray, showEvents, width, MiniBarHeight))
            {
                showEvents.target = !showEvents.target;
            }
            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * showEvents.faded);
                if(QUI.BeginFadeGroup(showEvents.faded))
                {
                    QUI.SetGUIBackgroundColor(enabled ? QUI.AccentColorBlue : QUI.AccentColorGray);
                    QUI.BeginVertical(width - SPACE_16);
                    {
                        QUI.Space(SPACE_2 * showEvents.faded);
                        QUI.PropertyField(OnStart, new GUIContent() { text = OnStartTitle }, width - 8);
                        QUI.Space(SPACE_2 * showEvents.faded);
                        QUI.PropertyField(OnFinish, new GUIContent() { text = OnFinishTitle }, width - 8);
                        QUI.Space(SPACE_2 * showEvents.faded);
                    }
                    QUI.EndVertical();
                    QUI.ResetColors();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();
        }

        void LoadPreset(AnimType animType)
        {
            if(serializedObject.isEditingMultipleObjects)
            {
                Undo.RecordObjects(targets, "LoadPreset");
                switch(animType)
                {
                    case AnimType.In:
                        Anim inAnim = UIAnimatorUtil.GetInAnim(inAnimationsPresetCategoryName.stringValue, inAnimationsPresetName.stringValue);
                        for(int i = 0; i < targets.Length; i++) { UIElement iTarget = (UIElement)targets[i]; iTarget.inAnimations = inAnim.Copy(); }
                        break;
                    case AnimType.Out:
                        Anim outAnim = UIAnimatorUtil.GetOutAnim(outAnimationsPresetCategoryName.stringValue, outAnimationsPresetName.stringValue);
                        for(int i = 0; i < targets.Length; i++) { UIElement iTarget = (UIElement)targets[i]; iTarget.outAnimations = outAnim.Copy(); }
                        break;
                    case AnimType.Loop:
                        Loop loopAnim = UIAnimatorUtil.GetLoop(loopAnimationsPresetCategoryName.stringValue, loopAnimationsPresetName.stringValue);
                        for(int i = 0; i < targets.Length; i++)
                        {
                            UIElement iTarget = (UIElement)targets[i];
                            bool autoStart = iTarget.loopAnimations.autoStart;
                            iTarget.loopAnimations = loopAnim.Copy();
                            iTarget.loopAnimations.autoStart = autoStart;
                        }
                        break;
                }
            }
            else
            {
                Undo.RecordObject(uiElement, "LoadPreset");
                switch(animType)
                {
                    case AnimType.In: uiElement.inAnimations = UIAnimatorUtil.GetInAnim(inAnimationsPresetCategoryName.stringValue, inAnimationsPresetName.stringValue).Copy(); break;
                    case AnimType.Out: uiElement.outAnimations = UIAnimatorUtil.GetOutAnim(outAnimationsPresetCategoryName.stringValue, outAnimationsPresetName.stringValue).Copy(); break;
                    case AnimType.Loop:
                        bool autoStart = uiElement.loopAnimations.autoStart;
                        uiElement.loopAnimations = UIAnimatorUtil.GetLoop(loopAnimationsPresetCategoryName.stringValue, loopAnimationsPresetName.stringValue).Copy();
                        uiElement.loopAnimations.autoStart = autoStart;
                        break;
                }
            }
            QUI.ExitGUI();
        }
        void NewPreset(AnimType animType)
        {
            ResetNewPresetState();
            switch(animType)
            {
                case AnimType.In:
                    inAnimationsNewPreset.target = true;
                    newPresetCategoryName = inAnimationsPresetCategoryName.stringValue;
                    break;
                case AnimType.Out:
                    outAnimationsNewPreset.target = true;
                    newPresetCategoryName = outAnimationsPresetCategoryName.stringValue;
                    break;
                case AnimType.Loop:
                    loopAnimationsNewPreset.target = true;
                    newPresetCategoryName = loopAnimationsPresetCategoryName.stringValue;
                    break;
            }
        }
        void DeletePreset(AnimType animType)
        {
            string categoryName = "";
            string presetName = "";
            switch(animType)
            {
                case AnimType.In:
                    categoryName = inAnimationsPresetCategoryName.stringValue;
                    presetName = inAnimationsPresetName.stringValue;
                    break;
                case AnimType.Out:
                    categoryName = outAnimationsPresetCategoryName.stringValue;
                    presetName = outAnimationsPresetName.stringValue;
                    break;
                case AnimType.Loop:
                    categoryName = loopAnimationsPresetCategoryName.stringValue;
                    presetName = loopAnimationsPresetName.stringValue;
                    break;
            }

            if(QUI.DisplayDialog("Delete Preset",
                                            "Are you sure you want to delete the '" + presetName + "' preset from the '" + categoryName + "' preset category?",
                                            "Yes",
                                            "No"))
            {
                switch(animType)
                {
                    case AnimType.In:
                        DUIData.Instance.DatabaseInAnimations.GetCategory(inAnimationsPresetCategoryName.stringValue).DeleteAnimData(inAnimationsPresetName.stringValue, UIAnimatorUtil.RELATIVE_PATH_IN_ANIM_DATA);
                        if(DatabaseInAnimations.GetCategory(inAnimationsPresetCategoryName.stringValue).IsEmpty()) //category is empty -> remove it from the database (sanity check)
                        {
                            DatabaseInAnimations.RemoveCategory(inAnimationsPresetCategoryName.stringValue, UIAnimatorUtil.RELATIVE_PATH_IN_ANIM_DATA, true);
                        }
                        if(serializedObject.isEditingMultipleObjects)
                        {
                            for(int i = 0; i < targets.Length; i++)
                            {
                                UIElement iTarget = (UIElement)targets[i];
                                if(iTarget.inAnimationsPresetCategoryName.Equals(inAnimationsPresetCategoryName.stringValue) ||
                                    iTarget.inAnimationsPresetName.Equals(inAnimationsPresetName.stringValue))
                                {
                                    iTarget.inAnimationsPresetCategoryName = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                                    iTarget.inAnimationsPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                }
                            }
                        }
                        inAnimationsPresetCategoryName.stringValue = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                        inAnimationsPresetName.stringValue = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                        break;
                    case AnimType.Out:
                        DUIData.Instance.DatabaseOutAnimations.GetCategory(outAnimationsPresetCategoryName.stringValue).DeleteAnimData(outAnimationsPresetName.stringValue, UIAnimatorUtil.RELATIVE_PATH_OUT_ANIM_DATA);
                        if(DatabaseOutAnimations.GetCategory(outAnimationsPresetCategoryName.stringValue).IsEmpty()) //category is empty -> remove it from the database (sanity check)
                        {
                            DatabaseOutAnimations.RemoveCategory(outAnimationsPresetCategoryName.stringValue, UIAnimatorUtil.RELATIVE_PATH_OUT_ANIM_DATA, true);
                        }
                        if(serializedObject.isEditingMultipleObjects)
                        {
                            for(int i = 0; i < targets.Length; i++)
                            {
                                UIElement iTarget = (UIElement)targets[i];
                                if(iTarget.outAnimationsPresetCategoryName.Equals(outAnimationsPresetCategoryName.stringValue) ||
                                    iTarget.outAnimationsPresetName.Equals(outAnimationsPresetName.stringValue))
                                {
                                    iTarget.outAnimationsPresetCategoryName = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                                    iTarget.outAnimationsPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                }
                            }
                        }
                        outAnimationsPresetCategoryName.stringValue = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                        outAnimationsPresetName.stringValue = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                        break;
                    case AnimType.Loop:
                        DUIData.Instance.DatabaseLoopAnimations.GetCategory(loopAnimationsPresetCategoryName.stringValue).DeleteLoopData(loopAnimationsPresetName.stringValue, UIAnimatorUtil.RELATIVE_PATH_LOOP_DATA);
                        if(DatabaseLoopAnimations.GetCategory(loopAnimationsPresetCategoryName.stringValue).IsEmpty()) //category is empty -> remove it from the database (sanity check)
                        {
                            DatabaseLoopAnimations.RemoveCategory(loopAnimationsPresetCategoryName.stringValue, UIAnimatorUtil.RELATIVE_PATH_LOOP_DATA, true);
                        }
                        if(serializedObject.isEditingMultipleObjects)
                        {
                            for(int i = 0; i < targets.Length; i++)
                            {
                                UIElement iTarget = (UIElement)targets[i];
                                if(iTarget.loopAnimationsPresetCategoryName.Equals(loopAnimationsPresetCategoryName.stringValue) ||
                                    iTarget.loopAnimationsPresetName.Equals(loopAnimationsPresetName.stringValue))
                                {
                                    iTarget.loopAnimationsPresetCategoryName = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                                    iTarget.loopAnimationsPresetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                                }
                            }
                        }
                        loopAnimationsPresetCategoryName.stringValue = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
                        loopAnimationsPresetName.stringValue = UIAnimatorUtil.DEFAULT_PRESET_NAME;
                        break;
                }
                serializedObject.ApplyModifiedProperties();
            }
        }
        bool SavePreset(AnimType animType)
        {
            if(createNewCategoryName.target && string.IsNullOrEmpty(newPresetCategoryName.Trim()))
            {
                QUI.DisplayDialog("Info",
                                  "The new preset category name cannot be an empty string.",
                                  "Ok");
                return false; //return false that a new preset was not created
            }

            if(string.IsNullOrEmpty(newPresetName.Trim()))
            {
                QUI.DisplayDialog("Info",
                                  "The new preset name cannot be an empty string.",
                                  "Ok");
                return false; //return false that a new preset was not created
            }

            tempBool = false; //test if the database contains the new preset name
            switch(animType)
            {
                case AnimType.In: tempBool = DatabaseInAnimations.Contains(newPresetCategoryName, newPresetName); break;
                case AnimType.Out: tempBool = DatabaseOutAnimations.Contains(newPresetCategoryName, newPresetName); break;
                case AnimType.Loop: tempBool = DatabaseLoopAnimations.Contains(newPresetCategoryName, newPresetName); break;
            }

            if(tempBool)
            {
                QUI.DisplayDialog("Info",
                                  "There is another preset with the '" + newPresetName + "' preset name in the '" + newPresetCategoryName + "' preset category." +
                                    "\n\n" +
                                    "Try a different preset name maybe?",
                                  "Ok");
                return false; //return false that a new preset was not created
            }

            tempBool = false; //test if the database contains the new category
            switch(animType)
            {
                case AnimType.In: tempBool = !DatabaseInAnimations.ContainsCategory(newPresetCategoryName); break;
                case AnimType.Out: tempBool = !DatabaseOutAnimations.ContainsCategory(newPresetCategoryName); break;
                case AnimType.Loop: tempBool = !DatabaseLoopAnimations.ContainsCategory(newPresetCategoryName); break;
            }

            if(tempBool) //if creating a new category, check that it does not already exist
            {
                switch(animType) //create the new category 
                {
                    case AnimType.In: DatabaseInAnimations.AddCategory(newPresetCategoryName, UIAnimatorUtil.RELATIVE_PATH_IN_ANIM_DATA, true); break;
                    case AnimType.Out: DatabaseOutAnimations.AddCategory(newPresetCategoryName, UIAnimatorUtil.RELATIVE_PATH_OUT_ANIM_DATA, true); break;
                    case AnimType.Loop: DatabaseLoopAnimations.AddCategory(newPresetCategoryName, UIAnimatorUtil.RELATIVE_PATH_LOOP_DATA, true); break;
                }
            }

            switch(animType) //create the new preset
            {
                case AnimType.In: DatabaseInAnimations.GetCategory(newPresetCategoryName).AddAnimData(UIAnimatorUtil.CreateInAnimPreset(newPresetCategoryName, newPresetName, uiElement.inAnimations.Copy())); break;
                case AnimType.Out: DatabaseOutAnimations.GetCategory(newPresetCategoryName).AddAnimData(UIAnimatorUtil.CreateOutAnimPreset(newPresetCategoryName, newPresetName, uiElement.outAnimations.Copy())); break;
                case AnimType.Loop: DatabaseLoopAnimations.GetCategory(newPresetCategoryName).AddLoopData(UIAnimatorUtil.CreateLoopPreset(newPresetCategoryName, newPresetName, uiElement.loopAnimations.Copy())); break;
            }

            switch(animType) //set the values
            {
                case AnimType.In:
                    if(serializedObject.isEditingMultipleObjects)
                    {
                        for(int i = 0; i < targets.Length; i++)
                        {
                            UIElement iTarget = (UIElement)targets[i];
                            iTarget.inAnimationsPresetCategoryName = newPresetCategoryName;
                            iTarget.inAnimationsPresetName = newPresetName;
                        }
                    }
                    else
                    {
                        inAnimationsPresetCategoryName.stringValue = newPresetCategoryName;
                        inAnimationsPresetName.stringValue = newPresetName;
                    }
                    break;
                case AnimType.Out:
                    if(serializedObject.isEditingMultipleObjects)
                    {
                        for(int i = 0; i < targets.Length; i++)
                        {
                            UIElement iTarget = (UIElement)targets[i];
                            iTarget.outAnimationsPresetCategoryName = newPresetCategoryName;
                            iTarget.outAnimationsPresetName = newPresetName;
                        }
                    }
                    else
                    {
                        outAnimationsPresetCategoryName.stringValue = newPresetCategoryName;
                        outAnimationsPresetName.stringValue = newPresetName;
                    }
                    break;
                case AnimType.Loop:
                    if(serializedObject.isEditingMultipleObjects)
                    {
                        for(int i = 0; i < targets.Length; i++)
                        {
                            UIElement iTarget = (UIElement)targets[i];
                            iTarget.loopAnimationsPresetCategoryName = newPresetCategoryName;
                            iTarget.loopAnimationsPresetName = newPresetName;
                        }
                    }
                    else
                    {
                        loopAnimationsPresetCategoryName.stringValue = newPresetCategoryName;
                        loopAnimationsPresetName.stringValue = newPresetName;
                    }
                    break;
            }

            serializedObject.ApplyModifiedProperties();
            ResetNewPresetState();
            return true; //return true that a new preset has been created
        }
        void ResetNewPresetState()
        {
            inAnimationsNewPreset.target = false;
            outAnimationsNewPreset.target = false;
            loopAnimationsNewPreset.target = false;

            createNewCategoryName.target = false;
            newPresetCategoryName = "";
            newPresetName = "";

            QUI.ResetKeyboardFocus();
        }

        void DrawSpecialAnimationsButtons(float width)
        {
            QUI.BeginHorizontal(width);
            {
                if(QUI.GhostButton("IN -> OUT", QColors.Color.Orange))
                {
                    for(int i = 0; i < targets.Length; i++)
                    {
                        UIElement iTarget = (UIElement)targets[i];
                        iTarget.outAnimations = iTarget.inAnimations.Copy();
                        iTarget.outAnimations.animationType = Anim.AnimationType.Out;
                        iTarget.outAnimations.move.animationType = Anim.AnimationType.Out;
                        iTarget.outAnimations.rotate.animationType = Anim.AnimationType.Out;
                        iTarget.outAnimations.scale.animationType = Anim.AnimationType.Out;
                        iTarget.outAnimations.fade.animationType = Anim.AnimationType.Out;
                    }
                }
                if(QUI.GhostButton("IN <- OUT", QColors.Color.Orange))
                {
                    for(int i = 0; i < targets.Length; i++)
                    {
                        UIElement iTarget = (UIElement)targets[i];
                        iTarget.inAnimations = iTarget.outAnimations.Copy();
                        iTarget.inAnimations.animationType = Anim.AnimationType.In;
                        iTarget.inAnimations.move.animationType = Anim.AnimationType.In;
                        iTarget.inAnimations.rotate.animationType = Anim.AnimationType.In;
                        iTarget.inAnimations.scale.animationType = Anim.AnimationType.In;
                        iTarget.inAnimations.fade.animationType = Anim.AnimationType.In;
                    }
                }
            }
            QUI.EndHorizontal();
            QUI.Space(SPACE_8);
        }

        void CheckIfLinkedToNotification()
        {
            uiElement.linkedToNotification = false;
            uiElement.autoRegister = true;

            Transform parent = uiElement.transform.parent;
            while(parent != null)
            {
                if(parent.GetComponent<UINotification>() != null)
                {
                    UINotification n = parent.GetComponent<UINotification>();
                    if(n.notificationContainer != null && n.notificationContainer == uiElement)
                    {
                        uiElement.linkedToNotification = true;
                        uiElement.autoRegister = false;
                        uiElement.name = DUI.DUISettings.UIElement_Inspector_RenameGameObjectPrefix + "Notification Container" + DUI.DUISettings.UIElement_Inspector_RenameGameObjectSuffix;
                    }
                    else if(n.overlay != null && n.overlay == uiElement)
                    {
                        uiElement.linkedToNotification = true;
                        uiElement.autoRegister = false;
                        uiElement.name = DUI.DUISettings.UIElement_Inspector_RenameGameObjectPrefix + "Background Overlay" + DUI.DUISettings.UIElement_Inspector_RenameGameObjectSuffix;
                    }
                    else if(n.specialElements != null && n.specialElements.Length > 0)
                    {
                        for(int i = 0; i < n.specialElements.Length; i++)
                        {
                            if(n.specialElements[i] == null) { continue; }
                            if(n.specialElements[i] != uiElement) { continue; }
                            uiElement.linkedToNotification = true;
                            uiElement.autoRegister = false;
                            uiElement.name = DUI.DUISettings.UIElement_Inspector_RenameGameObjectPrefix + "Special Element " + i + DUI.DUISettings.UIElement_Inspector_RenameGameObjectSuffix;
                        }
                    }
                    break;
                }
                if(parent != null) { parent = parent.transform.parent; }
            }
        }
        void UnlinkFromNotification()
        {
            elementCategory.stringValue = DUI.CUSTOM_NAME;

            Transform parent = uiElement.transform.parent;
            while(parent != null)
            {
                if(parent.GetComponent<UINotification>() != null)
                {
                    UINotification n = parent.GetComponent<UINotification>();
                    if(n.notificationContainer != null && n.notificationContainer == uiElement)
                    {
                        n.notificationContainer = null;
                        break;
                    }
                    else if(n.overlay != null && n.overlay == uiElement)
                    {
                        n.overlay = null;
                        break;
                    }
                    else if(n.specialElements != null && n.specialElements.Length > 0)
                    {
                        for(int i = 0; i < n.specialElements.Length; i++)
                        {
                            if(n.specialElements[i] == null) { continue; }
                            if(n.specialElements[i] != uiElement) { continue; }
                            n.specialElements[i] = null;
                            break;
                        }
                    }
                    break;
                }
                if(parent != null) { parent = parent.transform.parent; }
            }

            linkedToNotification.boolValue = false;
            autoRegister.boolValue = true;
            uiElement.name = DUI.DUISettings.UIElement_Inspector_RenameGameObjectPrefix + "Unlinked from Notification" + DUI.DUISettings.UIElement_Inspector_RenameGameObjectSuffix;
        }

        void DrawMainBar(string title, bool enabled, SerializedProperty loadPresetAtRuntime, AnimBool show, AnimBool newPreset, float width, float height)
        {
            if(QUI.GhostBar(title, !enabled && !loadPresetAtRuntime.boolValue ? QColors.Color.Gray : QColors.Color.Blue, show, width - height * 4, height))
            {
                show.target = !show.target;
                if(!show.target) //if closing -> reset any new preset settings
                {
                    if(newPreset.target)
                    {
                        ResetNewPresetState();
                        QUI.ExitGUI();
                    }
                }
            }
        }
        void DrawMainGhostButtons(AnimType animType, float width, float height)
        {
            switch(animType)
            {
                case AnimType.In:
                    if(QUI.GhostButton("M", uiElement.inAnimations.move.enabled ? QColors.Color.Green : QColors.Color.Gray, BarHeight, BarHeight, showInAnimations.target))
                    {
                        Undo.RecordObject(uiElement, "ToggleMove" + animType);
                        uiElement.inAnimations.move.enabled = !uiElement.inAnimations.move.enabled;
                        if(uiElement.inAnimations.move.enabled) { showInAnimations.target = true; }
                    }
                    if(QUI.GhostButton("R", uiElement.inAnimations.rotate.enabled ? QColors.Color.Orange : QColors.Color.Gray, BarHeight, BarHeight, showInAnimations.target))
                    {
                        Undo.RecordObject(uiElement, "ToggleRotate" + animType);
                        uiElement.inAnimations.rotate.enabled = !uiElement.inAnimations.rotate.enabled;
                        if(uiElement.inAnimations.rotate.enabled) { showInAnimations.target = true; }
                    }
                    if(QUI.GhostButton("S", uiElement.inAnimations.scale.enabled ? QColors.Color.Red : QColors.Color.Gray, BarHeight, BarHeight, showInAnimations.target))
                    {
                        Undo.RecordObject(uiElement, "ToggleScale" + animType);
                        uiElement.inAnimations.scale.enabled = !uiElement.inAnimations.scale.enabled;
                        if(uiElement.inAnimations.scale.enabled) { showInAnimations.target = true; }
                    }
                    if(QUI.GhostButton("F", uiElement.inAnimations.fade.enabled ? QColors.Color.Purple : QColors.Color.Gray, BarHeight, BarHeight, showInAnimations.target))
                    {
                        Undo.RecordObject(uiElement, "ToggleFade" + animType);
                        uiElement.inAnimations.fade.enabled = !uiElement.inAnimations.fade.enabled;
                        if(uiElement.inAnimations.fade.enabled) { showInAnimations.target = true; }
                    }
                    break;
                case AnimType.Out:
                    if(QUI.GhostButton("M", uiElement.outAnimations.move.enabled ? QColors.Color.Green : QColors.Color.Gray, BarHeight, BarHeight, showOutAnimations.target))
                    {
                        Undo.RecordObject(uiElement, "ToggleMove" + animType);
                        uiElement.outAnimations.move.enabled = !uiElement.outAnimations.move.enabled;
                        if(uiElement.outAnimations.move.enabled) { showOutAnimations.target = true; }
                    }
                    if(QUI.GhostButton("R", uiElement.outAnimations.rotate.enabled ? QColors.Color.Orange : QColors.Color.Gray, BarHeight, BarHeight, showOutAnimations.target))
                    {
                        Undo.RecordObject(uiElement, "ToggleRotate" + animType);
                        uiElement.outAnimations.rotate.enabled = !uiElement.outAnimations.rotate.enabled;
                        if(uiElement.outAnimations.rotate.enabled) { showOutAnimations.target = true; }
                    }
                    if(QUI.GhostButton("S", uiElement.outAnimations.scale.enabled ? QColors.Color.Red : QColors.Color.Gray, BarHeight, BarHeight, showOutAnimations.target))
                    {
                        Undo.RecordObject(uiElement, "ToggleScale" + animType);
                        uiElement.outAnimations.scale.enabled = !uiElement.outAnimations.scale.enabled;
                        if(uiElement.outAnimations.scale.enabled) { showOutAnimations.target = true; }
                    }
                    if(QUI.GhostButton("F", uiElement.outAnimations.fade.enabled ? QColors.Color.Purple : QColors.Color.Gray, BarHeight, BarHeight, showOutAnimations.target))
                    {
                        Undo.RecordObject(uiElement, "ToggleFade" + animType);
                        uiElement.outAnimations.fade.enabled = !uiElement.outAnimations.fade.enabled;
                        if(uiElement.outAnimations.fade.enabled) { showOutAnimations.target = true; }
                    }
                    break;
                case AnimType.Loop:
                    if(QUI.GhostButton("M", uiElement.loopAnimations.move.enabled ? QColors.Color.Green : QColors.Color.Gray, BarHeight, BarHeight, showLoopAnimations.target))
                    {
                        Undo.RecordObject(uiElement, "ToggleMove" + animType);
                        uiElement.loopAnimations.move.enabled = !uiElement.loopAnimations.move.enabled;
                        if(uiElement.loopAnimations.move.enabled) { showLoopAnimations.target = true; }
                    }
                    if(QUI.GhostButton("R", uiElement.loopAnimations.rotate.enabled ? QColors.Color.Orange : QColors.Color.Gray, BarHeight, BarHeight, showLoopAnimations.target))
                    {
                        Undo.RecordObject(uiElement, "ToggleRotate" + animType);
                        uiElement.loopAnimations.rotate.enabled = !uiElement.loopAnimations.rotate.enabled;
                        if(uiElement.loopAnimations.rotate.enabled) { showLoopAnimations.target = true; }
                    }
                    if(QUI.GhostButton("S", uiElement.loopAnimations.scale.enabled ? QColors.Color.Red : QColors.Color.Gray, BarHeight, BarHeight, showLoopAnimations.target))
                    {
                        Undo.RecordObject(uiElement, "ToggleScale" + animType);
                        uiElement.loopAnimations.scale.enabled = !uiElement.loopAnimations.scale.enabled;
                        if(uiElement.loopAnimations.scale.enabled) { showLoopAnimations.target = true; }
                    }
                    if(QUI.GhostButton("F", uiElement.loopAnimations.fade.enabled ? QColors.Color.Purple : QColors.Color.Gray, BarHeight, BarHeight, showLoopAnimations.target))
                    {
                        Undo.RecordObject(uiElement, "ToggleFade" + animType);
                        uiElement.loopAnimations.fade.enabled = !uiElement.loopAnimations.fade.enabled;
                        if(uiElement.loopAnimations.fade.enabled) { showLoopAnimations.target = true; }
                    }
                    break;
            }

        }
    }
}
