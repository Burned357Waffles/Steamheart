using System.Collections.Generic;
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
        [SerializeField] public GameObject endTurnButton;
        [SerializeField] public GameObject hexPrefab;
        
        private HexGrid _hexGrid;
        private HexPlacer _hexPlacer;
        private HexTypeSelector _hexTypeSelector;
        private Spawner _spawner;
        private int _currentPlayer;
        private UnitMovement _unitMovement;
        private FMODUnity.StudioEventEmitter _endTurnEmitter;

        private Dictionary<GameObject, GameObject> _hexPrefabsDict = new Dictionary<GameObject, GameObject>();

        private Dictionary<Hex.Hex.HexType, GameObject> _hexTypeDict = new Dictionary<Hex.Hex.HexType, GameObject>();


        private void Start()
        {
            _currentPlayer = 1;
            _hexGrid = FindObjectOfType<HexGrid>();
            _hexPlacer = FindObjectOfType<HexPlacer>();
            _hexTypeSelector = FindObjectOfType<HexTypeSelector>();
            _spawner = FindObjectOfType<Spawner>();
            _unitMovement = FindObjectOfType<UnitMovement>();
            _endTurnEmitter = endTurnButton.GetComponent<FMODUnity.StudioEventEmitter>();
            playerIndicator.text = _currentPlayer.ToString();
            
            _hexPrefabsDict.Add(_hexGrid.ownedBasicHex, _hexGrid.basicHex);
            _hexPrefabsDict.Add(_hexGrid.ownedForestHex, _hexGrid.forestHex);
            _hexPrefabsDict.Add(_hexGrid.ownedMountainHex, _hexGrid.mountainHex);
            _hexPrefabsDict.Add(_hexGrid.ownedCityPrefab, _hexGrid.cityPrefab);
            
            _hexTypeDict.Add(Hex.Hex.HexType.Basic, _hexGrid.basicHex);
            _hexTypeDict.Add(Hex.Hex.HexType.Forest, _hexGrid.forestHex);
            _hexTypeDict.Add(Hex.Hex.HexType.Mountain, _hexGrid.mountainHex);
            _hexTypeDict.Add(Hex.Hex.HexType.Building, _hexGrid.cityPrefab);
            //ChangeViews();
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

        private void ChangeViews()
        {
            foreach (var kv in _hexGrid.GetHexObjectDictionary().ToList())
            {
                if (kv.Key.GetOwnerID() != _currentPlayer && kv.Key.GetOwnerID() != 0)
                {
                    if (kv.Key.GetHexType() == Hex.Hex.HexType.Air) continue;
                    Debug.Log("Changing hex of type: " + kv.Key.GetHexType());
                    hexPrefab = _hexTypeDict[kv.Key.GetHexType()];
                    Vector3 oldPosition = kv.Key.WorldPosition;
                    Quaternion oldRotation = kv.Value.transform.rotation;
                    
                    _hexGrid.GetGameObjectList().Remove(kv.Value);
                    GameObject newHexObject = Instantiate(hexPrefab,
                        oldPosition,
                        oldRotation,
                        _hexGrid.transform);
                    _hexGrid.GetGameObjectList().Add(newHexObject);
                    GameObject oldGameObject = kv.Value;
                    _hexGrid.GetHexObjectDictionary()[kv.Key] = newHexObject;
                    Debug.Log(oldGameObject.name);
                    Destroy(oldGameObject);
                }
                // handle owned
            }
            
        }
        
        public void ProcessEndTurn()
        {
            _endTurnEmitter.Play();
            //HealCity(); 
            ResetUnitMovementPoints();
            //ChangeViews();
            AdvancePlayer();
            _hexTypeSelector.ResetPlacementCount();
            CheckForWin();
        }
    }
}