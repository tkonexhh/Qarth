// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

#if dUI_PlayMaker
using UnityEngine;
using DoozyUI;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("DoozyUI")]
    [Tooltip("When the ApplicationQuit command has been issued we either exit play mode (if in editor) or we quit the application if in build mode.")]
    public class ApplicationQuitOrClose : FsmStateAction
    {
        public override void OnEnter()
        {
            UIManager.ApplicationQuit();

            Finish();
        }
    }
}
#endif