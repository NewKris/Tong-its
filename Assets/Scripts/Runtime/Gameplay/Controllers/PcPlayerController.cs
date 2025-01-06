using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NordicBibo.Runtime.Gameplay.Cards;
using NordicBibo.Runtime.Gameplay.Melds;
using NordicBibo.Runtime.Gameplay.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace NordicBibo.Runtime.Gameplay.Controllers {
    public class PcPlayerController : TongItsPlayer {
        [Header("PC Settings")]
        public float actionDelay;
        public UnityEvent<TongItsPlayer> onAcceptChallenge;
        public UnityEvent<TongItsPlayer> onDeclineChallenge;

        public override void StartTurn() {
            StartCoroutine(PlayTurn());
        }

        public override void EndTurn() { }

        public override void Challenge() {
            onAcceptChallenge.Invoke(this);
        }

        private IEnumerator PlayTurn() {
            WaitForSeconds wait = new WaitForSeconds(actionDelay);

            yield return wait;
            
            PickNewCard();

            yield return wait;
            
            PlayCards();
            
            yield return wait;
            
            DiscardCard();
        }

        private void PickNewCard() {
            if (TryMeldWithDiscardCard()) {
                return;
            }
            
            if (stockStack.Count > 0) {
                base.Draw();
            }
        }

        private void PlayCards() {
            // 1. Check each individual card in hand if they can be added to an exposed meld
            //     a. Reset loop each time a play is made
            // 2. If hand tally is 0 (all cards are in melds) or only one card is not in a meld,
            //      then play all melds for the Tong-its win
        }

        private void DiscardCard() {
            if (playerHand.Count > 0) {
                int leastImportant = MeldFinder.FindLeastImportantCard(playerHand.CardIndices);
                base.Discard(playerHand.IndexToCard(leastImportant));
            }
        }

        private bool TryMeldWithDiscardCard() {
            PlayingCard discardCard = discardStack.TopCard;
            
            List<int> mockHand = playerHand.CardIndices;
            mockHand.Add(discardCard.Index);

            List<Meld> melds = MeldFinder.FindValidMelds(mockHand, out List<int> junk);

            if (junk.Contains(discardCard.Index)) {
                return false;
            }

            Meld meldFromDiscardCard = melds.First(m => m.Contains(discardCard.Index));
            List<PlayingCard> cardsToMeld = playerHand.IndicesToCards(meldFromDiscardCard.cards);
            cardsToMeld.Add(discardCard);
            
            meldCreator.CreateMeld(cardsToMeld, meldCreator.FirstEmptyStack);

            return true;
        }
    }
}