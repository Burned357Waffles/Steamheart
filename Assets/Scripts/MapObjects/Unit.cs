using System;
using UnityEngine;

namespace MapObjects
{
    /// <summary> ************************************************************
    /// This class is the data class for the unit. It will hold the data for
    /// the unit including the health, damage, attack radius, unit type,
    /// base movement points, current movement points, and owner ID.
    /// </summary> ***********************************************************
    public class Unit
    {
        public int Q;
        public int R;
        public int S;

        public int Health;
        public int Damage;
        public int AttackRadius;
        public int IronCost;
        public int WoodCost;

        private readonly int _ownerID;
        private UnitType _unitType;
        private int _baseMovementPoints;
        private int _currentMovementPoints;

        public Unit(int q, int r, int ownerID, string type)
        {
            Q = q;
            R = r;
            S = S = -(q + r);
            _ownerID = ownerID;
            _currentMovementPoints = _baseMovementPoints;
            SetType(type);
        }
        
        public Vector3 GetVectorCoordinates()
        {
            return new Vector3(Q, R, S);
        }

        public UnitType GetUnitType() { return _unitType; }
        public int GetOwnerID() { return _ownerID; }
        public int GetCurrentMovementPoints() { return _currentMovementPoints; }
        public void UseMovementPoints() { _currentMovementPoints--; }
        public void DepleteMovementPoints() { _currentMovementPoints = 0;}
        public void ResetMovementPoints() { _currentMovementPoints = _baseMovementPoints; }

        public void SetType(string type)
        {
            _unitType = type switch
            {
                "melee_unit" => UnitType.Melee,
                "ranged_unit" => UnitType.Ranged,
                "airship_unit" => UnitType.Airship,
                "settler_unit" => UnitType.Settler,
                _ => throw new Exception($"Unit of type {_unitType} not supported")
            };
            
            int[] stats = UnitTypesData.GetStats(_unitType);
            Damage= stats[0];
            Health = stats[1];
            _baseMovementPoints = stats[2];
            AttackRadius= stats[3];
            IronCost = stats[4];
            WoodCost = stats[5];
        }
        public enum  UnitType
        {
            Melee,
            Ranged,
            Airship,
            Settler
        }
    }
}