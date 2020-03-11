// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace DoozyUI
{
    [CustomEditor(typeof(UICanvas))]
    [CanEditMultipleObjects]
    public class UICanvasEditor : QEditor
    {
        UICanvas uiCanvas { get { return (UICanvas)target; } }
        List<string> DatabaseUICanvases { get { return DUIData.Instance.DatabaseUICanvases; } }

        private const float BUTTON_HEIGHT = 20;

        SerializedProperty
            canvasName, customCanvasName,
            dontDestroyOnLoad;

        int canvasNameIndex;

        float GlobalWidth { get { return DUI.GLOBAL_EDITOR_WIDTH; } }

        float tempFloat = 0;

        protected override void SerializedObjectFindProperties()
        {
            base.SerializedObjectFindProperties();

            canvasName = serializedObject.FindProperty("canvasName");
            customCanvasName = serializedObject.FindProperty("customCanvasName");
            dontDestroyOnLoad = serializedObject.FindProperty("dontDestroyOnLoad");
        }

        protected override void GenerateInfoMessages()
        {
            base.GenerateInfoMessages();

            infoMessage.Add("NotRootCanvas",
                            new InfoMessage()
                            {
                                title = "Disabled",
                                message = "The UICanvas is not attached to a RootCanvas. " +
                                            "This component should be attached to a top canvas in the Hierarchy.",
                                type = InfoMessageType.Error,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("MasterCanvasInfo",
                            new InfoMessage()
                            {
                                title = UICanvas.MASTER_CANVAS_NAME,
                                message = "This UICanvas is your main/default canvas." +
                                            "\n\n" +
                                            "You should NOT have, in a scene, more than one '" + UICanvas.MASTER_CANVAS_NAME + "' at any given time.",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            requiresContantRepaint = true;

            SyncData();
        }

        void SyncData()
        {
            DUIData.Instance.ScanForUICanvases(true);
            DUIData.Instance.ValidateUICanvases();

            ValidateCanvasName();
        }
        void ValidateCanvasName()
        {
            if(customCanvasName.boolValue) //check if customCanvasName is true
            {
                return; //custom customCanvasName is true --> it is no longer needed to continue any further
            }

            if(!DUIData.Instance.DatabaseUICanvases.Contains(canvasName.stringValue)) //canvas name does not exist in canvas datatabase -> ask if it should be added
            {
                if(!string.IsNullOrEmpty(canvasName.stringValue.Trim())
                    && QUI.DisplayDialog("Action Required",
                                         "The '" + canvasName.stringValue + "' canvas name does not exist in the canvas names database." +
                                            "\n\n" +
                                            "Do you want to add it now?",
                                         "Yes",
                                         "No"))
                {
                    DUIData.Instance.AddUICanvas(canvasName.stringValue); //add a new canvas name to the generated database
                    DUIData.Instance.ScanForUICanvases(true); //add a new canvas name to the static database
                    customCanvasName.boolValue = false; //set customCanvasName to false, as this no longer is a custom canvas name
                }
                else if(QUI.DisplayDialog("Action Required",
                                          "Reset canvas name to the default value of '" + UICanvas.MASTER_CANVAS_NAME + "'?",
                                          "Yes",
                                          "No"))
                {
                    canvasName.stringValue = UICanvas.MASTER_CANVAS_NAME;
                    customCanvasName.boolValue = false;
                }
                else
                {
                    QUI.DisplayDialog("Information",
                                      "The canvas name was left unchanged and this UICanvas was set to use a custom canvas name." +
                                        "\n\n" +
                                        "Having a custom canvas name means that the name is not in the Canvas Database.",
                                      "Ok");

                    customCanvasName.boolValue = true; //set customCanvasName as true, as the dev did not want to add the custom canvas name to the database
                    return;
                }
            }
            canvasNameIndex = DUIData.Instance.DatabaseUICanvases.IndexOf(canvasName.stringValue); //find the canvas name in the database and return its index
        }

        public override void OnInspectorGUI()
        {
            DrawHeader(DUIResources.headerUICanvas.texture, WIDTH_420, HEIGHT_42);

            if(!infoMessage["NotRootCanvas"].show.value)
            {
                serializedObject.Update();

                DrawDatabaseButtons(GlobalWidth);
                QUI.Space(SPACE_4);

                DrawCanvasName(GlobalWidth);
                QUI.Space(SPACE_2 * (1 - infoMessage["MasterCanvasInfo"].show.faded));

                infoMessage["MasterCanvasInfo"].show.target = canvasName.stringValue.Equals(UICanvas.MASTER_CANVAS_NAME);
                DrawInfoMessage("MasterCanvasInfo", GlobalWidth);

                QUI.Space(SPACE_4 * infoMessage["MasterCanvasInfo"].show.faded);


                DrawDontDestroyOnLoad(GlobalWidth);
                QUI.Space(SPACE_4);

                DrawUpdateSortingLayerButton(GlobalWidth);

                serializedObject.ApplyModifiedProperties();
            }
            else
            {
                DrawRemoveComponentButton(GlobalWidth);
                QUI.Space(SPACE_4);
            }

            infoMessage["NotRootCanvas"].show.target = !uiCanvas.Canvas.isRootCanvas;
            DrawInfoMessage("NotRootCanvas", GlobalWidth);

            QUI.Space(SPACE_4);
        }

        void DrawDatabaseButtons(float width)
        {
            QUI.BeginHorizontal(width);
            {
                if(QUI.GhostButton("UICanvases Database", QColors.Color.Gray, width, 18))
                {
                    ControlPanelWindow.OpenWindow(ControlPanelWindow.Page.UICanvases);
                }
            }
            QUI.EndHorizontal();
        }

        void DrawCanvasName(float width)
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

                if(customCanvasName.boolValue)
                {
                    QUI.PropertyField(canvasName, tempFloat);

                    if(Event.current.isKey
                       && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.Escape)
                       && Event.current.type == EventType.KeyUp) //if the Enter or Escape key have been pressed
                    {
                        customCanvasName.boolValue = false;
                    }
                }
                else
                {
                    ValidateCanvasName();
                    QUI.BeginChangeCheck();
                    canvasNameIndex = EditorGUILayout.Popup(canvasNameIndex, DatabaseUICanvases.ToArray(), GUILayout.Width(tempFloat));
                    if(QUI.EndChangeCheck())
                    {
                        Undo.RecordObject(uiCanvas, "UpdateCanvasName");
                        canvasName.stringValue = DUIData.Instance.DatabaseUICanvases[canvasNameIndex];
                    }
                }
                QUI.Space(SPACE_4);
                QLabel.text = "custom";
                QLabel.style = Style.Text.Normal;
                QUI.BeginChangeCheck();
                QUI.BeginVertical(QLabel.x + 28, 18);
                {
                    QUI.Space(1);
                    QUI.QToggle("custom", customCanvasName);
                }
                QUI.EndVertical();
                if(QUI.EndChangeCheck())
                {
                    if(!customCanvasName.boolValue)
                    {
                        ValidateCanvasName();
                    }
                }

                GUI.enabled = true;
            }
            QUI.EndHorizontal();
        }

        void DrawDontDestroyOnLoad(float width)
        {
            QUI.QToggle("Don't Destroy On Load", dontDestroyOnLoad);
        }

        void DrawUpdateSortingLayerButton(float width)
        {
            if(QUI.GhostButton("Update Sorting", QColors.Color.Gray, width, BUTTON_HEIGHT))
            {
                if(!uiCanvas.Canvas.isRootCanvas)
                {
                    EditorUtility.DisplayDialog("UICanvas Issue",
                                                "The UICanvas, on the " + uiCanvas.name + " gameObject, is not attached to a root canvas. This component should be attached to a top canvas in the Hierarchy.",
                                                "Ok");
                    return;
                }
                if(EditorUtility.DisplayDialog("Update Sorting",
                                                "You are about to change the Sorting Layer Name of all the Canvases and Renderers, under this gameObject, to '" + uiCanvas.Canvas.sortingLayerName + "'." +
                                                "\n" + "\n" +
                                                "'" + uiCanvas.Canvas.sortingLayerName + "' is the Sorting Layer set to the Canvas component attached to this gameObject. (root canvas)" +
                                                "\n" + "\n" +
                                                "Are you sure you want to do that?" +
                                                "\n" +
                                                "(operation cannot be undone)",
                                                "Ok",
                                                "Cancel"))
                {
                    UIManager.UpdateCanvasSortingLayerName(uiCanvas.gameObject, uiCanvas.Canvas.sortingLayerName);
                    UIManager.UpdateRendererSortingLayerName(uiCanvas.gameObject, uiCanvas.Canvas.sortingLayerName);
                }
            }
            QUI.Space(SPACE_4);
        }

        void DrawRemoveComponentButton(float width)
        {
            if(QUI.GhostButton("Remove Component", QColors.Color.Red, width, BUTTON_HEIGHT))
            {
                DestroyImmediate(uiCanvas);
                QUI.ExitGUI();
            }

        }
    }
}
