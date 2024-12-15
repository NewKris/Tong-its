using System;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Controllers {
    public abstract class TongItsPlayer : MonoBehaviour {
        public static event Action OnDiscard;

        public abstract void StartTurn();

        public abstract void EndTurn();

        protected void Discard() {
            OnDiscard?.Invoke();
        }
    }
}