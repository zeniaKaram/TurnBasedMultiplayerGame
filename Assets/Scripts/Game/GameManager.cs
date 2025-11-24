using System.Collections.Generic;
using System.Linq;
using CardGame.Data;
using CardGame.Events;
using CardGame.Networking;
using UnityEngine;

namespace CardGame.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public int CurrentTurn { get; private set; }
        public int TotalTurns { get; private set; } = 6;
        public string CurrentPlayerId { get; private set; }
        public Dictionary<string, Player> Players { get; private set; }
        public bool IsGameActive { get; private set; }
        public float TurnTimer { get; private set; }
        public float TurnDuration { get; private set; } = 30f;

        private NetworkGameManager _networkManager;
        private bool _bothPlayersReady = false;
        private Dictionary<string, List<int>> _pendingCardSelections = new Dictionary<string, List<int>>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Players = new Dictionary<string, Player>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _networkManager = FindObjectOfType<NetworkGameManager>();
            GameEvents.OnPlayerEndedTurn += HandlePlayerEndedTurn;
        }

        private void Update()
        {
            if (IsGameActive && CurrentPlayerId != null)
            {
                TurnTimer -= Time.deltaTime;
                if (TurnTimer <= 0)
                {
                    AutoEndTurn();
                }
            }
        }

        public void StartGame(string[] playerIds)
        {
            IsGameActive = true;
            CurrentTurn = 0;
            Players.Clear();

            foreach (string playerId in playerIds)
            {
                Player player = new Player(playerId);
                List<CardData> deck = CardDatabase.Instance.GetRandomDeck(12);
                player.InitializeDeck(deck);
                player.DrawCards(3);
                Players[playerId] = player;
                
                // Notify hand change
                GameEvents.InvokeHandChanged(new HandChangedEventArgs
                {
                    playerId = playerId,
                    handCardIds = player.Hand.ConvertAll(c => c.id).ToArray()
                });
            }

            GameEvents.InvokeGameStart(new GameStartEventArgs
            {
                playerIds = playerIds,
                totalTurns = TotalTurns
            });

            StartNextTurn();
        }

        private void StartNextTurn()
        {
            CurrentTurn++;
            if (CurrentTurn > TotalTurns)
            {
                EndGame();
                return;
            }

            // Draw card at start of turn
            foreach (var player in Players.Values)
            {
                player.DrawCard();
                GameEvents.InvokeHandChanged(new HandChangedEventArgs
                {
                    playerId = player.PlayerId,
                    handCardIds = player.Hand.ConvertAll(c => c.id).ToArray()
                });
            }

            // Both players can select cards simultaneously each turn
            // CurrentPlayerId is used for timer tracking (alternates)
            string[] playerIds = Players.Keys.ToArray();
            CurrentPlayerId = playerIds[(CurrentTurn - 1) % playerIds.Length];

            TurnTimer = TurnDuration;
            _bothPlayersReady = false;
            _pendingCardSelections.Clear();

            GameEvents.InvokeTurnStart(new TurnStartEventArgs
            {
                turnNumber = CurrentTurn,
                currentPlayerId = CurrentPlayerId,
                availableCost = CurrentTurn
            });
        }

        public void EndTurn(string playerId, List<int> selectedCardIds)
        {
            if (!IsGameActive)
                return;

            if (!Players.ContainsKey(playerId))
                return;

            Player player = Players[playerId];
            
            // Validate cost if cards are selected
            if (selectedCardIds.Count > 0 && !player.CanPlayCards(selectedCardIds))
            {
                Debug.LogWarning($"Player {playerId} cannot play selected cards (cost too high)");
                return;
            }

            // Play cards if any selected
            if (selectedCardIds.Count > 0)
            {
                player.PlayCards(selectedCardIds);
            }
            
            _pendingCardSelections[playerId] = selectedCardIds;

            GameEvents.InvokePlayerEndedTurn(new PlayerEndedTurnEventArgs
            {
                playerId = playerId,
                selectedCardIds = selectedCardIds.ToArray()
            });

            // Check if both players have ended their turn
            CheckForReveal();
        }

        private void HandlePlayerEndedTurn(PlayerEndedTurnEventArgs args)
        {
            // This is called when network message is received
            if (!Players.ContainsKey(args.playerId))
                return;

            Player player = Players[args.playerId];
            
            // Play cards if any selected
            if (args.selectedCardIds.Length > 0)
            {
                List<int> cardIds = new List<int>(args.selectedCardIds);
                if (player.CanPlayCards(cardIds))
                {
                    player.PlayCards(cardIds);
                }
            }
            
            _pendingCardSelections[args.playerId] = new List<int>(args.selectedCardIds);
            CheckForReveal();
        }

        private void CheckForReveal()
        {
            if (_pendingCardSelections.Count >= Players.Count)
            {
                // Both players have selected cards, reveal them
                RevealCards();
            }
        }

        private void RevealCards()
        {
            foreach (var kvp in _pendingCardSelections)
            {
                string playerId = kvp.Key;
                List<int> cardIds = kvp.Value;

                GameEvents.InvokeRevealCards(new RevealCardsEventArgs
                {
                    playerId = playerId,
                    cardIds = cardIds.ToArray()
                });
            }

            // Resolve cards
            ResolveCards();

            // Clear cards in play and start next turn
            foreach (var player in Players.Values)
            {
                player.ClearCardsInPlay();
            }

            StartNextTurn();
        }

        private void ResolveCards()
        {
            // First, handle DestroyOpponentCardInPlay abilities
            foreach (var kvp in Players)
            {
                string playerId = kvp.Key;
                Player player = kvp.Value;
                Player opponent = Players.Values.FirstOrDefault(p => p.PlayerId != playerId);

                foreach (var card in player.CardsInPlay.ToList())
                {
                    if (card.ability != null && card.ability.type == "DestroyOpponentCardInPlay")
                    {
                        for (int i = 0; i < card.ability.value && opponent.CardsInPlay.Count > 0; i++)
                        {
                            var cardToDestroy = opponent.CardsInPlay[0];
                            opponent.RemoveCardFromPlay(cardToDestroy.id);
                        }
                    }
                }
            }

            // Then resolve other abilities and calculate power
            foreach (var kvp in Players)
            {
                string playerId = kvp.Key;
                Player player = kvp.Value;
                Player opponent = Players.Values.FirstOrDefault(p => p.PlayerId != playerId);

                foreach (var card in player.CardsInPlay)
                {
                    int power = card.power;

                    // Apply DoublePower
                    if (card.ability != null && card.ability.type == "DoublePower")
                    {
                        power *= card.ability.value;
                    }

                    // Add power to score
                    player.AddScore(power);
                    GameEvents.InvokeScoreChanged(new ScoreChangedEventArgs
                    {
                        playerId = playerId,
                        newScore = player.Score
                    });

                    // Resolve other abilities
                    if (card.ability != null)
                    {
                        ResolveAbility(card, player, opponent);
                    }
                }
            }
        }

        private void ResolveAbility(CardData card, Player owner, Player opponent)
        {
            switch (card.ability.type)
            {
                case "GainPoints":
                    owner.AddScore(card.ability.value);
                    GameEvents.InvokeScoreChanged(new ScoreChangedEventArgs
                    {
                        playerId = owner.PlayerId,
                        newScore = owner.Score
                    });
                    break;

                case "StealPoints":
                    int pointsToSteal = Mathf.Min(card.ability.value, opponent.Score);
                    opponent.RemoveScore(pointsToSteal);
                    owner.AddScore(pointsToSteal);
                    GameEvents.InvokeScoreChanged(new ScoreChangedEventArgs
                    {
                        playerId = owner.PlayerId,
                        newScore = owner.Score
                    });
                    GameEvents.InvokeScoreChanged(new ScoreChangedEventArgs
                    {
                        playerId = opponent.PlayerId,
                        newScore = opponent.Score
                    });
                    break;

                case "DrawExtraCard":
                    owner.DrawCards(card.ability.value);
                    GameEvents.InvokeHandChanged(new HandChangedEventArgs
                    {
                        playerId = owner.PlayerId,
                        handCardIds = owner.Hand.ConvertAll(c => c.id).ToArray()
                    });
                    break;

                case "DiscardOpponentRandomCard":
                    for (int i = 0; i < card.ability.value && opponent.Hand.Count > 0; i++)
                    {
                        int randomIndex = Random.Range(0, opponent.Hand.Count);
                        opponent.RemoveCardFromHand(opponent.Hand[randomIndex].id);
                    }
                    GameEvents.InvokeHandChanged(new HandChangedEventArgs
                    {
                        playerId = opponent.PlayerId,
                        handCardIds = opponent.Hand.ConvertAll(c => c.id).ToArray()
                    });
                    break;

                case "DoublePower":
                case "DestroyOpponentCardInPlay":
                    // Already handled
                    break;
            }
        }

        private void AutoEndTurn()
        {
            if (CurrentPlayerId != null)
            {
                EndTurn(CurrentPlayerId, new List<int>());
            }
        }

        private void EndGame()
        {
            IsGameActive = false;
            string winnerId = null;
            int highestScore = int.MinValue;

            Dictionary<string, int> finalScores = new Dictionary<string, int>();
            foreach (var kvp in Players)
            {
                finalScores[kvp.Key] = kvp.Value.Score;
                if (kvp.Value.Score > highestScore)
                {
                    highestScore = kvp.Value.Score;
                    winnerId = kvp.Key;
                }
            }

            GameEvents.InvokeGameEnd(new GameEndEventArgs
            {
                winnerId = winnerId,
                finalScores = new Events.SerializableDictionary<string, int>(finalScores)
            });
        }

        private void OnDestroy()
        {
            GameEvents.OnPlayerEndedTurn -= HandlePlayerEndedTurn;
        }
    }
}

