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
	public GameObject screenGameOver;

	// Private 
	private float accumulateur;

	private string EnemyType;

	public string CollisonSide;

	public float Angle;

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
		// Initialise pour detection d'angle
		Vector2 size = GetComponent<BoxCollider2D>().size;
         size = Vector2.Scale (size, (Vector2)transform.localScale);
         topAngle = Mathf.Atan (size.x / size.y) * Mathf.Rad2Deg;
         sideAngle = 90.0f - topAngle;
	}

	void LateUpdate() {
		// Lorsque la vie arrive à zero fait l'animation de mort
		if(hp <= 0){
			if(gameObject.GetComponent<animationSprite>().currentAnim.name != "die")
			{
				gameObject.GetComponent<animationSprite>().ChangeAnimation("die", true);
			}
			// Le dernier sprite de l'animation est vide et fait disparaitre le personnage
			if(GetComponent<SpriteRenderer>().sprite == null){
				gameObject.SetActive(false);
				// Si c'est un mechant il est le GameObject est détruit
				if (EnemyType == "Player"){
					DestroyImmediate(this.gameObject);
				}

				if (EnemyType == "Enemy"){
					screenGameOver.SetActive(true);
				}
				
			}	
		}
		
	}

    public void OnCollisionStay2D (Collision2D coll)
    {
		// Detecte de quel cote viens la collison
		Vector2 v = coll.contacts[0].point - (Vector2)transform.position;
 
		if (Vector2.Angle(v, transform.up) <= topAngle) {
			CollisonSide = "T";
			Angle = Vector2.Angle(v, transform.up);

		}
		else if (Vector2.Angle(v, transform.right) <= sideAngle)  {
			CollisonSide = "R";
			Angle = Vector2.Angle(v, transform.right);
		}
		else if (Vector2.Angle(v, -transform.right) <= sideAngle) {
			CollisonSide = "L";
			Angle = Vector2.Angle(v, transform.right);
		}
		else {
			CollisonSide = "B";
			Angle = Vector2.Angle(v, transform.up);
		}

		// Permet de n'infliger qu'une seul fois des dégats par coup
        accumulateur += Time.deltaTime;

		// Lorsqu'une collison à lieu avec un enemie
		if(coll.gameObject.tag == EnemyType){
			
			string OtherCollSide = coll.gameObject.GetComponent<fight>().CollisonSide;
			bool OtherflipX = GetComponent<SpriteRenderer>().flipX;

			// Pour chaque attack regarde si le sprite correspond au sprite de degat associé
			foreach (Attack att in Attacks){
				if(GetComponent<SpriteRenderer>().sprite == att.SpriteDamage 
				&& accumulateur > 0.2
				// Vérifie que les ennemies sont bien face a face pour se frapper
				&& ((CollisonSide == "R" && OtherCollSide == "L" && OtherflipX == false) 
				|| (CollisonSide == "L" && OtherCollSide == "R" && OtherflipX == true))
				)

				{	
					// Inflige des dégats en fonction de l'attaque
					coll.gameObject.GetComponent<fight>().hp -= att.dammageValue;

					// Déclenche l'animation hit
					if (coll.gameObject.GetComponent<fight>().hp > 0){
						coll.gameObject.GetComponent<animationSprite>().ChangeAnimation("hit", true);
					}

					// Si le personnage a une barre de combo la rempli en fonction de l'attaque
					if (UltBarVar != null){
						AddHit(att.comboValue);
					}
					accumulateur = 0;
				}
			}
		}
    }

	private void AddHit(float AddedHit) {
		UltBarVar.GetComponent<UltBar>().hit += AddedHit;
	}
}