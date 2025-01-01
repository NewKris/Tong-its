using System;
using UnityEngine;
using UnityEngine.Audio;

namespace NordicBibo.Runtime.Gameplay.Ui.Settings.Audio {
    [Serializable]
    public struct AudioParameters {
        public float maxValue;
        public float minValue;
        public float defaultValue;
    }
    
    public class AudioController : MonoBehaviour {
        private const string MASTER_VOLUME_KEY = "MasterVolume";
        private const string MUSIC_VOLUME_KEY = "MusicVolume";
        private const string SFX_VOLUME_KEY = "SfxVolume";

        public AudioMixer mixer;

        public AudioParameters master;
        public AudioParameters music;
        public AudioParameters sfx;

        public float MasterVolume {
            get => GetVolume(MASTER_VOLUME_KEY);
            set => SetVolume(MASTER_VOLUME_KEY, master, value);
        }

        public float MasterVolumePercentage {
            get => GetVolumePercentage(MasterVolume, master);
            set => SetVolumePercentage(MASTER_VOLUME_KEY, master, value);
        }

        public float MusicVolume {
            get => GetVolume(MUSIC_VOLUME_KEY);
            set => SetVolume(MUSIC_VOLUME_KEY, music, value);
        }

        public float MusicVolumePercentage {
            get => GetVolumePercentage(MusicVolume, music);
            set => SetVolumePercentage(MUSIC_VOLUME_KEY, music, value);
        }

        public float SfxVolume {
            get => GetVolume(SFX_VOLUME_KEY);
            set => SetVolume(SFX_VOLUME_KEY, sfx, value);
        }

        public float SfxVolumePercentage {
            get => GetVolumePercentage(SfxVolume, sfx);
            set => SetVolumePercentage(SFX_VOLUME_KEY, sfx, value);
        }
        
        public void LoadAudioSettings() {
            MasterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, master.defaultValue);
            MusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, music.defaultValue);
            SfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, sfx.defaultValue);
        }

        private float GetVolume(string parameterKey) {
            mixer.GetFloat(parameterKey, out float value);
            return value;
        }

        private float GetVolumePercentage(float currentVolume, AudioParameters parameters) {
            return (currentVolume - parameters.minValue) / (parameters.maxValue - parameters.minValue);
        }
        
        private void SetVolume(
            string parameterKey, 
            AudioParameters parameters, 
            float value
        ) {
            float val = Mathf.Clamp(value, parameters.minValue, parameters.maxValue);
            
            mixer.SetFloat(parameterKey, val);
            PlayerPrefs.SetFloat(parameterKey, val);
        }

        private void SetVolumePercentage(
            string parameterKey, 
            AudioParameters parameters, 
            float value
        ) {
            float val = Mathf.Lerp(parameters.minValue, parameters.maxValue, value);
            SetVolume(parameterKey, parameters, val);
        }
    }
}