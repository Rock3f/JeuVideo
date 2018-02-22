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

	public Animation[] anims;
    public int imagesParSecondes = 24;

    private SpriteRenderer spriteRenderer;

    private int currentSpriteIdx = 0;
    private Animation currentAnim;
    private float accumulateur = 0;

    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer> ();
    }

    public void SetSpeed (float v) {
        this.spriteRenderer.flipX = v < 0 || this.spriteRenderer.flipX && v == 0;
        if (Mathf.Abs (v) > 0 && currentAnim.name != "run") {
            ChangeAnimation ("run");
        }
        if (Mathf.Abs (v) == 0 && currentAnim.name != "normal") {
            ChangeAnimation ("normal");
        }
    }

    public void SetAction(string input)
    {
        switch(input)
        {
            default: 
            break;
        }

        if(currentAnim.name != input)
        {
            ChangeAnimation(input);
        }   
        else
        {
            NextFrame();
        }     
    }

    public void ChangeAnimation (string anim) {
        this.currentAnim = GetAnim (anim);
        this.currentSpriteIdx = 0;
    }

    // Update is called once per frame
    void Update () {
        // durée souhaitée d'une frame
        float frameDuration = GetFrameDurationInSec ();

        // durée accumulée depuis le dernier changement de frame
        accumulateur += Time.deltaTime;

        // vide l'accumulateur et fait avancer les frames        
        if (accumulateur > frameDuration) {
            NextFrame ();
            accumulateur -= frameDuration;
        }

    }

    private Animation GetAnim (string animName) {
        return anims.FirstOrDefault(x => x.name == animName);
    }

    private float GetFrameDurationInSec () {
        return 1f / imagesParSecondes;
    }

    private void NextFrame () {
        currentSpriteIdx = (currentSpriteIdx + 1) % currentAnim.sprites.Length;
        Debug.Log("currentSpriteIdx : " + currentSpriteIdx);
        for (int i = 0; i < currentAnim.sprites.Length; i++)
        {
            Debug.Log("currentAnim.sprite.Length["+i+"] : " + currentAnim.sprites.Length[i]);
        }
        spriteRenderer.sprite = currentAnim.sprites[currentSpriteIdx];
        Debug.Log("spriteRenderer.sprite : " + spriteRenderer.sprite);
    }
}