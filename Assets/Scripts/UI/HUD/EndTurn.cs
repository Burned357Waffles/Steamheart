using System;
using System.Collections.Generic;
using System.Linq;
using Hex;
using MapObjects;
using Misc;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using QualitySettings = Settings.QualitySettings;

namespace UI.HUD
{
    public class EndTurn : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI playerIndicator;
        [SerializeField] public UnityEngine.UI.Image player1Icon;
        [SerializeField] public UnityEngine.UI.Image player2Icon;
        [SerializeField] public GameObject endTurnButton;
        [SerializeField] public GameObject infoPanel;
        [SerializeField] public GameObject hexPrefab;
        [SerializeField] public GameObject sunLight;
        [SerializeField] public GameObject constantSunLight;

        private HexGrid _hexGrid;
        private HexPlacer _hexPlacer;
        private HexTypeSelector _hexTypeSelector;
        private Spawner _spawner;
        private ResourceCounter _resourceCounter;
        private MapObjectInfo _objectInfo;
        private int _currentPlayer;
        private UnitMovement _unitMovement;
        private FMODUnity.StudioEventEmitter _endTurnEmitter;
        private MoveCamera _cameraRig;
        private Light _light;
        private int _currentSunIndex;
        private bool _dayNightOn;

        private bool _shouldLerp;
        private float _timeStarted;
        private float _lerpTime;
        private Vector3 _startPos;
        private Vector3 _endPos;
        private Color _startColor;
        private Color _endColor;

        private Dictionary<GameObject, GameObject> _hexPrefabsDict = new Dictionary<GameObject, GameObject>();
        private Dictionary<Hex.Hex.HexType, GameObject> _hexTypeDict = new Dictionary<Hex.Hex.HexType, GameObject>();

        // angle : color
        private Dictionary<Vector3, Color> _sunSettings = new Dictionary<Vector3, Color>();

        private FMODUnity.StudioEventEmitter _bgmEmitter;

        public void InitEndTurn()
        {
            _dayNightOn = true;
            if (QualitySettings.GetDayNightQuality() == 1) _dayNightOn = false; 
            _currentPlayer = 1;
            _hexGrid = FindObjectOfType<HexGrid>();
            _hexPlacer = FindObjectOfType<HexPlacer>();
            _hexTypeSelector = FindObjectOfType<HexTypeSelector>();
            _spawner = FindObjectOfType<Spawner>();
            _unitMovement = FindObjectOfType<UnitMovement>();
            _cameraRig = FindObjectOfType<MoveCamera>();
            _resourceCounter = FindObjectOfType<ResourceCounter>();
            _objectInfo = infoPanel.GetComponent<MapObjectInfo>();
            _light = sunLight.GetComponent<Light>();
            _endTurnEmitter = endTurnButton.GetComponent<FMODUnity.StudioEventEmitter>();
            _bgmEmitter = GameObject.Find("BGM").GetComponent<FMODUnity.StudioEventEmitter>();
            playerIndicator.text = _currentPlayer.ToString();

            Color player1IconColor = player1Icon.color;
            Color player2IconColor = player2Icon.color;
            // make P1's icon visible and P2's icon transparent
            player1IconColor.a = 1f;
            player1Icon.color = player1IconColor;
            player2IconColor.a = 0f;
            player2Icon.color = player2IconColor;

            _lerpTime = 1f;

            _hexPrefabsDict.Add(_hexGrid.ownedBasicHex, _hexGrid.basicHex);
            _hexPrefabsDict.Add(_hexGrid.ownedForestHex, _hexGrid.forestHex);
            _hexPrefabsDict.Add(_hexGrid.ownedMountainHex, _hexGrid.mountainHex);
            _hexPrefabsDict.Add(_hexGrid.ownedCityPrefab, _hexGrid.ownedCityPrefab);
            
            _hexTypeDict.Add(Hex.Hex.HexType.Basic, _hexGrid.basicHex);
            _hexTypeDict.Add(Hex.Hex.HexType.Forest, _hexGrid.forestHex);
            _hexTypeDict.Add(Hex.Hex.HexType.Mountain, _hexGrid.mountainHex);
            _hexTypeDict.Add(Hex.Hex.HexType.Building, _hexGrid.ownedCityPrefab);
            
            //ChangeViews();
            InitColors();
            _currentSunIndex = -1;
            if (_dayNightOn) CycleTime();
            else
            {
                sunLight.SetActive(false);
                constantSunLight.SetActive(true);
            }
            AccumulateMaterials();
            CenterCameraToPlayerCapital();
        }

