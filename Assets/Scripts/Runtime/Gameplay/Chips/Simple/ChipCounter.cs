using System;
using TMPro;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Chips.Simple {
    public class ChipCounter : MonoBehaviour {
        public ChipHolder chipHolder;
        public float countTimer = 0.1f;

        private int _currentCount;
        private float _timer;
        private TextMeshProUGUI _display;

        private void Awake() {
            _display = GetComponent<TextMeshProUGUI>();
            _display.text = chipHolder.initialCount.ToString();
        }

        private void Update() {
            _timer += Time.deltaTime;
            
            if (_timer < countTimer || _currentCount == chipHolder.Chips) return;

            _timer %= countTimer;
            
            int d = Math.Sign(chipHolder.Chips - _currentCount);
            _currentCount += d;
            _display.text = _currentCount.ToString();
        }
    }
}
