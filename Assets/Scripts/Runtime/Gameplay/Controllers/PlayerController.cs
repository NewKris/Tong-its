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
        
        [Header("References")]
        public MeldCreator playerMeldCreator;
        
        [Header("Buttons")] 
        public Button meldButton;
        public Button discardButton;

        [Header("UI")] 
        public GameObject challengePanel;
        
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

        public override void Challenge() {
            challengePanel.SetActive(true);
        }

        public void CreateMeld() {
            MeldWithStack(_selectedCards, playerMeldCreator.FirstEmptyStack);
            _selectedCards.Clear();
            UpdateButtons();
        }
        
        public void DiscardCard() {
            PlayingCard cardToDiscard = GetCardToDiscard();
            
            _selectedCards.Remove(cardToDiscard);
            base.Discard(cardToDiscard);
        }
        
        private void OnEnable() {
            PlayingCard.OnCardSelected += SelectCard;
            PlayingCard.OnCardDeSelected += DeSelectCard;
            PlayerMouseController.OnCardStackClicked += TryMeldWithStack;
        }

        private void OnDisable() {
            PlayingCard.OnCardSelected -= SelectCard;
            PlayingCard.OnCardDeSelected -= DeSelectCard;
            PlayerMouseController.OnCardStackClicked -= TryMeldWithStack;
        }

        private void TryMeldWithStack(CardStack stack) {
            if (_selectedCards.Count == 0) return;
            
            if (CanMeldCards(_selectedCards, stack.CardsInStack)) {
                MeldWithStack(_selectedCards, stack);
                _selectedCards.Clear();
                UpdateButtons();
            }
        }

        private void MeldWithStack(List<PlayingCard> cards, CardStack stack) {
            if (cards.Any(card => card.ParentStack == discardStack)) {
                _hasDrawnCard = true;
                stockStack.SetInteractable(false);
                discardStack.SetInteractable(false);
            }
            
            playerMeldCreator.CreateMeld(cards.ToList(), stack);
            this.CheckForEmptyHand();
        }
        
        private void DrawFromStock() {
            base.Draw();
            
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
            meldButton.interactable = CanMeldCards(_selectedCards.Where(card => card.ParentStack != stockStack));
        }
        
        private PlayingCard GetCardToDiscard() {
            return _selectedCards.FirstOrDefault(card => card.ParentStack == playerHand);
        }
        
        private static bool CanMeldCards(params IEnumerable<PlayingCard>[] cards) {
            List<int> cardsInPlayerHand = cards
                .SelectMany(card => card)
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