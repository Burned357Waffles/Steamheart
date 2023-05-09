using System.Collections.Generic;

namespace Network
{
    public class Lobby
    {
        private const int LobbySize = 6;
        private static Lobby instance;
        private Player[] _lobby;
        private int _thisPlayerIndex;
        private int _currentTurnIndex;

        private Lobby()
        {
            _lobby = new Player[LobbySize];
        }

        public static Lobby Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Lobby();
                }
                return instance;
            }
        }

        public Player GetThisPlayer()
        {
            return _lobby[_thisPlayerIndex];
        }

        public List<Player> GetPlayers()
        {
            List<Player> playerList = new List<Player>();
            foreach (Player player in _lobby)
            {
                if (player != null)
                {
                    playerList.Add(player);
                }
            }

            return playerList;
        }

        public bool AddPlayer(Player player)
        {
            if (_lobby[player.GetID() - 1] != null)
            {
                return false;
            }
            if (IsEmpty())
            {
                _thisPlayerIndex = player.GetID() - 1;
            }
            _lobby[player.GetID() - 1] = player;
            return true;
        }

        public Player RemovePlayer(int playerID)
        {
            if (_lobby[playerID - 1] == null)
            {
                return null;
            }

            Player player = _lobby[playerID - 1];
            _lobby[playerID - 1] = null;
            return player;
        }

        public bool IsEmpty()
        {
            foreach (Player player in _lobby)
            {
                if (player != null)
                {
                    return false;
                }
            }
            return true;
        }

        public override string ToString()
        {
            string lobbyString = "[\n";
            foreach (Player player in _lobby)
            {
                
                lobbyString += (player != null) ? "\t" + player + "\n" : "";
            }
            lobbyString += "]";
            return lobbyString;
        }
        
    }
}