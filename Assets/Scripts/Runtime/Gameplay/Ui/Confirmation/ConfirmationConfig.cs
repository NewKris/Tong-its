using System;

namespace NordicBibo.Runtime.Gameplay.Ui.Confirmation {
    public readonly struct ConfirmationButtonConfig {
        public readonly string buttonText;
        public readonly Action onClick;
        
        public ConfirmationButtonConfig(string buttonText, Action onClick) {
            this.buttonText = buttonText;
            this.onClick = onClick;
        }
    }
    
    public readonly struct ConfirmationConfig {
        public readonly string title;
        public readonly string infoText;
        public readonly string cancelText;
        public readonly ConfirmationButtonConfig confirmAction;
        
        public ConfirmationConfig(
            string title, 
            string infoText,
            string cancelText,
            ConfirmationButtonConfig confirmAction 
        ) {
            this.title = title;
            this.infoText = infoText;
            this.cancelText = cancelText;
            this.confirmAction = confirmAction;
        }
    }
}