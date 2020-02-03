using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using UnityEngine.UI;

namespace Qarth
{
    public class ButtonCommand : AbstractGuideCommand
    {
        private IUINodeFinder m_Finder;
        private Button m_TargetButton;

        public override void SetParam(object[] param)
        {
            m_Finder = param[0] as IUINodeFinder;
        }

        protected override void OnStart()
        {
            var target = m_Finder.FindNode(false);

            if (target == null)
            {
                return;
            }

             m_TargetButton = target.GetComponent<Button>();

            if (m_TargetButton == null)
            {
                return;
            }

            m_TargetButton.onClick.AddListener(OnClickTargetButton);
        }

        protected override void OnFinish(bool forceClean)
        {
            if (m_TargetButton != null)
            {
                m_TargetButton.onClick.RemoveListener(OnClickTargetButton);
                m_TargetButton = null;
            }
        }

        protected void OnClickTargetButton()
        {
            m_TargetButton.onClick.RemoveListener(OnClickTargetButton);
            m_TargetButton = null;
            FinishStep();
        }
    }
}
