using System;
using UnityEngine;

namespace Hex
{
    /// <summary> ************************************************************
    /// The data class for each Hex. This holds the grid coordinates of this
    /// hex, can convert grid coordinates into world coordinates, holds
    /// the hex type that this tile is, and the owner ID. If ownerID = 0, it
    /// is unowned.
    /// </summary> ***********************************************************
    public class Hex
    {
        // go to https://www.redblobgames.com/grids/hexagons/ to see how grid system works
        // Q + R + S = 0
        
        public readonly HexCoord HexCoord;
        public int Q { get { return HexCoord.Q; } }
        public int R { get { return HexCoord.R; } }
        public int S { get { return HexCoord.S; } }
        public Vector3 WorldPosition;
    
        private HexType _hexType;
        private int _ownerID;
        private bool _isLand;
        private bool _isBlocked;
        static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;

        public Hex(int q, int r)
        {
            HexCoord = new HexCoord(q, r);
            _ownerID = 0;
        }
    
        /// <summary> ***********************************************
        /// This function returns the world position for the hex so
        /// that they properly tile and assigns it to WorldPosition.
        /// </summary> **********************************************
        public void SetPosition()
        {
            const float radius = 1f;
            const float height = radius * 2;
            float width = WIDTH_MULTIPLIER * height;

            const float vertical = height * 0.75f;
            float horizontal = width;
        
            WorldPosition = new Vector3(horizontal * (this.Q + this.R / 2f), 0, vertical * this.R);
        }
    
        /// <summary> ***********************************************
        /// This function returns the coordinates of this hex in
        /// a Vector3.
        /// </summary> **********************************************
        public Vector3 GetVectorCoordinates()
        {
            return new Vector3(Q, R, S);
        }

        /// <summary> ***********************************************
        /// This function saves what type this hex is and sets if it
        /// is a land type or not. It also sets the isBlocked
        /// property to determine if it is traversable.
        /// </summary> **********************************************
        public void SetHexType(string type)
        {
            _hexType = type switch
            {
                "air_tile" => HexType.Air,
                "basic_tile_2" => HexType.Basic,
                "forest_tile_2" => HexType.Forest,
                "mountain_tile_2" => HexType.Mountain,
                "basic_tile_owned_2" => HexType.Basic,
                "forest_tile_owned_2" => HexType.Forest,
                "mountain_tile_owned_2" => HexType.Mountain,
                _ => throw new Exception($"Hex of type {_hexType} not supported")
            };
            _isLand = _hexType != HexType.Air;
            _isBlocked = _hexType is HexType.Air or HexType.Mountain;
        }

        /// <summary> ***********************************************
        /// This function sets the HexType to a building and ensures
        /// that it is marked as a land hex.
        /// </summary> **********************************************
        public void MakeHexBuildingType()
        {
            _hexType = HexType.Building;
            _isBlocked = false;
            _isLand = true;
        }

        public bool IsValidLocation(int id)
        {
            return _ownerID == id || _ownerID == 0;
        }

        /// <summary> ***********************************************
        /// This function sets who owns this hex.
        /// </summary> **********************************************
        public void SetOwnerID(int ownerID) { _ownerID = ownerID; }
        public int GetOwnerID(){return _ownerID;}

        /// <summary> ***********************************************
        /// This function returns what type this hex is.
        /// </summary> **********************************************
        public HexType GetHexType() { return  _hexType; }
    
        /// <summary> ***********************************************
        /// This function returns if the land is land or air.
        /// </summary> **********************************************
        public bool IsLand() { return _isLand; }

        public bool IsBlocked() {return _isBlocked;}

        /// <summary> ***********************************************
        /// This enum stores the different types of hexes.
        /// </summary> **********************************************
        public enum HexType
        {
            Air, // only accessible to airships and cannot be considered within city border
            Basic, // the rest are accessible to all and can be considered within city border
            Forest, 
            Mountain,
            Building
        }

    }

    public class HexCoord
    {
        public int Q => _vec.x;

        public int R => _vec.y;
        public int S => _vec.z;

        private Vector3Int _vec;

        public HexCoord(int q, int r)
        {
            _vec = new Vector3Int(q, r, -(q + r));
        }

        // define type conversion from HexCoord to Vector3
        public static implicit operator Vector3(HexCoord hexCoord)
        {
            return new Vector3(hexCoord.Q, hexCoord.R, hexCoord.S);
        }

        // define type conversion from Vector3 to HexCoord
        public static implicit operator HexCoord(Vector3 vec)
        {
            return new HexCoord((int)vec.x, (int)vec.y);
        }

        public static HexCoord operator+ (HexCoord a, HexCoord b)
        {
            return new HexCoord(a.Q + b.Q, a.R + b.R);
        }
    }
}
