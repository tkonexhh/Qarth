using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class AsyncSpriteRenderer : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer m_SpriteRenderer;
        [SerializeField]
        private string m_SpriteName;
        [SerializeField]
        private bool m_IsAsync = false;

        private ResLoader m_Loader;

        private void Awake()
        {
            if (m_SpriteRenderer == null)
            {
                m_SpriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (m_SpriteRenderer == null)
            {
                return;
            }

            Load(m_SpriteName);
        }

        public void Load(string sprite)
        {
            if (m_SpriteRenderer == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(sprite))
            {
                return;
            }

            if (m_Loader != null)
            {
                m_Loader.ReleaseAllRes();
            }
            else
            {
                m_Loader = ResLoader.Allocate("AsyncSpriteRenderer");
            }

            m_SpriteName = sprite;

            m_Loader.Add2Load(sprite, (result, res) =>
            {
                if (result)
                {
                    m_SpriteRenderer.sprite = res.asset as Sprite;
                }
            });

            if (m_IsAsync)
            {
                m_Loader.LoadAsync();
            }
            else
            {
                m_Loader.LoadSync();
            }
        }

        protected void OnDestroy()
        {
            if (m_Loader != null)
            {
                m_Loader.Recycle2Cache();
                m_Loader = null;
            }
        }
    }
}
