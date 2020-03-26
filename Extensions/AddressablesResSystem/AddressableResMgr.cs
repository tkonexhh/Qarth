using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Qarth
{
    public class AddressableResMgr : TSingleton<AddressableResMgr>
    {
        private List<AddressableRes> m_LstHandle;
        private Dictionary<string, AddressableRes> m_ResMap;

        public override void OnSingletonInit()
        {
            m_LstHandle = new List<AddressableRes>();
            m_ResMap = new Dictionary<string, AddressableRes>();
        }

        public AsyncOperationHandle LoadSceneAsync(string name, Action<SceneInstance> callback = null, LoadSceneMode mode = LoadSceneMode.Single)
        {
            var handle = Addressables.LoadSceneAsync(name, mode).AddCompleteCallback(
                   (result) =>
                   {
                       if (callback != null)
                           callback.Invoke(result.Result);
                   });

            return handle;
        }

        public AddressableRes LoadAssetAsync<T>(string assetName, Action<T> completeCallback = null, string label = "")
        {
            AddressableRes res = null;
            if (m_ResMap.TryGetValue(assetName, out res))
            {
                return res;
            }
            else
            {
                res = new AddressableRes();
                m_LstHandle.Add(res);
                m_ResMap.Add(assetName, res);
                res.LoadAssetAsync<T>(assetName, completeCallback);
            }
            return res;
        }


        public void ReleaseRes(AddressableRes res)
        {
            if (res == null) return;
            if (m_LstHandle.Contains(res))
                res.Release();
        }

        public void ReleaseAllAsset()
        {
            for (int i = m_LstHandle.Count - 1; i >= 0; --i)
            {
                m_LstHandle[i].Release();
            }
        }
    }

    public static class AddressableResMgrExtension
    {
        public static AsyncOperationHandle AddCompleteCallback(this AsyncOperationHandle handle, Action<AsyncOperationHandle> completedCallBack)
        {
            handle.Completed += completedCallBack;
            return handle;
        }

        public static AsyncOperationHandle<T> AddCompleteCallback<T>(this AsyncOperationHandle<T> handle, Action<AsyncOperationHandle<T>> completedCallBack)
        {
            handle.Completed += completedCallBack;
            return handle;
        }
    }

    public class AddressableRes : IAddressableRes
    {
        private AsyncOperationHandle m_loadHandler;
        protected List<AsyncOperationHandle> m_LstInstanceHandler;

        public bool isReady = false;
        public string assetName = "";

        //只能用于GameObject
        public AsyncOperationHandle<GameObject> InstantiateAsync(Action<GameObject> instanceCallback = null, string label = "")
        {
            Debug.Assert(!string.IsNullOrEmpty(assetName), "assetName is null");

            var handle = Addressables.InstantiateAsync(assetName);

            handle.AddCompleteCallback((result) =>
            {
                if (instanceCallback != null)
                    instanceCallback.Invoke(result.Result);
            });
            m_LstInstanceHandler.Add(handle);

            return handle;
        }

        public AsyncOperationHandle LoadAssetAsync<T>(string assetName, Action<T> completeCallback, string label = "")
        {
            this.assetName = assetName;
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetName);
            handle.AddCompleteCallback((result) =>
            {
                if (completeCallback != null)
                    completeCallback.Invoke(result.Result);
            });

            m_loadHandler = handle;
            if (m_LstInstanceHandler == null) m_LstInstanceHandler = new List<AsyncOperationHandle>();
            return handle;
        }

        private void OnLoadDone<T>(AsyncOperationHandle<T> handle)
        {
            isReady = true;
            //if ()
        }

        public void Release()
        {
            if (m_LstInstanceHandler == null) return;
            for (int i = m_LstInstanceHandler.Count - 1; i >= 0; i--)
            {
                Addressables.Release(m_LstInstanceHandler[i]);
                m_LstInstanceHandler.RemoveAt(i);
            }
            m_LstInstanceHandler = null;
            Addressables.ReleaseInstance(m_loadHandler);
            //Addressables.Release(loadHandler);
        }
    }

    public class IAddressableRes
    {

    }


}