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
public class lifebar : MonoBehaviour {
	// Public
	public float hp = 5;
	public float maxHp = 5;
	public float maxCombo = 5;
	public float combo = 0;
	public bool ennemy;
	public Attack[] Attacks;
	// Private 
	private float accumulateur;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCollisionStay2D (Collision2D col)
    {	
        accumulateur += Time.deltaTime;
		foreach (Attack att in Attacks){
			if(col.gameObject.tag == "Enemy" && this.GetComponent<SpriteRenderer>().sprite == att.SpriteDamage && accumulateur > 0.2)
			{
				col.gameObject.GetComponent<lifebar>().hp -= att.dammageValue;
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