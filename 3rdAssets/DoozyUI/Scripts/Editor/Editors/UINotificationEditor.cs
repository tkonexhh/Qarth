// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UI;

namespace DoozyUI
{
    [CustomEditor(typeof(UINotification))]
    [CanEditMultipleObjects]
    public class UINotificationEditor : QEditor
    {
        UINotification uiNotification { get { return (UINotification)target; } }
        List<string> DatabaseUICanvases { get { return DUIData.Instance.DatabaseUICanvases; } }

        SerializedProperty
            listenForBackButton,
            targetCanvasName, customTargetCanvasName,
            notificationContainer,
            overlay,
            title,
            message,
            icon,
            buttons,
            closeButton,
            specialElements,
            effects;

        AnimBool showNotificationButtons,
                 showSpecialElements,
                 showNotificationEffects;

        int canvasNameIndex;

        float GlobalWidth { get { return DUI.GLOBAL_EDITOR_WIDTH; } }
        int BarHeight { get { return DUI.BAR_HEIGHT; } }
        int MiniBarHeight { get { return DUI.MINI_BAR_HEIGHT; } }

        float tempFloat = 0;

        protected override void SerializedObjectFindProperties()
        {
            base.SerializedObjectFindProperties();

            listenForBackButton = serializedObject.FindProperty("listenForBackButton");
            targetCanvasName = serializedObject.FindProperty("targetCanvasName");
            customTargetCanvasName = serializedObject.FindProperty("customTargetCanvasName");
            notificationContainer = serializedObject.FindProperty("notificationContainer");
            overlay = serializedObject.FindProperty("overlay");
            title = serializedObject.FindProperty("title");
            message = serializedObject.FindProperty("message");
            icon = serializedObject.FindProperty("icon");
            buttons = serializedObject.FindProperty("buttons");
            closeButton = serializedObject.FindProperty("closeButton");
            specialElements = serializedObject.FindProperty("specialElements");
            effects = serializedObject.FindProperty("effects");
        }

        protected override void InitAnimBools()
        {
            base.InitAnimBools();

            showNotificationButtons = new AnimBool(buttons.arraySize == 0, Repaint);
            showSpecialElements = new AnimBool(specialElements.arraySize == 0, Repaint);
            showNotificationEffects = new AnimBool(effects.arraySize == 0, Repaint);
        }

