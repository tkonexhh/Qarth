// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace DoozyUI
{
    public class NotificationWindow : QWindow
    {
        public static NotificationWindow Instance;
        private static bool _utility = true;
        private static string _title = "DoozyUI - Notification";
        private static bool _focus = true;

        private static float _minWidth = 600;
        private static float _minHeight = 300;

        private string nTitle = "";
        private string nMessage = "";
        private enum NotificationType { Ok, Cancel, Continue, OkCancel, YesNo }
        private NotificationType nType = NotificationType.Ok;
        private UnityAction buttonOneCallback = null;
        private UnityAction buttonTwoCallback = null;

        static void Init()
        {
            Instance = GetWindow<NotificationWindow>(_utility, _title, _focus);
        }

        public static void Ok(string title, string message)
        {
            Init();
            Instance.SetupWindow(NotificationType.Ok, title, message);
        }

        public static void Cancel(string title, string message, UnityAction CancelCallback)
        {
            Init();
            Instance.SetupWindow(NotificationType.Cancel, title, message, CancelCallback);
        }

        public static void Continue(string title, string message, UnityAction ContinueCallback)
        {
            Init();
            Instance.SetupWindow(NotificationType.Continue, title, message, ContinueCallback);
        }

        public static void OkCancel(string title, string message, UnityAction OkCallback, UnityAction CancelCallback)
        {
            Init();
            Instance.SetupWindow(NotificationType.OkCancel, title, message, OkCallback, CancelCallback);
        }

        public static void YesNo(string title, string message, UnityAction YesCallback, UnityAction NoCallback)
        {
            Init();
            Instance.SetupWindow(NotificationType.YesNo, title, message, YesCallback, NoCallback);
        }

        private void SetupWindow(NotificationType notificationType, string title, string message)
        {
            SetupWindow(notificationType, title, message, null, null);
        }

        private void SetupWindow(NotificationType notificationType, string title, string message, UnityAction ButtonOneCallback)
        {
            SetupWindow(notificationType, title, message, ButtonOneCallback, null);
        }

        private void SetupWindow(NotificationType notificationType, string title, string message, UnityAction ButtonOneCallback, UnityAction ButtonTwoCallback)
        {
            titleContent = new GUIContent(_title);
            minSize = new Vector2(_minWidth, _minHeight);
            maxSize = minSize;
            CenterWindow();
            ResetData();
            nTitle = title;
            nMessage = message;
            nType = notificationType;
            buttonOneCallback = ButtonOneCallback;
            buttonTwoCallback = ButtonTwoCallback;
        }

        private void ResetData()
        {
            nTitle = "";
            nMessage = "";
            nType = NotificationType.Ok;
            buttonOneCallback = null;
            buttonTwoCallback = null;
        }

        private void OnGUI()
        {
            //if(EditorApplication.isCompiling) { CloseWindow(); }
            DrawBackground();
            QUI.Space(58);
            DrawTitle();
            QUI.Space(12);
            DrawMessage();
            QUI.Space(12);
            DrawButtons();
            Repaint();
        }

        void DrawBackground()
        {
            QUI.DrawTexture(DUIResources.notificationWindowBackground.texture, position.width, position.height);
            QUI.Space(-position.height);
        }

        void DrawTitle()
        {
            QUI.BeginHorizontal(position.width);
            {
                QUI.Space(110);
                QUI.Label(nTitle, DUIStyles.GetStyle(DUIStyles.NotificationWindow.NotificationTitleBlack), 480, 40);
                QUI.Space(10);
            }
            QUI.EndHorizontal();
        }

        void DrawMessage()
        {
            QUI.BeginHorizontal(position.width);
            {
                QUI.Space(10);
                QUI.Label(nMessage, DUIStyles.GetStyle(DUIStyles.NotificationWindow.NotificationMessageBlack), 580, 104);
                QUI.Space(10);
            }
            QUI.EndHorizontal();
        }

        void DrawButtons()
        {
            QUI.BeginHorizontal(position.width);
            {
                QUI.FlexibleSpace();
                switch (nType)
                {
                    case NotificationType.Ok:
                        if (QUI.Button(DUIStyles.GetStyle(DUIStyles.NotificationWindow.NotificationButtonOk), 120, 40))
                        {
                            CloseWindow();
                        }
                        break;
                    case NotificationType.Cancel:
                        if (QUI.Button(DUIStyles.GetStyle(DUIStyles.NotificationWindow.NotificationButtonCancel), 120, 40))
                        {
                            if (buttonOneCallback != null) { buttonOneCallback.Invoke(); }
                            CloseWindow();
                        }
                        break;
                    case NotificationType.Continue:
                        if (QUI.Button(DUIStyles.GetStyle(DUIStyles.NotificationWindow.NotificationButtonContinue), 120, 40))
                        {
                            if (buttonOneCallback != null) { buttonOneCallback.Invoke(); }
                            CloseWindow();
                        }
                        break;
                    case NotificationType.OkCancel:
                        if (QUI.Button(DUIStyles.GetStyle(DUIStyles.NotificationWindow.NotificationButtonOk), 120, 40))
                        {
                            if (buttonOneCallback != null) { buttonOneCallback.Invoke(); }
                            CloseWindow();
                        }
                        QUI.Space(5);
                        if (QUI.Button(DUIStyles.GetStyle(DUIStyles.NotificationWindow.NotificationButtonCancel), 120, 40))
                        {
                            if (buttonTwoCallback != null) { buttonTwoCallback.Invoke(); }
                            CloseWindow();
                        }
                        break;
                    case NotificationType.YesNo:
                        if (QUI.Button(DUIStyles.GetStyle(DUIStyles.NotificationWindow.NotificationButtonYes), 120, 40))
                        {
                            if (buttonOneCallback != null) { buttonOneCallback.Invoke(); }
                            CloseWindow();
                        }
                        QUI.Space(5);
                        if (QUI.Button(DUIStyles.GetStyle(DUIStyles.NotificationWindow.NotificationButtonNo), 120, 40))
                        {
                            if (buttonTwoCallback != null) { buttonTwoCallback.Invoke(); }
                            CloseWindow();
                        }
                        break;
                }
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }

        void CloseWindow()
        {
            ResetData();
            if (Instance == null)
            {
                Close();
            }
            else
            {
                Instance.Close();
            }
        }
    }
}
