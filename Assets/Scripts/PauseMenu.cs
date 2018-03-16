using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject menuButton;

    public GameObject restartButton;
    public Text Pausetext;
    void Start() {
        Pause();

        restartButton.SetActive(false);
        menuButton.SetActive(false);
        Pausetext.text = "P l a y";
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
                EventSystem.current.firstSelectedGameObject = menuButton;
            }
        }
    }
    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        restartButton.SetActive(true);
        menuButton.SetActive(true);
        Pausetext.text = "P a u s e";
    }
    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
