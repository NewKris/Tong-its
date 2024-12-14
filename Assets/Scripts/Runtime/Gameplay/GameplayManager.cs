using System;
using System.Collections;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay {
    public class GameplayManager : MonoBehaviour {
        public bool playOnStart;
        public CardDeck cardDeck;
        public int cardsPerPlayer = 12;
        public CardStack[] playerHands;

        [Header("Juice")] 
        [Tooltip("Time in seconds between dealt card")] public float dealSpeed;

        private void Start() {
            if (!playOnStart) {
                return;
            }
            
            cardDeck.SpawnCards();
            StartCoroutine(RunGame());
        }

        private IEnumerator RunGame() {
            if (!HasValidGameParameters()) {
                Debug.LogWarning("Invalid game parameters!");
                yield break;
            }
            
            cardDeck.Shuffle();
            
            yield return DealCards();
        }

        private IEnumerator DealCards() {
            int maxDealCount = playerHands.Length * cardsPerPlayer;
            int dealtCount = 0;
            int targetPlayerHand = 0;

            int audioPlays = 0;
            float t = 0;
            while (dealtCount < maxDealCount) {
                if (t > dealSpeed) {
                    PlayingCard card = cardDeck.Pop();
                    
                    card.PlayBatchDrawSound(audioPlays);
                    audioPlays++;
                    
                    playerHands[targetPlayerHand].AddCard(card);

                    targetPlayerHand = (targetPlayerHand + 1) % playerHands.Length;
                    
                    dealtCount++;
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
