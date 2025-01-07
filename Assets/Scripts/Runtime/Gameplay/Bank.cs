using System.Collections.Generic;
using NordicBibo.Runtime.Gameplay.Chips;
using NordicBibo.Runtime.Gameplay.Controllers;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay {
    public class Bank : MonoBehaviour {
        public ChipHolder bettingPile;
        public ChipHolder jackpotPile;
        public int bettingCount;
        public int jackpotCount;

        public void ResetChips() {
            bettingPile.ResetChips();
            jackpotPile.ResetChips();
        }

        public void PayoutPlayer(TongItsPlayer player) {
            bettingPile.MoveChips(player.chips, bettingPile.Count);
        }
        
        public void PlaceBets(List<TongItsPlayer> players) {
            foreach (TongItsPlayer tongItsPlayer in players) {
                tongItsPlayer.chips.MoveChips(bettingPile, bettingCount);
            }
        }

        public void PlaceJackpot(List<TongItsPlayer> players) {
            foreach (TongItsPlayer tongItsPlayer in players) {
                tongItsPlayer.chips.MoveChips(jackpotPile, jackpotCount);
            }
        }
    }
}