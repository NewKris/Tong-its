using System;
using System.Collections;
using UnityEngine;

namespace NordicBibo.Runtime {
    public class MusicPlayer : MonoBehaviour {
        private static MusicPlayer Instance;
        
        public AudioClip defaultMusic;
        public AudioSource source;
        public float musicFadeTime;
        
        private void Awake() {
            if (Instance) {
                Destroy(gameObject);
            }
            else {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                StartCoroutine(StartPlayMusic(defaultMusic));
            }
        }

        private void OnDestroy() {
            if (Instance == this) {
                Instance = null;
            }
        }

        private IEnumerator StartPlayMusic(AudioClip music) {
            if (source.isPlaying) {
                yield return FadeVolume(1, 0, musicFadeTime);
            }
            
            source.Stop();
            
            source.clip = music;
            
            source.Play();

            yield return FadeVolume(0, 1, musicFadeTime);
        }

        private IEnumerator FadeVolume(float from, float to, float duration) {
            float t = 0;
            while (t < duration) {
                source.volume = Mathf.Lerp(from, to, t / duration);
                
                t += Time.deltaTime;
                yield return null;
            }

            source.volume = to;
        }
    }
}
