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
    [Tooltip("Triggers SHOW and/or HIDE for set UIElements")]
    public class ShowHide : FsmStateAction
    {

        public bool debug = false;

        [ActionSection("Show Elements")]
        [Title("")]
        public NavigationPointer[] show;

        [ActionSection("Hide Elements")]
        [Title("")]
        public NavigationPointer[] hide;

        public override void OnEnter()
        {

            if (show != null && show.Length > 0)
            {
                for (int i = 0; i < show.Length; i++)
                {
                    UIManager.ShowUiElement(show[i].name, show[i].category);

                    if (debug) { Debug.Log("[DoozyUI] - Playmaker - State Name [" + State.Name + "] -> Show [ " + show[i].category + " ][ " + show[i].name + "]"); }
                }
            }

            if (hide != null && hide.Length > 0)
            {
                for (int i = 0; i < hide.Length; i++)
                {
                    UIManager.HideUiElement(hide[i].name, hide[i].category);

                    if (debug) { Debug.Log("[DoozyUI] - Playmaker - State Name [" + State.Name + "] -> Hide [ " + hide[i].category + " ][ " + hide[i].name + "]"); }
                }
            }

            Finish();
        }
    }
}
#endif
