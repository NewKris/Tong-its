using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NordicBibo.Runtime.Gameplay.Cards;
using NordicBibo.Runtime.Gameplay.Controllers;
using NordicBibo.Runtime.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace NordicBibo.Runtime.Gameplay {
    public class TongItsEngine : MonoBehaviour {
        public CardDeck cardDeck;
        public int cardsPerPlayer = 12;
        public TongItsPlayer[] players;
        public CardStack[] nonDeckStacks;

        [Header("Juice")] 
        public float dealSpeed;

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
            
            yield return ReturnCardsToStock();
            
            yield return actionPadding;
            
            cardDeck.Shuffle();
            yield return DealCards();

            yield return actionPadding;

            _playerTurn = 0;
            players[0].StartTurn();
        }
        
        private IEnumerator DealCards() {
            int maxDealCount = players.Length * cardsPerPlayer;
            int dealtCount = 0;
            int targetPlayerHand = 0;

            int audioPlays = 0;
            float t = 0;
            while (dealtCount < maxDealCount) {
                if (t > dealSpeed) {
                    PlayingCard card = cardDeck.Peek();
                    card.MoveCardToStack(players[targetPlayerHand].playerHand, audioPlays);
                    
                    targetPlayerHand = (targetPlayerHand + 1) % players.Length;
                    dealtCount++;
                    audioPlays++;
                    t = 0;
                }
                
                t += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator ReturnCardsToStock() {
            WaitForSeconds waitForSeconds = new WaitForSeconds(dealSpeed);

            int audioPlays = 0;
            
            while (!cardDeck.HasAllCards) {
                foreach (CardStack nonDeckStack in nonDeckStacks) {
                    if (nonDeckStack.Count == 0) continue;
                    
                    nonDeckStack.CardsInStack[0].MoveCardToStack(cardDeck, audioPlays);
                }

                audioPlays++;
                
                yield return waitForSeconds;
            }
        }
        
        private bool HasValidGameParameters() {
            return cardDeck.TotalCount - (players.Length * cardsPerPlayer) > 0;
        }
    }
}