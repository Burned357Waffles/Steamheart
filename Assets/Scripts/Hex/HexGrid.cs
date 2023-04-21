using System.Collections.Generic;
using System.Linq;
using MapObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hex
{
    /// <summary> ************************************************************
    /// This file will generate the map by creating a center hex, then
    /// creating each one in a spiraling pattern outwards. The variable
    /// mapRadius can be changed within Unity to change the size of the map.
    /// Later on this can be used to generate different map sizes in match
    /// creation via user selection. 
    /// </summary> ***********************************************************
    public class HexGrid : MonoBehaviour
    {
        [SerializeField] public GameObject hexPrefab; // to be set as one of the below types
        [SerializeField] public GameObject airHex;
        [SerializeField] public GameObject basicHex;
        [SerializeField] public GameObject forestHex;
        [SerializeField] public GameObject mountainHex;
    
        [SerializeField] public GameObject ownedHexPrefab; // to be set as one of the below types
        [SerializeField] public GameObject ownedBasicHex;
        [SerializeField] public GameObject ownedForestHex;
        [SerializeField] public GameObject ownedMountainHex;

        [SerializeField] public GameObject cityPrefab;

        public int mapRadius;
        public int centerIslandRadius;
        public int playerCount;
        public int capitolDistance;
    
        public readonly Dictionary<global::Hex.Hex, GameObject> _hexDictionary = new Dictionary<global::Hex.Hex, GameObject>();
        private readonly Dictionary<Unit, GameObject> _unitObjectDictionary = new Dictionary<Unit, GameObject>();
        private readonly Dictionary<global::Hex.Hex, Unit> _unitDictionary = new Dictionary<global::Hex.Hex, Unit>();
        private readonly List<global::Hex.Hex> _hexList = new List<global::Hex.Hex>();
        private readonly List<GameObject> _gameObjects = new List<GameObject>();
        private readonly List<City> _cityList = new List<City>();

        private static readonly Vector3[] DirectionVectors =
        {
            new Vector3(1, 0, -1),
            new Vector3(1, -1, 0),
            new Vector3(0, -1, 1),
            new Vector3(-1, 0, 1),
            new Vector3(-1, 1, 0),
            new Vector3(0, 1, -1)
        };

        // PUBLIC FUNCTIONS
        /// <summary> ***********************************************
        /// This function returns the result of adding the two
        /// passed Vectors.
        /// </summary> ***********************************************
        public static Vector3 AddCoordinates(Vector3 hexCoordinates, Vector3 addCoordinates)
        {
            return hexCoordinates + addCoordinates;
        }
    
        /// <summary> ***********************************************
        /// This function returns the result of multiplying the
        /// passed Vector by a scalar value.
        /// </summary> **********************************************
        public static Vector3 CoordinateScale(Vector3 coordinates, int factor)
        {
            return coordinates * factor;
        }
    
        /// <summary> ***********************************************
        /// This functions the neighbor of a tile in the direction
        /// given using the array of direction vectors.
        /// </summary> **********************************************
        public static Vector3 HexNeighbor(Vector3 coordinates, int direction)
        { 
            return AddCoordinates(coordinates, DirectionVectors[direction]);
        }
    
        /// <summary> ***********************************************
        /// This does the same as the above, but adds to Dictionary
        /// instead.
        /// </summary> **********************************************
        public void HexRing(Vector3 center, int radius, Dictionary<global::Hex.Hex, int> hexDict)
        {
            Vector3 hexCoordinates = AddCoordinates(center,
                CoordinateScale(DirectionVectors[4], radius));
            for (int i = 0; i < 6; i++)
            {
                for(int j = 0; j < radius; j++)
                {
                    global::Hex.Hex hex = _hexList.FirstOrDefault(hex => hex.Q == (int)hexCoordinates.x
                                                                         && hex.R == (int)hexCoordinates.y
                                                                         && hex.S == (int)hexCoordinates.z);
                    if (hex != null) hexDict.Add(hex, _hexList.IndexOf(hex));
                    hexCoordinates = HexNeighbor(hexCoordinates, i);
                }
            }
        }

        /// <summary> ***********************************************
        /// These are getter methods. Not much to say about these.
        /// </summary> **********************************************
        public List<global::Hex.Hex> GetHexList() { return _hexList; }
        public List<GameObject> GetGameObjectList() { return _gameObjects; }
        public Dictionary<global::Hex.Hex, Unit> GetUnitDictionary() { return _unitDictionary; } 
        public Dictionary<Unit, GameObject> GetUnitObjectDictionary() { return _unitObjectDictionary; } 
        public Vector3[] GetDirectionVector(){ return DirectionVectors; }
    
    
    
    
        // PRIVATE FUNCTIONS
        /// <summary> ***********************************************
        /// Runs once on startup. This is from MonoBehavior.
        /// </summary> **********************************************
        private void Start()
        {
            GenerateGrid();
            CreateCapitols();
        }

        /// <summary> ***********************************************
        /// This function creates the center tile at (0, 0, 0)
        /// then calls HexRing consecutively from the center outward.
        /// After all hexes are created and stored in _hexList,
        /// InstantiateHexes() is called.
        /// </summary> **********************************************
        private void GenerateGrid()
        {
            // store the center of the map
            global::Hex.Hex center = new global::Hex.Hex(0, 0);
            _hexList.Add(center);
            // call ring from center outward. while i < 4, generate only land for center island
            for (int i = 1; i < mapRadius; i++)
            {
                HexRing(center.GetVectorCoordinates(), i, _hexList);
            }
            InstantiateHexes();
        }

        /// <summary> ***********************************************
        /// This function generates a ring of hexes in a radius from
        /// the center tile. It starts at corner number 4 and works
        /// its way counter-clockwise. It adds each new hex to the
        /// list that the user passed in.
        /// </summary> **********************************************
        private static void HexRing(Vector3 center, int radius, List<global::Hex.Hex> hexListToAddTo)
        {
            Vector3 hexCoordinates = AddCoordinates(center,
                CoordinateScale(DirectionVectors[4], radius));
            for (int i = 0; i < 6; i++)
            {
                for(int j = 0; j < radius; j++)
                {
                    hexListToAddTo.Add(new global::Hex.Hex((int)hexCoordinates.x, (int)hexCoordinates.y));
                    hexCoordinates = HexNeighbor(hexCoordinates, i);
                }
            }
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
                hex.SetPosition();
                hex.WorldPosition = AddHeight(hex.WorldPosition);
            
                GameObject newHex = Instantiate(hexPrefab,
                    hex.WorldPosition,
                    Quaternion.identity,
                    this.transform);
                newHex.transform.Rotate(0f, Random.Range(0, 7) * 60, 0f, Space.Self);
                _gameObjects.Add(newHex);
                _hexDictionary.Add(hex, newHex);
                hexCount++;
            }
        }

        /// <summary> ***********************************************
        /// This function assigns which type each hex will be.
        /// It takes in coordinates and a boolean to signify if
        /// air hexes will be generated.
        /// </summary> **********************************************
        private void GetHexType(global::Hex.Hex hex, bool hasAir)
        {
            // air, basic, forest, mountain
            int[] typeCount = new int[4];
            // get all neighbors
            for (int i = 0; i < 6; i++)
            {
                global::Hex.Hex neighbor = GetHexAt(HexNeighbor(hex.GetVectorCoordinates(), i));

                if (neighbor == null) {} // do nothing
                else if (neighbor.GetHexType() == global::Hex.Hex.HexType.Air) typeCount[0]++;
                else if (neighbor.GetHexType() == global::Hex.Hex.HexType.Basic) typeCount[1]++;
                else if (neighbor.GetHexType() == global::Hex.Hex.HexType.Forest) typeCount[2]++;
                else if (neighbor.GetHexType() == global::Hex.Hex.HexType.Mountain) typeCount[3]++;
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
                else if (typeCount[2] > 1) hexPrefab = forestHex;
                else if (typeCount[3] > 1) hexPrefab = mountainHex;
                else hexPrefab = airHex;   
            }
            else hexPrefab = basicHex;
        }
    
        /// <summary> ***********************************************
        /// This function takes in a Vector3 and returns the hex at
        /// those coordinates. Returns null if hex doesn't exist.
        /// </summary> **********************************************
        public global::Hex.Hex GetHexAt(Vector3 coordinates)
        {
            return _hexList.FirstOrDefault(hex => hex.Q == (int)coordinates.x 
                                                  && hex.R == (int)coordinates.y 
                                                  && hex.S == (int)coordinates.z);
        }
    
        /// <summary> ***********************************************
        /// This function is used to add a randomized height to the
        /// hexes. It returns the new Vector3 with the changed
        /// y-value.
        /// </summary> **********************************************
        private static Vector3 AddHeight(Vector3 hex)
        {
            float randomOffset = Random.Range(0, 8);
            hex.y += randomOffset/100;

            return hex;
        }
    
        /// <summary> ***********************************************
        /// This function creates all the player capitols at the
        /// start of the match.
        /// </summary> ***********************************************
        private void CreateCapitols()
        {
            for (int i = 0; i < playerCount; i++)
            {
                global::Hex.Hex hexToPut = GetHexAt(AddCoordinates(_hexList[0].GetVectorCoordinates(), CoordinateScale(DirectionVectors[i], capitolDistance)));
                CreateCityAt(hexToPut, i + 1, true);
            }
        }
    
        /// <summary> ***********************************************
        /// This function will create a city.
        /// </summary> **********************************************
        private void CreateCityAt(global::Hex.Hex cityCenter, int ownerID, bool isCapitol)
        {
            City city = new City(cityCenter, ownerID, isCapitol);

            for (int i = 0; i < _hexList.Count(); i++)
            {
                if (_hexList[i].GetVectorCoordinates() != cityCenter.GetVectorCoordinates()) continue;
            
                Destroy(_gameObjects[i]);
                
                GameObject cityObject = Instantiate(cityPrefab,
                    cityCenter.WorldPosition,
                    Quaternion.identity,
                    this.transform);
                
                _hexList[i].MakeHexBuildingType();
                _gameObjects[i] = cityObject;
                _cityList.Add(city);
                ChangeCityHexPrefabs(city);
                return;
            }
        }

        /// <summary> ***********************************************
        /// This changes all of the tiles to owned type. This will
        /// likely be changed.
        /// </summary> **********************************************
        private void ChangeCityHexPrefabs(City city)
        {
            foreach (KeyValuePair<global::Hex.Hex, int> entry in city.GetCityDictionary())
            {
                if (entry.Key.GetHexType() == global::Hex.Hex.HexType.Basic) ownedHexPrefab = ownedBasicHex;
                else if (entry.Key.GetHexType() == global::Hex.Hex.HexType.Forest) ownedHexPrefab = ownedForestHex;
                else if (entry.Key.GetHexType() == global::Hex.Hex.HexType.Mountain) ownedHexPrefab = ownedMountainHex;
                else continue;
            
                Destroy(_gameObjects[entry.Value]);
                entry.Key.SetPosition();
                entry.Key.WorldPosition = AddHeight(entry.Key.WorldPosition);
                GameObject newHex = Instantiate(ownedHexPrefab,
                    entry.Key.WorldPosition,
                    Quaternion.identity,
                    this.transform);
                newHex.transform.Rotate(0f, Random.Range(0, 7) * 60, 0f, Space.Self);
                _gameObjects[entry.Value] = newHex;
            }
        }
    }
}