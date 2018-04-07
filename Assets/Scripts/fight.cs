using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

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
	public UnityEngine.EventSystems.EventSystem eventSystem;
	public GameObject screenGameOver;
	public GameObject cameraMain;
	public GameObject menuButton;
	public bool isBoss = false;
	public bool isStory = false;
	public GameObject screenVictory;

	public Sprite parade;

	// Private 
	private float accumulateur;
	private string[] enemyType;

	public string CollisonSide;

	public float Angle;

	private float topAngle;
    private float sideAngle;

	private AudioSource[] sounds;
	// Use this for initialization
	void Start () {
		enemyType = new string[3];
		// Determine le type de personnage qu'il peut frapper
		if (this.gameObject.tag == "Player"){
			enemyType[0] = "Enemy";
			enemyType[1] = "Shooter";
		}
		else{
			enemyType[0] = "Player";
		}

		if(cameraMain != null)
		{
			sounds = cameraMain.GetComponents<AudioSource>();
		}		
	}
	
	// Update is called once per frame
	void Update () {
		// Initialise pour detection d'angle
		Vector2 size = GetComponent<BoxCollider2D>().size;
         size = Vector2.Scale (size, (Vector2)transform.localScale);
         topAngle = Mathf.Atan (size.x / size.y) * Mathf.Rad2Deg;
         sideAngle = 100.0f - topAngle;
	}

	void LateUpdate() {
		// Lorsque la vie arrive à zero fait l'animation de mort
		if(hp <= 0){
			if (enemyType[0] == "Enemy"){
				GetComponent<movePlayer>().IsDead = true;
			}
			
			if(gameObject.GetComponent<animationSprite>().currentAnim.name != "die")
			{
				gameObject.GetComponent<animationSprite>().ChangeAnimation("die", true);		

				if(sounds != null)
				{
					sounds.FirstOrDefault(x => x.clip.name.Contains("deathPlayer")).Play();
				}				
			}

			string spritesLastName = GetComponent<animationSprite>().currentAnim.sprites.Last().name;
			string spriteCurrentName =  GetComponent<animationSprite>().currentAnim.sprites[GetComponent<animationSprite>().currentSpriteIdx].name;
			
			// Le dernier sprite de l'animation est vide et fait disparaitre le personnage
			if( spritesLastName == spriteCurrentName){
				gameObject.SetActive(false);
				
				// Si c'est un mechant il est le GameObject est détruit
				if (enemyType[0] == "Player"){
					DestroyImmediate(this.gameObject);

					if(isBoss && !isStory)
					{
						screenVictory.SetActive(true);

						sounds.FirstOrDefault(x => x.clip.name.Contains("win")).PlayOneShot(sounds.FirstOrDefault(x => x.clip.name.Contains("win")).clip);

						foreach(AudioSource sound in sounds)
						{
							if(!sound.clip.name.Contains("win"))
								sound.mute = true;
						}
					}
				}

				if (enemyType[0] == "Enemy" || enemyType[1] == "Shooter"){
					
					if(!isStory)
					{
						//Lecture d'une musique et d'un son
						Time.timeScale = 0f;
						sounds.FirstOrDefault(x => x.clip.name.Contains("SoundLevel1")).Stop();
						screenGameOver.SetActive(true);
						eventSystem.SetSelectedGameObject(menuButton);

						sounds.FirstOrDefault(x => x.clip.name.Contains("gameOver")).PlayOneShot(sounds.FirstOrDefault(x => x.clip.name.Contains("gameOver")).clip);

						foreach(AudioSource sound in sounds)
						{
							if(!sound.clip.name.Contains("gameOver"))
								sound.mute = true;
						}
					}
				}
				
			}	
		}

		if (hp > maxHp){
			hp = maxHp;
		}
		
	}
	public void OnCollisionEnter2D(Collision2D coll) {

		// Detecte de quel cote viens la collison
		Vector2 v = coll.contacts[0].point - (Vector2)transform.position;
		if (Vector2.Angle(v, transform.right) <= sideAngle)  {
			CollisonSide = "R";
			Angle = Vector2.Angle(v, transform.right);
		}
		else if (Vector2.Angle(v, -transform.right) <= sideAngle) {
			CollisonSide = "L";
			Angle = Vector2.Angle(v, transform.right);
		}
	}
    public void OnCollisionStay2D (Collision2D coll)
    {
		// Detecte de quel cote viens la collison
		Vector2 v = coll.contacts[0].point - (Vector2)transform.position;
		
		if (Vector2.Angle(v, transform.right) <= sideAngle)  {
			CollisonSide = "R";
			Angle = Vector2.Angle(v, transform.right);
		}
		else if (Vector2.Angle(v, -transform.right) <= sideAngle) {
			CollisonSide = "L";
			Angle = Vector2.Angle(v, transform.right);
		}
		

		// Permet de n'infliger qu'une seul fois des dégats par coup
        accumulateur += Time.deltaTime;

		foreach (string enemyType in enemyType)
		{
			// Lorsqu'une collison à lieu avec un ennemie
			if(coll.gameObject.tag == enemyType){
			
			string OtherCollSide = coll.gameObject.GetComponent<fight>().CollisonSide;
			bool OtherflipX = GetComponent<SpriteRenderer>().flipX;
			bool EnemyParade = coll.gameObject.GetComponent<fight>().parade == coll.gameObject.GetComponent<SpriteRenderer>().sprite;

			// Pour chaque attack regarde si le sprite correspond au sprite de degat associé
			foreach (Attack att in Attacks){
				if(
				EnemyParade == false
				&& GetComponent<SpriteRenderer>().sprite == att.SpriteDamage 
				&& accumulateur > 0.2
				// Vérifie que les ennemies sont bien face a face pour se frapper
				&& ((CollisonSide == "R" && OtherCollSide == "L" && OtherflipX == false) 
				|| (CollisonSide == "L" && OtherCollSide == "R" && OtherflipX == true))
				)

				{	
					accumulateur = 0;
					// Inflige des dégats en fonction de l'attaque
					coll.gameObject.GetComponent<fight>().hp -= att.dammageValue;
					if(!isStory)
					{
						if(att.name == "coup de poing")
						{
							AudioSource source = sounds.FirstOrDefault(x => x.clip.name.Contains("punch"));
							source.PlayOneShot(source.clip);
						}

						if(att.name == "air kick")
						{
							AudioSource source = sounds.FirstOrDefault(x => x.clip.name.Contains("airKick"));
							source.PlayOneShot(source.clip);
						}

						if(att.name == "high kick")
						{
							AudioSource source = sounds.FirstOrDefault(x => x.clip.name.Contains("HighKick"));
							source.PlayOneShot(source.clip);
						}
					}

						if(att.name == "high kick")
						{
							coll.gameObject.GetComponent<animationSprite>().ChangeAnimation("hit", true);
							coll.gameObject.GetComponent<SpriteRenderer>().color = new Color(255,0,0,255);
							if (sounds != null){
								sounds.FirstOrDefault(x => x.clip.name == "hurt").PlayOneShot(sounds.FirstOrDefault(x => x.clip.name == "hurt").clip);
							}
						}

						// Déclenche l'animation hit
						if (coll.gameObject.GetComponent<fight>().hp > 0){
							if(coll.gameObject.GetComponent<animationSprite>().currentAnim.name != "hit")
							{
								cameraMain.GetComponents<AudioSource>().FirstOrDefault(x => x.clip.name == "hurt").PlayOneShot(cameraMain.GetComponents<AudioSource>().FirstOrDefault(x => x.clip.name == "hurt").clip);
								coll.gameObject.GetComponent<animationSprite>().ChangeAnimation("hit", true);
							}
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
		
    }

	private void AddHit(float AddedHit) {
		UltBarVar.GetComponent<UltBar>().hit += AddedHit;
	}
}