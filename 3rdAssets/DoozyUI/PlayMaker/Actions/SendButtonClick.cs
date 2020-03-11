// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

#if dUI_PlayMaker
using UnityEngine;
using HutongGames.PlayMaker;
using DoozyUI;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("DoozyUI")]
    [Tooltip("Simulates a button click without playing any animations")]
    public class SendButtonClick : FsmStateAction
    {
        public bool debug = false;

        [RequiredField]
        public FsmString buttonName;

        public UIButton.ButtonClickType clickType = UIButton.ButtonClickType.OnClick;

        public override void Reset()
        {
            buttonName = new FsmString { UseVariable = false };
            clickType = UIButton.ButtonClickType.OnClick;
        }

        public override void OnEnter()
        {
            UIManager.Instance.SendButtonAction(buttonName.Value, clickType);
            if (debug) { Debug.Log("[DoozyUI] - Playmaker - State Name [" + State.Name + "] - Simulated an " + clickType + " button click - using the '" + buttonName + "' button name."); }
            Finish();
        }
    }
}
#endif