using UnityEngine;

namespace MapObjects
{
    /// <summary> ************************************************************
    /// This class handles the health reduction of the defender by the
    /// attacker.
    /// </summary> ***********************************************************
    public static class Combat
    {
        private static Unit _attackerUnit;
        private static Unit _defenderUnit;
        private static City _attackerCity;
        private static City _defenderCity;
        private static Animator animator;

        public static bool InitiateCombat(Unit attacker, Unit defender)
        {
            _attackerUnit = attacker;
            _defenderUnit = defender;
            return _attackerUnit.GetOwnerID() != _defenderUnit.GetOwnerID() && UnitFight();
        }
        
        public static bool InitiateCombat(Unit attacker, City defender)
        {
            _attackerUnit = attacker;
            _defenderCity = defender;
            return _attackerUnit.GetOwnerID() != _defenderCity.GetOwnerID() && CityFight();
        }
        
        public static bool InitiateRetaliation(Unit attacker, Unit defender)
        {
            _attackerUnit = attacker;
            _defenderUnit = defender;
            return _attackerCity.GetOwnerID() != _defenderUnit.GetOwnerID() && UnitRetaliation();
        }
        
        public static bool InitiateRetaliation(City attacker, Unit defender)
        {
            _attackerCity = attacker;
            _defenderUnit = defender;
            return _attackerCity.GetOwnerID() != _defenderUnit.GetOwnerID() && CityRetaliation();
        }

        /// <summary> ***********************************************
        /// This function checks if a unit is killed by an attack. If
        /// it is, return true, if not, then subtract health and
        /// return false.
        /// </summary> **********************************************
        private static bool UnitFight()
        {
            if (_attackerUnit.Damage >= _defenderUnit.Health) return true;

            _defenderUnit.Health -= _attackerUnit.Damage;
            return false;
        }
        
        /// <summary> ***********************************************
        /// This function checks if a city is destroyed by an attack. If
        /// it is, return true, if not, then subtract health and
        /// return false.
        /// </summary> **********************************************
        private static bool CityFight()
        {
            if (_attackerUnit.Damage >= _defenderCity.Health) return true;

            _defenderCity.Health -= _attackerUnit.Damage;
            return false;
        }
        
        private static bool UnitRetaliation()
        {
            int retaliationDamage = _attackerUnit.Damage / 2;
            if (retaliationDamage >= _defenderUnit.Health) return true;

            _defenderUnit.Health -= retaliationDamage;
            return false;
        }        
        
        private static bool CityRetaliation()
        {
            if (_attackerCity.Damage >= _defenderUnit.Health) return true;

            _defenderUnit.Health -= _attackerCity.Damage;
            return false;
        }
    }
}