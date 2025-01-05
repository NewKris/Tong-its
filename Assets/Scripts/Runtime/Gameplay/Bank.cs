using System.Collections.Generic;
using NordicBibo.Runtime.Gameplay.Chips.Simple;
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
            ChipHolder.MoveChips(bettingPile, player.chips, bettingPile.Chips);
        }
        
        public void PlaceBets(List<TongItsPlayer> players) {
            foreach (TongItsPlayer tongItsPlayer in players) {
                ChipHolder.MoveChips(tongItsPlayer.chips, bettingPile, bettingCount);
            }
        }

        public void PlaceJackpot(List<TongItsPlayer> players) {
            foreach (TongItsPlayer tongItsPlayer in players) {
                ChipHolder.MoveChips(tongItsPlayer.chips, jackpotPile, jackpotCount);
            }
        }
    }
}