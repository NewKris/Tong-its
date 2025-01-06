using System;
using System.Collections.Generic;
using System.Linq;
using NordicBibo.Runtime.Gameplay.Utility;

namespace NordicBibo.Runtime.Gameplay.Melds {
    public static class MeldFinder {
        public static int FindLeastImportantCard(List<int> cards) {
            List<Meld> validMelds = FindValidMelds(cards, out List<int> junk);

            if (junk.Count > 0) {
                junk.Sort(SortByColumn);
                return junk[^1];
            }
            
            validMelds.Sort(SortByTally);
            
            foreach (Meld validMeld in validMelds) {
                validMeld.cards.Sort(SortByColumn);
            }
            
            return validMelds[0][^1];
        }
        
        public static List<Meld> FindValidMelds(List<int> cards, out List<int> junk) {
            List<Meld> melds = PopSets(cards);
            melds.AddRange(PopRuns(cards));

            junk = cards;
            return melds;
        }
        
        private static List<Meld> PopSets(List<int> cards) {
            // Group by column
            
            return new List<Meld>();
        }
        
        private static List<Meld> PopRuns(List<int> cards) {
            // Group by row
            // Group by consecutive runs of >= 3 cards
            
            return new List<Meld>();
        }
        
        private static int SortByColumn(int a, int b) {
            return Math.Sign(CardMatrix.GetColumn(a) - CardMatrix.GetColumn(b));
        }
        
        private static int SortByTally(Meld a, Meld b) {
            return Math.Sign(a.Tally - b.Tally);
        }
    }
}