using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyFollow : MonoBehaviour {

	public float speed;
	private GameObject[] PlayerList;
	private Transform Player1;

	private Transform Player2;

	private Transform target;
	private float normalspeed;

	private bool IsAlive;

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

			string spritesLastName = GetComponent<animationSprite>().currentAnim.sprites.Last().name;
			string spriteCurrentName =  GetComponent<animationSprite>().currentAnim.sprites[GetComponent<animationSprite>().currentSpriteIdx].name;

			if( spritesLastName == spriteCurrentName){
				attackChoice = "attack" + rnd.Next(1,4);
			}
		}
		else
		{
			// gameObject.GetComponent<animationSprite>().ChangeAnimation("die", true);
			// DestroyImmediate(this.gameObject);
		}
	}



	public void OnCollisionEnter2D(Collision2D coll) {
		if ( coll.gameObject.tag == "Player" && IsAlive){
			speed = 0;
		}
	}
	public void OnCollisionExit2D(Collision2D coll) {
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
