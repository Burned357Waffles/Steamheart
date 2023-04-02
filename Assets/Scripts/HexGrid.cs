using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    Dictionary<Vector3Int, Hex> hexTileDict = new Dictionary<Vector3Int, Hex>();
    Dictionary<Vector3Int, List<Vector3Int>> hexTileNeighborsDict = new Dictionary<Vector3Int, List<Vector3Int>>();

    private void Start()
     {

        foreach(Hex hex in FindObjectsOfType<Hex>())
        {
            hexTileDict[hex.HexCoords] = hex;
        }
        // testing code
        List<Vector3Int> neighbors = GetNeighborsFor(new Vector3Int(0,0,0));
        Debug.Log("Neighbors for (0,0,0) are:");
        foreach (Vector3Int neighborsPos in neighbors)
        {
            Debug.Log(neighborsPos);
        }
    }

    public Hex GetTileAt(Vector3Int hexCoordinates)
    {
        Hex result = null;
        hexTileDict.TryGetValue(hexCoordinates, out result);
        return result;
    }

    public List<Vector3Int> GetNeighborsFor(Vector3Int hexCoordinates)
    {
        if(hexTileDict.ContainsKey(hexCoordinates)==false)
            return new List<Vector3Int>();

        if(hexTileNeighborsDict.ContainsKey(hexCoordinates))
            return hexTileNeighborsDict[hexCoordinates];

        hexTileNeighborsDict.Add(hexCoordinates, new List<Vector3Int>());

        foreach( var direction in Direction.GetDirectionList(hexCoordinates.z))
        {
            if (hexTileDict.ContainsKey(hexCoordinates + direction))
            {
                hexTileNeighborsDict[hexCoordinates].Add(hexCoordinates + direction);
            }
        }
        return hexTileNeighborsDict[hexCoordinates]; 
    }

}
// shows up around 12:31 in video
public static class Direction
{
    public static List<Vector3Int> directionOffsetOdd = new List<Vector3Int>
    {

        new Vector3Int (-1,0,1),  //N1
        new Vector3Int (0,0,1),   //N2
        new Vector3Int (1,0,0),   //E
        new Vector3Int (0,0,-1),  //s2
        new Vector3Int (-1,0,-1), //s1
        new Vector3Int (-1,0,0),  //w
    };

    public static List<Vector3Int> directionOffsetEven = new List<Vector3Int>
    {

        new Vector3Int (0,0,1),  //N1
        new Vector3Int (1,0,1),   //N2
        new Vector3Int (1,0,0),   //E
        new Vector3Int (1,0,-1),  //s2
        new Vector3Int (0,0,-1), //s1
        new Vector3Int (-1,0,0),  //w
    };

    public static List<Vector3Int> GetDirectionList(int z)
        => z % 2 == 0 ? directionOffsetEven : directionOffsetOdd;    

}