using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambience : MonoBehaviour
{    
    public float cityMaxDistance = 100f;
    public float cityFadeDistance = 50f;
    [SerializeField]
    private Misc.MoveCamera cameraRig = null;
    [SerializeField]
    private Hex.HexGrid hexGrid = null;
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

        if (hexGrid == null)
        {
            Debug.LogError("WindSound: No HexGrid set on this GameObject.");
        }

        emitter.Play();
    }

    // Update is called once per frame
    void Update()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Camera Zoom", GetNormZoom());
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Balance City Nature", 1 - _CityAttenuation());
    }

    private float GetNormZoom()
    {
        float maxZoom = cameraRig.maxZoom;
        float minZoom = cameraRig.minZoom;
        float currentZoom = cameraRig.newZoom.y;
        float normZoom = (currentZoom - minZoom) / (maxZoom - minZoom);

        return normZoom;
    }

    private float GetMinCityDistance()
    {
        float minCityDistance = Mathf.Infinity;
        
        List<MapObjects.City> cities = hexGrid.GetCityList();

        Vector3 minCityLocation = Vector3.zero;

        Vector3 position = cameraRig.transform.position;

        for (int i = 0; i < cities.Count; i++)
        {
            Vector3 cityLocation = cities[i].GetCityCenter().WorldPosition;
            float distance = Vector3.Distance(position, cityLocation);
            if (distance < minCityDistance || minCityDistance == 0)
            {
                minCityDistance = distance;
                minCityLocation = cityLocation;
            }
        }

        return minCityDistance;
    }

    private float _CityAttenuation()
    {
        float minCityDistance = GetMinCityDistance();
        float attenuation = 1 - (minCityDistance - cityFadeDistance) / (cityMaxDistance - cityFadeDistance);

        return Mathf.Clamp(attenuation, 0, 1);
    }
}
