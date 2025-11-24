using System;
using UnityEngine;

namespace CardGame.Data
{
    [Serializable]
    public class CardAbility
    {
        public string type;
        public int value;
    }

    [Serializable]
    public class CardData
    {
        public int id;
        public string name;
        public int cost;
        public int power;
        public CardAbility ability;
    }

    [Serializable]
    public class CardDataList
    {
        public CardData[] cards;
    }
}

