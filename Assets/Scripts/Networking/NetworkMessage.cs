using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Networking
{
    [Serializable]
    public class NetworkMessage
    {
        public string action;
        public string data;

        public NetworkMessage(string action, string data)
        {
            this.action = action;
            this.data = data;
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public static NetworkMessage FromJson(string json)
        {
            return JsonUtility.FromJson<NetworkMessage>(json);
        }
    }

    // Specific message types
    [Serializable]
    public class GameStartMessage
    {
        public string action = "gameStart";
        public string[] playerIds;
        public int totalTurns;
    }

    [Serializable]
    public class TurnStartMessage
    {
        public string action = "turnStart";
        public int turnNumber;
        public string currentPlayerId;
        public int availableCost;
    }

    [Serializable]
    public class EndTurnMessage
    {
        public string action = "endTurn";
        public string playerId;
        public int[] selectedCardIds;
    }

    [Serializable]
    public class RevealCardsMessage
    {
        public string action = "revealCards";
        public string playerId;
        public int[] cardIds;
    }

    [Serializable]
    public class GameEndMessage
    {
        public string action = "gameEnd";
        public string winnerId;
        public Events.SerializableDictionary<string, int> finalScores;
    }
}
