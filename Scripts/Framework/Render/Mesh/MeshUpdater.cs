using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class MeshUpdater : MonoBehaviour
    {
        [SerializeField]
        private MeshFilter m_MeshFilter;
        [SerializeField]
        private Renderer m_Render;
        [SerializeField]
        private bool m_AsyncLoad = false;

        private ResLoader m_Loader;
        private MeshHolder m_MeshHolder;

        private string m_MeshName;
        private string m_MeshHolderName;

        private ResLoader m_PreLoader;

        public Mesh mesh
        {
            set
            {
                if (meshFilter != null)
                {
                    meshFilter.mesh = value;
                }
            }
        }

        public void SetMesh(string holderName, string meshName)
        {
            if (m_MeshHolderName == holderName)
            {
                m_MeshName = meshName;
                m_MeshHolderName = holderName;

                if (m_MeshHolder != null)
                {
                    mesh = m_MeshHolder.Find(m_MeshName);
                }
            }
            else
            {
                m_MeshHolderName = holderName;
                m_MeshName = meshName;

                LoadMesh();
            }
        }

        public void SetMaterial(string materialName)
        {
            ResLoader loader = ResLoader.Allocate("Temp");
            m_Render.material = loader.LoadSync(materialName) as Material;
        }

        public MeshFilter meshFilter
        {
            get
            {
                if (m_MeshFilter == null)
                {
                    m_MeshFilter = GetComponent<MeshFilter>();
                }

                return m_MeshFilter;
            }
        }

        protected void LoadMesh()
        {
            m_MeshHolder = null;

            ResLoader loader = ResLoader.Allocate(null);
            loader.Add2Load(m_MeshHolderName, OnMeshLoadResult);

            if (m_PreLoader != null)
            {
                m_PreLoader.Recycle2Cache();
                m_PreLoader = null;
            }

            m_PreLoader = m_Loader;

            m_Loader = loader;

            if (m_AsyncLoad)
            {
                m_Loader.LoadAsync();
            }
            else
            {
                m_Loader.LoadSync();
            }
        }

        protected void OnMeshLoadResult(bool result, IRes res)
        {
            if (result)
            {
                MeshHolder holder = res.asset as MeshHolder;
                if (holder != null)
                {
                    mesh = holder.Find(m_MeshName);
                }
            }

            if (m_PreLoader != null)
            {
                m_PreLoader.Recycle2Cache();
                m_PreLoader = null;
            }
        }

        protected void OnDestroy()
        {
            if (m_PreLoader != null)
            {
                m_PreLoader.Recycle2Cache();
            }

            if (m_Loader != null)
            {
                m_Loader.Recycle2Cache();
            }
        }
    }
}
