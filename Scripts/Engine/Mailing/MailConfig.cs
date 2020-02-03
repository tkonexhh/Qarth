using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qarth
{
    [System.Serializable]
    public class MailConfig : ScriptableObject
    {
        private const string MAIL_CONFIG_PATH = "Resources/Config/MailConfig";
        public enum MailClientType
        {
            HOTMAIL,
            GMAIL,
            CUSTOM,
        }

        private static MailConfig s_Instance;

        #region 序列化区域

        [SerializeField]
        private string m_MailSender = "vega4games@hotmail.com";

        [SerializeField]
        private string m_MailPWD = "Vega2018";
        [SerializeField]
        private string m_MailReceiver = "";

        [SerializeField]
        private bool m_EnableSSL = true;

        [SerializeField]
        private int m_SmtpPort = 587;

        // [SerializeField]
        // private string m_AttachmentPath = "";

        [SerializeField]
        private MailClientType m_MailClient;

        [SerializeField]
        private string m_CustomSmtp = "";


        #endregion


        private static string ResourcesPath2Path(string path)
        {
            return path.Substring(10);
        }

        private static MailConfig LoadInstance()
        {
            UnityEngine.Object obj = Resources.Load(ResourcesPath2Path(MAIL_CONFIG_PATH));
            if (obj == null)
            {
                Log.e("Not Find Mail Config File.");
                return null;
            }
            s_Instance = obj as MailConfig;
            return s_Instance;
        }

        public static MailConfig S
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = LoadInstance();
                }
                return s_Instance;
            }
        }

        public static string mailSender
        {
            get
            {
                return S.m_MailSender;
            }
        }

        public static string mailPWD
        {
            get
            {
                return S.m_MailPWD;
            }
        }

        public static string mailReceiver
        {
            get
            {
                return S.m_MailReceiver;
            }
        }

        public static int smtpPort
        {
            get
            {
                return S.m_SmtpPort;
            }
        }

        public static bool enableSSL
        {
            get
            {
                return S.m_EnableSSL;
            }
        }

        public static MailClientType mailClient
        {
            get
            {
                return S.m_MailClient;
            }
        }
        public static string customSmtp
        {
            get
            {
                return S.m_CustomSmtp;
            }
        }

    }
}
