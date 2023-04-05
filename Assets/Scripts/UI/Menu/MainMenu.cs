using UnityEngine;
using UnityEngine.UI;
using UI.Menu;
using Unity.VisualScripting;

namespace UI.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject newGame;
        [SerializeField] private GameObject loadGame;
        [SerializeField] private GameObject joinGame;
        [SerializeField] private GameObject settings;
        [SerializeField] private GameObject quit;

        [SerializeField] private Button newGameButton;
        [SerializeField] private Button loadGameButton;
        [SerializeField] private Button joinGameButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;

        private Canvas _mainMenuCanvas;
        private Canvas _newGameCanvas;
        private Canvas _loadGameCanvas;
        private Canvas _joinGameCanvas;
        private Canvas _settingsCanvas;
        private Canvas _quitCanvas;
    
        void Start()
        {
            _mainMenuCanvas = mainMenu.GetComponent<Canvas>();
            //_mainMenuCanvas.enabled = true;
            
            _newGameCanvas = newGame.GetComponent<Canvas>();
            //_loadGameCanvas = loadGame.GetComponent<Canvas>();
            _joinGameCanvas = joinGame.GetComponent<Canvas>();
            _settingsCanvas = settings.GetComponent<Canvas>();
            //_quitCanvas = quit.GetComponent<Canvas>();
            
            Menu.ButtonAction(newGameButton, NewGame);
            Menu.ButtonAction(loadGameButton, LoadGame);
            Menu.ButtonAction(joinGameButton, JoinGame);
            Menu.ButtonAction(settingsButton, Settings);
            Menu.ButtonAction(quitButton, Quit);
        }
    
        private void NewGame()
        {
            Menu.CanvasTransition(_mainMenuCanvas, _newGameCanvas);
        }

        private void JoinGame()
        {
            Menu.CanvasTransition(_mainMenuCanvas, _joinGameCanvas);
        }

        private void LoadGame()
        {
            Debug.Log("not implemented yet!");
        }

        private void Settings()
        {
            Menu.CanvasTransition(_mainMenuCanvas, _settingsCanvas);
        }

        private void Quit()
        {
            Debug.Log("not implemented yet!");
        }
    }
}
