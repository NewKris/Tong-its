using UnityEngine;

namespace NordicBibo.Runtime.Common.Settings.Audio {
    [CreateAssetMenu(menuName = "Audio Slider Settings")]
    public class AudioSliderSettings : ScriptableObject {
        public string parameterKey;
        
        [Header("Volume")]
        public float maxDecibels;
        public float minDecibels;
        public float defaultDecibels;

        public void SaveValue(float value) {
            PlayerPrefs.SetFloat(parameterKey, value);
        }
        
        public float GetSavedValue() {
            return Mathf.Clamp(PlayerPrefs.GetFloat(parameterKey, defaultDecibels), minDecibels, maxDecibels);
        }
    }
}