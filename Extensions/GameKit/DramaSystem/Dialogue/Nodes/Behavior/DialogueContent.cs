using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace GFrame.Drama
{
    [CreateNodeMenu("GFrame/Drama System/Content")]
    public class DialogueContent : DialogueBaseNode
    {
        [Input] public Connection Input;
        [Output] public Connection Output;

        [Input]
        [Header("标题")]
        public string Title;

        [Input]
        [TextArea]
        [Header("显示文本")]
        public string Content;

        public UnityEvent FinishAction;


        //不可以直接用Conten，否则获取不到外部输入节点
        public string GetContent()
        {
            return GetInputValue<string>("Content", this.Content);
        }

        public string GetTitle()
        {
            return GetInputValue<string>("Title", this.Title);
        }

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