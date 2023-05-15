using UnityEngine;
using Network;
using TMPro;

namespace testNetwork
{
    public class PanelManager : MonoBehaviour
    {
        private const int LobbySize = 6;
        [SerializeField] GameObject[] _playerList = new GameObject[LobbySize];
        private Lobby _lobby;
        private Player _thisPlayer;
        private Player _currentTurnPlayer;

        public void Awake()
        {
            _lobby = Lobby.Instance;
            for (int i = 0; i < LobbySize; i++)
            {
                _playerList[i].SetActive(false);
            }
        }

        public void Start()
        {
            
        }

        public void UpdateCards()
        {
            _thisPlayer = _lobby.GetThisPlayer();
            _currentTurnPlayer = _lobby.GetTurnPlayer();
            if (_thisPlayer == null)
            {
                for (int i = 0; i < LobbySize; i++)
                {
                    _playerList[i].SetActive(false);
                }

                return;
            }
            int index = 0;
            _playerList[index].GetComponentInChildren<TMP_Text>().text
                = _thisPlayer.GetUsername() + (_thisPlayer == _currentTurnPlayer ? " *" : "");
            _playerList[index++].SetActive(true);
            foreach(Player player in _lobby.GetPlayers())
            {
                if (player != _thisPlayer)
                {
                    _playerList[index].GetComponentInChildren<TMP_Text>().text
                        = player.GetUsername() + (player == _currentTurnPlayer ? " *" : "");
                    _playerList[index++].SetActive(true);
                }
            }
            while (index < LobbySize)
            {
                _playerList[index++].SetActive(false);
            }
        }
        
        
    }
}