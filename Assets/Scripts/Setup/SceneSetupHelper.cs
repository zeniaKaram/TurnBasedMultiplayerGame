using CardGame.Data;
using CardGame.Game;
using CardGame.Networking;
using CardGame.UI;
using Mirror;
using UnityEngine;

namespace CardGame.Setup
{
    /// <summary>
    /// Helper script to quickly set up the game scene with all required components.
    /// Attach this to an empty GameObject and run it once to set up the scene.
    /// </summary>
    public class SceneSetupHelper : MonoBehaviour
    {
        [ContextMenu("Setup Scene")]
        public void SetupScene()
        {
            // Create NetworkManager
            GameObject networkManagerObj = GameObject.Find("NetworkManager");
            if (networkManagerObj == null)
            {
                networkManagerObj = new GameObject("NetworkManager");
                CustomNetworkManager networkManager = networkManagerObj.AddComponent<CustomNetworkManager>();
                
                // Configure NetworkManager
                networkManager.autoCreatePlayer = true;
                networkManager.playerPrefab = CreatePlayerPrefab();
            }

            // Create CardDatabase
            GameObject cardDatabaseObj = GameObject.Find("CardDatabase");
            if (cardDatabaseObj == null)
            {
                cardDatabaseObj = new GameObject("CardDatabase");
                cardDatabaseObj.AddComponent<CardDatabase>();
            }

            // Create GameManager
            GameObject gameManagerObj = GameObject.Find("GameManager");
            if (gameManagerObj == null)
            {
                gameManagerObj = new GameObject("GameManager");
                gameManagerObj.AddComponent<GameManager>();
            }

            // Create NetworkGameManager
            GameObject networkGameManagerObj = GameObject.Find("NetworkGameManager");
            if (networkGameManagerObj == null)
            {
                networkGameManagerObj = new GameObject("NetworkGameManager");
                networkGameManagerObj.AddComponent<NetworkGameManager>();
            }

            // Create LocalPlayerManager
            GameObject localPlayerManagerObj = GameObject.Find("LocalPlayerManager");
            if (localPlayerManagerObj == null)
            {
                localPlayerManagerObj = new GameObject("LocalPlayerManager");
                localPlayerManagerObj.AddComponent<LocalPlayerManager>();
            }

            Debug.Log("Scene setup complete! Remember to set up UI manually.");
        }

        private GameObject CreatePlayerPrefab()
        {
            GameObject playerPrefab = new GameObject("PlayerPrefab");
            playerPrefab.AddComponent<PlayerNetworkIdentity>();
            return playerPrefab;
        }
    }
}

