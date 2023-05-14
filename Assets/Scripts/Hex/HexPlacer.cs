using UnityEngine;
using Random = UnityEngine.Random;
using MapObjects;

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
        [SerializeField] public GameObject infoPanel;
        
        public int placementCount;

        private int _currentPlayer;
        private HexGrid _hexGrid;
        private UnitMovement _unitMovement;
        private Camera _camera;
        private bool _isHexPrefabNull;
        private bool _isSelecting;

        private Animator _animator;

        public void SetPlayer(int id)
        {
            _currentPlayer = id;
        }

        private void Start()
        {
            _isHexPrefabNull = hexPrefab == null;
            _camera = Camera.main;
            _hexGrid = FindObjectOfType<HexGrid>();
            _unitMovement = FindObjectOfType<UnitMovement>();
            SetPlayer(1);
        }

        private void Update()
        {
            DetectClick();
        }

        /// <summary> ***********************************************
        /// This function will detect if a Hex is clicked. And does
        /// the checks to place a hex at that location.
        /// </summary> **********************************************
        private void DetectClick()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (_isHexPrefabNull) return; // if button is not chosen
                Ray ray = _camera!.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out RaycastHit hit)) return;

                int hexIndex = _hexGrid.GetHexIndexAtWorldPos(hit.transform.position);
                if (hexIndex < 0) return;
                
                //if (!_hexGrid.GetHexList()[hexIndex].IsValidLocation(_currentPlayer)) return;
                if (!SelectedTileIsNeighbor(hexIndex))
                {
                    if (!CheckForUnit(hexIndex)) return;
                }
                
                if (!PlacementCount(hexIndex)) return;
                _unitMovement.SetCurrentIndex(-1);
                ConvertHex(hexIndex);
            }
        }

        private bool SelectedTileIsNeighbor(int hexIndex)
        {
            Hex selectedHex = _hexGrid.GetHexList()[hexIndex];
            for (int j = 0; j < 6; j++)
            {
                Vector3 neighbor = HexGrid.HexNeighbor(selectedHex.GetVectorCoordinates(), j);
                if (_hexGrid.GetHexAt(neighbor).GetOwnerID() == _currentPlayer )
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckForUnit(int hexIndex)
        {
            Hex selectedHex = _hexGrid.GetHexList()[hexIndex];
            for (int j = 0; j < 6; j++)
            {
                Vector3 neighbor = HexGrid.HexNeighbor(selectedHex.GetVectorCoordinates(), j);
                if (!_hexGrid.GetUnitDictionary().ContainsKey(_hexGrid.GetHexAt(neighbor))) continue;
                if (_hexGrid.GetUnitDictionary()[_hexGrid.GetHexAt(neighbor)].GetOwnerID() == _currentPlayer)
                    return true;
            }
            return false;
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
            Hex selectedHex = _hexGrid.GetHexList()[hexIndex];
            GameObject hexObject = _hexGrid.GetGameObjectList()[hexIndex];
        
            Destroy(hexObject);

            // TODO: land tiles placed here
            GameObject newHex = Instantiate(hexPrefab,
                selectedHex.WorldPosition,
                Quaternion.identity,
                this.transform);
            newHex.transform.Rotate(0f, Random.Range(0, 7) * 60, 0f, Space.Self);
            
            _animator = newHex.transform.GetChild(0).GetComponent<Animator>();
            selectedHex.SetHexType(hexPrefab.name);
            selectedHex.SetOwnerID(_currentPlayer);

            if (selectedHex.GetHexType() == Hex.HexType.Mountain)
            {
                _animator.Play("MountainCreation");
            }
            else { _animator.Play("LandCreation"); }

            _hexGrid.GetGameObjectList()[hexIndex] = newHex;
            _hexGrid.GetHexObjectDictionary()[_hexGrid.GetHexList()[hexIndex]] = newHex;
            
        }

        /// <summary> ***********************************************
        /// This function checks every neighbor to see if there is an
        /// adjacent land hex and returns true if there is false if
        /// not.
        /// </summary> **********************************************
        private bool CheckForNeighbors(Hex selectedHex)
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
            if (_hexGrid.GetHexList()[hexIndex].GetHexType() != Hex.HexType.Air) return false;
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
    
