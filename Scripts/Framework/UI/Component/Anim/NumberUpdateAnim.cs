using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Qarth
{
    
    public class NumberUpdateAnim : MonoBehaviour
    {
        [SerializeField]
        private Text m_ShowNumberLabel;

        private BigInteger m_Number;
        private BigInteger m_TargetNumber;
        private BigInteger m_StepValue;
        private bool m_IsChange;
        private float m_Duration;
        private float m_Passtime;
        private int m_StepCount;
        private string m_Unit;

        private void Awake()
        {
            if (m_ShowNumberLabel == null)
            {
                m_ShowNumberLabel = GetComponent<Text>();
            }
        }

        public void InitLabel(BigInteger number, string unit = "")
        {
            m_Number = number;
            m_Unit = unit;
            if (m_ShowNumberLabel != null)
            {
                m_ShowNumberLabel.text = unit + number.ToString();
            }
        }

        public void SetTargetValue(BigInteger num,float dur = 0.5f)
        {
            if (num == m_Number)
            {
                return;
            }

            if (!m_IsChange)
            {
                m_Duration = dur;
                m_TargetNumber = num;
                m_IsChange = true;
                m_StepValue = m_TargetNumber - m_Number;
            }
            else
            {
                m_TargetNumber = num;
                //if (num < m_Number)
                //{
                //    m_IsChange = false;
                //    m_Passtime = 0;
                //    m_Number = m_TargetNumber;
                //    m_ShowNumberLabel.text = m_Number.ToString();
                //}
            }
        }

        private void Update()
        {
            if (m_IsChange)
            {
                float time = Time.deltaTime;
                m_Passtime += time;
                m_StepCount = (int)(m_Duration / time);
                m_Number = m_Number + (m_StepValue / m_StepCount);
                m_ShowNumberLabel.text = m_Unit + (m_Number).ToString();
                if (m_Passtime >= m_Duration)
                {
                    m_Number = m_TargetNumber;
                    m_ShowNumberLabel.text = m_Unit + m_Number.ToString();
                    m_Passtime = 0;
                    m_IsChange = false;
                }
            }
        }
    }
}