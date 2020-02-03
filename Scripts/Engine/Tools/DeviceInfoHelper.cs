using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Qarth
{
    public static class DeviceInfoHelper
    {

#if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern string _GetIDFA();

        [DllImport("__Internal")]
        private static extern bool _HasIDFA();
#endif

        public static string GetIDFA()
        {
#if UNITY_IOS && !UNITY_EDITOR
            string idfa = _GetIDFA();
            if (string.IsNullOrEmpty(idfa))
            {
                Log.e("IDFA is NULL");
                return "";
            }
            return idfa;
#else
            return "";
#endif
        }

        public static string GetDeviceModel()
        {
            return SystemInfo.deviceModel;
        }

        public static string GetUUID()
        {
            return SystemInfo.deviceUniqueIdentifier;
        }

        public static bool HasIDFA()
        {
#if UNITY_IOS && !UNITY_EDITOR
            return _HasIDFA();
#else
            return false;
#endif
        }
    }
}