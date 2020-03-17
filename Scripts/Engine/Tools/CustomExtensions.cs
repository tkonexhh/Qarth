using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace Qarth
{
    public static class CustomExtensions
    {
        #region helper
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static List<T> GetRandomList<T>(List<T> inputList)
        {
            //Set outputList and random
            List<T> outputList = new List<T>();

            while (inputList.Count > 0)
            {
                //Select an index and item
                int rdIndex = RandomHelper.Range(0, inputList.Count);
                T remove = inputList[rdIndex];

                //remove it from copyList and add it to output
                inputList.Remove(remove);
                outputList.Add(remove);
            }
            return outputList;
        }

        public static T CreateHandler<T>(string className) where T : class
        {
            if (string.IsNullOrEmpty(className))
            {
                return null;
            }

            Type type = Type.GetType(className);
            if (type == null)
            {
                Debug.LogError("Not Find Handler Class:" + className);
                return null;
            }

            try
            {
                object obj = Activator.CreateInstance(type, true);

                return obj as T;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }

        public static void DoBuzz()
        {
            if (PlayerPrefs.GetInt(EngineDefine.BUZZ_STATE, 1) > 0 && CustomVibration.HasVibrator())
            {
                CustomVibration.Vibrate(10);
            }
        }


        public static T GetStringEnum<T>(string val)
        {
            return (T)Enum.Parse(typeof(T), val);
        }

        public static Coroutine CallWithDelay(this MonoBehaviour obj, System.Action call, float delay)
        {
            return obj.StartCoroutine(doCallWithDelay(call, delay));
        }

        static IEnumerator doCallWithDelay(System.Action call, float delay)
        {
            if (delay <= 0)
                yield return null;
            else
            {
                float start = Time.realtimeSinceStartup;
                while (Time.realtimeSinceStartup < start + delay)
                {
                    yield return null;
                }
            }

            if (call != null)
                call.Invoke();
        }


        public static Toggle GetActive(this ToggleGroup aGroup)
        {
            return aGroup.ActiveToggles().FirstOrDefault();
        }

        public static string TrimHeadChar(string str, string item)
        {
            if (str.Substring(0, 1) == item)
            {
                return TrimHeadChar(str.Substring(1, str.Length - 1), item);
            }
            else
                return str;
        }
        #endregion


        #region transform





        public static float Pixel2DP(float pixel)
        {
            return pixel * 160 / Screen.dpi;
        }

        public static float DP2Pixel(float dp)
        {
            return dp * (Screen.dpi / 160);
        }
        #endregion

        #region physics
        static public void SetRigidBodiesKinematic(this GameObject obj, bool state)
        {
            var bodies = obj.GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < bodies.Length; i++)
            {
                if (bodies[i].gameObject.tag == "StayKinematic")
                    bodies[i].isKinematic = true;
                else
                    bodies[i].isKinematic = state;
            }
        }
        static public void SetColliderEnable(this GameObject obj, bool state)
        {
            var cols = obj.GetComponentsInChildren<Collider>();
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i].enabled = state;
            }
        }

        static public void AddRigidBodiesForce(this GameObject obj, Vector3 force, ForceMode mode = ForceMode.Force)
        {
            var bodies = obj.GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < bodies.Length; i++)
                bodies[i].AddForce(force, mode);
        }
        static public void SetRigidBodiesDrag(this GameObject obj, float dragVal = 0)
        {
            var bodies = obj.GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < bodies.Length; i++)
                bodies[i].drag = dragVal;
        }
        static public void SetRigidBodiesAngularDrag(this GameObject obj, float dragVal = 0.05f)
        {
            var bodies = obj.GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < bodies.Length; i++)
                bodies[i].angularDrag = dragVal;
        }
        #endregion


        #region render
        public static Color HexToColor(string hex, bool alpha = false)
        {
            byte br = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte bg = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte bb = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            float r = br / 255f;
            float g = bg / 255f;
            float b = bb / 255f;
            if (alpha)
            {
                byte cc = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                float a = cc / 255f;
                return new Color(r, g, b, a);
            }
            else
                return new Color(r, g, b);
        }

        public static Texture2D toTexture2D(this RenderTexture rTex, int size = 256)
        {
            Texture2D tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
            RenderTexture.active = rTex;
            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.Apply();
            return tex;
        }

        static public void SampleAnim(this Animator animator, string stateName, float normalizedTime, int layer = -1)
        {
            animator.StopPlayback();
            animator.Play(stateName, layer, normalizedTime);
            animator.StartPlayback();
        }
        static public void SampleAnim(this Animation anim, string stateName, float normalizedTime)
        {
            anim.Play(stateName);
            anim[stateName].speed = 1;
            anim[stateName].normalizedTime = normalizedTime;
            anim[stateName].speed = 0;
        }
        static public void PlayAnim(this Animation anim, string stateName)
        {
            anim.Play(stateName);
            anim[stateName].speed = 1;
        }

        //获取网格的size
        static public Vector3 GetMeshSize(this GameObject objRoot)
        {
            var mesh = objRoot.GetComponent<Renderer>();
            return mesh == null ? Vector3.zero : mesh.bounds.size;
        }
        //获取网格的中心
        static public Vector3 GetMeshCenter(this GameObject objRoot)
        {
            Vector3 boundsCenter = Vector3.zero;
            var meshes = objRoot.GetComponentsInChildren<Renderer>();
            for (int j = 0; j < meshes.Length; j++)
            {
                boundsCenter += meshes[j].bounds.center;
            }
            boundsCenter /= meshes.Length * 1.0f;
            return boundsCenter;
        }

        private static SpritesHandler m_GlobalSprHandler = new SpritesHandler();
        public static Sprite FindSprite(ResLoader loader, string assetName, string spriteName)
        {
            Sprite result = null;
            result = loader.LoadSync(spriteName) as Sprite;
            if (result == null)
            {
                var data = loader.LoadSync(assetName) as SpritesData;
                if (data != null)
                {
                    m_GlobalSprHandler.SetData(new SpritesData[] { data });
                    return m_GlobalSprHandler.FindSprite(spriteName) as Sprite;
                }
            }
            return result;
        }
        #endregion

        #region time
        private static long Jan1st1970Ms = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Local).Ticks;

        public static long CurrentTimeMillis()
        {
            return (System.DateTime.UtcNow.Ticks - Jan1st1970Ms) / 10000;
        }
        public static long CurrentTimeMillis(long ticks)
        {
            return (ticks - Jan1st1970Ms) / 10000;
        }

        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

        public static string GetTimeStampUni()
        {
            TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

        public static string GetTimeStampUniSec()
        {
            TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }


        public static DateTime GetTimeFromTimestamp(string timestamp)
        {
            DateTime dtStart = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return dtStart.AddMilliseconds(long.Parse(timestamp));
        }

        public static long GetSecFromTimestamps(string timestamp)
        {
            DateTime dtStart = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var dtLast = dtStart.AddMilliseconds(long.Parse(timestamp));
            return (long)(DateTime.Now - dtLast).TotalSeconds;
        }

        public static string GetTimeStampAfterSec(string timestamp, int sec)
        {
            DateTime dtStart = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var dtLast = dtStart.AddMilliseconds(long.Parse(timestamp)).AddSeconds(sec);
            return Convert.ToInt64((dtLast - dtStart).TotalMilliseconds).ToString();
        }
        public static string GetTimeStamp(DateTime dt)
        {
            return ((dt.Ticks - 621355968000000000) / 10000000).ToString();
        }

        public static int CheckIsNewDay(string dayKey = "lastsignstr--12354371")
        {
            string timeString = PlayerPrefs.GetString(dayKey, "");
            DateTime lastSignDate;
            if (!string.IsNullOrEmpty(timeString))
            {
                if (DateTime.TryParse(timeString, out lastSignDate))
                {
                    DateTime today = DateTime.Today;
                    TimeSpan pass = today - lastSignDate;

                    if (pass.TotalDays >= 1)
                    {
                        PlayerPrefs.SetString(dayKey, DateTime.Today.ToShortDateString());
                    }
                    return pass.Days;
                }
                else
                {
                    PlayerPrefs.SetString(dayKey, DateTime.Today.ToShortDateString());
                    return -1;
                }
            }
            else
            {
                PlayerPrefs.SetString(dayKey, DateTime.Today.ToShortDateString());
                return 999999;
            }
        }
        #endregion

        #region screen_pos
        public static void ScreenPosition2UIPosition(Camera sceneCamera, Camera uiCamera, Vector3 posInScreen, Transform uiTarget)
        {
            Vector3 viewportPos = sceneCamera.ScreenToViewportPoint(posInScreen);
            Vector3 worldPos = uiCamera.ViewportToWorldPoint(viewportPos);
            uiTarget.position = worldPos;
            Vector3 localPos = uiTarget.localPosition;
            localPos.z = 0f;
            uiTarget.localPosition = localPos;
        }
        public static void ScenePosition2UIPosition(Camera sceneCamera, Camera uiCamera, Vector3 posInScene, Transform uiTarget)
        {
            Vector3 viewportPos = sceneCamera.WorldToViewportPoint(posInScene);
            Vector3 worldPos = uiCamera.ViewportToWorldPoint(viewportPos);
            uiTarget.position = worldPos;
            Vector3 localPos = uiTarget.localPosition;
            localPos.z = 0f;
            uiTarget.localPosition = localPos;
        }

        public static Vector3 UIPosToScenePos(Camera sceneCamera, Camera uiCamera, Vector3 uiPos)
        {
            Vector3 viewPos = uiCamera.WorldToViewportPoint(uiPos);
            Vector3 worldPos = sceneCamera.ViewportToWorldPoint(viewPos);
            worldPos.z = 0;
            return worldPos;
        }
        #endregion


        public static bool IsPermissionGranted(string permission)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return AndroidPermissionsManager.IsPermissionGranted(permission);
#endif
            return true;
        }

        public static void RequestPermission(
            string permission,
            Action<string> onGrantedCallback,
            Action<string> onDeniedCallback,
            Action<string> onDeniedAndDontAskAgainCallback)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (!IsPermissionGranted(permission))
            {
                AndroidPermissionsManager.RequestPermission(new[] { permission },
                    new AndroidPermissionCallback(onGrantedCallback, onDeniedCallback, onDeniedAndDontAskAgainCallback));
            }
