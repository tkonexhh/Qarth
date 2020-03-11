// Copyright (c) 2016 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEngine.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DoozyUI.Internal
{
    public class DUIVersion : ScriptableObject
    {
        public static DUIVersion _instance;
        public static DUIVersion Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = Q.GetResource<DUIVersion>(DUI.RESOURCES_PATH_VERSION, "DUIVersion");

#if UNITY_EDITOR
                    if(_instance == null)
                    {
                        _instance = Q.CreateAsset<DUIVersion>(DUI.RELATIVE_PATH_VERSION, "DUIVersion");
                    }
#endif
                }
                return _instance;
            }
        }

        public string version = "unknown";

        public bool performDatabaseUpgrade = true;
	}
}
