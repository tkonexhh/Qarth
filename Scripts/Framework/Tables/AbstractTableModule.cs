using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class AbstractTableModule : AbstractModule
    {
        private bool m_IsTableLoadFinish = false;
        private string m_CurrentLanguage;
        public bool isTableLoadFinish
        {
            get { return m_IsTableLoadFinish; }
        }

        public void SwitchLanguage(string key)
        {
            TDTableMetaData[] tables = new TDTableMetaData[]
            {
                TDLanguageTable.GetLanguageMetaData(),
            };

            m_IsTableLoadFinish = false;
            actor.StartCoroutine(TableMgr.S.ReadAll(tables, OnLanguageSwitchLoadFinish));
        }

        public void ReloadTable(TDTableMetaData[] tables, Action callback)
        {
            actor.StartCoroutine(TableMgr.S.ReadAll(tables, callback));
        }

        private string FormatTableName(string rawName, string key)
        {
            return string.Format("{0}_{1}", rawName, key);
        }

        protected override void OnComAwake()
        {
            InitPreLoadTableMetaData();
            //InitDelayLoadTableMetaData();

            m_IsTableLoadFinish = false;
            actor.StartCoroutine(TableMgr.S.PreReadAll(HandleTableLoadFinish));
            EventSystem.S.Register(EngineEventID.OnLanguageChange, OnLanguageSwitch);
        }

        protected void OnLanguageSwitch(int key, params object[] args)
        {
            SwitchLanguage(I18Mgr.S.langugePrefix);
        }

        private void OnLanguageSwitchLoadFinish()
        {
            m_IsTableLoadFinish = true;
            Log.i("OnLanguageSwitchLoadFinish.");

            AutoTranslation.ReTranslationAll();

            EventSystem.S.Send(EngineEventID.OnLanguageTableSwitchFinish);
        }

        protected void HandleTableLoadFinish()
        {
            OnTableLoadFinish();
            m_IsTableLoadFinish = true;
        }

        protected virtual void OnTableLoadFinish()
        {

        }

        protected virtual void InitPreLoadTableMetaData()
        {

        }
    }
}
