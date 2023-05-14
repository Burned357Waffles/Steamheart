using System.Collections.Generic;
using Hex;
using MapObjects;

namespace Hex
{
    public class Visibility
    {
        private List<HexCoord> _visibleHexes = new List<HexCoord>();

        public List<HexCoord> GetVisibleHexes() { return _visibleHexes; }

        public bool AddHex(HexCoord hex)
        {
            if (!_visibleHexes.Contains(hex)) 
            {
                _visibleHexes.Add(hex);
                return true;
            };
            return false;
        }

        public List<HexCoord> AddHexes(List<HexCoord> hexes)
        {
            List<HexCoord> addedHexes = new List<HexCoord>();
            foreach (HexCoord hex in hexes)
            {
                if (AddHex(hex)) addedHexes.Add(hex);
            }
            return addedHexes;
        }

        public bool RemoveHex(HexCoord hex)
        {
            if (_visibleHexes.Contains(hex)) 
            {
                _visibleHexes.Remove(hex);
                return true;
            }
            return false;
        }

        public List<HexCoord> RemoveHexes(List<HexCoord> hexes)
        {
            List<HexCoord> removedHexes = new List<HexCoord>();
            foreach (HexCoord hex in hexes)
            {
                if(RemoveHex(hex)) removedHexes.Add(hex);
            }
            return removedHexes;
        }

        public void ClearHexes() { _visibleHexes.Clear(); }

        public List<HexCoord> AddCityVisibility(City city)
        {
            return AddHexes(HexCoord.ToHexCoordList(city.GetCityHexes()));
        }

        public List<HexCoord> AddUnitVisibility(Unit unit)
        {
            return AddHexes(unit.GetVisibleHexes());
        }
    }
}
