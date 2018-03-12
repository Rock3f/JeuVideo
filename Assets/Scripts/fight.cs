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
	public GameObject screenGameOver;
	public GameObject cameraMain;
	public GameObject menuButton;
	public bool isBoss = false;
	public GameObject screenVictory;

	// Private 
	private float accumulateur;
	private string EnemyType;

	public string CollisonSide;

	public float Angle;

	private float topAngle;
    private float sideAngle;

	private AudioSource[] sounds;
	// Use this for initialization
	void Start () {

		// Determine le type de personnage qu'il peut frapper
		if (this.gameObject.tag == "Player"){
			EnemyType = "Enemy";
		}
		else{
			EnemyType = "Player";
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
         sideAngle = 90.0f - topAngle;
	}

	void LateUpdate() {
		// Lorsque la vie arrive à zero fait l'animation de mort
		if(hp <= 0){
			if (EnemyType == "Enemy"){
				GetComponent<movePlayer>().IsDead = true;

			}
			
			if(gameObject.GetComponent<animationSprite>().currentAnim.name != "die")
			{
				gameObject.GetComponent<animationSprite>().ChangeAnimation("die", true);		

				if(cameraMain != null)
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
				if (EnemyType == "Player"){
					DestroyImmediate(this.gameObject);

					if(isBoss)
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

				if (EnemyType == "Enemy"){
					
					Time.timeScale = 0f;
					sounds.FirstOrDefault(x => x.clip.name.Contains("SoundLevel1")).Stop();
					screenGameOver.SetActive(true);
					EventSystem.current.firstSelectedGameObject = menuButton;

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

	private void AddHit(float AddedHit) {
		UltBarVar.GetComponent<UltBar>().hit += AddedHit;
	}
}