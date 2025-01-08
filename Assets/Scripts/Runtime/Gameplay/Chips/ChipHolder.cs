using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NordicBibo.Runtime.Gameplay.Chips {
    public class ChipHolder : MonoBehaviour {
        public int initialCount;
        public float chipMoveIntervalSpeed;
        public GameObject chipPrefab;
        public UnityEvent<int> onChipsChanged;

        private List<Chip> _chips;
        private List<Chip> _originalChips;

        public int Count => _chips.Count;

        public void ResetChips() {
            ReturnOriginalChips();
        }

        public void RemoveChip(Chip chip) {
            _chips.Remove(chip);
            onChipsChanged.Invoke(Count);
        }

        public void AddChip(Chip chip) {
            _chips.Add(chip);
            onChipsChanged.Invoke(Count);
        }

        public void MoveChips(ChipHolder to, int moveCount) {
            int chipsToMove = Math.Min(Count, moveCount);
            StartCoroutine(MoveChipsAsync(to, chipsToMove));
        }

        public void InitializeChips() {
            _chips = new List<Chip>(initialCount);
            _originalChips = new List<Chip>(initialCount);
            
            for (int i = 0; i < initialCount; i++) {
                Chip chip = Instantiate(chipPrefab, transform, false).GetComponent<Chip>();
                chip.Initialize(this);
                
                _chips.Add(chip);
                _originalChips.Add(chip);
            }
            
            onChipsChanged.Invoke(Count);
        }

        private void ReturnOriginalChips() {
            foreach (Chip originalChip in _originalChips) {
                originalChip.Move(this);
            }
            
            onChipsChanged.Invoke(Count);
        }
        
        private IEnumerator MoveChipsAsync(ChipHolder to, int count) {
            WaitForSeconds wait = new WaitForSeconds(chipMoveIntervalSpeed);
            
            for (int i = 0; i < count; i++) {
                _chips[0].Move(to);
                onChipsChanged.Invoke(Count);
                yield return wait;
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.2f);
        }
    }
}