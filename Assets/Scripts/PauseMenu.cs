using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public UnityEngine.EventSystems.EventSystem eventSystem;
    public GameObject selectedGameObject;
    public GameObject GameOverScreen;



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
            }
        }

        if(GameIsPaused || GameOverScreen.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
            {
                eventSystem.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
            }

            if(eventSystem.currentSelectedGameObject != selectedGameObject)
            {
                if(eventSystem.currentSelectedGameObject == null)
                {
                    eventSystem.SetSelectedGameObject(selectedGameObject);
                }
                else
                {
                    selectedGameObject = eventSystem.currentSelectedGameObject;
                }
            }
        }
    }
    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        StartCoroutine("highlightBtn");
    }

    public void StartMenu()
    {
        SceneManager.LoadScene("MenuStart");
        Resume();
    }

    public void replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Resume();
    }

    IEnumerator highlightBtn()
     {
         eventSystem.SetSelectedGameObject(null);
         yield return null;
         eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
     }
}
