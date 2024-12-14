using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NordicBibo.Runtime.Gameplay {
    public class TongItsEngine : MonoBehaviour {
        public CardDeck cardDeck;
        public int cardsPerPlayer = 12;
        public CardStack[] playerHands;
        public PlayerController playerController;

        [Header("Juice")] 
        [Tooltip("Time in seconds between dealt card")] public float dealSpeed;
        
        public void StartNewGame() {
            if (!cardDeck.HasSpawnedCards) {
                cardDeck.SpawnCards();
            }
            
            StartCoroutine(RunGame());
        }

        private IEnumerator RunGame() {
            if (!HasValidGameParameters()) {
                Debug.LogWarning("Invalid game parameters!");
                yield break;
            }
            
            yield return DealCards();

            yield return new WaitForSeconds(0.25f);
            
            playerController.StartTurn();
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