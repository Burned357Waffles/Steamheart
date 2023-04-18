using UnityEngine;
using Random = UnityEngine.Random;

namespace Hex
{
    /// <summary> ************************************************************
    /// This will detect if a grid position is clicked and will convert an air
    /// tile to a land tile. Currently, it is constantly listening for click
    /// on Update(), but it will eventually only start listening when tile
    /// selection is active.
    /// </summary> ***********************************************************
    public class HexPlacer : MonoBehaviour
    {
        [SerializeField] public GameObject hexPrefab; // this will be changed depending on button selected
    
        public int placementCount;

        private int _playerID;
        private HexGrid _hexGrid;

        public void SetPlayer(int id)
        {
            _playerID = id;
        }
    

        private void Start()
        {
            _hexGrid = GameObject.FindObjectOfType<HexGrid>();
            _playerID = 1;
        }

        private void Update()
        {
            DetectClick();
        }

        /// <summary> ***********************************************
        /// This function will detect if a Hex is clicked.
        /// </summary> **********************************************
        private void DetectClick()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (hexPrefab == null) return; // if button is not chosen
                Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out RaycastHit hit)) return;

                int hexIndex = GetHexIndexAtWorldPos(hit.transform.position);
                if (!_hexGrid.GetHexList()[hexIndex].IsValidLocation(_playerID)) return;
                if (!PlacementCount(hexIndex)) return;
                if (hexIndex != -1) ConvertHex(hexIndex);
            }
        }

        /// <summary> ***********************************************
        /// This function takes in a Vector3 of world coordinates
        /// and returns the Hex at that position.
        /// </summary> **********************************************
        private int GetHexIndexAtWorldPos(Vector3 coordinates)
        {
            int hexIndex = -1;
            for (int i = 0; i < _hexGrid.GetHexList().Count; i++)
            {
                if (_hexGrid.GetHexList()[i].WorldPosition != coordinates) continue;
                hexIndex = i;
                break;
            }
            return hexIndex;
        }
    
        /// <summary> ***********************************************
        /// This function takes in a Vector3 of grid coordinates and
        /// returns the Hex at that position.
        /// </summary> **********************************************
        private int GetHexIndexAtGridPos(Vector3 coordinates)
        {
            int hexIndex = -1;
            for (int i = 0; i < _hexGrid.GetHexList().Count; i++)
            {
                if (_hexGrid.GetHexList()[i].GetVectorCoordinates() == coordinates)
                {
                    hexIndex = i;
                    break;
                }
            }
            return hexIndex;
        }

        /// <summary> ***********************************************
        /// This function takes the list index of a Hex and if the
        /// Hex at that position is an air tile, it will be replaced
        /// with the selected Hex type.
        /// </summary> **********************************************
        private void ConvertHex(int hexIndex)
        {
            global::Hex.Hex selectedHex = _hexGrid.GetHexList()[hexIndex];
            GameObject hexObject = _hexGrid.GetGameObjectList()[hexIndex];
        
            Destroy(hexObject);

            GameObject newHex = Instantiate(hexPrefab,
                selectedHex.WorldPosition,
                Quaternion.identity,
                this.transform);
            newHex.transform.Rotate(0f, Random.Range(0, 7) * 60, 0f, Space.Self);
            _hexGrid.GetGameObjectList()[hexIndex] = newHex;
            selectedHex.SetHexType(hexPrefab.name);
        }

        /// <summary> ***********************************************
        /// This function checks every neighbor to see if there is an
        /// adjacent land hex and returns true if there is false if
        /// not.
        /// </summary> **********************************************
        private bool CheckForNeighbors(global::Hex.Hex selectedHex)
        {
            Vector3 hexCoordinates = HexGrid.AddCoordinates(selectedHex.GetVectorCoordinates(),
                HexGrid.CoordinateScale(_hexGrid.GetDirectionVector()[4], 1));
            int landCount = 0;
            for (int i = 0; i < 6; i++)
            {
                for(int j = 0; j < 1; j++)
                {
                    int hexIndex = GetHexIndexAtGridPos(hexCoordinates);
                    if (_hexGrid.GetHexList()[hexIndex].IsLand())
                    {
                        landCount++;
                        break;
                    }
                
                    hexCoordinates = HexGrid.HexNeighbor(hexCoordinates, i);
                }
            }
            return landCount > 0;
        }

        /// <summary> ***********************************************
        /// This function checks if the placement count has reached
        /// the max value for each hex type. If it has not reached
        /// max, then increment and return true to indicate that new
        /// hex can be placed. If has reached max count, then return
        /// false indicating that a new hex may not be placed.
        /// </summary> **********************************************
        private bool PlacementCount(int hexIndex)
        {
            if (_hexGrid.GetHexList()[hexIndex].GetHexType() != global::Hex.Hex.HexType.Air) return false;
            if (!CheckForNeighbors(_hexGrid.GetHexList()[hexIndex])) return false;
        
            if (hexPrefab == _hexGrid.ownedBasicHex && placementCount < 2)
            {
                placementCount++;
                return true;
            }

            if (hexPrefab != _hexGrid.ownedForestHex && hexPrefab != _hexGrid.ownedMountainHex) return false;
            if (placementCount >= 1) return false;
            placementCount++;
            return true;
        }
    }
}
    