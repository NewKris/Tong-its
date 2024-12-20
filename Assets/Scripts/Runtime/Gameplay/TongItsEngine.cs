using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NordicBibo.Runtime.Gameplay.Cards;
using NordicBibo.Runtime.Gameplay.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace NordicBibo.Runtime.Gameplay {
    public class TongItsEngine : MonoBehaviour {
        public CardDeck cardDeck;
        public int cardsPerPlayer = 12;
        public CardStack[] playerHands;
        public TongItsPlayer[] players;

        [Header("Juice")] 
        public float dealSpeed;

        private int _playerTurn;
        private bool _gameEnded;
        
        public void StartNewRound() {
            if (!cardDeck.HasSpawnedCards) {
                cardDeck.SpawnCards();
            }
            
            StartCoroutine(SetUpRound());
        }

        private void Awake() {
            TongItsPlayer.OnDiscard += EndPlayerTurn;
        }

        private void OnDestroy() {
            TongItsPlayer.OnDiscard -= EndPlayerTurn;
        }

        private void EndPlayerTurn() {
            players[_playerTurn].EndTurn();
            
            if (_gameEnded) return;
            
            _playerTurn = (_playerTurn + 1) % players.Length;
            players[_playerTurn].StartTurn();
        }
        
        private void EndByTongIts() {
        }

        private void EndByStockOut() {
            
        }

        private void EndByDraw() {
            
        }

        private IEnumerator SetUpRound() {
            if (!HasValidGameParameters()) {
                Debug.LogWarning("Invalid game parameters!");
                yield break;
            }

            _gameEnded = false;

            List<PlayingCard> cardsOutsideDeck = cardDeck.CardsOutsideDeck;
            if (cardsOutsideDeck.Count > 0) {
                yield return ReturnCardsToStock(cardsOutsideDeck);
            }
            
            cardDeck.Shuffle();
            
            yield return DealCards();

            yield return new WaitForSeconds(0.25f);

            _playerTurn = 0;
            players[0].StartTurn();
        }
        
        private IEnumerator DealCards() {
            int maxDealCount = playerHands.Length * cardsPerPlayer;
            int dealtCount = 0;
            int targetPlayerHand = 0;

            int audioPlays = 0;
            float t = 0;
            while (dealtCount < maxDealCount) {
                if (t > dealSpeed) {
                    PlayingCard card = cardDeck.Peek();
                    card.MoveCardToStack(playerHands[targetPlayerHand], audioPlays);
                    
                    targetPlayerHand = (targetPlayerHand + 1) % playerHands.Length;
                    dealtCount++;
                    audioPlays++;
                    t = 0;
                }
                
                t += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator ReturnCardsToStock(List<PlayingCard> cards) {
            WaitForSeconds waitForSeconds = new WaitForSeconds(dealSpeed);

            foreach (PlayingCard playingCard in cards) {
                playingCard.MoveCardToStack(cardDeck);
                
                yield return waitForSeconds;
            }
        }
        
        private bool HasValidGameParameters() {
            return cardDeck.Count - (playerHands.Length * cardsPerPlayer) > 0;
        }
    }
}