        private void Update()
        {
            if (!_shouldLerp) return;
            
            _light.color = LerpColor(_startColor, _endColor, _timeStarted, _lerpTime);
            _light.intensity = 0.004f;
            Vector3 rot = LerpRotation(_startPos, _endPos, _timeStarted, _lerpTime);
            sunLight.transform.localRotation = Quaternion.Euler(rot);
            
        }

        private void InitColors()
        {
            // sunrise
            _sunSettings.Add(new Vector3(20, 145, 0), new Color(255, 222, 221, 255));
            // morning
            _sunSettings.Add(new Vector3(40, 160, 0), new Color(255, 235, 221, 255));
            // noon
            _sunSettings.Add(new Vector3(60, 175, 0), new Color(255, 244, 214, 255));
            // afternoon
            _sunSettings.Add(new Vector3(40, 205, 0), new Color(222, 197, 166, 255));
            // sunset
            _sunSettings.Add(new Vector3(20, 220, 0), new Color(214, 90, 60, 255));
            // midnight
            _sunSettings.Add(new Vector3(40, 190, 0), new Color(53, 46, 108, 255));
        }

        private Vector3 LerpRotation(Vector3 start, Vector3 end, float timeStarted, float lerpTime = 1)
        {
            float timeSinceStart = Time.time - timeStarted;
            return Vector3.Lerp(start, end, timeSinceStart / lerpTime);
        }
        
        private Color LerpColor(Color start, Color end, float timeStarted, float lerpTime = 1)
        {
            float timeSinceStart = Time.time - timeStarted;
            return Color.Lerp(start, end, timeSinceStart / lerpTime);
        }
        
        private void CycleTime()
        {
            if (_currentPlayer != 1) return;
                
            int i = 0;
            //KeyValuePair<Vector3, Color> currentKV = new KeyValuePair<Vector3, Color>();
            foreach (var kv in _sunSettings)
            {
                //if (_currentSunIndex == i) currentKV = kv;
                Debug.Log("csi: " + _currentSunIndex + " i: " + i);
                if (_currentSunIndex + 1 == i)
                {
                    _timeStarted = Time.time;
                    _shouldLerp = true;
                    
                    _startPos = sunLight.transform.localRotation.eulerAngles;;
                    _endPos = kv.Key;
                    _startColor = _light.color;
                    _endColor = kv.Value;
                    
                    _currentSunIndex = i;
                    if (_currentSunIndex >= _sunSettings.Count - 1) _currentSunIndex = -1;
                    return;
                }
                i++;
            }
        }

        private void HealCity()
        {
            foreach (var city in _hexGrid.GetCityList().Where(city => city.GetOwnerID() == _currentPlayer))
            {
                if (city.Health >= 25) continue;
                int add = 25 - (city.Health % 25);
                if (add > 4) add = 4;
                city.Health += add;
            }
        }

