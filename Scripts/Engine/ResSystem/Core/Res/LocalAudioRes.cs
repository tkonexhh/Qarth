using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class LocalAudioRes : AbstractRes, IDownloadTask
    {
        public const string PREFIX_KEY = "LocalMusic:";
        private string m_Url;
        private object m_RawAsset;
        private WWW m_WWW;

        public static LocalAudioRes Allocate(string name)
        {
            LocalAudioRes res = ObjectPool<LocalAudioRes>.S.Allocate();
            if (res != null)
            {
                res.name = name;
                res.SetUrl(name.Substring(11));
            }
            return res;
        }

        public void SetDownloadProgress(int totalSize, int download)
        {

        }

        public string localResPath
        {
            get
            {
                return m_Url;
            }
        }

        public override object rawAsset
        {
            get { return m_RawAsset; }
        }

        public bool needDownload
        {
            get
            {
                return refCount > 0;
            }
        }

        public string url
        {
            get
            {
                return m_Url;
            }
        }

        public bool logError
        {
            get { return true; }
        }

        public int fileSize
        {
            get
            {
                return 1;
            }
        }

        public void SetUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            m_Url = url;
        }

        public override bool UnloadImage(bool flag)
        {
            return false;
        }

        public override bool LoadSync()
        {
            return false;
        }

        public override void LoadAsync()
        {
            if (!CheckLoadAble())
            {
                return;
            }

            if (string.IsNullOrEmpty(m_Name))
            {
                return;
            }

            DoLoadWork();
        }

        private void DoLoadWork()
        {
            resState = eResState.kLoading;

            OnDownLoadResult(true);
        }

        protected override void OnReleaseRes()
        {
            if (m_Asset != null)
            {
                GameObject.Destroy(m_Asset);
                m_Asset = null;
            }

            m_RawAsset = null;
        }

        public override void Recycle2Cache()
        {
            ObjectPool<LocalAudioRes>.S.Recycle(this);
        }

        public override void OnCacheReset()
        {

        }

        public void DeleteOldResFile()
        {
            //throw new NotImplementedException();
        }

        public void OnDownLoadResult(bool result)
        {
            if (!result)
            {
                OnResLoadFaild();
                return;
            }

            if (refCount <= 0)
            {
                resState = eResState.kWaiting;
                return;
            }

            ResMgr.S.PostIEnumeratorTask(this);
            //ResMgr.S.PostLoadTask(LoadImage());
        }

        //完全的WWW方式,Unity 帮助管理纹理缓存，并且效率貌似更高
        public override IEnumerator StartIEnumeratorTask(Action finishCallback)
        {
            if (refCount <= 0)
            {
                OnResLoadFaild();
                finishCallback();
                yield break;
            }

            string path = null;
#if UNITY_EDITOR
            path = "file:///" + localResPath;
#else
            path = "file://" + localResPath;
#endif

            WWW www = new WWW(path);

            m_WWW = www;

            yield return www;

            m_WWW = null;

            if (www.error != null)
            {
                Log.e(string.Format("Res:{0}, WWW Errors:{1}", path, www.error));
                OnResLoadFaild();
                finishCallback();
                yield break;
            }

            if (!www.isDone)
            {
                Log.e("LocalAudioRes WWW Not Done! Url:" + m_Url);
                OnResLoadFaild();
                finishCallback();

                www.Dispose();
                www = null;

                yield break;
            }

            if (refCount <= 0)
            {
                OnResLoadFaild();
                finishCallback();

                www.Dispose();
                www = null;
                yield break;
            }

            //TimeDebugger dt = new TimeDebugger("Tex");
            //dt.Begin("LoadingTask");
            //这里是同步的操作
            m_Asset = www.GetAudioClip();
            //dt.End();

            www.Dispose();
            www = null;

            //dt.Dump(-1);

            resState = eResState.kReady;

            finishCallback();
        }

        protected override float CalculateProgress()
        {
            if (m_WWW == null)
            {
                return 0;
            }

            return m_WWW.progress;
        }

        public bool OnValidCheck(byte[] raw)
        {
            return true;
        }
    }
}
