using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    [Serializable]
    public class RenderTextureConfig
    {
        [SerializeField]
        private Vector2Int m_Size;
        [SerializeField]
        private int m_Depth;
        [SerializeField]
        private RenderTextureFormat m_Format = RenderTextureFormat.ARGBHalf;
        [SerializeField]
        private FilterMode m_FilterMode;
        [SerializeField]
        private TextureWrapMode m_WrapMode = TextureWrapMode.Repeat;
        [SerializeField]
        private bool m_ReadWrite = false;

        public RenderTexture Allocate()
        {
            var rt = RenderTexture.GetTemporary(m_Size.x, m_Size.y, m_Depth, m_Format);
            rt.filterMode = m_FilterMode;
            rt.wrapMode = m_WrapMode;
            return rt;
        }
    }
}
