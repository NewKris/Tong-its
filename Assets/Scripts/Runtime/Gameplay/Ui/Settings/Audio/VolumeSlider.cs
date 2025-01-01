using System;
using UnityEngine;
using UnityEngine.UI;

namespace NordicBibo.Runtime.Gameplay.Ui.Settings.Audio {
    [RequireComponent(typeof(AudioSamplePlayer))]
    public class VolumeSlider : MonoBehaviour {
        public Slider slider;
        public AudioChannel targetChannel;
        public AudioController audioController;

        public void SetValue(float value) {
            switch (targetChannel) {
                case AudioChannel.MASTER:
                    audioController.MasterVolumePercentage = value;
                    break;
                case AudioChannel.MUSIC:
                    audioController.MusicVolumePercentage = value;
                    break;
                case AudioChannel.SFX:
                    audioController.SfxVolumePercentage = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void OnEnable() {
            slider.value = targetChannel switch {
                AudioChannel.MASTER => audioController.MasterVolumePercentage,
                AudioChannel.MUSIC => audioController.MusicVolumePercentage,
                AudioChannel.SFX => audioController.SfxVolumePercentage,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            slider.onValueChanged.AddListener(SetValue);
            slider.onValueChanged.AddListener(_ => GetComponent<AudioSamplePlayer>().Play());
        }

        private void OnDisable() {
            slider.onValueChanged.RemoveAllListeners();
        }
    }
}