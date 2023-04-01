using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public void DisplayCanvas(Canvas canvas)
        {
            //allows you to turn menu screen on & off
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            foreach(Canvas c in canvases)
            {
                c.gameObject.SetActive(false);
            }
            canvas.gameObject.SetActive(true);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
