using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary> ******************************************************
/// This file will generate the map by creating a center hex,
/// then creating each one in a spiraling pattern outwards.
/// The variable mapRadius can be changed within Unity to change
/// the size of the map. Later on this can be used to generate
/// different map sizes in match creation via user selection. 
/// </summary> *****************************************************
public class HexGrid : MonoBehaviour
{
    [SerializeField] public GameObject hexPrefab; // to be set as one of the below types
    [SerializeField] public GameObject airHex;
    [SerializeField] public GameObject basicHex;
    [SerializeField] public GameObject forestHex;
    [SerializeField] public GameObject mountainHex;

    public int mapRadius;
    public int centerIslandRadius;
    private List<Hex> _hexList = new List<Hex>();
    private List<GameObject> _gameObjects = new List<GameObject>();
    
    // TODO: this will probably be changed once we implement multiplayer
    private List<Hex> _ownedHexes = new List<Hex>(); 
    private readonly Vector3[] _directionVectors =
    {
        new Vector3(1, 0, -1),
        new Vector3(1, -1, 0),
        new Vector3(0, -1, 1),
        new Vector3(-1, 0, 1),
        new Vector3(-1, 1, 0),
        new Vector3(0, 1, -1)
    };

    private void Start()
    {
        GenerateGrid();
    }

    /// <summary> ***********************************************
    /// This function creates the center tile at (0, 0, 0)
    /// then calls HexRing consecutively from the center outward.
    /// After all hexes are created and stored in _hexList,
    /// InstantiateHexes() is called
    /// </summary> **********************************************
    private void GenerateGrid()
    {
        // store the center of the map
        Hex center = new Hex(0, 0);
        _hexList.Add(center);
        // call ring from center outward. while i < 4, generate only land for center island
        for (int i = 1; i < mapRadius; i++)
        {
            HexRing(center.GetVectorCoordinates(), i);
        }
        InstantiateHexes();
    }

    /// <summary> ***********************************************
    /// This function generates a ring of hexes in a radius from
    /// the center tile. It starts at corner number 4 and works
    /// its way counter-clockwise.
    /// </summary> **********************************************
    private void HexRing(Vector3 center, int radius)
    {
        Vector3 hexCoordinates = AddCoordinates(center,
            CoordinateScale(_directionVectors[4], radius));
        for (int i = 0; i < 6; i++)
        {
            for(int j = 0; j < radius; j++)
            {
                _hexList.Add(new Hex((int)hexCoordinates.x, (int)hexCoordinates.y));
                hexCoordinates = HexNeighbor(hexCoordinates, i);
            }
        }
    }
    
    /// <summary> ***********************************************
    /// This function returns the result of adding the two
    /// passed Vectors
    /// </summary> ***********************************************
    private Vector3 AddCoordinates(Vector3 hexCoordinates, Vector3 addCoordinates)
    {
        return hexCoordinates + addCoordinates;
    }
    
    /// <summary> ***********************************************
    /// This function returns the result of multiplying the
    /// passed Vector by a scalar value.
    /// </summary> **********************************************
    private Vector3 CoordinateScale(Vector3 coordinates, int factor)
    {
        return coordinates * factor;
    }
    
    /// <summary> ***********************************************
    /// This functions the neighbor of a tile in the direction
    /// given using the array of direction vectors.
    /// </summary> **********************************************
    private Vector3 HexNeighbor(Vector3 coordinates, int direction)
    { 
        return AddCoordinates(coordinates, _directionVectors[direction]);
    }

    /// <summary> ***********************************************
    /// This function first checks if the center island is
    /// being generated, if so, then make sure no air tiles
    /// spawn. If not, then allow air tiles to spawn. After
    /// this check, a new GameObject is instantiated and
    /// a random rotation is applied.
    /// </summary> **********************************************
    private void InstantiateHexes()
    {
        int hexCount = 1;
        foreach (var hex in _hexList)
        {
            // get the tile type
            GetHexType(hex, hexCount > centerIslandRadius * 12 + 1); 
            hex.SetHexType(hexPrefab.gameObject.name);
            
            GameObject newHex = Instantiate(hexPrefab,
                hex.Position(),
                Quaternion.identity,
                this.transform);
            newHex.transform.Rotate(0f, Random.Range(0, 7) * 60, 0f, Space.Self);
            _gameObjects.Add(newHex);
            hexCount++;
        }
    }

    /// <summary> ***********************************************
    /// This function assigns which type each hex will be.
    /// It takes in coordinates and a boolean to signify if
    /// air hexes will be generated.
    /// </summary> **********************************************
    private void GetHexType(Hex hex, bool hasAir)
    {
        // air, basic, forest, mountain
        int[] typeCount = new int[4];
        // get all neighbors
        for (int i = 0; i < 6; i++)
        {
            Hex neighbor = GetHexAt(HexNeighbor(hex.GetVectorCoordinates(), i));

            if (neighbor == null) ; // do nothing
            else if (neighbor.GetHexType() == Hex.HexType.Air) typeCount[0]++;
            else if (neighbor.GetHexType() == Hex.HexType.Basic) typeCount[1]++;
            else if (neighbor.GetHexType() == Hex.HexType.Forest) typeCount[2]++;
            else if (neighbor.GetHexType() == Hex.HexType.Mountain) typeCount[3]++;
        }

        int randomOffset = Random.Range(0, 1000); // random offset so generation is different each run
        float noise = Mathf.PerlinNoise(randomOffset + hex.Q/(float)mapRadius, 
                                        randomOffset + hex.R/(float)mapRadius);
        
        if (noise > .65 && noise < .8) hexPrefab = basicHex;
        else if (noise > .8 && noise < .9) hexPrefab = forestHex;
        else if (noise > .9 && noise < 1) hexPrefab = mountainHex;
        else if (hasAir)
        {
            if (typeCount[1] + typeCount[2] + typeCount[3] > 2) hexPrefab = basicHex;
            else if (typeCount[2] > 2) hexPrefab = forestHex;
            else if (typeCount[3] > 2) hexPrefab = mountainHex;
            else hexPrefab = airHex;   
        }
        else hexPrefab = basicHex;
    }

    /// <summary> ***********************************************
    /// This function takes in a Vector3 and returns the hex at
    /// those coordinates. Returns null if hex doesn't exist.
    /// </summary> **********************************************
    public Hex GetHexAt(Vector3 coordinates)
    {
        foreach (Hex hex in _hexList)
        {
            if (hex.Q == (int)coordinates.x &&
                hex.R == (int)coordinates.y &&
                hex.S == (int)coordinates.z)
                return hex;
        }

        return null;
    }
    
    public List<Hex> GetHexList() { return _hexList; }
    public List<GameObject> GetGameObjectList() { return _gameObjects; }
}