using Hex;
using TMPro;
using System.Collections.Generic;
using System.Data;
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
        private HexGrid _hexGrid;
        private int _currentHexIndex;
        private UnitMovement _unitMovement;
        private int _currentPlayer;
        public Material lowPolyCharacterTexture;
        public bool unitTypeSelected;


        private List<City> _cityList;
        private City _city;
        private Camera _camera;



        private void Start()
        {
            UnitTypesData.InitUnitTypeDict();
            _camera = Camera.main;
            _hexGrid = FindObjectOfType<HexGrid>();
            _currentHexIndex = -1;
            _unitMovement = FindObjectOfType<UnitMovement>();
            _currentPlayer = 1;
            playerIndicator.text = _currentPlayer.ToString();
            _cityList = new List<City>();
            unitTypeSelected = false;
        }

        private void Update()
        {
            DetectClick();
        }

        public void DetectClick()
        {
            if(Input.GetKeyDown(KeyCode.C)) // this will be replaced soon
            {
                if (!unitTypeSelected) return;
                SpawnUnit(0, 0, _currentPlayer);
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _camera!.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out RaycastHit hit)) return;
                if(!hit.transform.CompareTag("City")) return;

                _currentHexIndex = _hexGrid.GetHexIndexAtWorldPos(hit.transform.position);
                
                
            }
        }
        
        /// <summary> ***********************************************
        /// This function will spawn a unit given coordinates and
        /// ownerID of the current player.
        /// </summary> ***********************************************
        private void SpawnUnit(int q, int r, int ownerID)
        {
            
            Hex.Hex hex = _hexGrid.GetHexAt(new Vector3(0, 0, 0));

            List<City> citylist = _hexGrid.GetCityList();
            // loop through city

            foreach(City city in citylist)
            {
                if (city.OwnerID() != _currentPlayer) continue;
                if (_hexGrid.GetUnitDictionary().ContainsKey(city.GetCityHexes()[0])) return;
                
                GameObject newUnitObject = Instantiate(unit, city.GetCityHexes()[0].WorldPosition, transform.rotation);
                Unit newUnit = new Unit(q, r, ownerID);
                newUnit.SetType(unit.name);
                _hexGrid.GetUnitDictionary().Add(_hexGrid.GetHexAt(city.GetCityHexes()[0].GetVectorCoordinates()), newUnit);
                _hexGrid.GetUnitObjectDictionary().Add(newUnit, newUnitObject);

            }
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