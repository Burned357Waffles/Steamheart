using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class JoinGame : MonoBehaviour
    {

        [SerializeField] private GameObject joinGame;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject guestLobby;
        
        [SerializeField] private Button backButton;

        private Canvas _mainMenuCanvas;
        private Canvas _joinGameCanvas;
        private Canvas _guestLobbyCanvas;
        
        private void Start()
        {
            _mainMenuCanvas = mainMenu.GetComponent<Canvas>();
            _joinGameCanvas = joinGame.GetComponent<Canvas>();
            //_guestLobbyCanvas = guestLobby.GetComponent<Canvas>();
            
            
            Menu.ButtonAction(backButton, Back);
            // initialize rest
        }

        public void Back()
        {
            Menu.CanvasTransition(_joinGameCanvas, _mainMenuCanvas);
        }
        
    }
}
