using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField]
        private Transform m_Target;
        [SerializeField]
        private Vector3 m_Offset;
        [SerializeField]
        private Vector3 m_Axis = new Vector3(1,1,1);
        [SerializeField]
        private bool m_LookAtTarget = false;

        public Transform target
        {
            get { return m_Target; }
            set { m_Target = value; LateUpdate(); }
        }

        private void LateUpdate()
        {
            if (m_Target == null)
            {
                return;
            }

            Vector3 pos = m_Target.position;
            transform.position =  new Vector3(pos.x * m_Axis.x, pos.y * m_Axis.y, pos.z * m_Axis.z) + m_Offset;

            if (m_LookAtTarget)
            {
                transform.rotation = Quaternion.LookRotation(-m_Offset);
            }
        }

        protected void OnValidate()
        {
            if (m_Target == null)
            {
                return;
            }

            transform.position = m_Target.position + m_Offset;
            if (m_LookAtTarget)
            {
                transform.rotation = Quaternion.LookRotation(-m_Offset);
            }
        }
    }
}
