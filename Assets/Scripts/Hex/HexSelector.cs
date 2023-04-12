using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary> ************************************************************
/// This is meant to be called by a button to change the type of the Hex
/// to be placed by HexPlacer.
/// </summary> ************************************************************
public class HexSelector : MonoBehaviour
{
    [SerializeField] private GameObject NONE;
    [SerializeField] private HexGrid _hexGrid;
    [SerializeField] private HexPlacer _hexPlacer;
    [SerializeField] private GameObject _basicButton;
    [SerializeField] private GameObject _forestButton;
    [SerializeField] private GameObject _mountainButton;
    
    /// <summary> ***********************************************
    /// This function is called on button click and will allow
    /// the player to select to place basic hex type
    /// </summary> **********************************************
    public void SetHexTypeBasicButton() 
    {
        _hexPlacer.hexPrefab = _hexGrid.basicHex;
        _forestButton.GetComponent<Button>().interactable = false;
        _mountainButton.GetComponent<Button>().interactable = false;
    }
    /// <summary> ***********************************************
    /// This function is called on button click and will allow
    /// the player to select to place forest hex type
    /// </summary> **********************************************
    public void SetHexTypeForestButton() 
    {
        _hexPlacer.hexPrefab = _hexGrid.forestHex;
        _basicButton.GetComponent<Button>().interactable = false;
        _mountainButton.GetComponent<Button>().interactable = false;
    }
    
    /// <summary> ***********************************************
    /// This function is called on button click and will allow
    /// the player to select to place mountain hex type
    /// </summary> **********************************************
    public void SetHexTypeMountainButton() 
    {
        _hexPlacer.hexPrefab = _hexGrid.mountainHex;
        _basicButton.GetComponent<Button>().interactable = false;
        _forestButton.GetComponent<Button>().interactable = false;
    }
    
    public void ResetPlacementCount()
    {
        _hexPlacer.placementCount = 0;
        _hexPlacer.hexPrefab = NONE;
        _basicButton.GetComponent<Button>().interactable = true;
        _forestButton.GetComponent<Button>().interactable = true;
        _mountainButton.GetComponent<Button>().interactable = true;
    }
}