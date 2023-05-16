using Hex;
using UnityEngine;
using UnityEngine.UI;

namespace MapObjects
{
    public class Settler : MonoBehaviour
    {
        // spawner creates settler, settler calls create city
        private HexGrid _hexGrid;
        private Hex.Hex _selectedPosition;
        private Spawner _spawner;
        private Unit _unit;
        private Camera _camera;
        private int _currentPlayer;
        private int _currentHexIndex;
        private FMODUnity.StudioEventEmitter _cityCreateEmitter;
        private FMODUnity.StudioEventEmitter _selectEmitter;
        private Button _settleButton;
        
        private void Start()
        {
            _hexGrid = FindObjectOfType<HexGrid>();
            _spawner = FindObjectOfType<Spawner>();
            _cityCreateEmitter = GameObject.Find("CityCreate").GetComponent<FMODUnity.StudioEventEmitter>();
            _selectEmitter = GameObject.Find("Select").GetComponent<FMODUnity.StudioEventEmitter>();
            _camera = Camera.main;
        }
        private void Update() 
        {
            DetectClick();

        }
    
        /// <summary> ***********************************************
        /// This function detects if a settler unit is selected and
        /// if the player wants to settle at that units' location.
        /// </summary> **********************************************
        private void DetectClick()
        {
            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = _camera!.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out RaycastHit hit)) return;

                if(!hit.transform.CompareTag("Unit")) return; 
                _currentHexIndex = _hexGrid.GetHexIndexAtWorldPos(hit.transform.position);
                _selectedPosition = _hexGrid.GetHexList()[_currentHexIndex];
                _unit = _hexGrid.GetUnitDictionary()[_selectedPosition];
                _currentPlayer = _spawner.GetCurrentPlayer();

                Debug.Log("unit id: " + _unit.GetOwnerID() + " current: " + _currentPlayer);
                if (_unit.GetOwnerID() != _currentPlayer)
                {
                    Debug.Log("not your unit");
                    _currentHexIndex = -1;
                    _selectedPosition = null;
                    return;
                }
                
                // is the unit a settler
                if (_unit.GetUnitType() == Unit.UnitType.Settler) return;
            
                _currentHexIndex = -1;
                _selectedPosition = null;

                _selectEmitter.Play();
                return;
            
            }

            if (_selectedPosition == null){ return;}
        
            if(Input.GetKeyDown(KeyCode.N)) // this will be replaced soon
            {
                _selectedPosition = _hexGrid.GetHexAt(_unit.GetVectorCoordinates());
                if (!_hexGrid.GetUnitDictionary().ContainsKey(_selectedPosition)) return;
                if (_hexGrid.GetUnitDictionary()[_selectedPosition].GetCurrentMovementPoints() <= 0) return; // TODO: error here sometimes
                if(CheckValidPlacement(_selectedPosition))
                {
                    SetPlayer();
                    _hexGrid.CreateCityAt(_selectedPosition, _currentPlayer, false);
                    Destroy(_hexGrid.GetUnitObjectDictionary()[_unit]);
                    _hexGrid.GetUnitDictionary().Remove(_selectedPosition);
                    _hexGrid.GetUnitObjectDictionary().Remove(_unit);
                    _cityCreateEmitter.Play();

                    _hexGrid.GetCityAt(_selectedPosition).CanSpawnThisTurn = false;
                }

                _selectedPosition = null;
                _currentHexIndex = -1;
            }
        }

        public void AfterButtonClick()
        {

        }

        /// <summary> ***********************************************
        /// This function checks all of the tiles in a 6 tile radius
        /// to ensure that city borders do not overlap. Returns true
        /// if the placement is a valid location.
        /// </summary> **********************************************
        private bool CheckValidPlacement(Hex.Hex selectedPosition)
        {
            int mapRadius = 7;
            Vector3 center = selectedPosition.GetVectorCoordinates();   
            for (int i = 1; i < mapRadius; i++)
            {
                Vector3 hexCoordinates = HexGrid.AddCoordinates(center,
                    HexGrid.CoordinateScale(_hexGrid.GetDirectionVector()[4], i));
                for (int k = 0; k < 6; k++)
                {
                    for(int j = 0; j < i; j++)
                    {
                        City city = _hexGrid.GetCityAt(_hexGrid.GetHexAt(hexCoordinates));
                        if (city != null) return false;
                        hexCoordinates = HexGrid.HexNeighbor(hexCoordinates, k);
                    }
                }
            }
            return true;
        }
    
        /// <summary> ***********************************************
        /// This function sets the current player in this file
        /// </summary> **********************************************
        private void SetPlayer() { _currentPlayer = _spawner.GetCurrentPlayer(); }
    }
}
