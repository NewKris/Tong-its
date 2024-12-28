using System;
using TMPro;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Chips.Simple {
    public class ChipCounter : MonoBehaviour {
        private TextMeshProUGUI _display;

        public void UpdateDisplay(int chipCount) {
            _display.text = chipCount.ToString();
        }
        
        private void Awake() {
            _display = GetComponent<TextMeshProUGUI>();
        }
    }
}
