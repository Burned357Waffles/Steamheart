using UnityEngine;
using Object = UnityEngine.Object;

namespace MapObjects
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] public GameObject unit;
        private HexGrid _hexGrid;

        private void Start()
        {
            _hexGrid = Object.FindObjectOfType<HexGrid>();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space)) // this will be replaced soon
            {
                SpawnUnit(0, 0, 1);
            }
        }
        
        /// <summary> ***********************************************
        /// This function will spawn a unit given coordinates and
        /// ownerID of the current player
        /// </summary> ***********************************************
        private void SpawnUnit(int q, int r, int ownerID)
        {
            
            Hex.Hex hex = _hexGrid.GetHexAt(new Vector3(0, 0, 0));
            if (_hexGrid.GetUnitDictionary().ContainsKey(hex)) return;
            Unit newUnit = new Unit(q, r, ownerID);
            Vector3 vectorToPlaceAt = new Vector3(hex.GetVectorCoordinates().x,
                hex.GetVectorCoordinates().y + 1.3f,
                hex.GetVectorCoordinates().z);
            GameObject newUnitObject = Instantiate(unit, vectorToPlaceAt, transform.rotation);
        
            _hexGrid.GetUnitDictionary().Add(hex, newUnit);
            _hexGrid.GetUnitObjectDictionary().Add(newUnit, newUnitObject); 
        }
    }
}  