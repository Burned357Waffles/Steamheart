using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// code shows up around 8:35
[SelectionBase]
public class Hex : MonoBehaviour
{
    private HexCoordinates hexCoordinates;

    public Vector3Int HexCoords => hexCoordinates.GetHexCoords();

    private void Awake() 
    {
        hexCoordinates = GetComponent<HexCoordinates>();
    }
}
