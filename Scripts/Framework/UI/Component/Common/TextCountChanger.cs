using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace Qarth
{
    public class TextCountChanger : MonoBehaviour
    {
        [SerializeField]
        protected Text m_Label;

        private int m_Count = -1;
        private Sequence m_Tweener;

        protected virtual void Awake()
        {
            if (m_Label == null)
            {
                m_Label = GetComponent<Text>();
            }
        }

        public void SetCount(int count, bool anim = true)
        {
            if (m_Count > 0 && anim)
            {
                PlayPopAnim();
            }

            m_Count = count;
            m_Label.text = count.ToString();
        }

        protected void PlayPopAnim()
        {
            if (m_Tweener != null)
            {
                m_Tweener.Kill();
                m_Tweener = null;
            }

            m_Tweener = DOTween.Sequence()
                .Append(transform.DOScale(1.2f, 0.3f))
                .Append(transform.DOScale(1.0f, 0.2f))
                .OnComplete(OnTweenComplate);
        }

        protected void OnTweenComplate()
        {
            m_Tweener = null;
        }
    }
}
