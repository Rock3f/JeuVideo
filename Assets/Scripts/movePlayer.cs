﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class movePlayer : MonoBehaviour {

	public float acceleration = 8f; // unit per second, per second
	public float maxSpeed = 4f; // unit per second

	public string Player;
	public Vector3 currentSpeed;
	public GameObject mainCamera;	

	public bool IsDead = false;

	private animationSprite ac;
	private string Horizontal;
	private string Vertical;

	private string Fire1;
	private string Fire2;
	private string Fire3;

	private bool isAction = false;
	private bool IsUpdatedNow; 

	// Use this for initialization
	void Start () {
		// Récupère une référence au script AnimationCourse attaché au même GameObject
		ac = GetComponent<animationSprite> ();
		// SBO: très bien !
		Horizontal = "Horizontal" + Player;
		Vertical = "Vertical" + Player;
		Fire1 = "Fire1" + Player;
		Fire2 = "Fire2" + Player;
		Fire3 = "Fire3" + Player;
	}

	// Update is called once per frame
	void Update () {

		if(!IsDead)
		{
			IsUpdatedNow = false;
			if(Input.GetButtonDown(Fire1)){
				isAction = true;
				IsUpdatedNow = true;
				if(ac.currentAnim.name != "attack1")
				{
					ac.ChangeAnimation("attack1", isAction);
				}			
			}
			else
			{
				if(!IsUpdatedNow)
				{
					isAction = false;
				}
			}

			if(Input.GetButtonDown(Fire2)){
				isAction = true;
				IsUpdatedNow = true;
				if(ac.currentAnim.name != "attack2")
				{
					ac.ChangeAnimation("attack2", isAction);
					mainCamera.GetComponents<AudioSource>().FirstOrDefault( x => x.clip.name.Contains("jumpGrunt")).PlayOneShot(mainCamera.GetComponents<AudioSource>().FirstOrDefault( x => x.clip.name.Contains("jumpGrunt")).clip);
				}			
			}
			else
			{
				if(!IsUpdatedNow)
				{
					isAction = false;
				}
				
			}
			
			if(Input.GetButtonDown(Fire3)) {
				isAction = true;
				IsUpdatedNow = true;

				if(ac.currentAnim.name != "attack3")
				{
					ac.ChangeAnimation("attack3", isAction);
				}			
			}
			else
			{
				if(!IsUpdatedNow)
				{
					isAction = false;
				}
			}

			// SBO: logique à simplifier, c'est difficile à suivre
			// proposition : enlever isAction et IsUpdatedNow de toutes les logiques de input => réaction
			//               utiliser à part la formule "bool isAction = Input.GetButtonDown(Fire1) || Input.GetButtonDown(Fire2) || Input.GetButtonDown(Fire3)
			

			// SBO : IsDead a déjà été vérifié, this.gameObject.GetComponent<fight>().hp > 0 ne semble pas nécessaire
			// + éviter GetComponent à chaque update
			if(!isAction && this.gameObject.GetComponent<fight>().hp > 0) {
				// Calcule une acceleration en fonction de l'entrée utilisateur et de l'accelération configurée pour l'objet
				// Chaque valeur du Vector3 est exprimée en unité par seconde par seconde
				// celà veut dire que chaque seconde, la vitesse augmente de cette valeur configurée.
				// Ex: Chaque seconde, la vitesse augmente de 8 unités par seconde.
				Vector3 currentAcceleration = new Vector3 (
					Input.GetAxis (Horizontal) * acceleration,
					Input.GetAxis (Vertical) * acceleration,
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
				this.transform.position += currentSpeed * Time.deltaTime;
				this.transform.position = new Vector3(
				transform.position.x,
				transform.position.y,
				transform.position.y // SBO: super idée ! En 2D cependant la position Z peut être écrasée par la valeur du SpriteRenderer.orderInLayer. Regarder de ce côté là si vous avez des problèmes d'ordre d'affichage.
				);

				// Utilise l'entrée utilisateur pour décider quelle animation afficher.
				// celà permet d'avoir un feedback (retour visuel) immédiat qui lui indique que son
				// action (bouger, ne plus bouger, changer de direction) est prise en compte.
				
				ac.SetAnimationFromSpeed (Input.GetAxis (Horizontal) + 0.001f * currentAcceleration.magnitude, isAction);
			}
		}
	}
}
