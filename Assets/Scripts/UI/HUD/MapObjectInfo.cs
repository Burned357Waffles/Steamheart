using TMPro;
using UnityEngine;
using MapObjects;

namespace UI.HUD
{
    public class MapObjectInfo : MonoBehaviour
    {
        [SerializeField] public GameObject infoPanel;
        [SerializeField] public GameObject movementLabel;
        [SerializeField] public GameObject resourcesLabel;
        [SerializeField] public GameObject owner;
        [SerializeField] public GameObject type;
        [SerializeField] public GameObject health;
        [SerializeField] public GameObject damage;
        [SerializeField] public GameObject range;
        [SerializeField] public GameObject movement;
        [SerializeField] public GameObject resources;
        
        private TMP_Text _ownerText;
        private TMP_Text _typeText;
        private TMP_Text _healthText;
        private TMP_Text _damageText;
        private TMP_Text _rangeText;
        private TMP_Text _movementText;
        private TMP_Text _resourcesText;

        private void Start()
        {
            _ownerText = owner.GetComponent<TMP_Text>();
            _typeText = type.GetComponent<TMP_Text>();
            _healthText = health.GetComponent<TMP_Text>();
            _damageText = damage.GetComponent<TMP_Text>();
            _rangeText = range.GetComponent<TMP_Text>();
            _movementText = movement.GetComponent<TMP_Text>();
            _resourcesText = resources.GetComponent<TMP_Text>();
            infoPanel.SetActive(false);
        }

        public void DisplayInfo(Unit unit)
        {
            infoPanel.SetActive(true);
            _ownerText.text = unit.GetOwnerID().ToString();
            _typeText.text = unit.GetUnitType().ToString();
            _healthText.text = unit.Health.ToString();
            _damageText.text = unit.Damage.ToString();
            _rangeText.text = unit.AttackRadius.ToString();
            _movementText.text = unit.GetCurrentMovementPoints() + " / " + unit.BaseMovementPoints;
            movementLabel.SetActive(true);
            resourcesLabel.SetActive(false);
        }
        
        public void DisplayInfo(City city)
        {
            infoPanel.SetActive(true);
            _ownerText.text = city.GetOwnerID().ToString();
            _typeText.text = "City";
            _healthText.text = city.Health.ToString();
            _damageText.text = city.Damage.ToString();
            _rangeText.text = city.AttackRadius.ToString();
            _resourcesText.text = "+" + city.IronCount + " | " + "+" + city.WoodCount;
            resourcesLabel.SetActive(true);
            movementLabel.SetActive(false);
        }

        public void DisableInfoPanel() { infoPanel.SetActive(false); }
    }
}