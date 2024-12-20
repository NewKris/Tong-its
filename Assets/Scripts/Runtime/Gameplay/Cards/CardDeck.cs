using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Cards {
    public class CardDeck : CardStack {
        [Header("Deck Options")]
        public GameObject cardPrefab;
        public bool includeJokers;

        private readonly List<PlayingCard> _cardsInGame = new List<PlayingCard>();

        public bool HasSpawnedCards => _cardsInGame.Count != 0;
        public bool HasAllCards => CardsInStack.Count == _cardsInGame.Count;
        public int TotalCount => _cardsInGame.Count;
        
        public void SpawnCards() {
            int cardCount = includeJokers ? 56 : 52;
            CreateCentralStock(cardCount);
        }

        private void CreateCentralStock(int count) {
            for (int i = 0; i < count; i++) {
                PlayingCard card = Instantiate(cardPrefab, transform.position, transform.rotation)
                    .GetComponent<PlayingCard>();
                
                card.Initialize(i);
                
                _cardsInGame.Add(card);
                this.AddCard(card);
            }
        }
    }
}