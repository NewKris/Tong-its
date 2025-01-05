using UnityEngine;

namespace NordicBibo.Runtime {
    public class SceneController : MonoBehaviour {
        public void LoadScene(string sceneName) {
            GameManager.LoadScene(sceneName);
        }
    }
}