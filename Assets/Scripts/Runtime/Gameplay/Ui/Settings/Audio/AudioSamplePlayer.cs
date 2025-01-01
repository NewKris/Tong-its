using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Ui.Settings.Audio {
    public class AudioSamplePlayer : MonoBehaviour {
        public float cooldown;

        private float _timer;
        
        public void Play() {
            if (_timer > 0) return;
            
            _timer = cooldown;
            GetComponent<AudioSource>().Play();
        }
        
        private void Update() {
            _timer -= Time.deltaTime;
        }
    }
}