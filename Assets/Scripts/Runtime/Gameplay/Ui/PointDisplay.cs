using System.Collections.Generic;
using System.Linq;
using NordicBibo.Runtime.Gameplay.Cards;
using TMPro;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Ui {
    public class PointDisplay : MonoBehaviour {
        public void UpdateDisplay(List<PlayingCard> cards) {
            int sum = cards.Sum(playingCard => playingCard.Tally);
            GetComponent<TextMeshProUGUI>().text = sum.ToString();
        }
    }
}