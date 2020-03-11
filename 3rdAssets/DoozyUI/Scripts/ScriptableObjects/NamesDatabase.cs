// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using System.Collections.Generic;
using System;

namespace DoozyUI
{
    [Serializable]
    public class NamesDatabase : ScriptableObject
    {
        public bool isDirty = true;
        public List<string> data = new List<string>();

        public void Init()
        {
            if (IsNull) { data = new List<string>(); }
            isDirty = true;
        }
        public void Add(string name)
        {
            Init();
            name = name.Trim();
            if (string.IsNullOrEmpty(name)) { return; }
            if (Contains(name)) { return; }
            data.Add(name);
            Sort();
            isDirty = true;
        }
        public void Clear()
        {
            Init();
            if (IsEmpty) { return; }
            data.Clear();
            isDirty = true;
        }
        public bool Contains(string name)
        {
            Init();
            return data.Contains(name);
        }
        public int Count
        {
            get
            {
                Init();
                return data.Count;
            }
        }
        public string GetName(int index)
        {
            Init();
            return ((index < 0) || (index > data.Count - 1)) ? "" : data[index];
        }
        public int IndexOf(string name)
        {
            Init();
            return Contains(name) ? data.IndexOf(name) : -1;
        }
        public bool IsEmpty { get { return Count == 0; } }
        public bool IsNull { get { return data == null; } }
        public void Remove(string name)
        {
            Init();
            if (IsEmpty || !Contains(name)) { return; }
            data.Remove(name);
            isDirty = true;
        }
        public void RemoveAt(int index)
        {
            Init();
            if (IsEmpty) { return; }
            if (index < 0 || index >= data.Count - 1) { return; }
            data.RemoveAt(index);
            isDirty = true;
        }
        public void Reverse()
        {
            Init();
            if (IsEmpty) { return; }
            data.Reverse();
            isDirty = true;
        }
        public void Sort()
        {
            Init();
            if (IsEmpty) { return; }
            data.Sort();
            isDirty = true;
        }
        public void RemoveEmpty()
        {
            Init();
            if (IsEmpty) { return; }
            for(int i = data.Count - 1; i >= 0; i--)
            {
                if(string.IsNullOrEmpty(data[i]))
                {
                    data.RemoveAt(i);
                }
            }
            isDirty = true;
        }
        public string[] ToArray()
        {
            Init();
            return data.ToArray();
        }
    }
}
