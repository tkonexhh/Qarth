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
    [CustomEditor(typeof(PlaymakerEventDispatcher), true)]
    [CanEditMultipleObjects]
    public class PlaymakerEventDispatcherEditor : QEditor
    {
#if dUI_PlayMaker
        PlaymakerEventDispatcher playmakerEventDispatcher { get { return (PlaymakerEventDispatcher)target; } }

        SerializedProperty
            debug,
            overrideTargetFSM, targetFSM,
            dispatchGameEvents, dispatchButtonClicks;

        AnimBool
            showHowToUse;

        protected override void SerializedObjectFindProperties()
        {
            base.SerializedObjectFindProperties();

            debug = serializedObject.FindProperty("debug");
            overrideTargetFSM = serializedObject.FindProperty("overrideTargetFSM");
            targetFSM = serializedObject.FindProperty("targetFSM");
            dispatchGameEvents = serializedObject.FindProperty("dispatchGameEvents");
            dispatchButtonClicks = serializedObject.FindProperty("dispatchButtonClicks");
        }

        protected override void GenerateInfoMessages()
        {
            base.GenerateInfoMessages();

            infoMessage.Add("DisabledSelectListener",
                            new InfoMessage()
                            {
                                title = "Disabled",
                                message = "Select at least one listener in order to activate the event dispatcher.",
                                type = InfoMessageType.Error,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("DisabledMissingTargetFSM", 
                            new InfoMessage()
                            {
                                title = "Disabled",
                                message = "Reference a Target FSM in order to activate the event dispatcher.",
                                type = InfoMessageType.Error,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("DispatchingGameEvents",
                            new InfoMessage()
                            {
                                title = "",
                                message = "Dispatching Game Events...",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("DispatchingButtonClicks", 
                            new InfoMessage()
                            {
                                title = "",
                                message = "Dispatching Button Clicks...",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("DispatchingGameEventsAndButtonClicks", 
                            new InfoMessage()
                            {
                                title = "",
                                message = "Dispatching Game Events and Button Clicks...",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("HowToUse", 
                            new InfoMessage()
                            {
                                title = "",
                                message = "This dispatcher auto targets the first FSM on this GameObject." +
                                          "\n" +
                                          "You can override that and reference the FSM you want to target." +
                                          "\n" + "\n" +
                                          "For this dispatcher to work with the target Playmaker FSM you have to create FSM Events named exactly as the Game Events or Button Names that you want to listen for and react to." +
                                          "The FSM Events, Game Events and Button Names are case sensitive." +
                                          "\n" + "\n" +
                                          "To dispatch Game Events, you have to create, in the target FSM, Events named exactly as the Game Event commands you wants to catch." +
                                          "\n" + "\n" +
                                          "To dispatch Button Clicks, you have to create, in the target FSM, Events named exactly as the Button Names you wants to catch.",
                                type = InfoMessageType.Info,
                                show = new AnimBool(false, Repaint)
                            });
        }

        protected override void InitAnimBools()
        {
            base.InitAnimBools();

            showHowToUse = new AnimBool(false, Repaint);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            requiresContantRepaint = true;
        }

        public override void OnInspectorGUI()
        {
            DrawHeader(DUIResources.headerPlayMakerEventDispatcher.texture, WIDTH_420, HEIGHT_42);
            serializedObject.Update();
            QUI.Space(-SPACE_4);

            infoMessage["DisabledMissingTargetFSM"].show.target = targetFSM.objectReferenceValue == null;
            DrawInfoMessage("DisabledMissingTargetFSM", WIDTH_420);

            infoMessage["DisabledSelectListener"].show.target = !dispatchGameEvents.boolValue && !dispatchButtonClicks.boolValue && targetFSM.objectReferenceValue != null;
            DrawInfoMessage("DisabledSelectListener", WIDTH_420);

            infoMessage["DispatchingGameEventsAndButtonClicks"].show.target = dispatchGameEvents.boolValue && dispatchButtonClicks.boolValue && targetFSM.objectReferenceValue != null; ;
            DrawInfoMessage("DispatchingGameEventsAndButtonClicks", WIDTH_420);

            infoMessage["DispatchingGameEvents"].show.target = dispatchGameEvents.boolValue && !dispatchButtonClicks.boolValue && targetFSM.objectReferenceValue != null; ;
            DrawInfoMessage("DispatchingGameEvents", WIDTH_420);

            infoMessage["DispatchingButtonClicks"].show.target = !dispatchGameEvents.boolValue && dispatchButtonClicks.boolValue && targetFSM.objectReferenceValue != null; ;
            DrawInfoMessage("DispatchingButtonClicks", WIDTH_420);

            QUI.Space(SPACE_4);
            DrawDebug();
            QUI.Space(SPACE_2);
            DrawTargetFSM();
            DrawDispatcherSelector();
            QUI.Space(SPACE_4);
            DrawAbout();
            serializedObject.ApplyModifiedProperties();
            QUI.Space(SPACE_4);
        }

        void DrawDebug()
        {
            QUI.QToggle("debug", debug);
        }
        void DrawTargetFSM()
        {
            QUI.BeginHorizontal(WIDTH_420);
            {
                GUI.enabled = overrideTargetFSM.boolValue;
                QUI.QObjectPropertyField("Target FSM", targetFSM, WIDTH_420 - 78);
                GUI.enabled = true;
                if(targetFSM == null) { overrideTargetFSM.boolValue = true; }
                QUI.QToggle("override", overrideTargetFSM);
            }
            QUI.EndHorizontal();
            QUI.Space(-SPACE_2);
            QUI.BeginHorizontal(WIDTH_420);
            {
                QUI.Space(78);
                QLabel.text = "FSM Name: " + (playmakerEventDispatcher.targetFSM == null ? "---" : playmakerEventDispatcher.targetFSM.FsmName);
                QLabel.style = Style.Text.Help;
                QUI.Label(QLabel);
            }
            QUI.EndHorizontal();

        }
        void DrawDispatcherSelector()
        {
            QUI.BeginHorizontal(WIDTH_420);
            {
                QLabel.text = "Dispatch";
                QLabel.style = Style.Text.Normal;
                QUI.Label(QLabel);
                QUI.QToggle("Game Events", dispatchGameEvents);
                QUI.QToggle("Button Clicks", dispatchButtonClicks);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawAbout()
        {
            QUI.BeginHorizontal(WIDTH_420);
            {
                showHowToUse.target = QUI.QToggle("Remind me how to use this with PlayMaker", showHowToUse.target);
            }
            QUI.EndHorizontal();
            infoMessage["HowToUse"].show.target = showHowToUse.target;
            DrawInfoMessage("HowToUse", WIDTH_420);
        }
#endif
    }
}
