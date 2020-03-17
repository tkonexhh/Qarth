using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame.Drama
{
    public class DialoguePlayer : IPlayerForNode
    {
        private DialogueBaseNode m_CurNode;
        private DialogueBluePrint m_CurBluePrint;
        private DialogueState m_State;
        public DialogueState State
        {
            get { return m_State; }
        }


        public Action<DialogueContent> OnDoContent;
        public Action<DialogueChoose> OnDoChoose;
        public Action<DialogueFinish> OnDoFinish;

        private List<DialogueBluePrint> m_LstBluePrint = new List<DialogueBluePrint>();

        public void AddBluePrint(DialogueBluePrint bluePrint)
        {
            if (!m_LstBluePrint.Contains(bluePrint))
            {
                m_LstBluePrint.Add(bluePrint);
            }
        }

        public void PlayDialogue()
        {
            if (m_LstBluePrint.Count > 0)
            {
                PlayDialogue(m_LstBluePrint[0]);
            }
        }

        public void PlayDialogue(DialogueBluePrint bluePrint)
        {
            m_State = DialogueState.None;
            AddBluePrint(bluePrint);
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
                Debug.LogError("[Drama][CurNode Is Null]");
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

        public void PickChoose(DialogueChoose.Choose choose)
        {
            NextWithParam(choose);
        }


        /// <summary>
        /// 执行一个普通内容对话
        /// </summary>
        /// <param name="content"></param>
        public void DoContent(DialogueContent content)
        {
            m_CurNode = content;
            m_State = DialogueState.Content;
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
            m_State = DialogueState.Choose;
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
            m_State = DialogueState.Finish;
            if (OnDoFinish != null)
                OnDoFinish.Invoke(finish);

            //当前剧情结束，移除剧情
            m_LstBluePrint.Remove(m_CurBluePrint);
            m_CurBluePrint = null;

            //检测是否有可播放的剧情
            if (m_LstBluePrint.Count > 0)
            {
                Debug.LogError("PlayNext");
                PlayDialogue(m_LstBluePrint[0]);
            }
        }
    }

}