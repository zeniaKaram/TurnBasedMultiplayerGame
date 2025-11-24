using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Events
{
    public static class GameEvents
    {
        // Game Flow Events
        public static event Action<GameStartEventArgs> OnGameStart;
        public static event Action<TurnStartEventArgs> OnTurnStart;
        public static event Action<PlayerEndedTurnEventArgs> OnPlayerEndedTurn;
        public static event Action<RevealCardsEventArgs> OnRevealCards;
        public static event Action<GameEndEventArgs> OnGameEnd;

        // Card Events
        public static event Action<CardPlayedEventArgs> OnCardPlayed;
        public static event Action<CardDrawnEventArgs> OnCardDrawn;

        // Player Events
        public static event Action<ScoreChangedEventArgs> OnScoreChanged;
        public static event Action<HandChangedEventArgs> OnHandChanged;

        public static void InvokeGameStart(GameStartEventArgs args)
        {
            OnGameStart?.Invoke(args);
        }

        public static void InvokeTurnStart(TurnStartEventArgs args)
        {
            OnTurnStart?.Invoke(args);
        }

        public static void InvokePlayerEndedTurn(PlayerEndedTurnEventArgs args)
        {
            OnPlayerEndedTurn?.Invoke(args);
        }

        public static void InvokeRevealCards(RevealCardsEventArgs args)
        {
            OnRevealCards?.Invoke(args);
        }

        public static void InvokeGameEnd(GameEndEventArgs args)
        {
            OnGameEnd?.Invoke(args);
        }

        public static void InvokeCardPlayed(CardPlayedEventArgs args)
        {
            OnCardPlayed?.Invoke(args);
        }

        public static void InvokeCardDrawn(CardDrawnEventArgs args)
        {
            OnCardDrawn?.Invoke(args);
        }

        public static void InvokeScoreChanged(ScoreChangedEventArgs args)
        {
            OnScoreChanged?.Invoke(args);
        }

        public static void InvokeHandChanged(HandChangedEventArgs args)
        {
            OnHandChanged?.Invoke(args);
        }
    }

    // Event Args Classes
    [Serializable]
    public class GameStartEventArgs
    {
        public string[] playerIds;
        public int totalTurns;
    }

    [Serializable]
    public class TurnStartEventArgs
    {
        public int turnNumber;
        public string currentPlayerId;
        public int availableCost;
    }

    [Serializable]
    public class PlayerEndedTurnEventArgs
    {
        public string playerId;
        public int[] selectedCardIds;
    }

    [Serializable]
    public class RevealCardsEventArgs
    {
        public string playerId;
        public int[] cardIds;
    }

    [Serializable]
    public class GameEndEventArgs
    {
        public string winnerId;
        public SerializableDictionary<string, int> finalScores;
    }

    [Serializable]
    public class SerializableDictionary<TKey, TValue>
    {
        public TKey[] keys;
        public TValue[] values;

        public SerializableDictionary(Dictionary<TKey, TValue> dict)
        {
            keys = new TKey[dict.Count];
            values = new TValue[dict.Count];
            int index = 0;
            foreach (var kvp in dict)
            {
                keys[index] = kvp.Key;
                values[index] = kvp.Value;
                index++;
            }
        }

        public Dictionary<TKey, TValue> ToDictionary()
        {
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            for (int i = 0; i < keys.Length; i++)
            {
                dict[keys[i]] = values[i];
            }
            return dict;
        }
    }

    [Serializable]
    public class CardPlayedEventArgs
    {
        public string playerId;
        public int cardId;
    }

    [Serializable]
    public class CardDrawnEventArgs
    {
        public string playerId;
        public int cardId;
    }

    [Serializable]
    public class ScoreChangedEventArgs
    {
        public string playerId;
        public int newScore;
    }

    [Serializable]
    public class HandChangedEventArgs
    {
        public string playerId;
        public int[] handCardIds;
    }
}

