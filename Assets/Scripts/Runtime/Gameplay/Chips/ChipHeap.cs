using System;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Chips {
    public class ChipHeap : MonoBehaviour {
        public int initialSize;
        public ChipStack[] stacks;

        [Header("Debug")]
        public float radius;
        
        public Chip Pop() {
            foreach (ChipStack chipStack in stacks) {
                if (chipStack.Count == 0) continue;

                return chipStack.Pop();
            }

            throw new HeapIsEmptyException();
        }

        public void Push(Chip chip) {
            foreach (ChipStack chipStack in stacks) {
                if (chipStack.IsFull) continue;
                
                chipStack.Push(chip);
                return;
            }

            throw new HeapIsFullException();
        }

        private void Start() {
            int remainingChips = initialSize;
            
            foreach (ChipStack chipStack in stacks) {
                if (remainingChips <= 0) break;
                
                int chipsToSpawn = Math.Min(remainingChips, chipStack.maxChips);
                remainingChips -= chipsToSpawn;
                
                chipStack.Initialize(chipsToSpawn);
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}