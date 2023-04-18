using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSound : MonoBehaviour
{    
    [SerializeField]
    private Misc.MoveCamera cameraRig = null;
    private FMODUnity.StudioEventEmitter emitter = null;

    // Start is called before the first frame update
    void Start()
    {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        if (emitter == null)
        {
            Debug.LogError("WindSound: No FMODUnity.StudioEventEmitter found on this GameObject.");
        }

        if (cameraRig == null)
        {
            Debug.LogError("WindSound: No MoveCamera set on this GameObject.");
        }

        emitter.Play();
    }

    // Update is called once per frame
    void Update()
    {
        float maxZoom = cameraRig.maxZoom;
        float minZoom = cameraRig.minZoom;
        float currentZoom = cameraRig.newZoom.y;
        float normZoom = (currentZoom - minZoom) / (maxZoom - minZoom);
        
        emitter.SetParameter("Camera Zoom", normZoom);
    }
}
