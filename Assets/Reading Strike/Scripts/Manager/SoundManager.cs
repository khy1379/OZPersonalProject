using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        public class Volume
        {
            public AudioType type;
            public float volume;
            public bool isMute;
            public Slider slider;
            public Toggle muteToggle;
            static public AudioMixer mixer;
            public static void MixerSet(AudioMixer mixer)
            {
                Volume.mixer = mixer;
            }
            public void Init(AudioMixer mixer)
            {
                volume = SaveLoadManager.LoadDataPlF($"{type.ToString()}_Vol", 0.5f);
                isMute = SaveLoadManager.LoadDataPlB($"{type.ToString()}_Mute", false);
                if (slider != null)
                {
                    slider.value = volume;
                    VolumeSliderControl(volume);
                    slider.onValueChanged.AddListener((value) => VolumeSliderControl(value));
                }
                if (muteToggle != null)
                {
                    muteToggle.isOn = isMute;
                    MuteToggleControl(isMute);
                    muteToggle.onValueChanged.AddListener((isMute) => MuteToggleControl(isMute));
                }
            }
            public void VolumeSliderControl(float value)
            {
                volume = value;
                SaveLoadManager.SaveDataPlF($"{type.ToString()}_Vol", value);
                if (!isMute) { VolumeSet(value); }
            }
            void VolumeSet(float value)
            {
                float db = Mathf.Log10(Mathf.Max(0.0001f, value)) * 20;
                mixer.SetFloat(type.ToString(), db);
            }
            public void MuteToggleControl(bool isToggle)
            {
                isMute = isToggle;
                SaveLoadManager.SaveDataPlB($"{type.ToString()}_Mute", isToggle);
                if (isToggle) { VolumeSet(0.0001f); }
                else { VolumeSet(volume); }
            }
        }

        [SerializeField] private AudioMixer mixer;
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private List<AudioClip> bgmList;
        [SerializeField] private List<Volume> volumeList;
        private void Awake()
        {
            GameManager.instance.AddRequestGameInit(Init);
        }
        void Init()
        {
            BGMChange(0);
            Volume.MixerSet(mixer);
            if (mixer != null)
            {
                for(int i = 0; i < volumeList.Count; i++)
                {
                    volumeList[i].Init(mixer);
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
            if (bgmList.Count <= index)
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