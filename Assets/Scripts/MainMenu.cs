using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Scene startScene;
    public UnityEngine.EventSystems.EventSystem eventSystem;
    public GameObject selectedGameObject;
    private bool buttonSelected;

     private void Start() {
        selectedGameObject = eventSystem.firstSelectedGameObject;     
    }

    void Update() {
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
    
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT !");
        Application.Quit();
    }
    
}
