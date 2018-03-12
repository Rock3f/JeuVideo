using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraWarningPlayer : MonoBehaviour {

	public Transform P1;
	public Transform P2;
	public GameObject P1GO;
	public GameObject P2GO;
	public float frameDuration = 0.2f;
	public GameObject warning;
	public Camera cameraMain;
	public Transform cameraTransform;

	private float accumulateur = 0;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		// durée accumulée depuis le dernier changement de frame
        accumulateur += Time.deltaTime;

        // vide l'accumulateur et fait avancer les frames        
        while (accumulateur > frameDuration && frameDuration > 0) {
			AudioSource siren = cameraMain.GetComponents<AudioSource>().FirstOrDefault(x => x.clip.name.Contains("siren"));
            if((P1.position.x - cameraTransform.position.x) > (cameraMain.orthographicSize * 2)
			 || (P1.position.x - cameraTransform.position.x) < (cameraMain.orthographicSize * -2) 
			 || (P2.position.x - cameraTransform.position.x) > (cameraMain.orthographicSize * 2) 
			 || (P2.position.x - cameraTransform.position.x) < (cameraMain.orthographicSize * -2))
			{
				warning.SetActive(!warning.activeSelf);

				if(!siren.isPlaying)
					siren.PlayOneShot(siren.clip);
			}     
			else
			{
				if(siren.isPlaying)
					siren.Stop();

				warning.SetActive(false);
			}

			if((P1.position.x - cameraTransform.position.x) > (cameraMain.orthographicSize * 2.1)
			 || (P1.position.x - cameraTransform.position.x) < (cameraMain.orthographicSize * -2.1))
			{
				P1GO.GetComponent<fight>().hp -= 0.1f;
			} 

			if((P2.position.x - cameraTransform.position.x) > (cameraMain.orthographicSize * 2.1) 
			 || (P2.position.x - cameraTransform.position.x) < (cameraMain.orthographicSize * -2.1))
			{
				P2GO.GetComponent<fight>().hp -= 0.1f;
			}
            
            accumulateur -= frameDuration;
        }
		
	}
}
