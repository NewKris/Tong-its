using System;
using System.Collections.Generic;
using System.Linq;
using NordicBibo.Runtime.Gameplay.Cards;
using NordicBibo.Runtime.Gameplay.Chips.Simple;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

namespace NordicBibo.Runtime.Gameplay.Controllers {
    public abstract class TongItsPlayer : MonoBehaviour {
        public static event Action<TongItsPlayer> OnDiscard;
        public static event Action<TongItsPlayer> OnHandEmptied;

        public bool isMainPlayer;
        public ChipHolder chips;
        
        [Header("Stacks")]
        public CardStack playerHand;
        public CardStack discardStack;
        public CardStack stockStack;
        
        [Header("Effects")]
        public VisualEffect drawEffect;
        public Image chipSprite;

        public abstract void StartTurn();

        public abstract void EndTurn();
        
        public float StockDrawTime { get; private set; }
        public int Tally { get; private set; }

        public void SetNextWinner(bool isNextWinner) {
            chipSprite.color = isNextWinner ? Color.yellow : Color.white;
        }
        
        protected void Discard(PlayingCard card) {
            card.MoveCardToStack(discardStack);

            if (playerHand.Count == 0) {
                OnHandEmptied?.Invoke(this);
            }
            else {
                OnDiscard?.Invoke(this);
            }
        }

        protected void CheckForEmptyHand() {
            if (playerHand.Count == 0) {
                OnHandEmptied?.Invoke(this);
            }
        }

        protected void Draw() {
            stockStack.Peek().MoveCardToStack(playerHand);
            drawEffect.Play();
            StockDrawTime = Time.time;
        }

        private void Awake() {
            playerHand.onStackUpdated.AddListener(TallyCards);
        }

        private void TallyCards(List<PlayingCard> cards) {
            // TODO: Make it not count valid melds in hand

            Tally = cards.Sum(card => card.Tally);
        }
    }
}