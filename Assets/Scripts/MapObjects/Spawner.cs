using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField] public GameObject unit;

    // Won't work?
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) // this will be replaced soon
        {
            SpawnUnit();
            Debug.Log("unit spawned");
        }
        
        // check if a unit is clicked
        if (!Input.GetMouseButtonDown(0)) return;   
        Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;
        if(!hit.transform.CompareTag($"Unit")) return;
        
        
    }

    private void SpawnUnit()
    {
       Instantiate(unit, new Vector3(0, 1, 0), transform.rotation);
    }
}  