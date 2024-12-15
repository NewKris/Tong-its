using System;
using System.Collections.Generic;
using System.Linq;
using NordicBibo.Runtime.Gameplay.Utility;
using NordicBibo.Runtime.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace NordicBibo.Runtime.Gameplay.Cards {
    [RequireComponent(typeof(PivotMaster))]
    public class CardStack : MonoBehaviour {
        public bool onlyTopCard;
        public UnityEvent<List<PlayingCard>> onStackUpdated;

        private PivotMaster _pivots;
        private readonly List<PlayingCard> _cardsInStack = new List<PlayingCard>(56);

        public PlayingCard this[int i] => _cardsInStack[i];
        public int Count => _cardsInStack.Count;
        
        private bool Interactable { get; set; }

        public PlayingCard IndexToCard(int i) {
            return _cardsInStack.Find(card => card.Index == i);
        }
        
        public List<int> GetCardIndices() {
            return _cardsInStack.Select(card => card.Index).ToList();
        }
        
        public void Shuffle() {
            _cardsInStack.Shuffle();
        }

        public void Sort() {
            _cardsInStack.Sort(CompareCards);
        }

        public void SetInteractable(bool canBeInteracted) {
            Interactable = canBeInteracted;
            
            RefreshInteractables();
        }
        
        public void AddCard(PlayingCard card) {
            _cardsInStack.Add(card);
            card.ParentStack = this;
            card.SetPivot(_pivots.CreatePivot());
            
            if (onlyTopCard) {
                RefreshInteractables();
            }
            else {
                card.SetInteractable(Interactable);
            }
            
            onStackUpdated.Invoke(_cardsInStack);
        }

        public void RemoveCard(PlayingCard card) {
            _cardsInStack.Remove(card);
            card.ParentStack = null;
            _pivots.DestroyPivot(card.PopPivot());
            
            onStackUpdated.Invoke(_cardsInStack);
            
            if (onlyTopCard) {
                RefreshInteractables();
            }
        }

        public PlayingCard Peek() {
           return _cardsInStack[0];
        }

        private void Awake() {
            _pivots = GetComponent<PivotMaster>();
        }

        private void RefreshInteractables() {
            if (_cardsInStack.Count == 0) {
                return;
            }
            
            if (onlyTopCard) {
                _cardsInStack.ForEach(card => card.SetInteractable(false));
                _cardsInStack.Last().SetInteractable(Interactable);
            }
            else {
                _cardsInStack.ForEach(card => card.SetInteractable(Interactable));
            }
        }
        
        private int CompareCards(PlayingCard a, PlayingCard b) {
            return Math.Sign(b.Index - a.Index);
        }
    }
}