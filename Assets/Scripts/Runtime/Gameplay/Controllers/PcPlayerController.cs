using System;
using System.Collections;
using System.Collections.Generic;
using NordicBibo.Runtime.Gameplay.Cards;
using NordicBibo.Runtime.Gameplay.Utility;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Controllers {
    public class PcPlayerController : TongItsPlayer {
        [Header("PC Settings")]
        public float actionDelay;
        
        public override void StartTurn() {
            StartCoroutine(PlayTurn());
        }

        public override void EndTurn() {
        }

        private IEnumerator PlayTurn() {
            WaitForSeconds wait = new WaitForSeconds(actionDelay);

            yield return wait;
            
            if (stockStack.Count > 0) {
                base.Draw();
            }

            yield return wait;
            
            // Create melds
            // 1. Check each individual card in hand if they can be added to an exposed meld
            //     a. Reset loop each time a play is made
            // 2. Check hand if any possible meld can be made
            
            yield return wait;
            
            // Discard highest point card not in a Possible Meld
            
            if (playerHand.Count > 0) {
                int leastImportant = MeldEvaluator.FindLeastImportantCard(playerHand.GetCardIndices());
                base.Discard(playerHand.IndexToCard(leastImportant));
            }
        }
    }
}