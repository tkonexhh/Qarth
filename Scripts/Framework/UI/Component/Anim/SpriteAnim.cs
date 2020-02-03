using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using UnityEngine.UI;

namespace Qarth
{
    public class SpriteAnim : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer m_TargetImage;
        [SerializeField]
        private String[] m_SpritesNames;
        [SerializeField]
        private float m_Duration = 1.0f;

        private Sprite[] m_SpritesArray;
        private int m_TimerID;
        private int m_CurrentIndex = 0;

        private ResLoader m_ResLoader;

        public SpriteRenderer targetImage
        {
            get
            {

                if (m_TargetImage == null)
                {
                    m_TargetImage = GetComponent<SpriteRenderer>();
                }
                return m_TargetImage;
             }
        }

        public string[] spriteNames
        {
            get { return m_SpritesNames; }
            set
            {
                m_SpritesNames = value;

                UpdateSprites();
            }
        }

        private void Awake()
        {
            UpdateSprites();
        }

        private void UpdateSprites()
        {
            CleanPreRes();
            if (targetImage == null || m_SpritesNames == null || m_SpritesNames.Length == 0)
            {
                return;
            }

            m_ResLoader = ResLoader.Allocate();
            m_ResLoader.Add2Load(m_SpritesNames);

            m_ResLoader.LoadSync();
            m_SpritesArray = new Sprite[m_SpritesNames.Length];

            for (int i = 0; i < m_SpritesNames.Length; ++i)
            {
                m_SpritesArray[i] = ResMgr.S.GetAsset<Sprite>(m_SpritesNames[i]);
            }

            OnTimeReach(0);
            m_TimerID = Timer.S.Post2Scale(OnTimeReach, m_Duration, -1);
        }

        private void OnTimeReach(int count)
        {
            m_CurrentIndex = (++m_CurrentIndex) % m_SpritesArray.Length;
            m_TargetImage.sprite = m_SpritesArray[m_CurrentIndex];
        }

        private void CleanPreRes()
        {
            if (m_ResLoader != null)
            {
                m_ResLoader.Recycle2Cache();
                m_ResLoader = null;
            }

            if (m_TimerID > 0)
            {
                Timer.S.Cancel(m_TimerID);
                m_TimerID = 0;
                return;
            }
        }

        private void OnDestroy()
        {
            if (MonoSingleton.isApplicationQuit)
            {
                return;
            }

            CleanPreRes();
        }
    }
}
