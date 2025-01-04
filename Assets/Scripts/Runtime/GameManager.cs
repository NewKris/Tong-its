using System;
using NordicBibo.Runtime.Common.Settings;
using UnityEngine;

namespace NordicBibo.Runtime {
    public class GameManager : MonoBehaviour {
        private static GameManager Instance;

        public SettingsLoader settingsLoader;
        
        private void Start() {
            if (Instance) {
                Destroy(gameObject);
            }
            else {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                settingsLoader.LoadPlayerSettings();
            }
        }
    }
}