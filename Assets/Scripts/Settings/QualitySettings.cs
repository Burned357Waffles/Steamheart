using System;
using TMPro;
using UnityEngine;

namespace Settings
{
    public class QualitySettings : MonoBehaviour
    {
        [SerializeField] public GameObject particleMenu;
        [SerializeField] public GameObject waterMenu;
        [SerializeField] public GameObject dayNightMenu;

        private static TMP_Dropdown _particleDrop;
        private static TMP_Dropdown _waterDrop;
        private static TMP_Dropdown _dayNightDrop;

        private static int _particleQuality;
        private static int _waterQuality;
        private static int _dayNightOn;

        private void Start()
        {
            _particleDrop = particleMenu.GetComponent<TMP_Dropdown>();
            _waterDrop = waterMenu.GetComponent<TMP_Dropdown>();
            _dayNightDrop = dayNightMenu.GetComponent<TMP_Dropdown>();
        }

        public static void SetParticleQuality()
        {
            _particleQuality = _particleDrop.value;
            Debug.Log("Setting Particle Quality");
        }
        
        public static void SetWaterQuality()
        {
            _waterQuality = _waterDrop.value;
            Debug.Log("Setting Water Quality");
        }
        
        public static void SetDayNight()
        {
            _dayNightOn = _dayNightDrop.value;
            Debug.Log("Setting Day Night Cycle");
        }
        
        public static int GetParticleQuality() { return _particleQuality; }
        public static int GetWaterQuality() { return _waterQuality; }
        public static int GetDayNightQuality() { return _dayNightOn; }
    }
}