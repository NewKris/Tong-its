using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay {
    public class MeldCreator : MonoBehaviour {
        public float moveCardsSpeed;
        public CardStack[] meldStacks;

        public void CreateNewMeld(List<PlayingCard> cards) {
            CardStack firstAvailableStack = meldStacks.First(stack => stack.Count == 0);
            StartCoroutine(AddCardsToStack(cards, firstAvailableStack));
        }

        private IEnumerator AddCardsToStack(List<PlayingCard> cards, CardStack toStack) {
            int audioPlayCount = 0;
            
            cards.ForEach(card => card.SetInteractable(false));
            
            foreach (PlayingCard playingCard in cards) {
                playingCard.MoveCardToStack(toStack, audioPlayCount);
                toStack.Sort();
                
                audioPlayCount++;
                yield return new WaitForSeconds(moveCardsSpeed);
            }
        }
    }
}