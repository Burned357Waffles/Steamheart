using UnityEngine;

namespace MapObjects
{
    public static class Combat
    {
        private static Unit _attacker;
        private static Unit _defender;
        
        // TODO: check ownership and move unit to defender's pos after defeating it
        public static bool InitiateCombat(Unit attacker, Unit defender)
        {
            _attacker = attacker;
            _defender = defender;
            return _attacker.GetOwnerID() != _defender.GetOwnerID() && Fight();
        }

        /// <summary> ***********************************************
        /// This function checks if a unit is killed by an attack. If
        /// it is, return true, if not, then subtract health and
        /// return false.
        /// </summary> **********************************************
        private static bool Fight()
        {
            if (_attacker.Damage >= _defender.Health) return true;

            _defender.Health -= _attacker.Damage;
            return false;
        }
    }
}