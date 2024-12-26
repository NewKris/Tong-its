using System;
using System.Collections.Generic;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Chips {
    public class ChipStack : MonoBehaviour {
        public int maxChips;
        public Vector3 offset;
        public GameObject chipPrefab;
        
        [Header("Debug")]
        public Vector3 cubeSize;
        public Color fromColor;
        public Color toColor;
        
        private Stack<Chip> _chips;

        public bool IsFull => _chips.Count >= maxChips;
        public int Count => _chips.Count;
        
        public Chip Pop() {
            return _chips.Pop();
        }

        public void Push(Chip chip) {
            chip.TargetPosition = CreatePivotPosition(_chips.Count);
            _chips.Push(chip);
        }

        public void Initialize(int count) {
            for (int i = 0; i < count; i++) {
                Vector3 spawnPos = CreatePivotPosition(i);
                Chip chip = Instantiate(chipPrefab, spawnPos, Quaternion.identity).GetComponent<Chip>();
                chip.TargetPosition = spawnPos;
            }
        }

        private Vector3 CreatePivotPosition(int index) {
            return transform.position + offset * index;
        }

        private void OnDrawGizmos() {
            for (int i = 0; i < maxChips; i++) {
                Gizmos.color = Color.Lerp(fromColor, toColor, i / (float)maxChips);
                Gizmos.DrawCube(CreatePivotPosition(i), cubeSize);
            }
        }
    }
}
