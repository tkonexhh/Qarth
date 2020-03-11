// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using QuickEngine.Extensions;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace DoozyUI
{
    [CustomEditor(typeof(UITrigger), true)]
    [CanEditMultipleObjects]
    public class UITriggerEditor : QEditor
    {
        UITrigger uiTrigger { get { return (UITrigger)target; } }

        DUIData.Database DatabaseUIButtons { get { return DUIData.Instance.DatabaseUIButtons; } }

        Color AccentColorBlue { get { return QUI.IsProSkin ? QColors.Blue.Color : QColors.BlueLight.Color; } }
        Color AccentColorGray { get { return QUI.IsProSkin ? QColors.UnityLight.Color : QColors.UnityLight.Color; } }

        SerializedProperty
            triggerOnGameEvent, triggerOnButtonClick, triggerOnButtonDoubleClick, triggerOnButtonLongClick,
            dispatchAll,
            gameEvent, buttonCategory, buttonName,
            onTriggerEvent,
            gameEvents;

        AnimBool
            showGameEventsAnimBool, dispatchAllAnimBool;

        int buttonNameIndex = 0;
        int buttonCategoryIndex = 0;

        float GlobalWidth { get { return DUI.GLOBAL_EDITOR_WIDTH; } }
        int BarHeight { get { return DUI.BAR_HEIGHT; } }
        int MiniBarHeight { get { return DUI.MINI_BAR_HEIGHT; } }

        float tempFloat;

        protected override void SerializedObjectFindProperties()
        {
            base.SerializedObjectFindProperties();

            triggerOnGameEvent = serializedObject.FindProperty("triggerOnGameEvent");
            triggerOnButtonClick = serializedObject.FindProperty("triggerOnButtonClick");
            triggerOnButtonDoubleClick = serializedObject.FindProperty("triggerOnButtonDoubleClick");
            triggerOnButtonLongClick = serializedObject.FindProperty("triggerOnButtonLongClick");
            dispatchAll = serializedObject.FindProperty("dispatchAll");
            gameEvent = serializedObject.FindProperty("gameEvent");
            buttonCategory = serializedObject.FindProperty("buttonCategory");
            buttonName = serializedObject.FindProperty("buttonName");
            onTriggerEvent = serializedObject.FindProperty("onTriggerEvent");
            gameEvents = serializedObject.FindProperty("gameEvents");
        }

        protected override void GenerateInfoMessages()
        {
            base.GenerateInfoMessages();

            infoMessage.Add("Disabled",
                            new InfoMessage()
                            {
                                title = "Select what would you like this trigger to listen for.",
                                message = "",
                                type = InfoMessageType.Error,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("SetGameEvent",
                            new InfoMessage()
                            {
                                title = "Set a game event to listen for or enable 'dispatch all' game events.",
                                message = "",
                                type = InfoMessageType.Error,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("SetButtonName",
                          new InfoMessage()
                          {
                              title = "Set a button name to listen for or enable 'dispatch all' button clicks.",
                              message = "",
                              type = InfoMessageType.Error,
                              show = new AnimBool(false, Repaint)
                          });

            infoMessage.Add("DispatchAllGameEvents",
                           new InfoMessage()
                           {
                               title = "Any game event, sent by the system, will trigger this UITrigger.",
                               message = "",
                               type = InfoMessageType.Info,
                               show = new AnimBool(false, Repaint)
                           });

            infoMessage.Add("DispatchAllButtonClicks",
                          new InfoMessage()
                          {
                              title = "Any click, registered by the system, will trigger this UITrigger.",
                              message = "",
                              type = InfoMessageType.Info,
                              show = new AnimBool(false, Repaint)
                          });

            infoMessage.Add("DispatchAllDoubleButtonClicks",
                         new InfoMessage()
                         {
                             title = "Any double click, registered by the system, will trigger this UITrigger.",
                             message = "",
                             type = InfoMessageType.Info,
                             show = new AnimBool(false, Repaint)
                         });

            infoMessage.Add("DispatchAllLongButtonClicks",
                        new InfoMessage()
                        {
                            title = "Any long click, registered by the system, will trigger this UITrigger.",
                            message = "",
                            type = InfoMessageType.Info,
                            show = new AnimBool(false, Repaint)
                        });
        }

        protected override void InitAnimBools()
        {
            base.InitAnimBools();

            showGameEventsAnimBool = new AnimBool(gameEvents.arraySize > 0, Repaint);
            dispatchAllAnimBool = new AnimBool(dispatchAll.boolValue, Repaint);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            requiresContantRepaint = true;

            SyncData();
        }

        void SyncData()
        {
            DUIData.Instance.ValidateUIButtons(); //validate the database
            ValidateButtonCategoryAndButtonName();
        }
        void ValidateButtonCategoryAndButtonName()
        {
            if(dispatchAll.boolValue)
            {
                buttonCategory.stringValue = DUI.UNCATEGORIZED_CATEGORY_NAME;
                buttonName.stringValue = DUI.DISPATCH_ALL;
                return;
            }

            //CHECK FOR CUSTOM NAME
            if(uiTrigger.buttonCategory.Equals(DUI.CUSTOM_NAME)) //category is set to CUSTOM NAME -> get the index for the category name and set the name index to -1
            {
                buttonCategoryIndex = DatabaseUIButtons.CategoryNameIndex(DUI.CUSTOM_NAME); //set the index
                buttonNameIndex = -1; //set the index
                return; //stop here as there is a CUSTOM NAME set
            }

            //SANITY CHECK FOR EMPTY CATEGORY NAME
            if(uiTrigger.buttonCategory.IsNullOrEmpty()) //category name is empty (sanity check) -> reset both category and name
            {
                uiTrigger.buttonCategory = DUI.UNCATEGORIZED_CATEGORY_NAME; //reset the value
                buttonCategoryIndex = DatabaseUIButtons.CategoryNameIndex(uiTrigger.buttonCategory); //set the index

                uiTrigger.buttonName = DUI.DEFAULT_BUTTON_NAME; //reset the value
                buttonNameIndex = DatabaseUIButtons.ItemNameIndex(uiTrigger.buttonCategory, uiTrigger.buttonName); //set the index
                return;
            }

            //CHECK THAT CATEGORY EXISTS IN THE DATABASE
            if(!DatabaseUIButtons.ContainsCategoryName(uiTrigger.buttonCategory)) //the set category does not exist in the database
            {
                if(QUI.DisplayDialog("Action Required",
                                     "The category '" + uiTrigger.buttonCategory + "' was not found in the database." +
                                       "\n\n" +
                                       "Do you want to add it to the database?",
                                     "Yes",
                                     "No")) //ask the dev if he wants to add this category to the database
                {
                    DatabaseUIButtons.AddCategory(uiTrigger.buttonCategory, true); //add the category to the database and save
                    buttonCategoryIndex = DatabaseUIButtons.CategoryNameIndex(uiTrigger.buttonCategory); //set the index
                }
                else
                {
                    QUI.DisplayDialog("Info",
                                      "Button category has been reset to the default '" + DUI.UNCATEGORIZED_CATEGORY_NAME + "' value.",
                                      "Ok"); //inform the dev that becuase he did not add the category to the database, it has been reset to its default value
                    uiTrigger.buttonCategory = DUI.UNCATEGORIZED_CATEGORY_NAME; //reset the value
                    buttonCategoryIndex = DatabaseUIButtons.CategoryNameIndex(uiTrigger.buttonCategory); //set the index
                }
            }

            //CHECK THAT THE NAME EXISTS IN THE CATEGORY
            if(!DatabaseUIButtons.Contains(uiTrigger.buttonCategory, uiTrigger.buttonName)) //the set element name does not exist under the set category
            {
                if(QUI.DisplayDialog("Action Required",
                                     "The name '" + uiTrigger.buttonName + "' was not found in the '" + uiTrigger.buttonCategory + "' category." +
                                       "\n\n" +
                                       "Do you want to add it to the database?",
                                     "Yes",
                                     "No")) //ask the dev if he wants to add this name to the database
                {
                    DatabaseUIButtons.GetCategory(uiTrigger.buttonCategory).AddItemName(uiTrigger.buttonName, true); //add the item name to the database and save
                    buttonNameIndex = DatabaseUIButtons.ItemNameIndex(uiTrigger.buttonCategory, uiTrigger.buttonName); //set the index
                }
                else
                {
                    QUI.DisplayDialog("Info",
                                      "Button name has been reset to the default '" + DUI.DEFAULT_BUTTON_NAME + "' value.",
                                      "Ok"); //inform the dev that becuase he did not add the name to the database, it has been reset to its default value
                    uiTrigger.buttonName = DUI.DEFAULT_BUTTON_NAME; //reset the value
                    buttonNameIndex = DatabaseUIButtons.ItemNameIndex(uiTrigger.buttonCategory, uiTrigger.buttonName); //set the index
                }
            }
            else
            {
                buttonCategoryIndex = DatabaseUIButtons.CategoryNameIndex(uiTrigger.buttonCategory);
                buttonNameIndex = DatabaseUIButtons.ItemNameIndex(uiTrigger.buttonCategory, uiTrigger.buttonName);
            }
        }

        public override void OnInspectorGUI()
        {
            DrawHeader(DUIResources.headerUITrigger.texture, WIDTH_420, HEIGHT_42);

            serializedObject.Update();

            DrawListenSelector(GlobalWidth);

            dispatchAllAnimBool.target = dispatchAll.boolValue;

            infoMessage["Disabled"].show.target = !triggerOnGameEvent.boolValue && !triggerOnButtonClick.boolValue && !triggerOnButtonDoubleClick.boolValue && !triggerOnButtonLongClick.boolValue;
            DrawInfoMessage("Disabled", GlobalWidth);

            DrawGameEventOptions(GlobalWidth);

            DrawButtonNameOptions(GlobalWidth);

            QUI.Space(SPACE_2);

            DrawEvents(GlobalWidth);

            serializedObject.ApplyModifiedProperties();

            QUI.Space(SPACE_4);
        }

        void DrawDatabaseButtons(float width)
        {
            if(QUI.GhostButton("UIButtons Database", QColors.Color.Gray, width, 18))
            {
                ControlPanelWindow.OpenWindow(ControlPanelWindow.Page.UIButtons);
            }
        }

        void DrawListenSelector(float width)
        {
            GUI.enabled = false; //disable gui interractions

            QUI.BeginHorizontal(width);
            {
                GUI.enabled = !triggerOnButtonClick.boolValue && !triggerOnButtonDoubleClick.boolValue && !triggerOnButtonLongClick.boolValue;
                QUI.BeginChangeCheck();
                QUI.QToggle("game event", triggerOnGameEvent);
                if(QUI.EndChangeCheck())
                {
                    if(triggerOnGameEvent.boolValue && !dispatchAll.boolValue) //if game event has been selected and dispatch all is disabled -> select the text field
                    {
                        QUI.FocusTextInControl("gameEvent");
                    }
                    if(!triggerOnGameEvent.boolValue) //if game event has been disabled -> reset game event string
                    {
                        gameEvent.stringValue = dispatchAll.boolValue ? DUI.DISPATCH_ALL : string.Empty;
                    }
                }

                GUI.enabled = false;
                QUI.Space(SPACE_8);

                GUI.enabled = !triggerOnGameEvent.boolValue && !triggerOnButtonDoubleClick.boolValue && !triggerOnButtonLongClick.boolValue;
                QUI.QToggle("button click", triggerOnButtonClick);

                GUI.enabled = false;
                QUI.Space(SPACE_8);

                GUI.enabled = !triggerOnGameEvent.boolValue && !triggerOnButtonClick.boolValue && !triggerOnButtonLongClick.boolValue;
                QUI.QToggle("double click", triggerOnButtonDoubleClick);

                GUI.enabled = false;
                QUI.Space(SPACE_8);

                GUI.enabled = !triggerOnGameEvent.boolValue && !triggerOnButtonClick.boolValue && !triggerOnButtonDoubleClick.boolValue;
                QUI.QToggle("long click", triggerOnButtonLongClick);

                QUI.FlexibleSpace();

            }
            QUI.EndHorizontal();

            GUI.enabled = true; //enable gui interractions
        }

        void DrawGameEventOptions(float width)
        {
            if(!triggerOnGameEvent.boolValue)
            {
                return;
            }

            buttonCategory.stringValue = DUI.UNCATEGORIZED_CATEGORY_NAME;
            buttonName.stringValue = DUI.DEFAULT_BUTTON_NAME;

            QUI.BeginHorizontal(width);
            {
                if(dispatchAll.boolValue) //if dispatchAll is enabled -> set game event to empty and disable the field
                {
                    gameEvent.stringValue = string.Empty; //set game event as an empty string
                    GUI.enabled = false; //disable gui
                }

                QLabel.text = "Game Event";
                QLabel.style = Style.Text.Normal;
                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(SPACE_2);
                }
                QUI.EndVertical();

                tempFloat = QLabel.x; //save label width

                QLabel.text = "dispatch all"; //create and calculate a new label
                QLabel.style = Style.Text.Normal;

                tempFloat += QLabel.x; //add the second label width
                tempFloat += 12; //add the size of a toggle
                tempFloat += 8; //add space (4 to left and 4 to right)
                tempFloat += 24;

                QUI.SetNextControlName("gameEvent");
                EditorGUILayout.DelayedTextField(gameEvent, GUIContent.none, GUILayout.Width(width - tempFloat));

                GUI.enabled = true; //enable gui in case it was disabled by dispatch all

                QUI.BeginChangeCheck();
                QUI.QToggle("dispatch all", dispatchAll);
                if(QUI.EndChangeCheck())
                {
                    if(triggerOnGameEvent.boolValue)
                    {
                        gameEvent.stringValue = dispatchAll.boolValue ? DUI.DISPATCH_ALL : string.Empty;
                        if(!dispatchAll.boolValue)
                        {
                            QUI.FocusTextInControl("gameEvent");
                        }
                    }
                    else if(triggerOnButtonClick.boolValue || triggerOnButtonDoubleClick.boolValue || triggerOnButtonLongClick.boolValue)
                    {
                        buttonName.stringValue = dispatchAll.boolValue ? DUI.DISPATCH_ALL : DUI.DEFAULT_BUTTON_NAME;
                        if(dispatchAll.boolValue)
                        {
                            buttonCategory.stringValue = DUI.UNCATEGORIZED_CATEGORY_NAME;
                        }
                        else
                        {
                            buttonCategory.stringValue = DUI.UNCATEGORIZED_CATEGORY_NAME;
                            buttonName.stringValue = DUI.DEFAULT_BUTTON_NAME;
                            ValidateButtonCategoryAndButtonName();
                        }
                    }
                }
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            infoMessage["SetGameEvent"].show.target = triggerOnGameEvent.boolValue && string.IsNullOrEmpty(gameEvent.stringValue) && !dispatchAll.boolValue;
            DrawInfoMessage("SetGameEvent", GlobalWidth);

            infoMessage["DispatchAllGameEvents"].show.target = dispatchAll.boolValue;
            DrawInfoMessage("DispatchAllGameEvents", width);
        }

        void DrawButtonNameOptions(float width)
        {
            if(!triggerOnButtonClick.boolValue
                && !triggerOnButtonDoubleClick.boolValue
                && !triggerOnButtonLongClick.boolValue)
            {
                return;
            }

            gameEvent.stringValue = string.Empty;

            DrawDatabaseButtons(width);

            QUI.Space(SPACE_4);

            QUI.BeginHorizontal(width);
            {
                QUI.QToggle("dispatch all", dispatchAll);
                QUI.FlexibleSpace();

            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2);

            if(dispatchAll.boolValue)
            {
                gameEvent.stringValue = string.Empty;
            }

            QUI.Space(-SPACE_8 * dispatchAllAnimBool.faded);

            if(QUI.BeginFadeGroup((1 - dispatchAllAnimBool.faded)))
            {
                QUI.BeginVertical(width);
                {
                    DrawButtonCategoryAndButtonName(width);
                }
                QUI.EndVertical();
            }
            QUI.EndFadeGroup();

            QUI.Space(SPACE_2);

            infoMessage["SetButtonName"].show.target = (string.IsNullOrEmpty(buttonName.stringValue) || buttonName.stringValue.Equals(DUI.DEFAULT_BUTTON_NAME)) && !dispatchAll.boolValue;
            DrawInfoMessage("SetButtonName", GlobalWidth);

            infoMessage["DispatchAllButtonClicks"].show.target = triggerOnButtonClick.boolValue && dispatchAll.boolValue;
            DrawInfoMessage("DispatchAllButtonClicks", GlobalWidth);

            infoMessage["DispatchAllDoubleButtonClicks"].show.target = triggerOnButtonDoubleClick.boolValue && dispatchAll.boolValue;
            DrawInfoMessage("DispatchAllDoubleButtonClicks", GlobalWidth);

            infoMessage["DispatchAllLongButtonClicks"].show.target = triggerOnButtonLongClick.boolValue && dispatchAll.boolValue;
            DrawInfoMessage("DispatchAllLongButtonClicks", GlobalWidth);
        }

        void DrawButtonCategoryAndButtonName(float width)
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

                QLabel.text = "Button Category";
                QLabel.style = Style.Text.Small;
                QUI.BeginHorizontal((width - 6) / 2);
                {
                    QUI.Label(QLabel);
                    QUI.FlexibleSpace();
                }
                QUI.EndHorizontal();

                QLabel.text = "Button Name";
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
                DrawButtonCategory((width - 6) / 2);
                DrawButtonName((width - 6) / 2);
            }
            QUI.EndHorizontal();
        }
        void DrawButtonCategory(float width)
        {
            QUI.BeginHorizontal(width);
            {
                if(EditorApplication.isPlayingOrWillChangePlaymode)
                {
                    QLabel.text = buttonCategory.stringValue;
                    QLabel.style = Style.Text.Help;
                    QUI.Label(QLabel);
                }
                else
                {
                    buttonCategoryIndex = DatabaseUIButtons.CategoryNameIndex(buttonCategory.stringValue); //set the index
                    QUI.BeginChangeCheck();
                    buttonCategoryIndex = EditorGUILayout.Popup(buttonCategoryIndex, DatabaseUIButtons.categoryNames.ToArray(), GUILayout.Width(width - 5));
                    if(QUI.EndChangeCheck())
                    {
                        buttonCategory.stringValue = DatabaseUIButtons.categoryNames[buttonCategoryIndex];
                        if(buttonCategory.stringValue == DUI.CUSTOM_NAME)
                        {
                            buttonNameIndex = -1;
                        }
                    }
                }

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawButtonName(float width)
        {
            QUI.BeginHorizontal(width);
            {
                if(EditorApplication.isPlayingOrWillChangePlaymode)
                {

                    QLabel.text = buttonName.stringValue;
                    QLabel.style = Style.Text.Help;
                    QUI.Label(QLabel);
                }
                else
                {
                    if(buttonCategory.stringValue.Equals(DUI.CUSTOM_NAME))
                    {
                        QUI.PropertyField(buttonName, width - 5);
                    }
                    else
                    {
                        if(!DatabaseUIButtons.ContainsCategoryName(buttonCategory.stringValue)) //the category does not exist -> reset category and name
                        {
                            QUI.DisplayDialog("Info",
                                                 "Button category has been reset to the default '" + DUI.UNCATEGORIZED_CATEGORY_NAME + "' value." +
                                                   "\n\n" +
                                                   "Button name has been reset to the default '" + DUI.DEFAULT_BUTTON_NAME + "' value.",
                                                 "Ok"); //inform the dev that becuase he did not add the name to the database, it has been reset to its default value
                            buttonCategory.stringValue = DUI.UNCATEGORIZED_CATEGORY_NAME; //reset the category
                            buttonCategoryIndex = DatabaseUIButtons.CategoryNameIndex(buttonCategory.stringValue); //set the index
                            buttonName.stringValue = DUI.DEFAULT_BUTTON_NAME; //reset the name
                            buttonNameIndex = DatabaseUIButtons.ItemNameIndex(buttonCategory.stringValue, buttonName.stringValue); //set the index
                        }
                        else if(!DatabaseUIButtons.Contains(buttonCategory.stringValue, buttonName.stringValue)) //category does not contain the set name -> ask de dev is it should be added
                        {
                            if(QUI.DisplayDialog("Action Required",
                                                    "The name '" + buttonName.stringValue + "' was not found in the '" + buttonCategory.stringValue + "' category." +
                                                      "\n\n" +
                                                      "Do you want to add it to the database?",
                                                    "Yes",
                                                    "No")) //ask the dev if he wants to add this name to the database
                            {
                                DatabaseUIButtons.GetCategory(buttonCategory.stringValue).AddItemName(buttonName.stringValue, true); //add the item name to the database and save
                                buttonNameIndex = DatabaseUIButtons.ItemNameIndex(buttonCategory.stringValue, buttonName.stringValue); //set the index
                            }
                            else if(!DatabaseUIButtons.GetCategory(buttonCategory.stringValue).IsEmpty()) //select the first item in the category because it's not empty
                            {
                                buttonNameIndex = 0; //set the index
                                buttonName.stringValue = DatabaseUIButtons.GetCategory(buttonCategory.stringValue).itemNames[buttonNameIndex]; //get the name
                            }
                            else //reset category and name
                            {
                                QUI.DisplayDialog("Info",
                                                  "Button category has been reset to the default '" + DUI.UNCATEGORIZED_CATEGORY_NAME + "' value." +
                                                    "\n\n" +
                                                    "Button name has been reset to the default '" + DUI.DEFAULT_BUTTON_NAME + "' value.",
                                                  "Ok"); //inform the dev that becuase he did not add the name to the database, it has been reset to its default value
                                buttonCategory.stringValue = DUI.UNCATEGORIZED_CATEGORY_NAME; //reset the category
                                buttonCategoryIndex = DatabaseUIButtons.CategoryNameIndex(buttonCategory.stringValue); //set the index
                                buttonName.stringValue = DUI.DEFAULT_BUTTON_NAME; //reset the name
                                buttonNameIndex = DatabaseUIButtons.ItemNameIndex(buttonCategory.stringValue, buttonName.stringValue); //set the index
                            }
                        }
                        else //category contains the set name -> get its index
                        {
                            buttonNameIndex = DatabaseUIButtons.ItemNameIndex(buttonCategory.stringValue, buttonName.stringValue); //set the index
                        }
                        QUI.BeginChangeCheck();
                        buttonNameIndex = EditorGUILayout.Popup(buttonNameIndex, DatabaseUIButtons.GetCategory(buttonCategory.stringValue).itemNames.ToArray(), GUILayout.Width(width - 5));
                        if(QUI.EndChangeCheck())
                        {
                            buttonName.stringValue = DatabaseUIButtons.GetCategory(buttonCategory.stringValue).itemNames[buttonNameIndex];
                        }
                    }
                }

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
            QUI.Space(SPACE_4);
        }

        void DrawEvents(float width)
        {
            if(!triggerOnGameEvent.boolValue && !triggerOnButtonClick.boolValue && !triggerOnButtonDoubleClick.boolValue && !triggerOnButtonLongClick.boolValue) { return; }
            if(triggerOnGameEvent.boolValue && (!dispatchAll.boolValue && string.IsNullOrEmpty(gameEvent.stringValue))) { return; }
            if((triggerOnButtonClick.boolValue || triggerOnButtonDoubleClick.boolValue || triggerOnButtonLongClick.boolValue) && (!dispatchAll.boolValue && string.IsNullOrEmpty(buttonName.stringValue))) { return; }

            QUI.SetGUIBackgroundColor(AccentColorBlue);
            QUI.PropertyField(onTriggerEvent, new GUIContent("On Trigger Event"), width);
            QUI.ResetColors();

            QUI.Space(SPACE_4);

            QUI.DrawCollapsableList("Game Events", showGameEventsAnimBool, gameEvents.arraySize > 0 ? QColors.Color.Blue : QColors.Color.Gray, gameEvents, width, 18, "Not sending any Game Events on trigger... Click [+] to start...");
        }
    }
}
