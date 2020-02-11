using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Qarth
{
    public static class GameObjectExtensions
    {

        public static RectTransform rectTransform(this Component cp)
        {
            return cp.transform as RectTransform;
        }

        /// <summary>
        /// 遍历go c
        /// </summary>
        /// <param name="go"></param>
        /// <param name="handle"></param>
        static public void IterateGameObject(this GameObject go, Action<GameObject> handle)
        {
            Queue q = new Queue();
            q.Enqueue(go);
            while (q.Count != 0)
            {
                GameObject tmpGo = (GameObject)q.Dequeue();
                foreach (Transform t in tmpGo.transform)
                {
                    q.Enqueue(t.gameObject);
                }
                if (handle != null)
                {
                    handle(tmpGo);
                }
            }
        }

        /// <summary>
        /// 设置go层级关系 c
        /// </summary>
        /// <param name="go"></param>
        /// <param name="layer"></param>
        static public void SetAllLayer(this GameObject go, int layer)
        {
            IterateGameObject(go, (g) =>
            {
                g.layer = layer;
            });
        }

        /// <summary>
        /// 重置某个物体的三围等 c
        /// </summary>
        /// <param name="go"></param>
        static public void Reset(this GameObject go)
        {
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        static public void ResetLocalAngle(this GameObject go)
        {
            go.transform.localEulerAngles = Vector3.zero;
        }

        static public void ResetLocalPos(this GameObject go)
        {
            go.transform.localPosition = Vector3.zero;
        }

        static public void ResetLocalScale(this GameObject go)
        {
            go.transform.localScale = Vector3.one;
        }

        static public void SetAngle(this GameObject go, Vector3 angle)
        {
            go.transform.localEulerAngles = angle;
        }


        static public void SetLocalPos(this GameObject go, Vector3 pos)
        {
            go.transform.localPosition = pos;
        }
        static public void SetPos(this GameObject go, Vector3 pos)
        {
            go.transform.position = pos;
        }

        static public void SetParent(this GameObject go, Transform parent)
        {
            go.transform.SetParent(parent);
        }

        static public void SetParent(this GameObject go, GameObject parent)
        {
            go.transform.SetParent(parent.transform);
        }

        static public T AddMissingComponent<T>(this GameObject go) where T : Component
        {
            T comp = go.GetComponent<T>();
            if (comp == null)
            {
                comp = go.AddComponent<T>();
            }
            return comp;
        }
    }

}