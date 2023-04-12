using UnityEngine;

/// <summary>
/// this will convert an air tile to a land tile. It can 
/// </summary>
public class HexPlacer : MonoBehaviour
{
    [SerializeField] private HexGrid _hexGrid;
    [SerializeField] public GameObject hexPrefab; // this will be changed depending on button selected
    
    private void Update()
    {
        DetectClick();
    }

    /// <summary> ***********************************************
    /// This function will detect if a Hex is clicked
    /// </summary> **********************************************
    private void DetectClick()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out hit)) return;
        
        SetHexType();
        int hexIndex = GetHexIndexAtWorldPos(hit.transform.position);
        if (hexIndex != -1) ConvertHex(hexIndex);
    }

    /// <summary> ***********************************************
    /// This function takes in a Vector3 of world coordinates and
    /// returns the Hex at that position.
    /// </summary> **********************************************
    private int GetHexIndexAtWorldPos(Vector3 coordinates)
    {
        int hexIndex = -1;
        for (int i = 0; i < _hexGrid.GetHexList().Count; i++)
        {
            if (_hexGrid.GetHexList()[i].Position() != coordinates) continue;
            hexIndex = i;
            break;
        }
        return hexIndex;
    }
    
    /// <summary> ***********************************************
    /// This function takes in a Vector3 of grid coordinates and
    /// returns the Hex at that position.
    /// </summary> **********************************************
    private int GetHexIndexAtGridPos(Vector3 coordinates)
    {
        int hexIndex = -1;
        for (int i = 0; i < _hexGrid.GetHexList().Count; i++)
        {
            if (_hexGrid.GetHexList()[i].GetVectorCoordinates() == coordinates)
            {
                hexIndex = i;
                break;
            }
        }
        return hexIndex;
    }

    /// <summary> ***********************************************
    /// This function takes the list index of a Hex and if the
    /// Hex at that position is an air tile, it will be replaced
    /// with the selected Hex type
    /// </summary> **********************************************
    private void ConvertHex(int hexIndex)
    {
        Hex selectedHex = _hexGrid.GetHexList()[hexIndex];
        GameObject hexObject = _hexGrid.GetGameObjectList()[hexIndex];
        if (selectedHex.GetHexType() != Hex.HexType.Air) return;
        if (!CheckForNeighbors(selectedHex)) return;
        
        Destroy(hexObject);

        GameObject newHex = Instantiate(hexPrefab,
            selectedHex.Position(),
            Quaternion.identity,
            this.transform);
        newHex.transform.Rotate(0f, Random.Range(0, 7) * 60, 0f, Space.Self);
        hexObject = newHex;
        selectedHex.SetHexType(hexPrefab.name);
    }

    /// <summary> ***********************************************
    /// This function checks every neighbor to see if there is an
    /// adjacent land hex and returns true if there is false if
    /// not.
    /// </summary> **********************************************
    private bool CheckForNeighbors(Hex selectedHex)
    {
        Vector3 hexCoordinates = _hexGrid.AddCoordinates(selectedHex.GetVectorCoordinates(),
            _hexGrid.CoordinateScale(_hexGrid.GetDirectionVector()[4], 1));
        int landCount = 0;
        for (int i = 0; i < 6; i++)
        {
            for(int j = 0; j < 1; j++)
            {
                int hexIndex = GetHexIndexAtGridPos(hexCoordinates);
                if (_hexGrid.GetHexList()[hexIndex].isLand())
                {
                    landCount++;
                    break;
                }
                
                hexCoordinates = _hexGrid.HexNeighbor(hexCoordinates, i);
            }
        }
        
        return landCount > 0;
    }

    /// <summary> ***********************************************
    /// This function will set the hex type based on the player's
    /// selection.
    /// </summary> **********************************************
    private void SetHexType()
    {
        hexPrefab = _hexGrid.basicHex;
    }
}
    
