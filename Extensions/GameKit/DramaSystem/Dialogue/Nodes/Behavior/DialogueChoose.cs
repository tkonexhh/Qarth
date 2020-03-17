using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GFrame.Drama
{
    [CreateNodeMenu("GFrame/Drama System/Choose")]
    public class DialogueChoose : DialogueBaseNode
    {

        [Input] public Connection Input;

        [Input]
        [Header("显示标题")]
        public string Title;

        [Input]
        [TextArea]
        [Header("显示文本")]
        public string Content;

        //[HideInInspector]
        public List<Choose> Chooses;

        [System.Serializable]
        public class Choose
        {
            public string Title;
        }


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

            Choose choose = (Choose)(args[0]);
            int index = -1;
            for (int i = 0; i < Chooses.Count; i++)
            {
                if (Chooses[i] == choose)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                var port = GetOutputPort("Chooses " + index);
                if (port.ConnectionCount > 0)
                {
                    var next = port.GetConnection(0).node as DialogueBaseNode;
                    next.Enter(player);
                }
                else
                {
                    Debug.LogError("[Drama][Choose Item Node No Connect]");
                }
            }
            else
            {
                Debug.LogError("[Drama][Choose Item No Choose]");
            }

        }
        #endregion
    }



}