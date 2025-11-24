using System.Collections.Generic;
using System.Linq;
using CardGame.Data;
using UnityEngine;

namespace CardGame.Game
{
    public class Player
    {
        public string PlayerId { get; private set; }
        public int Score { get; private set; }
        public List<CardData> Hand { get; private set; }
        public List<CardData> Deck { get; private set; }
        public List<CardData> CardsInPlay { get; private set; }

        public Player(string playerId)
        {
            PlayerId = playerId;
            Score = 0;
            Hand = new List<CardData>();
            Deck = new List<CardData>();
            CardsInPlay = new List<CardData>();
        }

        public void InitializeDeck(List<CardData> deck)
        {
            Deck = new List<CardData>(deck);
            ShuffleDeck();
        }

        public void DrawCard()
        {
            if (Deck.Count > 0)
            {
                CardData card = Deck[0];
                Deck.RemoveAt(0);
                Hand.Add(card);
            }
        }

        public void DrawCards(int count)
        {
            for (int i = 0; i < count; i++)
            {
                DrawCard();
            }
        }

        public bool CanPlayCards(List<int> cardIds)
        {
            int totalCost = 0;
            foreach (int cardId in cardIds)
            {
                CardData card = Hand.FirstOrDefault(c => c.id == cardId);
                if (card == null) return false;
                totalCost += card.cost;
            }
            return totalCost <= GetAvailableCostForTurn(GameManager.Instance.CurrentTurn);
        }

        public void PlayCards(List<int> cardIds)
        {
            foreach (int cardId in cardIds)
            {
                CardData card = Hand.FirstOrDefault(c => c.id == cardId);
                if (card != null)
                {
                    Hand.Remove(card);
                    CardsInPlay.Add(card);
                }
            }
        }

        public void AddScore(int points)
        {
            Score += points;
        }

        public void RemoveScore(int points)
        {
            Score = Mathf.Max(0, Score - points);
        }

        public void RemoveCardFromHand(int cardId)
        {
            CardData card = Hand.FirstOrDefault(c => c.id == cardId);
            if (card != null)
            {
                Hand.Remove(card);
            }
        }

        public void RemoveCardFromPlay(int cardId)
        {
            CardData card = CardsInPlay.FirstOrDefault(c => c.id == cardId);
            if (card != null)
            {
                CardsInPlay.Remove(card);
            }
        }

        public void ClearCardsInPlay()
        {
            CardsInPlay.Clear();
        }

        private void ShuffleDeck()
        {
            for (int i = Deck.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                CardData temp = Deck[i];
                Deck[i] = Deck[randomIndex];
                Deck[randomIndex] = temp;
            }
        }

        private int GetAvailableCostForTurn(int turnNumber)
        {
            return Mathf.Clamp(turnNumber, 1, 6);
        }
    }
}

