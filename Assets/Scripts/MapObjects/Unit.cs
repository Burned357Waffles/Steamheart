
namespace MapObjects
{
    public class Unit
    {
        public int Q;
        public int R;
        public int S;

        public int Health;
        public int Damage;
        
        private int _ownerID;

        public Unit(int q, int r, int ownerID, int health, int damage)
        {
            Q = q;
            R = r;
            S = S = -(q + r);
            _ownerID = ownerID;
            Health = health;
            Damage = damage;
        }

        public int GetOwnerID() { return _ownerID; }
    }
}