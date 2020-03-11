using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GFrame.Drame
{
    [CreateNodeMenu("GFrame/Drama System/Content")]
    public class DialogueContent : DialogueBaseNode
    {
        [Input] public Connection Input;
        [Output] public Connection Output;

        [Input]
        [TextArea]
        [Header("显示文本")]
        public string Content;

        [Header("对话立绘")]
        public Texture2D Speaker;

        [Header("对话框背景")]
        public Texture2D ContentBg;

        protected override void Init()
        {
            base.Init();
            this.name = "Content";
        }


        #region 
        public override void Enter(IPlayerForNode player)
        {
            player.DoContent(this);
        }

        public override void Exit(IPlayerForNode player)
        {

        }

        public override void DoNext(IPlayerForNode player)
        {
            var output = GetOutputPort(DialogueDefine.NODE_CONTENT_OUTPUT);
            if (output.ConnectionCount >= 1)
            {
                var next = output.GetConnection(0).node as DialogueBaseNode;
                next.Enter(player);
            }
            else
            {
                Debug.LogError("#Drama Content Node No Connect");
                //意外结束
                //player.DoFinish("");
            }
        }

        public override void DoNextWhitParam(IPlayerForNode player, params object[] args)
        {

        }
        #endregion
    }

}