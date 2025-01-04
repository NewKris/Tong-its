using System;
using TMPro;
using UnityEngine;

namespace NordicBibo.Runtime.Common.Settings.Graphics {
    public class GraphicsController : MonoBehaviour {
        private static ScreenConfig CurrentScreenSettings;
        private const string RESOLUTION_WIDTH_KEY = "Resolution:Width";
        private const string RESOLUTION_HEIGHT_KEY = "Resolution:Height";
        private const string FULL_SCREEN_KEY = "FullScreen";

        public ResolutionSetting defaultResolution;

        [Header("UI")] 
        public ResolutionDropdown resolutionDropdown;
        public FullScreenDropdown fullScreenDropdown;
        public GraphicsConfirmationPopup confirmationPopup;

        public static void ApplyScreenSettings(ScreenConfig config) {
            CurrentScreenSettings = config;
            
            PlayerPrefs.SetInt(RESOLUTION_WIDTH_KEY, config.resolution.x);
            PlayerPrefs.SetInt(RESOLUTION_HEIGHT_KEY, config.resolution.y);
            PlayerPrefs.SetInt(FULL_SCREEN_KEY, (int)config.fullScreenMode);
            
            Screen.SetResolution(config.resolution.x, config.resolution.y, config.fullScreenMode);
        }

        public void TryApplyGraphicsSettings() {
            confirmationPopup.Open(CurrentScreenSettings);
            ApplyScreenSettings(CreateScreenConfigFromInput());
        }
        
        public void LoadGraphicsSettings() {
            ApplyScreenSettings(CreateScreenConfigFromPlayerPrefs());
        }

        private ScreenConfig CreateScreenConfigFromPlayerPrefs() {
            return new ScreenConfig(
                CreateResolutionSettingFromPlayerPrefs(),
                (FullScreenMode)PlayerPrefs.GetInt(FULL_SCREEN_KEY, 0)
            );
        }

        private Vector2Int CreateResolutionSettingFromPlayerPrefs() {
            return new Vector2Int() {
                x = PlayerPrefs.GetInt(RESOLUTION_WIDTH_KEY, defaultResolution.width),
                y = PlayerPrefs.GetInt(RESOLUTION_HEIGHT_KEY, defaultResolution.height)
            };
        }

        private ScreenConfig CreateScreenConfigFromInput() {
            return new ScreenConfig(
                resolutionDropdown.Value,
                fullScreenDropdown.Value
            );
        }
    }
}