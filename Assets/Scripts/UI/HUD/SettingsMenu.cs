using System;
using Misc;
using Unity.VisualScripting;
using UnityEngine;

namespace UI.HUD
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] public GameObject cameraRig;
        [SerializeField] public Canvas settingsCanvas;

        private FMODUnity.StudioEventEmitter _uiClickEmitter;
        
        private void Start()
        {
            settingsCanvas.enabled = false;
            _uiClickEmitter = GameObject.Find("Select").GetComponent<FMODUnity.StudioEventEmitter>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (settingsCanvas.enabled)
                {
                    Resume();
                    _uiClickEmitter.Play();
                }
                else 
                {
                    OpenMenu();
                    _uiClickEmitter.Play();
                    
                }
            }
        }

        public void OpenMenu()
        {
            cameraRig.GetComponent<MoveCamera>().enabled = false;
            settingsCanvas.enabled = true;
        }

        public void Resume()
        {
            cameraRig.GetComponent<MoveCamera>().enabled = true;
            settingsCanvas.enabled = false;
        }

        public void Exit()
        {
            #if UNITY_EDITOR
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
            #else 
		    {
			    Application.Quit();
		    }
            #endif
        }
    }
}