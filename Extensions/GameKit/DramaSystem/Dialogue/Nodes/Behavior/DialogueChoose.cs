using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GFrame.Drame
{
    [CreateNodeMenu("GFrame/Drama System/Choose")]
    public class DialogueChoose : DialogueBaseNode
    {

        [Input] public Connection Input;

        [Input]
        [TextArea]
        [Header("显示文本")]
        public string Content;

        //[HideInInspector]
        public List<Choose> Chooses;

        [System.Serializable]
        public struct Choose
        {
            public string title;
        }

        protected override void Init()
        {
            base.Init();
            this.name = "Choose";
        }


        #region 
        public override void Enter(IPlayerForNode player)
        {
            player.DoChoose(this);
        }

        public override void Exit(IPlayerForNode player)
        {

        }

        public override void DoNext(IPlayerForNode player)
        {

        }

        public override void DoNextWhitParam(IPlayerForNode player, params object[] args)
        {
            if (args == null || args.Length <= 0) return;
            int index = (int)args[0];
            var port = GetOutputPort("Chooses " + index);
            if (port.ConnectionCount > 0)
            {
                var next = port.GetConnection(0).node as DialogueBaseNode;
                next.Enter(player);
            }
            else
            {
                Debug.LogError("#Drama Choose Item Node No Connect");
            }

        }
        #endregion
    }



}