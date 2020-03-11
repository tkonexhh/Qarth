// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace DoozyUI
{
    [CustomEditor(typeof(UINotificationManager))]
    [DisallowMultipleComponent]
    public class UINotificationManagerEditor : QEditor
    {
        UINotificationManager uiNotificationManager { get { return (UINotificationManager)target; } }

        SerializedProperty
            NotificationItems;

        AnimBool showUINotifications;

        float GlobalWidth { get { return DUI.GLOBAL_EDITOR_WIDTH; } }
        int BarHeight { get { return DUI.BAR_HEIGHT; } }
        int MiniBarHeight { get { return DUI.MINI_BAR_HEIGHT; } }

        float tempFloat;

        protected override void SerializedObjectFindProperties()
        {
            base.SerializedObjectFindProperties();

            NotificationItems = serializedObject.FindProperty("NotificationItems");
        }

        protected override void InitAnimBools()
        {
            base.InitAnimBools();

            showUINotifications = new AnimBool(NotificationItems.arraySize == 0, Repaint);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            requiresContantRepaint = true;
        }

        public override void OnInspectorGUI()
        {
            DrawHeader(DUIResources.headerUINotificationManager.texture, WIDTH_420, HEIGHT_42);
            serializedObject.Update();

            DrawNotificationItems(GlobalWidth);

            serializedObject.ApplyModifiedProperties();

            QUI.Space(SPACE_4);
        }

        void DrawNotificationItems(float width)
        {
            tempFloat = (20 + 2 + 18 * (uiNotificationManager.NotificationItems.Count + 1) + 2) * showUINotifications.faded; //background height
            if(showUINotifications.faded > 0.1f)
            {
                QUI.BeginHorizontal(width);
                {
                    QUI.Space(4 * showUINotifications.faded);
                    QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, uiNotificationManager.NotificationItems.Count > 0 ? QColors.Color.Purple : QColors.Color.Gray), width - 4, tempFloat);
                }
                QUI.EndHorizontal();
                QUI.Space(-tempFloat);
            }

            if(QUI.SlicedBar("UINotifications", uiNotificationManager.NotificationItems.Count > 0 ? QColors.Color.Purple : QColors.Color.Gray, showUINotifications, width, BarHeight))
            {
                showUINotifications.target = !showUINotifications.target;
            }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(8 * showUINotifications.faded);
                if(QUI.BeginFadeGroup(showUINotifications.faded))
                {
                    QUI.BeginVertical(width - 8);
                    {

                        QUI.Space(2);

                        if(uiNotificationManager.NotificationItems.Count == 0)
                        {
                            QUI.BeginHorizontal(width - 8);
                            {
                                QLabel.text = "No UINotifications referenced... Click [+] to start...";
                                QLabel.style = Style.Text.Help;
                                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                                {
                                    QUI.Label(QLabel);
                                    QUI.Space(2);
                                }
                                QUI.EndVertical();

                                QUI.FlexibleSpace();

                                QUI.BeginVertical(16, QUI.SingleLineHeight);
                                {
                                    if(QUI.ButtonPlus())
                                    {
                                        NotificationItems.InsertArrayElementAtIndex(NotificationItems.arraySize);
                                    }
                                    QUI.Space(1);
                                }
                                QUI.EndVertical();

                                QUI.Space(4);
                            }
                            QUI.EndHorizontal();
                        }
                        else
                        {
                            QUI.BeginVertical(width - 8);
                            {
                                for(int i = 0; i < uiNotificationManager.NotificationItems.Count; i++)
                                {
                                    QUI.BeginHorizontal(width - 8, QUI.SingleLineHeight);
                                    {
                                        if(NotificationItems.GetArrayElementAtIndex(i).FindPropertyRelative("notificationPrefab").objectReferenceValue != null)
                                        {
                                            NotificationItems.GetArrayElementAtIndex(i).FindPropertyRelative("notificationName").stringValue = NotificationItems.GetArrayElementAtIndex(i).FindPropertyRelative("notificationPrefab").objectReferenceValue.name;
                                        }
                                        else
                                        {
                                            NotificationItems.GetArrayElementAtIndex(i).FindPropertyRelative("notificationName").stringValue = "Missing Reference";
                                        }

                                        QLabel.text = i + " - " + NotificationItems.GetArrayElementAtIndex(i).FindPropertyRelative("notificationName").stringValue;
                                        QLabel.style = Style.Text.Normal;

                                        QUI.Label(QLabel.text, Style.Text.Normal, 200);

                                        QUI.Space(2);

                                        QUI.PropertyField(NotificationItems.GetArrayElementAtIndex(i).FindPropertyRelative("notificationPrefab"), true, width - 200 - 2 - 16 - 12 - 8);

                                        if(QUI.ButtonMinus())
                                        {
                                            Undo.RecordObject(target, "ReferncedRemoved");
                                            uiNotificationManager.NotificationItems.RemoveAt(i);
                                            QUI.ExitGUI();
                                        }

                                        QUI.Space(8);
                                    }
                                    QUI.EndHorizontal();
                                }

                                QUI.BeginHorizontal(width - 8);
                                {
                                    QUI.FlexibleSpace();

                                    QUI.BeginVertical(16, QUI.SingleLineHeight);
                                    {
                                        if(QUI.ButtonPlus())
                                        {
                                            NotificationItems.InsertArrayElementAtIndex(NotificationItems.arraySize);
                                        }
                                        QUI.Space(1);
                                    }
                                    QUI.EndVertical();

                                    QUI.Space(4);
                                }
                                QUI.EndHorizontal();
                            }
                            QUI.EndVertical();

                            QUI.Space(2);

                            QUI.Space(4 * showUINotifications.faded);
                        }
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();

            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_8);
        }
    }
}
