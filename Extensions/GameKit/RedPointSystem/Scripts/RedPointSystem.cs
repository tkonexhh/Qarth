/************************
	FileName:/Qarth/Extensions/GameKit/RedPointSystem/Scripts/RedPointSystem.cs
	CreateAuthor:xuhonghua
	CreateTime:3/20/2020 2:37:01 PM
************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GFrame
{
    public class RedPointSystem : TSingleton<RedPointSystem>
    {
        public delegate void OnRedNumChange();
        private List<RedPointMudule> m_LstModules = new List<RedPointMudule>();
        private Dictionary<string, RedPointMudule> m_ModuleMap = new Dictionary<string, RedPointMudule>();

        public override void OnSingletonInit()
        {

        }

        public void AddModule(string key, RedPointMudule module)
        {
            if (!m_LstModules.Contains(module))
            {
                m_LstModules.Add(module);
                m_ModuleMap.Add(key, module);
            }

            module.Init();
        }

        public RedPointMudule GetModuleByKey(string key)
        {
            RedPointMudule module = null;
            if (m_ModuleMap.TryGetValue(key, out module))
            {
                return module;
            }
            return null;
        }

        public void Refesh()
        {
            foreach (var module in m_LstModules)
            {
                module.Refesh();
            }
        }

    }


    public class RedPointMudule
    {
        private RedPointNode m_RootNode;
        public RedPointNode RootNode
        {
            get
            {
                return m_RootNode;
            }
        }

        public void Init()
        {
            m_RootNode = new RedPointNode();
        }

        public void Refesh()
        {

        }



    }

}