using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay {
    public class CardCountDisplay : MonoBehaviour {
        public int maxCards;
        
        public void UpdateDisplay(List<PlayingCard> cards) {
            GetComponent<TextMeshProUGUI>().text = $"{cards.Count}/{maxCards}";
        }
    }
}
