using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NavigationMenu : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject selectedGameObject;
    private bool buttonSelected;
    void Update() {
        if(Input.GetAxisRaw("Horizontal") != 0 && buttonSelected == false)
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
}
