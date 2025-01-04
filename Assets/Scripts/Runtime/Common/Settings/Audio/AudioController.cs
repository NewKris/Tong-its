using UnityEngine;
using UnityEngine.Audio;

namespace NordicBibo.Runtime.Common.Settings.Audio {
    public class AudioController : MonoBehaviour {
        public AudioMixer audioMixer;
        public AudioSliderSettings[] sliderSettings;
        
        public void LoadAudioSettings() {
            foreach (AudioSliderSettings settings in sliderSettings) {
                audioMixer.SetFloat(settings.parameterKey, settings.GetSavedValue());
            }
        }
    }
}