        protected override void GenerateInfoMessages()
        {
            base.GenerateInfoMessages();

            infoMessage.Add("EmptyTargetCanvasName",
                            new InfoMessage()
                            {
                                title = "Empty Target Canvas Name",
                                message = "You need to set a target canvas name so that they system will know the canvas you want this notification shown on. " +
                                            "If left empty, the system will look for the '" + UICanvas.MASTER_CANVAS_NAME + "' automatically.",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("MissingNotificationContainer",
                            new InfoMessage()
                            {
                                title = "Missing Notification Container",
                                message = "A notification needs an UIElement to act as a container. " +
                                            "Fix this issue by adding a child UIElement to this gameObject and then reference it as the Notification Container.",
                                type = InfoMessageType.Error,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("BadNotificationContainer",
                            new InfoMessage()
                            {
                                title = "Bad Notification Container",
                                message = "The UIElement referenced as the Notification Container is not a child of this gameObject. " +
                                            "The UINotification may not behave as expected or it may not work at all.",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });
        }



        protected override void OnEnable()
        {
            base.OnEnable();
            requiresContantRepaint = true;

            SyncData();
            SetupAllChildUIElements();
        }

        void SyncData()
        {
            DUIData.Instance.ScanForUICanvases(true);
            DUIData.Instance.ValidateUICanvases();

            ValidateCanvasName();
        }
        void ValidateCanvasName()
        {
            if(customTargetCanvasName.boolValue) //check if customCanvasName is true
            {
                return; //custom customCanvasName is true --> it is no longer needed to continue any further
            }

            if(!DUIData.Instance.DatabaseUICanvases.Contains(targetCanvasName.stringValue)) //canvas name does not exist in canvas datatabase -> ask if it should be added
            {
                if(!string.IsNullOrEmpty(targetCanvasName.stringValue.Trim())
                    && QUI.DisplayDialog("Action Required",
                                         "The '" + targetCanvasName.stringValue + "' canvas name does not exist in the canvas names database." +
                                            "\n\n" +
                                            "Do you want to add it now?",
                                         "Yes",
                                         "No"))
                {
                    DUIData.Instance.AddUICanvas(targetCanvasName.stringValue); //add a new canvas name to the generated database
                    DUIData.Instance.ScanForUICanvases(true); //add a new canvas name to the static database
                    customTargetCanvasName.boolValue = false; //set customCanvasName to false, as this no longer is a custom canvas name
                }
                else if(QUI.DisplayDialog("Action Required",
                                          "Reset canvas name to the default value of '" + UICanvas.MASTER_CANVAS_NAME + "'?",
                                          "Yes",
                                          "No"))
                {
                    targetCanvasName.stringValue = UICanvas.MASTER_CANVAS_NAME;
                    customTargetCanvasName.boolValue = false;
                }
                else
                {
                    QUI.DisplayDialog("Information",
                                      "The canvas name was left unchanged and this UICanvas was set to use a custom canvas name." +
                                        "\n\n" +
                                        "Having a custom canvas name means that the name is not in the Canvas Database.",
                                      "Ok");

                    customTargetCanvasName.boolValue = true; //set customCanvasName as true, as the dev did not want to add the custom canvas name to the database
                    return;
                }
            }
            canvasNameIndex = DUIData.Instance.DatabaseUICanvases.IndexOf(targetCanvasName.stringValue); //find the canvas name in the database and return its index
        }

        public override void OnInspectorGUI()
        {
            DrawHeader(DUIResources.headerUINotification.texture, WIDTH_420, HEIGHT_42);
            serializedObject.Update();

            DrawListenForBackButton(GlobalWidth);
            QUI.Space(SPACE_4);

            DrawTargetCanvas(GlobalWidth);
            QUI.Space(SPACE_4);


            DrawNotificationContainer(GlobalWidth);
            QUI.Space(SPACE_2);


            if(!infoMessage["MissingNotificationContainer"].show.value)
            {
                if(uiNotification.notificationContainer != null)
                {
                    infoMessage["BadNotificationContainer"].show.target = uiNotification.notificationContainer.transform.parent != uiNotification.transform;
                    DrawInfoMessage("BadNotificationContainer", GlobalWidth);
                }

                DrawOverlay(GlobalWidth);
                QUI.Space(SPACE_2);

                DrawTitle(GlobalWidth);
                QUI.Space(SPACE_2);

                DrawMessage(GlobalWidth);
                QUI.Space(SPACE_2);

                DrawIcon(GlobalWidth);
                QUI.Space(SPACE_2);

                DrawButtons(GlobalWidth);
                QUI.Space(SPACE_4);

                DrawSpecialElements(GlobalWidth);
                QUI.Space(SPACE_4);

                DrawEffects(GlobalWidth);
                QUI.Space(SPACE_4);

                DrawCloseButton(GlobalWidth);
            }

            serializedObject.ApplyModifiedProperties();

            QUI.Space(SPACE_4);
        }

        void DrawListenForBackButton(float width)
        {
            QUI.QToggle("Listen for the 'Back' button (close when pressing 'Back' or ESC)", listenForBackButton);
        }

        void DrawTargetCanvas(float width)
        {
            QUI.BeginHorizontal(width);
            {
                tempFloat = width - 72; //background width
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), tempFloat, 20);
                QUI.Space(-tempFloat);
                QUI.Space(SPACE_4);

                QLabel.text = "Canvas Name";
                QLabel.style = Style.Text.Small;
                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Space(1);
                    QUI.Label(QLabel);
                }
                QUI.EndVertical();

                if(EditorApplication.isPlayingOrWillChangePlaymode)
                {
                    GUI.enabled = false;
                }

                tempFloat = width - QLabel.x - 92; //field width

                if(customTargetCanvasName.boolValue)
                {
                    QUI.PropertyField(targetCanvasName, tempFloat);

                    if(Event.current.isKey
                       && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.Escape)
                       && Event.current.type == EventType.KeyUp) //if the Enter or Escape key have been pressed
                    {
                        customTargetCanvasName.boolValue = false;
                    }
                }
                else
                {
                    ValidateCanvasName();
                    QUI.BeginChangeCheck();
                    canvasNameIndex = EditorGUILayout.Popup(canvasNameIndex, DatabaseUICanvases.ToArray(), GUILayout.Width(tempFloat));
                    if(QUI.EndChangeCheck())
                    {
                        Undo.RecordObject(target, "UpdateCanvasName");
                        targetCanvasName.stringValue = DUIData.Instance.DatabaseUICanvases[canvasNameIndex];
                    }
                }
                QUI.Space(SPACE_4);
                QLabel.text = "custom";
                QLabel.style = Style.Text.Normal;
                QUI.BeginChangeCheck();
                QUI.BeginVertical(QLabel.x + 28, 18);
                {
                    QUI.Space(1);
                    QUI.QToggle("custom", customTargetCanvasName);
                }
                QUI.EndVertical();
                if(QUI.EndChangeCheck())
                {
                    if(!customTargetCanvasName.boolValue)
                    {
                        ValidateCanvasName();
                    }
                }

                GUI.enabled = true;
            }
            QUI.EndHorizontal();

            infoMessage["EmptyTargetCanvasName"].show.target = string.IsNullOrEmpty(targetCanvasName.stringValue);
            DrawInfoMessage("EmptyTargetCanvasName", width);
        }

