using UnityEngine;

namespace NordicBibo.Runtime.Gameplay {
    public class GlobalAudioPlayer : MonoBehaviour {
        public AudioSource[] chipSoundSources;
        
        public void PlayChipSound() {
            int i = Random.Range(0, chipSoundSources.Length);
            chipSoundSources[i].Play();
        }
    }
}