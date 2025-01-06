using System;
using System.Collections.Generic;
using System.Linq;
using NordicBibo.Runtime.Gameplay.Cards;
using NordicBibo.Runtime.Gameplay.Chips.Simple;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

namespace NordicBibo.Runtime.Gameplay.Controllers {
    public abstract class TongItsPlayer : MonoBehaviour {
        public static event Action<TongItsPlayer> OnDiscard;
        public static event Action<TongItsPlayer> OnHandEmptied;

        public MeldCreator meldCreator;
        public ChipHolder chips;
        public bool isHuman;
        
        [Header("Stacks")]
        public CardStack playerHand;
        public CardStack discardStack;
        public CardStack stockStack;

        [Header("UI")] 
        public TextMeshProUGUI pointDisplay;
        
        [Header("Effects")]
        public VisualEffect drawEffect;
        public Image chipSprite;

        private bool _isNextWinner;
        
        public float StockDrawTime { get; private set; }
        public float DrawChallengeTime { get; set; }
        public int Tally { get; private set; }
        public bool Busted => chips.Chips <= 0 && !_isNextWinner;
        
        public abstract void StartTurn();
        public abstract void EndTurn();
        public abstract void Challenge();

        public void RevealPoints() {
            if (!pointDisplay) {
                return;
            }
            
            pointDisplay.text = Tally.ToString();
            pointDisplay.gameObject.SetActive(true);
        }

        public void HidePoints() {
            if (!pointDisplay) {
                return;
            }
            
            pointDisplay.gameObject.SetActive(false);
        }
        
        public void ForcePlayMelds() {
            // TODO: Forcefully expose all melds in hand
        }
        
        public void SetPotentialWinnerStatus(bool isNextWinner) {
            _isNextWinner = isNextWinner;
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
            playerHand.onStackUpdated.AddListener(_ => Tally = playerHand.CalculateTally());
        }
    }
}