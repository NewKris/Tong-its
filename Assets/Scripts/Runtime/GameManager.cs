using System;
using System.Collections;
using NordicBibo.Runtime.Common.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NordicBibo.Runtime {
    public class GameManager : MonoBehaviour {
        private static bool Busy;
        private static GameManager Instance;

        public SettingsLoader settingsLoader;
        public BlackScreen blackScreen;

        public static void LoadScene(string sceneName) {
            if (Busy) {
                return;
            }

            Instance.StartCoroutine(Instance.TransitionSceneAsync(sceneName));
        }
        
        private void Start() {
            if (Instance) {
                Destroy(gameObject);
            }
            else {
                Instance = this;
                Busy = false;

                DontDestroyOnLoad(gameObject);
                settingsLoader.LoadPlayerSettings();
            }
        }

        private IEnumerator TransitionSceneAsync(string toScene) {
            Busy = true;
            
            yield return blackScreen.FadeOut();

            yield return SceneManager.LoadSceneAsync(toScene);
            
            yield return blackScreen.FadeIn();

            Busy = false;
        }
    }
}