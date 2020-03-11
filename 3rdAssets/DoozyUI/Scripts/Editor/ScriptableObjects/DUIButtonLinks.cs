// Copyright (c) 2016 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using QuickEngine.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DoozyUI.Internal
{
    public class DUIButtonLinks : ScriptableObject
    {
        public static DUIButtonLinks _instance;
        public static DUIButtonLinks Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = Q.GetResource<DUIButtonLinks>(DUI.RESOURCES_PATH_BUTTON_LINKS, "DUIButtonLinks");

#if UNITY_EDITOR
                    if(_instance == null)
                    {
                        _instance = Q.CreateAsset<DUIButtonLinks>(DUI.RELATIVE_PATH_BUTTON_LINKS, "DUIButtonLinks");
                    }
#endif
                }
                return _instance;
            }
        }

        public List<LinkButtonData> recommendedAssets = new List<LinkButtonData>();
    }
}
