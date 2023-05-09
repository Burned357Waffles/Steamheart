using Misc;
using UnityEngine;

namespace UI.HUD
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] public GameObject cameraRig;
        [SerializeField] public Canvas settingsCanvas;

        private void Start()
        {
            settingsCanvas.enabled = false;
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