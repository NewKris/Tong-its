using System.Collections.Generic;
using System.Linq;
using NordicBibo.Runtime.Gameplay.Controllers;

namespace NordicBibo.Runtime {
    public static class WinnerFinder {
        public static TongItsPlayer FindStockOutWinner(List<TongItsPlayer> players) {
            List<TongItsPlayer> winnerCandidates = FindPlayerWithLowestPoints(players);

            if (winnerCandidates.Count == 1) {
                return winnerCandidates[0];
            }
            
            return FindPlayerWhoLastPickedStockCard(winnerCandidates);
        }
        
        public static TongItsPlayer FindDrawWinner(List<TongItsPlayer> players) {
            if (players.Count == 1) {
                return players[0];
            }
            
            TongItsPlayer drawCaller = players[0];
            List<TongItsPlayer> winnerCandidates = FindPlayerWithLowestPoints(players);
            
            // Player who called the draw loses on ties
            if (winnerCandidates.Count > 1 && winnerCandidates.Contains(drawCaller)) {
                winnerCandidates.Remove(drawCaller);
            }
            
            if (winnerCandidates.Count == 1) {
                return winnerCandidates[0];
            }
            
            return FindPlayerWhoFirstChallengedDraw(winnerCandidates);
        }
        
        private static List<TongItsPlayer> FindPlayerWithLowestPoints(List<TongItsPlayer> candidates) {
            int lowest = candidates.Select(tongItsPlayer => tongItsPlayer.Tally).Min();

            return candidates.Where(player => player.Tally == lowest).ToList();
        }
        
        private static TongItsPlayer FindPlayerWhoLastPickedStockCard(List<TongItsPlayer> candidates) {
            return candidates.Aggregate((a, b) => a.StockDrawTime > b.StockDrawTime ? a : b);
        }

        private static TongItsPlayer FindPlayerWhoFirstChallengedDraw(List<TongItsPlayer> candidates) {
            return candidates.Aggregate((a, b) => a.DrawChallengeTime < b.DrawChallengeTime ? a : b);
        }
    }
}