        void DrawNotificationContainer(float width)
        {
            QUI.BeginChangeCheck();
            QUI.QObjectPropertyField("Notification Container", notificationContainer, width);
            if(QUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                if(uiNotification.notificationContainer != null)
                {
                    uiNotification.notificationContainer.transform.SetParent(uiNotification.transform);
                    uiNotification.notificationContainer.linkedToNotification = true;
                    uiNotification.notificationContainer.autoRegister = false;
                    uiNotification.notificationContainer.name = DUI.DUISettings.UIElement_Inspector_RenameGameObjectPrefix + "Notification Container" + DUI.DUISettings.UIElement_Inspector_RenameGameObjectSuffix;
                }
            }

            infoMessage["MissingNotificationContainer"].show.target = notificationContainer.objectReferenceValue == null;
            DrawInfoMessage("MissingNotificationContainer", width);
        }

        void DrawOverlay(float width)
        {
            QUI.BeginChangeCheck();
            QUI.QObjectPropertyField("Background Overlay", overlay, width);
            if(QUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                if(uiNotification.overlay != null)
                {
                    uiNotification.overlay.transform.SetParent(uiNotification.transform);
                    uiNotification.overlay.linkedToNotification = true;
                    uiNotification.overlay.autoRegister = false;
                    uiNotification.overlay.name = DUI.DUISettings.UIElement_Inspector_RenameGameObjectPrefix + "Background Overlay" + DUI.DUISettings.UIElement_Inspector_RenameGameObjectSuffix;
                }
            }
        }

        void DrawTitle(float width)
        {
            QUI.BeginChangeCheck();
            QUI.QObjectPropertyField("Notification Title", title, width);
            if(QUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                if(uiNotification.title != null)
                {
                    uiNotification.title.name = "Notification Title";
                }
            }
        }

        void DrawMessage(float width)
        {
            QUI.BeginChangeCheck();
            QUI.QObjectPropertyField("Notification Message", message, width);
            if(QUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                if(uiNotification.message != null)
                {
                    uiNotification.message.name = "Notification Message";
                }
            }
        }

        void DrawIcon(float width)
        {
            QUI.BeginChangeCheck();
            QUI.QObjectPropertyField("Notification Icon", icon, width);
            if(QUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                if(uiNotification.icon != null)
                {
                    uiNotification.icon.name = "Notification Icon";
                }
            }
        }

        void DrawButtons(float width)
        {
            QUI.BeginChangeCheck();
            QUI.QObjectList("Notification Buttons", buttons, "No UIButtons referenced...", showNotificationButtons, width, BarHeight);
            if(QUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                if(uiNotification.buttons != null && uiNotification.buttons.Length > 0)
                {
                    for(int i = 0; i < uiNotification.buttons.Length; i++)
                    {
                        if(uiNotification.buttons[i] != null)
                        {
                            uiNotification.buttons[i].name = "Notification Button " + i;
                        }
                    }
                }
            }
        }

        void DrawSpecialElements(float width)
        {
            QUI.BeginChangeCheck();
            QUI.QObjectList("Special Elements", specialElements, "No UIElements referenced...", showSpecialElements, width, BarHeight);
            if(QUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                if(uiNotification.specialElements != null && uiNotification.specialElements.Length > 0)
                {

                    for(int i = 0; i < uiNotification.specialElements.Length; i++)
                    {
                        if(uiNotification.specialElements[i] != null)
                        {
                            uiNotification.specialElements[i].transform.SetParent(uiNotification.transform);
                            uiNotification.specialElements[i].name = DUI.DUISettings.UIElement_Inspector_RenameGameObjectPrefix + "Special Element " + i + DUI.DUISettings.UIElement_Inspector_RenameGameObjectSuffix;
                        }
                    }
                }
            }
        }

        void DrawEffects(float width)
        {
            QUI.BeginChangeCheck();
            QUI.QObjectList("Notification Effects", effects, "No UIEffects referenced...", showNotificationEffects, width, BarHeight);
            if(QUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                if(uiNotification.effects != null && uiNotification.effects.Length > 0)
                {

                    for(int i = 0; i < uiNotification.effects.Length; i++)
                    {
                        if(uiNotification.effects[i] != null)
                        {
                            uiNotification.effects[i].targetUIElement = uiNotification.notificationContainer;
                            uiNotification.effects[i].name = "Notification Effect " + i;
                        }
                    }
                }
            }
        }

        void DrawCloseButton(float width)
        {
            QUI.QObjectPropertyField("Close Button", closeButton, width);
        }

        void SetupAllChildUIElements()
        {
            uiNotification.Initialize();
        }
    }
}
