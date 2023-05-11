using System.Collections.Generic;

namespace Network
{
    public class Lobby
    {
        private const int LobbySize = 6;
        private static Lobby instance;
        private Player[] _lobby;
        private int _thisPlayerIndex = -1;
        private int _currentTurnIndex = -1;

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
        
        public void AdvanceTurn()
        {
            while (true)
            {
                int i;
                for (i = _currentTurnIndex + 1; i < LobbySize; i++)
                {
                    if (_lobby[_currentTurnIndex] == null) continue;
                    _currentTurnIndex = i;
                    return;
                }

                _currentTurnIndex = -1;
            }
        }

        public Player GetThisPlayer()
        {
            return _lobby[_thisPlayerIndex];
        }

        public Player GetPlayerByID(int id)
        {
            return _lobby[id - 1];
        }

        public Player GetTurnPlayer()
        {
            return _lobby[_currentTurnIndex];
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
            if (_thisPlayerIndex == -1)
            {
                _thisPlayerIndex = player.GetID() - 1;
            }
            _lobby[player.GetID() - 1] = player;
            return true;
        }

        private static void WipeLobby()
        {
            instance = new Lobby();
            /*
             * _lobby = new Player[6];
             * _thisPlayerIndex = -1;
             * _currentTurnIndex = -1;
             */
        }

        public Player RemovePlayer(int playerID)
        {
            if (_lobby[playerID - 1] == null)
            {
                return null;
            }

            if (playerID - 1 == _thisPlayerIndex)
            {
                Player thisPlayer = new Player(_lobby[_thisPlayerIndex]);
                WipeLobby();
                return thisPlayer;

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
            foreach (Player player in GetPlayers())
            {
                lobbyString += "\t" + player;
                if (player.GetID() - 1 == _thisPlayerIndex)
                {
                    lobbyString += " *";
                }

                if (player.GetID() - 1 == _currentTurnIndex)
                {
                    lobbyString += " !";
                }

                lobbyString += "\n";

            }
            lobbyString += "]";
            return lobbyString;
        }
        
        
        
    }
}