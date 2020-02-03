using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class AudioClipData : ScriptableObject
    {
        [SerializeField]
        private List<AudioClip> m_AudioClips;
        private Dictionary<string, AudioClip> m_AudioMap;

        public AudioClip GetRandomClip()
        {
            return m_AudioClips[RandomHelper.Range(0, m_AudioClips.Count)];
        }

        public AudioClip GetAudioClip(int index)
        {
            return m_AudioClips[index];
        }

        public List<AudioClip> audioClipList
        {
            get { return m_AudioClips; }
        }

        public bool isEmpty
        {
            get
            {
                if (m_AudioClips == null || m_AudioClips.Count == 0)
                {
                    return true;
                }
                return false;
            }
        }

        public void AddAudioClip(AudioClip clip)
        {
            if (clip == null)
            {
                return;
            }

            if (m_AudioMap == null)
            {
                m_AudioMap = new Dictionary<string, AudioClip>();
                m_AudioClips = new List<AudioClip>();
            }

            if (m_AudioMap.ContainsKey(clip.name))
            {
                return;
            }

            m_AudioMap.Add(clip.name, clip);
            m_AudioClips.Add(clip);
        }

        public void ClearAllData()
        {
            if (m_AudioMap != null)
            {
                m_AudioMap.Clear();
            }

            if (m_AudioClips != null)
            {
                m_AudioClips.Clear();
            }
        }

        private void BuildMap()
        {
            m_AudioMap = new Dictionary<string, AudioClip>();

            for (int i = m_AudioClips.Count - 1; i >= 0; --i)
            {
                if (m_AudioClips[i] != null)
                {
                    m_AudioMap.Add(m_AudioClips[i].name, m_AudioClips[i]);
                }
            }
        }

        public AudioClip Find(string clipName)
        {
            if (clipName == null)
            {
                return null;
            }

            if (m_AudioClips == null)
            {
                return null;
            }

            if (m_AudioMap == null)
            {
                BuildMap();
            }

            AudioClip result = null;
            if (m_AudioMap.TryGetValue(clipName, out result))
            {
                return result;
            }
            return null;
        }

        public void SortAsFileName()
        {
            if (m_AudioClips == null)
            {
                return;
            }

            m_AudioClips.Sort(AudioClipComparor);
        }

        protected int AudioClipComparor(AudioClip a, AudioClip b)
        {
            int va = GetNumFromFileName(a.name);
            if (va < 0)
            {
                return 0;
            }

            int vb = GetNumFromFileName(b.name);
            if (vb < 0)
            {
                return 0;
            }

            return va - vb;
        }

        protected int GetNumFromFileName(string fileName)
        {
            int index = fileName.LastIndexOf('_');
            if (index < 0)
            {
                return -1;
            }

            string numString = fileName.Substring(index + 1, fileName.Length - index - 1);
            return int.Parse(numString);
        }
    }
}
