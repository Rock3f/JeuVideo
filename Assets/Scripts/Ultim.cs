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
	public ParticleSystem croix;
	public int Player;
	public float actualSpeed;

	//Private
	private Vector3 initalPosition;
	private Vector3 upPosition;
	private Vector3 downPosition;
	private bool goingUp;
	private bool playAnimation = false;
	private bool playDance = false;
	private float MaxHit;
	private float accumulateur;
	private string UltPlayer;
	private AudioSource[] sounds;
	private GameObject[] EnemyList;
	private GameObject[] PlayerList;
	private SpriteRenderer spriteRenderer;
	private bool choiceBanousMalus = false;

	private ParticleSystem.EmissionModule croixEmission;


	// Use this for initialization
	void Start () {
		croix.Play(true);
		croixEmission = croix.emission;
		UltPlayer = "Ult" + Player;
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
		if(Input.GetButtonDown(UltPlayer) && UltBarVar.GetComponent<UltBar>().hit >= MaxHit){
			UltBarVar.GetComponent<UltBar>().hit = 0;
	
			System.Random Rand = new System.Random();
			choiceBanousMalus = Rand.Next(2) == 0 ? false :true;

			if(choiceBanousMalus == true){
				// Effet MALUS
				
				gameObject.GetComponent<movePlayer>().enabled = false;
				
				playDance = true;
				sounds.FirstOrDefault(x => x.clip.name.Contains("SoundLevel1")).Pause();
				sounds.FirstOrDefault(x => x.clip.name.Contains("dance")).PlayOneShot(sounds.FirstOrDefault(x => x.clip.name.Contains("dance")).clip);
			}

			else{
				// Effet BONUS
				croixEmission.enabled = true;
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
				if (playAnimation == true){
					StartCoroutine(UltAnimation());
				}
			}
		}
		
		if (playDance == true){
			UltDance();
		}

		

	}
	void LateUpdate() {
		if (transform.position == upPosition){
			
			foreach( GameObject players in PlayerList) {
				players.GetComponent<fight>().hp += 1;
			}
		
			goingUp = false;
		}

		if (transform.position == downPosition && playAnimation == true){
			croixEmission.enabled = false;
			sounds.FirstOrDefault(x => x.clip.name.Contains("explosion")).PlayOneShot(sounds.FirstOrDefault(x => x.clip.name.Contains("explosion")).clip);
			EventManager<float>.TriggerEvent ("Shake", 1);
			EnemyList = GameObject.FindGameObjectsWithTag("Enemy");
			foreach( GameObject enemys in EnemyList) {
				enemys.GetComponent<fight>().hp -= 5;
			}
			
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
	IEnumerator UltAnimation(){
		for (int i = 0; i < (speed); i++) {

			actualSpeed = CustumEase(i/speed) * speed * 2 * Time.deltaTime ;
			
			if (goingUp == true){
				spriteRenderer.sprite = GoUp;
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(upPosition.x, upPosition.y, upPosition.z), actualSpeed);
			}
			else if (goingUp == false)
			{
				spriteRenderer.sprite = GoDown;
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(downPosition.x, downPosition.y, downPosition.z),  actualSpeed);
			}
			yield return null;
		}
	}
		public static float CustumEase (float rate) {
			return TweenCore.Easing.QuartIn(TweenCore.FloatTools.Repeat(TweenCore.FloatTools.Lerp(rate, 0, 2f), 0, 1));
	}
}
