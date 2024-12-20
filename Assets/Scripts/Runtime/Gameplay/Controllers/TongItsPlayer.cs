using System;
using NordicBibo.Runtime.Gameplay.Cards;
using UnityEngine;
using UnityEngine.VFX;

namespace NordicBibo.Runtime.Gameplay.Controllers {
    public abstract class TongItsPlayer : MonoBehaviour {
        public static event Action OnDiscard;
        
        public VisualEffect drawEffect;
        
        [Header("Stacks")]
        public CardStack playerHand;
        public CardStack discardStack;
        public CardStack stockStack;

        public abstract void StartTurn();

        public abstract void EndTurn();

        protected void Discard(PlayingCard card) {
            card.MoveCardToStack(discardStack);
            OnDiscard?.Invoke();
        }

        protected void Draw() {
            stockStack.Peek().MoveCardToStack(playerHand);
            drawEffect.Play();
        }
    }
}