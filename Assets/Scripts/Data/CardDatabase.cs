using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CardGame.Data
{
    public class CardDatabase : MonoBehaviour
    {
        private static CardDatabase _instance;
        public static CardDatabase Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<CardDatabase>();
                }
                return _instance;
            }
        }

        private Dictionary<int, CardData> _cards = new Dictionary<int, CardData>();

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                LoadCards();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void LoadCards()
        {
            string jsonPath = Path.Combine(Application.streamingAssetsPath, "cards.json");
            
            if (!File.Exists(jsonPath))
            {
                Debug.LogError($"Cards JSON file not found at {jsonPath}");
                return;
            }

            string jsonContent = File.ReadAllText(jsonPath);
            CardDataList cardList = JsonUtility.FromJson<CardDataList>("{\"cards\":" + jsonContent + "}");

            foreach (var card in cardList.cards)
            {
                _cards[card.id] = card;
            }

            Debug.Log($"Loaded {_cards.Count} cards from database");
        }

        public CardData GetCard(int id)
        {
            return _cards.TryGetValue(id, out CardData card) ? card : null;
        }

        public List<CardData> GetAllCards()
        {
            return new List<CardData>(_cards.Values);
        }

        public List<CardData> GetRandomDeck(int count)
        {
            List<CardData> allCards = GetAllCards();
            List<CardData> deck = new List<CardData>();

            for (int i = 0; i < count; i++)
            {
                if (allCards.Count > 0)
                {
                    int randomIndex = Random.Range(0, allCards.Count);
                    deck.Add(allCards[randomIndex]);
                }
            }

            return deck;
        }
    }
}

