using CardGame.Networking;
using Mirror;
using UnityEngine;

namespace CardGame.Networking
{
    public class PlayerNetworkIdentity : NetworkBehaviour
    {
        [SyncVar(hook = nameof(OnPlayerIdChanged))]
        private string _playerId;

        public string PlayerId => _playerId;

        public override void OnStartServer()
        {
            base.OnStartServer();
            _playerId = $"P{connectionToClient.connectionId}";
            NetworkGameManager.Instance?.RegisterPlayer(_playerId, connectionToClient);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (isLocalPlayer)
            {
                // Set local player reference
                LocalPlayerManager.Instance?.SetLocalPlayer(this);
            }
        }

        private void OnPlayerIdChanged(string oldId, string newId)
        {
            _playerId = newId;
        }

        [Command]
        public void CmdEndTurn(int[] cardIds)
        {
            EndTurnMessage msg = new EndTurnMessage
            {
                playerId = _playerId,
                selectedCardIds = cardIds
            };
            string json = JsonUtility.ToJson(msg);
            NetworkGameManager.Instance?.CmdEndTurn(json);
        }
    }
}

