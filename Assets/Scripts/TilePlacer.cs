using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
                Hex hexClicked = GetHexAt(hit.transform.position);
                ConvertHex(hexClicked);
                Debug.Log("Hex at: " + hexClicked.Q + ", " + hexClicked.R);
            }
        }
    }

    /// <summary> ***********************************************
    /// This function takes in a Vector3 of world coordinates and
    /// returns the Hex at that position.
    /// </summary> **********************************************
    private Hex GetHexAt(Vector3 coordinates)
    {
        Hex hexClicked = null;
        foreach (Hex hex in _hexGrid.GetHexList())
        {
            if (hex.Position() == coordinates)
            {
                hexClicked = hex;
                break;
            }
        }
        return hexClicked;
    }

    private void ConvertHex(Hex hex)
    {
        if (hex.GetHexType() == Hex.HexType.Air)
        {
            Debug.Log(_hexGrid.basicHex.name);
            hex.SetHexType("basic_tile");
        }
    }
}
    
