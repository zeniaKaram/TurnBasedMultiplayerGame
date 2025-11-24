using CardGame.Networking;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.UI
{
    public class LobbyUI : MonoBehaviour
    {
        [Header("UI References")]
        public Button hostButton;
        public Button joinButton;
        public Button startButton;
        public TextMeshProUGUI statusText;
        public TMP_InputField ipInputField;
        public TextMeshProUGUI playerCountText;

        private NetworkManager _networkManager;
        private bool _isHost = false;

        private void Start()
        {
            _networkManager = FindObjectOfType<NetworkManager>();
            
            hostButton.onClick.AddListener(OnHostClicked);
            joinButton.onClick.AddListener(OnJoinClicked);
            startButton.onClick.AddListener(OnStartClicked);
            
            startButton.interactable = false;
            UpdateUI();
        }

        private void Update()
        {
            if (_networkManager != null)
            {
                if (_networkManager.numPlayers >= 2)
                {
                    startButton.interactable = _isHost;
                    playerCountText.text = $"Players: {_networkManager.numPlayers}/2";
                }
                else
                {
                    startButton.interactable = false;
                    playerCountText.text = $"Players: {_networkManager.numPlayers}/2";
                }
            }
        }

        private void OnHostClicked()
        {
            if (_networkManager != null)
            {
                _networkManager.StartHost();
                _isHost = true;
                statusText.text = "Hosting... Waiting for player 2";
                UpdateUI();
            }
        }

        private void OnJoinClicked()
        {
            if (_networkManager != null)
            {
                string ip = string.IsNullOrEmpty(ipInputField.text) ? "localhost" : ipInputField.text;
                _networkManager.networkAddress = ip;
                _networkManager.StartClient();
                _isHost = false;
                statusText.text = "Joining...";
                UpdateUI();
            }
        }

        private void OnStartClicked()
        {
            // Game starts automatically when 2 players connect
            statusText.text = "Starting game...";
        }

        private void UpdateUI()
        {
            hostButton.interactable = !_networkManager.isNetworkActive;
            joinButton.interactable = !_networkManager.isNetworkActive;
        }
    }
}

