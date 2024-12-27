using System;
using NordicBibo.Runtime.Gameplay.Cards;
using NordicBibo.Runtime.Gameplay.Chips.Simple;
using UnityEngine;
using UnityEngine.VFX;

namespace NordicBibo.Runtime.Gameplay.Controllers {
    public abstract class TongItsPlayer : MonoBehaviour {
        public static event Action OnDiscard;
        public static event Action OnHandEmptied;

        public bool isMainPlayer;
        public ChipHolder chips;
        
        [Header("Stacks")]
        public CardStack playerHand;
        public CardStack discardStack;
        public CardStack stockStack;
        
        [Header("Effects")]
        public VisualEffect drawEffect;

        public abstract void StartTurn();

        public abstract void EndTurn();

        protected void Discard(PlayingCard card) {
            card.MoveCardToStack(discardStack);

            if (playerHand.Count == 0) {
                OnHandEmptied?.Invoke();
            }
            else {
                OnDiscard?.Invoke();
            }
        }

        protected void CheckForEmptyHand() {
            if (playerHand.Count == 0) {
                OnHandEmptied?.Invoke();
            }
        }

        protected void Draw() {
            stockStack.Peek().MoveCardToStack(playerHand);
            drawEffect.Play();
        }
    }
}