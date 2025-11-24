using System.Collections.Generic;
using CardGame.Events;
using CardGame.Game;
using Mirror;
using UnityEngine;

namespace CardGame.Networking
{
    public class NetworkGameManager : NetworkBehaviour
    {
        public static NetworkGameManager Instance { get; private set; }

        private NetworkManager _networkManager;
        private Dictionary<string, NetworkConnectionToClient> _playerConnections = new Dictionary<string, NetworkConnectionToClient>();
        private int _connectedPlayers = 0;
        private const int MAX_PLAYERS = 2;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _networkManager = FindFirstObjectByType<NetworkManager>();
            GameEvents.OnGameStart += HandleGameStart;
            GameEvents.OnTurnStart += HandleTurnStart;
            GameEvents.OnPlayerEndedTurn += HandlePlayerEndedTurn;
            GameEvents.OnRevealCards += HandleRevealCards;
            GameEvents.OnGameEnd += HandleGameEnd;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            Debug.Log("Server started");
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            Debug.Log("Client connected");
        }

        [Server]
        public void RegisterPlayer(string playerId, NetworkConnectionToClient conn)
        {
            if (!_playerConnections.ContainsKey(playerId))
            {
                _playerConnections[playerId] = conn;
                _connectedPlayers++;

                if (_connectedPlayers >= MAX_PLAYERS)
                {
                    StartGame();
                }
            }
        }

        [Server]
        private void StartGame()
        {
            string[] playerIds = new string[_playerConnections.Keys.Count];
            _playerConnections.Keys.CopyTo(playerIds, 0);

            // Send game start message to all clients
            GameStartMessage msg = new GameStartMessage
            {
                playerIds = playerIds,
                totalTurns = 6
            };

            string json = JsonUtility.ToJson(msg);
            RpcSendGameStart(json);
        }

        [ClientRpc]
        private void RpcSendGameStart(string json)
        {
            GameStartMessage msg = JsonUtility.FromJson<GameStartMessage>(json);
            GameEvents.InvokeGameStart(new GameStartEventArgs
            {
                playerIds = msg.playerIds,
                totalTurns = msg.totalTurns
            });

            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartGame(msg.playerIds);
            }
        }

        [Command(requiresAuthority = false)]
        public void CmdEndTurn(string json)
        {
            EndTurnMessage msg = JsonUtility.FromJson<EndTurnMessage>(json);
            RpcEndTurn(json);
        }

        [ClientRpc]
        private void RpcEndTurn(string json)
        {
            EndTurnMessage msg = JsonUtility.FromJson<EndTurnMessage>(json);
            GameEvents.InvokePlayerEndedTurn(new PlayerEndedTurnEventArgs
            {
                playerId = msg.playerId,
                selectedCardIds = msg.selectedCardIds
            });
        }

        private void HandleGameStart(GameStartEventArgs args)
        {
            if (isServer)
            {
                GameStartMessage msg = new GameStartMessage
                {
                    playerIds = args.playerIds,
                    totalTurns = args.totalTurns
                };
                string json = JsonUtility.ToJson(msg);
                RpcSendGameStart(json);
            }
        }

        private void HandleTurnStart(TurnStartEventArgs args)
        {
            if (isServer)
            {
                TurnStartMessage msg = new TurnStartMessage
                {
                    turnNumber = args.turnNumber,
                    currentPlayerId = args.currentPlayerId,
                    availableCost = args.availableCost
                };
                string json = JsonUtility.ToJson(msg);
                RpcSendTurnStart(json);
            }
        }

        [ClientRpc]
        private void RpcSendTurnStart(string json)
        {
            TurnStartMessage msg = JsonUtility.FromJson<TurnStartMessage>(json);
            GameEvents.InvokeTurnStart(new TurnStartEventArgs
            {
                turnNumber = msg.turnNumber,
                currentPlayerId = msg.currentPlayerId,
                availableCost = msg.availableCost
            });
        }

        private void HandlePlayerEndedTurn(PlayerEndedTurnEventArgs args)
        {
            // Network message already sent via CmdEndTurn
        }

        private void HandleRevealCards(RevealCardsEventArgs args)
        {
            if (isServer)
            {
                RevealCardsMessage msg = new RevealCardsMessage
                {
                    playerId = args.playerId,
                    cardIds = args.cardIds
                };
                string json = JsonUtility.ToJson(msg);
                RpcSendRevealCards(json);
            }
        }

        [ClientRpc]
        private void RpcSendRevealCards(string json)
        {
            RevealCardsMessage msg = JsonUtility.FromJson<RevealCardsMessage>(json);
            GameEvents.InvokeRevealCards(new RevealCardsEventArgs
            {
                playerId = msg.playerId,
                cardIds = msg.cardIds
            });
        }

        private void HandleGameEnd(GameEndEventArgs args)
        {
            if (isServer)
            {
            GameEndMessage msg = new GameEndMessage
            {
                winnerId = args.winnerId,
                finalScores = args.finalScores
            };
                string json = JsonUtility.ToJson(msg);
                RpcSendGameEnd(json);
            }
        }

        [ClientRpc]
        private void RpcSendGameEnd(string json)
        {
            GameEndMessage msg = JsonUtility.FromJson<GameEndMessage>(json);
            GameEvents.InvokeGameEnd(new GameEndEventArgs
            {
                winnerId = msg.winnerId,
                finalScores = msg.finalScores
            });
        }

        private void OnDestroy()
        {
            GameEvents.OnGameStart -= HandleGameStart;
            GameEvents.OnTurnStart -= HandleTurnStart;
            GameEvents.OnPlayerEndedTurn -= HandlePlayerEndedTurn;
            GameEvents.OnRevealCards -= HandleRevealCards;
            GameEvents.OnGameEnd -= HandleGameEnd;
        }
    }
}

