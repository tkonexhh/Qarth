using UnityEngine;
using System.Runtime.InteropServices;

namespace Qarth
{
    public class UniClipboard
    {
        static IClipBoard _board;
        static IClipBoard board
        {
            get
            {
                if (_board == null)
                {
#if UNITY_EDITOR
                    _board = new EditorClipBoard();
#elif UNITY_ANDROID
                    _board = new AndroidClipBoard();
#elif UNITY_IOS
                    _board = new IOSClipBoard();
#endif
                }
                return _board;
            }
        }

        public static void SetText(string str)
        {
            board.SetText(str);
        }

        public static string GetText()
        {
            return board.GetText();
        }
    }

    interface IClipBoard
    {
        void SetText(string str);
        string GetText();
    }

    class EditorClipBoard : IClipBoard
    {
        public void SetText(string str)
        {
            Debug.Log("Copied: " + str);
            GUIUtility.systemCopyBuffer = str;
        }

        public string GetText()
        {
            return GUIUtility.systemCopyBuffer;
        }
    }

#if UNITY_IOS
    class IOSClipBoard : IClipBoard
    {
        [DllImport("__Internal")]
        static extern void SetText_(string str);
        [DllImport("__Internal")]
        static extern string GetText_();

        public void SetText(string str)
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                SetText_(str);
            }
        }

        public string GetText()
        {
            return GetText_();
        }
    }
#endif

#if UNITY_ANDROID
    class AndroidClipBoard : IClipBoard
    {

        AndroidJavaClass cb = new AndroidJavaClass("jp.ne.donuts.uniclipboard.Clipboard");

        public void SetText(string str)
        {
            FloatMessage.S.ShowMsg("已复制");
            cb.CallStatic("setText", str);
        }

        public string GetText()
        {
            return cb.CallStatic<string>("getText");
        }
    }
#endif
}