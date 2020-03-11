// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

#if dUI_PlayMaker
using UnityEngine;
using DoozyUI;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("DoozyUI")]
    [Tooltip("Toggles pause through timescale adjustment")]
    public class TogglePause : FsmStateAction
    {
        public FsmBool debugThis;

        public override void Reset()
        {
            debugThis = new FsmBool { UseVariable = false, Value = false };
        }

        public override void OnEnter()
        {
            UIManager.TogglePause();

            if (debugThis.Value)
                Debug.Log("[DoozyUI] - Playmaker - State Name [" + State.Name + "] - Toggle Pause");

            Finish();
        }
    }
}
#endif
