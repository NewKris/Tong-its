using System;
using UnityEngine;
using UnityEngine.VFX;

namespace NordicBibo.Runtime.Gameplay.Controllers {
    public abstract class TongItsPlayer : MonoBehaviour {
        public static event Action OnDiscard;
        
        public VisualEffect drawEffect;

        public abstract void StartTurn();

        public abstract void EndTurn();

        protected void Discard() {
            OnDiscard?.Invoke();
        }

        protected void Draw() {
            drawEffect.Play();
        }
    }
}