// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

#if dUI_PlayMaker
using UnityEngine;
using HutongGames.PlayMaker;
using System.Collections;
using DoozyUI;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("DoozyUI")]
    [Tooltip("DoozyUI - Show UINotification")]
    public class ShowUINotification : FsmStateAction
    {
#region Variables
        [RequiredField]
        [UIHint(UIHint.FsmString)]
        [Tooltip("The prefab name")]
        public FsmString prefabName;

        [UIHint(UIHint.FsmFloat)]
        [Tooltip("How long will the notification be on the screen. Infinite lifetime is -1")]
        public FsmFloat lifetime;

        [UIHint(UIHint.FsmBool)]
        [Tooltip("Should this notification be added to the NotificationQueue or shown right away")]
        public FsmBool addToNotificationQueue;

        [UIHint(UIHint.FsmString)]
        [Tooltip("The text you want to show in the title area (if linked)")]
        public FsmString title;

        [UIHint(UIHint.FsmString)]
        [Tooltip("The message you want to show in the message area (if linked)")]
        public FsmString message;

        [UIHint(UIHint.FsmObject)]
        [Tooltip("The sprite you want the notification icon to have (if linked)")]
        public FsmObject icon;

        [Tooltip("The button names you want the notification to have (from left to right). These values are the ones that we listen to as button click")]
        public string[] buttonNames;

        [Tooltip("The text on the buttons (example: 'OK', 'Cancel', 'Yes', 'No' and so on)")]
        public string[] buttonTexts;
#endregion

        public override void Reset()
        {
            prefabName = new FsmString { UseVariable = false, Value = string.Empty };
            lifetime = new FsmFloat { UseVariable = false, Value = UINotification.DEFAULT_LIFETIME };
            addToNotificationQueue = new FsmBool { UseVariable = false, Value = UINotification.DEFAULT_ADD_TO_NOTIFICATION_QUEUE };
            title = new FsmString { UseVariable = false, Value = UINotification.DEFAULT_TITLE };
            message = new FsmString { UseVariable = false, Value = UINotification.DEFAULT_MESSAGE };
            icon = new FsmObject { UseVariable = false, Value = UINotification.DEFAULT_ICON };
            buttonNames = UINotification.DEFAULT_BUTTON_NAMES;
            buttonTexts = UINotification.DEFAULT_BUTTON_TEXT;
        }

        public override void OnEnter()
        {
            Sprite _icon = icon.Value as Sprite;
            UIManager.ShowNotification(prefabName.Value, lifetime.Value, addToNotificationQueue.Value, title.Value, message.Value, _icon, buttonNames, buttonTexts);
            Finish();
        }
    }
}
#endif
