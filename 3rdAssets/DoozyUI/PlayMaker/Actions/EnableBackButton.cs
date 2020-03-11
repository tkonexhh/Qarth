// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

#if dUI_PlayMaker
using UnityEngine;
using DoozyUI;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("DoozyUI")]
    [Tooltip("Enables the back button by subtracting another disable level from the additive bool. Forced enable cleares the additive bool levels.")]
    public class EnableBackButton : FsmStateAction
    {
        public FsmBool forceEnable;
        public FsmBool debugThis;

        public override void Reset()
        {
            forceEnable = new FsmBool { UseVariable = false, Value = false };
            debugThis = new FsmBool { UseVariable = false, Value = false };
        }

        public override void OnEnter()
        {
            if(forceEnable.Value)
            {
                UIManager.EnableBackButtonByForce();

                if (debugThis.Value)
                    Debug.Log("[DoozyUI] - Playmaker - State Name [" + State.Name + "] - Enable Back Button - with Forced Enable");
            }
            else
            {
                UIManager.EnableBackButton();

                if (debugThis.Value)
                    Debug.Log("[DoozyUI] - Playmaker - State Name [" + State.Name + "] - Enable Back Button");
            }

            Finish();
        }
    }
}
#endif
