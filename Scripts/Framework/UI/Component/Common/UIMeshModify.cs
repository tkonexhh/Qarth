using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using UnityEngine.UI;

namespace Qarth
{
    public class UIMeshModify : BaseMeshEffect
    {
        [SerializeField]
        private Vector3[] m_BindPos;
        [SerializeField]
        protected Graphic m_GraphicTarget;
        [SerializeField]
        protected bool m_Debug;

        public override void ModifyMesh(VertexHelper vh)
        {
            if (m_BindPos == null || m_BindPos.Length < 4)
            {
                return;
            }

            Color32 color32 = Color.white;

            List<UIVertex> uiVertex = new List<UIVertex>();
            vh.GetUIVertexStream(uiVertex);
            vh.Clear();

            vh.AddVert(m_BindPos[0], color32, uiVertex[0].uv0);
            vh.AddVert(m_BindPos[1], color32, uiVertex[1].uv0);
            vh.AddVert(m_BindPos[2], color32, uiVertex[2].uv0);
            vh.AddVert(m_BindPos[3], color32, uiVertex[4].uv0);

            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(0, 2, 3);
        }

        /*
        protected void OnValidate()
        {
            base.OnValidate();
            if (m_Debug)
            {
                m_Debug = false;
                m_GraphicTarget.SetVerticesDirty();
            }
        }
        */
    }
}
