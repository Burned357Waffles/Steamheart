using UnityEngine;
using UnityEngine.UI;

/// <summary> ************************************************************
/// This is meant to be called by a button to change the type of the Hex
/// to be placed by HexPlacer.
/// </summary> ************************************************************
public class HexSelector : MonoBehaviour
{
    [SerializeField] private GameObject NONE;
    [SerializeField] private GameObject basicButton;
    [SerializeField] private GameObject forestButton;
    [SerializeField] private GameObject mountainButton;

    private HexGrid _hexGrid;
    private HexPlacer _hexPlacer;
    private void Start()
    {
        _hexGrid = GameObject.FindObjectOfType<HexGrid>();
        _hexPlacer = GameObject.FindObjectOfType<HexPlacer>();
    }

    /// <summary> ***********************************************
    /// This function is called on button click and will allow
    /// the player to select to place basic hex type
    /// </summary> **********************************************
    public void SetHexTypeBasicButton() 
    {
        _hexPlacer.hexPrefab = _hexGrid.basicHex;
        forestButton.GetComponent<Button>().interactable = false;
        mountainButton.GetComponent<Button>().interactable = false;
    }
    /// <summary> ***********************************************
    /// This function is called on button click and will allow
    /// the player to select to place forest hex type
    /// </summary> **********************************************
    public void SetHexTypeForestButton() 
    {
        _hexPlacer.hexPrefab = _hexGrid.forestHex;
        basicButton.GetComponent<Button>().interactable = false;
        mountainButton.GetComponent<Button>().interactable = false;
    }
    
    /// <summary> ***********************************************
    /// This function is called on button click and will allow
    /// the player to select to place mountain hex type
    /// </summary> **********************************************
    public void SetHexTypeMountainButton() 
    {
        _hexPlacer.hexPrefab = _hexGrid.mountainHex;
        basicButton.GetComponent<Button>().interactable = false;
        forestButton.GetComponent<Button>().interactable = false;
    }
    
    public void ResetPlacementCount()
    {
        _hexPlacer.placementCount = 0;
        _hexPlacer.hexPrefab = NONE;
        basicButton.GetComponent<Button>().interactable = true;
        forestButton.GetComponent<Button>().interactable = true;
        mountainButton.GetComponent<Button>().interactable = true;
    }
}