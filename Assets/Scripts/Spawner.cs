using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public GameObject unit;

    // Won't work?
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            spawnUnit();
            Debug.Log("unit spawned");
        }
    }

    // void Start() 
    // {
    //     spawnUnit();
    // }
    

    void spawnUnit()
    {
       Instantiate(unit, transform.position, transform.rotation);
    }
}  