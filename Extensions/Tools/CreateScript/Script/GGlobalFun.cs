using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using LitJson;

namespace Qarth
{


    public static class GGlobalFun
    {
        private static readonly char[] replaceChars = new char[] { ' ', '(', ')' };



        /// <summary>
        /// 写入路径
        /// </summary>
        /// <returns></returns>
        public static string GetWritePath()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                    return Application.dataPath.Replace("/Assets", "") + "/Data/";
                case RuntimePlatform.IPhonePlayer:
                    return Application.persistentDataPath + "/";
                case RuntimePlatform.WindowsEditor:
                    return Application.dataPath.ToString().Replace("/Assets", "") + "/Data/";
                case RuntimePlatform.WindowsPlayer:
                    var path = Application.dataPath.ToString().Replace("/Assets", "") + "/Data/";
                    GFileOperation.CreateDirctory(path);
                    return path;
                case RuntimePlatform.Android:
                    return Application.persistentDataPath + "/";
                default:
                    return Application.persistentDataPath + "/";
            }
        }

        public static string GetInsidePath()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                    return "file://" + Application.dataPath + "/StreamingAssets/";
                case RuntimePlatform.IPhonePlayer:
                    return Application.streamingAssetsPath + "/";
                case RuntimePlatform.WindowsEditor:
                    return "file://" + Application.dataPath + "/StreamingAssets/";
                case RuntimePlatform.WindowsPlayer:
                    return "file://" + Application.dataPath + "/StreamingAssets/";
                case RuntimePlatform.Android:
                    return Application.dataPath + "!/Assets/";
                default:
                    return Application.dataPath + "!/Assets/";
            }
        }

        public static string GetPlatform()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                    return "window";
                case RuntimePlatform.IPhonePlayer:
                    return "ios";
                case RuntimePlatform.WindowsEditor:
                    return "window";
                case RuntimePlatform.WindowsPlayer:
                    return "window";
                case RuntimePlatform.Android:
                    return "android";
                default:
                    return "android";
            }
        }

        public static string GetDeviceCode()
        {
            return SystemInfo.deviceUniqueIdentifier;
        }

        public static bool IsLengthZero(this string target)
        {
            return target.Length == 0;
        }

        public static string GetString(string value)
        {
            StringBuilder sb = new StringBuilder(value);
            foreach (var error in replaceChars)
            {
                sb.Replace(error, '_');
            }
            return sb.ToString();
        }

        public static T IsComponent<T>(this GameObject go) where T : Component
        {
            T com = go.GetComponent<T>();
            if (com == null) com = go.AddComponent<T>();
            return com;
        }

        public static T IsComponent<T>(this Transform target) where T : Component
        {
            T com = target.GetComponent<T>();
            if (com == null) com = target.gameObject.AddComponent<T>();
            return com;
        }

        public static string GetFirstUpper(string value)
        {
            return char.ToUpper(value[0]) + value.Substring(1);
        }

        // public static string[] GetComponentsNameAll(Transform t)
        // {
        //     var types = GetComponentsName(t);
        //     List<string> lsttype = new List<string>(types);
        //     lsttype.Add("Transform");
        //     lsttype.Add("GameObject");
        //     return lsttype.ToArray();
        // }

        public static string[] GetComponentsName(Transform t)
        {
            var coms = t.GetComponents<Component>();
            if (coms == null) return null;
            List<string> types = new List<string>();
            types.Add("Transform");
            types.Add("GameObject");
            for (int i = 0; i < coms.Length; i++)
            {
                if (coms[i] == null) continue;
                types.Add(coms[i].GetType().Name);
            }
            return types.ToArray();
        }

        public static string GetGameObjectPath(Transform target, Transform parent)
        {
            if (target == parent)
            {
                return "";
            }
            return GetFindPath(target, parent).Remove(0, 1);
        }

        public static Assembly GetAssembly()
        {
            Assembly[] AssbyCustmList = System.AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < AssbyCustmList.Length; i++)
            {
                string assbyName = AssbyCustmList[i].GetName().Name;
                if (assbyName == "Assembly-CSharp")
                {
                    return AssbyCustmList[i];
                }
            }
            return null;
        }

        public static string[] GetStringList(string target, int max = 10000)
        {
            if (target.Length < max) return new string[] { target };
            int count = target.Length / max;
            var array = new string[count + 1];
            for (int i = 0; i < count; i++)
            {
                array[i] = target.Substring(i * max, max) + "\n";
            }
            array[count] = target.Substring(count * max, target.Length % max);
            return array;
        }

        public static T GetInstance<T>(this string instanceName, params object[] param)
        {
            return (T)Assembly.Load(Assembly.GetAssembly(typeof(T)).GetName().Name).CreateInstance(typeof(T).Namespace + "." + instanceName, true, BindingFlags.CreateInstance, null, param, Thread.CurrentThread.CurrentCulture, null);
        }

        public static string GetFind(Transform target)
        {
            if (target.parent == null)
            {
                if (target == null) return string.Empty;
                return target.name;
            }
            return GetFind(target.parent) + "/" + target.name;
        }

        public static Transform GetParent(this string value)
        {
            var index = value.LastIndexOf('/');
            return GameObject.Find(value.Remove(index, value.Length - index)).transform;
        }



        private static string GetFindPath(Transform target, Transform parent)
        {
            if (target == parent)
                return string.Empty;
            return GetFindPath(target.parent, parent) + "/" + target.name;
        }



        #region GGlobalFunction 迁移

        public static readonly Vector2 Vector2Center = new Vector2(0.5f, 0.5f);
        public static readonly Vector3 Vector3Center = new Vector3(0.5f, 0.5f, 0.5f);

        public static Transform[] GetTransforms(this Transform transform)
        {
            List<Transform> list = new List<Transform>();
            foreach (Transform item in transform)
            {
                list.Add(item);
                if (item.childCount > 0)
                {
                    list.AddRange(item.GetTransforms());
                }
            }
            return list.ToArray();
        }

        public static Sprite ToSprite(this Texture2D value)
        {
            return Sprite.Create(value, new Rect(0, 0, value.width, value.height), Vector2.zero, 1f);
        }

        public static bool IsPointerOverLayer(int layer)
        {
            PointerEventData pointerData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            pointerData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            UnityEngine.EventSystems.EventSystem.current.RaycastAll(pointerData, results);
            //if (tag.Length != 0)
            /*{
                for (int j = 0; j < results.Count; j++)
                {
                    if (results[j].gameObject.layer == layer)
                        return false;
                }
            }*/

            return results.Count > 0 && results[0].gameObject.layer == layer;
        }

        public static float Distance(this float a, float b)
        {
            return Mathf.Abs(a) - Mathf.Abs(b);
        }

        /// <summary>
        /// 2D旋转矩阵
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Matrix4x4 Rotate2D(this Matrix4x4 mat, float angle)
        {
            angle = angle * Mathf.Deg2Rad;
            mat.m00 = Mathf.Cos(angle);
            mat.m01 = Mathf.Sin(angle);
            mat.m10 = -Mathf.Sin(angle);
            mat.m11 = Mathf.Cos(angle);
            return mat;
        }

        /// <summary>
        /// UV旋转
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="angle"></param>
        /// <param name="channelIndex"></param>
        /// <returns></returns>
        public static Mesh RotateUV(this Mesh mesh, float angle, int channelIndex = 0)
        {
            List<Vector2> uvs = new List<Vector2>(mesh.uv);

            var mat = Rotate2D(new Matrix4x4(), angle);

            for (int i = 0; i < uvs.Count; i++)
            {
                Vector2 uv = uvs[i] - Vector2Center;
                uv = (mat * uv);
                uv += Vector2Center;
                uvs[i] = uv;
            }
            mesh.SetUVs(channelIndex, uvs);
            return mesh;
        }

        /// <summary>
        /// 纹理分割
        /// </summary>
        /// <param name="target"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Texture2D TextureClipRect(Texture2D target, RectInt rect)
        {
            var width = rect.width;
            var height = rect.height;

            if (rect.x + rect.width >= target.width) width = target.width - rect.x;
            if (rect.y + rect.height >= target.height) height = target.height - rect.y;

            var result = new Texture2D(width, height);
            var data = target.GetPixels(rect.x, rect.y, width, height);
            result.SetPixels(data);
            result.Apply();

            return result;
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="target"></param>
        /// <param name="result"></param>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        public static void DrawRect(Texture2D target, ref Texture2D result, RectInt rect, Color color)
        {
            var data = target.GetPixels();
            var width = target.width;
            var offsetWidth = rect.width - rect.x;
            var offsetHeight = rect.height - rect.y;

            if (offsetWidth >= offsetHeight)
            {
                offsetWidth = rect.x + rect.width;
                offsetHeight = rect.y + rect.height;
                if (offsetWidth > width) offsetWidth = width;
                if (offsetHeight > target.height) offsetHeight = target.height;
                for (int y = rect.y; y < offsetHeight; y++)
                {
                    for (int x = rect.x; x < offsetWidth; x++)
                    {
                        data[y * width + x] = color;
                    }
                }
            }
            else
            {
                offsetWidth = rect.x + rect.width;
                offsetHeight = rect.y + rect.height;
                if (offsetWidth > width) offsetWidth = width;
                if (offsetHeight > target.height) offsetHeight = target.height;
                for (int x = rect.x; x < offsetWidth; x++)
                {
                    for (int y = rect.y; y < offsetHeight; y++)
                    {
                        data[y * width + x] = color;
                    }
                }
            }
            result.SetPixels(data);
            result.Apply();
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="target"></param>
        /// <param name="result"></param>
        /// <param name="rect"></param>
        /// <param name="coefficient"></param>
        public static void DrawRect(Texture2D target, ref Texture2D result, RectInt rect, float coefficient)
        {
            var data = target.GetPixels();
            var width = target.width;
            var offsetWidth = rect.width - rect.x;
            var offsetHeight = rect.height - rect.y;
            Color color;

            var w = rect.x + rect.width;
            var h = rect.y + rect.height;
            if (w > width) w = width;
            if (h > target.height) h = target.height;

            if (offsetWidth >= offsetHeight)
            {
                offsetWidth = rect.x + rect.width;
                offsetHeight = rect.y + rect.height;
                if (offsetWidth > width) offsetWidth = width;
                if (offsetHeight > target.height) offsetHeight = target.height;
                for (int y = rect.y; y < h; y++)
                {
                    for (int x = rect.x; x < w; x++)
                    {
                        color = data[y * width + x];
                        data[y * width + x] = new Color(color.r * coefficient, color.g * coefficient, color.b * coefficient, color.a);
                    }
                }
            }
            else
            {
                offsetWidth = rect.x + rect.width;
                offsetHeight = rect.y + rect.height;
                if (offsetWidth > width) offsetWidth = width;
                if (offsetHeight > target.height) offsetHeight = target.height;
                for (int x = rect.x; x < w; x++)
                {
                    for (int y = rect.y; y < h; y++)
                    {
                        color = data[y * width + x];
                        data[y * width + x] = new Color(color.r * coefficient, color.g * coefficient, color.b * coefficient, color.a);
                    }
                }
            }
            result.SetPixels(data);
            result.Apply();
        }

        /// <summary>
        /// 交换值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void Swap<T>(ref T a, ref T b)
        {
            var tmp = a;
            a = b;
            b = tmp;
        }

        /// <summary>
        /// 绘制直角三角形
        /// </summary>
        /// <param name="target"></param>
        /// <param name="result"></param>
        /// <param name="color"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        public static void DrawRightAngleTriangle(Texture2D target, ref Texture2D result, Color color, Vector2Int p1, Vector2Int p2, Vector2Int p3)
        {
            var data = target.GetPixels();
            var width = target.width;

            #region 确定 p3
            if (p1.y < p2.y) Swap(ref p1, ref p2);
            if (p2.y < p3.y) Swap(ref p2, ref p3);
            #endregion

            #region 确定 p2
            if (p1.x < p2.x) Swap(ref p1, ref p2);
            #endregion

            int count = 0;
            Vector2Int[] pos = null;
            if (p1.x == p3.x)
            {
                if (p2.y == p3.y)
                {
                    pos = GetBLinePoints(p2, p1);
                }

                else
                {
                    pos = GetBLinePoints(p2, p3);
                }
                Debug.Log(p1 + "" + p2 + p3);

                for (int i = 0; i < pos.Length; i++)
                {
                    count = p1.x - pos[i].x;
                    while (count-- != 0)
                    {
                        data[pos[i].y * width + pos[i].x + count] = color;
                    }
                }
            }
            else
            {
                if (p1.y == p3.y)
                    pos = GetBLinePoints(p1, p2);
                else
                    pos = GetBLinePoints(p1, p3);
                for (int i = 0; i < pos.Length; i++)
                {
                    count = pos[i].x - p2.x;
                    while (count-- != 0)
                    {
                        data[pos[i].y * width + count + p2.x] = color;
                    }
                }
            }

            result.SetPixels(data);
            result.Apply();
        }

        public static void DrawRightAngleTriangleTest(Texture2D target, ref Texture2D result)
        {
            var center = target.height / 2;
            DrawRightAngleTriangle(target, ref result, Color.green, new Vector2Int(0, 0), new Vector2Int(0, center), new Vector2Int(target.width, center));
            DrawRightAngleTriangle(result, ref result, Color.blue, new Vector2Int(0, 0), new Vector2Int(target.width - 1, 0), new Vector2Int(target.width - 1, center));
            DrawRightAngleTriangle(result, ref result, Color.red, new Vector2Int(0, center + 1), new Vector2Int(0, target.height - 1), new Vector2Int(target.width, target.height - 1));
            DrawRightAngleTriangle(result, ref result, Color.black, new Vector2Int(0, center + 1), new Vector2Int(target.width - 1, center + 1), new Vector2Int(target.width - 1, target.height - 1));
        }

        /// <summary>
        /// 绘制直角三角形
        /// </summary>
        /// <param name="target"></param>
        /// <param name="result"></param>
        /// <param name="coefficient">系数</param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        public static void DrawRightAngleTriangle(Texture2D target, ref Texture2D result, float coefficient, Vector2Int p1, Vector2Int p2, Vector2Int p3)
        {
            var data = target.GetPixels();
            var width = target.width;

            #region 确定 p3
            if (p1.y < p2.y) Swap(ref p1, ref p2);
            if (p2.y < p3.y) Swap(ref p2, ref p3);
            #endregion

            #region 确定 p2
            if (p1.x < p2.x) Swap(ref p1, ref p2);
            #endregion

            int count = 0;
            Vector2Int[] pos = null;
            Color color;
            int index;

            if (p1.x == p3.x)
            {
                if (p2.y == p3.y)
                {
                    pos = GetBLinePoints(p2, p1);
                }

                else
                {
                    pos = GetBLinePoints(p2, p3);
                }

                for (int i = 0; i < pos.Length; i++)
                {
                    count = p1.x - pos[i].x;
                    while (count-- != 0)
                    {
                        index = pos[i].y * width + pos[i].x + count;
                        color = data[index];
                        data[index] = new Color(color.r * coefficient, color.g * coefficient, color.b * coefficient, color.a);
                    }
                }
            }
            else
            {
                if (p1.y == p3.y)
                    pos = GetBLinePoints(p1, p2);
                else
                    pos = GetBLinePoints(p1, p3);
                for (int i = 0; i < pos.Length; i++)
                {
                    count = pos[i].x - p2.x;
                    while (count-- != 0)
                    {
                        index = pos[i].y * width + count + p2.x;
                        color = data[index];
                        data[index] = new Color(color.r * coefficient, color.g * coefficient, color.b * coefficient, color.a);
                    }
                }
            }

            result.SetPixels(data);
            result.Apply();
        }

        /// <summary>
        /// 获得直线坐标
        /// </summary>
        /// <param name="point0"></param>
        /// <param name="point1"></param>
        /// <returns></returns>
        public static Vector2Int[] GetBLinePoints(Vector2Int point0, Vector2Int point1)
        {
            var points = new List<Vector2Int>();
            int x0 = (int)point0.x;
            int y0 = (int)point0.y;
            int x1 = (int)point1.x;
            int y1 = (int)point1.y;

            var dx = Mathf.Abs(x1 - x0);
            var dy = Mathf.Abs(y1 - y0);
            var sx = (x0 < x1) ? 1 : -1;
            var sy = (y0 < y1) ? 1 : -1;
            var err = dx - dy;
            int err2;

            while (true)
            {
                points.Add(new Vector2Int(x0, y0));
                if ((x0 == x1) && (y0 == y1))
                {
                    break;
                }

                err2 = 2 * err;

                if (err2 + dy > 0)
                {
                    x0 += sx;
                    err -= dy;
                }

                if (err2 - dx < 0)
                {
                    y0 += sy;
                    err += dx;
                }
            }
            return points.ToArray();

        }

        /// <summary>
        /// 绘制直线
        /// </summary>
        /// <param name="result"></param>
        /// <param name="width"></param>
        /// <param name="color"></param>
        /// <param name="point0"></param>
        /// <param name="point1"></param>
        public static void DrawBLine(ref Color[] result, int width, Color color, Vector2 point0, Vector2 point1)
        {
            int x0 = (int)point0.x;
            int y0 = (int)point0.y;
            int x1 = (int)point1.x;
            int y1 = (int)point1.y;

            var dx = Mathf.Abs(x1 - x0);
            var dy = Mathf.Abs(y1 - y0);
            var sx = (x0 < x1) ? 1 : -1;
            var sy = (y0 < y1) ? 1 : -1;
            var err = dx - dy;
            int err2;

            while (true)
            {
                result[x0 + y0 * width] = color;
                if ((x0 == x1) && (y0 == y1))
                {
                    break;
                }

                err2 = 2 * err;

                if (err2 + dy > 0)
                {
                    x0 += sx;
                    err -= dy;
                }

                if (err2 - dx < 0)
                {
                    y0 += sy;
                    err += dx;
                }
            }
        }

        #endregion

    }
}