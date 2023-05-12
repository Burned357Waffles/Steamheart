using TMPro;
using UnityEngine;
using MapObjects;

namespace UI.HUD
{
    public class UnitInfo : MonoBehaviour
    {
        [SerializeField] public GameObject unitInfoPanel;
        [SerializeField] public GameObject unitOwner;
        [SerializeField] public GameObject unitType;
        [SerializeField] public GameObject unitHealth;
        [SerializeField] public GameObject unitDamage;
        [SerializeField] public GameObject unitRange;
        [SerializeField] public GameObject unitMovement;
        
        private TMP_Text _ownerText;
        private TMP_Text _typeText;
        private TMP_Text _healthText;
        private TMP_Text _damageText;
        private TMP_Text _rangeText;
        private TMP_Text _movementText;

        private void Start()
        {
            _ownerText = unitOwner.GetComponent<TMP_Text>();
            _typeText = unitType.GetComponent<TMP_Text>();
            _healthText = unitHealth.GetComponent<TMP_Text>();
            _damageText = unitDamage.GetComponent<TMP_Text>();
            _rangeText = unitRange.GetComponent<TMP_Text>();
            _movementText = unitMovement.GetComponent<TMP_Text>();
            unitInfoPanel.SetActive(false);
        }

        public void DisplayUnitInfo(Unit unit)
        {
            unitInfoPanel.SetActive(true);
            _ownerText.text = unit.GetOwnerID().ToString();
            _typeText.text = unit.GetUnitType().ToString();
            _healthText.text = unit.Health.ToString();
            _damageText.text = unit.Damage.ToString();
            _rangeText.text = unit.AttackRadius.ToString();
            _movementText.text = unit.GetCurrentMovementPoints() + " / " + unit.BaseMovementPoints;
        }

        public void DisableUnitInfo() { unitInfoPanel.SetActive(false); }
    }
}