using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NordicBibo.Runtime.Gameplay {
    public class PlayerController : MonoBehaviour {
        [Header("Buttons")] 
        public Button drawButton;
        public Button meldButton;
        public Button discardButton;
        
        [Header("Stacks")]
        public CardStack playerHand;
        public CardStack discardStack;
        public CardStack stockStack;
        
        private readonly List<PlayingCard> _selectedCards = new List<PlayingCard>(16);

        public void StartTurn() {
            playerHand.SetInteractable(true);
            discardStack.SetLatestCardInteractable(true);
            stockStack.SetLatestCardInteractable(true);
        }

        public void EndTurn() {
            drawButton.interactable = false;
            meldButton.interactable = false;
            discardButton.interactable = false;
            
            playerHand.SetInteractable(false);
            discardStack.SetLatestCardInteractable(false);
            stockStack.SetLatestCardInteractable(false);
        }
        
        private void OnEnable() {
            PlayingCard.OnCardSelected += SelectCard;
            PlayingCard.OnCardDeSelected += DeSelectCard;
        }

        private void OnDisable() {
            PlayingCard.OnCardSelected -= SelectCard;
            PlayingCard.OnCardDeSelected -= DeSelectCard;
        }

        private void SelectCard(PlayingCard card) {
            _selectedCards.Add(card);
            UpdateButtons();
        }

        private void DeSelectCard(PlayingCard card) {
            _selectedCards.Remove(card);
            UpdateButtons();
        }

        private void UpdateButtons() {
            discardButton.interactable = CanDiscardSelection();
            meldButton.interactable = CanMeldSelection();
            drawButton.interactable = CanDrawSelection();
        }
        
        private PlayingCard GetCardToDiscard() {
            return _selectedCards.FirstOrDefault(card => card.ParentStack == playerHand);
        }

        private List<PlayingCard> GetCardsToMeld() {
            return _selectedCards
                .Where(card => card.ParentStack != stockStack)
                .ToList();
        }

        private PlayingCard GetCardToDraw() {
            return _selectedCards.FirstOrDefault(card => card.ParentStack == stockStack);
        }
        
        private bool CanDrawSelection() {
            return _selectedCards.Any(card => card.ParentStack == stockStack);
        }
        
        private bool CanMeldSelection() {
            List<int> cardsInPlayerHand = _selectedCards
                .Where(card => card.ParentStack != stockStack)
                .Select(card => card.Index)
                .ToList();

            return MeldEvaluator.IsValidMeld(cardsInPlayerHand);
        }
        
        private bool CanDiscardSelection() {
            return _selectedCards.Count(card => card.ParentStack == playerHand) == 1;
        }
    }
}