using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePnjBackGround : MonoBehaviour {

public float acceleration = 8f; // unit per second, per second
	public float maxSpeed = 4f; // unit per second
	public Vector3 currentSpeed;	

	private animationSprite ac;

	public float xmin = 119;
	public float xmax = 130;

	private bool isGoingLeft = false;

	// Use this for initialization
	void Start () {
		ac = GetComponent<animationSprite> ();
	}
	
	// Update is called once per frame
	void Update () {
			// Calcule une acceleration en fonction de l'entrée utilisateur et de l'accelération configurée pour l'objet
			// Chaque valeur du Vector3 est exprimée en unité par seconde par seconde
			// celà veut dire que chaque seconde, la vitesse augmente de cette valeur configurée.
			// Ex: Chaque seconde, la vitesse augmente de 8 unités par seconde.
			Vector3 currentAcceleration = new Vector3 (
				1 * acceleration,
				0,
				0
			);

			// Calcule la nouvelle vitesse à partir de l'accélération
			// currentAcceleration retourne un changement de vitesse par seconde mais lors d'un update 
			// seulement "Time.deltaTime" secondes se sont écoulées. On applique donc l'accélération proportionellement 
			// au temps écoulé.
			currentSpeed += currentAcceleration * Time.deltaTime;
			// Vector3.ClampMagnitude permet de limiter la vitesse globale en borant l'amplitude du vecteur de vitesse
			currentSpeed = Vector3.ClampMagnitude (currentSpeed, maxSpeed);
			// Simule un freinage lorsque le personnage cesse de se déplacer
			if (currentAcceleration.magnitude == 0) currentSpeed *= 0.8f;

			// finalement, ajoute la vitesse en cours à la position pour créer le mouvement.
			// currentSpeed est exprimé en unités par seconde mais lors d'un update 
			// seulement "Time.deltaTime" secondes se sont écoulées. On applique donc la vitesse proportionellement 
			// au temps écoulé.

			if(this.transform.position.x >= xmax)
			{
				isGoingLeft = true;
				
			}

			if(this.transform.position.x <= xmin)
			{
				isGoingLeft = false;
			}

			if(isGoingLeft)
			{
				this.transform.position -= currentSpeed * Time.deltaTime;
			}
			else
			{
				this.transform.position += currentSpeed * Time.deltaTime;
			}
			
			this.transform.position = new Vector3(
			transform.position.x,
			transform.position.y,
			transform.position.y
			);

			// Utilise l'entrée utilisateur pour décider quelle animation afficher.
			// celà permet d'avoir un feedback (retour visuel) immédiat qui lui indique que son
			// action (bouger, ne plus bouger, changer de direction) est prise en compte.
			
			ac.SetAnimationPNJ (1 + 0.001f * currentAcceleration.magnitude, isGoingLeft);
	}
}
