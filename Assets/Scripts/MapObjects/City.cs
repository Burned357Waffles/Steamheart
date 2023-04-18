using System.Collections.Generic;
using UnityEngine;

namespace MapObjects
{
    /// <summary> ************************************************************
    /// This class is the data class for the city. It will hold the data for
    /// the city including the indexes oft the hexes within the borders from
    /// the HexGrid Lists, the health, damage, owner ID, and resources
    /// provided.
    /// </summary> ***********************************************************
    public class City
    {
        private readonly int _ownerID;
        private bool _isCapitol;
        private int _health;
        private int _damage;
        private readonly int _cityRadius;
        private readonly HexGrid _hexGrid;

        private readonly Dictionary<Hex.Hex, int> _controlledHexDictionary = new Dictionary<Hex.Hex, int>();
        private readonly List<Hex.Hex> _controlledHexes = new List<Hex.Hex>();

        public City(Hex.Hex cityCenterHex, int ownerID, bool isCapitol)
        {
            _cityRadius = 4;
            _ownerID = ownerID;
            _isCapitol = isCapitol;
            _health = 100; // we can play with values
            _damage = 10; // we can play with values
            _controlledHexes.Add(cityCenterHex);
            _hexGrid = Object.FindObjectOfType<HexGrid>();
            CreateCity();
        }
        
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
    }
}