using NordicBibo.Runtime.Common.Settings.Audio;
using NordicBibo.Runtime.Common.Settings.Graphics;
using UnityEngine;

namespace NordicBibo.Runtime.Common.Settings {
    public class SettingsLoader : MonoBehaviour {
        public GraphicsController graphicsController;
        public AudioController audioController;
        
        public void LoadPlayerSettings() {
            audioController.LoadAudioSettings();
            graphicsController.LoadGraphicsSettings();
        }
    }
}
