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
	public float maxCombo = 5;
	public float combo = 0;
	public Attack[] Attacks;
	// Private 
	private float accumulateur;

	public string EnemyType;

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
        accumulateur += Time.deltaTime;
		foreach (Attack att in Attacks){
			if(col.gameObject.tag == EnemyType && this.GetComponent<SpriteRenderer>().sprite == att.SpriteDamage && accumulateur > 0.2)
			{
				col.gameObject.GetComponent<fight>().hp -= att.dammageValue;
				combo += att.comboValue;
				accumulateur = 0;
			}
		}
    }


	public void AddHp (float amount) {
		hp += amount;
	}

	public void AddCombo (float amount){
		combo += amount;
	}
}