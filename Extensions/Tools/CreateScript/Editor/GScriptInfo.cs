using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qarth
{
    public class GScriptInfo : ScriptableObject
    {
        [SerializeField]
        private List<ClassInfo> list = new List<ClassInfo>();

        public void SetClassInfo(string className, ScriptVersion version, FieldInfo[] infos)
        {
            list.Clear();
            foreach (var key in list)
            {
                if (key.className == className)
                {
                    key.fieldInfos = null;
                    key.fieldInfos = infos;
                    return;
                }
            }
            list.Add(new ClassInfo { className = className, version = version, fieldInfos = infos });
        }

        public FieldInfo[] GetFieldInfos(string className)
        {
            foreach (var key in list)
            {
                if (key.className == className)
                {
                    return key.fieldInfos;
                }
            }
            return null;
        }

        public ScriptVersion GetVersion(string className)
        {
            foreach (var key in list)
            {
                if (key.className == className)
                {
                    return key.version;
                }
            }
            return ScriptVersion.Mono;
        }

        [Serializable]
        private class ClassInfo
        {
            public ScriptVersion version;
            public string className;
            public FieldInfo[] fieldInfos;
        }

        [Serializable]
        public class FieldInfo
        {
            public string path;
            public FieldAttrInfo mainAttrInfo = new FieldAttrInfo();
            public List<FieldAttrInfo> subField;

        }

        [Serializable]
        public class FieldAttrInfo
        {
            public string name;
            public string type;
            public bool isEvent;
            public bool isAttr;
        }

    }
}