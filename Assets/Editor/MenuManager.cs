using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadSCene(sceneName);
    }
}
