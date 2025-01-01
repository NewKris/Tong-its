using TMPro;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Ui {
    public class GameEndScreen : MonoBehaviour {
        public TextMeshProUGUI endText;
        
        public void Show(bool victory) {
            endText.text = victory ? "Victory" : "Defeat";
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }
    }
}