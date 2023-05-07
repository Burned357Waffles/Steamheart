using System.Collections.Generic;
using Hex;
using UnityEngine;

namespace MapObjects
{
    /// <summary> ************************************************************
    /// This class is the data class for the city. It will hold the data for
    /// the city including the indexes of the hexes within the borders from
    /// the HexGrid Lists, the health, damage, owner ID, and resources
    /// provided.
    /// </summary> ***********************************************************
    public class City
    {
        private Hex.Hex _center;
        private readonly int _ownerID;
        private bool _isCapitol;
        public int Health;
        public int Damage;
        private readonly int _cityRadius;
        private readonly HexGrid _hexGrid;
        public bool CanSpawnThisTurn;

        private readonly Dictionary<Hex.Hex, int> _controlledHexDictionary = new Dictionary<Hex.Hex, int>();
        private readonly List<Hex.Hex> _controlledHexes = new List<Hex.Hex>();

        public City(Hex.Hex cityCenterHex, int ownerID, bool isCapitol)
        {
            _center = cityCenterHex;
            _cityRadius = 4;
            _ownerID = ownerID;
            _isCapitol = isCapitol;
            Health = 6; // we can play with values
            Damage = 10; // we can play with values
            _controlledHexes.Add(_center);
            _hexGrid = Object.FindObjectOfType<HexGrid>();
            CreateCity();
            CanSpawnThisTurn = true;
        }

        public int GetOwnerID() { return _ownerID; }

        /// <summary> ***********************************************
        /// This is the same logic of creating the map.
        /// </summary> **********************************************
        private void CreateCity()
        {
            // call ring from center outward. while i < 4, generate only land for center island
            Hex.Hex center = _controlledHexes[0];
            for (int i = 1; i < _cityRadius; i++)
            {
                _hexGrid.HexRing(center.GetVectorCoordinates(), i, _controlledHexDictionary);
            }
            SetOwnership();
        }
        
        /// <summary> ***********************************************
        /// This function loops through all the hexes in the list and
        /// sets their owner.
        /// </summary> **********************************************
        private void SetOwnership()
        {
            foreach (var key in _controlledHexDictionary.Keys)
            {
                key.SetOwnerID(_ownerID);
            }
        }

        /// <summary> ***********************************************
        /// Getter methods
        /// </summary> **********************************************
        public List<Hex.Hex> GetCityHexes(){ return _controlledHexes; }
        public Dictionary<Hex.Hex, int> GetCityDictionary() { return _controlledHexDictionary; }
        public Hex.Hex GetCityCenter() { return _center; }
    }
}