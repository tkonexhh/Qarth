// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace DoozyUI
{
    [DisallowMultipleComponent]
    public class Soundy : MonoBehaviour
    {

        public AudioMixerGroup audioMixerGroup = null;
        public float masterVolume = 1f;
        public float masterPitch = 1f;
        public int numberOfChannels = 20;

        private GameObject channelsHolder = null;
        private List<AudioSource> channels = new List<AudioSource>();

        private Dictionary<string, UISound> soundsDatabase = new Dictionary<string, UISound>();

        private void Awake()
        {
            CreateSoundChannels(numberOfChannels);
        }

        private void CreateSoundChannels(int channelCount = 20)
        {
            channelsHolder = new GameObject("SoundChannels");
            channelsHolder.transform.parent = transform;
            channels = new List<AudioSource>();
            for (int i = 0; i < numberOfChannels; i++)
            {
                channels.Add(channelsHolder.AddComponent<AudioSource>());
                channels[i].playOnAwake = false;
                channels[i].volume = masterVolume;
                if(audioMixerGroup != null)
                {
                    channels[i].outputAudioMixerGroup = audioMixerGroup;
                }
            }
        }

        private AudioSource GetChannel
        {
            get
            {
                for (int i = 0; i < channels.Count; i++)
                {
                    if (!channels[i].isPlaying) { return channels[i]; }
                }
                return null;
            }
        }

        private void InitSoundsDatabase()
        {
            if (soundsDatabase != null) { return; }
            soundsDatabase = new Dictionary<string, UISound>();
        }

        /// <summary>
        /// Retrurns true if the soundName was found or was added to the database
        /// </summary>
        /// <param name="soundName"></param>
        /// <returns></returns>
        private bool AddSoundToDatabase(string soundName)
        {
            InitSoundsDatabase();
            if (soundsDatabase.ContainsKey(soundName)) { return true; }
            UISound soundItem = DUI.GetUISound(soundName);
            if (soundItem == null) { Debug.Log("NULL UISound"); return false; } //the UISound was not found in the Resources folder -> thus it could not be added
            soundsDatabase.Add(soundName, soundItem);
            return true;
        }

        public void PlaySound(AudioClip aClip)
        {
            PlaySound(aClip, masterVolume, 1);
        }
        public void PlaySound(AudioClip aClip, float volumePercentage)
        {
            PlaySound(aClip, volumePercentage, 1);
        }
        public void PlaySound(AudioClip aClip, float volumePercentage, float pitch)
        {
            if (aClip == null) { return; }
            AudioSource aSource = GetChannel;
            if (aSource == null) { return; }
            aSource.clip = aClip;
            aSource.volume = volumePercentage;
            aSource.pitch = pitch;
            aSource.Play();
        }

        public void PlaySound(string soundName)
        {
#if dUI_MasterAudio
            DarkTonic.MasterAudio.MasterAudio.PlaySoundAndForget(soundName);
#else
            PlaySound(soundName, masterVolume, masterPitch);
#endif
        }
        public void PlaySound(string soundName, float volumePercentage)
        {
#if dUI_MasterAudio
            DarkTonic.MasterAudio.MasterAudio.PlaySoundAndForget(soundName, volumePercentage);
#else
            PlaySound(soundName, volumePercentage, masterPitch);
#endif
        }
        public void PlaySound(string soundName, float volumePercentage, float pitch)
        {
            if (string.IsNullOrEmpty(soundName)) { return; }

#if dUI_MasterAudio
            DarkTonic.MasterAudio.MasterAudio.PlaySoundAndForget(soundName, volumePercentage, pitch);
#else
            AudioClip audioClip = null;
            if (AddSoundToDatabase(soundName)) //the soundName was either found or just added to the soundsDatabase
            {
                audioClip = soundsDatabase[soundName].audioClip; //we check if we have an AudioClip reference for it
            }
            if (audioClip != null)
            {
                PlaySound(audioClip, volumePercentage, pitch); //we have an AudioClip reference -> play the AudioClip
            }
            else
            {
                PlaySoundFromResources(soundName, volumePercentage, pitch); //we do not have an AudioClip reference -> try to play the sound from Resources -> it it fails, a debug log will get printed to the console
            }
#endif
        }

        public void PlaySoundFromResources(string soundName)
        {
            PlaySoundFromResources(soundName, masterVolume, masterPitch);
        }
        public void PlaySoundFromResources(string soundName, float volume)
        {
            PlaySoundFromResources(soundName, volume, masterPitch);
        }
        public void PlaySoundFromResources(string soundName, float volume, float pitch)
        {
            if (string.IsNullOrEmpty(soundName)) { return; }
            AudioClip clip = Resources.Load(soundName) as AudioClip;
            if (clip == null) { Debug.Log("[Soundy] There is no sound file with the name " + soundName + "' in any of the Resources folders.\n Check that the spelling of the fileName (without the extension) is correct or if the file exists in under a Resources folder"); return; }
            PlaySound(clip, volume, pitch);
        }
    }
}
