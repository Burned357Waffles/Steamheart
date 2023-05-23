using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class NewGame : MonoBehaviour
    {

        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject newGame;
        [SerializeField] private GameObject hostNewLobby;
        [SerializeField] private GameObject characterSelectOpen;

        [SerializeField] private Button backButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button characterButton;
        [SerializeField] private Button charSelectButton;
        [SerializeField] private Button charSelectButton2;
        
        // something for number of players
        // something for settings name

        private Canvas _mainMenuCanvas;
        private Canvas _newGameCanvas;
        private Canvas _hostNewLobbyCanvas;
        private Canvas _characterCanvas;
        
        void Start()
        {
            _mainMenuCanvas = mainMenu.GetComponent<Canvas>();
            _newGameCanvas = GameObject.Find("CharacterSelect").GetComponent<Canvas>();
            _hostNewLobbyCanvas = hostNewLobby.GetComponent<Canvas>();
            _characterCanvas = characterSelectOpen.GetComponent<Canvas>();

            Menu.ButtonAction(characterButton, OpenCharacterSelect);
            Menu.ButtonAction(charSelectButton, CloseCharacterSelect);
            Menu.ButtonAction(charSelectButton2, CloseCharacterSelect);
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
        
        private void OpenCharacterSelect()
        {
            _characterCanvas.enabled = true;
        }
        
        private void CloseCharacterSelect()
        {
            _characterCanvas.enabled = false;
        }

    }
}
