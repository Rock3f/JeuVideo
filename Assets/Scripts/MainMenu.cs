using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Canvas pauseCanvas;
    public Scene startScene;
    public EventSystem eventSystem;
    public GameObject selectedGameObject;
    private bool buttonSelected;
    void Update() {
        if(Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedGameObject);
            buttonSelected = true;
        }

        if(Input.GetButtonDown("Fire1"))
        {
            eventSystem.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }
    }

    private void OnDisable() {
        buttonSelected = false;
    }

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
