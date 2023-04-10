using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public GameObject unit;
    void Start()
    {
        Instantiate(unit, transform.position, transform.rotation);
    }

    void spawnUnit()
    {
       //Instantiate(unit, transform.position, transform.rotation);
    }
}