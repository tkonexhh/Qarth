using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XNode;


namespace  GFrame.Drama
{
    [Serializable, CreateAssetMenu(fileName = "New Drama Graph", menuName = "GFrame/Drama System/Create Drama Graph")]
    public class DialogueBluePrint : NodeGraph
    {

        #region 

        private DialogueStart m_StartNode;

        public void StartDialogue(IPlayerForNode player)
        {
            foreach (var item in nodes)
            {
                if (item.name == DialogueDefine.NODE_START && m_StartNode == null)
                {
                    m_StartNode = (DialogueStart)item;
                }
            }


            if (m_StartNode != null)
            {
                m_StartNode.Enter(player);
                return;
            }
        }

        #endregion
    }
}
