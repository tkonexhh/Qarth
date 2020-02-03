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
using UnityEngine.UI;

namespace Qarth
{
    public class NetImageView : MonoBehaviour
    {
        [SerializeField]
        private RawImage    m_Image;
        [SerializeField]
        private string      m_PrefixKey = NetImageRes.PREFIX_KEY;
        [SerializeField]
        private string      m_Url;
        [SerializeField]
        private bool        m_Refresh = false;
        [SerializeField]
        private bool        m_HideWhenLoading = true;

        private IRes        m_Res;

        private IRes        m_PreRes;

        private Action m_ShowCallback;

        public void RegisterShowCallback(Action callBack)
        {
            m_ShowCallback = callBack;
        }

        public string prefixKey
        {
            get { return m_PrefixKey; }
            set { m_PrefixKey = value; }
        }

        private void Awake()
        {
            if (m_Image == null)
            {
                m_Image = GetComponent<RawImage>();
            }

            RequestUpdateImage();
        }

        public string imageUrl
        {
            get
            {
                return m_Url;
            }
            set
            {
                if (m_Url != value)
                {
                    m_Url = value;
                }
                else
                {
                    return;
                }

                RequestUpdateImage();
            }
        }
        private void OnDestroy()
        {
            if (m_Res != null)
            {
                m_Res.UnRegisteResListener(OnResLoadFinish);
                m_Res.SubRef();
                m_Res = null;
            }

            if (m_PreRes != null)
            {
                m_PreRes.UnRegisteResListener(OnResLoadFinish);
                m_PreRes.SubRef();
                m_PreRes = null;
            }
        }

        private void RequestUpdateImage()
        {
            if (m_PreRes != null)
            {
                m_PreRes.UnRegisteResListener(OnResLoadFinish);
                m_PreRes.SubRef();
                m_PreRes = null;
            }

            if (m_Res != null)
            {
                if (!m_HideWhenLoading)
                {
                    m_PreRes = m_Res;
                }
            }

            if (m_Image != null && m_PreRes == null)
            {
                m_Image.enabled = false;
            }

            if (string.IsNullOrEmpty(m_Url))
            {
                return;
            }

            m_Res = ResMgr.S.GetRes(string.Format("{0}{1}", m_PrefixKey, m_Url), true);
            m_Res.AddRef();

            m_Res.RegisteResListener(OnResLoadFinish);
            m_Res.LoadAsync();
        }

        private void OnResLoadFinish(bool result, IRes res)
        {
            if (m_PreRes != null)
            {
                m_PreRes.UnRegisteResListener(OnResLoadFinish);
                m_PreRes.SubRef();
                m_PreRes = null;
            }

            if (m_Image == null)
            {
                return;
            }

            if (result)
            {
                m_Image.enabled = true;
                m_Image.texture = res.asset as Texture;

                if (m_ShowCallback != null)
                {
                    m_ShowCallback();
                }
            }
        }

        void OnValidate()
        {
            if (m_Refresh)
            {
                m_Refresh = false;
                RequestUpdateImage();
            }
        }
    }
}
