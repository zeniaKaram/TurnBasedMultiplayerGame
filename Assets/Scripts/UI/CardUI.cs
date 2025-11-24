using CardGame.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.UI
{
    public class CardUI : MonoBehaviour
    {
        [Header("UI References")]
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI costText;
        public TextMeshProUGUI powerText;
        public TextMeshProUGUI abilityText;
        public Button cardButton;
        public Image cardImage;
        public Image selectedIndicator;

        private CardData _cardData;
        private GameUI _gameUI;
        private bool _isSelected = false;

        public void Initialize(CardData card, GameUI gameUI)
        {
            _cardData = card;
            _gameUI = gameUI;

            nameText.text = card.name;
            costText.text = $"Cost: {card.cost}";
            powerText.text = $"Power: {card.power}";

            if (card.ability != null)
            {
                abilityText.text = $"{card.ability.type} ({card.ability.value})";
            }
            else
            {
                abilityText.text = "";
            }

            cardButton.onClick.AddListener(OnCardClicked);
            UpdateSelectionVisual();
        }

        private void OnCardClicked()
        {
            _isSelected = !_isSelected;
            _gameUI?.OnCardSelected(_cardData, _isSelected);
            UpdateSelectionVisual();
        }

        private void UpdateSelectionVisual()
        {
            if (selectedIndicator != null)
            {
                selectedIndicator.gameObject.SetActive(_isSelected);
            }

            // Change color to indicate selection
            if (cardImage != null)
            {
                cardImage.color = _isSelected ? Color.yellow : Color.white;
            }
        }
    }
}