#endif
        }

        public static void RequestPermissions(
            string[] permissions,
            Action<string> onGrantedCallback,
            Action<string> onDeniedCallback,
            Action<string> onDeniedAndDontAskAgainCallback)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidPermissionsManager.RequestPermission(permissions,
                new AndroidPermissionCallback(onGrantedCallback, onDeniedCallback, onDeniedAndDontAskAgainCallback));
#endif
        }

        public static void RequestSensitivePermissions()
        {
            RequestPermission(AndroidPermissionDefine.READ_PHONE_STATE, (p) => { }, (p) => { }, (p) => { });
            RequestPermission(AndroidPermissionDefine.WRITE_EXTERNAL_STORAGE, (p) => { }, (p) => { }, (p) => { });
            RequestPermission(AndroidPermissionDefine.ACCESS_COARSE_LOCATION, (p) => { }, (p) => { }, (p) => { });
        }

        public static Color StringToColor(string colorStr)
        {
            if (string.IsNullOrEmpty(colorStr))
            {
                return new Color();
            }
            int colorInt = int.Parse(colorStr, System.Globalization.NumberStyles.AllowHexSpecifier);
            return IntToColor(colorInt);
        }

        public static Color IntToColor(int colorInt)
        {
            float basenum = 255;

            int b = 0xFF & colorInt;
            int g = 0xFF00 & colorInt;
            g >>= 8;
            int r = 0xFF0000 & colorInt;
            r >>= 16;
            return new Color((float)r / basenum, (float)g / basenum, (float)b / basenum, 1);

        }

    }

}