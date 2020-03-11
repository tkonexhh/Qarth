// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DoozyUI
{
    [DisallowMultipleComponent]
    public class UINotificationManager : MonoBehaviour
    {
        /// <summary>
        /// Helper class for the NotificationManager.
        /// </summary>
        [Serializable]
        public class NotificationItem
        {
            /// <summary>
            /// Notification Name
            /// </summary>
            public string notificationName;
            /// <summary>
            /// Notification Prefab
            /// </summary>
            public UINotification notificationPrefab;
        }

        /// <summary>
        /// List of notification items that have been set up in the Inspector.
        /// </summary>
        public List<NotificationItem> NotificationItems = new List<NotificationItem>();
        /// <summary>
        /// Internal list used as a Notification Queue.
        /// </summary>
        private List<UINotification.NotificationData> m_notificationQueue;
        /// <summary>
        /// The Notification Queue list.
        /// </summary>
        public List<UINotification.NotificationData> NotificationQueue { get { if (m_notificationQueue == null) { m_notificationQueue = new List<UINotification.NotificationData>(); } return m_notificationQueue; } }

        private int count;

        public UINotification GetUINotification(string notificationName)
        {
            count = NotificationItems != null ? NotificationItems.Count : 0;
            if (count == 0) { return null; }
            for (int i = 0; i < count; i++)
            {
                if (NotificationItems[i].notificationName.Equals(notificationName)) { return NotificationItems[i].notificationPrefab; }
            }
            return null;
        }

        /// <summary>
        /// Every notification that needs to enter the Notification Queue will be added to the notificatioQueue list as the last item.
        /// </summary>
        public void RegisterToNotificationQueue(UINotification.NotificationData nData)
        {
            if (nData == null) { return; }
            NotificationQueue.Add(nData);
            if (NotificationQueue.Count == 1) { ShowNextNotificationInQueue(); } //this is the last (and only) notification data in the queue -> show the notification now
        }
        /// <summary>
        /// Unregisteres a notification, by removing the notification data that started it.
        /// </summary>
        public void UnregisterFromNotificationQueue(UINotification.NotificationData nData)
        {
            NotificationQueue.Remove(nData);
            if (NotificationQueue.Count > 0) { LoadNotification(NotificationQueue[0]); } //always show the first item in the list because it is the oldest
            else { NotificationQueue.Clear(); }
        }
        /// <summary>
        /// Shows the next notification in the Notification Queue, if there is one.
        /// </summary>
        private void ShowNextNotificationInQueue()
        {
            if (NotificationQueue.Count > 0)  //if the Notification Queue is not null and it has at least 1 notification data in it -> show it
            {
                LoadNotification(NotificationQueue[0]); //always show the first item in the list because it is the oldest
            }
        }

        /// <summary>
        /// Sets up a Notification.
        /// </summary>
        private UINotification SetupNotification(UINotification.NotificationData nData)
        {
            if (string.IsNullOrEmpty(nData.prefabName) && nData.prefab == null)
            {
                Debug.Log("[DoozyUI] [SetupNotification]: The nPrefabName is null or empty and the nPrefab is null as well. Something went wrong.");
                return null;
            }

            if (nData.addToNotificationQueue)
            {
                RegisterToNotificationQueue(nData); //register the notification to the NotificationQueue and let the system it handle it
                return null;
            }

            return LoadNotification(nData); //show the notification without adding it to the queue
        }
        /// <summary>
        /// Loads the notification by instatiating the prefab and doing the initial setup to it
        /// </summary>
        private UINotification LoadNotification(UINotification.NotificationData nData)
        {
            GameObject notification = null;
            UINotification componentReference = null;
            string targetCanvasName;
            UICanvas targetCanvas;

            if (nData.prefab != null) //we have a prefab reference
            {
                componentReference = nData.prefab.GetComponent<UINotification>();
                if (componentReference == null) //make sure the notification gameobject has an UINotification component attached (this is a fail safe in case the developer links the wrong prefab)
                {
                    Debug.Log("[DoozyUI] [SetupNotification] [Error]: The notification prefab named " + notification.name + " does not have an UINotification component attached. Check if this prefab is really a notification or not.");
                    return null;
                }
                targetCanvasName = (string.IsNullOrEmpty(componentReference.targetCanvasName) || componentReference.targetCanvasName.Equals(UICanvas.MASTER_CANVAS_NAME)) //make sure the target canvas name is not an empty string
                                     ? DUI.MASTER_CANVAS_NAME
                                     : componentReference.targetCanvasName;
                targetCanvas = UIManager.GetCanvas(targetCanvasName);
                notification = Instantiate(nData.prefab, targetCanvas.transform.position, Quaternion.identity);
            }
            else if (!string.IsNullOrEmpty(nData.prefabName)) //we don't have a prefab reference and we check if we have a prefabName we should be looking for in Resources
            {
                if (GetUINotification(nData.prefabName) != null) //look for the notification in the UI Notification manager
                {
                    componentReference = GetUINotification(nData.prefabName);
                    targetCanvasName = (string.IsNullOrEmpty(componentReference.targetCanvasName) || componentReference.targetCanvasName.Equals(UICanvas.MASTER_CANVAS_NAME)) //make sure the target canvas name is not an empty string
                                       ? DUI.MASTER_CANVAS_NAME
                                       : componentReference.targetCanvasName;
                    targetCanvas = UIManager.GetCanvas(targetCanvasName);
                    notification = Instantiate(GetUINotification(nData.prefabName).gameObject, targetCanvas.transform.position, Quaternion.identity);
                }
                else // the notification does not exist in the UINotification --> look for it in the resources
                {
                    GameObject notificationPrefab = null;
                    //look for the notification prefab; do this in a 'try catch' just in case the name was mispelled or the prefab does not exist
                    try { notificationPrefab = Resources.Load(nData.prefabName) as GameObject; }
                    catch (UnityException e) { Debug.Log("[DoozyUI] [SetupNotification] [Error]: " + e); }

                    if (notificationPrefab == null)
                    {
                        Debug.Log("[DoozyUI] [SetupNotification]: The notification named [" + nData.prefabName + "] prefab does not exist or is not located under a Resources folder");
                        return null;
                    }
                    componentReference = notificationPrefab.GetComponent<UINotification>();
                    if (componentReference == null) //make sure the notification gameobject has an UINotification component attached (this is a fail safe in case the developer links the wrong prefab)
                    {
                        Debug.Log("[DoozyUI] [SetupNotification] [Error]: The notification prefab named " + notification.name + " does not have an UINotification component attached. Check if this prefab is really a notification or not.");
                        return null;
                    }
                    targetCanvasName = (string.IsNullOrEmpty(componentReference.targetCanvasName) || componentReference.targetCanvasName.Equals(UICanvas.MASTER_CANVAS_NAME)) //make sure the target canvas name is not an empty string
                                      ? DUI.MASTER_CANVAS_NAME
                                      : componentReference.targetCanvasName;
                    targetCanvas = UIManager.GetCanvas(targetCanvasName);
                    notification = Instantiate(notificationPrefab, targetCanvas.transform.position, Quaternion.identity);
                }
            }
            else //the developer didn't link a prefab, nor did he set a prefabName; this is a fail safe option
            {
                Debug.Log("[DoozyUI] [SetupNotification] [Error]: You are trying to show a notification, but you didn't set neither a prefab reference, nor a prefabName. This is a fail safe debug log. Check your ShowNotification method call and fix this.");
                return null;
            }

            if (notification.GetComponent<UINotification>() == null) //make sure the notification gameobject has an UINotification component attached (this is a fail safe in case the developer links the wrong prefab)
            {
                Debug.Log("[DoozyUI] [SetupNotification] [Error]: The notification prefab named " + notification.name + " does not have an UINotification component attached. Check if this prefab is really a notification or not.");
                return null;
            }

            notification.transform.SetParent(targetCanvas.transform, false);
            notification.gameObject.layer = notification.transform.parent.gameObject.layer; //set the physics layer (just in case the camera is set to see another layer)
            UIManager.UpdateCanvasSortingLayerName(notification.gameObject, targetCanvas.Canvas.sortingLayerName); //update the sorting layers for all the canvases (just in case)
            UIManager.UpdateRendererSortingLayerName(notification.gameObject, targetCanvas.Canvas.sortingLayerName); //update the sorting layers for all the rendereres (just in case)
            RectTransform rt = notification.GetComponent<RectTransform>();
            rt.anchoredPosition = targetCanvas.RectTransform.anchoredPosition;
            if (targetCanvas.Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
            }
            notification.GetComponent<UINotification>().ShowNotification(nData, targetCanvas);
            return notification.GetComponent<UINotification>();
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="title">The text you want to show in the title area (if linked)</param>
        /// <param name="message">The message you want to show in the message area (if linked)</param>
        /// <param name="icon">The sprite you want the notification icon to have (if linked)</param>
        /// <param name="buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        /// <param name="buttonTexts">The text on the buttons (example: 'OK', 'Cancel', 'Yes', 'No' and so on)</param>
        public UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, string _message, Sprite _icon, string[] _buttonNames, string[] _buttonTexts, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            if (UIManager.Instance.debugUINotifications)
            {
                if (_lifetime == -1)
                {
                    Debug.Log("[DoozyUI] Showing notification " + _prefabName);
                }
                else
                {
                    Debug.Log("[DoozyUI] Showing notification " + _prefabName + " for " + _lifetime + " seconds");
                }
            }

            UINotification.NotificationData nData =
                new UINotification.NotificationData()
                {
                    prefabName = _prefabName,
                    lifetime = _lifetime,
                    addToNotificationQueue = _addToNotificationQueue,
                    title = _title,
                    message = _message,
                    icon = _icon,
                    buttonNames = _buttonNames,
                    buttonTexts = _buttonTexts,
                    buttonCallback = _buttonCallback,
                    hideCallback = _hideCallback
                };

            return SetupNotification(nData);
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        /// <param name="_buttonTexts">The text on the buttons (example: 'OK', 'Cancel', 'Yes', 'No' and so on)</param>
        public UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, string _message, Sprite _icon, string[] _buttonNames, string[] _buttonTexts, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            if (UIManager.Instance.debugUINotifications)
            {
                if (_lifetime == -1)
                {
                    Debug.Log("[DoozyUI] Showing notification " + _prefab.name);
                }
                else
                {
                    Debug.Log("[DoozyUI] Showing notification " + _prefab.name + " for " + _lifetime + " seconds");
                }
            }

            UINotification.NotificationData nData =
                new UINotification.NotificationData()
                {
                    prefab = _prefab,
                    lifetime = _lifetime,
                    addToNotificationQueue = _addToNotificationQueue,
                    title = _title,
                    message = _message,
                    icon = _icon,
                    buttonNames = _buttonNames,
                    buttonTexts = _buttonTexts,
                    buttonCallback = _buttonCallback,
                    hideCallback = _hideCallback
                };

            return SetupNotification(nData);
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        public UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
                ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                UINotification.DEFAULT_TITLE,
                UINotification.DEFAULT_MESSAGE,
                UINotification.DEFAULT_ICON,
                UINotification.DEFAULT_BUTTON_NAMES,
                UINotification.DEFAULT_BUTTON_TEXT,
                _buttonCallback,
                _hideCallback
                );
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        public UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, UnityAction _hideCallback = null)
        {
            return
                ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                UINotification.DEFAULT_TITLE,
                UINotification.DEFAULT_MESSAGE,
                UINotification.DEFAULT_ICON,
                UINotification.DEFAULT_BUTTON_NAMES,
                UINotification.DEFAULT_BUTTON_TEXT,
                null,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        public UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                _title,
                UINotification.DEFAULT_MESSAGE,
                UINotification.DEFAULT_ICON,
                UINotification.DEFAULT_BUTTON_NAMES,
                UINotification.DEFAULT_BUTTON_TEXT,
                null,
                _hideCallback
                );
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        public UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                _title,
                UINotification.DEFAULT_MESSAGE,
                UINotification.DEFAULT_ICON,
                UINotification.DEFAULT_BUTTON_NAMES,
                UINotification.DEFAULT_BUTTON_TEXT,
                null,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        public UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, string _message, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                _title,
                _message,
                UINotification.DEFAULT_ICON,
                UINotification.DEFAULT_BUTTON_NAMES,
                UINotification.DEFAULT_BUTTON_TEXT,
                null,
                _hideCallback
                );
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        public UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, string _message, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                _title,
                _message,
                UINotification.DEFAULT_ICON,
                UINotification.DEFAULT_BUTTON_NAMES,
                UINotification.DEFAULT_BUTTON_TEXT,
                null,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        public UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, string _message, Sprite _icon, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                _title,
                _message,
                _icon,
                UINotification.DEFAULT_BUTTON_NAMES,
                UINotification.DEFAULT_BUTTON_TEXT,
                null,
                _hideCallback
                );
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        public UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, string _message, Sprite _icon, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                _title,
                _message,
                _icon,
                UINotification.DEFAULT_BUTTON_NAMES,
                UINotification.DEFAULT_BUTTON_TEXT,
                null,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        public UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, string _message, Sprite _icon, string[] _buttonNames, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                _title,
                _message,
                _icon,
                _buttonNames,
                UINotification.DEFAULT_BUTTON_TEXT,
                _buttonCallback,
                _hideCallback
                );
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_message">The message you want to show in the message area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        public UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, string _message, Sprite _icon, string[] _buttonNames, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                _title,
                _message,
                _icon,
                _buttonNames,
                UINotification.DEFAULT_BUTTON_TEXT,
                _buttonCallback,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        public UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, Sprite _icon, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                UINotification.DEFAULT_TITLE,
                UINotification.DEFAULT_MESSAGE,
                _icon,
                UINotification.DEFAULT_BUTTON_NAMES,
                UINotification.DEFAULT_BUTTON_TEXT,
                null,
                _hideCallback
                );
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        public UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, Sprite _icon, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                UINotification.DEFAULT_TITLE,
                UINotification.DEFAULT_MESSAGE,
                _icon,
                UINotification.DEFAULT_BUTTON_NAMES,
                UINotification.DEFAULT_BUTTON_TEXT,
                null,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        public UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, Sprite _icon, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                _title,
                UINotification.DEFAULT_MESSAGE,
                _icon,
                UINotification.DEFAULT_BUTTON_NAMES,
                UINotification.DEFAULT_BUTTON_TEXT,
                null,
                _hideCallback
                );
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_icon">The sprite you want the notification icon to have (if linked)</param>
        public UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, Sprite _icon, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                _title,
                UINotification.DEFAULT_MESSAGE,
                _icon,
                UINotification.DEFAULT_BUTTON_NAMES,
                UINotification.DEFAULT_BUTTON_TEXT,
                null,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        public UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string[] _buttonNames, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                UINotification.DEFAULT_TITLE,
                UINotification.DEFAULT_MESSAGE,
                UINotification.DEFAULT_ICON,
                _buttonNames,
                UINotification.DEFAULT_BUTTON_TEXT,
                _buttonCallback,
                _hideCallback
                );
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        public UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string[] _buttonNames, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                UINotification.DEFAULT_TITLE,
                UINotification.DEFAULT_MESSAGE,
                UINotification.DEFAULT_ICON,
                _buttonNames,
                UINotification.DEFAULT_BUTTON_TEXT,
                _buttonCallback,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        /// <param name="_buttonTexts">The text on the buttons (example: 'OK', 'Cancel', 'Yes', 'No' and so on)</param>
        public UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string[] _buttonNames, string[] _buttonTexts, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                UINotification.DEFAULT_TITLE,
                UINotification.DEFAULT_MESSAGE,
                UINotification.DEFAULT_ICON,
                _buttonNames,
                _buttonTexts,
                _buttonCallback,
                _hideCallback
                );
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        /// <param name="_buttonTexts">The text on the buttons (example: 'OK', 'Cancel', 'Yes', 'No' and so on)</param>
        public UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string[] _buttonNames, string[] _buttonTexts, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                UINotification.DEFAULT_TITLE,
                UINotification.DEFAULT_MESSAGE,
                UINotification.DEFAULT_ICON,
                _buttonNames,
                _buttonTexts,
                _buttonCallback,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        public UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, string[] _buttonNames, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                _title,
                UINotification.DEFAULT_MESSAGE,
                UINotification.DEFAULT_ICON,
                _buttonNames,
                UINotification.DEFAULT_BUTTON_TEXT,
                _buttonCallback,
                _hideCallback
                );
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        public UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, string[] _buttonNames, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                _title,
                UINotification.DEFAULT_MESSAGE,
                UINotification.DEFAULT_ICON,
                _buttonNames,
                UINotification.DEFAULT_BUTTON_TEXT,
                _buttonCallback,
                _hideCallback
                );
        }

        /// <summary>
        /// Show a premade notification with the given settings, using a prefabName.
        /// </summary>
        /// <param name="_prefabName">The prefab name</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        /// <param name="_buttonTexts">The text on the buttons (example: 'OK', 'Cancel', 'Yes', 'No' and so on)</param>
        public UINotification ShowNotification(string _prefabName, float _lifetime, bool _addToNotificationQueue, string _title, string[] _buttonNames, string[] _buttonTexts, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefabName,
                _lifetime,
                _addToNotificationQueue,
                _title,
                UINotification.DEFAULT_MESSAGE,
                UINotification.DEFAULT_ICON,
                _buttonNames,
                _buttonTexts,
                _buttonCallback,
                _hideCallback
                );
        }
        /// <summary>
        /// Show a premade notification with the given settings, using a prefab GameObject reference.
        /// </summary>
        /// <param name="_prefab">The prefab GameObject reference</param>
        /// <param name="_lifetime">How long will the notification be on the screen. Infinite lifetime is -1</param>
        /// <param name="_addToNotificationQueue">Should this notification be added to the NotificationQueue or shown rightaway</param>
        /// <param name="_title">The text you want to show in the title area (if linked)</param>
        /// <param name="_buttonNames">The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click</param>
        /// <param name="_buttonTexts">The text on the buttons (example: 'OK', 'Cancel', 'Yes', 'No' and so on)</param>
        public UINotification ShowNotification(GameObject _prefab, float _lifetime, bool _addToNotificationQueue, string _title, string[] _buttonNames, string[] _buttonTexts, UnityAction[] _buttonCallback = null, UnityAction _hideCallback = null)
        {
            return
            ShowNotification(
                _prefab,
                _lifetime,
                _addToNotificationQueue,
                _title,
                UINotification.DEFAULT_MESSAGE,
                UINotification.DEFAULT_ICON,
                _buttonNames,
                _buttonTexts,
                _buttonCallback,
                _hideCallback
                );
        }
    }
}

