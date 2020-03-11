// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System;
using UnityEngine;

namespace DoozyUI
{
    [Serializable]
    public class UISound : ScriptableObject
    {
        public string soundName = "";
        public AudioClip audioClip = null;
        public SoundType soundType = SoundType.All;

        public UISound(string sName, AudioClip aClip, SoundType sType = SoundType.All)
        {
            soundName = sName;
            audioClip = aClip;
            soundType = sType;
        }
    }

    public enum SoundType
    {
        All,
        UIButtons,
        UIElements
    }
}
