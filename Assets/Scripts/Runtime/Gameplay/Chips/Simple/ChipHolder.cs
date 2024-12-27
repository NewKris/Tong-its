using System;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Chips.Simple {
    public class ChipHolder : MonoBehaviour {
        public int initialCount;
        
        public int Chips { get; private set; }

        public static void MoveChips(ChipHolder from, ChipHolder to, int count) {
            int chipsToMove = Math.Min(from.Chips, count);
            
            from.Chips -= chipsToMove;
            to.Chips += chipsToMove;
        }
        
        private void Awake() {
            Chips = initialCount;
        }
    }
}