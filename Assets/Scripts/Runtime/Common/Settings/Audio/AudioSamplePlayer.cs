using System;
using UnityEngine;

namespace NordicBibo.Runtime.Common.Settings.Audio {
    [RequireComponent(typeof(AudioSource))]
    public class AudioSamplePlayer : MonoBehaviour {
        public float playCooldown;

        private float _timer;
        
        public void Play() {
            if (_timer < 0) {
                GetComponent<AudioSource>().Play();
                _timer = playCooldown;
            }
        }

        private void Update() {
            _timer -= Time.deltaTime;
        }
    }
}