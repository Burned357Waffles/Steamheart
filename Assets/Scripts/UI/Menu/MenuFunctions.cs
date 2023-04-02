using Misc;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Menu
{
    public class MenuFunctions : MonoBehaviour
    {

        [SerializeField] GameObject mainMenu;
        [SerializeField] GameObject createGame;
        [SerializeField] GameObject settings;

        [SerializeField] private Button newGameButton;
        [SerializeField] private Button joinGameButton;
        

        public void Start()
        {
            createGame.SetActive(false);
            settings.SetActive(false);

            RegisterFunc(newGameButton, NewGame);
            RegisterFunc(joinGameButton, JoinGame);
        }


        public void NewGame()
        {
            mainMenu.SetActive(false);
            createGame.SetActive(true);
        }

        public void JoinGame()
        {
            Debug.Log("join game not implemented yet!");
        }


        private void RegisterFunc(Button button, UnityAction functionName)
        {
            try
            {
                button.onClick.AddListener(functionName);
            }
            catch
            {
                Debug.LogError("\tRegisterFunc with button "
                               + button.name
                               + "\n\tand function "
                               + functionName.Method.Name
                               + "\n\thas failed by "
                               + (button == null ? "null button" : "add listener"));
            }
        }
    }
}
