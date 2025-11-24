using Mirror;
using UnityEngine;

namespace CardGame.Networking
{
    public class CustomNetworkManager : NetworkManager
    {
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            
            GameObject player = conn.identity.gameObject;
            PlayerNetworkIdentity playerIdentity = player.GetComponent<PlayerNetworkIdentity>();
            
            if (playerIdentity != null)
            {
                Debug.Log($"Player connected: {playerIdentity.PlayerId}");
            }
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            Debug.Log("Player disconnected");
            base.OnServerDisconnect(conn);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            Debug.Log("Client connected to server");
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            Debug.Log("Client disconnected from server");
        }
    }
}

