using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NordicBibo.Runtime.Gameplay.Cards;
using NordicBibo.Runtime.Gameplay.Chips.Simple;
using NordicBibo.Runtime.Gameplay.Controllers;
using NordicBibo.Runtime.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace NordicBibo.Runtime.Gameplay {
    public class TongItsEngine : MonoBehaviour {
        public CardDeck cardDeck;
        public Dealer dealer;
        public TongItsPlayer[] players;
        
        [Header("Betting")]
        public ChipHolder bettingPile;
        public ChipHolder jackpotPile;
        public int bettingCount;
        public int jackpotCount;

        private int _playerTurn;
        
        public void StartNewRound() {
            if (!cardDeck.HasSpawnedCards) {
                cardDeck.SpawnCards();
            }
            
            StartCoroutine(SetUpRound());
        }
        
        public void EndByDraw() {
            StartNewRound();
        }

        private void Awake() {
            TongItsPlayer.OnDiscard += EndPlayerTurn;
            TongItsPlayer.OnHandEmptied += EndByTongIts;
        }

        private void OnDestroy() {
            TongItsPlayer.OnDiscard -= EndPlayerTurn;
            TongItsPlayer.OnHandEmptied -= EndByTongIts;
        }

        private void EndPlayerTurn() {
            players[_playerTurn].EndTurn();

            if (cardDeck.Count == 0) {
                EndByStockOut();
                return;
            }
            
            _playerTurn = (_playerTurn + 1) % players.Length;
            players[_playerTurn].StartTurn();
        }
        
        private void EndByTongIts() {
            StartNewRound();
        }

        private void EndByStockOut() {
            StartNewRound();
        }

        private IEnumerator SetUpRound() {
            if (!HasValidGameParameters()) {
                Debug.LogWarning("Invalid game parameters!");
                yield break;
            }

            WaitForSeconds actionPadding = new WaitForSeconds(0.25f);

            if (!cardDeck.HasAllCards) {
                yield return dealer.ReturnAllCards();
                yield return actionPadding;
            }

            PlaceBets();
            PlaceJackpot();
            
            cardDeck.Shuffle();
            HideOpponentCards();
            
            yield return dealer.DealCards(players);

            yield return actionPadding;

            _playerTurn = 0;
            players[0].StartTurn();
        }
        
        private IEnumerator TallyHands() {
            yield return new WaitForSeconds(5);
        }

        private void PlaceBets() {
            foreach (TongItsPlayer tongItsPlayer in players) {
                ChipHolder.MoveChips(tongItsPlayer.chips, bettingPile, bettingCount);
            }
        }

        private void PlaceJackpot() {
            foreach (TongItsPlayer tongItsPlayer in players) {
                ChipHolder.MoveChips(tongItsPlayer.chips, jackpotPile, jackpotCount);
            }
        }
        
        private void RevealPlayerCards() {
            foreach (TongItsPlayer tongItsPlayer in players) {
                if (tongItsPlayer.isMainPlayer) continue;

                tongItsPlayer.playerHand.SetCardFaceUp(true);
            }
        }

        private void HideOpponentCards() {
            foreach (TongItsPlayer tongItsPlayer in players) {
                if (tongItsPlayer.isMainPlayer) continue;
                
                tongItsPlayer.playerHand.SetCardFaceUp(false);
            }
        }
        
        private bool HasValidGameParameters() {
            return cardDeck.TotalCount - (players.Length * dealer.cardsPerPlayer) > 0;
        }
    }
}