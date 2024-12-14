using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay {
    public static class MeldEvaluator {
        // TODO Support wildcards
        // TODO Support both high-ace and low-ace
        
        private const int COLUMN_COUNT = 13;
        private const int ROW_COUNT = 4;
        private const int CARD_COUNT = COLUMN_COUNT * ROW_COUNT;
        
        public static bool IsValidMeld(List<int> cards) {
            if (cards.Count < 3) {
                return false;
            }

            return CardsAreStraightFlush(cards) || CardsAreSameRank(cards);
        }

        private static bool CardsAreStraightFlush(List<int> cards) {
            int baseRow = GetRow(cards[0]);
            bool allAreSameRow = cards.All(card => GetRow(card) == baseRow);
            
            cards.Sort();
            bool allAreConsecutive = true;
            int previous = cards[0];
            
            for (int i = 1; i < cards.Count; i++) {
                if (cards[i] - previous == 1) {
                    continue;
                }
                
                allAreConsecutive = false;
                break;
            }

            return allAreSameRow && allAreConsecutive;
        }
        
        private static bool CardsAreSameRank(List<int> cards) {
            int baseColumn = GetColumn(cards[0]);
            return cards.All(card => GetColumn(card) == baseColumn);
        }

        private static int GetRow(int index) {
            return index / COLUMN_COUNT;
        }

        private static int GetColumn(int index) {
            return index % COLUMN_COUNT;
        }
    }
}