using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class MeshHolder : ScriptableObject
    {
        [SerializeField]
        private List<Mesh> m_MeshList;
        private Dictionary<string, Mesh> m_MeshMap;

        public bool isEmpty
        {
            get
            {
                if (m_MeshList == null || m_MeshList.Count == 0)
                {
                    return true;
                }
                return false;
            }
        }

        public Mesh Find(string name)
        {
            if (name == null)
            {
                return null;
            }

            if (m_MeshList == null)
            {
                return null;
            }

            if (m_MeshMap == null)
            {
                BuildMeshMap();
            }

            Mesh mesh;

            m_MeshMap.TryGetValue(name, out mesh);

            return mesh;
        }

        public void AddMesh(Mesh mesh)
        {
            if (mesh == null)
            {
                return;
            }

            if (m_MeshMap == null)
            {
                m_MeshMap = new Dictionary<string, Mesh>();
                m_MeshList = new List<Mesh>();
            }

            if (m_MeshMap.ContainsKey(mesh.name))
            {
                return;
            }

            m_MeshMap.Add(mesh.name, mesh);
            m_MeshList.Add(mesh);
        }

        public void ClearAllMesh()
        {
            if (m_MeshMap != null)
            {
                m_MeshMap.Clear();
            }

            if (m_MeshList != null)
            {
                m_MeshList.Clear();
            }
        }

        protected void BuildMeshMap()
        {
            m_MeshMap = new Dictionary<string, Mesh>();

            for (int i = 0; i < m_MeshList.Count; ++i)
            {
                m_MeshMap.Add(m_MeshList[i].name, m_MeshList[i]);
            }
        }
    }
}
