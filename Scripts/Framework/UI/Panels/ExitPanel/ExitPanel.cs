using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Qarth
{
    public class ExitPanel : AbstractPanel
    {
        [SerializeField]
        private Button m_OkButton;
        [SerializeField]
        private Button m_CancelButton;

        protected override void OnUIInit()
        {
            m_OkButton.onClick.AddListener(OnClickOKButton);
            m_CancelButton.onClick.AddListener(CloseSelfPanel);
        }

        protected override void OnOpen()
        {
            EventSystem.S.Send(EngineEventID.OnNeedHideBanner);
        }

        protected override void OnPanelOpen(params object[] args)
        {
            OpenDependPanel(EngineUI.MaskPanel, -1);
        }

        protected override void OnClose()
        {
            EventSystem.S.Send(EngineEventID.OnNeedShowBanner);
        }

        private void OnClickOKButton()
        {
            Application.Quit();
        }

        public override BackKeyCodeResult OnBackKeyDown()
        {
            CloseSelfPanel();
            return BackKeyCodeResult.PROCESS_AND_BLOCK;
        }
    }
}
