using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary> ************************************************************
/// This will detect if a grid position is clicked and will convert an air
/// tile to a land tile. Currently, it is constantly listening for click
/// on Update(), but it will eventually only start listening when tile
/// selection is active.
/// </summary> ***********************************************************
public class HexPlacer : MonoBehaviour
{
    [SerializeField] private HexGrid hexGrid;
    [SerializeField] public GameObject hexPrefab; // this will be changed depending on button selected
    
    public int placementCount = 0;

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
        
        Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit)) return;
        if (hexPrefab == null) return; // if button is not chosen
        if(!PlacementCount()) return;
        
        int hexIndex = GetHexIndexAtWorldPos(hit.transform.position);
        if (hexIndex != -1) ConvertHex(hexIndex);
    }

    /// <summary> ***********************************************
    /// This function takes in a Vector3 of world coordinates
    /// and returns the Hex at that position.
    /// </summary> **********************************************
    private int GetHexIndexAtWorldPos(Vector3 coordinates)
    {
        int hexIndex = -1;
        for (int i = 0; i < hexGrid.GetHexList().Count; i++)
        {
            if (hexGrid.GetHexList()[i].Position() != coordinates) continue;
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
        for (int i = 0; i < hexGrid.GetHexList().Count; i++)
        {
            if (hexGrid.GetHexList()[i].GetVectorCoordinates() == coordinates)
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
        Hex selectedHex = hexGrid.GetHexList()[hexIndex];
        GameObject hexObject = hexGrid.GetGameObjectList()[hexIndex];
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
    /// TODO: check for ownership 
    private bool CheckForNeighbors(Hex selectedHex)
    {
        Vector3 hexCoordinates = HexGrid.AddCoordinates(selectedHex.GetVectorCoordinates(),
            HexGrid.CoordinateScale(hexGrid.GetDirectionVector()[4], 1));
        int landCount = 0;
        for (int i = 0; i < 6; i++)
        {
            for(int j = 0; j < 1; j++)
            {
                int hexIndex = GetHexIndexAtGridPos(hexCoordinates);
                if (hexGrid.GetHexList()[hexIndex].isLand())
                {
                    landCount++;
                    break;
                }
                
                hexCoordinates = hexGrid.HexNeighbor(hexCoordinates, i);
            }
        }
        
        return landCount > 0;
    }

    /// <summary> ***********************************************
    /// This function checks if the placement count has reached
    /// the max value for each hex type. If it has not reached
    /// max, then increment and return true to indicate that new
    /// hex can be placed. If has reached max count, then return
    /// false indicating that a new hex may not be placed.
    /// </summary> **********************************************
    private bool PlacementCount()
    {
        if (hexPrefab == hexGrid.basicHex && placementCount < 2)
        {
            placementCount++;
            return true;
        }
        else if (hexPrefab == hexGrid.forestHex || hexPrefab == hexGrid.mountainHex)
            if (placementCount < 1)
            {
                placementCount++;
                return true;
            }
        return false;
    }


}
    
