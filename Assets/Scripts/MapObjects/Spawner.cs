using System.Collections.Generic;
using TMPro;
using System.Linq;
using Hex;
using Misc;
using UI.HUD;
using UnityEngine;
using UnityEngine.UI;

namespace MapObjects
{
    /// <summary> ************************************************************
    /// This class handles getting user input and spawning a unit at a
    /// chosen location. It also keeps track of which player owns the unit.
    /// </summary> ***********************************************************
    public class Spawner : MonoBehaviour
    {
        [SerializeField] public GameObject unit;
        [SerializeField] public GameObject NONE;
        [SerializeField] public GameObject meleeUnit;
        [SerializeField] public GameObject rangedUnit;
        [SerializeField] public GameObject airshipUnit;
        [SerializeField] public GameObject settlerUnit;
        
        
        public bool unitTypeSelected;
        //public bool citySpawnedThisTurn;
        
        private Transform _unitSelectorPanel;
        private HexGrid _hexGrid;
        private Hex.Hex _currentHex;
        private UnitMovement _hexMovement;
        private ResourceCounter _resourceCounter;
        private int _currentPlayer;
        private Animation _anim;


        private List<City> _cityList;
        private City _city;
        private UnitMovement _unitMovement;
        private UnitProductionSelector _unitTypeSelector;
        private int _currentHexIndex;

        private Camera _camera;
        private FMODUnity.StudioEventEmitter _selectEmitter;

        private void Start()
        {
            UnitTypesData.InitUnitTypeDict();
            _camera = Camera.main;
            _hexGrid = FindObjectOfType<HexGrid>();
            _currentHexIndex = -1;
            _unitMovement = FindObjectOfType<UnitMovement>();
            _unitTypeSelector = FindObjectOfType<UnitProductionSelector>();
            _resourceCounter = FindObjectOfType<ResourceCounter>();
            _selectEmitter = GameObject.Find("Select").GetComponent<FMODUnity.StudioEventEmitter>();
            _currentPlayer = 1;
            unitTypeSelected = false;
            _cityList = new List<City>();
            _anim = gameObject.GetComponent<Animation>();
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
            if (!hit.transform.CompareTag("City")) return;
            //if(!CheckIfCityOrButton(hit)) return;
            
            _currentHexIndex = _hexGrid.GetHexIndexAtWorldPos(hit.transform.position);
            _city = FindSelectedCity(_hexGrid.GetHexList()[_currentHexIndex]);
            if (_city == null) return;
            if (_city.GetOwnerID() != _currentPlayer)
            {
                _currentHexIndex = -1;
                return;
            }
            if (!_city.CanSpawnThisTurn) return;
            
            // bring up unit selection menu
            var obj = _hexGrid.GetGameObjectList()[_currentHexIndex];
            _unitSelectorPanel = obj.transform.GetChild(0);
            UnitProductionSelector.AssignButtons(_unitSelectorPanel);
            _unitSelectorPanel.gameObject.SetActive(true);
            _selectEmitter.Play();
        }

        private bool CheckIfCityOrButton(RaycastHit hit)
        {
            if (hit.collider.CompareTag("UnitSelectButton") ||
                hit.transform.CompareTag("City"))
            {
                Debug.Log("true");
                return true;
            }

            if (_unitSelectorPanel != null) _unitSelectorPanel.gameObject.SetActive(false);
            Debug.Log(hit.collider.tag);
            Debug.Log("false");
            
            return false;   
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
            if (city.GetOwnerID() != _currentPlayer)
            {
                _currentHexIndex = -1;
                _unitTypeSelector.ResetOnlyButtons();
                return;
            }
            
            if (_hexGrid.GetUnitDictionary().ContainsKey(city.GetCityCenter()))
            {
                _unitTypeSelector.ResetOnlyButtons();
                return;
            }

            Player player = _hexGrid.FindPlayerOfID(ownerID);
            Unit newUnit = new Unit(city.GetCityCenter().Q, city.GetCityCenter().R, ownerID, unit.name);
            Debug.Log("before resource check");
            Debug.Log("Iron Cost: " + newUnit.IronCost + " Total Iron: " + player.TotalIronCount);
            Debug.Log("Wood Cost: " + newUnit.WoodCost + " Total Wood: " + player.TotalWoodCount);
            if (newUnit.IronCost > player.TotalIronCount || newUnit.WoodCost > player.TotalWoodCount)
            {
                Debug.Log("not enough resources!");
                _unitTypeSelector.ResetOnlyButtons();
                return;
            }
            
            GameObject newUnitObject = Instantiate(unit, city.GetCityHexes()[0].WorldPosition, transform.rotation);
            _anim.Play();
            _hexGrid.GetUnitDictionary()
                .Add(_hexGrid.GetHexAt(city.GetCityHexes()[0].GetVectorCoordinates()), newUnit);
            _hexGrid.GetUnitObjectDictionary().Add(newUnit, newUnitObject);

            player.TotalIronCount -= newUnit.IronCost;
            player.TotalWoodCount -= newUnit.WoodCost;
            _resourceCounter.UpdateResourceCounts(_currentPlayer);
            Debug.Log("after unit creation");
            Debug.Log("Iron Cost: " + newUnit.IronCost + " Total Iron: " + player.TotalIronCount);
            Debug.Log("Wood Cost: " + newUnit.WoodCost + " Total Wood: " + player.TotalWoodCount);
            
            city.CanSpawnThisTurn = false;
        }

        /// <summary> ***********************************************
        /// This function is called by a the end turn button and
        /// cycles the unit controller to the next player.
        /// </summary> **********************************************
        public void SetPlayer(int id) { _currentPlayer = id; }
        
        /// <summary> ***********************************************
        /// Getter for _currentPlayer.
        /// </summary> **********************************************
        public int GetCurrentPlayer() { return _currentPlayer; }

    }
}  