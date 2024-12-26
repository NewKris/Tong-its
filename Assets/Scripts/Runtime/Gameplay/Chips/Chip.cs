using System;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Chips {
    public class Chip : MonoBehaviour {
        public float moveDamping;

        private Vector3 _velocity;
        
        public Vector3 TargetPosition { get; set; }

        public Vector3 CurrentPosition {
            get => transform.position;
            set => transform.position = value;
        }

        private void Update() {
            CurrentPosition = Vector3.SmoothDamp(
                CurrentPosition, 
                TargetPosition, 
                ref _velocity, 
                moveDamping
            );
        }
    }
}