using Hex;
using MapObjects;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class UnitProductionSelector : MonoBehaviour
    {
        [SerializeField] private GameObject NONE;
        [SerializeField] private GameObject meleeButton;
        [SerializeField] private GameObject rangedButton;
        [SerializeField] private GameObject airshipButton;
        [SerializeField] private GameObject settlerButton;
        
        private Spawner _spawner;
    
        private void Start()
        {
            _spawner = FindObjectOfType<Spawner>();
        }
        
        public void SetUnitTypeMeleeButton() 
        {
            _spawner.unit = _spawner.meleeUnit;
            rangedButton.GetComponent<Button>().interactable = false;
            airshipButton.GetComponent<Button>().interactable = false;
            settlerButton.GetComponent<Button>().interactable = false;
            _spawner.unitTypeSelected = true;
        }
        
        public void SetUnitTypeRangedButton() 
        {
            _spawner.unit = _spawner.rangedUnit;
            meleeButton.GetComponent<Button>().interactable = false;
            airshipButton.GetComponent<Button>().interactable = false;
            settlerButton.GetComponent<Button>().interactable = false;
            _spawner.unitTypeSelected = true;
        }
        
        public void SetUnitTypeAirshipButton() 
        {
            _spawner.unit = _spawner.airshipUnit;
            rangedButton.GetComponent<Button>().interactable = false;
            meleeButton.GetComponent<Button>().interactable = false;
            settlerButton.GetComponent<Button>().interactable = false;
            _spawner.unitTypeSelected = true;
        }
        
        public void SetUnitTypeSettlerButton() 
        {
            _spawner.unit = _spawner.settlerUnit;
            rangedButton.GetComponent<Button>().interactable = false;
            airshipButton.GetComponent<Button>().interactable = false;
            meleeButton.GetComponent<Button>().interactable = false;
            _spawner.unitTypeSelected = true;
        }

        public void ResetButtons()
        {
            _spawner.unit = NONE;
            meleeButton.GetComponent<Button>().interactable = true;
            rangedButton.GetComponent<Button>().interactable = true;
            airshipButton.GetComponent<Button>().interactable = true;
            settlerButton.GetComponent<Button>().interactable = true;
            _spawner.unitTypeSelected = false;
        }
        
    }
}