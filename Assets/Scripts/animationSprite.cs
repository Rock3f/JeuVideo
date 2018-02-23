using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public struct Animation {
    public string name;
    public Sprite[] sprites;
}

public class animationSprite : MonoBehaviour {

  // liste des animations configurables
    public Animation[] anims;

    // nombre d'images par seconde à laquelle l'animation doit être jouée
    public int imagesParSecondes = 12;

    // références interne
    private SpriteRenderer spriteRenderer;

    // état privé
    // indice de la frame actuellement affichée
    private int currentSpriteIdx = 0;
    // indice de l'animation actuellement affichée    
    public Animation currentAnim;
    // accumulateur pour mesurer le temps cumulé qui passe
    private float accumulateur = 0;
    private bool isAction = false;
    private bool isRunning = false;

    // Use this for initialization
    void Start () {
        // Récupère une référence au script AnimationCourse attaché au même GameObject
        spriteRenderer = GetComponent<SpriteRenderer> ();
    }

    public void SetAnimationFromSpeed (float vitesse, bool isAction) {
        // Si la vitesse est négative, on va vers la gauche et le sprite devrait être inversé
        // Si la vitesse est positive, on va vers la droite et le sprite ne devrait pas être inversé
        // Si on allait précédemment vers la gauche (flipX est vrai) et que la vitesse tombe à 0, on reste vers la gauche
        this.spriteRenderer.flipX = vitesse < 0 || this.spriteRenderer.flipX && vitesse == 0;
        // Si le personnage se déplace et qu'il n'est pas déjà en train de courir, le faire courir
        if (vitesse != 0 && currentAnim.name != "run") {
            ChangeAnimation ("run", isAction);
        }

        //Si le personnage est arrêté et qu'il n'est pas déjà en animation d'attente, déclencher l'animation d'attente.       
        if (vitesse == 0 && currentAnim.name != "normal" && !isAction && !isRunning) {
            ChangeAnimation ("normal", isAction);
        }
    }

    public void ChangeAnimation (string anim, bool isAction) {
        GetCurrentAnimation(anim);
        // Lors d'une nouvelle animation, repartir à la première image de cette animation
        this.currentSpriteIdx = 0;
        this.isAction = isAction;

        if(isAction)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
      
    }

    // Update is called once per frame
    void Update () {
        // durée souhaitée d'une frame
        float frameDuration = GetFrameDurationInSec ();

        // durée accumulée depuis le dernier changement de frame
        accumulateur += Time.deltaTime;

        // vide l'accumulateur et fait avancer les frames        
        while (accumulateur > frameDuration && frameDuration > 0) {
            if(isAction)
            {
                 NextFrameAction();
            }
            else
            {
                NextFrame ();
            }          
            
            accumulateur -= frameDuration;
        }
    }

    private void GetCurrentAnimation (string animName) {
        currentAnim = anims.FirstOrDefault(x => x.name == animName);
    }

    private float GetFrameDurationInSec () {
        // 1 seconde divisée par le nombre de frame à afficher par secondes nous donne le temps
        // qu'il convient d'allouer à chaque frame (en seconde).
        return 1f / imagesParSecondes;
    }

    private void NextFrame () {
       
        currentSpriteIdx = (currentSpriteIdx + 1) % currentAnim.sprites.Length;
        // Affiche dans le sprite renderer la frame en cours de l'animation en cours.
        spriteRenderer.sprite = currentAnim.sprites[currentSpriteIdx];
           
    }

    private void NextFrameAction() {
        currentSpriteIdx++;
        // Affiche dans le sprite renderer la frame en cours de l'animation en cours.

        if(currentSpriteIdx < currentAnim.sprites.Length)
            spriteRenderer.sprite = currentAnim.sprites[currentSpriteIdx];
       else
       {
            isRunning = false;
            ChangeAnimation("normal", false);
       }
      
    }
}