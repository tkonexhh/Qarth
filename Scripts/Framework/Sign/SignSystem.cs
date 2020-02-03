using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class SignSystem : TSingleton<SignSystem>
    {

        private SignState m_WeekSignState;
        //private SignState m_MonthSignState;

        public class SignState
        {
            private string m_LastSignDayKey;
            private string m_LastSignDayIndexKey;
            private string m_Name;

            private int m_LastSignIndex;
            private int m_MaxSignIndex;
            private int m_SignAbleIndex;

            public SignState(string name, int maxIndex)
            {
                m_Name = name;
                m_MaxSignIndex = maxIndex;
                m_LastSignDayIndexKey = string.Format("signindex_key_{0}_1298", m_Name);
                m_LastSignDayKey = string.Format("sign_key_{0}_1298", m_Name);

                Check();
            }

            public void Check()
            {
                Load();
            }

            public bool isSignAble
            {
                get
                {
                    return m_SignAbleIndex >= 0;
                }
            }

            public int lastSignIndex
            {
                get
                {
                    return m_LastSignIndex;
                }
            }

            public int signAbleIndex
            {
                get
                {
                    return m_SignAbleIndex;
                }
            }

            public int maxSignIndex
            {
                get
                {
                    return m_MaxSignIndex;
                }
            }

            public void Reset2NextSignDay()
            {
                PlayerPrefs.SetString(m_LastSignDayKey, DateTime.Today.ToShortDateString());
                Load();
            }

            public void Reset()
            {
                m_SignAbleIndex = 0;
                m_LastSignIndex = -1;
                Save();
            }

            public bool Sign()
            {
                if (!isSignAble)
                {
                    return false;
                }

                Save();
                m_LastSignIndex = m_SignAbleIndex;
                m_SignAbleIndex = -1;

                return true;
            }

            private void Save()
            {
                PlayerPrefs.SetInt(m_LastSignDayIndexKey, m_SignAbleIndex);
                PlayerPrefs.SetString(m_LastSignDayKey, DateTime.Today.ToShortDateString());
            }

            private void Load()
            {
                m_LastSignIndex = PlayerPrefs.GetInt(m_LastSignDayIndexKey, -1);
                string timeString = PlayerPrefs.GetString(m_LastSignDayKey, "");

                DateTime lastSignDate;
                if (!string.IsNullOrEmpty(timeString))
                {
                    if (DateTime.TryParse(timeString, out lastSignDate))
                    {
                        DateTime today = DateTime.Today;
                        TimeSpan pass = today - lastSignDate;

                        if (pass.Days < 1)
                        {
                            m_SignAbleIndex = -1;
                        }
                        else if (pass.Days == 1)
                        {
                            m_SignAbleIndex = m_LastSignIndex + 1;
                            if (m_SignAbleIndex > m_MaxSignIndex)
                            {
                                Reset();
                            }
                        }
                        else
                        {
                            Reset();
                        }
                    }
                    else
                    {
                        Reset();
                    }
                }
                else
                {
                    Reset();
                }
            }
        }

        public void InitSignSystem()
        {
            EventSystem.S.Register(EngineEventID.OnDateUpdate, OnPassDayEvent);
        }

        private void OnPassDayEvent(int key, params object[] args)
        {
            weekSignState.Check();
            EventSystem.S.Send(EngineEventID.OnSignStateChange);
        }

        public SignState weekSignState
        {
            get
            {
                if (m_WeekSignState == null)
                {
                    m_WeekSignState = new SignState("week", 6);
                }

                return m_WeekSignState;
            }
        }
    }
}
