
namespace MapObjects
{
    public class Unit
    {
        public int Q;
        public int R;
        public int S;

        private int _health;
        private int _damage;
        private int _ownerID;

        public Unit(int q, int r, int ownerID)
        {
            Q = q;
            R = r;
            S = S = -(q + r);
            _ownerID = ownerID;
        }

        public int GetOwnerID() { return _ownerID; }
    }
}