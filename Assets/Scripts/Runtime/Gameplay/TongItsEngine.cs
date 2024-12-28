using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NordicBibo.Runtime.Gameplay.Cards;
using NordicBibo.Runtime.Gameplay.Chips.Simple;
using NordicBibo.Runtime.Gameplay.Controllers;
using NordicBibo.Runtime.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace NordicBibo.Runtime.Gameplay {
    public class TongItsEngine : MonoBehaviour {
        public CardDeck cardDeck;
        public Dealer dealer;
        public TongItsPlayer[] players;
        
        [Header("Betting")]
        public ChipHolder bettingPile;
        public ChipHolder jackpotPile;
        public int bettingCount;
        public int jackpotCount;

        private int _playerTurn;
        private TongItsPlayer _lastWinner;
        
        public void StartNewRound() {
            if (!cardDeck.HasSpawnedCards) {
                cardDeck.SpawnCards();
            }
            
            StartCoroutine(SetUpRound());
        }
        
        public void EndByDraw() {
            TongItsPlayer winner = FindDrawWinner();
            TryPayoutPlayer(winner);
            StartNewRound();
        }

        private void Awake() {
            TongItsPlayer.OnDiscard += EndPlayerTurn;
            TongItsPlayer.OnHandEmptied += EndByTongIts;
        }

        private void OnDestroy() {
            TongItsPlayer.OnDiscard -= EndPlayerTurn;
            TongItsPlayer.OnHandEmptied -= EndByTongIts;
        }

        private void EndPlayerTurn(TongItsPlayer playerTurnEnded) {
            playerTurnEnded.EndTurn();

            if (cardDeck.Count == 0) {
                EndByStockOut();
                return;
            }
            
            _playerTurn = (_playerTurn + 1) % players.Length;
            players[_playerTurn].StartTurn();
        }
        
        private void EndByTongIts(TongItsPlayer playerEmptiedHand) {
            TryPayoutPlayer(playerEmptiedHand);
            StartNewRound();
        }

        private void EndByStockOut() {
            TongItsPlayer winner = FindStockOutWinner();
            TryPayoutPlayer(winner);
            StartNewRound();
        }

        private IEnumerator SetUpRound() {
            if (!HasValidGameParameters()) {
                Debug.LogError("Invalid game parameters!");
                yield break;
            }

            WaitForSeconds actionPadding = new WaitForSeconds(0.25f);

            if (!cardDeck.HasAllCards) {
                yield return dealer.ReturnAllCards();
                yield return actionPadding;
            }

            PlaceBets();
            PlaceJackpot();
            
            cardDeck.Shuffle();
            
            yield return dealer.DealCards(players);

            yield return actionPadding;

            _playerTurn = 0;
            players[0].StartTurn();
        }

        private void PlaceBets() {
            foreach (TongItsPlayer tongItsPlayer in players) {
                ChipHolder.MoveChips(tongItsPlayer.chips, bettingPile, bettingCount);
            }
        }

        private void PlaceJackpot() {
            foreach (TongItsPlayer tongItsPlayer in players) {
                ChipHolder.MoveChips(tongItsPlayer.chips, jackpotPile, jackpotCount);
            }
        }
        
        private bool HasValidGameParameters() {
            return cardDeck.TotalCount - (players.Length * dealer.cardsPerPlayer) > 0;
        }

        private void TryPayoutPlayer(TongItsPlayer player) {
            // Player must win twice in a row to earn payout
            
            _lastWinner?.SetNextWinner(false);
            
            if (_lastWinner != player) {
                _lastWinner = player;
                _lastWinner.SetNextWinner(true);
                return;
            };
            
            ChipHolder.MoveChips(bettingPile, player.chips, bettingPile.Chips);
            _lastWinner = null;
        }

        private TongItsPlayer FindStockOutWinner() {
            TongItsPlayer[] winnerCandidates = FindPlayerWithLowestPoints(players);

            if (winnerCandidates.Length == 1) {
                return winnerCandidates[0];
            }
            
            return FindPlayerWhoLastPickedStockCard(winnerCandidates);
        }
        
        private TongItsPlayer FindDrawWinner() {
            TongItsPlayer[] winnerCandidates = FindPlayerWithLowestPoints(players);
            
            if (winnerCandidates.Length == 1) {
                return winnerCandidates[0];
            }
            
            // TODO: Make Draw caller lose on ties. Make other players win ties based on who challenged first
            
            return FindPlayerWhoLastPickedStockCard(winnerCandidates);
        }

        private static TongItsPlayer[] FindPlayerWithLowestPoints(TongItsPlayer[] candidates) {
            int lowest = candidates.Select(tongItsPlayer => tongItsPlayer.Tally).Min();

            return candidates.Where(player => player.Tally == lowest).ToArray();
        }

        private static TongItsPlayer FindPlayerWhoLastPickedStockCard(TongItsPlayer[] candidates) {
            float latest = 0;
            TongItsPlayer candidate = candidates[0];
            
            foreach (TongItsPlayer player in candidates) {
                if (player.StockDrawTime < latest) continue;
                
                latest = player.StockDrawTime;
                candidate = player;
            }

            return candidate;
        }
    }
}