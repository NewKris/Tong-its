using System.Collections;
using System.Collections.Generic;
using NordicBibo.Runtime.Gameplay.Cards;
using NordicBibo.Runtime.Gameplay.Controllers;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay {
    public class Dealer : MonoBehaviour {
        public CardDeck cardDeck;
        public CardStack discardStack;
        public CardStack[] playerHands;
        public CardStack[] melds;
        public int cardsPerPlayer;
        
        [Header("Juice")] 
        public float dealSpeed;
        
        public IEnumerator DealCards(List<TongItsPlayer> players) {
            int maxDealCount = players.Count * cardsPerPlayer;
            int dealtCount = 0;
            int targetPlayerHand = 0;

            int audioPlays = 0;
            float t = 0;
            while (dealtCount < maxDealCount) {
                if (t > dealSpeed) {
                    PlayingCard card = cardDeck.Peek();
                    card.MoveCardToStack(players[targetPlayerHand].playerHand, audioPlays);
                    
                    targetPlayerHand = (targetPlayerHand + 1) % players.Count;
                    dealtCount++;
                    audioPlays++;
                    t = 0;
                }
                
                t += Time.deltaTime;
                yield return null;
            }
        }
        
        public IEnumerator ReturnAllCards() {
            List<CardStack> stacksToClear = new List<CardStack>(20);
            
            stacksToClear.Add(discardStack);
            stacksToClear.AddRange(melds);
            stacksToClear.AddRange(playerHands);

            yield return ReturnCardsToDeck(stacksToClear);
        }

        private IEnumerator ReturnCardsToDeck(List<CardStack> stacksToClear) {
            WaitForSeconds waitForSeconds = new WaitForSeconds(dealSpeed);

            int audioPlays = 0;
            
            while (!cardDeck.HasAllCards) {
                foreach (CardStack nonDeckStack in stacksToClear) {
                    if (nonDeckStack.Count == 0) continue;
                    
                    nonDeckStack.CardsInStack[0].MoveCardToStack(cardDeck, audioPlays);
                }

                audioPlays++;
                
                yield return waitForSeconds;
            }
        }
    }
}