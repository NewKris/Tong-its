using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NordicBibo.Runtime.Common;
using NordicBibo.Runtime.Gameplay.Cards;
using NordicBibo.Runtime.Gameplay.Chips;
using NordicBibo.Runtime.Gameplay.Controllers;
using NordicBibo.Runtime.Gameplay.Ui;
using NordicBibo.Runtime.Utility;
using TMPro;
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

        // Debug
        private int _selectedPlayer;
        
        private int _playerTurn;
        private int _drawAnswers;
        private TongItsPlayer _lastWinner;
        private List<TongItsPlayer> _activePlayers = new List<TongItsPlayer>();
        
        private readonly List<TongItsPlayer> _drawParticipants = new List<TongItsPlayer>();

        public void RestartGame() {
            _lastWinner = null;
            
            foreach (TongItsPlayer tongItsPlayer in players) {
                tongItsPlayer.chips.ResetChips();
                tongItsPlayer.SetPotentialWinnerStatus(false);
            }

            bank.ResetChips();
            gameEndScreen.Hide();
            
            StartCoroutine(SetUpRound());
        }

        public void BeginDraw(TongItsPlayer player) {
            _drawParticipants.Clear();
            _drawParticipants.Add(player);
            _drawAnswers = 1;
            
            foreach (TongItsPlayer tongItsPlayer in _activePlayers) {
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
            
            if (_drawAnswers == _activePlayers.Count) {
                EndByDraw();
            }
        }

        public void DeclineDraw(TongItsPlayer player) {
            _drawAnswers++;

            if (_drawAnswers == _activePlayers.Count) {
                EndByDraw();
            }
        }

        private void OnGUI() {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));

            if (GUILayout.Button("Restart Game")) {
                RestartGame();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Select Player: ");
            int.TryParse(GUILayout.TextField(_selectedPlayer.ToString(), 1), out _selectedPlayer);
            GUILayout.EndHorizontal();
            
            if (GUILayout.Button("Bust Selected Player") && _selectedPlayer >= 0 && _selectedPlayer < players.Count) {
                ChipHolder holder = players[_selectedPlayer].chips; 
                holder.MoveChips(bank.jackpotPile, holder.Count);
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
            
            _playerTurn = (_playerTurn + 1) % _activePlayers.Count;
            _activePlayers[_playerTurn].StartTurn();
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
            ShowPoints(_activePlayers);
            
            TongItsPlayer winner = WinnerFinder.FindStockOutWinner(_activePlayers);
            TryPayoutPlayer(winner);
            
            StartCoroutine(new CoroutineSequenceBuilder()
                .AddCoroutine(ShowWinner(winner))
                .AddCoroutine(SetUpRound())
                .Build()
            );
        }
        
        private void EndByDraw() {
            ShowPoints(_activePlayers);
            
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

            HidePoints();
            
            if (!cardDeck.HasAllCards) {
                yield return dealer.ReturnAllCards();
                yield return actionPadding;
            }
            
            cardDeck.Shuffle();

            _activePlayers = players.Where(player => !player.Busted).ToList();
            bool humanPlayerNotBust = _activePlayers.Any(player => player.isHuman);

            if (_activePlayers.Count <= 1 || !humanPlayerNotBust) {
                gameEndScreen.Show(humanPlayerNotBust);
                yield break;
            }
            
            yield return dealer.DealCards(_activePlayers);

            yield return actionPadding;

            bank.PlaceBets(_activePlayers);
            bank.PlaceJackpot(_activePlayers);
            
            audioPlayer.PlayChipSound();
            
            yield return actionPadding;
            
            _playerTurn = 0;
            _activePlayers[0].StartTurn();
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

        private void ShowPoints(List<TongItsPlayer> activePlayers) {
            foreach (TongItsPlayer tongItsPlayer in activePlayers) {
                tongItsPlayer.RevealPoints();
            }
        }
        
        private void HidePoints() {
            foreach (TongItsPlayer tongItsPlayer in players) {
                tongItsPlayer.HidePoints();
            }
        }

        private IEnumerator ShowWinner(TongItsPlayer winner) {
            yield return new WaitForSeconds(3);
        }
    }
}