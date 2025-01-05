using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NordicBibo.Runtime.Common;
using NordicBibo.Runtime.Gameplay.Cards;
using NordicBibo.Runtime.Gameplay.Chips.Simple;
using NordicBibo.Runtime.Gameplay.Controllers;
using NordicBibo.Runtime.Gameplay.Ui;
using NordicBibo.Runtime.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace NordicBibo.Runtime.Gameplay {
    public class TongItsEngine : MonoBehaviour {
        public GlobalAudioPlayer audioPlayer;
        public CardDeck cardDeck;
        public Dealer dealer;
        public Bank bank;
        public List<TongItsPlayer> players;
        
        [Header("UI")] 
        public GameEndScreen gameEndScreen;

        private int _playerTurn;
        private int _drawAnswers;
        private TongItsPlayer _lastWinner;
        private readonly List<TongItsPlayer> _drawParticipants = new List<TongItsPlayer>();

        public void RestartGame() {
            _lastWinner = null;
            
            foreach (TongItsPlayer tongItsPlayer in players) {
                tongItsPlayer.chips.ResetChips();
            }

            bank.ResetChips();
            gameEndScreen.Hide();
            
            StartCoroutine(SetUpRound());
        }

        public void BeginDraw(TongItsPlayer player) {
            _drawParticipants.Clear();
            _drawParticipants.Add(player);
            _drawAnswers = 1;
            
            foreach (TongItsPlayer tongItsPlayer in players) {
                tongItsPlayer.EndTurn();

                if (tongItsPlayer != player) {
                    tongItsPlayer.Challenge();
                }
            }
        }

        public void AcceptDraw(TongItsPlayer player) {
            player.DrawChallengeTime = Time.time;
            
            _drawParticipants.Add(player);
            _drawAnswers++;
            
            if (_drawAnswers == players.Count) {
                EndByDraw();
            }
        }

        public void DeclineDraw(TongItsPlayer player) {
            _drawAnswers++;

            if (_drawAnswers == players.Count) {
                EndByDraw();
            }
        }

        private void OnGUI() {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));

            if (GUILayout.Button("Restart Game")) {
                RestartGame();
            }
            
            GUILayout.EndArea();
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
            
            _playerTurn = (_playerTurn + 1) % players.Count;
            players[_playerTurn].StartTurn();
        }
        
        private void EndByTongIts(TongItsPlayer playerEmptiedHand) {
            TryPayoutPlayer(playerEmptiedHand);
            StartCoroutine(new CoroutineSequenceBuilder()
                .AddCoroutine(ShowWinner(playerEmptiedHand))
                .AddCoroutine(SetUpRound())
                .Build()
            );
        }

        private void EndByStockOut() {
            TongItsPlayer winner = WinnerFinder.FindStockOutWinner(players);
            TryPayoutPlayer(winner);
            
            StartCoroutine(new CoroutineSequenceBuilder()
                .AddCoroutine(ShowWinner(winner))
                .AddCoroutine(SetUpRound())
                .Build()
            );
        }
        
        private void EndByDraw() {
            TongItsPlayer winner = WinnerFinder.FindDrawWinner(_drawParticipants);
            TryPayoutPlayer(winner);
            
            StartCoroutine(new CoroutineSequenceBuilder()
                .AddCoroutine(ShowWinner(winner))
                .AddCoroutine(SetUpRound())
                .Build()
            );
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
            
            cardDeck.Shuffle();
            
            yield return dealer.DealCards(players);

            yield return actionPadding;

            bank.PlaceBets(players);
            bank.PlaceJackpot(players);
            
            audioPlayer.PlayChipSound();
            
            yield return actionPadding;
            
            _playerTurn = 0;
            players[0].StartTurn();
        }
        
        private bool HasValidGameParameters() {
            return cardDeck.TotalCount - (players.Count * dealer.cardsPerPlayer) > 0;
        }

        private void TryPayoutPlayer(TongItsPlayer player) {
            // Player must win twice in a row to earn payout
            
            _lastWinner?.SetPotentialWinnerStatus(false);
            
            if (_lastWinner != player) {
                _lastWinner = player;
                _lastWinner.SetPotentialWinnerStatus(true);
                return;
            };
            
            bank.PayoutPlayer(_lastWinner);
            audioPlayer.PlayChipSound();
            _lastWinner = null;
        }

        private IEnumerator ShowWinner(TongItsPlayer winner) {
            yield return new WaitForSeconds(3);
        }
    }
}