using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;


namespace Qarth
{
    public class CustomPopopMeau : MonoBehaviour
    {
        enum eState
        {
            kNone,
            kPull,
            kPop,
        }

        [SerializeField]
        private Vector3 m_Direction = Vector3.up;
        [SerializeField]
        private Transform m_ContextRoot;
        [SerializeField]
        private float m_AnimSpeed = 200;
        [SerializeField]
        private Button m_ControllerButton;

        private eState m_CurrentState = eState.kNone;
        private int m_ButtonCount;


        private void Awake()
        {
            m_ButtonCount = m_ContextRoot.childCount;
            m_ControllerButton.onClick.AddListener(OnClickSelf);
            Pull();
        }

        public void Pop(bool anim = true)
        {
            if (m_ContextRoot == null)
            {
                return;
            }

            Vector3 nextPos = new Vector3(m_Direction.x * 4, m_Direction.y * 4, 0);

            m_CurrentState = eState.kPop;

            Vector3 dir = Vector3.zero;

            int childCount = m_ContextRoot.childCount;

            float duration = 0;
            for (int i = childCount - 1; i >= 0; --i)
            {
                int order = childCount - i - 1;
                RectTransform tr = m_ContextRoot.GetChild(i) as RectTransform;

                tr.DOKill();
                if (order >= m_ButtonCount)
                {
                    tr.gameObject.SetActive(false);
                    continue;
                }

                Vector3 curPos = tr.localPosition;
                dir.x = tr.rect.width;
                dir.y = tr.rect.height;

                Vector3 offsetPos = new Vector3(dir.x * m_Direction.x, dir.y * m_Direction.y, 0);

                nextPos += offsetPos;
                Vector3 moveOffset = nextPos - curPos;

                duration = moveOffset.magnitude / m_AnimSpeed;

                tr.DOLocalMove(nextPos, duration);
                tr.gameObject.SetActive(true);
            }

        }

        public void Pull(bool anim = true)
        {
            if (m_ContextRoot == null)
            {
                return;
            }

            m_CurrentState = eState.kPull;

            int childCount = m_ContextRoot.childCount;
            float duration = 0;

            for (int i = childCount - 1; i >= 0; --i)
            {
                int order = childCount - i - 1;

                Transform tr = m_ContextRoot.GetChild(i);

                tr.DOKill();
                if (order >= m_ButtonCount)
                {
                    tr.gameObject.SetActive(false);
                    continue;
                }

                Vector3 curPos = tr.localPosition;

                duration = Mathf.Abs(curPos.magnitude) / m_AnimSpeed;

                Tweener tw = tr.DOLocalMove(Vector3.zero, duration);
                tw.OnComplete(() =>
                {
                    tr.gameObject.SetActive(false);
                });
            }

        }

        private void OnClickSelf()
        {
            switch (m_CurrentState)
            {
                case eState.kPop:
                    Pull();
                    break;
                case eState.kNone:
                case eState.kPull:
                    Pop();
                    break;
                default:
                    break;
            }
        }


    }
}