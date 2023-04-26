using Hex;
using TMPro;
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

        private void Start()
        {
            UnitTypesData.InitUnitTypeDict();
            _hexGrid = FindObjectOfType<HexGrid>();
            _hexMovement = FindObjectOfType<HexMovement>();
            _currentPlayer = 1;
            playerIndicator.text = _currentPlayer.ToString();
            //GetComponent<Renderer>().material = lowPolyCharacterTexture;
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
            if (_hexGrid.GetUnitDictionary().ContainsKey(hex)) return;
            
            Unit newUnit = new Unit(q, r, ownerID, Unit.UnitType.Ranged);  // TODO: change to player select

            Vector3 vectorToPlaceAt = new Vector3(hex.GetVectorCoordinates().x,
                hex.GetVectorCoordinates().y + 1.3f,
                hex.GetVectorCoordinates().z);
            GameObject newUnitObject = Instantiate(unit, vectorToPlaceAt, transform.rotation);
            
            _hexGrid.GetUnitDictionary().Add(hex, newUnit);
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
            _hexMovement.SetPlayer(_currentPlayer);

            playerIndicator.text = _currentPlayer.ToString();
        }
    }
}  