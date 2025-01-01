using System;
using NordicBibo.Runtime.Gameplay.Ui.Settings.Audio;
using NordicBibo.Runtime.Gameplay.Ui.Settings.Graphics;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Ui.Settings {
    public class SettingsMaster : MonoBehaviour {
        private const string FULL_SCREEN_KEY = "Graphics:Full_Screen";
        private const string RESOLUTION_KEY = "Graphics:Resolution";
        
        private static SettingsMaster Instance;

        public AudioController audioController;
        
        private void Start() {
            if (Instance) {
                Destroy(gameObject);
            }
            else {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                ApplyInitialSettings();
            }
        }

        private void OnDestroy() {
            if (Instance == this) {
                Instance = null;
            }
        }

        private void ApplyInitialSettings() {
            audioController.LoadAudioSettings();
        }
    }
}