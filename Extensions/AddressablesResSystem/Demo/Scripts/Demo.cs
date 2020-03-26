using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;
using UnityEngine.AddressableAssets;

namespace GameWish.Game
{
    public class Demo : MonoBehaviour
    {
        [SerializeField] Image m_ImgDemo;
        AddressableRes handle;
        AddressableRes handle_Icon;

        void Start()
        {
            handle_Icon = AddressableResMgr.S.LoadAssetAsync<Sprite>("folder_icon", OnAssetLoaded1);
            handle = AddressableResMgr.S.LoadAssetAsync<GameObject>("Cube");
        }

        UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle handle_scene;
        bool sc = false;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                for (int i = 0; i < 50; i++)
                    SpawnCube();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                AddressableResMgr.S.ReleaseAllAsset();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                AddressableResMgr.S.ReleaseRes(handle_Icon);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (sc)
                {
                    Addressables.Release(handle_scene);
                }
                else
                {
                    handle_scene = AddressableResMgr.S.LoadSceneAsync("AddressDemo", null);
                    sc = true;
                }


            }
        }

        void SpawnCube()
        {
            if (handle != null)
                (handle).InstantiateAsync(OnAssetLoaded);
        }

        private void OnAssetLoaded1(Sprite handle)
        {
            m_ImgDemo.sprite = handle;
        }

        private void OnAssetLoaded(GameObject handle)
        {
            GameObject gameObject = handle;
            Vector3 pos = Random.insideUnitSphere * 10;
            gameObject.SetPos(pos);
        }
    }

}