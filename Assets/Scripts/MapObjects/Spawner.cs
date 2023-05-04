using Hex;
using TMPro;
using System.Linq;
using UI.HUD;
using UnityEngine;

namespace MapObjects
{
    /// <summary> ************************************************************
    /// This class handles getting user input and spawning a unit at a
    /// chosen location. It also keeps track of which player owns the unit.
    /// </summary> ***********************************************************
    public class Spawner : MonoBehaviour
    {
        [SerializeField] public GameObject unit;
        [SerializeField] public GameObject meleeUnit;
        [SerializeField] public GameObject rangedUnit;
        [SerializeField] public GameObject airshipUnit;
        [SerializeField] public GameObject settlerUnit;
        [SerializeField] public TextMeshProUGUI playerIndicator;
        
        public bool unitTypeSelected;
        public bool citySpawnedThisTurn;
        
        private Transform _unitSelectorPanel;
        private HexGrid _hexGrid;
        private Hex.Hex _currentHex;
        private City _city;
        private UnitMovement _unitMovement;
        private UnitProductionSelector _unitTypeSelector;
        private int _currentPlayer;
        private int _currentHexIndex;

        private Camera _camera;

        private void Start()
        {
            UnitTypesData.InitUnitTypeDict();
            _camera = Camera.main;
            _hexGrid = FindObjectOfType<HexGrid>();
            _currentHexIndex = -1;
            _unitMovement = FindObjectOfType<UnitMovement>();
            _unitTypeSelector = FindObjectOfType<UnitProductionSelector>();
            _currentPlayer = 1;
            playerIndicator.text = _currentPlayer.ToString();
            unitTypeSelected = false;
        }

        private void Update()
        {
            DetectClick();
        }

        private void DetectClick()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            
            Ray ray = _camera!.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit)) return;
            if(!hit.transform.CompareTag("City")) return;

            _currentHexIndex = _hexGrid.GetHexIndexAtWorldPos(hit.transform.position);
            _city = FindSelectedCity(_hexGrid.GetHexList()[_currentHexIndex]);
            if (_city == null) return;
                
            if (!_city.CanSpawnThisTurn) return;
            
            // bring up unit selection menu
            var obj = _hexGrid.GetGameObjectList()[_currentHexIndex];
            _unitSelectorPanel = obj.transform.GetChild(0);
            UnitProductionSelector.AssignButtons(_unitSelectorPanel);
            _unitSelectorPanel.gameObject.SetActive(true);
        }

        public void AfterButtonClick()
        {
            if (!unitTypeSelected)
            {
                _currentHexIndex = -1;
                return;
            }
            
            SpawnUnit(_city, _currentPlayer);
            _unitSelectorPanel.gameObject.SetActive(false);
            _unitTypeSelector.ResetButtons();
        }

        private City FindSelectedCity(Hex.Hex hex)
        {
            return _hexGrid.GetCityList().FirstOrDefault(city => hex == city.GetCityCenter());
        }
        
        /// <summary> ***********************************************
        /// This function will spawn a unit given coordinates and
        /// ownerID of the current player.
        /// </summary> ***********************************************
        private void SpawnUnit(City city, int ownerID)
        {
            if (_hexGrid.GetUnitDictionary().ContainsKey(city.GetCityHexes()[0])) return;
            
            GameObject newUnitObject = Instantiate(unit, city.GetCityHexes()[0].WorldPosition, transform.rotation);
            Unit newUnit = new Unit(city.GetCityCenter().Q, city.GetCityCenter().R, ownerID);
            newUnit.SetType(unit.name);
            _hexGrid.GetUnitDictionary().Add(_hexGrid.GetHexAt(city.GetCityHexes()[0].GetVectorCoordinates()), newUnit);
            _hexGrid.GetUnitObjectDictionary().Add(newUnit, newUnitObject);
        }

        /// <summary> ***********************************************
        /// This function is called by a the end turn button and
        /// cycles the unit controller to the next player.
        /// </summary> **********************************************
        public void SetPlayer()
        {
            _currentPlayer++;
            if (_currentPlayer > _hexGrid.playerCount) _currentPlayer = 1;
            _unitMovement.SetPlayer(_currentPlayer);

            playerIndicator.text = _currentPlayer.ToString();
        }
        
        /// <summary> ***********************************************
        /// Getter for _currentPlayer.
        /// </summary> **********************************************
        public int GetCurrentPlayer() { return _currentPlayer; }

    }
}  