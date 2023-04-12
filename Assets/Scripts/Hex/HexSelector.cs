using UnityEngine;
public class HexSelector : MonoBehaviour
{
    [SerializeField] private HexGrid _hexGrid;
    [SerializeField] private HexPlacer _hexPlacer;
    
    /// <summary> ***********************************************
    /// This function is called on button click and will allow
    /// the player to select to place basic hex type
    /// </summary> **********************************************
    public void SetHexTypeBasicButton() 
    {
        Debug.Log("BASIC");
        _hexPlacer.hexPrefab = _hexGrid.basicHex;
    }
    /// <summary> ***********************************************
    /// This function is called on button click and will allow
    /// the player to select to place forest hex type
    /// </summary> **********************************************
    public void SetHexTypeForestButton() 
    {
        Debug.Log("FOREST");
        _hexPlacer.hexPrefab = _hexGrid.forestHex;
    }
    
    /// <summary> ***********************************************
    /// This function is called on button click and will allow
    /// the player to select to place mountain hex type
    /// </summary> **********************************************
    public void SetHexTypeMountainButton() 
    {
        Debug.Log("MOUNTAIN");
        _hexPlacer.hexPrefab = _hexGrid.mountainHex;
    }
}