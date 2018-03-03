using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour {

	public Transform foregroundBarHealthP1;
	public Transform backgroundBarHealthP1;

	public Transform foregroundBarHealthP2;
	public Transform namePlayer1;
	public Transform namePlayer2;
	public Transform backgroundBarHealthP2;
	public Transform foregroundBarUlt;
	public Transform backgroundBarUlt;

	public Transform foregroundBarUltP2;
	public Transform backgroundBarUltP2;
	public Transform target;
	public float trackingDistance = 1f;
	public bool isCameraFix = false;
	// Use this for initialization
	void Start () {
		backgroundBarHealthP1.position = new Vector3(-2.5f,4.2f,2);
		foregroundBarHealthP1.position = new Vector3(-2.5f,4.2f,1);	
		namePlayer1.position = new Vector3(-8.8f, 4.5f, 1);
		namePlayer2.position = new Vector3(-6f, 4.6f, 1);

		backgroundBarHealthP2.position = new Vector3(-1.4f,4.2f,2);
		foregroundBarHealthP2.position = new Vector3(-1.4f,4.2f,1);			

		backgroundBarUlt.position = new Vector3(-2.5f,3.5f,2);
		foregroundBarUlt.position = new Vector3(-2.5f,3.5f,2);

		backgroundBarUltP2.position = new Vector3(-1.4f,3.5f,2);
		foregroundBarUltP2.position = new Vector3(-1.4f,3.5f,2);
	}
	
	// Update is called once per frame
	void Update () {
		if(!isCameraFix)
		{
			// Position vers laquelle la caméra doit tendre
		// attention à bien conserver la position Z actuelle
		// on ne poursuit que les positions x et y
		Vector3 targetPos = new Vector3 (
			target.position.x,
			this.transform.position.y,
			this.transform.position.z
		);

		// l'offset est un déplacement relatif 
		// On va appliquer à la position de la caméra
		// Il est initialisé à zéro (pas de mouvement)
		Vector3 moveOffset = Vector3.zero;

		// Si la cible s'éoigne de la distance de Tracking
		// Le offset va prendre une valeur pour déplacer la Caméra vers la cible
		// Attention, ici on utiliser Vector2.Distance plutôt que Vector3.Distance, 
		//		      car seule la distance des coordonnées x et y nous intéresse.
		//            Avec Vector3.Distance la distance en Z serait prise en compte aussi
		//            ce qui n'est pas souhaité.
		//            Les Vector3 passés en paramètre sont transformé automatique en Vector2.
		if (Vector2.Distance (target.position, this.transform.position) > trackingDistance) {
			// C'est la formule magique du easing joli et rapide à mettre en place
			// Chaque frame, on va déplacer la Caméra de 5% de la distance qui la sépare de la cible
			// La formule est offset = (cible - valeur) * pourcentageDeProgression
			// C'est équivalent à Mathf.Lerp(valeur, cible, pourcentageDeProgression);
			// Pour une Vector3, C'est Vector3.Lerp(position, destination, pourcentageDeProgression)
			moveOffset = (targetPos - this.transform.position) * 0.05f;
		}

		// Applique le déplacement à la position en additionnant les vecteurs
		// équivalent de <code>this.transform.position = this.transform.position + moveOffset;</code>
		this.transform.position += moveOffset;
		backgroundBarHealthP1.position += moveOffset;	
		foregroundBarHealthP1.position += moveOffset;	
		namePlayer1.position += moveOffset;
		namePlayer2.position += moveOffset;

		backgroundBarHealthP2.position += moveOffset;	
		foregroundBarHealthP2.position += moveOffset;	

		backgroundBarUlt.position += moveOffset;	
		foregroundBarUlt.position += moveOffset;	
		backgroundBarUltP2.position += moveOffset;	
		foregroundBarUltP2.position += moveOffset;	
		}		
	}
}
