using System;
using TMPro;

using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Settings
{
    public class MatchSettings : MonoBehaviour
    {
        [SerializeField] public GameObject playerCountMenu;
        [SerializeField] public GameObject mapSizeMenu;
        [SerializeField] public GameObject seedMenu;
        
        private static TMP_Dropdown _playerDropdown;
        private static TMP_Dropdown _sizeDropdown;
        private static TMP_InputField _seedEntry;
        
        private static int _playerCount = 2;
        private static int _mapSize = 16;
        private static int _seed = 963; // 963 for best seed
        

        private static int[] _mapSizes = { 16, 32, 64, 96, 128 };

        private void Start()
        {
            _playerDropdown = playerCountMenu.GetComponent<TMP_Dropdown>();
            _sizeDropdown = mapSizeMenu.GetComponent<TMP_Dropdown>();
            _seedEntry = seedMenu.GetComponent<TMP_InputField>();
            _seed = Random.Range(0, 1000);
        }

        /// <summary> ***********************************************
        /// This function is called by the selector in the new game
        /// menu.
        /// </summary> **********************************************
        public static void SetPlayerCount()
        {
            _playerCount = _playerDropdown.value + 2;
            Debug.Log("Setting player count to: " + _playerCount);
        }
        public static int GetPlayerCount() { return _playerCount;}
        
        /// <summary> ***********************************************
        /// This function is called by the selector in the new game
        /// menu.
        /// </summary> **********************************************
        public static void SetMapSize()
        {
            _mapSize = _mapSizes[_sizeDropdown.value];
            Debug.Log("Setting map size to: " + _mapSize);
        }
        public static int GetMapSize() { return _mapSize; }

        public static void SetSeed()
        {
            _seed = Int32.Parse(_seedEntry.text);
            Debug.Log("Seed set to " + _seed);
        }

        public static int GetSeed()
        {
            Debug.Log("SEED: " + _seed);
            return _seed;
        }
        
    }
}