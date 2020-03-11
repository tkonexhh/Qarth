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

        public override void OnSingletonInit()
        {
            m_LstHandle = new List<AddressableRes>();
        }

        public AddressableRes LoadAssetAsync<T>(string assetName, string label = "")
        {
            AddressableRes res = new AddressableRes();
            m_LstHandle.Add(res);
            res.LoadAssetAsync<T>(assetName, label);//.AddCompleteCallback(OnLoadDone);
            return res;
        }

        public AsyncOperationHandle<GameObject> InstanceAsync(string assetName, string label = "")
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(assetName);
            return handle;
        }

        public void ReleaseRes(AddressableRes res)
        {
            if (res == null) return;

            res.Release();
        }

        // public void ReleaseAsset(AsyncOperationHandle handle)
        // {
        //     // if (m_LstHandle.Remove(handle))
        //     // {
        //     //     Addressables.Release(handle);
        //     // }
        //     // else
        //     // {
        //     // }
        // }

        public void ReleaseAllAsset()
        {
            for (int i = m_LstHandle.Count - 1; i >= 0; --i)
            {
                Addressables.Release(m_LstHandle[i]);
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

        public static AsyncOperationHandle AddCompleteCallback(this AsyncOperationHandle<GameObject> handle, Action<AsyncOperationHandle<GameObject>> completedCallBack)
        {
            handle.Completed += completedCallBack;
            return handle;
        }

    }


    public class AddressableRes : IAddressableRes
    {
        public AsyncOperationHandle loadHandler;
        private List<AsyncOperationHandle> m_LstInstanceHandler;
        public bool isReady = false;
        public string assetName = "";

        //只能用于GameObject
        public AsyncOperationHandle<GameObject> InstantiateAsync(string label = "")
        {

            Debug.Assert(!string.IsNullOrEmpty(assetName), "assetName is null");

            if (m_LstInstanceHandler == null) m_LstInstanceHandler = new List<AsyncOperationHandle>();

            var handle = Addressables.InstantiateAsync(assetName);
            m_LstInstanceHandler.Add(handle);
            return handle;
        }

        public AsyncOperationHandle LoadAssetAsync<T>(string assetName, string label = "")
        {
            this.assetName = assetName;
            AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(assetName);
            handle.AddCompleteCallback(OnLoadDone);
            loadHandler = handle;
            return handle;
        }

        private void OnLoadDone(AsyncOperationHandle handle)
        {
            isReady = true;
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

            //Addressables.Release(loadHandler);
        }
    }

    public class IAddressableRes
    {

    }


}