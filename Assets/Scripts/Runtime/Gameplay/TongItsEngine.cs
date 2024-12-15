using System;
using System.Collections;
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
        
        public void StartNewGame() {
            if (!cardDeck.HasSpawnedCards) {
                cardDeck.SpawnCards();
            }
            
            StartCoroutine(RunGameStart());
        }

        private void Awake() {
            TongItsPlayer.OnDiscard += EndPlayerTurn;
        }

        private void OnDestroy() {
            TongItsPlayer.OnDiscard -= EndPlayerTurn;
        }

        private void EndPlayerTurn() {
            players[_playerTurn].EndTurn();
            _playerTurn = (_playerTurn + 1) % players.Length;
            players[_playerTurn].StartTurn();
        }

        private IEnumerator RunGameStart() {
            if (!HasValidGameParameters()) {
                Debug.LogWarning("Invalid game parameters!");
                yield break;
            }
            
            yield return DealCards();

            yield return new WaitForSeconds(0.25f);

            _playerTurn = 0;
            players[0].StartTurn();
        }
        
        private IEnumerator DealCards() {
            cardDeck.Shuffle();
            
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
        
        private bool HasValidGameParameters() {
            return cardDeck.Count - (playerHands.Length * cardsPerPlayer) > 0;
        }
    }
}