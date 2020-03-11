// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor.AnimatedValues;
#endif

namespace DoozyUI
{
    [Serializable]
    public class AnimData : ScriptableObject
    {
        public string presetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
        public string presetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
        public Anim data;
#if UNITY_EDITOR
        public AnimBool isExpanded = new AnimBool(false);
#endif

        public AnimData(Anim.AnimationType aType)
        {
            presetName = UIAnimatorUtil.DEFAULT_PRESET_NAME;
            presetCategory = UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME;
            data = new Anim(aType);
#if UNITY_EDITOR
            isExpanded = new AnimBool(false);
#endif
        }
        public bool LoadDefaultValues { get { return presetName.Equals(UIAnimatorUtil.DEFAULT_PRESET_NAME) && presetCategory.Equals(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME); } }
    }
}