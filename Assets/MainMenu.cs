using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Canvas pauseCanvas;
    public Scene startScene;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartMenu()
    {
        SceneManager.LoadScene("MenuStart");
        pauseCanvas.GetComponent<PauseMenu>().Resume();
    }

    public void replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        pauseCanvas.GetComponent<PauseMenu>().Resume();
    }

    public void QuitGame()
    {
        Debug.Log("QUIT !");
        Application.Quit();
    }
}
