﻿using Hex;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    /// <summary> ************************************************************
    /// This is meant to be called by a button to change the type of the Hex
    /// to be placed by HexPlacer.
    /// </summary> ************************************************************
    public class HexTypeSelector : MonoBehaviour
    {
        [SerializeField] private GameObject NONE;
        [SerializeField] private GameObject basicButton;
        [SerializeField] private GameObject forestButton;
        [SerializeField] private GameObject mountainButton;

        private HexGrid _hexGrid;
        private HexPlacer _hexPlacer;
    
        private void Start()
        {
            _hexGrid = FindObjectOfType<HexGrid>();
            _hexPlacer = FindObjectOfType<HexPlacer>();
        }

        /// <summary> ***********************************************
        /// This function is called on button click and will allow
        /// the player to select to place basic hex type.
        /// </summary> **********************************************
        public void SetHexTypeBasicButton() 
        {
            _hexPlacer.hexPrefab = _hexGrid.ownedBasicHex;
            forestButton.GetComponent<Button>().interactable = false;
            mountainButton.GetComponent<Button>().interactable = false;
        }
        /// <summary> ***********************************************
        /// This function is called on button click and will allow
        /// the player to select to place forest hex type.
        /// </summary> **********************************************
        public void SetHexTypeForestButton()
        {
            _hexPlacer.hexPrefab = _hexGrid.ownedForestHex;
            basicButton.GetComponent<Button>().interactable = false;
            mountainButton.GetComponent<Button>().interactable = false;
        }
    
        /// <summary> ***********************************************
        /// This function is called on button click and will allow
        /// the player to select to place mountain hex type.
        /// </summary> **********************************************
        public void SetHexTypeMountainButton() 
        {
            _hexPlacer.hexPrefab = _hexGrid.ownedMountainHex;
            basicButton.GetComponent<Button>().interactable = false;
            forestButton.GetComponent<Button>().interactable = false;
        }
    
        /// <summary> ***********************************************
        /// This function is called on the End Turn button. It resets
        /// the buttons, placement count, and hex prefab type. It also
        /// advances the turn to the next player
        /// </summary> ***********************************************
        public void ResetPlacementCount()
        {
            Debug.Log("resetting buttons");
            _hexPlacer.placementCount = 0;
            _hexPlacer.hexPrefab = NONE;
            basicButton.GetComponent<Button>().interactable = true;
            forestButton.GetComponent<Button>().interactable = true;
            mountainButton.GetComponent<Button>().interactable = true;
            Debug.Log("reset buttons");
        }
    }
}