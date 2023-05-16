using System;
using Hex;
using MapObjects;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class UnitProductionSelector : MonoBehaviour
    {
        private static GameObject _meleeButton;
        private static GameObject _rangedButton;
        private static GameObject _airshipButton;
        private static GameObject _settlerButton;
        
        private HexGrid _hexGrid;
        private Spawner _spawner;
    
        private void Start()
        {
            _hexGrid = FindObjectOfType<HexGrid>();
            _spawner = FindObjectOfType<Spawner>();
        }

        public static void AssignButtons(Transform canvas)
        {
            _meleeButton = canvas.Find("Melee").gameObject;
            _rangedButton = canvas.Find("Ranged").gameObject;
            _airshipButton = canvas.Find("Airship").gameObject;
            _settlerButton = canvas.Find("Settler").gameObject;
        }
        
        public void SetUnitTypeMeleeButton() 
        {
            _spawner.unit = _spawner.meleeUnit;
            _rangedButton.GetComponent<Button>().interactable = false;
            _airshipButton.GetComponent<Button>().interactable = false;
            _settlerButton.GetComponent<Button>().interactable = false;
            _spawner.unitTypeSelected = true;
            _spawner.AfterButtonClick();
        }
        
        public void SetUnitTypeRangedButton() 
        {
            _spawner.unit = _spawner.rangedUnit;
            _meleeButton.GetComponent<Button>().interactable = false;
            _airshipButton.GetComponent<Button>().interactable = false;
            _settlerButton.GetComponent<Button>().interactable = false;
            _spawner.unitTypeSelected = true;
            _spawner.AfterButtonClick();
        }
        
        public void SetUnitTypeAirshipButton() 
        {
            _spawner.unit = _spawner.airshipUnit;
            _rangedButton.GetComponent<Button>().interactable = false;
            _meleeButton.GetComponent<Button>().interactable = false;
            _settlerButton.GetComponent<Button>().interactable = false;
            _spawner.unitTypeSelected = true;
            _spawner.AfterButtonClick();
        }
        
        public void SetUnitTypeSettlerButton() 
        {
            _spawner.unit = _spawner.settlerUnit;
            _rangedButton.GetComponent<Button>().interactable = false;
            _airshipButton.GetComponent<Button>().interactable = false;
            _meleeButton.GetComponent<Button>().interactable = false;
            _spawner.unitTypeSelected = true;
            _spawner.AfterButtonClick();
        }

        
        public void ResetOnlyButtons()
        {
            _spawner.unit = _spawner.NONE;
            _meleeButton.GetComponent<Button>().interactable = true;
            _rangedButton.GetComponent<Button>().interactable = true;
            _airshipButton.GetComponent<Button>().interactable = true;
            _settlerButton.GetComponent<Button>().interactable = true;
            _spawner.unitTypeSelected = false;
        }
        
        public void ResetButtons()
        {
            foreach (City city in _hexGrid.GetCityList())
            {
                
                GameObject obj = _hexGrid.GetHexObjectDictionary()[city.GetCityCenter()];
                AssignButtons(obj.transform.GetChild(0));
                
                _spawner.unit = _spawner.NONE;
                _meleeButton.GetComponent<Button>().interactable = true;
                _rangedButton.GetComponent<Button>().interactable = true;
                _airshipButton.GetComponent<Button>().interactable = true;
                _settlerButton.GetComponent<Button>().interactable = true;
                _spawner.unitTypeSelected = false;
            }
            
            foreach (City city in _hexGrid.GetCityList())
            {
                city.CanSpawnThisTurn = true;
            }
        }
        
    }
}