using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct Target{
	public Transform target;

	public bool WasHit;

}

public class EnemyFollow : MonoBehaviour {

	public float speed;
	public float attackRange;
	public Target[] targets;
	public bool isStory = false;

	private GameObject[] PlayerList;
	private Transform Player1;

	private Transform Player2;

	private Transform target;
	private float normalspeed;

	private bool IsAlive;

	private string spritesLastName;
	private string spriteCurrentName;

	private string attackChoice;

	private System.Random rnd = new System.Random();

	// Use this for initialization
	void Start () {
		normalspeed = speed;
		PlayerList = GameObject.FindGameObjectsWithTag("Player");
		Debug.Log(PlayerList[0].name +" "+ PlayerList[1].name);
		Player1 = PlayerList[0].transform;
		Player2 = PlayerList[1].transform;
	}
	
	// Update is called once per frame
	void Update () {

		IsAlive = gameObject.GetComponent<fight>().hp > 0;
		
		if(IsAlive)
		{
			if(isStory)
			{
				foreach(Target targetIA in targets)
				{
					if(!targetIA.WasHit)
					{
						target = targetIA.target;
					}
				}
			}	
			else
			{
				switch (this.gameObject.tag)
				{
					// Pour le cas où notre objet est un tireur
					case("Shooter"):
						// On défini le target de notre tireur non pas sur la distance mais les hp des joueurs
						if (Player2.gameObject.GetComponent<fight>().hp < Player1.gameObject.GetComponent<fight>().hp){
							target = Player2.GetComponent<Transform>();
						}
						else{
							target = Player1.GetComponent<Transform>();
						}

						gameObject.GetComponent<animationSprite>().SetAnimationFromSpeed(speed, true);
						
						if (transform.position.x > target.position.x){
							gameObject.GetComponent<SpriteRenderer>().flipX = true;
						}

						// Ensuite on défini la position à partir de laquelle il doit se tenir du joueur
						// Le tireur va s'avancer vers le joueur cible pour entrer dans sa range d'attaque
						// Autrement il va essayer de ne pas dépasser cette range pour se faire taper dessus
						if (Vector3.Distance(target.GetComponent<Transform>().position, transform.position) > attackRange)
						{
							transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x +1, target.position.y, target.position.z), speed * Time.deltaTime);
						}

						spritesLastName = GetComponent<animationSprite>().currentAnim.sprites.Last().name;
						spriteCurrentName =  GetComponent<animationSprite>().currentAnim.sprites[GetComponent<animationSprite>().currentSpriteIdx].name;

						if( spritesLastName == spriteCurrentName){
							attackChoice = "attack1";
							// Projectiles à rajouter
						}
						
					break;

					// Par défaut, tag = Enemy. Ce qui correspond aux ennemies au corp-à-corp.
					default:
						if (Vector3.Distance(Player1.GetComponent<Transform>().position, transform.position) > Vector3.Distance(Player2.GetComponent<Transform>().position, transform.position)){
						target = Player2.GetComponent<Transform>();
						}
						else{
							target = Player1.GetComponent<Transform>();
						}
						transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x +1, target.position.y, target.position.z), speed * Time.deltaTime);

						gameObject.GetComponent<animationSprite>().SetAnimationFromSpeed(speed, true);

						
						if (transform.position.x > target.position.x){
							gameObject.GetComponent<SpriteRenderer>().flipX = true;
						}

						spritesLastName = GetComponent<animationSprite>().currentAnim.sprites.Last().name;
						spriteCurrentName =  GetComponent<animationSprite>().currentAnim.sprites[GetComponent<animationSprite>().currentSpriteIdx].name;

						if( spritesLastName == spriteCurrentName){
							attackChoice = "attack" + rnd.Next(1,4);
						}
					break;
				}
			}
		}
		/*else
		{
			// gameObject.GetComponent<animationSprite>().ChangeAnimation("die", true);
			// DestroyImmediate(this.gameObject);
		}*/
	}



	public void OnCollisionEnter2D(Collision2D coll) {
		if ( coll.gameObject.tag == "Player" && IsAlive){
			speed = 0;
		}
		
	}
	public void OnCollisionExit2D(Collision2D coll) {
		speed = normalspeed;
	}

	public void OnTriggerExit2D(Collider2D other)
    {
        speed = normalspeed;
    }

	public void OnCollisionStay2D(Collision2D coll) {

		if ( coll.gameObject.tag == "Player" && IsAlive){
			if(gameObject.GetComponent<animationSprite>().currentAnim.name != attackChoice && gameObject.GetComponent<animationSprite>().currentAnim.name != "hit")
			{
				gameObject.GetComponent<animationSprite>().ChangeAnimation(attackChoice, true);
			}
		}
	}
}
