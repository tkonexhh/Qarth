using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG;
using DG.Tweening;

namespace Qarth
{
    public class RotateForever : MonoBehaviour
    {
        [SerializeField]
        private Transform m_object;
        [SerializeField]
        private float m_Duration = 5;

        public void OnEnable()
        {
            StartAnim(m_object);
        }

        public void StartAnim(Transform m_Transform) {
            if (m_Transform==null)
            {
                m_Transform = this.transform;
            }
            m_Transform.DORotate(new Vector3(0, 0, -360), m_Duration, RotateMode.LocalAxisAdd)
                .SetLoops(-1)
                .SetEase(Ease.Linear);
        }
    }
}