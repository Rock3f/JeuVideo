using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
	// Private 
	private float accumulateur;

	private string EnemyType;

	private float angle;

	// Use this for initialization
	void Start () {
		if (this.gameObject.tag == "Player"){
			EnemyType = "Enemy";
		}
		else{
			EnemyType = "Player";
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (hp <= 0){
			DestroyImmediate(this.gameObject);
		}
	}

    public void OnCollisionStay2D (Collision2D col)
    {

        angle = Vector2.SignedAngle(col.gameObject.transform.position, transform.position);
		
        if (angle < 5.0f){
            print("close");
		}


        accumulateur += Time.deltaTime;
		foreach (Attack att in Attacks){
			if(col.gameObject.tag == EnemyType && this.GetComponent<SpriteRenderer>().sprite == att.SpriteDamage && accumulateur > 0.2)
			{
				col.gameObject.GetComponent<fight>().hp -= att.dammageValue;
				if (UltBarVar != null){
					UltBarVar.GetComponent<UltBar>().hit += att.comboValue;
				}
				accumulateur = 0;
			}
		}
    }
}