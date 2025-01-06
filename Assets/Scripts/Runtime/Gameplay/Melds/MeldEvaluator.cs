using System.Collections.Generic;
using System.Linq;
using NordicBibo.Runtime.Gameplay.Utility;

namespace NordicBibo.Runtime.Gameplay.Melds {
    public static class MeldEvaluator {
        // TODO Support wildcards
        // TODO Support both high-ace and low-ace
        
        public static bool IsValidMeld(List<int> cards) {
            if (cards.Count < 3) {
                return false;
            }

            return CardsAreStraightFlush(cards) || CardsAreSameRank(cards);
        }
        
        private static bool CardsAreStraightFlush(List<int> cards) {
            int baseRow = CardMatrix.GetRow(cards[0]);
            bool allAreSameRow = cards.All(card => CardMatrix.GetRow(card) == baseRow);

            if (!allAreSameRow) {
                return false;
            }
            
            cards.Sort();
            bool allAreConsecutive = true;
            int previous = cards[0];
            
            for (int i = 1; i < cards.Count; i++) {
                if (cards[i] - previous == 1) {
                    previous = cards[i];
                    continue;
                }
              
                allAreConsecutive = false;
                break;
            }
            
            return allAreConsecutive;
        }
        
        private static bool CardsAreSameRank(List<int> cards) {
            int baseColumn = CardMatrix.GetColumn(cards[0]);
            return cards.All(card => CardMatrix.GetColumn(card) == baseColumn);
        }
    }
}