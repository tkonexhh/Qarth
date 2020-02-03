using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class I18Mgr : TSingleton<I18Mgr>
    {
        private const string LAN_KEY = "lan_key";

        private SystemLanguage m_Language = SystemLanguage.Unknown;
        private string m_LanguagePrefix;

        public string langugePrefix
        {
            get { return m_LanguagePrefix; }
        }

        public SystemLanguage language
        {
            get { return m_Language; }
        }

        public void Init()
        {
            CheckLanguageEnvironment();
            Log.i("Init[I18Mgr]");
        }

        public void SwitchLanguage(SystemLanguage language)
        {
            if (language == m_Language)
            {
                return;
            }

            m_Language = language;

            PlayerPrefs.SetInt(LAN_KEY, (int)m_Language);

            CheckLanguageEnvironment();

            //
            EventSystem.S.Send(EngineEventID.OnLanguageChange);
        }

        private void CheckLanguageEnvironment()
        {
            int preLan = PlayerPrefs.GetInt(LAN_KEY, -1);

            if (preLan >= 0)
            {
                m_Language = (SystemLanguage)preLan;
            }
            else
            {
                m_Language = Application.systemLanguage;
            }

            switch (m_Language)
            {
                case SystemLanguage.Afrikaans: SetLang("af"); break;
                case SystemLanguage.Arabic: SetLang("ar"); break;
                case SystemLanguage.Basque: SetLang("eu"); break;
                case SystemLanguage.Belarusian: SetLang("be"); break;
                case SystemLanguage.Bulgarian: SetLang("bg"); break;
                case SystemLanguage.Catalan: SetLang("ca"); break;
                case SystemLanguage.Chinese: SetLang("zh"); break;
                case SystemLanguage.Czech: SetLang("cs"); break;
                case SystemLanguage.Danish: SetLang("da"); break;
                case SystemLanguage.Dutch: SetLang("nl"); break;
                case SystemLanguage.English: SetLang("en"); break;
                case SystemLanguage.Estonian: SetLang("et"); break;
                case SystemLanguage.Faroese: SetLang("fo"); break;
                case SystemLanguage.Finnish: SetLang("fu"); break;
                case SystemLanguage.French: SetLang("fr"); break;
                case SystemLanguage.German: SetLang("de"); break;
                case SystemLanguage.Greek: SetLang("el"); break;
                case SystemLanguage.Hebrew: SetLang("he"); break;
                case SystemLanguage.Icelandic: SetLang("is"); break;
                case SystemLanguage.Indonesian: SetLang("id"); break;
                case SystemLanguage.Italian: SetLang("it"); break;
                case SystemLanguage.Japanese: SetLang("ja"); break;
                case SystemLanguage.Korean: SetLang("ko"); break;
                case SystemLanguage.Latvian: SetLang("lv"); break;
                case SystemLanguage.Lithuanian: SetLang("lt"); break;
                case SystemLanguage.Norwegian: SetLang("nn"); break; // TODO: Check  
                case SystemLanguage.Polish: SetLang("pl"); break;
                case SystemLanguage.Portuguese: SetLang("pt"); break;
                case SystemLanguage.Romanian: SetLang("ro"); break;
                case SystemLanguage.Russian: SetLang("ru"); break;
                case SystemLanguage.SerboCroatian: SetLang("sr"); break; // TODO: Check  
                case SystemLanguage.Slovak: SetLang("sk"); break;
                case SystemLanguage.Slovenian: SetLang("sl"); break;
                case SystemLanguage.Spanish: SetLang("es"); break;
                case SystemLanguage.Swedish: SetLang("sv"); break;
                case SystemLanguage.Thai: SetLang("th"); break;
                case SystemLanguage.Turkish: SetLang("tr"); break;
                case SystemLanguage.Ukrainian: SetLang("uk"); break;
                case SystemLanguage.Vietnamese: SetLang("vi"); break;
                case SystemLanguage.ChineseSimplified: SetLang("zh_Hans"); break;
                case SystemLanguage.ChineseTraditional: SetLang("tw_Hant"); break;
                case SystemLanguage.Hungarian: SetLang("hu"); break;
                case SystemLanguage.Unknown:
                    {
                        m_Language = SystemLanguage.English;
                        SetLang("en"); break; // Unknow Fallback to defaultLang  
                    }
            }
        }

        private void SetLang(string lan)
        {
            int indexA = lan.IndexOf('_');
            if (indexA > 0)
            {
                lan = lan.Substring(0, indexA);
            }

            m_LanguagePrefix = lan.ToLower();
        }
    }
}
