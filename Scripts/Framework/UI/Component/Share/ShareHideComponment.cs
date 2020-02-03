using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class ShareHideComponment : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] m_Target;

        private void Awake()
        {
            EventSystem.S.Register(EngineEventID.OnShareCaptureBegin, OnShareCaptureBegin);
            EventSystem.S.Register(EngineEventID.OnShareCaptureEnd, OnShareCaptureEnd);
        }

        private void OnDestroy()
        {
            EventSystem.S.UnRegister(EngineEventID.OnShareCaptureBegin, OnShareCaptureBegin);
            EventSystem.S.UnRegister(EngineEventID.OnShareCaptureEnd, OnShareCaptureEnd);
        }

        private void OnShareCaptureBegin(int key, params object[] args)
        {
            SetVisibleState(false);
        }

        private void OnShareCaptureEnd(int key, params object[] args)
        {
            SetVisibleState(true);
        }

        private void SetVisibleState(bool state)
        {
            if (m_Target == null || m_Target.Length == 0)
            {
                gameObject.SetActive(state);
            }
            else
            {
                for (int i = m_Target.Length - 1; i >= 0; --i)
                {
                    m_Target[i].SetActive(state);
                }
            }
        }
    }
}
