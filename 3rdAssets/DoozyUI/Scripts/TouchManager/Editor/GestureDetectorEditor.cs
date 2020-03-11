// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using QuickEngine.Extensions;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace DoozyUI.Gestures
{
    [CustomEditor(typeof(GestureDetector), true)]
    [DisallowMultipleComponent]
    public class GestureDetectorEditor : QEditor
    {
        private static string _IMAGES = "";
        public static string IMAGES
        {
            get
            {
                if(_IMAGES.IsNullOrEmpty())
                {
                    _IMAGES = QuickEngine.IO.File.GetRelativeDirectoryPath("DoozyUI") + "/Images";
                }
                return _IMAGES;
            }
        }

        private static string _HEADERS;
        public static string HEADERS { get { if(string.IsNullOrEmpty(_HEADERS)) { _HEADERS = IMAGES + "/Headers/"; } return _HEADERS; } }

        public static QTexture headerGestureDetector;

        GestureDetector Detector { get { return (GestureDetector)target; } }

        SerializedProperty
            debug,
            isGlobalGestureDetector,
            overrideTarget, targetGameObject,
            gestureType,
            swipeDirection,
            OnTap, OnLongTap, OnSwipe;

#if dUI_DoozyUI
        SerializedProperty
            gameEvents;
#endif

        AnimBool
            showTarget,
            showSwipeDirection,
            showOnTap,
            showOnLongTap,
            showOnSwipe,
            showGameEventsAnimBool,
#pragma warning disable 0414 //value never used
            showNavigation;
#pragma warning restore 0414

#if dUI_DoozyUI
        EditorNavigationPointerData editorNavigationData = new EditorNavigationPointerData();
#endif

#if dUI_DoozyUI
        float GlobalWidth { get { return DUI.GLOBAL_EDITOR_WIDTH; } }
        int MiniBarHeight { get { return DUI.MINI_BAR_HEIGHT; } }
#else
        float GlobalWidth { get { return 420; } }
        int MiniBarHeight { get { return 18; } }
#endif

        protected override void SerializedObjectFindProperties()
        {
            base.SerializedObjectFindProperties();

            debug = serializedObject.FindProperty("debug");
            isGlobalGestureDetector = serializedObject.FindProperty("isGlobalGestureDetector");
            overrideTarget = serializedObject.FindProperty("overrideTarget");
            targetGameObject = serializedObject.FindProperty("targetGameObject");
            gestureType = serializedObject.FindProperty("gestureType");
            swipeDirection = serializedObject.FindProperty("swipeDirection");
            OnTap = serializedObject.FindProperty("OnTap");
            OnLongTap = serializedObject.FindProperty("OnLongTap");
            OnSwipe = serializedObject.FindProperty("OnSwipe");

#if dUI_DoozyUI
            gameEvents = serializedObject.FindProperty("gameEvents");
#endif
        }

        protected override void InitAnimBools()
        {
            base.InitAnimBools();

            showTarget = new AnimBool(!isGlobalGestureDetector.boolValue, Repaint);
            showSwipeDirection = new AnimBool(gestureType.enumValueIndex == (int)GestureType.Swipe, Repaint);

            showOnTap = new AnimBool(false, Repaint);
            showOnLongTap = new AnimBool(false, Repaint);
            showOnSwipe = new AnimBool(false, Repaint);
#if dUI_DoozyUI
            showGameEventsAnimBool = new AnimBool(gameEvents.arraySize > 0, Repaint);
#endif
            showNavigation = new AnimBool(false, Repaint);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            requiresContantRepaint = true;

            UpdateTarget();
            SyncData();

            headerGestureDetector = new QTexture(HEADERS, "headerGestureDetector" + QResources.IsProSkinTag);
        }

        void UpdateTarget()
        {
            if(Detector.isGlobalGestureDetector) { return; }
            if(Detector.overrideTarget) { return; }
            if(Detector.targetGameObject == null)
            {
                Detector.targetGameObject = Detector.gameObject;
            }
        }

        void SyncData()
        {
#if dUI_DoozyUI
            DUIData.Instance.ValidateUIElements(); //validate the database (used by the Navigation Manager)
            UpdateAllNavigationData();
#endif
        }
        void UpdateAllNavigationData()
        {
#if dUI_DoozyUI
            if(!UIManager.IsNavigationEnabled)
            {
                return;
            }

            DUIUtils.UpdateNavigationDataList(Detector.navigationPointerData.show, editorNavigationData.showIndex);
            DUIUtils.UpdateNavigationDataList(Detector.navigationPointerData.hide, editorNavigationData.hideIndex);
#endif
        }

        public override void OnInspectorGUI()
        {
            DrawHeader(headerGestureDetector.texture, WIDTH_420, HEIGHT_42);

            serializedObject.Update();

            QUI.QToggle("debug", debug);
            QUI.Space(SPACE_2);
            QUI.QToggle("is Global Gesture Detector", isGlobalGestureDetector);
            QUI.Space(SPACE_2);
            showTarget.target = !isGlobalGestureDetector.boolValue;
            if(QUI.BeginFadeGroup(showTarget.faded))
            {
                QUI.BeginHorizontal(GlobalWidth);
                {
                    GUI.enabled = overrideTarget.boolValue;
                    QUI.QObjectPropertyField("Target GameObject", targetGameObject, GlobalWidth - 100);
                    GUI.enabled = true;
                    QUI.Space(SPACE_2);
                    QUI.QToggle("override", overrideTarget, 20);
                }
                QUI.EndHorizontal();
            }
            QUI.EndFadeGroup();
            QUI.Space(SPACE_2);
            showSwipeDirection.target = (GestureType)gestureType.enumValueIndex == GestureType.Swipe;
            QUI.BeginHorizontal(GlobalWidth);
            {
                QUI.QObjectPropertyField("Gesture Type", gestureType, ((GlobalWidth - SPACE_2) / 2), 20, false);
                QUI.Space(SPACE_2);
                if(showSwipeDirection.faded > 0.2f)
                {
                    QUI.QObjectPropertyField("Swipe Direction", swipeDirection, ((GlobalWidth - SPACE_2) / 2) * showSwipeDirection.faded, 20, false);
                }
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
            QUI.Space(SPACE_2);
#if dUI_DoozyUI
            switch((GestureType) gestureType.enumValueIndex)
            {
                case GestureType.Tap: DUIUtils.DrawUnityEvents(Detector.OnTap.GetPersistentEventCount() > 0, showOnTap, OnTap, "OnTap", GlobalWidth, MiniBarHeight); break;
                case GestureType.LongTap: DUIUtils.DrawUnityEvents(Detector.OnLongTap.GetPersistentEventCount() > 0, showOnLongTap, OnLongTap, "OnLongTap", GlobalWidth, MiniBarHeight); break;
                case GestureType.Swipe: DUIUtils.DrawUnityEvents(Detector.OnSwipe.GetPersistentEventCount() > 0, showOnSwipe, OnSwipe, "OnSwipe", GlobalWidth, MiniBarHeight); break;
            }
#else
            switch((GestureType)gestureType.enumValueIndex)
            {
                case GestureType.Tap: DrawUnityEvents(Detector.OnTap.GetPersistentEventCount() > 0, showOnTap, OnTap, "OnTap", GlobalWidth, MiniBarHeight); break;
                case GestureType.LongTap: DrawUnityEvents(Detector.OnLongTap.GetPersistentEventCount() > 0, showOnLongTap, OnLongTap, "OnLongTap", GlobalWidth, MiniBarHeight); break;
                case GestureType.Swipe: DrawUnityEvents(Detector.OnSwipe.GetPersistentEventCount() > 0, showOnSwipe, OnSwipe, "OnSwipe", GlobalWidth, MiniBarHeight); break;
            }
#endif

#if dUI_DoozyUI
            QUI.Space(SPACE_2);
            QUI.DrawCollapsableList("Game Events", showGameEventsAnimBool, gameEvents.arraySize > 0 ? QColors.Color.Blue : QColors.Color.Gray, gameEvents, GlobalWidth, 18, "Not sending any Game Events on gesture... Click [+] to start...");
            QUI.Space(SPACE_2);
            DUIUtils.DrawNavigation(this, Detector.navigationPointerData, editorNavigationData, showNavigation, UpdateAllNavigationData, true, false, GlobalWidth, MiniBarHeight);
#endif

            serializedObject.ApplyModifiedProperties();
            QUI.Space(SPACE_4);
        }

        public static Color AccentColorBlue { get { return QUI.IsProSkin ? QColors.Blue.Color : QColors.BlueLight.Color; } }
        public static Color AccentColorGray { get { return QUI.IsProSkin ? QColors.UnityLight.Color : QColors.UnityLight.Color; } }
        public static void DrawUnityEvents(bool enabled, AnimBool showEvents, SerializedProperty unityEvent, string unityEventTitle, float width, float miniBarHeight)
        {
            if(QUI.GhostBar("Unity Events", enabled ? QColors.Color.Blue : QColors.Color.Gray, showEvents, width, miniBarHeight))
            {
                showEvents.target = !showEvents.target;
            }
            QUI.BeginHorizontal(width);
            {
                QUI.Space(8 * showEvents.faded);
                if(QUI.BeginFadeGroup(showEvents.faded))
                {
                    QUI.SetGUIBackgroundColor(enabled ? AccentColorBlue : AccentColorGray);
                    QUI.BeginVertical(width - 16);
                    {
                        QUI.Space(2 * showEvents.faded);
                        QUI.PropertyField(unityEvent, new GUIContent() { text = unityEventTitle }, width - 8);
                        QUI.Space(2 * showEvents.faded);
                    }
                    QUI.EndVertical();
                    QUI.ResetColors();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();
        }
    }
}
