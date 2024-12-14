using System;
using System.Collections.Generic;
using System.Linq;
using NordicBibo.Runtime.Utility;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay {
    public class CardDeck : CardStack {
        [Header("Deck Options")]
        public GameObject cardPrefab;
        public bool includeJokers;
        
        public void SpawnCards() {
            int cardCount = includeJokers ? 56 : 52;
            CreateCentralStock(cardCount);
        }

        private void CreateCentralStock(int count) {
            for (int i = 0; i < count; i++) {
                PlayingCard card = Instantiate(cardPrefab, transform.position, Quaternion.identity)
                    .GetComponent<PlayingCard>();
                
                card.Initialize(i);
                this.AddCard(card, true);
            }
        }
    }
}