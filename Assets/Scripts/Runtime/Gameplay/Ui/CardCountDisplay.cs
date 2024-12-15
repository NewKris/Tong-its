using System.Collections.Generic;
using NordicBibo.Runtime.Gameplay.Cards;
using TMPro;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Ui {
    public class CardCountDisplay : MonoBehaviour {
        public int maxCards;
        
        public void UpdateDisplay(List<PlayingCard> cards) {
            GetComponent<TextMeshProUGUI>().text = $"{cards.Count}/{maxCards}";
        }
    }
}
