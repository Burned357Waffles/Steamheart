using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class HexGrid : MonoBehaviour
{
    [SerializeField] private GameObject hexPrefab;
    [SerializeField] private GameObject airHex;
    [SerializeField] private GameObject basicHex;
    [SerializeField] private GameObject forestHex;
    [SerializeField] private GameObject mountainHex;

    public int mapRadius;
    private List<Hex> hexList = new List<Hex>();
    private Vector3[] directionVectors =
    {
        new Vector3(1, 0, -1),
        new Vector3(1, -1, 0),
        new Vector3(0, -1, 1),
        new Vector3(-1, 0, 1),
        new Vector3(-1, 1, 0),
        new Vector3(0, 1, -1)
    };
    private void Start()
    {
        GenerateGrid();
     }

    // the map will generate in a spiral pattern
    private void GenerateGrid()
    {
        // store the center of the map
        Hex center = new Hex(0, 0);
        hexList.Add(center);
        // call ring from center outward. while i < 4, generate only land for center island
        for (int i = 1; i < mapRadius; i++)
        {
            HexRing(center.GetVectorCoordinates(), i);
        }
        InstantiateHexes();
    }

    private void InstantiateHexes()
    {
        GameObject new_hex;
        int hexCount = 1;
        foreach (var hex in hexList)
        {
            // get the tile type
            if (hexCount > 37) GetHexType(hex.Q, hex.R, true); // no longer the center island
            else GetHexType(hex.Q, hex.R, false); // still is center island
            float hexRotation = Random.Range(0, 7) * 60;
            Debug.Log(hexRotation);
            new_hex = Instantiate(hexPrefab,
                                  hex.Position(),
                                  Quaternion.identity,
                                  this.transform);
            new_hex.transform.Rotate(0f, hexRotation, 0f, Space.Self);
            hexCount++;
        }
    }

    private void GetHexType(int q, int r, bool hasAir)
    {
        int randomOffset = Random.Range(0, 1000);
        float noise = Mathf.PerlinNoise(randomOffset + q/(float)mapRadius, randomOffset + r/(float)mapRadius);
        //Debug.Log("Q: " + q +", R: " + r + ", N: " + noise);
        if (noise > .7 && noise < .8) hexPrefab = basicHex;
        else if (noise > .8 && noise < .9) hexPrefab = forestHex;
        else if (noise > .9 && noise < 1) hexPrefab = mountainHex;
        else
        {
            if (hasAir) hexPrefab = airHex;
            else hexPrefab = basicHex;
        }
    }

    private Vector3 AddCoordinates(Vector3 hexCoordinates, Vector3 addCoordinates)
    {
        return hexCoordinates + addCoordinates;
    }
    private Vector3 CoordinateScale(Vector3 coordinates, int factor)
    {
        return coordinates * factor;
    }
    private Vector3 hexNeighbor(Vector3 coordinates, int direction)
    { 
        return AddCoordinates(coordinates, directionVectors[direction]);
    }
    // this function will generate hexes in a ring with given radius
    private void HexRing(Vector3 center, int radius)
    {
        Vector3 hexCoordinates = AddCoordinates(center,
                                    CoordinateScale(directionVectors[4], radius));
        for (int i = 0; i < 6; i++)
        {
            for(int j = 0; j < radius; j++)
            {
                hexList.Add(new Hex((int)hexCoordinates.x, (int)hexCoordinates.y));
                hexCoordinates = hexNeighbor(hexCoordinates, i);
            }
        }
    }




}