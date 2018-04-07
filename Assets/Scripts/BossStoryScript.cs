using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStoryScript : MonoBehaviour {

	// Use this for initializationpublic GameObject cameraMain;
    public GameObject uiToDisplay;
	public int secondsToWait = 2;
    private bool IsEnd;

    private void Update() {
        if(Input.GetButtonDown("Fire1")){
            if(IsEnd)
            {
                uiToDisplay.SetActive(false);
		        Time.timeScale = 1f;
                enabled = false;
            }	
            else
            {
                IsEnd = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.name == "Black_mamba_boss")
        {
            Time.timeScale = 0f;
		    uiToDisplay.SetActive(true);
        }
    }
}
