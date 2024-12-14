using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NordicBibo.Runtime.Gameplay {
    public class PlayerController : MonoBehaviour {
        public MeldCreator playerMeldCreator;
        
        [Header("Buttons")] 
        public Button meldButton;
        public Button discardButton;
        
        [Header("Stacks")]
        public CardStack playerHand;
        public CardStack discardStack;
        public CardStack stockStack;

        private bool _hasDrawnCard;
        private readonly List<PlayingCard> _selectedCards = new List<PlayingCard>(16);

        public void StartTurn() {
            playerHand.SetInteractable(true);
            discardStack.SetInteractable(true);
            stockStack.SetInteractable(true);

            _hasDrawnCard = false;
        }

        private void EndTurn() {
            meldButton.interactable = false;
            discardButton.interactable = false;
            
            playerHand.SetInteractable(false);
            discardStack.SetInteractable(false);
            stockStack.SetInteractable(false);
        }
        
        public void DrawFromStock() {
            PlayingCard card = stockStack.Peek();
            
            card.MoveCardToStack(playerHand);
            stockStack.SetInteractable(false);

            _hasDrawnCard = true;
        }

        public void CreateMeld() {
            List<PlayingCard> cards = GetCardsToMeld();
            
            if (cards.Any(card => discardStack)) {
                _hasDrawnCard = true;
                stockStack.SetInteractable(false);
            }
            
            // TODO here
        }
        
        public void DiscardCard() {
            PlayingCard cardToDiscard = GetCardToDiscard();
            
            _selectedCards.Remove(cardToDiscard);
            cardToDiscard.MoveCardToStack(discardStack);
            
            //EndTurn();
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
            if (card.ParentStack == stockStack) {
                DrawFromStock();
                return;
            }
            
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
        }
        
        private PlayingCard GetCardToDiscard() {
            return _selectedCards.FirstOrDefault(card => card.ParentStack == playerHand);
        }

        private List<PlayingCard> GetCardsToMeld() {
            return _selectedCards
                .Where(card => card.ParentStack != stockStack)
                .ToList();
        }
        
        private bool CanMeldSelection() {
            List<int> cardsInPlayerHand = _selectedCards
                .Where(card => card.ParentStack != stockStack)
                .Select(card => card.Index)
                .ToList();

            return MeldEvaluator.IsValidMeld(cardsInPlayerHand);
        }
        
        private bool CanDiscardSelection() {
            return _selectedCards.Count(card => card.ParentStack == playerHand) == 1
                && _hasDrawnCard;
        }
    }
}