using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using UnityEngine.UI;

namespace Qarth
{
    public class DelayCommand : AbstractGuideCommand
    {
        private int m_Timer = -1;
        private float m_Delay;
        public override void SetParam(object[] param)
        {
            if (param != null && param.Length > 0)
            {
                m_Delay = float.Parse(param[0].ToString());
            }
        }

        protected override void OnStart()
        {
            if (m_Timer > 0)
            {
                Timer.S.Cancel(m_Timer);
            }
            m_Timer = Timer.S.Post2Scale(OnTimerCount, m_Delay);
        }

        protected override void OnFinish(bool forceClean)
        {
            if (m_Timer > 0)
            {
                Timer.S.Cancel(m_Timer);
                m_Timer = -1;
            }
        }

        protected void OnTimerCount(int count)
        {
            FinishStep();
        }
    }
}
