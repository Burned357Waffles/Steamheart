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
            _meleeButton = canvas.GetChild(0).gameObject;
            _rangedButton = canvas.GetChild(1).gameObject;
            _airshipButton = canvas.GetChild(2).gameObject;
            _settlerButton = canvas.GetChild(3).gameObject;
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
            _spawner.unit = _spawner.NONE;
            _meleeButton.GetComponent<Button>().interactable = true;
            _rangedButton.GetComponent<Button>().interactable = true;
            _airshipButton.GetComponent<Button>().interactable = true;
            _settlerButton.GetComponent<Button>().interactable = true;
            _spawner.unitTypeSelected = false;
            foreach (City city in _hexGrid.GetCityList())
            {
                city.CanSpawnThisTurn = true;
            }
        }
        
    }
}