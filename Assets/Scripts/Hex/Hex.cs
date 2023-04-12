using System;
using UnityEngine;

public class Hex
{
    // go to https://www.redblobgames.com/grids/hexagons/ to see how grid system works
    // Q + R + S = 0
    // S = -(Q + R)
    public readonly int Q;
    public readonly int R;
    public readonly int S;
    
    private HexType _hexType;
    private bool _isLand;
    
    static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;

    public Hex(int q, int r)
    {
        this.Q = q;
        this.R = r;
        this.S = -(q + r);
    }
    
    /// <summary> ***********************************************
    /// This function returns the world position for the hex so
    /// that they properly tile. The resulting vector is returned
    /// </summary> **********************************************
    public Vector3 Position()
    {
        const float radius = 1f;
        const float height = radius * 2;
        float width = WIDTH_MULTIPLIER * height;

        float vertical = height * 0.75f;
        float horizontal = width;

        return new Vector3(horizontal * (this.Q + this.R / 2f), 0, vertical * this.R);
    }
    
    /// <summary> ***********************************************
    /// This function returns the coordinates of this hex in
    /// a Vector3
    /// </summary> **********************************************
    public Vector3 GetVectorCoordinates()
    {
        return new Vector3(Q, R, S);
    }

    /// <summary> ***********************************************
    /// This function saves what type this hex is
    /// </summary> **********************************************
    public void SetHexType(string type)
    {
        _hexType = type switch
        {
            "air_tile" => HexType.Air,
            "basic_tile" => HexType.Basic,
            "forest_tile" => HexType.Forest,
            "mountain_tile" => HexType.Mountain,
            _ => throw new Exception($"Hex of type {_hexType} not supported")
        };
        if (_hexType == HexType.Air) _isLand = false;
        else _isLand = true;
    }
    
    /// <summary> ***********************************************
    /// This function returns what type this hex is
    /// </summary> **********************************************
    public HexType GetHexType() { return  _hexType; }
    
    /// <summary> ***********************************************
    /// This function returns if the land is land or air.
    /// </summary> **********************************************
    public bool isLand() { return _isLand; }

    /// <summary> ***********************************************
    /// This enum stores the different types of hexes
    /// </summary> **********************************************
    public enum HexType
    {
        Air, // only accessible to airships and cannot be considered within city border
        Basic, // the rest are accessible to all and can be considered within city border
        Forest, 
        Mountain 
    }
}
