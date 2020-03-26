/************************
	FileName:/Qarth/Extensions/GameKit/RedPointSystem/Scripts/RedPointNode.cs
	CreateAuthor:xuhonghua
	CreateTime:3/20/2020 2:37:19 PM
************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GFrame
{
    public class RedPointNode
    {
        public event RedPointSystem.OnRedNumChange OnNumChange;
        public RedPointNode parent = null;
        private int m_PointNum = 0;

        private List<RedPointNode> m_ChildNode = null;

        public bool IsRoot()
        {
            return parent == null;
        }

        public bool IsEnd()
        {
            if (m_ChildNode == null || m_ChildNode.Count == 0)
                return true;

            return false;
        }

        public int PointNum
        {
            get
            {
                return m_PointNum;
            }
            set
            {
                if (IsEnd())
                {
                    m_PointNum = value;
                    if (m_PointNum < 0) m_PointNum = 0;
                    NotifyPointNumChange();

                    if (parent != null)
                    {
                        parent.ChangeNum();
                    }
                }
            }
        }

        private void NotifyPointNumChange()
        {
            if (OnNumChange != null)
                OnNumChange();
        }

        private void ChangeNum()
        {
            int num = 0;
            if (m_ChildNode != null)
                foreach (var node in m_ChildNode)
                {
                    num += node.m_PointNum;
                }

            if (num != m_PointNum)
            {
                m_PointNum = num;
                NotifyPointNumChange();
            }

            if (parent != null)
            {
                parent.ChangeNum();
            }

        }


        public void AddNode(RedPointNode node)
        {
            if (m_ChildNode == null)
                m_ChildNode = new List<RedPointNode>();

            node.parent = this;
            m_ChildNode.Add(node);
        }

    }

}