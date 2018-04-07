using System.Collections;
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
	public bool isStory = false;
	public int directionStory;

	private animationSprite ac;
	private string Horizontal;
	private string Vertical;

	private Vector3 initalPosition;
    public Vector3 upPosition;
    public Vector3 downPosition;
	private bool playAnimation = false;
	public float speed = 20.0f;
	public float speedJump = 20.0f;
	public float actualSpeed;
	private SpriteRenderer spriteRenderer;

	private string Fire1;
	private string Fire2;
	private string Fire3;

	private string Parade;

	private bool isAction = false;
	private bool IsUpdatedNow; 
	private int numberofButtonDown = 3;
	private Rigidbody2D rb2d; 

	public float timerParade;

	// Use this for initialization
	void Start () {
		// Récupère une référence au script AnimationCourse attaché au même GameObject
		ac = GetComponent<animationSprite> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		rb2d = GetComponent<Rigidbody2D> ();
		Horizontal = "Horizontal" + Player;
		Vertical = "Vertical" + Player;
		Fire1 = "Fire1" + Player;
		Fire2 = "Fire2" + Player;
		Fire3 = "Fire3" + Player;
		Parade = "Parade" +Player;
		isAction = false;
	}

	// Update is called once per frame
	void Update () {

		if(!IsDead)
		{
			if(!isStory)
			{
				//Changement des animation en fonction des inputs réalisés par l'utilisateur 
				//IMAGE
				IsUpdatedNow = false;
				if(Input.GetButtonDown(Fire1)){
					isAction = true;
					IsUpdatedNow = true;
					if(ac.currentAnim.name != "attack1")
					{
						numberofButtonDown++;
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

					if(ac.currentAnim.name != "attack2" && isAction && numberofButtonDown >= 2){
						numberofButtonDown = 0;
						ac.ChangeAnimation("attack2", isAction);
						gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
						
						initalPosition = transform.position;
						
						if (gameObject.GetComponent<SpriteRenderer>().flipX == false){

							upPosition = new Vector3(initalPosition.x + 2 , initalPosition.y + 1.5f, initalPosition.z );
							downPosition = new Vector3(initalPosition.x + 3 , initalPosition.y, initalPosition.z);
						}
						else{

							upPosition = new Vector3(initalPosition.x - 2 , initalPosition.y + 1.5f, initalPosition.z );
							downPosition = new Vector3(initalPosition.x - 3 , initalPosition.y, initalPosition.z);
						}

						playAnimation = true;
						StartCoroutine(UltAnimation());			
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
						numberofButtonDown++;
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

				if(Input.GetButton(Parade)) {
					
					if (timerParade < 1){
						timerParade += Time.deltaTime;
						Debug.Log("PARADE !!");
						isAction = true;
						IsUpdatedNow = true;

						if(ac.currentAnim.name != "parade")
						{
							ac.ChangeAnimation("parade", isAction);
						}
					}	
				}
				else
				{
					if (timerParade > 0){
						timerParade -= 0.01f;
					} 

					if(!IsUpdatedNow)
					{
						isAction = false;
					}
				}
			
			}
		}
		setMovementForFight();

	}
	private void setMovementForFight() 
	{
		if(!isAction && this.gameObject.GetComponent<fight>().hp > 0) {
			// Calcule une acceleration en fonction de l'entrée utilisateur et de l'accelération configurée pour l'objet
			// Chaque valeur du Vector3 est exprimée en unité par seconde par seconde
			// celà veut dire que chaque seconde, la vitesse augmente de cette valeur configurée.
			// Ex: Chaque seconde, la vitesse augmente de 8 unités par seconde.
			
			Vector3 currentAcceleration = isStory ? 
				new Vector3 (directionStory,0,0) : 
				new Vector3 (
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
			transform.position.y
			);

			// Utilise l'entrée utilisateur pour décider quelle animation afficher.
			// celà permet d'avoir un feedback (retour visuel) immédiat qui lui indique que son
			// action (bouger, ne plus bouger, changer de direction) est prise en compte.
			
			if(isStory)
				ac.SetAnimationFromSpeed (directionStory + 0.001f * currentAcceleration.magnitude, isAction);
			else
				ac.SetAnimationFromSpeed (Input.GetAxis (Horizontal) + 0.001f * currentAcceleration.magnitude, isAction);
		}
	}

	IEnumerator UltAnimation(){

		for (int i = 0; i < speedJump; i++) {
			
			actualSpeed = CustumEase(i/speedJump);
			
			if (i < speedJump/2){ 
				
				transform.position = Vector3.Lerp(transform.position, new Vector3(upPosition.x, upPosition.y, upPosition.z), actualSpeed);
			}
			else
			{
				transform.position = Vector3.Lerp(transform.position, new Vector3(downPosition.x, downPosition.y, downPosition.z), 1-actualSpeed);
			}
			gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
			yield return null;
		}
	}

	public static float CustumEase (float rate) {
        return TweenCore.FloatTools.Zigzag(rate, TweenCore.Easing.SineIn);
	}
	
}