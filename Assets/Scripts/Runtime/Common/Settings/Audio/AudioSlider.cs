using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace NordicBibo.Runtime.Common.Settings.Audio {
    public class AudioSlider : MonoBehaviour {
        public AudioSliderSettings settings;
        public AudioMixer mixer;
        public AudioSamplePlayer samplePlayer;
        
        private void OnEnable() {
            Slider slider = GetComponentInChildren<Slider>();

            slider.maxValue = settings.maxDecibels;
            slider.minValue = settings.minDecibels;
            slider.value = settings.GetSavedValue();
            
            slider.onValueChanged.AddListener(UpdateValue);

            if (samplePlayer) {
                slider.onValueChanged.AddListener(_ => samplePlayer.Play());
            }
        }

        private void OnDisable() {
            GetComponentInChildren<Slider>().onValueChanged.RemoveAllListeners();
        }
        
        private void UpdateValue(float value) {
            value = Mathf.Clamp(value, settings.minDecibels, settings.maxDecibels);
            
            settings.SaveValue(value);
            mixer.SetFloat(settings.parameterKey, value);
        }
    }
}