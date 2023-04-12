using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this will convert an air tile to a land tile
public class tilePlacer : MonoBehaviour
{
    public GameObject tile;
    // Start is called before the first frame update
    void Start() {

    }


    // Update is called once per frame
    void Update() {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("mouse1 pressed");
            //Instantiate(tile, transform.position, transform.rotation);
        }
        
    }

}
