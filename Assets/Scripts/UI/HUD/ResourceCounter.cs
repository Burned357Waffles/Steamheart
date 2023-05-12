using Hex;
using Misc;
using TMPro;
using UnityEngine;

namespace UI.HUD
{
    public class ResourceCounter : MonoBehaviour
    {
        [SerializeField] public GameObject ironCounter;
        [SerializeField] public GameObject woodCounter;

        private HexGrid _hexGrid;
        private TMP_Text _ironNumber;
        private TMP_Text _woodNumber;

        public void Init()
        {
            _hexGrid = FindObjectOfType<HexGrid>();
            _ironNumber = ironCounter.GetComponent<TMP_Text>();
            _ironNumber.text = "0";
            _woodNumber = woodCounter.GetComponent<TMP_Text>();
            _woodNumber.text = "0";
        }

        public void UpdateResourceCounts(Player player)
        {
            Debug.Log("this player: " + player.GetPlayerID());
            _ironNumber.text = player.TotalIronCount.ToString();
            _woodNumber.text = player.TotalWoodCount.ToString();
        }
    }
}