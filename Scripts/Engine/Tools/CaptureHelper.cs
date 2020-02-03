//  Desc:        Framework For Game Develop with Unity3d
//  Copyright:   Copyright (C) 2017 SnowCold. All rights reserved.
//  WebSite:     https://github.com/SnowCold/Qarth
//  Blog:        http://blog.csdn.net/snowcoldgame
//  Author:      SnowCold
//  E-mail:      snowcold.ouyang@gmail.com
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Qarth
{
    public class CaptureHelper
    {
        //Capture:?cameraTag&save&
        //LocalResp

        private static string m_PersistentDataPath4Capture;
        // 外部资源目录
        public static string persistentDataPath4Capture
        {
            get
            {
                if (null == m_PersistentDataPath4Capture)
                {
                    m_PersistentDataPath4Capture = FilePath.persistentDataPath + "/Capture/";

                    if (!Directory.Exists(m_PersistentDataPath4Capture))
                    {
                        Directory.CreateDirectory(m_PersistentDataPath4Capture);
#if UNITY_IPHONE && !UNITY_EDITOR
                        UnityEngine.iOS.Device.SetNoBackupFlag(m_PersistentDataPath4Capture);
#endif
                    }
                }

                return m_PersistentDataPath4Capture;
            }
        }

        private static string GetTextureSavePath(Camera camera)
        {
            string name = string.Format("{0}{1}{2}", camera.name, Time.frameCount, Application.identifier);
            name = name.GetHashCode() + ".jpg";
            return string.Format("{0}{1}", persistentDataPath4Capture, name);
        }

        private static string GetTextureSavePath(string prefixName)
        {
            string name = string.Format("{0}{1}", prefixName, Application.identifier);
            name = name.GetHashCode() + ".jpg";
            return string.Format("{0}{1}", persistentDataPath4Capture, name);
        }

        public static IEnumerator CaptureAsUITransform(string prefixName, Transform leftBottom, Transform topRight, Action<bool, string> callBack, bool sendEvent)
        {
            Camera camera = UIMgr.S.uiRoot.uiCamera;
            Vector3 screenPos = camera.WorldToScreenPoint(leftBottom.position);
            Vector3 screenPos2 = camera.WorldToScreenPoint(topRight.position);

            float width = screenPos2.x - screenPos.x;
            float height = screenPos2.y - screenPos.y;
            if (sendEvent)
            {
                EventSystem.S.Send(EngineEventID.OnShareCaptureBegin);
            }
            yield return CaptureScreenTexture(prefixName, (int)screenPos.x, (int)screenPos.y, (int)width, (int)height, callBack, sendEvent);
        }

        public static IEnumerator CaptureScreenTexture(string prefixName, int capX, int capY, int width, int height, Action<bool, string> callBack, bool sendEvent)
        {
            yield return new WaitForEndOfFrame();

            Texture2D t = new Texture2D(width, height, TextureFormat.RGB24, true);
            t.ReadPixels(new Rect(capX, capY, width, height), 0, 0, false);
            t.Apply();

            if (sendEvent)
            {
                EventSystem.S.Send(EngineEventID.OnShareCaptureEnd);
            }

            byte[] byt = t.EncodeToJPG();
            string fileName = GetTextureSavePath(prefixName);
            try
            {
                System.IO.File.WriteAllBytes(fileName, byt);
                if (callBack != null)
                {
                    callBack(true, fileName);
                }
            }
            catch (Exception e)
            {
                Log.e(e);
                if (callBack != null)
                {
                    callBack(false, fileName);
                }
            }
        }

        public static IEnumerator Capture(Camera camera, bool autoSave, Action<Texture2D, string> listener)
        {
            if (listener == null || camera == null)
            {
                yield break;
            }

            yield return new WaitForEndOfFrame();

            Rect rect = new Rect(0, 0, Screen.width, Screen.height);

            RenderTexture renderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height);

            camera.targetTexture = renderTexture;

            camera.Render();

            RenderTexture.active = renderTexture;
            Texture2D tex = new Texture2D(Screen.width, Screen.height);

            tex.ReadPixels(rect, 0, 0);

            tex.Apply();

            camera.targetTexture = null;
            RenderTexture.active = null;

            RenderTexture.ReleaseTemporary(renderTexture);

            string savePath = null;
            if (autoSave)
            {
                try
                {
                    byte[] bytes = tex.EncodeToJPG();
                    savePath = GetTextureSavePath(camera);
                    File.WriteAllBytes(savePath, bytes);
                    Log.i("Success Save Capture to:" + savePath);
                }
                catch (Exception e)
                {
                    Log.e(e);
                }
            }

            if (listener != null)
            {
                listener(tex, savePath);
            }
        }
    }
}
