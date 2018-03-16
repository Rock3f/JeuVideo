using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryScript : MonoBehaviour {

	// Use this for initializationpublic GameObject cameraMain;
    public GameObject uiToDisplay;
	public int secondsToWait = 2;

    public void DisplayUI() {
        uiToDisplay.SetActive(true);
        Time.timeScale = 0f;		
        new WaitForSeconds(secondsToWait);
		Time.timeScale = 1f;
        uiToDisplay.SetActive(false);
    }

}
