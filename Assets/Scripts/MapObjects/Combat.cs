namespace MapObjects
{
    /// <summary> ************************************************************
    /// This class handles the health reduction of the defender by the
    /// attacker.
    /// </summary> ***********************************************************
    public static class Combat
    {
        private static Unit _attacker;
        private static Unit _defenderUnit;
        private static City _defenderCity;
        
        public static bool InitiateCombat(Unit attacker, Unit defender)
        {
            _attacker = attacker;
            _defenderUnit = defender;
            return _attacker.GetOwnerID() != _defenderUnit.GetOwnerID() && UnitFight();
        }
        
        public static bool InitiateCombat(Unit attacker, City defender)
        {
            _attacker = attacker;
            _defenderCity = defender;
            return _attacker.GetOwnerID() != _defenderCity.GetOwnerID() && CityFight();
        }

        /// <summary> ***********************************************
        /// This function checks if a unit is killed by an attack. If
        /// it is, return true, if not, then subtract health and
        /// return false.
        /// </summary> **********************************************
        private static bool UnitFight()
        {
            if (_attacker.Damage >= _defenderUnit.Health) return true;

            _defenderUnit.Health -= _attacker.Damage;
            _attacker.Health -= _defenderUnit.Damage;
            return false;
        }
        
        /// <summary> ***********************************************
        /// This function checks if a city is destroyed by an attack. If
        /// it is, return true, if not, then subtract health and
        /// return false.
        /// </summary> **********************************************
        private static bool CityFight()
        {
            if (_attacker.Damage >= _defenderCity.Health) return true;

            _defenderCity.Health -= _attacker.Damage;
            _attacker.Health -= _defenderCity.Damage;
            return false;
        }
    }
}