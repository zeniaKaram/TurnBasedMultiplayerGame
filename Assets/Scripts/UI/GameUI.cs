using System.Collections.Generic;
using System.Linq;
using CardGame.Data;
using CardGame.Events;
using CardGame.Game;
using CardGame.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.UI
{
    public class GameUI : MonoBehaviour
    {
        [Header("UI References")]
        public Transform handContainer;
        public GameObject cardPrefab;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI opponentScoreText;
        public TextMeshProUGUI turnText;
        public TextMeshProUGUI costText;
        public TextMeshProUGUI timerText;
        public Button endTurnButton;
        public TextMeshProUGUI gameStatusText;

        private List<CardUI> _handCards = new List<CardUI>();
        private List<int> _selectedCardIds = new List<int>();
        private int _currentCost = 0;
        private int _availableCost = 0;
        private string _localPlayerId;

        private void Start()
        {
            endTurnButton.onClick.AddListener(OnEndTurnClicked);
            GameEvents.OnGameStart += HandleGameStart;
            GameEvents.OnTurnStart += HandleTurnStart;
            GameEvents.OnHandChanged += HandleHandChanged;
            GameEvents.OnScoreChanged += HandleScoreChanged;
            GameEvents.OnCardDrawn += HandleCardDrawn;
            GameEvents.OnGameEnd += HandleGameEnd;
        }

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.IsGameActive)
            {
                UpdateTimer();
            }
        }

        private void HandleGameStart(GameStartEventArgs args)
        {
            // Find local player ID
            if (LocalPlayerManager.Instance != null && LocalPlayerManager.Instance.LocalPlayer != null)
            {
                _localPlayerId = LocalPlayerManager.Instance.LocalPlayer.PlayerId;
            }
            else
            {
                // Fallback: use first player as local for testing
                _localPlayerId = args.playerIds[0];
            }

            gameStatusText.text = "Game Started!";
        }

        private void HandleTurnStart(TurnStartEventArgs args)
        {
            _availableCost = args.availableCost;
            turnText.text = $"Turn {args.turnNumber}/6";
            costText.text = $"Cost: {_currentCost}/{_availableCost}";

            // Both players can select cards, but timer tracks current player
            bool isMyTurn = args.currentPlayerId == _localPlayerId;
            endTurnButton.interactable = true; // Both players can end turn
            gameStatusText.text = "Select your cards!";

            _selectedCardIds.Clear();
            _currentCost = 0;
            UpdateHand();
        }

        private void HandleHandChanged(HandChangedEventArgs args)
        {
            if (args.playerId == _localPlayerId)
            {
                UpdateHand();
            }
        }

        private void HandleScoreChanged(ScoreChangedEventArgs args)
        {
            if (args.playerId == _localPlayerId)
            {
                scoreText.text = $"Your Score: {args.newScore}";
            }
            else
            {
                opponentScoreText.text = $"Opponent Score: {args.newScore}";
            }
        }

        private void HandleCardDrawn(CardDrawnEventArgs args)
        {
            if (args.playerId == _localPlayerId)
            {
                UpdateHand();
            }
        }

        private void HandleGameEnd(GameEndEventArgs args)
        {
            bool isWinner = args.winnerId == _localPlayerId;
            gameStatusText.text = isWinner ? "You Win!" : "You Lose!";
            endTurnButton.interactable = false;
        }

        private void UpdateHand()
        {
            if (GameManager.Instance == null || !GameManager.Instance.Players.ContainsKey(_localPlayerId))
                return;

            Player player = GameManager.Instance.Players[_localPlayerId];

            // Clear existing cards
            foreach (var cardUI in _handCards)
            {
                if (cardUI != null)
                {
                    Destroy(cardUI.gameObject);
                }
            }
            _handCards.Clear();
            _selectedCardIds.Clear();
            _currentCost = 0;

            // Create card UIs
            foreach (var card in player.Hand)
            {
                GameObject cardObj = Instantiate(cardPrefab, handContainer);
                CardUI cardUI = cardObj.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    cardUI.Initialize(card, this);
                    _handCards.Add(cardUI);
                }
            }

            UpdateCostDisplay();
        }

        public void OnCardSelected(CardData card, bool selected)
        {
            if (selected)
            {
                if (!_selectedCardIds.Contains(card.id))
                {
                    int newCost = _currentCost + card.cost;
                    if (newCost <= _availableCost)
                    {
                        _selectedCardIds.Add(card.id);
                        _currentCost = newCost;
                    }
                }
            }
            else
            {
                if (_selectedCardIds.Contains(card.id))
                {
                    _selectedCardIds.Remove(card.id);
                    _currentCost -= card.cost;
                }
            }

            UpdateCostDisplay();
        }

        private void UpdateCostDisplay()
        {
            costText.text = $"Cost: {_currentCost}/{_availableCost}";
        }

        private void UpdateTimer()
        {
            if (GameManager.Instance != null)
            {
                float timeLeft = GameManager.Instance.TurnTimer;
                timerText.text = $"Time: {Mathf.Ceil(timeLeft)}s";
            }
        }

        private void OnEndTurnClicked()
        {
            if (GameManager.Instance == null || GameManager.Instance.CurrentPlayerId != _localPlayerId)
                return;

            if (LocalPlayerManager.Instance != null && LocalPlayerManager.Instance.LocalPlayer != null)
            {
                LocalPlayerManager.Instance.LocalPlayer.CmdEndTurn(_selectedCardIds.ToArray());
            }

            GameManager.Instance.EndTurn(_localPlayerId, _selectedCardIds);
        }

        private void OnDestroy()
        {
            GameEvents.OnGameStart -= HandleGameStart;
            GameEvents.OnTurnStart -= HandleTurnStart;
            GameEvents.OnHandChanged -= HandleHandChanged;
            GameEvents.OnScoreChanged -= HandleScoreChanged;
            GameEvents.OnCardDrawn -= HandleCardDrawn;
            GameEvents.OnGameEnd -= HandleGameEnd;
        }
    }
}

