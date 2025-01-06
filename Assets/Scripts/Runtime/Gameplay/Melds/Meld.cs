using System;
using System.Collections.Generic;
using NordicBibo.Runtime.Gameplay.Utility;

namespace NordicBibo.Runtime.Gameplay.Melds {
    public struct Meld {
        public List<int> cards;

        public int Tally => PointCalculator.TallyCards(cards);
        public int this[Index i] => cards[i];

        public bool Contains(int index) {
            return cards.Contains(index);
        }
        
        public bool CardComplementsMeld(int card) {
            cards.Add(card);
            bool isStillValid = MeldEvaluator.IsValidMeld(cards);
            cards.RemoveAt(cards.Count - 1);
            
            return isStillValid;
        }
    }
}