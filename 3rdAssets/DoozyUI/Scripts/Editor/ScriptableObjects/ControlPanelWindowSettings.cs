// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEngine.Core;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace DoozyUI
{
    public class ControlPanelWindowSettings : ScriptableObject
    {
        private static ControlPanelWindowSettings _instance;
        public static ControlPanelWindowSettings Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = Q.GetResource<ControlPanelWindowSettings>(DUI.RESOURCES_PATH_CONTROL_PANEL_WINDOW_SETTINGS, "ControlPanelWindowSettings");

                    if(_instance == null)
                    {
                        _instance = Q.CreateAsset<ControlPanelWindowSettings>(DUI.RELATIVE_PATH_CONTROL_PANEL_WINDOW_SETTINGS, "ControlPanelWindowSettings");
                    }
                }
                return _instance;
            }
        }

        public float windowMinimumWidth { get { return SidebarCurrentWidth + pageShadowWidth + pageWidth + pageWidthExtraSpace.value + scrollbarSize; } }
        public float windowMinimumHeight = 640;

        [Space(20)]
        public float sidebarExpandedWidth = 264;
        public float sidebarCollapsedWidth = 32;
        public AnimBool sidebarIsExpanded = new AnimBool(true);
        public float SidebarCurrentWidth { get { return (sidebarExpandedWidth * sidebarIsExpanded.faded) + (sidebarCollapsedWidth * (1 - sidebarIsExpanded.faded)); } }

        [Space(10)]
        public float sidebarButtonHeight = 32;
        public float sidebarVerticalSpacing = 16;
        public float sidebarLogoHeight = 80;

        [Space(20)]
        public float scrollbarSize = 32;

        [Space(20)]
        public float pageShadowWidth = 16;
        public float pageWidth = 640;
        public AnimFloat pageWidthExtraSpace = new AnimFloat(0);
        public float pageHeaderHeight = 64;
        public float CurrentPageContentWidth { get { return pageWidth + pageWidthExtraSpace.value; } }

        [Space(20)]
        public float editorAnimationSpeed = 4;

        [Space(20)]
        public ControlPanelWindow.Page currentPage = ControlPanelWindow.Page.General;
        public SoundType selectedUISoundsFilter = SoundType.All;
        public ControlPanelWindow.AnimatorPreset selectedAnimatorPresetTab = ControlPanelWindow.AnimatorPreset.In;
    }
}