        private void AdvancePlayer()
        {
            Debug.Log("Player before advance: " + _currentPlayer);
            bool searching = true;
            while (searching)
            {
                _currentPlayer++;
                if (_currentPlayer > _hexGrid.GetPlayerList().Last().GetPlayerID()) _currentPlayer = 1;
                foreach (Player player in _hexGrid.GetPlayerList())
                {
                    Debug.Log("looping: " + _currentPlayer);
                    if (player.GetPlayerID() == _currentPlayer)
                    {
                        searching = false;
                        break;
                    }
                } 
            }
            Color player1IconColor = player1Icon.color;
            Color player2IconColor = player2Icon.color;

            Debug.Log("Player after advance: " + _currentPlayer);
            _hexPlacer.SetPlayer(_currentPlayer);
            _unitMovement.SetPlayer(_currentPlayer);
            _spawner.SetPlayer(_currentPlayer);
            Debug.Log("Player count = " + _hexGrid.GetPlayerList().Count);
            playerIndicator.text = _currentPlayer.ToString();

            if (_currentPlayer == 1)
            {
                // make P1's icon visible and P2's icon transparent
                player1IconColor.a = 1f;
                player1Icon.color = player1IconColor;
                player2IconColor.a = 0f;
                player2Icon.color = player2IconColor;

            }

            else if (_currentPlayer == 2)
            {
                // make P2's icon visible and P1's icon transparent
                player1IconColor.a = 0f;
                player1Icon.color = player1IconColor;
                player2IconColor.a = 1f;
                player2Icon.color = player2IconColor;
            }
            
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

        public void AccumulateMaterials()
        {
            Player player = _hexGrid.FindPlayerOfID(_currentPlayer);
            int totalMountainCount = 0;
            int totalForestCount = 0;
            foreach (City city in player.GetOwnedCities())
            {
                int mountainCount = 0;
                int forestCount = 0;
                foreach (Hex.Hex hex in city.GetCityHexes())
                {
                    if (hex.GetHexType() == Hex.Hex.HexType.Mountain)
                    {
                        player.TotalIronCount++;
                        mountainCount++;
                    }
                    else if (hex.GetHexType() == Hex.Hex.HexType.Forest)
                    {
                        player.TotalWoodCount++;
                        forestCount++;
                    }
                }

                totalMountainCount += mountainCount;
                totalForestCount += forestCount;
                city.IronCount = mountainCount;
                city.WoodCount = forestCount;
            }

            player.IronCountPerTurn = totalMountainCount;
            player.WoodCountPerTurn = totalForestCount;
            
            _resourceCounter.Init();
            _resourceCounter.UpdateResourceCounts(player);
        }

        private void ResetCityUI()
        {
            foreach (City city in _hexGrid.GetCityList())
            {
                
                GameObject obj = _hexGrid.GetHexObjectDictionary()[city.GetCityCenter()];
                var border = obj.transform.GetChild(1);
                border.gameObject.SetActive(false);
                Transform unitSelectorPanel = obj.transform.GetChild(0);
                if (unitSelectorPanel != null) 
                    unitSelectorPanel.gameObject.SetActive(false);
            }
            
        }
        
        private void CenterCameraToPlayerCapital()
        {
            // TODO: if player has no capitol left, zoom in on next owned city
            Debug.Log("Centering on player: " + _currentPlayer);
            Player player = _hexGrid.FindPlayerOfID(_currentPlayer);
            Vector3 capitalPosition = new Vector3(-1, -1, -1);
            
            foreach (var city in player.GetOwnedCities().Where(city => city.IsCapitol()))
            {
                capitalPosition = city.GetCityCenter().WorldPosition;
            }
            
            // if capitol not found
            if (capitalPosition == new Vector3(-1, -1, -1))
            {
                capitalPosition = player.GetOwnedCities()[0].GetCityCenter().WorldPosition;
            }
            _cameraRig.CenterCamera(capitalPosition);
        }
        
        public void ProcessEndTurn()
        {
            _endTurnEmitter.Play();
            HealCity(); 
            ResetCityUI();
            ResetUnitMovementPoints();
            //ChangeViews();
            AdvancePlayer();
            if (_dayNightOn) CycleTime();
            CenterCameraToPlayerCapital();
            _bgmEmitter.SetParameter("City Health", _hexGrid.GetPlayerList()[_currentPlayer - 1].GetMinAliveCityHealth());
            AccumulateMaterials();
            _hexTypeSelector.ResetPlacementCount();
            _objectInfo.DisableInfoPanel();
            CheckForWin();
        }

        
    }
}