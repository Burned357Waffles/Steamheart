using System;
using TMPro;

using UnityEngine;

namespace Misc
{
    public class PlayerCount : MonoBehaviour
    {
        [SerializeField] public GameObject menu;
        private static TMP_Dropdown _dropdown;

        private void Start()
        {
            _dropdown = menu.GetComponent<TMP_Dropdown>();
        }

        private static int _playerCount = 2;

        /// <summary> ***********************************************
        /// This function is called by the selector in the new game
        /// menu.
        /// </summary> **********************************************
        public static void SetPlayerCount()
        {
            _playerCount = _dropdown.value + 2;
            Debug.Log("Setting player count to: " + _playerCount);
        }
        public static int GetPlayerCount() { return _playerCount;}
    }
}