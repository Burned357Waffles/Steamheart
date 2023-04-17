using System;
using System.Collections;
using System.Collections.Generic;
using MapObjects;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

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
            SpawnUnit();
            Debug.Log("unit spawned");
        }
    }

    private void SpawnUnit()
    {
        Unit newUnit = new Unit(0, 0);
        Hex hex = _hexGrid.GetHexAt(new Vector3(0, 0, 0));
        Vector3 vectorToPlaceAt = new Vector3(hex.GetVectorCoordinates().x,
                                          hex.GetVectorCoordinates().y + 1.3f,
                                            hex.GetVectorCoordinates().z);
        GameObject newUnitObject = Instantiate(unit, vectorToPlaceAt, transform.rotation);
        
        _hexGrid.GetUnitDictionary().Add(hex, newUnit);
        _hexGrid.GetUnitObjectDictionary().Add(newUnit, newUnitObject); 
    }
}  