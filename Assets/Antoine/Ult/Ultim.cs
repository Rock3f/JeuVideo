using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ultim : MonoBehaviour {

	//Public
	public float speed;
	public GameObject explosion;
	public GameObject UltBarVar;

	public float DanceTime;

	//Private

	public Vector3 initalPosition;
	public Vector3 upPosition;
	public Vector3 downPosition;
	
	public bool goingUp;

	public bool playAnimation = false;
	public bool playDance = false;

	private float MaxHit;

	public float accumulateur;

	// Use this for initialization
	void Start () {
		MaxHit = UltBarVar.GetComponent<UltBar>().maxHit;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Ult") && UltBarVar.GetComponent<UltBar>().hit >= MaxHit){
			UltBarVar.GetComponent<UltBar>().hit = 0;
			
			// Effet MALUS
       		gameObject.GetComponent<movePlayer>().enabled = false;
			playDance = true;
        	

			// Effet BONUS
			/*gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
			initalPosition = transform.position;
			if (gameObject.GetComponent<SpriteRenderer>().flipX == false){
				upPosition = new Vector3(initalPosition.x + 3 , initalPosition.y + 4, initalPosition.z +4 );
				downPosition = new Vector3(initalPosition.x + 4 , initalPosition.y, initalPosition.z);
			}
			else{
				upPosition = new Vector3(initalPosition.x - 3 , initalPosition.y + 4, initalPosition.z +4 );
				downPosition = new Vector3(initalPosition.x - 4 , initalPosition.y, initalPosition.z);
			}
			playAnimation =true;
			goingUp = true;*/
		}

		if (playAnimation == true){
			UltAnimation(initalPosition, upPosition, downPosition);
		}
		if (playDance == true){
			UltDance();
		}

	}
	void LateUpdate() {
		if (transform.position == upPosition){
			Debug.Log("goingup = false");
			goingUp = false;
		}

		if (transform.position == downPosition){
			Debug.Log("playanimation = false");
			playAnimation = false;
			gameObject.GetComponent<BoxCollider2D>().isTrigger = false;

		}
	}

	void UltDance(){
		accumulateur += Time.deltaTime;

        	if (accumulateur < DanceTime) {
				if( gameObject.GetComponent<animationSprite>().currentAnim.name != "dance")
				{
					gameObject.GetComponent<animationSprite>().ChangeAnimation("dance", true);
				}	
        	}

			else{
				accumulateur = 0;
				playDance = false;
				gameObject.GetComponent<movePlayer>().enabled = true;
			}
	}

	void UltAnimation(Vector3 initalPosition, Vector3 upPosition, Vector3 downPosition) {

		Debug.Log("Ult");
		
		if (goingUp == true){
			Debug.Log("GoingUp");
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(upPosition.x, upPosition.y, upPosition.z), speed * Time.deltaTime);
		}

		else if(goingUp == false){
			Debug.Log("GoingDown");
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(downPosition.x, downPosition.y, downPosition.z), speed * Time.deltaTime);
		}
	}
}
