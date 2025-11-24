using UnityEngine;

namespace CardGame.Networking
{
    public class LocalPlayerManager : MonoBehaviour
    {
        public static LocalPlayerManager Instance { get; private set; }

        public PlayerNetworkIdentity LocalPlayer { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetLocalPlayer(PlayerNetworkIdentity player)
        {
            LocalPlayer = player;
        }

        public bool IsLocalPlayer(string playerId)
        {
            return LocalPlayer != null && LocalPlayer.PlayerId == playerId;
        }
    }
}

