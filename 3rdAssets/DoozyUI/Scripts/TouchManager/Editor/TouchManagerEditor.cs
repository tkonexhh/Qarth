// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using QuickEngine.Extensions;
using UnityEditor;
using UnityEngine;

namespace DoozyUI.Gestures
{
    [CustomEditor(typeof(TouchManager), true)]
    [DisallowMultipleComponent]
    public class TouchManagerEditor : QEditor
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

        public static QTexture headerTouchManager;

        TouchManager Manager { get { return (TouchManager)target; } }

        SerializedProperty
            debug,
            minSwipeLength,
            longTapDuration,
            useEightDirections;

#if dUI_DoozyUI
        float GlobalWidth { get { return DUI.GLOBAL_EDITOR_WIDTH; } }
#else
        float GlobalWidth { get { return 420; } }
#endif

        protected override void SerializedObjectFindProperties()
        {
            base.SerializedObjectFindProperties();

            debug = serializedObject.FindProperty("debug");
            minSwipeLength = serializedObject.FindProperty("minSwipeLength");
            longTapDuration = serializedObject.FindProperty("longTapDuration");
            useEightDirections = serializedObject.FindProperty("useEightDirections");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            requiresContantRepaint = true;

            headerTouchManager = new QTexture(HEADERS, "headerTouchManager" + QResources.IsProSkinTag);
        }

        public override void OnInspectorGUI()
        {
            DrawHeader(headerTouchManager.texture, WIDTH_420, HEIGHT_42);
            serializedObject.Update();
            QUI.QToggle("debug", debug);
            QUI.Space(SPACE_2);
            QUI.QObjectPropertyField("Min Swipe Length", minSwipeLength, GlobalWidth, 20, false);
            QUI.Space(SPACE_2);
            QUI.QObjectPropertyField("Long Tap Duration", longTapDuration, 174, 20, false);
            QUI.Space(SPACE_2);
            QUI.QToggle("use eight directions", useEightDirections);
            serializedObject.ApplyModifiedProperties();
            QUI.Space(SPACE_4);
        }
    }
}
