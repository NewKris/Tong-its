using System.Collections.Generic;
using System.Linq;
using NordicBibo.Runtime.Gameplay.Cards;
using NordicBibo.Runtime.Gameplay.Melds;
using NordicBibo.Runtime.Gameplay.Utility;
using TMPro;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Ui {
    public class PointDisplay : MonoBehaviour {
        public void UpdateDisplay(CardStack stack) {
            List<Meld> _ = MeldFinder.FindValidMelds(stack.CardIndices, out List<int> junk);
            
            int sum = junk.Sum(PointCalculator.IndexToPoint);
            GetComponent<TextMeshProUGUI>().text = sum.ToString();
        }
    }
}