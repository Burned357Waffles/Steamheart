namespace Network
{
    public class Player
    {
        private int _id;
        private string _username;

        public Player(int id, string username)
        {
            this._id = id;
            this._username = username;
        }

        public int GetID()
        {
            return _id;
        }

        public string GetUsername()
        {
            return _username;
        }

        public override string ToString()
        {
            return "id: " + _id + "\tusername: " + _username;
        }
        
    }
}