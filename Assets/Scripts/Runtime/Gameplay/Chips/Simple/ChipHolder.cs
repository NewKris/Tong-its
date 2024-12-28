using System;
using UnityEngine;
using UnityEngine.Events;

namespace NordicBibo.Runtime.Gameplay.Chips.Simple {
    public class ChipHolder : MonoBehaviour {
        public int initialCount;
        public UnityEvent<int> onChipsChanged;
        
        public int Chips { get; private set; }

        public static void MoveChips(ChipHolder from, ChipHolder to, int count) {
            int chipsToMove = Math.Min(from.Chips, count);
            
            from.Chips -= chipsToMove;
            to.Chips += chipsToMove;

            from.onChipsChanged.Invoke(from.Chips);
            to.onChipsChanged.Invoke(to.Chips);
        }
        
        private void Awake() {
            Chips = initialCount;
        }

        private void Start() {
            onChipsChanged.Invoke(Chips);
        }
    }
}