using System;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Chips {
    public class Chip : MonoBehaviour {
        public float moveDamping = 0.02f;

        private Vector3 _velocity;
        
        public ChipHolder Holder { get; private set; }
        
        private bool IsAtRestPoint => transform.localPosition == Vector3.zero;

        public void Initialize(ChipHolder holder) {
            Holder = holder;
            transform.localPosition = Vector3.zero;
        }
        
        public void Move(ChipHolder to) {
            Holder.RemoveChip(this);

            Holder = to;
            Holder.AddChip(this);
            
            transform.SetParent(to.transform);
            
            enabled = true;
        }

        private void Start() {
            enabled = !IsAtRestPoint;
        }

        private void Update() {
            transform.localPosition = Vector3.SmoothDamp(
                transform.localPosition, 
                Vector3.zero, 
                ref _velocity, 
                moveDamping
            );

            if (IsAtRestPoint) {
                enabled = false;
            }
        }
    }
}