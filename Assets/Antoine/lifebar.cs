using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class lifebar : MonoBehaviour {

	public float hp = 5;
	public float maxHp = 5;
	public Transform background;
	public Transform foreground;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		background.transform.localScale = new Vector3(maxHp, 1,1);
		foreground.transform.localScale = new Vector3(Mathf.Clamp(hp, 0, maxHp),1,1);
	}

	public void AddHp (float amount) {
		hp += amount;
	}
}
