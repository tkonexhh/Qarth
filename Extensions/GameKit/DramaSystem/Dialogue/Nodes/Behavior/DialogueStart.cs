using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GFrame.Drame
{
    [CreateNodeMenu("GFrame/Drama System/Drama Start")]
    public class DialogueStart : DialogueBaseNode
    {
        [Output(ShowBackingValue.Unconnected, ConnectionType.Override)]
        public Connection Start;


        protected override void Init()
        {
            base.Init();
            this.name = "Start";
        }





        #region 

        public override void Enter(IPlayerForNode player)
        {
            var output = GetOutputPort(DialogueDefine.NODE_START_OUT);
            if (output.ConnectionCount >= 1)
            {
                var next = output.GetConnection(0).node as DialogueBaseNode;
                next.Enter(player);
            }
            else
            {
                Debug.Log("#Drama Start Node No Connect"); ;
            }
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