using System;
using System.Collections.Generic;
using System.Linq;
using NordicBibo.Runtime.Gameplay.Cards;
using NordicBibo.Runtime.Gameplay.Utility;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace NordicBibo.Runtime.Gameplay.Controllers {
    public class PlayerController : TongItsPlayer {
        // TODO Bug: If player has 1 card selected in hand at start of round and draws from stock,
        // the discard button remains not interactable
        
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

        public override void StartTurn() {
            playerHand.SetInteractable(true);
            discardStack.SetInteractable(true);
            stockStack.SetInteractable(true);

            if (discardStack.Count == 0) {
                DrawFromStock();
            }
            else {
                _hasDrawnCard = false;
            }
        }

        public override void EndTurn() {
            meldButton.interactable = false;
            discardButton.interactable = false;
            
            playerHand.SetInteractable(false);
            discardStack.SetInteractable(false);
            stockStack.SetInteractable(false);
        }

        public void CreateMeld() {
            List<PlayingCard> cards = GetCardsToMeld();
            
            if (cards.Any(card => card.ParentStack == discardStack)) {
                _hasDrawnCard = true;
                stockStack.SetInteractable(false);
                discardStack.SetInteractable(false);
            }
            
            cards.ForEach(card => _selectedCards.Remove(card));
            
            playerMeldCreator.CreateNewMeld(cards);
            UpdateButtons();
        }
        
        public void DiscardCard() {
            PlayingCard cardToDiscard = GetCardToDiscard();
            
            _selectedCards.Remove(cardToDiscard);
            cardToDiscard.MoveCardToStack(discardStack);
            
            this.Discard();
        }
        
        private void OnEnable() {
            PlayingCard.OnCardSelected += SelectCard;
            PlayingCard.OnCardDeSelected += DeSelectCard;
        }

        private void OnDisable() {
            PlayingCard.OnCardSelected -= SelectCard;
            PlayingCard.OnCardDeSelected -= DeSelectCard;
        }
        
        private void DrawFromStock() {
            base.Draw();
            
            PlayingCard card = stockStack.Peek();
            
            card.MoveCardToStack(playerHand);
            stockStack.SetInteractable(false);
            discardStack.SetInteractable(false);

            _hasDrawnCard = true;
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