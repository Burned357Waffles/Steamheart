using System.Linq;
using Hex;
using MapObjects;
using Misc;
using TMPro;
using UnityEngine;

namespace UI.HUD
{
    public class EndTurn : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI playerIndicator;
        
        private HexGrid _hexGrid;
        private HexPlacer _hexPlacer;
        private HexTypeSelector _hexTypeSelector;
        private Spawner _spawner;
        private int _currentPlayer;
        private UnitMovement _unitMovement;
        private FMODUnity.StudioEventEmitter _endTurnEmitter;

        private void Start()
        {
            _currentPlayer = 1;
            _hexGrid = FindObjectOfType<HexGrid>();
            _hexPlacer = FindObjectOfType<HexPlacer>();
            _hexTypeSelector = FindObjectOfType<HexTypeSelector>();
            _spawner = FindObjectOfType<Spawner>();
            _unitMovement = FindObjectOfType<UnitMovement>();
            _endTurnEmitter = GameObject.Find("EndTurnButton").GetComponent<FMODUnity.StudioEventEmitter>();
            playerIndicator.text = _currentPlayer.ToString();
        }

        private void HealCity()
        {
            foreach (var city in _hexGrid.GetCityList().Where(city => city.GetOwnerID() == _currentPlayer)) // TODO: fix healing each end turn
            {
                Debug.Log("City health before: " + city.Health);
                if (city.Health >= 25) continue;
                int add = 25 - (city.Health % 25);
                Debug.Log("First value: " + add);
                if (add > 4) add = 4;
                Debug.Log("Final value: " + add);
                city.Health += add;
                Debug.Log("City health after: " + city.Health);
            }
        }

        private void AdvancePlayer()
        {
            _currentPlayer++;
            if (_currentPlayer > _hexGrid.GetPlayerList().Count) _currentPlayer = 1;
            _hexPlacer.SetPlayer(_currentPlayer);
            _unitMovement.SetPlayer(_currentPlayer);
            _spawner.SetPlayer(_currentPlayer);
            Debug.Log("Player count = " + _hexGrid.playerCount);
            playerIndicator.text = _currentPlayer.ToString();
        }

        private void CheckForWin()
        {
            foreach (Player player in _hexGrid.GetPlayerList())
            {
                Debug.Log("Player " + player.GetPlayerID() + " has " + player.GetOwnedCities().Count +
                          " cities and " + player.GetNumCapitols() + " capitols");
                if (player.GetNumCapitols() != _hexGrid.playerCount) continue;
                Debug.Log("Player " + player.GetPlayerID() + " has won!");
                #if UNITY_EDITOR
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                }
                #else 
		        {
			        Application.Quit();
		        }
                #endif
            }
        }
        
        /// <summary> ***********************************************
        /// This function sets all unit movement points back to their
        /// base values.
        /// </summary> **********************************************
        public void ResetUnitMovementPoints()
        {
            foreach (Unit unit in _hexGrid.GetUnitDictionary().Values)
            {
                unit.ResetMovementPoints();
            }
        }
        
        public void ProcessEndTurn()
        {
            _endTurnEmitter.Play();
            //HealCity(); 
            ResetUnitMovementPoints();
            AdvancePlayer();
            _hexTypeSelector.ResetPlacementCount();
            CheckForWin();
        }
    }
}