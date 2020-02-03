using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace Qarth
{
    public class ScrollRectAutoAdjustPosition : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField]
        protected Layout m_Layout;
        [SerializeField]
        protected ScrollRect m_ScrollRect;

        private DG.Tweening.Tweener m_AdjustTween;
        protected int m_Count;
        private float m_CellPrecent;

        public void EnableAutoAdjust(int count)
        {
            m_Count = count;
            m_CellPrecent = 1.0f / (count - 1);
        }

        public void Move2Next()
        {
            float targetValue = 0;

            switch (m_Layout)
            {
                case Layout.Horizontal:
                    targetValue = m_ScrollRect.horizontalNormalizedPosition;
                    break;
                case Layout.Vertical:
                    targetValue = m_ScrollRect.verticalNormalizedPosition;
                    break;
            }

            targetValue += m_CellPrecent;

            Move2Target(targetValue);
        }


        public void Move2Pre()
        {
            float targetValue = 0;

            switch (m_Layout)
            {
                case Layout.Horizontal:
                    targetValue = m_ScrollRect.horizontalNormalizedPosition;
                    break;
                case Layout.Vertical:
                    targetValue = m_ScrollRect.verticalNormalizedPosition;
                    break;
            }

            targetValue -= m_CellPrecent;

            Move2Target(targetValue);
        }

        protected Vector2 m_BeginPosition;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (m_AdjustTween != null)
            {
                m_AdjustTween.Kill();
                m_AdjustTween = null;
            }

            m_BeginPosition = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Vector2 lastPosition = eventData.position;

            switch (m_Layout)
            {
                case Layout.Horizontal:
                    {
                        float currentValue = m_ScrollRect.horizontalNormalizedPosition;
                        float x = m_BeginPosition.x - lastPosition.x;
                        Move2Target(currentValue + x * 0.002f);
                    }
                    break;
                case Layout.Vertical:
                    {
                        float currentValue = m_ScrollRect.horizontalNormalizedPosition;
                        float y = m_BeginPosition.y - lastPosition.y;
                        Move2Target(currentValue + y * 0.001f);
                    }
                    break;
            }
        }


        protected void Move2Target(float precent)
        {
            switch (m_Layout)
            {
                case Layout.Horizontal:
                    {
                        precent = Mathf.Max(0, Mathf.Min(1.0f, precent));
                        float xV = precent * (m_Count - 1);
                        int xxv = Mathf.RoundToInt(xV);
                        float target = xxv * m_CellPrecent;
                        m_ScrollRect.DoScrollHorizontal(target, 0.25f);
                    }
                    break;
                case Layout.Vertical:
                    {
                        precent = Mathf.Max(0, Mathf.Min(1.0f, precent));
                        float xV = precent * (m_Count - 1);
                        int xxv = Mathf.RoundToInt(xV);
                        float target = xxv * m_CellPrecent;
                        m_ScrollRect.DoScrollVertical(target, 0.25f);
                    }
                    break;
            }
        }

    }
}
