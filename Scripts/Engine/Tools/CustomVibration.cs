using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace Qarth
{
    public static class CustomVibration
    {

#if UNITY_IOS && !UNITY_EDITOR

        [DllImport("__Internal")]
        private static extern bool _HasVibrator();

        [DllImport("__Internal")]
        private static extern void _Vibrate();

        [DllImport("__Internal")]
        private static extern void _VibratePop();

        [DllImport("__Internal")]
        private static extern void _VibratePeek();

        [DllImport("__Internal")]
        private static extern void _VibrateNope();
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
        public static AndroidJavaClass unityPlayer;
        public static AndroidJavaObject currentActivity;
        public static AndroidJavaObject vibrator;
#endif

        public static void Vibrate()
        {
#if UNITY_EDITOR
            Handheld.Vibrate();
#elif UNITY_ANDROID
        vibrator.Call("vibrate");
#elif UNITY_IOS
        _Vibrate();
#endif
        }


        public static void Vibrate(long milliseconds)
        {
#if UNITY_EDITOR
            Handheld.Vibrate();
#elif UNITY_ANDROID
        vibrator.Call("vibrate", milliseconds);
#elif UNITY_IOS
        if(milliseconds > 100)
            _VibratePop();
        else
            _VibratePeek();
#endif
        }

        public static void Vibrate(long[] pattern, int repeat)
        {
#if UNITY_EDITOR
            Handheld.Vibrate();
#elif UNITY_ANDROID
        vibrator.Call("vibrate", pattern, repeat);
#elif UNITY_IOS
        _VibrateNope();
#endif
        }

        public static bool HasVibrator()
        {
#if UNITY_EDITOR
            return false;
#elif UNITY_ANDROID
        return true;
#elif UNITY_IOS
        return _HasVibrator();
#else
        return false;
#endif
        }

        public static void Cancel()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        vibrator.Call("cancel");
#endif
        }
    }
}