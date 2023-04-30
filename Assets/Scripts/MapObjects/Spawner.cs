using Hex;
using TMPro;
using System.Collections.Generic;
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
        [SerializeField] public TextMeshProUGUI playerIndicator;
        private HexGrid _hexGrid;
        private HexMovement _hexMovement;
        private int _currentPlayer;
        public Material lowPolyCharacterTexture;


        private List<City> _cityList;
        private City _city;

        private void Start()
        {
            UnitTypesData.InitUnitTypeDict();
            _hexGrid = FindObjectOfType<HexGrid>();
            _hexMovement = FindObjectOfType<HexMovement>();
            _currentPlayer = 1;
            playerIndicator.text = _currentPlayer.ToString();
            _cityList = new List<City>();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.C)) // this will be replaced soon
            {
                SpawnUnit(0, 0, _currentPlayer);
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
                if(city.ownerID() == _currentPlayer )
                {
                    if (_hexGrid.GetUnitDictionary().ContainsKey(city.GetCityHexes()[0])) return;
                    //Vector3 vectorToPlace = new Vector3(city.GetCityHexes()[0].WorldPosition.x,
                        //city.GetCityHexes()[0].WorldPosition.y,
                        //city.GetCityHexes()[0].WorldPosition.z);
                    //vectorToPlace = new Vector3(0f, 0f, 0f);
                    GameObject newUnitObject = Instantiate(unit, city.GetCityHexes()[0].WorldPosition, transform.rotation);
                    Unit newUnit = new Unit(q, r, ownerID, Unit.UnitType.Melee);
                    _hexGrid.GetUnitDictionary().Add(_hexGrid.GetHexAt(city.GetCityHexes()[0].GetVectorCoordinates()), newUnit);
                    _hexGrid.GetUnitObjectDictionary().Add(newUnit, newUnitObject);
                }
                
            }

            //if (_hexGrid.GetUnitDictionary().ContainsKey(hex)) return;
            
            // Vector3 vectorToPlaceAt = new Vector3(hex.GetVectorCoordinates().x,
            //     hex.GetVectorCoordinates().y + 1.3f,
            //     hex.GetVectorCoordinates().z);
            // GameObject newUnitObject = Instantiate(unit, vectorToPlaceAt, transform.rotation);
        
            // _hexGrid.GetUnitDictionary().Add(hex, newUnit);
            // _hexGrid.GetUnitObjectDictionary().Add(newUnit, newUnitObject); 

        }

        /// <summary> ***********************************************
        /// This function is called by a the end turn button and
        /// cycles the unit controller to the next player.
        /// </summary> **********************************************
        public void SetPlayer()
        {
            _currentPlayer++;
            if (_currentPlayer > _hexGrid.playerCount) _currentPlayer = 1;
            _hexMovement.SetPlayer(_currentPlayer);

            playerIndicator.text = _currentPlayer.ToString();
        }

    }
}  