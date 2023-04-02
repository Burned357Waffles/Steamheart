using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// video used for HexCoordinates.cs, Hex.cs and HexGrid.cs : https://www.youtube.com/watch?v=htZijEO7ZmE&list=PLcRSafycjWFdahp7K-GJBl4NUwzhVmAby

// below code starts around 5 min in
public class HexCoordinates : MonoBehaviour
{
    public static float xOffset = 2, yOffset = 1, zOffset = 1.73f;

    internal Vector3Int GetHexCoords()
        => offsetCoordinates;

    [Header("Offset Coordinates")]
    [SerializeField]
    private Vector3Int offsetCoordinates;

    private void Awake() 
    {
        offsetCoordinates = ConvertPositionToOffset(transform.position);
    }
    
    private Vector3Int ConvertPositionToOffset(Vector3 position)
    {
        int x = Mathf.CeilToInt(position.x / xOffset);
        int y = Mathf.RoundToInt(position.y / yOffset);
        int z = Mathf.RoundToInt(position.z / zOffset);
        return new Vector3Int(x,y,z);

    }


}
