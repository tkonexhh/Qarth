using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame.Drame
{
    public class DialoguePlayer : IPlayerForNode
    {

        private DialogueBaseNode m_CurNode;
        private DialogueBluePrint m_CurBluePrint;


        public Action<DialogueContent> OnDoContent;
        public Action<DialogueChoose> OnDoChoose;
        public Action<DialogueFinish> OnDoFinish;

        public void PlayDialogue(DialogueBluePrint bluePrint)
        {
            // if (bluePrint.DialogueName.IsNullOrEmpty())
            // {
            //     Debug.LogError("[GameKit][Drama] 播放对话失败，指定的对话蓝图没有命名");
            //     //return;
            // }

            m_CurBluePrint = bluePrint;
            m_CurBluePrint.StartDialogue(this);
        }

        public void Next()
        {
            if (m_CurNode != null)
            {
                m_CurNode.Exit(this);
                m_CurNode.DoNext(this);
            }
            else
            {
                Debug.LogError("#Drama CurNode Is Null");
            }

        }

        public void NextWithParam(params object[] args)
        {
            if (m_CurNode != null)
            {
                m_CurNode.Exit(this);
                m_CurNode.DoNextWhitParam(this, args);
            }
        }



        /// <summary>
        /// 执行一个普通内容对话
        /// </summary>
        /// <param name="content"></param>
        public void DoContent(DialogueContent content)
        {
            m_CurNode = content;

            if (OnDoContent != null)
                OnDoContent.Invoke(content);
        }


        /// <summary>
        /// 执行一个选择分支对话
        /// </summary>
        /// <param name="choose"></param>
        public void DoChoose(DialogueChoose choose)
        {
            m_CurNode = choose;
            if (OnDoChoose != null)
                OnDoChoose.Invoke(choose);
        }

        /// <summary>
        /// 结束
        /// </summary>
        /// <param name="param"></param>
        public void DoFinish(DialogueFinish finish)
        {
            m_CurNode = null;
            if (OnDoFinish != null)
                OnDoFinish.Invoke(finish);
        }
    }

}