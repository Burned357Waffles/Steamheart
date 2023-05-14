using System.Collections.Generic;
using MapObjects;
using Hex;
using UnityEngine;

namespace Misc
{
    /// <summary> ************************************************************
    /// This class will be made for each player and will hold what they own
    /// and all of their resources.
    /// </summary> ***********************************************************
    public class Player
    {
        public bool IsAlive;
        public int IronCountPerTurn = 0;
        public int WoodCountPerTurn = 0;
        public int TotalIronCount = 0;
        public int TotalWoodCount = 0;
        private Visibility _visibility = new Visibility();
        
        private readonly int _playerID;
        private int _ownedCapitols;
        private List<City> _ownedCities = new List<City>();

        public Player(int id)
        {
            _playerID = id;
            IsAlive = true;
        }
        
        public int GetPlayerID() { return _playerID; }
        public int GetNumCapitols() { return _ownedCapitols; }
        public List<City> GetOwnedCities() { return _ownedCities; }

        public void AssignCity(City city)
        {
            city.SetOwnership(_playerID);
            if (city.IsCapitol()) _ownedCapitols++;
            _ownedCities.Add(city);
        }
        public void RemoveCity(City city)
        {
            _ownedCities.Remove(city);
            if (_ownedCities.Count == 0) { IsAlive = false; }
        }
        
        public void UpdateUnitVisibility(Unit unit)
        {
            _visibility.AddUnitVisibility(unit);
        }
    }
}