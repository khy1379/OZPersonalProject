using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace ReadingStrike.Manager
{
    public class SoundManager : MonoBehaviour
    {
        public enum AudioType
        {
            Master,
            BGM,
            SFX
        }
        [Serializable]
        public struct VolumeValueData
        {
            public AudioType type;
            public float volume;
            public bool isMute;
            public VolumeValueData(AudioType type, float volume, bool isMute)
            {
                this.type = type;
                this.volume = volume;
                this.isMute = isMute;
            }
        }
        [Serializable]
        public class AudioGroupSet
        {
            public VolumeValueData data;
            public Slider slider;
            public Toggle muteToggle;
            public void Init(AudioMixer mixer)
            {
                if (slider != null)
                {
                    slider.value = data.volume;
                    VolumeSliderControl(mixer, data.volume);
                    slider.onValueChanged.AddListener((float value) => VolumeSliderControl(mixer, value));
                }
                if (muteToggle != null)
                {
                    muteToggle.isOn = data.isMute;
                    MuteToggleControl(mixer, data.isMute);
                    muteToggle.onValueChanged.AddListener((bool isMute) => MuteToggleControl(mixer, isMute));
                }
            }
            public void LoadData(VolumeValueData loadData) { data = loadData; }
            public void VolumeSliderControl(AudioMixer mixer, float value)
            {
                data.volume = value;
                if(!data.isMute)
                {
                    VolumeSet(mixer, value);
                }
            }
            void VolumeSet(AudioMixer mixer, float value)
            {
                float db = Mathf.Log10(Mathf.Max(0.0001f, value)) * 20;
                mixer.SetFloat(data.type.ToString(), db);
            }
            public void MuteToggleControl(AudioMixer mixer, bool isToggle)
            {
                data.isMute = isToggle;
                if(isToggle)
                {
                    VolumeSet(mixer, 0.0001f);
                }
                else
                {
                    VolumeSet(mixer, data.volume);
                }
            }
        }

        [SerializeField] private AudioMixer mixer;
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private List<AudioClip> bgmList;
        [SerializeField] private List<AudioGroupSet> audioGroupSetList;
        private void Awake()
        {
            GameManager.instance.AddRequestGameInit(Init);
        }
        void Init() 
        {
            BGMChange(0); 

            if(mixer!=null)
            {
                foreach(AudioGroupSet audio in audioGroupSetList)
                {
                    audio.Init(mixer);
                }
            }
            GameManager.instance.AddRequestSceneChange(BGMChange);
        }
        public void BGMChange(int index)
        {
            if (!IsBGMChangePossible(index)) return;

            bgmSource.clip = bgmList[index];
            bgmSource.Play();
        }
        bool IsBGMChangePossible(int index)
        {
            if(bgmList.Count <= index)
            {
                Debug.Log("없는 번호의 bgm입니다.");
                return false;
            }
            else if (bgmSource.clip == bgmList[index])
            {
                Debug.Log("현재 재생중인 bgm입니다.");
                return false;
            }
            else
                return true;
        }
    }
}