using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Qarth
{
    public class DeviceID
    {
        public static string Get()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            string android_id = "";
            try
            {
                AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");
                AndroidJavaClass secure = new AndroidJavaClass("android.provider.Settings$Secure");
                android_id = secure.CallStatic<string>("getString", contentResolver, "android_id");
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            if (string.IsNullOrEmpty(android_id))
                android_id = "android";
            return string.Concat(android_id, "_", SystemInfo.deviceUniqueIdentifier);
#endif
            return SystemInfo.deviceUniqueIdentifier;
        }
    }

}