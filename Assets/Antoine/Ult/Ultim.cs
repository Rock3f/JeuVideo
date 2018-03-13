using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ultim : MonoBehaviour {

	//Public
	public Sprite GoUp;
	public Sprite GoDown;
	public float speed;
	public GameObject explosion;
	public GameObject UltBarVar;

	public float DanceTime;

	public GameObject cameraMain;

	//Private

	private Vector3 initalPosition;
	private Vector3 upPosition;
	private Vector3 downPosition;
	
	private bool goingUp;

	public bool playAnimation = false;
	public bool playDance = false;

	private float MaxHit;

	private float accumulateur;

	private AudioSource[] sounds;
	private GameObject[] EnemyList;
	private GameObject[] PlayerList;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		PlayerList = GameObject.FindGameObjectsWithTag("Player");
		MaxHit = UltBarVar.GetComponent<UltBar>().maxHit;
		if(cameraMain != null)
		{
			sounds = cameraMain.GetComponents<AudioSource>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Ult") && UltBarVar.GetComponent<UltBar>().hit >= MaxHit){
			UltBarVar.GetComponent<UltBar>().hit = 0;
			
			// Effet MALUS
       		/*
			gameObject.GetComponent<movePlayer>().enabled = false;
			playDance = true;
			sounds.FirstOrDefault(x => x.clip.name.Contains("SoundLevel1")).Pause();
			sounds.FirstOrDefault(x => x.clip.name.Contains("dance")).PlayOneShot(sounds.FirstOrDefault(x => x.clip.name.Contains("dance")).clip);
        	*/

			// Effet BONUS

			
			

			gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
			gameObject.GetComponent<movePlayer>().enabled = false;
			initalPosition = transform.position;
			if (gameObject.GetComponent<SpriteRenderer>().flipX == false){
				upPosition = new Vector3(initalPosition.x + 3 , initalPosition.y + 4, initalPosition.z +4 );
				downPosition = new Vector3(initalPosition.x + 5 , initalPosition.y, initalPosition.z);
			}
			else{
				upPosition = new Vector3(initalPosition.x - 3 , initalPosition.y + 4, initalPosition.z +4 );
				downPosition = new Vector3(initalPosition.x - 5 , initalPosition.y, initalPosition.z);
			}
			playAnimation =true;
			goingUp = true;
		}

		/*if (transform.position == downPosition){
			explosion.GetComponent<Transform>().position = downPosition;
			explosion.GetComponent<SpriteRenderer>().enabled = true;
		}*/

		if (playAnimation == true){
			UltAnimation(initalPosition, upPosition, downPosition);
		}
		if (playDance == true){
			UltDance();
		}

	}
	void LateUpdate() {
		if (transform.position == upPosition){
			
			foreach( GameObject enemys in PlayerList) {
				enemys.GetComponent<fight>().hp += 1;
			}
			Debug.Log("goingup = false");
			goingUp = false;
		}

		if (transform.position == downPosition && playAnimation == true){
			EnemyList = GameObject.FindGameObjectsWithTag("Enemy");
			foreach( GameObject enemys in EnemyList) {
				enemys.GetComponent<fight>().hp -= 5;
			}
			Debug.Log("playanimation = false");
			explosion.GetComponent<Transform>().position = new Vector3(downPosition.x, downPosition.y + 2, downPosition.z);
			explosion.GetComponent<SpriteRenderer>().enabled = true;
			playAnimation = false;
			gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
			gameObject.GetComponent<movePlayer>().enabled = true;

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
				sounds.FirstOrDefault(x => x.clip.name.Contains("SoundLevel1")).Play();
				sounds.FirstOrDefault(x => x.clip.name.Contains("dance")).Stop();
				gameObject.GetComponent<movePlayer>().enabled = true;
			}
	}

	void UltAnimation(Vector3 initalPosition, Vector3 upPosition, Vector3 downPosition) {

		Debug.Log("Ult");
		
		if (goingUp == true){
			spriteRenderer.sprite = GoUp;
			Debug.Log("GoingUp");
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(upPosition.x, upPosition.y, upPosition.z), speed * Time.deltaTime);
		}

		else if(goingUp == false){
			spriteRenderer.sprite = GoDown;
			Debug.Log("GoingDown");
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(downPosition.x, downPosition.y, downPosition.z), speed * Time.deltaTime);
		}
	}
}
