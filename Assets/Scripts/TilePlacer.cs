using UnityEngine;

/// <summary>
/// this will convert an air tile to a land tile. It can 
/// </summary>
public class TilePlacer : MonoBehaviour
{
    [SerializeField] private HexGrid _hexGrid;
    
    private void Update()
    {
        DetectClick();
    }

    /// <summary> ***********************************************
    /// This function will detect if a Hex is clicked
    /// </summary> **********************************************
    private void DetectClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit))
            {
                int hexIndex = GetHexIndexAt(hit.transform.position);
                ConvertHex(hexIndex);
                Debug.Log("Hex at: " + _hexGrid.GetHexList()[hexIndex].Q + ", " + _hexGrid.GetHexList()[hexIndex].R);
            }
        }
    }

    /// <summary> ***********************************************
    /// This function takes in a Vector3 of world coordinates and
    /// returns the Hex at that position.
    /// </summary> **********************************************
    private int GetHexIndexAt(Vector3 coordinates)
    {
        int hexIndex = -1;
        for (int i = 0; i < _hexGrid.GetHexList().Count; i++)
        {
            if (_hexGrid.GetHexList()[i].Position() == coordinates)
            {
                hexIndex = i;
                break;
            }
        }
        return hexIndex;
    }

    private void ConvertHex(int hexIndex)
    {
        if (_hexGrid.GetHexList()[hexIndex].GetHexType() == Hex.HexType.Air)
        {
            Destroy(_hexGrid.GetGameObjectList()[hexIndex]);
            
            GameObject newHex = Instantiate(_hexGrid.basicHex,
                _hexGrid.GetHexList()[hexIndex].Position(),
                Quaternion.identity,
                this.transform);
            newHex.transform.Rotate(0f, Random.Range(0, 7) * 60, 0f, Space.Self);
            _hexGrid.GetGameObjectList()[hexIndex] = newHex;
            _hexGrid.GetHexList()[hexIndex].SetHexType("basic_tile");
        }
    }
}
    
