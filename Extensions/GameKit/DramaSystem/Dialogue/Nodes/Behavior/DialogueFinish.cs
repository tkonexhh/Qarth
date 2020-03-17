using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;


namespace GFrame.Drama
{
    [CreateNodeMenu("GFrame/Drama System/Finish")]
    public class DialogueFinish : DialogueBaseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override)]
        public Connection Input;

        protected override void Init()
        {
            base.Init();
            this.name = "Finish";
        }


        #region 
        public override void Enter(IPlayerForNode player)
        {
            player.DoFinish(this);
        }

        public override void Exit(IPlayerForNode player)
        {

        }

        public override void DoNext(IPlayerForNode player)
        {

        }

        public override void DoNextWhitParam(IPlayerForNode player, params object[] args)
        {

        }
        #endregion
    }

}