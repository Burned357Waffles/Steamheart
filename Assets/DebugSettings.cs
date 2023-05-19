using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSettings : MonoBehaviour
{
    // Inspector variables
    [SerializeField]
    private bool _debugMode = false;
    [SerializeField]
    private bool _overrideStartingMaterials = false;
    [SerializeField]
    private int _startingIron = 100;
    [SerializeField]
    private int _startingWood = 100;

    // Static variables
    public static bool debugMode;
    public static bool overrideStartingMaterials;
    public static int startingIron;
    public static int startingWood;


    public void Awake()
    {
        debugMode = _debugMode;
        overrideStartingMaterials = _overrideStartingMaterials;
        startingIron = _startingIron;
        startingWood = _startingWood;
    }
}
