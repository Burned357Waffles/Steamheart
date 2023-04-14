using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private GameObject settings;
        [SerializeField] private GameObject mainMenu;

        [SerializeField] private Button backButton;
        // needs fields for sliders and drop down

        private Canvas _mainMenuCanvas;
        private Canvas _settingsCanvas;
        
        private void Start()
        {
            _mainMenuCanvas = mainMenu.GetComponent<Canvas>();
            _settingsCanvas = settings.GetComponent<Canvas>();
            
            Menu.ButtonAction(backButton, Back);
        }

        public void Back()
        {
            Menu.CanvasTransition(_settingsCanvas, _mainMenuCanvas);
        }
        
    }
}
