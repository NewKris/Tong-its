using System;
using TMPro;
using UnityEngine;

namespace NordicBibo.Runtime.Common.Settings.Graphics {
    public class GraphicsConfirmationPopup : MonoBehaviour {
        public float waitTime;
        public TextMeshProUGUI timerText;

        private float _timer;
        private ScreenConfig _fallbackSettings;

        public void Open(ScreenConfig fallbackSettings) {
            _fallbackSettings = fallbackSettings;
            _timer = waitTime;
            
            gameObject.SetActive(true);
        }

        public void KeepSettings() {
            gameObject.SetActive(false);
        }

        public void RevertSettings() {
            GraphicsController.ApplyScreenSettings(_fallbackSettings);
            gameObject.SetActive(false);
        }

        private void Update() {
            _timer = Mathf.Max(_timer - Time.deltaTime, 0);
            timerText.text = $"Changes will revert in <b>{_timer:0}</b> seconds";

            if (_timer > 0) {
                return;
            }
            
            RevertSettings();
        }
    }
}