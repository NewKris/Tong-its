using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NordicBibo.Runtime.Gameplay.Ui.Confirmation {
    public class ConfirmationWindow : MonoBehaviour {
        public TextMeshProUGUI title;
        public TextMeshProUGUI info;
        public Button confirmButton;
        public Button cancelButton;
        
        public void ShowActionPrompt(ConfirmationConfig config) {
            title.text = config.title;
            info.text = config.infoText;

            confirmButton.GetComponentInChildren<TextMeshProUGUI>().text = config.confirmAction.buttonText;
            confirmButton.onClick.AddListener(() => {
                gameObject.SetActive(false);
                config.confirmAction.onClick();
            });

            cancelButton.GetComponentInChildren<TextMeshProUGUI>().text = config.cancelText;
            
            gameObject.SetActive(true);
        }

        private void OnDisable() {
            confirmButton.onClick.RemoveAllListeners();
        }
    }
}
