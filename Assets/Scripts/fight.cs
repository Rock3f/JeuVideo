using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Attack{
	public string name;

	public Sprite SpriteDamage;

	public float dammageValue;

	public float comboValue;

}

[ExecuteInEditMode]
public class fight : MonoBehaviour {
	// Public
	public float hp = 5;
	public float maxHp = 5;
	public Attack[] Attacks;

	public GameObject UltBarVar;
	// Private 
	private float accumulateur;

	private string EnemyType;

	private string CollisonSide;

	private float topAngle;
    private float sideAngle;

	// Use this for initialization
	void Start () {

		// Determine le type de personnage qu'il peut frapper
		if (this.gameObject.tag == "Player"){
			EnemyType = "Enemy";
		}
		else{
			EnemyType = "Player";
		}
	}
	
	// Update is called once per frame
	void Update () {

		//Est detruit quand sa vie arrive à 0
		if (hp <= 0){
			DestroyImmediate(this.gameObject);
		}
		// Initialise pour detection d'angle
		Vector2 size = GetComponent<BoxCollider2D>().size;
         size = Vector2.Scale (size, (Vector2)transform.localScale);
         topAngle = Mathf.Atan (size.x / size.y) * Mathf.Rad2Deg;
         sideAngle = 90.0f - topAngle;
         Debug.Log (topAngle + ", " + sideAngle);
	}

    public void OnCollisionStay2D (Collision2D coll)
    {
		// Detecte de quel cote viens la collison
		Vector2 v = coll.contacts[0].point - (Vector2)transform.position;
 
		if (Vector2.Angle(v, transform.up) <= topAngle) {
			CollisonSide = "T";
		}
		else if (Vector2.Angle(v, transform.right) <= sideAngle)  {
			CollisonSide = "R";
		}
		else if (Vector2.Angle(v, -transform.right) <= sideAngle) {
			CollisonSide = "L";
		}
		else {
			CollisonSide = "B";
		}

		// Permet de n'infliger qu'une seul fois des dégats par coup
        accumulateur += Time.deltaTime;

		// Lorsqu'une collison à lieu avec un enemie
		if(coll.gameObject.tag == EnemyType){
			// Pour chaque attack regarde si le sprite correspond au sprite de degat associé
			foreach (Attack att in Attacks){
				if(GetComponent<SpriteRenderer>().sprite == att.SpriteDamage 
				&& accumulateur > 0.2 
				// Vérifie que les ennemies sont bien face a face pour se frapper
				&& ((CollisonSide == "R" && coll.gameObject.GetComponent<fight>().CollisonSide == "L" && GetComponent<SpriteRenderer>().flipX == false) 
				|| (CollisonSide == "L" && coll.gameObject.GetComponent<fight>().CollisonSide == "R" &&GetComponent<SpriteRenderer>().flipX))
				)

				{	
					// Inflige des dégats en fonction de l'attaque
					coll.gameObject.GetComponent<fight>().hp -= att.dammageValue;

					// RAJOUTER : Faire passer à l'animation normale (contres)

					// Si le personnage a une barre de combo la rempli en fonction de l'attaque
					if (UltBarVar != null){
						UltBarVar.GetComponent<UltBar>().hit += att.comboValue;
					}
					accumulateur = 0;
				}
			}
		}
    }
}