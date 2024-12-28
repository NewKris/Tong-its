using System;
using System.Collections.Generic;
using System.Linq;

namespace NordicBibo.Runtime.Gameplay.Utility {
    public static class MeldEvaluator {
        // TODO Support wildcards
        // TODO Support both high-ace and low-ace
        
        private const int COLUMN_COUNT = 13;
        private const int ROW_COUNT = 4;
        private const int CARD_COUNT = COLUMN_COUNT * ROW_COUNT;

        public static int FindLeastImportantCard(List<int> cards) {
            cards.Sort(SortByColumn);
            return cards[^1];
        }
        
        public static bool IsValidMeld(List<int> cards) {
            if (cards.Count < 3) {
                return false;
            }

            return CardsAreStraightFlush(cards) || CardsAreSameRank(cards);
        }

        public static List<List<int>> FindValidMelds(List<int> cards) {
            List<List<int>> melds = FindSets(cards);
            melds.AddRange(FindRuns(cards));
            
            return melds;
        }

        private static List<List<int>> FindSets(List<int> cards) {
            return new List<List<int>>();
        }
        
        private static List<List<int>> FindRuns(List<int> cards) {
            return new List<List<int>>();
        }
        
        private static bool CardsAreStraightFlush(List<int> cards) {
            int baseRow = GetRow(cards[0]);
            bool allAreSameRow = cards.All(card => GetRow(card) == baseRow);
            
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

        private static int SortByColumn(int a, int b) {
            return Math.Sign(GetColumn(a) - GetColumn(b));
        }
    }
}