using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class HostNewLobby : MonoBehaviour
    {

        [SerializeField] private GameObject hostNewLobby;
        [SerializeField] private GameObject newGame;

        [SerializeField] private Button nextButton;
        [SerializeField] private Button backButton;

        private Canvas _hostNewLobbyCanvas;
        private Canvas _newGameCanvas;
        
        private void Start()
        {
            _hostNewLobbyCanvas = hostNewLobby.GetComponent<Canvas>();
            _newGameCanvas = newGame.GetComponent<Canvas>();
            
            Menu.ButtonAction(nextButton, Next);
            Menu.ButtonAction(backButton, Back);
        }

        public void Next()
        {
            Debug.Log("not yet implemented!");
        }

        public void Back()
        {
            Menu.CanvasTransition(_hostNewLobbyCanvas, _newGameCanvas);
        }
        
    }
}
