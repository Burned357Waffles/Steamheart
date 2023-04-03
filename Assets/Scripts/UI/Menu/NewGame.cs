using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class NewGame : MonoBehaviour
    {

        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject newGame;
        [SerializeField] private GameObject hostNewLobby;

        [SerializeField] private Button backButton;
        [SerializeField] private Button nextButton;
        // something for number of players
        // something for settings name

        private Canvas _mainMenuCanvas;
        private Canvas _newGameCanvas;
        private Canvas _hostNewLobbyCanvas;
        
        void Start()
        {
            _mainMenuCanvas = mainMenu.GetComponent<Canvas>();
            _newGameCanvas = newGame.GetComponent<Canvas>();
            _hostNewLobbyCanvas = hostNewLobby.GetComponent<Canvas>();
            
            Menu.ButtonAction(backButton, Back);
            Menu.ButtonAction(nextButton, Next);
        }

        public void Back()
        {
            Menu.CanvasTransition(_newGameCanvas, _mainMenuCanvas);
        }

        public void Next()
        {
            Menu.CanvasTransition(_newGameCanvas, _hostNewLobbyCanvas);
        }

    }
}
