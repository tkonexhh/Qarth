using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using UnityEngine.UI;

namespace Qarth
{
    public class ModelShow3D : MonoBehaviour
    {
        [SerializeField]
        private RawImage m_Image;
        [SerializeField]
        private Camera m_Camera;
        [SerializeField]
        private RenderTextureConfig m_Config;

        private RenderTexture m_RenderTexture;

        public Transform cameraRoot
        {
            get { return m_Camera.transform; }
        }

        private void Awake()
        {
            m_RenderTexture = m_Config.Allocate();
            m_Camera.targetTexture = m_RenderTexture;
            m_Camera.ResetAspect();

            m_Image.texture = m_RenderTexture;

            ModelShow3DMgr.S.AddModelShow(this);
        }

        protected virtual void OnDestroy()
        {
            if (m_RenderTexture != null)
            {
                RenderTexture.ReleaseTemporary(m_RenderTexture);
                m_RenderTexture = null;
            }

            if (m_Camera != null)
            {
                GameObject.Destroy(m_Camera.gameObject);
                m_Camera = null;
            }
        }
    }
}
