using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HexGrid : MonoBehaviour
{
    [SerializeField] private GameObject hexPrefab; // if unassigned it is air
    [SerializeField] private GameObject airHex;
    [SerializeField] private GameObject basicHex;
    [SerializeField] private GameObject forestHex;
    [SerializeField] private GameObject mountainHex; 
    
    
    private void Start()
     {
        GenerateGrid();
     }

    private void GenerateGrid()
    {
        float[] size = {32, 32};
        GameObject new_hex;
        for(int i = 0; i < size[0]; i++)
        {
            for (int j = 0; j < size[1]; j++)
            {
                int randomOffset = Random.Range(0, 1000);
                float noise = Mathf.PerlinNoise(randomOffset + i/size[0], randomOffset + j/size[1]);
                
                Debug.Log(noise);
                if (noise > .55 && noise < .75) hexPrefab = basicHex;
                else if (noise > .75 && noise < .85) hexPrefab = forestHex;
                else if (noise > .85 && noise < 1) hexPrefab = mountainHex;
                else hexPrefab = airHex;
                
                //hexPrefab = basicHex; // remove after testing
                Hex hex = new Hex(i, j);

                new_hex = Instantiate(hexPrefab,
                    hex.Position(),
                    Quaternion.identity,
                    this.transform);
            }
        }
    }
}