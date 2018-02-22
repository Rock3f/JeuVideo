using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionPlayer : MonoBehaviour {

	private animationSprite anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<animationSprite> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Space)) {
			anim.SetAction("attack1");
		}
	}
}
