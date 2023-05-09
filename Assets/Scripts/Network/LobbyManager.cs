using System;
using testNetwork;
using TMPro;
using UnityEngine;

namespace Network
{
    public class LobbyManager : MonoBehaviour
    {
        private Lobby _lobby;
        private Player _thisPlayer;
        private MessageQueue _messageQueue;
        private NetworkManager _networkManager;
        private PanelManager _panelManager;

        public void Start()
        {
            _lobby = Lobby.Instance;
            _networkManager = GameObject.Find("Network Manager").GetComponent<NetworkManager>();
            _messageQueue = _networkManager.GetComponent<MessageQueue>();
            _panelManager = GameObject.Find("Panel Manager").GetComponent<PanelManager>();
            _messageQueue.AddCallback(Constants.SMSG_JOIN, OnPlayerJoin);
            _messageQueue.AddCallback(Constants.SMSG_LEAVE, OnPlayerLeave);
            DontDestroyOnLoad(gameObject);
        }
        
        public void OnPlayerJoin(ExtendedEventArgs extArgs)
        {
            ResponseJoinEventArgs args = extArgs as ResponseJoinEventArgs;
            if (args == null || args.status != 0)
            {
                return;
            }
            if (_lobby.AddPlayer(new Player(args.user_id, args.username)))
            {
                _thisPlayer = _lobby.GetThisPlayer();
            }
            else
            {
                Debug.Log("error in PlayerJoin");
            }
            _panelManager.UpdateCards();

        }

        public void OnPlayerLeave(ExtendedEventArgs extArgs)
        {
            ResponseLeaveEventArgs args = extArgs as ResponseLeaveEventArgs;
            if (args == null)
            {
                return;
            }
            Player removedPlayer = _lobby.RemovePlayer(args.user_id);
            if (removedPlayer != null)
            {
                Debug.Log("removed " + removedPlayer);
                
            }
            else
            {
                Debug.Log("something went wrong removing player");
            }

            _panelManager.UpdateCards();
        }
        
        
    }
}