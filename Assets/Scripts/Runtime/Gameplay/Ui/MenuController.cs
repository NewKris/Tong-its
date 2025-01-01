using NordicBibo.Runtime.Gameplay.Ui.Confirmation;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Ui {
    public class MenuController : MonoBehaviour {
        public ConfirmationWindow confirmationWindow;
        
        public void ConfirmExitGame() {
            confirmationWindow.ShowActionPrompt(new ConfirmationConfig(
                title: "Exit Game?",
                infoText: "Are you sure you want to exit the game?",
                cancelText: "Cancel",
                confirmAction: new ConfirmationButtonConfig("Exit Game", ExitGame)
            ));
        }

        private void ExitGame() {
            Debug.Log("Exiting game");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}