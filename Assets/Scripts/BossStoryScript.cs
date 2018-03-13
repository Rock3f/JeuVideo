using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStoryScript : MonoBehaviour {

	// Use this for initializationpublic GameObject cameraMain;
    public GameObject uiToDisplay;
	public int secondsToWait = 2;

    private void OnTriggerEnter2D(Collider2D other) {
        Time.timeScale = 0f;
		uiToDisplay.SetActive(true);
		wait(secondsToWait);
		uiToDisplay.SetActive(false);
		Time.timeScale = 1f;
    }

	IEnumerator wait(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
