using System;

namespace Network
{
    public class Player
    {
        private bool Equals(Player other)
        {
            return _id == other._id && _username == other._username;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Player)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id, _username);
        }

        private readonly int _id;
        private readonly string _username;

        public Player(int id, string username)
        {
            this._id = id;
            this._username = username;
        }

        public Player(Player copy)
        {
            this._id = copy._id;
            this._username = copy._username;
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
        
        public static bool operator ==(Player self, Player other)
        {
            if (self == null || other == null)
            {
                return false;
            }

            if (ReferenceEquals(self, other))
            {
                return true;
            }

            if (self._id == other._id && self._username.Equals(other._username))
            {
                return true;
            }

            return false;
        }

        public static bool operator !=(Player self, Player other)
        {
            return !(self == other);
        }
        
    }
}